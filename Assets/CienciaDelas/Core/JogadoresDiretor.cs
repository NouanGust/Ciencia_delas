using System;
using System.Collections.Generic;
using System.Linq;

namespace CienciaDelas
{
    public sealed class JogadoresDiretor
    {
        public static JogadoresDiretor Instance { get; } = new JogadoresDiretor();
        private JogadoresDiretor() { }
        public JogadorEstado[] Jogadores { get; private set; } = new JogadorEstado[0];
        internal Random Sorteio;
        public void Reiniciar(Cientista primeira, IEnumerable<CartaDados> cartas, int semente)
        {
            Sorteio = new Random(semente);
            Jogadores = new[] { new JogadorEstado { indice=0,cientista=primeira }, new JogadorEstado { indice=1,cientista=primeira==Cientista.MarieCurie?Cientista.JaquelineGoes:Cientista.MarieCurie } };
            foreach (var j in Jogadores)
            {
                j.baralho.AddRange(cartas.Where(c=>c.cientista==Cientista.Normal || c.cientista==j.cientista));
                Embaralhar(j.baralho);
                Comprar(j,PartidaDiretor.Instance.Regras.maoInicial);
            }
        }
        public void Embaralhar<T>(List<T> lista)
        {
            for(int i=lista.Count-1;i>0;i--) { int n=Sorteio.Next(i+1); T a=lista[n]; lista[n]=lista[i]; lista[i]=a; }
        }
        public void Comprar(JogadorEstado j,int quantidade)
        {
            for(int i=0;i<quantidade && j.mao.Count<PartidaDiretor.Instance.Regras.limiteMao;i++)
            {
                if(j.baralho.Count==0) { j.baralho.AddRange(j.descarte); j.descarte.Clear(); Embaralhar(j.baralho); }
                if(j.baralho.Count==0) break;
                j.mao.Add(j.baralho[0]); j.baralho.RemoveAt(0);
            }
        }
        public void NovaRodada(bool inicial)
        {
            var r=PartidaDiretor.Instance.Regras;
            foreach(var j in Jogadores)
            {
                j.confirmado=false; j.conhecimento=r.conhecimento;
                if(!inicial) Comprar(j,r.reporAteQuatro?Math.Max(1,r.maoInicial-j.mao.Count):1);
            }
        }
        public void Descartar(Jogada jogada)
        {
            var j=Jogadores[jogada.jogador];
            if(j.mao.Remove(jogada.carta)) j.descarte.Add(jogada.carta);
        }
    }
}
