using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    float pushingSpeed = 1f ;

    private void OnCollisionStay(Collision collision)
    {
        Vector3 newPosition = collision.transform.position;
        newPosition.x += pushingSpeed * Time.fixedDeltaTime;
        
        collision.transform.position = newPosition;
    }


    private void OnCollisionExit(Collision collision)
    {
        Destroy(collision.gameObject, 20f);
    }
}

