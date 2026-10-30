using System;
using System.Collections.Generic;

namespace CienciaDelas
{
    public enum Cientista { Normal, MarieCurie, JaquelineGoes }
    public enum Fase { Menu, Planejamento, Execucao, EscolhaExtra, Ameacas, Vitoria, Derrota }
    public enum Efeito { Recursos, Espiar, Radioatividade, Duplicar, Experimento, Legado, Anular, Diagnostico, Mapear, Converter }
    public enum Comparador { PeloMenos, MaiorQue, Exatamente }
    [Serializable]
    public struct Recursos
    {
        public int saude, infraestrutura, pesquisa;
        public Recursos(int s, int i, int p) { saude = s; infraestrutura = i; pesquisa = p; }
        public static Recursos operator +(Recursos a, Recursos b) => new Recursos(a.saude+b.saude, a.infraestrutura+b.infraestrutura, a.pesquisa+b.pesquisa);
        public static Recursos operator -(Recursos a, Recursos b) => new Recursos(a.saude-b.saude, a.infraestrutura-b.infraestrutura, a.pesquisa-b.pesquisa);
        public static Recursos operator *(Recursos a, int n) => new Recursos(a.saude*n, a.infraestrutura*n, a.pesquisa*n);
        public bool Cobre(Recursos b) => saude >= b.saude && infraestrutura >= b.infraestrutura && pesquisa >= b.pesquisa;
        public bool Zerado => saude <= 0 || infraestrutura <= 0 || pesquisa <= 0;
        public override string ToString() => $"Saúde {saude} | Infra {infraestrutura} | Pesquisa {pesquisa}";
    }
    [Serializable]
    public class CartaDados
    {
        public string id, nome, descricao;
        public Cientista cientista;
        public int conhecimento;
        public Recursos custo, ganho;
        public Efeito efeito;
    }
    [Serializable]
    public class AmeacaDados
    {
        public string id, nome;
        public Recursos requisito, penalidade, recompensa;
        public bool gastar;
        public Comparador comparador;
    }
    [Serializable]
    public class Regras
    {
        public Recursos iniciais = new Recursos(10, 5, 5);
        public int maximoRecurso = 30, maoInicial = 4, limiteMao = 6, conhecimento = 3, limiteSelecao = 3;
        public bool reporAteQuatro = true, embaralharAmeacas = true;
        public int duracaoRadioatividade = 2, perdaRadioatividade = 2, duracaoExperimento = 3, duracaoLegado = 2;
        public int cartasDiagnostico = 2, divisorConversao = 3;
    }
    public sealed class JogadorEstado
    {
        public int indice, conhecimento;
        public Cientista cientista;
        public bool confirmado;
        internal readonly List<CartaDados> baralho = new List<CartaDados>();
        internal readonly List<CartaDados> mao = new List<CartaDados>();
        internal readonly List<CartaDados> descarte = new List<CartaDados>();
        public IReadOnlyList<CartaDados> Baralho => baralho.AsReadOnly();
        public IReadOnlyList<CartaDados> Mao => mao.AsReadOnly();
        public IReadOnlyList<CartaDados> Descarte => descarte.AsReadOnly();
    }
    public sealed class Jogada
    {
        public int jogador;
        public CartaDados carta;
        public AmeacaAtiva alvo;
        public bool gratuita;
    }
    public sealed class AmeacaAtiva
    {
        public AmeacaDados dados;
        public int revelada, bloqueadaAte, convertidaNaRodada;
        public int conversaoMultiplicador = 1;
    }
}
