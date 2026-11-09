#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CienciaDelas
{
    public static class VerificacaoCarrossel
    {
        static void Exigir(bool ok, string mensagem) { if (!ok) throw new Exception(mensagem); }
        public static string Executar()
        {
            Exigir(Application.isPlaying, "Execute em Play.");
            var ui = InterfaceDiretor.Instance;
            var config = ui.GetComponent<InicializadorJogo>().configuracao;
            var p = PartidaDiretor.Instance;
            var am = AmeacasDiretor.Instance;
            var desenhar = typeof(InterfaceDiretor).GetMethod("Desenhar", BindingFlags.Instance | BindingFlags.NonPublic);
            void Atualizar() { desenhar.Invoke(ui, null); Canvas.ForceUpdateCanvases(); }
            var elementos = ui.GetComponentsInChildren<ElementoInterface>(true).ToDictionary(e => e.Chave);
            GameObject Objeto(string nome) => elementos["Mesa/" + nome].gameObject;
            void Clicar(string nome) { Objeto(nome).GetComponent<Button>().onClick.Invoke(); Atualizar(); }
            bool Visivel(string nome) => Objeto(nome).activeInHierarchy;
            string Centro() => Objeto("Ameaca 1/Nome").GetComponent<TMP_Text>().text;

            p.Iniciar(Cientista.MarieCurie, config.regras, config.cartas, config.ameacas, 123);
            foreach (int quantidade in new[] { 0, 1, 2, 5, config.ameacas.Count })
            {
                am.Reiniciar(config.ameacas);
                for (int n = 0; n < quantidade; n++) am.Revelar();
                Atualizar();
                Exigir(Visivel("Sem ameacas") == (quantidade == 0), "Estado vazio incorreto.");
                Exigir(Visivel("Ameaca 1") == (quantidade > 0), "Ameaça central incorreta.");
                Exigir(Visivel("Ameaca 2") == (quantidade > 2) && Visivel("Ameaca 3") == (quantidade > 1), "Prévias duplicadas ou ausentes.");
                Exigir(Objeto("Mais ameacas").GetComponent<Button>().interactable == (quantidade > 1), "Navegação habilitada incorretamente.");
                if (quantidade == 0) continue;
                for (int n = 0; n < quantidade; n++)
                {
                    Exigir(Centro() == am.Ativas[n].dados.nome, "O carrossel pulou uma ameaça.");
                    var nome = Objeto("Ameaca 1/Nome").GetComponent<TMP_Text>(); nome.ForceMeshUpdate();
                    Exigir(!nome.isTextOverflowing, "Nome central cortado: " + nome.text);
                    Clicar("Mais ameacas");
                }
                Exigir(Centro() == am.Ativas[0].dados.nome, "O carrossel não voltou ao início.");
                Clicar("Ameaca anterior");
                Exigir(Centro() == am.Ativas[quantidade - 1].dados.nome, "Navegação inversa incorreta.");
                if (quantidade > 1) { Clicar("Ameaca 3"); Exigir(Centro() == am.Ativas[0].dados.nome, "Clique na prévia não centralizou a ameaça."); }
            }
            Clicar("Mais ameacas");
            var jogador = JogadoresDiretor.Instance.Jogadores[0];
            int carta = jogador.Mao.ToList().FindIndex(c => c.conhecimento <= jogador.conhecimento && RecursosDiretor.Instance.Atual.Cobre(c.custo));
            Clicar("Carta J1 espaco " + (carta + 1));
            Exigir(PlanejamentoDiretor.Instance.Fila.Single().alvo == am.Ativas[1], "A carta não recebeu o alvo central.");
            Clicar("Mais ameacas");
            Exigir(PlanejamentoDiretor.Instance.Fila.Single().alvo == am.Ativas[1], "Navegar mudou o alvo de uma carta já enfileirada.");
            Clicar("Carta J1 espaco " + (carta + 1));

            var dicas = ui.GetComponentInChildren<TooltipDiretor>(true);
            var canvas = ui.GetComponentInChildren<Canvas>();
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            var go = Objeto("Carta J1 espaco 1"); var rt = (RectTransform)go.transform;
            var evento = new PointerEventData(EventSystem.current) { position = RectTransformUtility.WorldToScreenPoint(cam, rt.TransformPoint(rt.rect.center)) };
            var hits = new List<RaycastResult>(); canvas.GetComponent<GraphicRaycaster>().Raycast(evento, hits);
            Exigir(hits.Count > 0 && hits[0].gameObject == go, "Carta encoberta por outro elemento.");
            ExecuteEvents.Execute(go, evento, ExecuteEvents.pointerEnterHandler);
            Exigir(dicas.painel.gameObject.activeSelf && dicas.texto.text.Contains(jogador.Mao[0].descricao), "Descrição não abriu ao passar o mouse.");
            ExecuteEvents.Execute(go, evento, ExecuteEvents.pointerExitHandler);
            Exigir(!dicas.painel.gameObject.activeSelf, "Descrição permaneceu aberta após sair.");
            ExecuteEvents.Execute(go, new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
            Exigir(dicas.painel.gameObject.activeSelf, "Descrição inacessível pelo teclado.");
            ExecuteEvents.Execute(go, new BaseEventData(EventSystem.current), ExecuteEvents.deselectHandler);
            Exigir(!dicas.painel.gameObject.activeSelf, "Descrição não fechou ao perder o foco.");
            foreach (var c in config.cartas)
            {
                var descricao = (string)typeof(InterfaceDiretor).GetMethod("DescricaoCarta", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ui, new object[] { c });
                foreach (var canto in new[] { Vector2.zero, new Vector2(Screen.width, Screen.height) })
                {
                    dicas.Mostrar(go.GetComponent<DicaInterface>(), descricao, canto, cam);
                    dicas.texto.ForceMeshUpdate();
                    Exigir(!dicas.texto.isTextOverflowing, "Descrição cortada: " + c.nome);
                    var vertices = new Vector3[4]; dicas.painel.GetWorldCorners(vertices);
                    var area = (RectTransform)dicas.painel.parent;
                    Exigir(vertices.All(v => area.rect.Contains(area.InverseTransformPoint(v))), "Descrição fora da tela.");
                }
            }
            Exigir(dicas.painel.GetComponentsInChildren<Graphic>().All(g => !g.raycastTarget), "Descrição bloqueia cliques.");
            Clicar("Pausa"); Exigir(p.Pausada && !dicas.painel.gameObject.activeSelf, "Pausa ou fechamento de dica falhou.");
            p.Pausar(); Atualizar(); Clicar("Informacoes");
            Exigir(ui.Telas.Single(t => t.nome == "Caderno").raiz.gameObject.activeSelf, "Caderno não abriu.");
            ui.Cancelar(); Atualizar();
            am.Reiniciar(config.ameacas); am.Revelar(); Atualizar();
            Exigir(Centro() == am.Ativas[0].dados.nome, "Ameaça removida permaneceu selecionada.");
            am.Revelar(); am.Revelar();
            foreach (var j in JogadoresDiretor.Instance.Jogadores) JogadoresDiretor.Instance.Comprar(j, 2);
            Atualizar(); dicas.Ocultar();
            Exigir(Visivel("Carta J1 espaco 6") && Visivel("Carta J2 espaco 6"), "Sexta carta não aparece.");
            return "PASS: 0/1/2/5/15 ameaças, ciclo nos dois sentidos, prévias, alvo das cartas, remoção do alvo, descrições de todas as cartas sem cortes, bordas da tela, mouse/teclado, cliques, pausa, caderno e seis cartas por mão.";
        }
    }
}
#endif
