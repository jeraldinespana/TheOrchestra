using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource waitingRoom;
    public AudioSource audition;

    void Awake()
    {
        Instance = this;
    }
    
    public void PlayWaiting()
    {
        if (musicSource.clip == waitingRoom) 
            return;
        musicSource.Stop();
        musicSource = waitingRoom;
        musicSource.Play();
    }

    public void PlayAud()
    {
        if (musicSource.clip == audition)
            return;
        musicSource.Stop();
        musicSource = audition;
        musicSource.Play();
    }
}
