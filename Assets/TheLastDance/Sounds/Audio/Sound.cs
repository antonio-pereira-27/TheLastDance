using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
   // variables
   public string name;
   [Range(0f, 1f)]
   public float volume;
   [Range(0.1f, 3f)]
   public float pitch;
   public bool loop;
   
   //references
   public AudioClip clip;
   
   [HideInInspector] public AudioSource audioSource;

}
