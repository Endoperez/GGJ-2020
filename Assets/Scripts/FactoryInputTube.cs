using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryInputTube : MonoBehaviour
{
    Factory parentFactory;

    private void Start()
    {
        parentFactory = transform.GetComponentInParent<Factory>();
        if (parentFactory == null)
        {
            Debug.LogError("ERROR! Can't find Factory component in parents of Factory Input Tube! ");
            this.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Checking what collided with factory input tube...");
        if(collision.transform.tag == "CarriableObject")
        {
            RawMaterial rawMaterialComponent = collision.gameObject.GetComponent<RawMaterial>();
            if (rawMaterialComponent == null)
                return;

            //Debug.Log("Adding objects to Factory as Factory Input object...");
            parentFactory.SetFactoryInput(collision.gameObject);
        }
    }
}
