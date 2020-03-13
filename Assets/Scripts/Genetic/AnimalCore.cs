using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCore : MonoBehaviour
{
    public EvolutionManager evolutionManager;
    
    public Animal thisAnimal;

    void Start()
    {
        // if (thisAnimal == null)
        //     thisAnimal = new Animal();
        evolutionManager = GameObject.Find("GameEngine").GetComponent<EvolutionManager>();
    }

    void FixedUpdate()
    {
        thisAnimal.lifeSpan--;
        if (thisAnimal.lifeSpan <= 0)
        {
            Debug.Log(gameObject.name + " has died");
            Destroy(gameObject);
        }
        else
            thisAnimal.refractoryPeriod--;
    }

    public void Diddle(GameObject _thisAnimal, GameObject _thatAnimal)
    {
        //why not in class? idk
        Animal thatAnimal = _thatAnimal.GetComponent<AnimalCore>().thisAnimal;
        //conceive
        Debug.Log("conception");
        // Debug.Log("this animal name: " + thisAnimal.animalName);
        // Debug.Log("that animal name: " + thatAnimal.animalName);


        Animal babyAnimal = Conceive(thisAnimal.genes, thatAnimal.genes);
        evolutionManager.numAnimals++;
        //spot
        Vector3 birthplace = Vector3.Lerp(gameObject.transform.position, _thatAnimal.transform.position, 0.5f);
        //birth
        Birth(birthplace, babyAnimal);
        thisAnimal.eggsInCarton--;
        thisAnimal.refractoryPeriod = thisAnimal.refractoryDefault;
        thatAnimal.eggsInCarton--;
        thatAnimal.refractoryPeriod = thatAnimal.refractoryDefault;

    }

    public Animal Conceive(DNA _pitcherDNA, DNA _catcherDNA) //confused about what should be in class and what should be in soul... soul is only individual things?
    {
        DNA babyDNA = new DNA();
        // Debug.Log("num animals in conceive: " + evolutionManager.numAnimals);
        // Debug.Log("pitcher : " + _pitcherDNA);
        // Debug.Log("catcher : " + _catcherDNA);

        return new Animal("animal: " + evolutionManager.numAnimals, babyDNA.Mutate(_pitcherDNA, _catcherDNA));
    }
    
    public void Birth(Vector3 _birthplace, Animal _babyAnimal) //pop the baby out
    {
        GameObject newAnimal = Instantiate(evolutionManager.animalPrefab, _birthplace, Quaternion.identity);
        newAnimal.transform.parent = evolutionManager.population.transform;
        newAnimal.name = _babyAnimal.animalName;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = _babyAnimal.moveSeed;
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = _babyAnimal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", _babyAnimal.genes.colorDNA);
    }
}
