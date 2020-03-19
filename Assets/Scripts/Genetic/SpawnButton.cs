using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject animalPrefab;
    [SerializeField] GameObject population;
    [SerializeField] Slider moveSlider;
    [SerializeField] Text moveValueText;
    [SerializeField] Slider fertilitySlider;
    [SerializeField] Text fertilityValueText;
    [SerializeField] Slider fecunditySlider;
    [SerializeField] Text fecundityValueText;
    [SerializeField] Slider lifeSlider;
    [SerializeField] Text lifeValueText;


    float[] fakeParents = new float[2];
    int numSpawned = 0;

    void Start()
    {
        // buttonReproductionTraits.Add("eggsInCarton", 3f);
        // buttonReproductionTraits.Add("fertility", .5f);
        // buttonReproductionTraits.Add("refractoryPeriod", 200f);
        // buttonReproductionTraits.Add("lifeSpan", 3000f);

        fakeParents[0] = 0.5f;
        fakeParents[1] = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        moveValueText.text = moveSlider.value.ToString();
        fertilityValueText.text = fertilitySlider.value.ToString();
        fecundityValueText.text = fecunditySlider.value.ToString();
        lifeValueText.text = lifeSlider.value.ToString();

    }

    public void ButtonSpawn()
    {
        Color spawnColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));        
        //spawn info
        Vector3 thisSpot = spawnPoint.transform.position;
        
        //update reproductionTraits
        //not using eggs anymore right?
        Dictionary<string, float> buttonReproductionTraits = new Dictionary<string, float>();
        buttonReproductionTraits.Add("eggsInCarton", 3f);
        buttonReproductionTraits.Add("fertility", fertilitySlider.value);
        buttonReproductionTraits.Add("refractoryPeriod", fecunditySlider.value);
        buttonReproductionTraits.Add("lifeSpan", lifeSlider.value);
        
        //fake parent thing
        float[] fakeParents = new float[2];
        fakeParents[0] = 0.5f;
        fakeParents[1] = 0.5f;

        DNA animal = new DNA(spawnColor, 0.3f, buttonReproductionTraits, moveSlider.value, fakeParents);
        // animal.moveSeed = e.data.GetField("speed").f;
        // animal.fertility = e.data.GetField("fertility").f;
        // animal.refractoryPeriod = e.data.GetField("reload").f;
        // animal.lifeSpan = e.data.GetField("life").f;


        //same as Birth() but immaculate
        GameObject newAnimal = Instantiate(animalPrefab, thisSpot, Quaternion.identity);
        newAnimal.transform.eulerAngles += new Vector3(0f, Random.Range(0f, 360f), 0f);
        newAnimal.transform.parent = population.transform;
        newAnimal.name = "animal: " + numSpawned;
        numSpawned++;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = animal.moveSeed;
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = animal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", animal.colorDNA);
    }
}
