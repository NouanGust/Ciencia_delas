using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiretorGame : MonoBehaviour
{
    public static DiretorGame Instance { private set;  get; }

    private int custoTotal;
    private int custoInicial;
    private int custoMax = 3;
    private int danoTotal;

    public Carta carta;
    public Ameaca ameaca;
    public RectTransform ameacaPos;

    public TextMeshProUGUI conhTxt;
    public TextMeshProUGUI saudeTxt;

    public int saude;
    public int saudeIni;
    public int saudeMax = 30;


    private List<Carta> cartasSelecionadas = new List<Carta>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        custoInicial = custoMax;
        if (ameaca != null) ameacaPos = ameaca.GetComponent<RectTransform>();
        saudeIni = 10;
        saude = saudeIni;
        AtualizaUI();
    }


    private void Update()
    {
        if (Time.timeScale > 0 && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TurnoInimigo();

        }
    }


    public void CalcularJogada(int valor, Carta carta)
    {

        custoInicial -= valor;
        danoTotal += carta.dano;
        cartasSelecionadas.Add(carta);
        AtualizaUI();

    }

    public void ConfirmarJogada()
    {
        if (Time.timeScale == 0 || ameaca == null || MaoJogador1.Instance == null) return;
        ScriptSound.Instance?.PlaySelect();
        danoTotal = MaoJogador1.Instance.danoTotal;

        ameaca.TomarDano(danoTotal);
        if (EfeitosDiretor.Instance != null && ameacaPos != null) EfeitosDiretor.Instance.MostrarDano(danoTotal, ameacaPos);
        EfeitosDiretor.Instance?.TremerTela();

        //foreach (c cartasSelecionadas)
        //{
        //    Destroy(c.gameObject);

        //}

    }

    public void TurnoInimigo()
    {
        // Se morreu retorna (cria outra ameaça)
        if (ameaca == null || Time.timeScale == 0) return;
        var danoTotal = ameaca.Atacar();
        ScriptSound.Instance?.PlaySpecial();
        saude = Mathf.Max(0, saude - danoTotal);
        if (saude == 0)
        {
            FimDeJogo();
        }
        AtualizaUI();
    }


    public void AtualizaUI()
    {
        if (conhTxt != null) conhTxt.text = $"Conhecimento: {custoInicial}/{custoMax}";

        if (saudeTxt != null) saudeTxt.text = $"Saúde: {saude}/{saudeMax}";
    }


    public void FimDeJogo()
    {
        Time.timeScale = 0f;
        Debug.Log("Fim do protótipo. Abra a cena CienciaDelas para a partida cooperativa completa.");
    }
}
