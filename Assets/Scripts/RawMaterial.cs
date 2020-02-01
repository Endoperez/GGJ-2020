using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RawMaterialType
{
    Metal,
    Cotton,
    Plastic,
    Wood,
    Sand
}

public class RawMaterial : MonoBehaviour
{
    [SerializeField]
    RawMaterialType type = RawMaterialType.Metal;
    [SerializeField]
    GameObject DroneGrabbingPoint;


    bool dropped;
    bool collected;
    bool broken;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
        {
            if(DroneGrabbingPoint != null)
            {
                transform.position = DroneGrabbingPoint.transform.position;
            }
        }
        else if (dropped)
        {

        }
    }

    public void Drop()
    {
        dropped = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string otherTag = collision.otherCollider.tag;
        if ( otherTag == "Ground" || otherTag == "Factory")
        {
            broken = true;
            Destroy(gameObject, 5f);
        }
    }
}
