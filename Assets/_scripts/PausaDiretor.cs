using UnityEngine;
using UnityEngine.InputSystem;

public class PausaDiretor : MonoBehaviour
{
    // Variavel para controlar o estado do jogo (Rodando=true ou Pausado=false)
    public bool taPausado = false;

    // Para controlar a visibilidade do menu de pausa
    public GameObject menuPanel;

    private void Update()
    {

        // Define se o estado do jogo é Pausado(true) ou Rodando (false)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            taPausado = !taPausado;
        }

        // Aplico o estado no jogo, mudando a escala do tempo
        if (taPausado)
        {
            Time.timeScale = 0f;
            menuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            menuPanel.SetActive(false);
        }

    }

}
