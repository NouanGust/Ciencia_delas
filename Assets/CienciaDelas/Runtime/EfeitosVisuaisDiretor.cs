using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CienciaDelas
{
    public sealed class EfeitosVisuaisDiretor : SingletonComponente<EfeitosVisuaisDiretor>
    {
        private ConfiguracaoJogo config;
        private RectTransform palco,camada;
        private Vector2 origemPalco;
        private Vector3 origemCamera;
        private Transform cameraTransform;
        private Tween tremor, tremorCamera;
        private readonly Dictionary<Texture2D,Sprite[]> frames=new Dictionary<Texture2D,Sprite[]>();
        public bool MovimentoReduzido { get; private set; }
        public void Inicializar(ConfiguracaoJogo c,RectTransform root,RectTransform layer)
        {
            config=c; palco=root; camada=layer; origemPalco=root.anchoredPosition;
            cameraTransform=Camera.main!=null?Camera.main.transform:null; if(cameraTransform!=null) origemCamera=cameraTransform.localPosition;
            MovimentoReduzido=PlayerPrefs.GetInt("CienciaDelas.MovimentoReduzido",0)==1;
            var particulas = GetComponent<ParticulasUIDiretor>();
            if (particulas == null) particulas = gameObject.AddComponent<ParticulasUIDiretor>();
            particulas.Inicializar(layer);
            PartidaDiretor.Instance.CartaExecutada+=Carta;
            PartidaDiretor.Instance.AmeacaResolvida+=Ameaca;
            PartidaDiretor.Instance.Evento+=Evento;
            RecursosDiretor.Instance.Alterados+=Recursos;
            PartidaDiretor.Instance.Alterada+=Pausa;
        }
        public void AlternarMovimento()
        {
            MovimentoReduzido=!MovimentoReduzido; PlayerPrefs.SetInt("CienciaDelas.MovimentoReduzido",MovimentoReduzido?1:0); PlayerPrefs.Save(); Limpar();
        }
        void Pausa() { if(PartidaDiretor.Instance.Pausada) Limpar(); }
        Image Imagem(string nome,Sprite sprite,Vector2 pos,Vector2 tamanho)
        {
            var i=new GameObject(nome,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image)).GetComponent<Image>(); i.transform.SetParent(camada,false); i.sprite=sprite; i.preserveAspect=true; i.raycastTarget=false;
            i.rectTransform.anchorMin=i.rectTransform.anchorMax=Vector2.zero; i.rectTransform.anchoredPosition=pos; i.rectTransform.sizeDelta=tamanho; return i;
        }
        public void Tremer(float multiplicador=1)
        {
            if(MovimentoReduzido || config.aparencia==null) return;
            var a=config.aparencia; tremor?.Kill(); palco.anchoredPosition=origemPalco;
            tremor=palco.DOShakeAnchorPos(a.duracaoShake,a.intensidadeShake*multiplicador,18,70,false,true).SetUpdate(true).SetLink(gameObject).OnComplete(()=>palco.anchoredPosition=origemPalco);
            if(cameraTransform==null) return;
            tremorCamera?.Kill(); cameraTransform.localPosition=origemCamera;
            tremorCamera=cameraTransform.DOShakePosition(a.duracaoShake,new Vector3(.045f,.045f,0)*multiplicador,18,70,false,true).SetUpdate(true).SetLink(gameObject).OnComplete(()=>cameraTransform.localPosition=origemCamera);
        }
        void Carta(Jogada jogada)
        {
            if(MovimentoReduzido || PartidaDiretor.Instance.Terminou) return;
            var a=config.aparencia; var destino=new Vector2(800,600);
            var cor=jogada.carta.cientista==Cientista.MarieCurie?(a!=null?a.brilhoMarie:Color.magenta):(a!=null?a.brilhoJaqueline:Color.cyan);
            var origem=InterfaceDiretor.Instance.PosicaoCarta(jogada.jogador,jogada.carta.id);
            var fantasma=Imagem("Carta executada",config.Imagem(jogada.carta.id),origem,new Vector2(105,158));
            var seq=DOTween.Sequence().SetUpdate(true).SetLink(fantasma.gameObject).SetId(this);
            seq.Append(fantasma.rectTransform.DOAnchorPos(destino,.35f).SetEase(Ease.OutCubic));
            seq.Join(fantasma.rectTransform.DOScale(1.35f,.35f));
            seq.AppendCallback(()=>{ ParticulasUIDiretor.Instance.Emitir(destino,a!=null?a.pesquisa:null,cor,a!=null?a.quantidadeParticulas:14); if(jogada.carta.cientista!=Cientista.Normal) { Sequencia(a!=null?a.slash:null,destino,260); Tremer(.45f); } });
            seq.Append(fantasma.DOFade(0,.18f)); seq.OnComplete(()=>Destroy(fantasma.gameObject));
        }
        void Ameaca(AmeacaAtiva ameaca,bool superada)
        {
            if(MovimentoReduzido) return;
            var pos=InterfaceDiretor.Instance.PosicaoAmeaca(ameaca.dados.id); var a=config.aparencia;
            Sequencia(a!=null?(superada?a.explosao:a.slash):null,pos,220);
            ParticulasUIDiretor.Instance.Emitir(pos,a!=null?(superada?a.conhecimento:a.saude):null,superada?new Color(.5f,1,.8f):new Color(1,.35f,.3f),a!=null?a.quantidadeParticulas:14);
            if(!superada) Tremer();
        }
        void Recursos(Recursos delta)
        {
            int[] valores={delta.saude,delta.infraestrutura,delta.pesquisa};
            for(int n=0;n<3;n++)
            {
                if(valores[n]==0 || MovimentoReduzido) continue;
                var go = new GameObject("Variação do recurso", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                go.transform.SetParent(camada, false);

                var t = go.GetComponent<TextMeshProUGUI>();
                if (config.fonte != null) t.font = config.fonte;
                t.fontSize = 26;
                t.alignment = TextAlignmentOptions.Center;
                t.raycastTarget = false;
                t.text = (valores[n] > 0 ? "+" : "") + valores[n];
                t.color = valores[n] > 0 ? new Color(.45f, 1, .65f) : new Color(1, .4f, .4f);
                t.text=(valores[n]>0?"+":"")+valores[n]; t.color=valores[n]>0?new Color(.45f,1,.65f):new Color(1,.4f,.4f);
                var r=t.rectTransform; r.anchorMin=r.anchorMax=Vector2.zero; r.sizeDelta=new Vector2(90,40); r.anchoredPosition=new Vector2(450+n*330,850);
                var seq=DOTween.Sequence().SetUpdate(true).SetLink(go).SetId(this); seq.Join(r.DOAnchorPosY(910,.85f)); seq.Join(t.DOFade(0,.85f)); seq.OnComplete(()=>Destroy(go));
            }
        }
        void Sequencia(Texture2D textura,Vector2 pos,float tamanho)
        {
            if(textura==null || config.aparencia==null) return;
            var a=config.aparencia;
            if(!frames.TryGetValue(textura,out var sprites))
            {
                int cel=Mathf.Max(1,a.tamanhoCelula); int total=Mathf.Min(a.quadros,textura.width/cel-a.primeiraColuna);
                int y=textura.height-(a.linhaDoTopo+1)*cel;
                if(total<=0 || y<0) return;
                sprites=new Sprite[total];
                for(int n=0;n<total;n++) sprites[n]=Sprite.Create(textura,new Rect((a.primeiraColuna+n)*cel,y,cel,cel),new Vector2(.5f,.5f),cel);
                frames.Add(textura,sprites);
            }
            var img=Imagem("Slash animado",sprites[0],pos,Vector2.one*tamanho);
            var seq=DOTween.Sequence().SetUpdate(true).SetLink(img.gameObject).SetId(this);
            foreach(var sprite in sprites) { var frame=sprite; seq.AppendCallback(()=>img.sprite=frame); seq.AppendInterval(1f/Mathf.Max(1,a.quadrosPorSegundo)); }
            seq.OnComplete(()=>Destroy(img.gameObject));
        }
        void Evento(string evento)
        {
            if(evento=="menu" || evento=="inicio") Limpar();
            if(evento=="erro") Tremer(.25f);
            if(evento=="vitoria" && !MovimentoReduzido)
            {
                var a=config.aparencia;
                for(int n=0;n<3;n++) ParticulasUIDiretor.Instance.Emitir(new Vector2(450+n*350,650),a!=null?a.conhecimento:null,new Color(1,.82f,.3f),24);
            }
            if(evento=="derrota") Tremer(1.3f);
        }
        void Limpar()
        {
            DOTween.Kill(this); tremor?.Kill(); tremorCamera?.Kill();
            if(palco!=null) palco.anchoredPosition=origemPalco;
            if(cameraTransform!=null) cameraTransform.localPosition=origemCamera;
            ParticulasUIDiretor.Instance?.Limpar();
            if(camada!=null) for(int n=camada.childCount-1;n>=0;n--) if(camada.GetChild(n).name!="Partícula") Destroy(camada.GetChild(n).gameObject);
        }
        protected override void OnDestroy()
        {
            PartidaDiretor.Instance.CartaExecutada-=Carta; PartidaDiretor.Instance.AmeacaResolvida-=Ameaca; PartidaDiretor.Instance.Evento-=Evento;
            PartidaDiretor.Instance.Alterada-=Pausa; RecursosDiretor.Instance.Alterados-=Recursos; Limpar();
            foreach(var sprites in frames.Values) foreach(var s in sprites) if(s!=null) Destroy(s);
            frames.Clear(); base.OnDestroy();
        }
    }
}
