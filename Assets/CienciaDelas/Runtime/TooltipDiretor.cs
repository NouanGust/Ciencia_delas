using TMPro;
using UnityEngine;

namespace CienciaDelas
{
    public sealed class TooltipDiretor : MonoBehaviour
    {
        public RectTransform painel;
        public TextMeshProUGUI texto;
        public Vector2 distanciaDoCursor = new Vector2(20, -20);
        [Min(100)] public float larguraMaxima = 430;
        private DicaInterface origem;

        public void Mostrar(DicaInterface dono, string nome, Vector2 posicaoNaTela, Camera cameraUI)
        {
            if (painel == null || texto == null || string.IsNullOrWhiteSpace(nome)) return;
            origem = dono;
            texto.text = nome;
            var area = (RectTransform)painel.parent;
            float limite = Mathf.Max(80, Mathf.Min(larguraMaxima, area.rect.width - 24));
            var medida = texto.GetPreferredValues(nome, limite - 32, 0);
            // Keep the measured wrapping width: shrinking it can create an extra clipped line.
            painel.sizeDelta = new Vector2(limite, Mathf.Min(medida.y + 28, area.rect.height - 24));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, posicaoNaTela, cameraUI, out var local);
            local += distanciaDoCursor;
            if (dono != null && dono.mostrarAoSelecionar)
            {
                // Card descriptions sit above the hand, leaving the card under the mouse visible.
                var carta = (RectTransform)dono.transform;
                var topo = area.InverseTransformPoint(carta.TransformPoint(new Vector2(carta.rect.center.x, carta.rect.yMax)));
                local.y = topo.y + painel.sizeDelta.y + 12;
            }
            var r = area.rect;
            local.x = Mathf.Clamp(local.x, r.xMin + 12, r.xMax - painel.sizeDelta.x - 12);
            local.y = Mathf.Clamp(local.y, r.yMin + painel.sizeDelta.y + 12, r.yMax - 12);
            painel.anchoredPosition = local;
            painel.gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void Ocultar(DicaInterface dono = null)
        {
            if (dono != null && dono != origem) return;
            origem = null;
            if (painel != null) painel.gameObject.SetActive(false);
        }

        void Awake() { Ocultar(); }
        void OnDisable() { Ocultar(); }
    }
}
