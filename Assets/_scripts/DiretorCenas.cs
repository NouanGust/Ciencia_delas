#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DiretorCenas : MonoBehaviour
{
    public static DiretorCenas Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Função que vai ser executada quando clicar no botão "Iniciar"
    public void Iniciar()
    {
        Debug.Log("Clicou no botão Inicar");

        ScriptSound.Instance.PlayButton();

        SceneManager.LoadScene("CenaPrincipal");
    } 

    // Função que vai ser executada quando clicar no botão "Opções"
    public void Opcoes()
    {
        Debug.Log("Clicou no botão Opções");
        ScriptSound.Instance.PlayButton();
    }

    // Função que vai ser executada quando clicar no botão "Sair"
    public void Sair()
    {
        Debug.Log("Clicou no botão Sair");
        StartCoroutine(TocarSomESair());


    }

    IEnumerator TocarSomESair()
    {
        ScriptSound.Instance?.PlayMenuCancel();

        // Se a música for privada, eu não consigo acessar a duração dela diretamente, preciso de uma função para isso.
        yield return new WaitForSecondsRealtime(1f);

        // Se a música for publica, eu consigo acessar a duração dela diretamente.
        //yield return new WaitForSeconds(ScriptSound.Instance.menuCancelSfx.length);

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif

        // Dentro da Unity
        // Fora da Unity
        Application.Quit();
    }
    
}
