using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignpostController : MonoBehaviour
{
    [SerializeField]
    GameObject productionGoalSign, sign1, sign1Background, sign2, sign2Background, sign3, sign3Background;

    [SerializeField]
    Material[] productionGoalMaterials, signMaterials;

    [SerializeField]
    Material signEmpty, backgroundPlateOff, backgroundPlateOn;

    ProductType currentProductionGoal = ProductType.CarTire;


    // Start is called before the first frame update
    void Start()
    {
        InitSignpost();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSignpost()
    {
        ChangeMaterial(productionGoalSign, productionGoalMaterials[(int)currentProductionGoal]);

        //Debug.Log("Setting material type plastic, nr " + (int)RawMaterialType.Plastic + " to use material " + signMaterials[(int)RawMaterialType.Plastic].name);

        ChangeMaterial(sign1, signMaterials[(int)RawMaterialType.Plastic]);
        ChangeMaterial(sign2, signMaterials[(int)RawMaterialType.Plastic]);
        ChangeMaterial(sign3, signMaterials[(int)RawMaterialType.Metal]);

        ChangeMaterial(sign1Background, backgroundPlateOff);
        ChangeMaterial(sign2Background, backgroundPlateOff);
        ChangeMaterial(sign3Background, backgroundPlateOff);


    }

    public void SetNewRecipe()
    {
        InitSignpost();
        ChangeMaterial(productionGoalSign, productionGoalMaterials[(int)currentProductionGoal]);

        ChangeMaterial(sign1, signMaterials[(int)RawMaterialType.Plastic]);
        ChangeMaterial(sign2, signMaterials[(int)RawMaterialType.Plastic]);
        ChangeMaterial(sign3, signMaterials[(int)RawMaterialType.Metal]);

    }

    public void SetNewRecipe( ProductType newGoal, RawMaterialType[] recipe )
    {
        InitSignpost();
        currentProductionGoal = newGoal;
        ChangeMaterial(productionGoalSign, productionGoalMaterials[(int)currentProductionGoal]);


        ChangeMaterial(sign1, signMaterials[(int)recipe[0]]);
        ChangeMaterial(sign2, signMaterials[(int)recipe[1]]);
        if (recipe.Length == 3)
            ChangeMaterial(sign3, signMaterials[(int)recipe[2]]);
        else
            ChangeMaterial(sign3, signEmpty);

    }

    public void SetSignCompleted(int signNumber)
    {
        switch (signNumber)
        {
            case 1:
                ChangeMaterial(sign1Background, backgroundPlateOn);
                break;

            case 2:
                ChangeMaterial(sign2Background, backgroundPlateOn);
                break;

            case 3:
                ChangeMaterial(sign3Background, backgroundPlateOn);
                break;

            default:
                break;


        }

    }


    void ChangeMaterial(GameObject gameObject, Material mat)
    {
        gameObject.GetComponent<Renderer>().material = mat;
    }



}
