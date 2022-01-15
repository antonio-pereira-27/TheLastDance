using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutScene4 : MonoBehaviour
{
    // VARIABLES
    private float timer;

    [HideInInspector]public string speech;
    
    // REFERENCES
    [SerializeField]private NavMeshAgent playerAgent;
    [SerializeField]private NavMeshAgent sarahAgent;
    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _sarah;
    
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
        timer += Time.deltaTime;
        if (timer > 0f)
        {
            sarahAgent.destination = positions[0].position;
            playerAgent.destination = positions[1].position;
            
            _player.GetComponent<Animator>().SetInteger("state", 0);
            _sarah.GetComponent<Animator>().SetInteger("state", 1);

        }

        if (timer > 10f)
        {
            _player.GetComponent<Animator>().SetInteger("state", 2);
            _sarah.GetComponent<Animator>().SetInteger("state", 0);
            
            _player.transform.LookAt(_sarah.transform.position);
            _sarah.transform.LookAt(_player.transform.position);
        }

        if (timer > 12f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);

            speech = "Cade: Sarah, are you ok?";
        }

        if (timer > 18f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);

            speech = "Sarah: Yes uncle.";
        }
        
        if (timer > 23f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);

            speech = "Cade: Did they hurt you?";
        }
        
        if (timer > 28f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);

            speech = "S: No, I'm fine!";
        }
        
        if (timer > 31f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);

            speech = "Cade: Let's get out of here!";
        }

        if (timer > 34f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);
            
            sarahAgent.destination = positions[2].position;
            playerAgent.destination = positions[3].position;
            
            _player.transform.LookAt(positions[2].transform.position);
            _sarah.transform.LookAt(positions[3].transform.position);
            
            _player.GetComponent<Animator>().SetInteger("state", 0);
            _sarah.GetComponent<Animator>().SetInteger("state", 1);
        }

        if (timer > 42f)
        {
            _audioManager.sounds[11].audioSource.Stop();
            _audioManager.sounds[1].audioSource.Play();
            SceneManager.LoadScene("StartMenu");
        }

    }
}
