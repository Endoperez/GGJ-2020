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
    GameObject[] productPrefabs;
    [SerializeField]
    GameObject factoryOutputPosition;

    [SerializeField]
    GameObject factoryInput;
    [SerializeField]
    GameObject factoryOutput;



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


        if (factoryInput != null)
        {
            Debug.Log("Factory input object found! Processing...");
            RawMaterial rawMaterial = GetComponent<RawMaterial>();
            if (rawMaterial != null)
            {
                Debug.Log("Checking raw material type...");
                CheckRawMaterial(rawMaterial);
            }
        }
    }

    void CheckRawMaterial(RawMaterial incomingMaterial)
    {
        RawMaterialType[] recipe = CheckRecipe(currentRepairTarget);
        Debug.Log("Checking recipes...");
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
            Destroy(factoryInput, 0.5f);

        }
    }

    void CreateProduct()
    {
        Debug.Log("Creating new products...");
        if ( productPrefabs[0] != null && factoryOutputPosition != null )
        {
            factoryOutput = Instantiate(productPrefabs[0], factoryOutput.transform.position, Quaternion.identity);
        }
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

    public void SetFactoryInput(GameObject newInputObject)
    {
        factoryInput = newInputObject;

    }
}
