using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCore : MonoBehaviour
{
    public EvolutionManager evolutionManager;
    
    public DNA thisAnimal;
    public float refractoryPeriodCounter = 0;
    public float lifeSpanCounter = 0;


    void Start()
    {
        // if (thisAnimal == null)
        //     thisAnimal = new Animal();
        evolutionManager = GameObject.Find("GameEngine").GetComponent<EvolutionManager>();
    }

    void FixedUpdate()
    {
        // thisAnimal.lifeSpan--;
        lifeSpanCounter++;
        if (thisAnimal.lifeSpan <= lifeSpanCounter)
        {
            Debug.Log(gameObject.name + " has died");
            Destroy(gameObject);
        }
        else
            // thisAnimal.refractoryPeriod--;
            refractoryPeriodCounter++;
    }

    public void Diddle(GameObject _thisAnimal, GameObject _thatAnimal)
    {
        //why not in class? idk
        // Animal thatAnimal = _thatAnimal.GetComponent<AnimalCore>().thisAnimal;

        DNA thatAnimal = _thatAnimal.GetComponent<AnimalCore>().thisAnimal;

        //conceive
        // Debug.Log("conception");
        // Debug.Log("this animal name: " + thisAnimal.animalName);
        // Debug.Log("that animal name: " + thatAnimal.animalName);


        // Animal babyAnimal = Conceive(thisAnimal.genes, thatAnimal.genes);
        DNA babyAnimal = Conceive(thisAnimal, thatAnimal);

        //spot
        Vector3 birthplace = Vector3.Lerp(gameObject.transform.position, _thatAnimal.transform.position, 0.5f);
        //birth
        Birth(birthplace, babyAnimal);
        thisAnimal.eggsInCarton--;
        refractoryPeriodCounter = 0;
        // thisAnimal.refractoryPeriod = thisAnimal.refractoryDefault;
        thatAnimal.eggsInCarton--;
        _thatAnimal.GetComponent<AnimalCore>().refractoryPeriodCounter = 0;
        // thatAnimal.refractoryPeriod = thatAnimal.refractoryDefault;

    }

    // public Animal Conceive(DNA _pitcherDNA, DNA _catcherDNA) //confused about what should be in class and what should be in soul... soul is only individual things?
    public DNA Conceive(DNA _pitcherDNA, DNA _catcherDNA)

    {
        DNA babyDNA = new DNA();
        return babyDNA.Mutate(_pitcherDNA, _catcherDNA);
        // return new Animal("animal: " + evolutionManager.numAnimals, babyDNA.Mutate(_pitcherDNA, _catcherDNA));
    }
    
    // public void Birth(Vector3 _birthplace, Animal _babyAnimal) //pop the baby out
    public void Birth(Vector3 _birthplace, DNA _babyAnimal) //pop the baby out

    {
        GameObject newAnimal = Instantiate(evolutionManager.animalPrefab, _birthplace, Quaternion.identity);
        newAnimal.transform.parent = evolutionManager.population.transform;
        newAnimal.name = "animal: " + evolutionManager.numAnimals;
        evolutionManager.numAnimals++;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = Mathf.FloorToInt(_babyAnimal.moveSeed);
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = _babyAnimal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", _babyAnimal.colorDNA);
        //update points
        // Debug.Log("")
        evolutionManager.player1score += _babyAnimal.pointArray[0];
        evolutionManager.player2score += _babyAnimal.pointArray[1];
    }
}
