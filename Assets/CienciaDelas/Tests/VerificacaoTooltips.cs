#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CienciaDelas
{
    public static class VerificacaoTooltips
    {
        static void Exigir(bool valor, string motivo) { if (!valor) throw new Exception(motivo); }
        public static string Executar()
        {
            var ui = InterfaceDiretor.Instance;
            Exigir(ui != null, "Execute a verificação em Play, com a mesa aberta.");
            var es = EventSystem.current;
            var elementos = ui.GetComponentsInChildren<ElementoInterface>(true).ToDictionary(e => e.Chave);
            var dicas = ui.GetComponentInChildren<TooltipDiretor>(true);
            var canvas = ui.GetComponentInChildren<Canvas>();
            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            Canvas.ForceUpdateCanvases();
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            string[] chaves = { "SAÚDE", "INFRAESTRUTURA", "PESQUISA" };
            string[] nomes = { "Saúde", "Infraestrutura", "Pesquisa" };
            for (int n = 0; n < chaves.Length; n++)
            {
                var icon = elementos["Mesa/Icone " + chaves[n]];
                var rt = (RectTransform)icon.transform;
                var ponto = RectTransformUtility.WorldToScreenPoint(cam, rt.TransformPoint(rt.rect.center));
                var evento = new PointerEventData(es) { position = ponto };
                var hits = new List<RaycastResult>(); raycaster.Raycast(evento, hits);
                Exigir(hits.Any(h => h.gameObject == icon.gameObject), "Ícone não recebe o mouse: " + chaves[n]);
                ExecuteEvents.Execute(icon.gameObject, evento, ExecuteEvents.pointerEnterHandler);
                Exigir(dicas.painel.gameObject.activeSelf && dicas.texto.text == nomes[n], "Nome incorreto no tooltip: " + chaves[n]);
                Exigir(!elementos["Mesa/Rotulo " + chaves[n]].gameObject.activeSelf, "Rótulo antigo ainda aparece.");
                ExecuteEvents.Execute(icon.gameObject, evento, ExecuteEvents.pointerExitHandler);
                Exigir(!dicas.painel.gameObject.activeSelf, "Tooltip não fechou ao sair.");
            }
            int links = 0;
            foreach (var texto in ui.GetComponentsInChildren<TextMeshProUGUI>().Where(t => t.text.Contains("<link=")))
            {
                texto.ForceMeshUpdate();
                for (int n = 0; n < texto.textInfo.linkCount; n++)
                {
                    var link = texto.textInfo.linkInfo[n];
                    var caractere = texto.textInfo.characterInfo[link.linkTextfirstCharacterIndex];
                    var centro = (caractere.bottomLeft + caractere.topRight) * .5f;
                    var ponto = RectTransformUtility.WorldToScreenPoint(cam, texto.transform.TransformPoint(centro));
                    var evento = new PointerEventData(es) { position = ponto };
                    ExecuteEvents.Execute(texto.gameObject, evento, ExecuteEvents.pointerEnterHandler);
                    Exigir(dicas.painel.gameObject.activeSelf && dicas.texto.text == link.GetLinkID(), "Tooltip de ícone dentro do texto falhou: " + texto.name);
                    ExecuteEvents.Execute(texto.gameObject, evento, ExecuteEvents.pointerExitHandler);
                    links++;
                }
            }
            Exigir(links >= 2, "Nenhum indicador com ícone dentro do texto foi validado.");
            var origem = elementos["Mesa/Icone SAÚDE"].GetComponent<DicaInterface>();
            foreach (var ponto in new[] { Vector2.zero, new Vector2(Screen.width, Screen.height), new Vector2(Screen.width, 0) })
            {
                dicas.Mostrar(origem, "Infraestrutura", ponto, cam);
                var area = (RectTransform)dicas.painel.parent;
                var corners = new Vector3[4]; dicas.painel.GetWorldCorners(corners);
                foreach (var c in corners) Exigir(area.rect.Contains(area.InverseTransformPoint(c)), "Tooltip ultrapassou a borda da tela.");
            }
            Exigir(dicas.painel.GetComponentsInChildren<Graphic>(true).All(g => !g.raycastTarget), "Tooltip intercepta cliques.");
            dicas.Ocultar();
            Exigir(ui.GetComponentsInChildren<TextMeshProUGUI>(true).All(t => t.font.name == "Xmind SDF"), "Há textos com a fonte anterior.");
            return "PASS: fonte Xmind, três ícones do cabeçalho, " + links + " ícones nos textos, nomes corretos, saída do mouse, limites da tela e tooltip sem interceptar cliques.";
        }
    }
}
#endif
