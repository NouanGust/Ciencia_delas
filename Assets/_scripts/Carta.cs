using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Carta : MonoBehaviour, IPointerClickHandler
{

    public int custo;
    public int dano;
    public string nome;
    public bool especial;




    bool taSelecionada = false;
    Vector3 posOriginal;
    Vector3 posSelecionada;
    float tempoAnimacao = 0.9f;


    

    private void Start()
    {
        posOriginal = transform.localPosition;
        posSelecionada = posOriginal + new Vector3(0f, 50f, 0f);
    }


    public void OnPointerClick(PointerEventData ed)
    {
        int cDisp = MaoJogador1.Instance.conhecimentoDisp;

        if (!taSelecionada)
        {
            if (custo <= cDisp)
            {
                taSelecionada = true;
                ScriptSound.Instance.PlaySelect();
                MaoJogador1.Instance.CalcularJogada(custo, this);
                AtualizarVisual(true);
            } else
            {
                Debug.Log("Conhecimento insuficiente");
                ScriptSound.Instance.PlaySwap();
                EfeitosDiretor.Instance.TremerCarta(this);
                AtualizarVisual(false);


            }

        } else
        {
            taSelecionada = false;
            MaoJogador1.Instance.ResetarJogada(custo);
            AtualizarVisual(false);
        }
        
 

    }



    

    private void AtualizarVisual(bool valida)
    {
        transform.DOKill();
        if (valida){
            transform.DOLocalMove(posSelecionada, tempoAnimacao).SetEase(Ease.OutBack);
        } else
        {
            transform.DOLocalMove(posOriginal, tempoAnimacao).SetEase(Ease.OutQuad);
        }

    }




}
