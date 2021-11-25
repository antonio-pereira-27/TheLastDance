using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    // REFERENCES
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform weaponOlder;

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
                        PickItem(pickable);
                }
            }
        }
    }

    private void PickItem(PickableItem item)
    {
        _pickableItem = item;

        item.Rigidbody.isKinematic = true;
        item.Rigidbody.velocity = Vector3.zero;
        item.Rigidbody.angularVelocity = Vector3.zero;
        
        item.transform.SetParent(weaponOlder);
        
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }
}
