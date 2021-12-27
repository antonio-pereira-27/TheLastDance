using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    // REFERENCES
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject weaponOlder;

    private PickableItem _pickableItem;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (_pickableItem)
                return;
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 5f))
                {
                    var pickable = hit.transform.GetComponent<PickableItem>();

                    if (pickable)
                    {
                        FindObjectOfType<AudioManager>().Play("weaponPick");
                        weaponOlder.GetComponent<SwitchGuns>().enabled = true;
                    }
                       
                }
            }
        }
    }
    
}
