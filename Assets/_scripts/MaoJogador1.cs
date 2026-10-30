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
    private readonly HashSet<Carta> selecao = new HashSet<Carta>();

    public TextMeshProUGUI conhTxt;



    private void Awake()
    {
        Instance = this;
        if (cartas == null) cartas = new List<Carta>();

        conhecimentoIni = conhecimentoMax;
        conhecimentoDisp = conhecimentoIni;

    }


    void Start()
    {
        MontarMao();
    }

    public void MontarMao()
    {
        Debug.Log($"[Mao Joagador 1 {cartas}");

        for (int i = 0; slots != null && i < slots.Count && i < cartas.Count; i++)
        {
            GameObject s = slots[i];
            Carta carta_a = cartas[i];
            //var c = s.GetComponentInChildren<Carta>();

            Debug.Log($"[Mao Jogador 1] {carta_a.nome}");
            Debug.Log($"[Mao Jogador 1] {carta_a.custo}");
            Debug.Log($"[Mao Jogador 1] {carta_a.dano}");


            if (carta_a != null && s != null)
            {
                carta_a.transform.SetParent(s.transform, false);
                carta_a.transform.localPosition = Vector3.zero;
            }

        }

        AtualizarUI();
        
    }


    public void AtualizarUI()
    {
        if (conhTxt != null) conhTxt.text = $"Conhecimento: {conhecimentoDisp}/{conhecimentoMax}";
    }

    public void CalcularJogada(int custo, Carta carta)
    {

        if (custo <= conhecimentoDisp && selecao.Add(carta))
        {
            danoTotal += carta.dano;
            conhecimentoDisp -= custo;
            AtualizarUI();
        }

    }

    public void ResetarJogada(int custo, Carta carta = null)
    {
        conhecimentoDisp += custo;
        if (carta != null) selecao.Remove(carta);
        danoTotal = 0;
        foreach (var selecionada in selecao) if (selecionada != null) danoTotal += selecionada.dano;
        if (conhecimentoDisp > conhecimentoMax)
        {
            conhecimentoDisp = conhecimentoMax;
        }
        AtualizarUI();

    }
}
