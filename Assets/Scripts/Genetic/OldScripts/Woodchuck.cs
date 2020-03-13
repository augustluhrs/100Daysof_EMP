// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Woodchuck
// {
//     public string name;
//     public DNA genes;
//     public int eggsInCarton; //amount of times it can reproduce before it dies

//     //scripts
//     // public GaussianRange GaussianRange;

//     public Woodchuck()
//     {
//         name = "no name";
//         genes = new DNA();
//         eggsInCarton = 1;
//     }

//     public Woodchuck(string _name, float _r, float _g, float _b)
//     {
//         name = _name;
//         genes = new DNA(_r, _g, _b);
//         eggsInCarton = 1;
//     }
//     public Woodchuck(string _name, Color _color)
//     {
//         name = _name;
//         genes = new DNA(_color);
//         eggsInCarton = 1;
//     }

//     // public Woodchuck(string _name, DNA oldDNA)
//     // {
//     //     name = _name;
//     //     genes = oldDNA.Mutate(50f); //arbitrary mutation rate for now 
//     //     eggsInCarton = 4;
//     // }

// }

// // public class DNA
// {
//     //components: speed, direction, pop rate, color, size
//     // public float speed;
//     // public float red;
//     // public float green;
//     // public float blue;
//     public Color colorDNA;

//     public DNA()
//     {
//         // red = Mathf.FloorToInt(Random.Range(0f, 256f));
//         // green = Mathf.FloorToInt(Random.Range(0f, 256f));
//         // blue = Mathf.FloorToInt(Random.Range(0f, 256f));
//         colorDNA = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
//     }

//     public DNA(float _r, float _g, float _b)
//     {
//         // red = _r;
//         // green = _g;
//         // blue = _b;
//         colorDNA = new Color(_r/255f, _g/255f, _b/255f);
//     }

//     public DNA(Color _color)
//     {
//         colorDNA = _color;
//     }
    
//     /*
//     public DNA Mutate(float mutationRate) //if asexual
//     {
//         //random mutation
//         float redMutation = Random.Range(-mutationRate, mutationRate);
//         float greenMutation = Random.Range(-mutationRate, mutationRate);
//         float blueMutation = Random.Range(-mutationRate, mutationRate);

//         //make new DNA by mutating old DNA.
//         DNA newDNA = new DNA();
//         newDNA.red = Mathf.FloorToInt(Mathf.Clamp(red + redMutation, 0f, 255f));
//         newDNA.green = Mathf.FloorToInt(Mathf.Clamp(green + greenMutation, 0f, 255f));
//         newDNA.blue = Mathf.FloorToInt(Mathf.Clamp(blue + blueMutation, 0f, 255f));

//         return newDNA;
//     }
//     */
//     public DNA Mutate(DNA papaDNA, DNA papa2DNA) //if paired
//     {
//         // DNA newDNA = otherDNA;
//         //need to implement crossover
//         //THERES A COLOR LERP
//         // float mutationRate = Random.Range() //need Gaussian
//         float mutationRate = GaussianRange.NextGaussian(.5f, 1f); //wow hope this works;
//         Debug.Log("mutation rate: " + mutationRate);
//         DNA babyDNA = new DNA(Color.Lerp(papaDNA.colorDNA, papa2DNA.colorDNA, mutationRate));
//         return babyDNA;
//         //returning just color for now

//     }
// }