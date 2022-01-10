using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    
    //References
    private CharacterController _controller;
    private AudioManager _audioManager;
    private PlayerMovement _player;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _player = GetComponent<PlayerMovement>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.isGrounded && _player.speed > 0.1f && _audioManager.sounds[4].audioSource.isPlaying == false)
        {
            _audioManager.sounds[4].audioSource.volume = Random.Range(0.2f, 0.5f);
            _audioManager.sounds[4].audioSource.pitch = Random.Range(0.8f, 1.1f);
            _audioManager.sounds[4].audioSource.Play();
        }
    }
}
