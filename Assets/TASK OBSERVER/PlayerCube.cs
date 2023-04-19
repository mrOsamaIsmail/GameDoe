using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : Subject
{
  
    private void Awake()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        Notify(Notification.GROUND_HIT);
    }
}
