using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class declarations for 
// ANIMAL and  DNA
/*
public class Animal : MonoBehaviour
{
    //still don't understand this
    public EvolutionManager EvolutionManager;
    
    //parts of the Animal
    public string animalName; //need? or Object.name enough?
    public DNA genes;
    //below all in DNA now, to make heritable and mutable
    // public int eggsInCarton;
    // public int eggsDefault = 3;
    // public float fertility;
    // public float fertilityDefault = .5f;
    // public float refractoryPeriod = 100f; //lowered for debugging 
    // public float refractoryDefault = 500f; 
    // public float lifeSpan;
    // public float lifeSpanDefault = 3000f;

    // public float moveSeed = 1f; //needs to be float? if whole number why not int?
    
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
*/

public class DNA
{
    //original parts of the DNA
    public Color colorDNA;
    public float mutationRate;

    //from Animal class -- reproduction traits
    public float eggsInCarton;
    // public int eggsDefault = 3;
    public float fertility;
    // public float fertilityDefault = .5f;
    public float refractoryPeriod; //lowered for debugging 
    // public float refractoryDefault = 500f; 
    public float lifeSpan;
    // public float lifeSpanDefault = 3000f;

    //movement based variables
    public float moveSeed;
    
    //point ratio tracker
    public float[] pointArray;
     
