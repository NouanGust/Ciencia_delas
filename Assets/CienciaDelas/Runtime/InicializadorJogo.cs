using UnityEngine;

namespace CienciaDelas
{
    public sealed class InicializadorJogo : SingletonComponente<InicializadorJogo>
    {
        public ConfiguracaoJogo configuracao;
        private float proximaExecucao;
        // Marca o instante a partir do qual as ameaças podem ser resolvidas.
        private float proximaResolucao;
        private void Start()
        {
            DG.Tweening.DOTween.Init(false,true).SetCapacity(768,192);
            Time.timeScale=1;
            if(configuracao==null) configuracao=Resources.Load<ConfiguracaoJogo>("CienciaDelasConfiguracao");
            if(configuracao==null) configuracao=ScriptableObject.CreateInstance<ConfiguracaoJogo>();
            PartidaDiretor.Instance.Menu();
            var audio = GetComponent<AudioPartidaDiretor>();
            if (audio == null) audio = gameObject.AddComponent<AudioPartidaDiretor>();
            audio.Inicializar(configuracao);
            var interfaceJogo = GetComponent<InterfaceDiretor>();
            if (interfaceJogo == null) interfaceJogo = gameObject.AddComponent<InterfaceDiretor>();
            interfaceJogo.Inicializar(configuracao);
            if (GetComponent<EntradaDiretor>() == null) gameObject.AddComponent<EntradaDiretor>();
        }
        private void Update()
        {
            var p=PartidaDiretor.Instance;
            if(p.Pausada) return;
            // Fase.Ameacas: aguarda intervaloResolucao para dar tempo aos efeitos visuais.
            if(p.Fase==Fase.Ameacas)
            {
                if(Time.unscaledTime<proximaResolucao) return;
                p.FinalizarRodada();
                return;
            }
            if(p.Fase!=Fase.Execucao) { proximaExecucao=Time.unscaledTime+configuracao.intervaloExecucao; return; }
            if(Time.unscaledTime<proximaExecucao) return;
            proximaExecucao=Time.unscaledTime+configuracao.intervaloExecucao;
            p.AvancarExecucao();
            // Se a última carta acabou de ser executada, agenda a pausa de resolução.
            if(p.Fase==Fase.Ameacas) proximaResolucao=Time.unscaledTime+configuracao.intervaloResolucao;
        }
    }
}
