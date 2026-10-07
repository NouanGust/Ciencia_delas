using DG.Tweening;
using TMPro;
using UnityEngine;

public class EfeitosDiretor : MonoBehaviour
{
    public static EfeitosDiretor Instance { private set; get; }

    public RectTransform objetoParaTeremer;
    public float duracaoShake = 0.3f;

    public float forcaShake = 30f;

    public GameObject prefabTextoDano;
    public Transform canvasPai;

    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }



    public void TremerTela()
    {
        objetoParaTeremer.DOComplete();
        objetoParaTeremer.DOShakeAnchorPos(duracaoShake, forcaShake, vibrato: 30, randomness: 90);

    }

    public void TremerCarta(Carta carta)
    {
        RectTransform tf = carta.GetComponentInParent<RectTransform>();

        tf.DOComplete();
        tf.DOShakePosition(duracaoShake, strength: new Vector3(forcaShake, 0f, 0f), vibrato: 30, randomness: 90, fadeOut: true);
    }


    public void MostrarDano(int valorDano, RectTransform posicao)
    {
        GameObject objDano = Instantiate(prefabTextoDano, canvasPai);

        objDano.transform.position = posicao.position; 

        TextMeshProUGUI textoMesh = objDano.GetComponent<TextMeshProUGUI>();
        textoMesh.text = $"-{valorDano}";

        Color corOriginal = textoMesh.color;
        corOriginal.a = 1f;
        textoMesh.color = corOriginal;

        float tempoAnim = 1.3f;


        float aleX = Random.Range(-1.5f, 1.5f);
        float aleY = Random.Range(-0.5f, 0.5f);
        Vector3 posDestino = posicao.position + new Vector3(aleX, aleY, 0f);

        objDano.transform.DOJump(posDestino, jumpPower: 1.5f, numJumps: 1, duration: tempoAnim);

        textoMesh.DOFade(0f, tempoAnim).SetEase(Ease.InExpo).OnComplete(() =>
        {
            Destroy(objDano);
        });
    }


}
