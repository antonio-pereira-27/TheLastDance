using System;
using UnityEngine;

namespace TheLastDance.Tutorial.Tutorial_Level
{
    public class Supply : MonoBehaviour
    {
        private float time = 0;
        private float speed = 50f;
        [HideInInspector] public bool trigger;
        private void Update()
        {
            time += Time.deltaTime;
            Quaternion yAxis = Quaternion.Euler(0, time * speed, 0);
            transform.rotation = yAxis;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                trigger = true;
            }
        }
    }
}