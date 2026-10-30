using System.Collections.Generic;

namespace CienciaDelas
{
    public sealed class AmeacasDiretor
    {
        public static AmeacasDiretor Instance { get; } = new AmeacasDiretor();
        private AmeacasDiretor() { }
        private readonly List<AmeacaDados> baralho = new List<AmeacaDados>();
        private readonly List<AmeacaAtiva> ativas = new List<AmeacaAtiva>();
        private readonly List<AmeacaDados> cemiterio = new List<AmeacaDados>();
        public IReadOnlyList<AmeacaAtiva> Ativas => ativas.AsReadOnly();
        public IReadOnlyList<AmeacaDados> Cemiterio => cemiterio.AsReadOnly();
        public int Restantes => baralho.Count;
        public AmeacaDados Proxima => baralho.Count>0?baralho[0]:null;
        public AmeacaAtiva UltimaRevelada { get; private set; }
        public bool Contem(AmeacaAtiva a) => ativas.Contains(a);
        public void Reiniciar(IEnumerable<AmeacaDados> dados)
        {
            baralho.Clear(); ativas.Clear(); cemiterio.Clear(); UltimaRevelada=null;
            baralho.AddRange(dados);
            if(PartidaDiretor.Instance.Regras.embaralharAmeacas) JogadoresDiretor.Instance.Embaralhar(baralho);
        }
        public void Revelar()
        {
            EfeitosCartasDiretor.Instance.Espiando=false;
            UltimaRevelada=null;
            if(baralho.Count==0) return;
            UltimaRevelada=new AmeacaAtiva { dados=baralho[0],revelada=PartidaDiretor.Instance.Rodada };
            ativas.Add(UltimaRevelada); baralho.RemoveAt(0);
        }
        static bool Compara(int v,int r,Comparador c) => r==0 || (c==Comparador.Exatamente?v==r:c==Comparador.MaiorQue?v>r:v>=r);
        public bool PodeSuperar(AmeacaDados a)
        {
            var v=RecursosDiretor.Instance.Atual; var r=a.requisito;
            if(a.gastar) return v.Cobre(r);
            return Compara(v.saude,r.saude,a.comparador) && Compara(v.infraestrutura,r.infraestrutura,a.comparador) && Compara(v.pesquisa,r.pesquisa,a.comparador);
        }
        public void Resolver()
        {
            var p=PartidaDiretor.Instance;
            // Reinicia pela mais antiga após cada sucesso; cada iteração remove uma carta.
            bool mudou=true;
            while(mudou && !p.Terminou)
            {
                mudou=false;
                foreach(var a in ativas.ToArray())
                {
                    if(!PodeSuperar(a.dados)) continue;
                    if(a.dados.gastar) RecursosDiretor.Instance.Alterar(a.dados.requisito*-1);
                    if(p.Terminou) return; // Não permite que recompensa reverta derrota por custo.
                    ativas.Remove(a); cemiterio.Add(a.dados);
                    RecursosDiretor.Instance.Alterar(a.dados.recompensa);
                    p.NotificarAmeaca(a,true);
                    p.Registrar("Ameaça superada: "+a.dados.nome);
                    mudou=true; break;
                }
            }
            foreach(var a in ativas.ToArray())
            {
                if(p.Terminou) return;
                var penalidade=EfeitosCartasDiretor.Instance.Penalidade(a);
                RecursosDiretor.Instance.Alterar(penalidade);
                if(penalidade.saude<0 || penalidade.infraestrutura<0 || penalidade.pesquisa<0) p.NotificarAmeaca(a,false);
                p.Registrar("Ameaça persistente: "+a.dados.nome);
            }
            if(!p.Terminou && baralho.Count==0 && ativas.Count==0) p.Encerrar(true);
        }
    }
}
