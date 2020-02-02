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
    GameObject destructionParticleObject;

    public bool isCarried = false;
    public bool broken;





    // Start is called before the first frame update
    void Start()
    {
        
    }


    public RawMaterialType GetRMType()
    {
        return type;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCarried)
            return;

        string otherTag = collision.gameObject.tag;
        if ( otherTag == "Ground")
        {
            broken = true;

            if (destructionParticleObject != null)
            {
                GameObject particleSystem = Instantiate(destructionParticleObject, transform.position, Quaternion.identity);
                Destroy(particleSystem, 5f);
            }
            Destroy(gameObject, 0.05f);
        }
    }
}
