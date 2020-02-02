using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{
    [SerializeField]
    RawMaterialType resourcePileType = RawMaterialType.Metal;

    [SerializeField]
    GameObject instancePrefab;

    public RawMaterialType CheckType()
    {
        return resourcePileType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            Drone drone = other.GetComponentInParent<Drone>();
            if (drone == null)
            {
                Debug.Log("Resource pile " +gameObject.name +" detected object " +other.name );
                return;
            }

            if ( !drone.IsCarryingSomething() )
                Instantiate(instancePrefab, drone.GetHookPosition(), Quaternion.identity);
        }

    }

}


