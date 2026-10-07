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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        foreach (GameObject s in slots)
        {
            var c = s.GetComponentInChildren<Carta>();
            cartas.Add(c);
        }

        AtualizarUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
