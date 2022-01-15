using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutScene2 : MonoBehaviour
{ 
    // VARIABLES
    private float timer;
    public bool watch;

    [HideInInspector]public string speech;
    [HideInInspector]public string instructions;
    
    // REFERENCES
    [SerializeField]private NavMeshAgent playerAgent;
    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;
    [SerializeField] private GameObject head;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _boss;
    
    [SerializeField] private Transform[] positions;

    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        _camera1.SetActive(false);
        _camera2.SetActive(true);

        _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.sounds[1].audioSource.Stop();
        _audioManager.Play("suspense");

        instructions = "";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        _camera1.transform.position = head.transform.position;
        if (timer >= 0f)
        {
            _player.GetComponent<Animator>().SetInteger("state", 0);
            playerAgent.destination = positions[0].position;
        }

        if (timer >= 6.5f)
            _player.GetComponent<Animator>().SetInteger("state", 1);
        
        if (timer >= 8f)
            speech = "Cade: Where is my niece?";
        
        if (timer > 11f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);
            _player.GetComponent<Animator>().SetInteger("state", 0);
            speech = "Eve: I'm loyal to this cause, you'll get nothing from me!";
        }
        if (timer > 16f)
        {
            playerAgent.destination = positions[2].position;
            _camera1.transform.LookAt(positions[1]);

        }
        
        if (timer > 18f)
        {
            _player.GetComponent<Animator>().SetInteger("state", 2);
            instructions = "Press E to watch the Object";
        }

        if (timer >= 21f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (watch)
                {
                    Time.timeScale = 1;
                    instructions = "Press E to watch the Object";
                    watch = false;
                }
                else
                {
                    Time.timeScale = 0;
                    instructions = "Press E to exit this view";
                    speech = "";
                    watch = true;
                }
                
            }
            
        }

        if (timer >= 25f)
            speech = "Cade: Train ticket to Manchester? What was your business in Manchester?";
        
        if (timer > 27f)
        {
            playerAgent.destination = positions[3].position;
            _camera1.transform.LookAt(positions[3].position);
            _player.GetComponent<Animator>().SetInteger("state", 0);
        }
        
        if (timer > 29f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);
            speech = "Eve: NOTHING!!";
        }

        if (timer > 75f)
        {
            _audioManager.sounds[11].audioSource.Stop();
            _audioManager.sounds[1].audioSource.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
    
}
