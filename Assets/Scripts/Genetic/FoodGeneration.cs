using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGeneration : MonoBehaviour
{
    
    [SerializeField] GameObject foodPrefab;
    [SerializeField] GameObject world;
    [Range(1, 1000)][SerializeField] int foodSpawnRate;
    private int foodSpawnCounter = 0;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foodSpawnCounter++;
        if(foodSpawnCounter % foodSpawnRate == 0)
        {
            SpawnFood();
        }
    }

    public void SpawnFood()
    {
        // float xOffset = Random.Range(-world.transform.localScale.x/2, world.transform.localScale.x/2);
        // float zOffset = Random.Range(-world.transform.localScale.z/2, world.transform.localScale.z/2);

        // float xFood = GaussianRange.NextGaussian(0f, .3413f * world.transform.localScale.x /2);
        // float zFood = GaussianRange.NextGaussian(0f, .3413f * world.transform.localScale.z /2);

        float xFood = GaussianRange.GetGaussian_ZeroMean();
        float zFood = GaussianRange.GetGaussian_ZeroMean();

        Color foodColor = new Color(1f - xFood, 1f - (xFood*zFood), 1f - zFood);
        GameObject newFood = Instantiate(foodPrefab, world.transform.position + new Vector3(xFood * world.transform.localScale.x/2 , 1.5f, zFood * world.transform.localScale.z/2), Quaternion.identity);
        // newFood.tag = "food";
        // newFood.AddComponent<MeshRenderer>();
        newFood.GetComponent<MeshRenderer>().material.SetColor("_Color", foodColor);
        newFood.transform.parent = gameObject.transform;
    }
}
