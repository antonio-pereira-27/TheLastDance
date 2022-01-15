using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutScene3 : MonoBehaviour
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
        _camera1.SetActive(false);
        _camera2.SetActive(true);

        _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.sounds[1].audioSource.Stop();
        _audioManager.Play("suspense");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0f)
        {
            _camera1.transform.LookAt(_player.transform.position);
            _player.GetComponent<Animator>().SetInteger("state", 0);
            playerAgent.destination = positions[0].position;
        }
        
        if (timer >=7f)
            _player.GetComponent<Animator>().SetInteger("state", 2);

        if (timer >= 9f)
            speech = "Cade: Enough games, tell me where my niece is or I'll kill you right here!";
        
        if (timer > 12f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);
            _camera1.transform.LookAt(_boss.transform.position);
            speech = "Will: They will kill me if I tell you!";
        }
        
        if (timer > 17f)
        {
            _camera1.SetActive(false);
            _camera2.SetActive(true);
            _player.GetComponent<Animator>().SetInteger("state", 1);
            speech = "Cade: Well, I guess you have to pick me and make a run for it..";
        }

        if (timer > 20f)
            speech = "Because I will put a bullet through your head right now if you don't speak!";

        if (timer > 25f)
        {
            _camera1.SetActive(true);
            _camera2.SetActive(false);
            _camera1.transform.LookAt(_boss.transform.position);
            speech = "Will: Alright alright! The base of operations right now is in the Manchester Building.";
        }

        if (timer > 33f)
            _player.GetComponent<Animator>().SetInteger("state", 2);
        
        if (timer > 35f)
        {
            _player.GetComponent<Animator>().SetInteger("state", 0);
            playerAgent.destination = positions[1].position;
            speech = "C: Good luck with your run.";
        }

        if (timer > 39f)
        {
            _audioManager.sounds[11].audioSource.Stop();
            _audioManager.sounds[1].audioSource.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


    }
}
