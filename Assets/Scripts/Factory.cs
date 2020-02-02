using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ProductType { 
    CarTire,
    NeedleAndThread,




    _Count

}

public class Factory : MonoBehaviour
{
    [SerializeField]
    GameObject[] productPrefabs;
    [SerializeField]
    GameObject factoryOutputPosition;

    [SerializeField]
    SignpostController signpostController;

    GameObject factoryInput;
    GameObject factoryOutput;



    [SerializeField]
    ProductType currentRepairTarget = ProductType.CarTire;

    int acceptedMaterials = 0;

    // Update is called once per frame
    void Update()
    {


        if (factoryInput != null)
        {
            //Debug.Log("Factory input object found! Processing...");
            RawMaterial incomingMaterial = factoryInput.GetComponent<RawMaterial>();
            if (incomingMaterial != null && !incomingMaterial.broken)
            {
                //Debug.Log("Checking raw material type...");
                CheckRawMaterial(incomingMaterial);
            }
        }
    }

    void CheckRawMaterial(RawMaterial incomingMaterial)
    {
        RawMaterialType[] recipe = CheckRecipe(currentRepairTarget);
        //Debug.Log("Checking recipes...");
        //Debug.Log("Comparing input type " + incomingMaterial.GetRMType() + " against recipe type " + recipe[acceptedMaterials] + "...");
        if (incomingMaterial.GetRMType() ==  recipe[acceptedMaterials] )
        {
            
            acceptedMaterials += 1;

            signpostController.SetSignCompleted(acceptedMaterials);

            incomingMaterial.broken = true;
            Destroy(factoryInput);
            //Debug.Log("Match! Accepted materials count is " + acceptedMaterials);
            if (acceptedMaterials == recipe.Length) {
                Invoke("CreateProduct", 1f);

            }
        }
        else
        {
            //Debug.Log("Mismatch! Destroying...");
            Destroy(factoryInput, 0.5f);

        }
    }

    void CreateProduct()
    {
        acceptedMaterials = 0;
        SelectNextProductType();


        //Debug.Log("Creating new products...");
        if ( productPrefabs[0] != null && factoryOutputPosition != null )
        {
            factoryOutput = Instantiate(productPrefabs[0], factoryOutputPosition.transform.position, Quaternion.identity);
        }
        else
        {
            //Debug.Log("Trying to create products, problem in productPrefabs or Output Position!");
        }
    }

    void SelectNextProductType()
    {
        currentRepairTarget = (ProductType)Random.Range(0, (int) ProductType.Count );


        signpostController.SetNewRecipe(currentRepairTarget, CheckRecipe(currentRepairTarget));
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

            case ProductType.NeedleAndThread:
                recipe = new RawMaterialType[2];
                recipe[0] = RawMaterialType.Metal;
                recipe[1] = RawMaterialType.Cotton;
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
