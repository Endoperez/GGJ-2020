using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProductType { 
    CarTire,
    NeedleAndThread

}

public class Factory : MonoBehaviour
{
    [SerializeField]
    GameObject FactoryInput;
    [SerializeField]
    GameObject FactoryOutput;

    [SerializeField]
    GameObject[] productPrefabs;

    [SerializeField]
    ProductType currentRepairTarget = ProductType.CarTire;

    int acceptedMaterials = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FactoryInput != null)
        {
            RawMaterial rawMaterial = GetComponent<RawMaterial>();
                if (rawMaterial != null)
            {
                CheckRawMaterial(rawMaterial);
            }
        }
    }

    void CheckRawMaterial(RawMaterial incomingMaterial)
    {
        RawMaterialType[] recipe = CheckRecipe(currentRepairTarget);
        if (incomingMaterial.GetRMType() ==  recipe[acceptedMaterials] )
        {
            acceptedMaterials += 1;
            if (acceptedMaterials == recipe.Length) {
                CreateProduct();
                acceptedMaterials = 0;
                SelectNextProductType();
            }
        }
        else
        {
            Destroy(FactoryInput, 0.5f);

        }
    }

    void CreateProduct()
    {

    }

    void SelectNextProductType()
    {
        currentRepairTarget = ProductType.CarTire;
    }

    RawMaterialType[] CheckRecipe( ProductType targetProduct)
    {
        RawMaterialType[] recipe;
        switch (targetProduct)
        {
            case ProductType.CarTire:
                recipe = new RawMaterialType[3];
                recipe[0] = RawMaterialType.Plastic;
                recipe[1] = RawMaterialType.Plastic;
                recipe[2] = RawMaterialType.Metal;

                return recipe;

            default:
                recipe = new RawMaterialType[2];
                recipe[0] = RawMaterialType.Plastic;
                recipe[1] = RawMaterialType.Plastic;
                return recipe;
        }

       
    }
}
