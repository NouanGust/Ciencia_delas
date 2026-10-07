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
    public int saudeMax = 10;


    private List<Carta> cartasSelecionadas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        } else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        custoInicial = custoMax;
        ameacaPos = ameaca.GetComponent<RectTransform>();
        saudeIni = 5;
        saude = saudeIni;
        AtualizaUI();
    }


    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
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
        ScriptSound.Instance.PlaySelect();
        danoTotal = MaoJogador1.Instance.danoTotal;

        ameaca.TomarDano(danoTotal);
        EfeitosDiretor.Instance.MostrarDano(danoTotal, ameacaPos);
        EfeitosDiretor.Instance.TremerTela();

        //foreach (c cartasSelecionadas)
        //{
        //    Destroy(c.gameObject);

        //}

    }

    public void TurnoInimigo()
    {
        // Se morreu retorna (cria outra ameaça)
        var danoTotal = ameaca.Atacar();
        ScriptSound.Instance.PlaySpecial();
        if (danoTotal > saude)
        {
            FimDeJogo();
        }
        saude -= danoTotal;
        AtualizaUI();
    }


    public void AtualizaUI()
    {
        conhTxt.text = $"Conhecimento: {custoInicial}/{custoMax}";

        saudeTxt.text = $"Saúde: {saude}/{saudeMax}";
    }


    public void FimDeJogo()
    {
        Time.timeScale = 0f;
        DiretorCenas.Instance.Sair();
    }
}
