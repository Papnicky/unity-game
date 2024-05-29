
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip bg;
    public AudioClip jump;
    public AudioClip checkpoint;
    public AudioClip collect;
    public AudioClip chest;
    public AudioClip attack;
    public AudioClip npc;
    public AudioClip enemydeath;
    public AudioClip portal;
    



    private void Start()
    {
        MusicSource.clip = bg;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip) 
    {
        SFXSource.PlayOneShot(clip);
    }
}