    public DNA() //random new DNA, just for reference?
    {
        colorDNA = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f));
        mutationRate = 0.3f;
        eggsInCarton = 3;
        fertility = 0.5f;
        refractoryPeriod = 300f;
        lifeSpan = 3000f;
        moveSeed = 10f;
        // pointArray = [.5f, .5f];
    }

    // public DNA(Color _color) //basic DNA init
    // {
    //     colorDNA = _color;
    // }

    // public DNA(Color _color, float _mutationRate)
    // {
    //     colorDNA = _color;
    //     mutationRate = _mutationRate;
    // }

    //new fancy all-encompassing DNA
    public DNA(Color _color, float _mutationRate, Dictionary<string, float> _reproductionTraits, float _moveSeed, float[] _pointArray)
    {
        colorDNA = _color;
        mutationRate = _mutationRate;
        eggsInCarton = _reproductionTraits["eggsInCarton"];
        fertility = _reproductionTraits["fertility"];
        refractoryPeriod = _reproductionTraits["refractoryPeriod"];
        lifeSpan = _reproductionTraits["lifeSpan"];
        moveSeed = _moveSeed;
        pointArray = _pointArray;
    }

    public DNA Mutate(DNA papaDNA, DNA papa2DNA) //gene crossover?
    {

        //new complex inheritance and mutation based on specific genotypes(?):
        // color, mutation rate, reproduction, movement -- CRRM
        //individual parentRatios, mutationRates, etc.

        //Color
        float parentRatioColor = GaussianRange.GetGaussian_PointFiveMean();
        Color babyColor = Color.Lerp(papaDNA.colorDNA, papa2DNA.colorDNA, parentRatioColor);
        float mutationCheckColor = Random.Range(0f, 1f);

        //Mutation Rate
        float parentRatioRate = GaussianRange.GetGaussian_PointFiveMean();
        float babyMutationRate = Mathf.Lerp(papaDNA.mutationRate, papa2DNA.mutationRate, parentRatioRate);
        float mutationCheckRate = Random.Range(0f, 1f);

        //Reproduction Traits
        float parentRatioReproduction = GaussianRange.GetGaussian_PointFiveMean();
        float babyEggsInCarton = Mathf.Lerp(papaDNA.eggsInCarton, papa2DNA.eggsInCarton, parentRatioReproduction);
        float babyFertility = Mathf.Lerp(papaDNA.fertility, papa2DNA.fertility, parentRatioReproduction);
        float babyRefractoryPeriod = Mathf.Lerp(papaDNA.refractoryPeriod, papa2DNA.refractoryPeriod, parentRatioReproduction);
        float babyLifeSpan = Mathf.Lerp(papaDNA.lifeSpan, papa2DNA.lifeSpan, parentRatioReproduction);
        float mutationCheckReproduction = Random.Range(0f, 1f);

        //Movement
        float parentRatioMovement = GaussianRange.GetGaussian_PointFiveMean();
        float babyMoveSeed = Mathf.Lerp(papaDNA.moveSeed, papa2DNA.moveSeed, parentRatioMovement);
        float mutationCheckMovement = Random.Range(0f, 1f);

        //Points
        //just going to average points since "pure" ones start at one extreme [1, 0] / [0, 1]
        float[] babyPoints = new float[2];
        float player1total = papaDNA.pointArray[0] + papa2DNA.pointArray[0];
        float player2total = papaDNA.pointArray[1] + papa2DNA.pointArray[1];
        babyPoints[0] = player1total / 2;
        babyPoints[1] = player2total / 2;
        Debug.Log("player 1 ratio: " + babyPoints[0]);
        Debug.Log("player 2 ratio: " + babyPoints[1]);

        //overall mutation chance -- not sure why not make all indiv, but seems overkill?
        float mutationChance = Random.Range(0f, babyMutationRate);
    
        //now checking for individual mutation types -- CRRM
        if(mutationCheckColor < mutationChance)
        {
            // float mutationRed = GaussianRange.GetGaussian_ZeroMean(); //new gaussian dist.
            // float mutationGreen = GaussianRange.GetGaussian_ZeroMean(); //new gaussian dist.
            // float mutationBlue = GaussianRange.GetGaussian_ZeroMean(); //new gaussian dist.
            float mutationRed = GaussianRange.GetGaussian_SlightZero(); //now muted
            float mutationGreen = GaussianRange.GetGaussian_SlightZero(); //now muted
            float mutationBlue = GaussianRange.GetGaussian_SlightZero(); 
            
            babyColor = new Color(babyColor.r + mutationRed, babyColor.g + mutationGreen, babyColor.b + mutationBlue); //need to worry about values out of range?
        }
        if (mutationCheckRate < mutationChance)
        {
            // mutationRate += GaussianRange.GetGaussian_ZeroMean();
            babyMutationRate += GaussianRange.GetGaussian_SlightZero();
        }
        if (mutationCheckReproduction < mutationChance)
        {
            babyEggsInCarton += GaussianRange.GetGaussian_SlightZero();
            babyFertility += GaussianRange.GetGaussian_SlightZero();
            babyRefractoryPeriod += GaussianRange.GetGaussian_SlightZero();
            babyLifeSpan += GaussianRange.GetGaussian_SlightZero();

        }
        if (mutationCheckMovement < mutationChance)
        {
            babyMoveSeed += GaussianRange.GetGaussian_SlightZero();
        }

        //reassembling DNA and returning new babyDNA
        Dictionary<string, float> babyReproductionTraits = new Dictionary<string, float>();
        babyReproductionTraits.Add("eggsInCarton", babyEggsInCarton);
        babyReproductionTraits.Add("fertility", babyFertility);
        babyReproductionTraits.Add("refractoryPeriod", babyRefractoryPeriod);
        babyReproductionTraits.Add("lifeSpan", babyLifeSpan);
        Debug.Log("new baby:");
        Debug.Log("red: " + babyColor.r);
        Debug.Log("green: " + babyColor.g);
        Debug.Log("blue: " + babyColor.b);
        Debug.Log("eggs: " + babyEggsInCarton);
        Debug.Log("fertility: " + babyFertility);
        Debug.Log("refractory: " + babyRefractoryPeriod);
        Debug.Log("lifeSpan: " + babyLifeSpan);
        Debug.Log("moveseed: " + babyMoveSeed);

        DNA babyDNA = new DNA(babyColor, babyMutationRate, babyReproductionTraits, babyMoveSeed, babyPoints);
        return babyDNA;
    }
}
