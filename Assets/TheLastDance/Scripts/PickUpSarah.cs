using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSarah : MonoBehaviour
{
    // VARIABLES
    [HideInInspector] public bool pickedUp = false;
    private PickableItem _item;
    
    // REFERENCES
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject sarahPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if(_item)
                return;
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 10f))
                {
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    if (pickable)
                    {
                        pickedUp = true;
                        this.transform.position = sarahPosition.transform.position;
                    }
                }
            }
        }
    }
}
