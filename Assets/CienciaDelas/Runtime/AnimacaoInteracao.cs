using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CienciaDelas
{
    // Uma vista por botão; estado e configurações permanecem nos diretores singleton.
    public sealed class AnimacaoInteracao : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool carta;
        [HideInInspector] public bool especial;
        public float duracao=.16f;
        public System.Action aoFocar;
        private Tween escala;
        private bool sobre, selecionado;
        private Vector3 escalaOriginal;
        void Awake() { escalaOriginal = transform.localScale; }
        bool Ativo => GetComponent<Button>().IsInteractable() && (EfeitosVisuaisDiretor.Instance==null || !EfeitosVisuaisDiretor.Instance.MovimentoReduzido);
        void Escalar(float valor)
        {
            escala?.Kill();
            if(!Ativo) { transform.localScale=escalaOriginal; return; }
            escala=transform.DOScale(escalaOriginal * valor,duracao).SetEase(Ease.OutQuad).SetUpdate(true).SetLink(gameObject);
        }
        void Atualizar() { Escalar(sobre || selecionado?(carta?1.075f:1.035f):1); }
        public void OnPointerEnter(PointerEventData e) { sobre=true; aoFocar?.Invoke(); Atualizar(); }
        public void OnPointerExit(PointerEventData e) { sobre=false; Atualizar(); }
        public void OnSelect(BaseEventData e) { selecionado=true; aoFocar?.Invoke(); Atualizar(); }
        public void OnDeselect(BaseEventData e) { selecionado=false; Atualizar(); }
        public void OnPointerDown(PointerEventData e) { if(e.button==PointerEventData.InputButton.Left) Escalar(.96f); }
        public void OnPointerUp(PointerEventData e) { Atualizar(); }
        void OnDestroy() { escala?.Kill(); }
        void OnDisable() { escala?.Kill(); sobre = selecionado = false; if (Application.isPlaying) transform.localScale = escalaOriginal; }
    }
}
