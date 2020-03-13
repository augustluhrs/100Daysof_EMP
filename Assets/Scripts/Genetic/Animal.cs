using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class declarations for 
// ANIMAL and  DNA

public class Animal : MonoBehaviour
{
    //still don't understand this
    public EvolutionManager EvolutionManager;
    
    //parts of the Animal
    public string animalName; //need? or Object.name enough?
    public DNA genes;
    public int eggsInCarton;
    public int eggsDefault = 3;
    public float fertility;
    public float fertilityDefault = .5f;
    public float refractoryPeriod = 100f; //lowered for debugging 
    public float refractoryDefault = 500f; 
    public float lifeSpan;
    public float lifeSpanDefault = 3000f;

    public float moveSeed = 1f; //needs to be float? if whole number why not int?

    public Animal() //basic empty
    {
        animalName = "no animalName";
        genes = new DNA();
        eggsInCarton = eggsDefault;
        fertility = fertilityDefault;
        moveSeed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
        lifeSpan = lifeSpanDefault;
    }

    public Animal(string _animalName, Color _color) //basic spawn for now
    {
        animalName = _animalName; //eventually jr jr jr?
        genes = new DNA(_color);
        eggsInCarton = eggsDefault;
        fertility = fertilityDefault;
        moveSeed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
        lifeSpan = lifeSpanDefault;

    }

    public Animal(string _animalName, DNA _dna) //for babies
    {
        animalName = _animalName;
        genes = _dna;
        eggsInCarton = eggsDefault;
        fertility = fertilityDefault;
        moveSeed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
        lifeSpan = lifeSpanDefault;

    }
}

public class DNA
{
    //parts of the DNA [insides?]
    public Color colorDNA;

    public DNA() //random new DNA
    {
        colorDNA = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
    }

    public DNA(Color _color) //basic DNA init
    {
        colorDNA = _color;
    }

    public DNA Mutate(DNA papaDNA, DNA papa2DNA) //gene crossover?
    {
        // float mutationRate = GaussianRange.NextGaussian(.5f, 1f); //need to check these outputs
        // Debug.Log("mutation rate: " + mutationRate);
        // DNA babyDNA = new DNA(Color.Lerp(papaDNA.colorDNA, papa2DNA.colorDNA, mutationRate)); //mutation rate not best description, more of parent ratio... mutation rate can be added in addition
        // DNA babyDNA = new DNA(Color.Lerp(papaDNA.colorDNA, papa2DNA.colorDNA, 0.5f)); //should be exact middle;
        
        //new mutation rate and slight shift of parent ratio
        DNA babyDNA;
        float parentRatio = Random.Range(0.2f,.8f);
        Color baseColor = Color.Lerp(papaDNA.colorDNA, papa2DNA.colorDNA, parentRatio);
        float mutationRate = Random.Range(0f, .3f); //
        float mutationCheck = Random.Range(0f, 1f);
        Debug.Log("rate: " + mutationRate + " , check: " + mutationCheck);
        if(mutationCheck < mutationRate)
        {
            float mutationRed = Random.Range(-.2f, .2f); //arbitrary, test for now
            float mutationGreen = Random.Range(-.2f, .2f); //arbitrary, test for now
            float mutationBlue = Random.Range(-.2f, .2f); //arbitrary, test for now
            Color mutatedColor = new Color(baseColor.r * mutationRed, 
            baseColor.g * mutationGreen, baseColor.b * mutationBlue);
            babyDNA = new DNA(mutatedColor); 
        }
        else
        {
            babyDNA = new DNA(baseColor);
        }
        
        return babyDNA;
    }
}
