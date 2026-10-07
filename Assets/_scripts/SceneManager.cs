//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CenaManager : MonoBehaviour
//{
    
//    public static CenaManager Instance { get; private set; }


//    private void Awake()
//    {
       
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//        // Não destroi o objeto ao trocar de cena
//        DontDestroyOnLoad(gameObject);
//    }

//    public void LoadScene(string sceneName)
//    {
//        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
//    }

//    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
//    {
//        //if (loadingScreenCanvasGroup != null)
//        //{
//        //    yield return StartCoroutine(FadeLoadingScreen(1f));
//        //}

//        // Chamada de função
//        // Chamada de função

//        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

//        operation.allowSceneActivation = false;

//        while (!operation.isDone)
//        {
//            if (operation.progress >= 0.9f)
//            {
//                operation.allowSceneActivation = true;
//            }
//            yield return null;
//        }

//        if (loadingScreenCanvasGroup != null)
//        {
//            yield return StartCoroutine(FadeLoadingScreen(0f));
//        }
//    }

   
//}
