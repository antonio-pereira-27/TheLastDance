using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTutorial : MonoBehaviour
{
    [HideInInspector] public bool trigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            trigger = true;

    }
}
