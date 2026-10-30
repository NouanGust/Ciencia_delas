using System;

namespace CienciaDelas
{
    public sealed class RecursosDiretor
    {
        public static RecursosDiretor Instance { get; } = new RecursosDiretor();
        private RecursosDiretor() { }
        public event Action<Recursos> Alterados;
        public Recursos Atual { get; private set; }
        public Recursos Reservado { get; private set; }
        public Recursos Disponivel => Atual - Reservado;
        public void Reiniciar() { Atual = PartidaDiretor.Instance.Regras.iniciais; Reservado = default; }
        public bool Reservar(Recursos custo)
        {
            if (!Disponivel.Cobre(custo)) return false;
            Reservado += custo; return true;
        }
        public void Liberar(Recursos custo) { Reservado -= custo; }
        public void LimparReservas() { Reservado = default; }
        public void Alterar(Recursos delta)
        {
            if (PartidaDiretor.Instance.Terminou) return;
            int max = PartidaDiretor.Instance.Regras.maximoRecurso;
            var anterior=Atual;
            Recursos v = Atual + delta;
            Atual = new Recursos(Math.Max(0,Math.Min(max,v.saude)),Math.Max(0,Math.Min(max,v.infraestrutura)),Math.Max(0,Math.Min(max,v.pesquisa)));
            Alterados?.Invoke(Atual-anterior);
            if (Atual.Zerado) PartidaDiretor.Instance.Encerrar(false);
        }
    }
}
