using System;
using System.Collections.Generic;
using System.Linq;

namespace CienciaDelas
{
    public sealed class PartidaDiretor
    {
        public static PartidaDiretor Instance { get; } = new PartidaDiretor();
        private PartidaDiretor() { }
        public event Action Alterada;
        public event Action<string> Evento;
        public event Action<Jogada> CartaExecutada;
        public event Action<AmeacaAtiva,bool> AmeacaResolvida;
        public void NotificarAmeaca(AmeacaAtiva ameaca,bool superada) => AmeacaResolvida?.Invoke(ameaca,superada);
        public Regras Regras { get; private set; } = new Regras();
        public Fase Fase { get; private set; } = Fase.Menu;
        public int Rodada { get; private set; }
        public bool Pausada { get; private set; }
        public bool Terminou => Fase==Fase.Derrota || Fase==Fase.Vitoria;
        public int JogadorExtra { get; private set; }
        public int ExtrasRestantes { get; private set; }
        public string Mensagem { get; private set; } = "Escolha as cientistas e inicie a partida.";
        private readonly List<string> historico=new List<string>();
        public IReadOnlyList<string> Historico => historico.AsReadOnly();
        public void Iniciar(Cientista primeira,Regras regras,IEnumerable<CartaDados> cartas,IEnumerable<AmeacaDados> ameacas,int semente)
        {
            Validar(regras,cartas,ameacas);
            Regras=regras; Rodada=0; Pausada=false; Fase=Fase.Planejamento; ExtrasRestantes=0; historico.Clear();
            PlanejamentoDiretor.Instance.Reiniciar(); EfeitosCartasDiretor.Instance.Reiniciar(); RecursosDiretor.Instance.Reiniciar();
            JogadoresDiretor.Instance.Reiniciar(primeira,cartas,semente); AmeacasDiretor.Instance.Reiniciar(ameacas);
            NovaRodada(); Evento?.Invoke("inicio");
        }
        public static void Validar(Regras r,IEnumerable<CartaDados> cartas,IEnumerable<AmeacaDados> ameacas)
        {
            if(r==null || r.iniciais.Zerado || r.maximoRecurso<1 || !new Recursos(r.maximoRecurso,r.maximoRecurso,r.maximoRecurso).Cobre(r.iniciais) || r.maoInicial<1 || r.limiteMao<r.maoInicial || r.conhecimento<1 || r.limiteSelecao<1 || r.divisorConversao<1 || r.duracaoRadioatividade<1 || r.duracaoExperimento<1 || r.duracaoLegado<1 || r.cartasDiagnostico<0 || r.perdaRadioatividade<0) throw new ArgumentException("Configuração de regras inválida.");
            var c=cartas.ToList(); var a=ameacas.ToList();
            if(c.Count!=20 || c.Count(x=>x.cientista==Cientista.Normal)!=10 || c.Count(x=>x.cientista==Cientista.MarieCurie)!=5 || c.Count(x=>x.cientista==Cientista.JaquelineGoes)!=5 || a.Count!=15) throw new ArgumentException("O catálogo deve conter 10 cartas normais, 5 de cada cientista e 15 ameaças.");
            if(c.Any(x=>string.IsNullOrWhiteSpace(x.id) || x.conhecimento<0 || !x.custo.Cobre(default) || !x.ganho.Cobre(default)) || c.Select(x=>x.id).Distinct().Count()!=20 || a.Any(x=>string.IsNullOrWhiteSpace(x.id) || !x.requisito.Cobre(default) || !x.penalidade.Cobre(default) || !x.recompensa.Cobre(default)) || a.Select(x=>x.id).Distinct().Count()!=15) throw new ArgumentException("IDs duplicados/vazios ou valores negativos no catálogo.");
        }
        void NovaRodada()
        {
            Rodada++; PlanejamentoDiretor.Instance.Reiniciar();
            JogadoresDiretor.Instance.NovaRodada(Rodada==1);
            EfeitosCartasDiretor.Instance.InicioRodada();
            if(Terminou) return;
            AmeacasDiretor.Instance.Revelar(); Fase=Fase.Planejamento;
            Notificar("Rodada "+Rodada+": planejem as jogadas."); Evento?.Invoke("compra");
        }
        public void IniciarExecucao() { Fase=Fase.Execucao; Notificar("As duas jogadas foram confirmadas."); Evento?.Invoke("confirmar"); }
        // Uma chamada resolve uma carta. Quando a fila esvazia, entra em Fase.Ameacas e para —
        // InicializadorJogo aguarda intervaloResolucao antes de chamar FinalizarRodada().
        // Testes que não dependem da UI devem chamar FinalizarRodada() após esta.
        public void AvancarExecucao()
        {
            if(Pausada || Fase!=Fase.Execucao) return;
            var jogada=PlanejamentoDiretor.Instance.Retirar();
            if(jogada==null)
            {
                Fase=Fase.Ameacas;
                EfeitosCartasDiretor.Instance.FimRodada();
                // Não chama Resolver/NovaRodada aqui: InicializadorJogo aguarda intervaloResolucao.
                Notificar("Verificando ameaças...");
                return;
            }
            var j=JogadoresDiretor.Instance.Jogadores[jogada.jogador];
            if(!RecursosDiretor.Instance.Atual.Cobre(jogada.carta.custo)) { Notificar("Carta cancelada: recursos insuficientes após efeitos anteriores."); return; }
            if(!jogada.gratuita) j.conhecimento-=jogada.carta.conhecimento;
            RecursosDiretor.Instance.Alterar(jogada.carta.custo*-1);
            JogadoresDiretor.Instance.Descartar(jogada);
            if(Terminou) return;
            EfeitosCartasDiretor.Instance.Executar(jogada);
            CartaExecutada?.Invoke(jogada);
            Registrar("J"+(jogada.jogador+1)+": "+jogada.carta.nome);
            if(!Terminou) Notificar("Executada: "+jogada.carta.nome);
            Evento?.Invoke(jogada.carta.cientista==Cientista.Normal?"carta":"especial");
        }
        // Chamado pelo InicializadorJogo após intervaloResolucao, e diretamente pelos testes.
        public void FinalizarRodada()
        {
            if(Fase!=Fase.Ameacas) return;
            if(!Terminou) AmeacasDiretor.Instance.Resolver();
            if(!Terminou) NovaRodada();
        }
        public void AbrirExtras(int jogador,int quantidade)
        {
            JogadorExtra=jogador; ExtrasRestantes=quantidade;
            Fase=quantidade>0?Fase.EscolhaExtra:Fase.Execucao;
        }
        public bool EscolherExtra(CartaDados carta,AmeacaAtiva alvo=null)
        {
            if(Pausada || Fase!=Fase.EscolhaExtra || ExtrasRestantes<=0) return false;
            if(!PlanejamentoDiretor.Instance.AdicionarExtra(JogadorExtra,carta,alvo??AmeacasDiretor.Instance.Ativas.FirstOrDefault())) return false;
            ExtrasRestantes--; if(ExtrasRestantes==0) Fase=Fase.Execucao;
            Notificar("Carta extra adicionada: "+carta.nome); return true;
        }
        public void FinalizarExtras() { if(!Pausada && Fase==Fase.EscolhaExtra) { ExtrasRestantes=0; Fase=Fase.Execucao; Notificar("Escolha de cartas extras concluída."); } }
        public void Pausar() { if(Fase==Fase.Menu || Terminou) return; Pausada=!Pausada; Notificar(Pausada?"Partida pausada.":"Partida retomada."); }
        public void Menu() { Pausada=false; Fase=Fase.Menu; PlanejamentoDiretor.Instance.Reiniciar(); EfeitosCartasDiretor.Instance.Reiniciar(); Notificar("Escolha as cientistas e inicie a partida."); Evento?.Invoke("menu"); }
        public void Encerrar(bool venceu) { if(Terminou) return; Fase=venceu?Fase.Vitoria:Fase.Derrota; Pausada=false; PlanejamentoDiretor.Instance.Reiniciar(); Notificar(venceu?"Vitória! Todas as ameaças foram superadas.":"Derrota: um recurso chegou a zero."); Evento?.Invoke(venceu?"vitoria":"derrota"); }
        public void Registrar(string texto) { historico.Add("R"+Rodada+" — "+texto); if(historico.Count>100) historico.RemoveAt(0); }
        public void Notificar(string texto) { Mensagem=texto; Alterada?.Invoke(); }
        public bool Recusar(string motivo) { Notificar(motivo); Evento?.Invoke("erro"); return false; }
    }
}
