using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    // References
    public GameObject pistol;
    public GameObject rifle;

    // Start is called before the first frame update
    void Start()
    {
       pistol.SetActive(false);
       rifle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pistol.SetActive(false);
            rifle.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pistol.SetActive(true);
            rifle.SetActive(false);
        }
    }
}
