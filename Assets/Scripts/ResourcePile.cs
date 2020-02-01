using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{
    [SerializeField]
    RawMaterialType resourcePileType = RawMaterialType.Metal;

    public RawMaterialType CheckType()
    {
        return resourcePileType;
    }

}


