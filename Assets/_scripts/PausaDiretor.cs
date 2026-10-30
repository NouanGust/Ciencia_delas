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
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            taPausado = !taPausado;
            Time.timeScale = taPausado ? 0f : 1f;
        }

        // Aplico o estado no jogo, mudando a escala do tempo
        if (taPausado)
        {
            if (menuPanel != null) menuPanel.SetActive(true);
        }
        else
        {
            if (menuPanel != null) menuPanel.SetActive(false);
        }

    }

}
