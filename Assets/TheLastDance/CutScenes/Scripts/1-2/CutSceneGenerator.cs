using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutSceneGenerator : MonoBehaviour
{
    // VARIABLES
    private float timer;

    [HideInInspector]public string speech;
    
    // REFERENCES
    [SerializeField]private NavMeshAgent playerAgent;
    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _boss;
    
    [SerializeField] private Transform[] positions;

    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        _camera1.SetActive(true);
        _camera2.SetActive(false);

        _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.sounds[1].audioSource.Stop();
        _audioManager.Play("suspense");
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0f)
        {
            _camera1.transform.LookAt(_boss.transform);
            _player.GetComponent<Animator>().SetInteger("state", 0);
            playerAgent.destination = positions[0].position;
            timer += Time.deltaTime;
        }

        if (timer >=7f)
            _player.GetComponent<Animator>().SetInteger("state", 1);
        
        if (timer >= 9f)
            speech = "Cade: Where is Sarah?";
        
        if (timer > 12f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);
            timer += Time.deltaTime;
            speech = "Adam: You will never get her back!";
        }
        if (timer > 17f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);
            _camera1.transform.LookAt(_boss.transform);
            timer += Time.deltaTime;
            speech = "Cade: Tell me or I'll make sure you suffer before you die!";
        }
        if (timer > 27f)
        {
            timer += Time.deltaTime;
            speech = "Adam: Screw it, you'll never get there in time... They're holding her in a warehouse in the southern part of London.";
        }

        if (timer > 37f)
        {
            timer += Time.deltaTime;
            speech = "Cade: That wasn't so hard was it?";
        }

        if (timer > 47f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);
        }

        if (timer > 57f)
        {
            _camera2.transform.LookAt(positions[1]);
            _audioManager.Play("dead");
            speech = "*Adam Dies*";
        }
        
        if (timer > 67f)
        {
            _player.GetComponent<Animator>().SetInteger("state", 0);
            playerAgent.destination = positions[1].position;
            
            speech = "";
        }

        if (timer > 75f)
        {
            _audioManager.sounds[11].audioSource.Stop();
            _audioManager.sounds[1].audioSource.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        
        
    }
    
}
