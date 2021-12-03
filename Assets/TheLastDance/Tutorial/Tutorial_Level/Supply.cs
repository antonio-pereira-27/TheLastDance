using System;
using UnityEngine;

namespace TheLastDance.Tutorial.Tutorial_Level
{
    public class Supply : MonoBehaviour
    {
        private float time = 0;
        private float speed = 50f;
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
                Debug.Log("This are the supplys that youll get while playing! The Med kit will give you 20 life points and the bullet will give you one loader on the gun you have active");
            }
        }
    }
}