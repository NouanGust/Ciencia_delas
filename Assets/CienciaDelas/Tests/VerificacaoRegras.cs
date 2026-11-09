#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace CienciaDelas
{
    // Testes determinísticos sem dependência da interface, executados pelo menu Ciência Delas.
    public static class VerificacaoRegras
    {
        static readonly List<string> aprovados=new List<string>();
        static PartidaDiretor P => PartidaDiretor.Instance;
        static RecursosDiretor R => RecursosDiretor.Instance;
        static PlanejamentoDiretor Plan => PlanejamentoDiretor.Instance;
        static JogadorEstado J(int i) => JogadoresDiretor.Instance.Jogadores[i];
        static List<CartaDados> cartas;
        static List<AmeacaDados> ameacas;
        static void Exigir(bool ok,string erro) { if(!ok) throw new Exception(erro); }
        static void Novo(bool facil=false)
        {
            cartas=CatalogoPadrao.Cartas(); ameacas=CatalogoPadrao.Ameacas();
            if(facil) foreach(var a in ameacas) { a.requisito=default; a.penalidade=default; a.gastar=false; }
            P.Iniciar(Cientista.MarieCurie,new Regras { embaralharAmeacas=false },cartas,ameacas,123);
        }
        static CartaDados Mao(int jogador,string id)
        {
            var c=cartas.Single(x=>x.id==id); var j=J(jogador);
            j.baralho.Remove(c); j.descarte.Remove(c); if(!j.mao.Contains(c)) j.mao.Add(c); return c;
        }
        static void Executar(string id,int jogador=0,AmeacaAtiva alvo=null) => EfeitosCartasDiretor.Instance.Executar(new Jogada { carta=cartas.Single(x=>x.id==id),jogador=jogador,alvo=alvo??AmeacasDiretor.Instance.Ativas.FirstOrDefault() });
        static void Rodada()
        {
            Plan.Confirmar(0); Plan.Confirmar(1); int guarda=100;
            // AvancarExecucao joga uma carta por vez; quando a fila esvazia, entra em Fase.Ameacas.
            // FinalizarRodada resolve as ameaças e inicia a próxima rodada (equivale ao intervalo de tempo da UI).
            while((P.Fase==Fase.Execucao || P.Fase==Fase.EscolhaExtra || P.Fase==Fase.Ameacas) && guarda-->0)
            {
                if(P.Fase==Fase.EscolhaExtra) P.FinalizarExtras();
                else if(P.Fase==Fase.Ameacas) P.FinalizarRodada();
                else P.AvancarExecucao();
            }
            Exigir(guarda>0,"Execução não terminou.");
        }
        static void Caso(string nome,Action teste) { teste(); aprovados.Add(nome); }
        public static string ExecutarTudo()
        {
            aprovados.Clear();
            Caso("Estado inicial, dois baralhos, cientistas e mãos independentes",()=> {
                Novo(); Exigir(R.Atual.Equals(new Recursos(10,5,5)),"Recursos iniciais.");
                foreach(var j in JogadoresDiretor.Instance.Jogadores) { Exigir(j.Mao.Count==4 && j.Baralho.Count==11 && j.conhecimento==3,"Compra inicial."); Exigir(j.Mao.Concat(j.Baralho).Count(c=>c.cientista!=Cientista.Normal)==5,"Cinco especiais."); }
                Exigir(J(0).cientista!=J(1).cientista,"Cientistas distintas.");
            });
            Caso("Reserva cruzada, liberação, limite de três e renumeração",()=> {
                Novo(); var a=Mao(0,"analise"); var b=Mao(1,"insumos"); a.custo=new Recursos(0,4,0); b.custo=new Recursos(0,2,0);
                Exigir(Plan.Alternar(0,a),"Reserva 1."); Exigir(R.Atual.infraestrutura==5 && R.Disponivel.infraestrutura==1,"Reserva não consome.");
                Exigir(!Plan.Alternar(1,b),"Reserva duplicada."); Plan.Alternar(0,a); Exigir(Plan.Alternar(1,b),"Liberação.");
                Plan.Alternar(0,Mao(0,"monitoramento")); Plan.Alternar(0,Mao(0,"controlado")); Plan.Alternar(0,Mao(0,"insumos"));
                Exigir(!Plan.Alternar(0,a),"Limite zero custo."); Plan.Alternar(1,b); Exigir(Plan.Fila.Count==3 && Plan.Posicao(Plan.Fila[0].carta,0)==1,"Renumeração.");
            });
            Caso("Dupla confirmação, bloqueio, custo efetivo e descarte",()=> {
                Novo(true); var c=Mao(0,"rapido");
                if(J(0).mao.Count>4) { var outra=J(0).mao.First(x=>x!=c); J(0).mao.Remove(outra); J(0).baralho.Add(outra); }
                c.custo=new Recursos(1,0,0); Plan.Alternar(0,c); Plan.Confirmar(0);
                Exigir(P.Fase==Fase.Planejamento && !Plan.Alternar(0,c),"Bloqueio individual.");
                Plan.Confirmar(1); P.AvancarExecucao(); Exigir(R.Atual.saude==9 && R.Atual.pesquisa==7 && J(0).conhecimento==2,"Custo/efeito.");
                Exigir(J(0).Descarte.Contains(c) && !J(0).Mao.Contains(c),"Descarte."); P.AvancarExecucao();
                Exigir(P.Fase==Fase.Ameacas,"A fila vazia deve aguardar a resolução das ameaças.");
                P.FinalizarRodada();
                Exigir(P.Rodada==2 && J(0).Mao.Count==4 && J(1).Mao.Count==5 && J(0).conhecimento==3,"Reposição.");
            });
            Caso("Compra limitada e reembaralhamento preservam as 15 cartas",()=> {
                Novo(); var j=J(0); j.descarte.AddRange(j.baralho); j.baralho.Clear(); JogadoresDiretor.Instance.Comprar(j,50);
                Exigir(j.Mao.Count==6 && j.Baralho.Count==9 && j.Descarte.Count==0,"Limite/reembaralhamento.");
                Exigir(j.Mao.Concat(j.Baralho).Distinct().Count()==15,"Cartas duplicadas.");
            });
            Caso("Dez cartas normais e ganhos diretos das cientistas",()=> {
                foreach(var id in new[]{"analise","rapido","resposta","insumos","amostras","controlado","campo","intervencao","vacinas","energia","imediata"})
                { Novo(); var antes=R.Atual; var c=cartas.Single(x=>x.id==id); Executar(id); Exigir(R.Atual.Equals(antes+c.ganho),id); }
                Novo(); Executar("monitoramento"); Exigir(EfeitosCartasDiretor.Instance.Espiando && AmeacasDiretor.Instance.Proxima!=null,"Espiar.");
                Rodada(); Exigir(!EfeitosCartasDiretor.Instance.Espiando,"Expiração da previsão.");
            });
            Caso("Duplicação global não duplica custos",()=> {
                Novo(true); Executar("descoberta"); var c=Mao(1,"rapido"); Plan.Alternar(1,c); Plan.Confirmar(0); Plan.Confirmar(1); P.AvancarExecucao();
                Exigir(R.Atual.pesquisa==9 && J(1).conhecimento==2 && EfeitosCartasDiretor.Instance.ProximaMultiplicacao==1,"Duplicação.");
            });
            Caso("Radioatividade, alvo e expiração de duas rodadas",()=> {
                Novo(); foreach(var a in ameacas) a.requisito=new Recursos(30,30,30);
                var alvo=AmeacasDiretor.Instance.Ativas[0]; Executar("radioatividade",0,alvo);
                Exigir(alvo.bloqueadaAte==2,"Duração bloqueio."); Rodada(); Exigir(R.Atual.saude==8,"Radioatividade rodada 1.");
                Rodada(); Exigir(R.Atual.saude==6,"Radioatividade rodada 2."); Rodada(); Exigir(R.Atual.saude==3,"Fim radioatividade e retorno penalidade.");
            });
            Caso("Experimento três finais e Legado dois inícios",()=> {
                Novo(true); Executar("avancado"); Executar("legado"); Rodada(); Exigir(R.Atual.Equals(new Recursos(11,6,7)),"Temporários 1.");
                Rodada(); Exigir(R.Atual.Equals(new Recursos(12,7,9)),"Temporários 2."); Rodada(); Exigir(R.Atual.Equals(new Recursos(13,8,10)),"Temporários 3.");
                Rodada(); Exigir(R.Atual.Equals(new Recursos(13,8,10)),"Temporários expiraram.");
            });
            Caso("Anulação, redução e conversão de penalidades",()=> {
                Novo(); var a=AmeacasDiretor.Instance.Ativas[0]; Executar("genetico",1); Exigir(EfeitosCartasDiretor.Instance.Penalidade(a).Equals(default(Recursos)),"Anulação.");
                Novo(); a=AmeacasDiretor.Instance.Ativas[0]; a.dados.penalidade=new Recursos(3,1,2); Executar("mapeamento",1);
                Exigir(EfeitosCartasDiretor.Instance.Penalidade(a).Equals(new Recursos(-2,0,-1)),"Redução.");
                Executar("controle",1,a); Exigir(EfeitosCartasDiretor.Instance.Penalidade(a).Equals(new Recursos(1,0,0)),"Conversão.");
            });
            Caso("Diagnóstico escolhe extras da própria mão, sem custo de conhecimento",()=> {
                Novo(true); var d=Mao(1,"diagnostico"); var c=Mao(1,"rapido"); c.custo=new Recursos(0,1,0);
                Plan.Alternar(1,d); Plan.Confirmar(0); Plan.Confirmar(1); P.AvancarExecucao(); Exigir(P.Fase==Fase.EscolhaExtra && J(1).conhecimento==0,"Diagnóstico.");
                Exigir(P.EscolherExtra(c),"Carta extra."); Exigir(!P.EscolherExtra(c),"Extra duplicada.");
                P.FinalizarExtras(); P.AvancarExecucao(); Exigir(R.Atual.pesquisa==7 && R.Atual.infraestrutura==4 && J(1).conhecimento==0,"Custo extra.");
            });
            Caso("Possuir não gasta; gastar consome; recompensa reavalia ameaça antiga",()=> {
                Novo(); ameacas[0].requisito=new Recursos(0,0,8); ameacas[0].penalidade=default;
                ameacas[1].requisito=new Recursos(0,0,2); ameacas[1].gastar=true; ameacas[1].recompensa=new Recursos(0,0,5);
                Rodada(); Rodada(); Exigir(AmeacasDiretor.Instance.Cemiterio.Count==2 && R.Atual.pesquisa==8,"Reavaliação.");
            });
            Caso("Igualdade, comparação estrita e teto dos recursos",()=> {
                Novo(); var a=ameacas[0]; a.requisito=new Recursos(0,0,5); a.comparador=Comparador.Exatamente;
                Exigir(AmeacasDiretor.Instance.PodeSuperar(a),"Exatamente."); a.comparador=Comparador.MaiorQue; Exigir(!AmeacasDiretor.Instance.PodeSuperar(a),"Maior que.");
                R.Alterar(new Recursos(100,100,100)); Exigir(R.Atual.Equals(new Recursos(30,30,30)),"Teto.");
            });
            Caso("Derrota imediata em cada recurso, antes de recompensa ou efeito",()=> {
                foreach(var delta in new[]{new Recursos(-10,0,0),new Recursos(0,-5,0),new Recursos(0,0,-5)}) { Novo(); R.Alterar(delta); Exigir(P.Fase==Fase.Derrota,"Derrota."); R.Alterar(new Recursos(30,30,30)); Exigir(R.Atual.Zerado,"Derrota terminal."); }
                Novo(); var c=Mao(0,"analise"); c.custo=new Recursos(0,0,5); Plan.Alternar(0,c); Rodada(); Exigir(P.Fase==Fase.Derrota && R.Atual.pesquisa==0,"Efeito não ressuscita.");
                Novo(); ameacas[0].gastar=true; ameacas[0].requisito=new Recursos(0,0,5); ameacas[0].recompensa=new Recursos(0,0,10); Rodada(); Exigir(P.Fase==Fase.Derrota,"Recompensa não ressuscita.");
            });
            Caso("Pausa bloqueia planejamento e execução; reinício limpa estado",()=> {
                Novo(); var c=Mao(0,"analise"); P.Pausar(); Exigir(!Plan.Alternar(0,c),"Pausa seleção."); Plan.Confirmar(0); Exigir(!J(0).confirmado,"Pausa confirmação.");
                P.Pausar(); Plan.Alternar(0,c); Plan.Confirmar(0); Plan.Confirmar(1); P.Pausar(); P.AvancarExecucao(); Exigir(Plan.Fila.Count==1,"Pausa execução.");
                Novo(); Exigir(!P.Pausada && Plan.Fila.Count==0 && P.Rodada==1 && AmeacasDiretor.Instance.Cemiterio.Count==0,"Reinício.");
            });
            Caso("Partida completa até vitória e até derrota",()=> {
                Novo(true); for(int i=0;i<15;i++) Rodada(); Exigir(P.Fase==Fase.Vitoria && AmeacasDiretor.Instance.Cemiterio.Count==15 && AmeacasDiretor.Instance.Restantes==0,"Vitória.");
                Novo(); foreach(var a in ameacas) { a.requisito=new Recursos(30,30,30); a.penalidade=new Recursos(2,0,0); }
                for(int i=0;i<10 && !P.Terminou;i++) Rodada(); Exigir(P.Fase==Fase.Derrota,"Derrota em partida.");
            });
            Caso("100 partidas simuladas: conservação de cartas, reservas e ausência de travamento",()=> {
                for(int seed=0;seed<100;seed++)
                {
                    cartas=CatalogoPadrao.Cartas(); ameacas=CatalogoPadrao.Ameacas(); P.Iniciar(Cientista.MarieCurie,new Regras(),cartas,ameacas,seed);
                    for(int rodada=0;rodada<40 && !P.Terminou;rodada++)
                    {
                        for(int j=0;j<2;j++) foreach(var c in J(j).Mao.ToArray()) Plan.Alternar(j,c);
                        Rodada();
                        foreach(var j in JogadoresDiretor.Instance.Jogadores) Exigir(j.Mao.Count<=6 && j.Mao.Count+j.Baralho.Count+j.Descarte.Count==15,"Conservação semente "+seed);
                        Exigir(R.Reservado.Equals(default(Recursos)),"Reserva residual.");
                    }
                }
            });
            P.Menu();
            return string.Join("\n",aprovados.Select(x=>"PASS: "+x))+"\nTOTAL: "+aprovados.Count+" cenários aprovados.";
        }
    }
}
#endif
