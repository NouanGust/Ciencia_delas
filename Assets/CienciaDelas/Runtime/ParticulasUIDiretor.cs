using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CienciaDelas
{
    // Partículas em coordenadas de UI: ficam visíveis no Canvas e não exigem material de outra render pipeline.
    public sealed class ParticulasUIDiretor : SingletonComponente<ParticulasUIDiretor>
    {
        private readonly List<Image> pool=new List<Image>();
        private RectTransform camada;
        public void Inicializar(RectTransform layer) { camada=layer; }
        public void Emitir(Vector2 origem,Sprite sprite,Color cor,int quantidade)
        {
            if(camada==null) return;
            for(int n=0;n<Mathf.Clamp(quantidade,0,40);n++)
            {
                Image img=pool.Find(x=>!x.gameObject.activeSelf);
                if(img==null)
                {
                    if(pool.Count>=96) break;
                    img=new GameObject("Partícula",typeof(RectTransform),typeof(CanvasRenderer),typeof(Image)).GetComponent<Image>();
                    img.transform.SetParent(camada,false); img.raycastTarget=false; pool.Add(img);
                }
                img.gameObject.SetActive(true); img.sprite=sprite; img.color=cor;
                var rt=img.rectTransform; rt.anchorMin=rt.anchorMax=Vector2.zero; rt.pivot=new Vector2(.5f,.5f);
                rt.anchoredPosition=origem; rt.sizeDelta=Vector2.one*Random.Range(8f,20f); rt.localScale=Vector3.one; rt.localRotation=Quaternion.identity;
                float vida=Random.Range(.45f,.95f); Vector2 direcao=Random.insideUnitCircle.normalized*Random.Range(45f,170f);
                var seq=DOTween.Sequence().SetUpdate(true).SetLink(img.gameObject).SetId(this);
                seq.Join(rt.DOAnchorPos(origem+direcao,vida).SetEase(Ease.OutQuad));
                seq.Join(rt.DOScale(.05f,vida)); seq.Join(rt.DOLocalRotate(new Vector3(0,0,Random.Range(-150f,150f)),vida)); seq.Join(img.DOFade(0,vida));
                var alvo=img; seq.OnComplete(()=>alvo.gameObject.SetActive(false));
            }
        }
        public void Limpar() { DOTween.Kill(this); foreach(var img in pool) if(img!=null) img.gameObject.SetActive(false); }
        protected override void OnDestroy() { Limpar(); base.OnDestroy(); }
    }
}
