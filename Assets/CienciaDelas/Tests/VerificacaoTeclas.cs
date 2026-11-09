#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace CienciaDelas
{
    public static class VerificacaoTeclas
    {
        static void Exigir(bool valor, string motivo) { if (!valor) throw new Exception(motivo); }
        public static string Executar()
        {
            Exigir(Application.isPlaying, "Execute em Play.");
            var ui = InterfaceDiretor.Instance; var entrada = EntradaDiretor.Instance;
            var elementos = ui.GetComponentsInChildren<ElementoInterface>(true).ToDictionary(e => e.Chave);
            void Desenhar() { ui.Atualizar(); ui.SendMessage("LateUpdate"); Canvas.ForceUpdateCanvases(); }
            string Foco(int jogador) => elementos.Values.Single(e => e.Chave.StartsWith("Mesa/") &&
                e.Chave.Contains("J" + (jogador + 1)) && e.Chave.EndsWith("/Cursor do jogador") &&
                e.gameObject.activeInHierarchy && e.GetComponent<Image>().enabled).Chave;
            elementos["Menu/Iniciar partida"].GetComponent<Button>().onClick.Invoke();
            foreach (var j in JogadoresDiretor.Instance.Jogadores) JogadoresDiretor.Instance.Comprar(j, 2);
            Desenhar();
            var tecladoAnterior = Keyboard.current;
            Keyboard teclado = null;
            var settings = InputSystem.settings;
            var fundoAnterior = settings.backgroundBehavior;
            var editorAnterior = settings.editorInputBehaviorInPlayMode;
            void Pressionar(params Key[] teclas)
            {
                InputSystem.QueueStateEvent(teclado, new KeyboardState(teclas)); InputSystem.Update();
                entrada.ProcessarTeclado(teclado); Desenhar();
                InputSystem.QueueStateEvent(teclado, new KeyboardState()); InputSystem.Update();
            }
            try
            {
                // Tool-driven checks run outside Game View; use the game's input buffer temporarily.
                settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
                settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
                InputSystem.Update();
                teclado = InputSystem.AddDevice<Keyboard>();
                InputSystem.Update();
                string j1 = Foco(0), j2 = Foco(1);
                Pressionar(Key.D);
                Exigir(Foco(0).Contains("espaco 2") && Foco(1) == j2, "D afetou a mão errada.");
                Pressionar(Key.RightArrow);
                Exigir(Foco(1).Contains("espaco 2") && Foco(0).Contains("espaco 2"), "Seta afetou J1.");
                Pressionar(Key.A, Key.LeftArrow);
                Exigir(Foco(0) == j1 && Foco(1) == j2, "Navegação simultânea misturou os focos.");
                Pressionar(Key.Z, Key.Enter);
                var plan = PlanejamentoDiretor.Instance;
                Exigir(plan.Fila.Count == 2 && plan.Fila.Count(c => c.jogador == 0) == 1 && plan.Fila.Count(c => c.jogador == 1) == 1,
                    "Z/Enter simultâneos não selecionaram uma carta de cada mão.");
                Pressionar(Key.X);
                Exigir(plan.Fila.Count == 1 && plan.Fila[0].jogador == 1, "X removeu carta de J2.");
                Pressionar(Key.Backspace);
                Exigir(plan.Fila.Count == 0, "Backspace não cancelou J2.");
                Pressionar(Key.Z); Pressionar(Key.Z);
                Exigir(plan.Fila.Count == 0, "Selecionar novamente não retirou a carta.");
                for (int n = 0; n < 14; n++) Pressionar(Key.D);
                Exigir(Foco(0) == j1 && Foco(1) == j2, "A navegação ultrapassou a mão de J1 ou perdeu o ciclo.");
                Pressionar(Key.Tab);
                Exigir(!ui.MesaAberta, "Caderno não abriu.");
                Pressionar(Key.RightArrow); Pressionar(Key.Escape);
                Exigir(ui.MesaAberta && Foco(0) == j1 && Foco(1) == j2, "Caderno alterou os focos das mãos.");
                Pressionar(Key.Escape);
                Exigir(PartidaDiretor.Instance.Pausada, "Esc não pausou.");
                Pressionar(Key.Escape);
                Exigir(ui.MesaAberta && Foco(0) == j1 && Foco(1) == j2, "Pausa perdeu os focos.");
                AmeacasDiretor.Instance.Revelar(); Desenhar();
                string ameaca = elementos["Mesa/Ameaca 1/Nome"].GetComponent<TMPro.TMP_Text>().text;
                Pressionar(Key.E);
                Exigir(elementos["Mesa/Ameaca 1/Nome"].GetComponent<TMPro.TMP_Text>().text != ameaca && Foco(0) == j1 && Foco(1) == j2, "E não mudou só a ameaça.");
                Pressionar(Key.Q);
                Exigir(elementos["Mesa/Ameaca 1/Nome"].GetComponent<TMPro.TMP_Text>().text == ameaca, "Q não voltou à ameaça.");
                Pressionar(Key.A);
                Exigir(Foco(0).Contains("Confirmar J1"), "J1 não alcança a confirmação com A/D.");
                Pressionar(Key.Z);
                Exigir(JogadoresDiretor.Instance.Jogadores[0].confirmado && !JogadoresDiretor.Instance.Jogadores[1].confirmado, "Confirmação cruzou jogadores.");
                Exigir(Foco(1) == j2, "Confirmar J1 deslocou o foco de J2.");
                Pressionar(Key.D, Key.Z);
                Exigir(!JogadoresDiretor.Instance.Jogadores[1].confirmado, "J1 confirmado interferiu em J2.");
                Pressionar(Key.LeftArrow);
                Exigir(Foco(1).Contains("Confirmar J2"), "J2 não alcança a confirmação com setas.");
                Pressionar(Key.Enter);
                Exigir(PartidaDiretor.Instance.Fase == Fase.Execucao, "As duas confirmações não iniciaram a execução.");
                Exigir(elementos.Values.Where(e => e.Chave.StartsWith("Mesa/Tecla ")).Count(e => e.GetComponent<Image>().sprite != null) == 10,
                    "Ícones de teclado ausentes.");
                return "PASS: eventos simulados no Input System; A/D exclusivos de J1, setas exclusivas de J2, dois focos persistentes, entradas simultâneas, Z/Enter, cancelamentos separados, ciclo de seis cartas e confirmação, pausa/caderno, Q/E e dez ícones associados.";
            }
            finally
            {
                if (teclado != null) InputSystem.RemoveDevice(teclado);
                if (tecladoAnterior != null && tecladoAnterior.added) tecladoAnterior.MakeCurrent();
                settings.backgroundBehavior = fundoAnterior;
                settings.editorInputBehaviorInPlayMode = editorAnterior;
            }
        }
    }
}
#endif
