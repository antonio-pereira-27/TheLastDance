using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
   private Rigidbody _rigidbody;
   public Rigidbody Rigidbody => _rigidbody;
   private void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }
}
