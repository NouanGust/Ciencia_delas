using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MaoJogador1 : MonoBehaviour
{

    public static MaoJogador1 Instance;


    public List<GameObject> slots;
    public List<Carta> cartas;

    public int conhecimentoMax = 3;
    public int conhecimentoIni;
    public int conhecimentoDisp;

    public int danoTotal = 0;

    public TextMeshProUGUI conhTxt;



    private void Awake()
    {
        Instance = this;

        conhecimentoIni = conhecimentoMax;
        conhecimentoDisp = conhecimentoIni;

    }


    void Start()
    {
        Debug.Log($"[Mao Joagador 1 {cartas}");

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject s = slots[i];
            Carta carta_a = cartas[i];
            //var c = s.GetComponentInChildren<Carta>();

            Debug.Log($"[Mao Jogador 1] ${carta_a.nome}");
            Debug.Log($"[Mao Jogador 1] ${carta_a.custo}");
            Debug.Log($"[Mao Jogador 1] ${carta_a.dano}");

            Instantiate(carta_a, s.gameObject.transform);

        }

        AtualizarUI();
        
    }


    public void AtualizarUI()
    {
        conhTxt.text = $"Conhecimento: {conhecimentoDisp}/{conhecimentoMax}";
    }

    public void CalcularJogada(int custo, Carta carta)
    {

        if (custo <= conhecimentoDisp)
        {
            danoTotal += carta.dano;
            conhecimentoDisp -= custo;
            AtualizarUI();
        }

    }

    public void ResetarJogada(int custo)
    {
        conhecimentoDisp += custo;
        danoTotal = 0 ;
        if (conhecimentoDisp > conhecimentoMax)
        {
            conhecimentoDisp = conhecimentoMax;
        }
        AtualizarUI();

    }
}
