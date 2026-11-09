using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton para garantir que apenas uma instância exista
    public static GameManager Instancia { get; private set; }

    [Header("Configurações de Tempo")]
    [SerializeField] private float tempoRestante = 200f; // 200 segundos = 4 minutos
    [SerializeField] private float tempoAvisoCritico = 15f;
    private bool timerAtivo = true;

    [Header("Pontuação")]
    public int pontuacao = 0;

    [Header("Componentes de Interface (Use Tags nos objetos da UI)")]
    private TextMeshProUGUI textoTimer;
    private TextMeshProUGUI textoPontuacao;
    [SerializeField] private Color corNormal = Color.white;
    [SerializeField] private Color corCritica = Color.red;

    private void Awake()
    {
        // Configuração do Padrão Singleton
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // Protege o objeto inteiro (não apenas o script)
        }
        else
        {
            Destroy(gameObject); // Destrói cópias se você voltar para a cena inicial
        }
    }

    private void OnEnable()
    {
        // Inscreve-se no evento para saber quando uma nova cena é carregada
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    // Chamado automaticamente toda vez que uma cena nova abre
    private void AoCarregarCena(Scene scene, LoadSceneMode mode)
    {
        VincularInterface();
    }

    private void VincularInterface()
    {
        // Busca a UI na nova cena através de Tags. 
        // Lembre-se de criar e adicionar as tags "TextoTimer" e "TextoPontuacao" nos seus TextMeshPro na Unity.
        GameObject objTimer = GameObject.FindWithTag("TextoTimer");
        if (objTimer != null)
        {
            textoTimer = objTimer.GetComponent<TextMeshProUGUI>();
            textoTimer.color = tempoRestante <= tempoAvisoCritico ? corCritica : corNormal;
        }

        GameObject objPontuacao = GameObject.FindWithTag("TextoPontuacao");
        if (objPontuacao != null)
        {
            textoPontuacao = objPontuacao.GetComponent<TextMeshProUGUI>();
        }

        AtualizarDisplayTempo(tempoRestante);
        AtualizarDisplayPontuacao();
    }

    void Update()
    {
        if (timerAtivo)
        {
            if (tempoRestante > 0)
            {
                tempoRestante -= Time.deltaTime;
                AtualizarDisplayTempo(tempoRestante);

                if (textoTimer != null && tempoRestante <= tempoAvisoCritico)
                {
                    textoTimer.color = corCritica;
                }
            }
            else
            {
                tempoRestante = 0;
                timerAtivo = false;
                AtualizarDisplayTempo(tempoRestante);
                TempoEsgotado();
            }
        }
    }

    void AtualizarDisplayTempo(float tempoParaMostrar)
    {
        if (textoTimer == null) return;
        if (tempoParaMostrar < 0) tempoParaMostrar = 0;

        float minutos = Mathf.FloorToInt(tempoParaMostrar / 60);
        float segundos = Mathf.FloorToInt(tempoParaMostrar % 60);

        // Corrigido de {4:00} para {1:00}
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void AdicionarPontos(int pontos)
    {
        pontuacao += pontos;
        AtualizarDisplayPontuacao();
    }

    void AtualizarDisplayPontuacao()
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text = "Score: " + pontuacao.ToString("0000");
        }
    }

    void TempoEsgotado()
    {
        Debug.Log("O tempo acabou!");
        // Ação de fim de jogo aqui
    }
}