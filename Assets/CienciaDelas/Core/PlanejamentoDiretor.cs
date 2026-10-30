using System.Collections.Generic;
using System.Linq;

namespace CienciaDelas
{
    public sealed class PlanejamentoDiretor
    {
        public static PlanejamentoDiretor Instance { get; } = new PlanejamentoDiretor();
        private PlanejamentoDiretor() { }
        private readonly List<Jogada> fila = new List<Jogada>();
        public IReadOnlyList<Jogada> Fila => fila.AsReadOnly();
        public int Reservado(int jogador) => fila.Where(x=>x.jogador==jogador && !x.gratuita).Sum(x=>x.carta.conhecimento);
        public int Posicao(CartaDados c,int jogador) => fila.FindIndex(x=>x.carta==c && x.jogador==jogador)+1;
        public void Reiniciar() { fila.Clear(); RecursosDiretor.Instance.LimparReservas(); }
        public bool Alternar(int jogador,CartaDados carta,AmeacaAtiva alvo=null)
        {
            var p=PartidaDiretor.Instance;
            if(p.Pausada || p.Fase!=Fase.Planejamento || jogador<0 || jogador>1) return false;
            var j=JogadoresDiretor.Instance.Jogadores[jogador];
            if(j.confirmado || !j.Mao.Contains(carta)) return false;
            var anterior=fila.Find(x=>x.jogador==jogador && x.carta==carta);
            if(anterior!=null) { RecursosDiretor.Instance.Liberar(carta.custo); fila.Remove(anterior); p.Notificar("Seleção removida."); return true; }
            if(fila.Count(x=>x.jogador==jogador)>=p.Regras.limiteSelecao) return p.Recusar("Limite de três cartas por jogador.");
            if(j.conhecimento-Reservado(jogador)<carta.conhecimento) return p.Recusar("Conhecimento insuficiente.");
            if(!RecursosDiretor.Instance.Reservar(carta.custo)) return p.Recusar("Recursos compartilhados já reservados ou insuficientes.");
            fila.Add(new Jogada { jogador=jogador,carta=carta,alvo=alvo??AmeacasDiretor.Instance.Ativas.FirstOrDefault() });
            p.Notificar("Carta selecionada: "+carta.nome); return true;
        }
        public void Confirmar(int jogador)
        {
            var p=PartidaDiretor.Instance;
            if(p.Pausada || p.Fase!=Fase.Planejamento || jogador<0 || jogador>1) return;
            var j=JogadoresDiretor.Instance.Jogadores[jogador];
            if(j.confirmado) return;
            j.confirmado=true;
            p.Notificar("Jogador "+(jogador+1)+" confirmou.");
            if(JogadoresDiretor.Instance.Jogadores.All(x=>x.confirmado)) p.IniciarExecucao();
        }
        internal Jogada Retirar()
        {
            if(fila.Count==0) return null;
            var x=fila[0]; fila.RemoveAt(0); RecursosDiretor.Instance.Liberar(x.carta.custo); return x;
        }
        internal bool AdicionarExtra(int jogador,CartaDados carta,AmeacaAtiva alvo)
        {
            if(!JogadoresDiretor.Instance.Jogadores[jogador].Mao.Contains(carta) || fila.Any(x=>x.jogador==jogador && x.carta==carta)) return false;
            if(!RecursosDiretor.Instance.Reservar(carta.custo)) return PartidaDiretor.Instance.Recusar("Recursos insuficientes para a carta extra.");
            // As escolhas extras entram no fim, preservando a ordem global já confirmada.
            fila.Add(new Jogada { jogador=jogador,carta=carta,alvo=alvo,gratuita=true }); return true;
        }
    }
}
