using UnityEngine;

public class AudioManager : MonoBehaviour
{

    
    public AudioSource sfx;
    void Start()
    {

        sfx.PlayDelayed(5);
        sfx.Play();

        
    }

    void Update()
    {
        
    }



    public void PlaySwapSFX()
    {
        sfx.Play();
    }

    public void PlaySfx(string sfxName)
    {
        
    }

    public void PlaySfx(AudioSource sfx)
    {
        sfx.Play();
    }
}
