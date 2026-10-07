using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSound : MonoBehaviour
{
    public static ScriptSound Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // AudioSource é como a caixa de som
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic; // AudioClip é como a música
    [SerializeField] private AudioClip inGameMusic;
    [SerializeField] private AudioClip finalThreatMusic;

    [Tooltip("Tempo da transição entre as músicas.")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Menu SFX")]
    [SerializeField] private AudioClip cancelSfx;
    [SerializeField] private AudioClip menuCancelSfx;
    [SerializeField] private AudioClip menuSelectSfx;
    [SerializeField] private AudioClip playSfx;

    [Header("Game SFX")]
    [SerializeField] private AudioClip selectSfx;
    [SerializeField] private AudioClip specialSfx;
    [SerializeField] private AudioClip swapSfx;

    private Coroutine musicTransition;
    private float musicVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        // Faz o Diretor de Audio permanecer entre cenas!
        DontDestroyOnLoad(gameObject);

        if (musicSource != null)
        {
            musicVolume = musicSource.volume;
        }
        else
        {
            musicVolume = 1f;
        }

        // Entrando no evento de carregar a cena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        UpdateMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            // Saindo do evento de carregar a cena  
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
    }

    private void UpdateMusicForScene(string sceneName)
    {
        AudioClip selectedMusic = null;
        // Toca a música do menu principal
        if (sceneName == "Menu inicio")
        {
            selectedMusic = menuMusic;
        }
        // Toca a música da cena principal
        else if (sceneName == "CenaPrincipal")
        {
            selectedMusic = inGameMusic;
        }

        if (selectedMusic != null)
        {
            ChangeMusic(selectedMusic);
        }
    }

    private void ChangeMusic(AudioClip newMusic)
    {
        if (musicSource == null || newMusic == null)
        {
            return;
        }

        if (musicSource.clip == newMusic && musicSource.isPlaying)
        {
            return;
        }

        if (musicTransition != null)
        {
            StopCoroutine(musicTransition);
        }

        musicTransition = StartCoroutine(ChangeMusicRoutine(newMusic));
    }

    private IEnumerator ChangeMusicRoutine(AudioClip newMusic)
    {
        
        if (fadeDuration <= 0f)
        {
            musicSource.Stop();
            musicSource.clip = newMusic;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();

            musicTransition = null;
            yield break;
        }

        if (musicSource.isPlaying)
        {
            float initialVolume = musicSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;

                musicSource.volume = Mathf.Lerp(
                    initialVolume,
                    0f,
                    elapsedTime / fadeDuration
                );

                yield return null;
            }
        }

        musicSource.Stop();

        musicSource.clip = newMusic;
        musicSource.loop = true;
        musicSource.volume = 0f;
        musicSource.Play();

        float fadeInTime = 0f;

        while (fadeInTime < fadeDuration)
        {
            fadeInTime += Time.unscaledDeltaTime;

            musicSource.volume = Mathf.Lerp(
                0f,
                musicVolume,
                fadeInTime / fadeDuration
            );

            yield return null;
        }

        musicSource.volume = musicVolume;
        musicTransition = null;
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Métodos publicos, que serão usados pelos outros scripts

    public void PlayMenuMusic()
    {
        ChangeMusic(menuMusic);
    }

    public void PlayInGameMusic()
    {
        ChangeMusic(inGameMusic);
    }

    public void PlayFinalThreatMusic()
    {
        ChangeMusic(finalThreatMusic);
    }

    public void PlayCancel()
    {
        PlaySfx(cancelSfx);
    }

    public void PlayMenuCancel()
    {
        PlaySfx(menuCancelSfx);
    }

    public void PlayMenuSelect()
    {
        PlaySfx(menuSelectSfx);
    }

    public void PlayButton()
    {
        PlaySfx(playSfx);
    }

    public void PlaySelect()
    {
        PlaySfx(selectSfx);
    }

    public void PlaySpecial()
    {
        PlaySfx(specialSfx);
    }

    public void PlaySwap()
    {
        PlaySfx(swapSfx);
    }


    public bool TaTocandoSFX()
    {
        if (sfxSource.isPlaying){
            return true;
        }
        else
        {
            return false;
        }
    } 


    // Me fala (retorna) qual a duração da música em segundos!
    public float DuracaoSFX()
    {
        return sfxSource.clip.length;
    }
}