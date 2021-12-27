using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //variable
    [HideInInspector] public float volume;
    
    // REFERENCES
    public Sound[] sounds;
    public static AudioManager audioManager;
    // Start is called before the first frame update
    void Awake()
    {

        // verify if there is more than one audiomanager in scene
        if (audioManager == null)
        {
            audioManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        } 
        DontDestroyOnLoad(gameObject);
        
        // got the list of sounds and add the attributes to the audiosource component 
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;

            if (sound.audioSource.name == "Background")
                volume = sound.audioSource.volume;
           
        }
    }

    private void Start()
    {
        //play the background sound
        Play("Background");
    }

    public void Play(string name)
    {
        // find the sound name to play, if the name matches play it, else return an error
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.Log("No " + name + " Found");
            return;
        }
        sound.audioSource.Play();
    }
}
