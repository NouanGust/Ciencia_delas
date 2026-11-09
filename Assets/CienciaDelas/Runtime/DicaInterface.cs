using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CienciaDelas
{
    [DisallowMultipleComponent]
    public sealed class DicaInterface : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, ISelectHandler, IDeselectHandler
    {
        [Tooltip("Nome exibido ao passar o mouse. Deixe vazio para usar os links dos ícones no texto TMP.")]
        public string nome;
        public TooltipDiretor tooltip;
        public bool mostrarAoSelecionar;
        private TMP_Text texto;
        private bool sobre;
        private Camera cameraUI;

        public void OnPointerEnter(PointerEventData e) { sobre = true; cameraUI = e.enterEventCamera; Atualizar(e.position); }
        public void OnPointerMove(PointerEventData e) { Atualizar(e.position); }
        public void OnPointerExit(PointerEventData e) { sobre = false; tooltip?.Ocultar(this); }

        public void OnSelect(BaseEventData e)
        {
            if (!mostrarAoSelecionar || tooltip == null) return;
            var canvas = GetComponentInParent<Canvas>();
            cameraUI = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            var r = (RectTransform)transform;
            Atualizar(RectTransformUtility.WorldToScreenPoint(cameraUI, r.TransformPoint(new Vector2(r.rect.xMax, r.rect.yMax))));
        }
        public void OnDeselect(BaseEventData e) { if (!sobre) tooltip?.Ocultar(this); }

        void Update()
        {
            if (sobre && Mouse.current != null) Atualizar(Mouse.current.position.ReadValue());
        }

        void Atualizar(Vector2 posicao)
        {
            if (tooltip == null) return;
            string dica = nome;
            if (string.IsNullOrEmpty(dica))
            {
                if (texto == null) texto = GetComponent<TMP_Text>();
                int indice = texto != null ? TMP_TextUtilities.FindIntersectingLink(texto, posicao, cameraUI) : -1;
                if (indice < 0) { tooltip.Ocultar(this); return; }
                dica = texto.textInfo.linkInfo[indice].GetLinkID();
            }
            tooltip.Mostrar(this, dica, posicao, cameraUI);
        }

        void OnDisable() { sobre = false; tooltip?.Ocultar(this); }
    }
}
