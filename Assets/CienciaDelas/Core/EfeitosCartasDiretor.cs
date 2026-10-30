using System.Collections.Generic;

namespace CienciaDelas
{
    public sealed class EfeitosCartasDiretor
    {
        public static EfeitosCartasDiretor Instance { get; } = new EfeitosCartasDiretor();
        private EfeitosCartasDiretor() { }
        private sealed class Temporario { public int primeiro, ultimo; public bool inicio; public Recursos delta; }
        private readonly List<Temporario> temporarios = new List<Temporario>();
        public int ProximaMultiplicacao { get; private set; } = 1;
        public int ReducaoPenalidade { get; private set; }
        public bool Espiando { get; internal set; }
        public void Reiniciar() { temporarios.Clear(); ProximaMultiplicacao=1; ReducaoPenalidade=0; Espiando=false; }
        public void InicioRodada() { ReducaoPenalidade=0; AplicarTemporarios(true); }
        public void FimRodada() { AplicarTemporarios(false); }
        void AplicarTemporarios(bool inicio)
        {
            int rodada=PartidaDiretor.Instance.Rodada;
            foreach(var t in temporarios.ToArray())
                if(t.inicio==inicio && rodada>=t.primeiro && rodada<=t.ultimo && !PartidaDiretor.Instance.Terminou) RecursosDiretor.Instance.Alterar(t.delta);
            temporarios.RemoveAll(t=>t.ultimo<rodada || (t.inicio==inicio && t.ultimo==rodada));
        }
        void Agendar(Recursos delta,int duracao,bool inicio)
        {
            int primeiro=PartidaDiretor.Instance.Rodada+(inicio?1:0);
            temporarios.Add(new Temporario { primeiro=primeiro,ultimo=primeiro+duracao-1,inicio=inicio,delta=delta });
        }
        public void Executar(Jogada jogada)
        {
            var p=PartidaDiretor.Instance;
            int m=ProximaMultiplicacao; ProximaMultiplicacao=1;
            var c=jogada.carta;
            var alvo=jogada.alvo;
            if(alvo!=null && !AmeacasDiretor.Instance.Contem(alvo)) alvo=null;
            switch(c.efeito)
            {
                case Efeito.Recursos: RecursosDiretor.Instance.Alterar(c.ganho*m); break;
                case Efeito.Espiar: Espiando=true; break;
                case Efeito.Duplicar: ProximaMultiplicacao=2*m; break;
                case Efeito.Radioatividade:
                    if(alvo!=null) alvo.bloqueadaAte=System.Math.Max(alvo.bloqueadaAte,p.Rodada+p.Regras.duracaoRadioatividade*m-1);
                    Agendar(new Recursos(-p.Regras.perdaRadioatividade*m,0,0),p.Regras.duracaoRadioatividade,false); break;
                case Efeito.Experimento: Agendar(new Recursos(m,m,m),p.Regras.duracaoExperimento,false); break;
                case Efeito.Legado: Agendar(new Recursos(0,0,m),p.Regras.duracaoLegado,true); break;
                case Efeito.Anular:
                    var nova=AmeacasDiretor.Instance.UltimaRevelada;
                    if(nova!=null && nova.revelada==p.Rodada) nova.bloqueadaAte=System.Math.Max(nova.bloqueadaAte,p.Rodada+m-1);
                    break;
                case Efeito.Diagnostico: p.AbrirExtras(jogada.jogador,p.Regras.cartasDiagnostico*m); break;
                case Efeito.Mapear: ReducaoPenalidade+=m; break;
                case Efeito.Converter:
                    if(alvo!=null) { alvo.convertidaNaRodada=p.Rodada; alvo.conversaoMultiplicador=m; } break;
            }
        }
        public Recursos Penalidade(AmeacaAtiva ameaca)
        {
            var p=PartidaDiretor.Instance;
            if(ameaca.bloqueadaAte>=p.Rodada) return default;
            var d=ameaca.dados.penalidade;
            // Conversão opera sobre a perda original; não sofre redução do Mapeamento.
            if(ameaca.convertidaNaRodada==p.Rodada)
                return new Recursos(d.saude/p.Regras.divisorConversao,d.infraestrutura/p.Regras.divisorConversao,d.pesquisa/p.Regras.divisorConversao)*ameaca.conversaoMultiplicador;
            return new Recursos(-System.Math.Max(0,d.saude-ReducaoPenalidade),-System.Math.Max(0,d.infraestrutura-ReducaoPenalidade),-System.Math.Max(0,d.pesquisa-ReducaoPenalidade));
        }
    }
}
