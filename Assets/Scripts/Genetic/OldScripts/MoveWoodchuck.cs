// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MoveWoodchuck : MonoBehaviour
// {
//     //scripts?
//     // public Woodchuck Woodchuck;
//     // public DNA DNA;
//     public EvolutionManager EvolutionManager;
//     List<Woodchuck> populationList;
//     // [SerializeField] GameObject engine;
    
//     private float moveCounter = 0;
//     private bool onEdge = false;
//     // private bool newMate = true;

//     private Woodchuck thisSheep;

//     public float moveSeed = 1f; //unique movement adjustment
//     void Start()
//     {
//         // populationList = GetComponent<EvolutionManager>().populationList;
//         // populationList = engine.GetComponent<EvolutionManager>().populationList;
//         // populationList = GameObject.Find("GameEngine").GetComponent<EvolutionManager>().populationList;
//         EvolutionManager = GameObject.Find("GameEngine").GetComponent<EvolutionManager>();
//         foreach(Woodchuck sheep in EvolutionManager.populationList) //just for eggs
//         {
//             if(sheep.name == gameObject.name)
//                 thisSheep = sheep;
//         }
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         //clunky move animation for now, trying to raycast the edge of the map
//         Ray ray = new Ray(transform.position + new Vector3(0f, 1f, 0f), (transform.forward - transform.up).normalized);
//         RaycastHit hit;
//         float maxDistance = 2f;
//         moveCounter++;
        
//         // Debug.Log("frame: " + moveCounter);
//         // Debug.Log(gameObject.name + ": " + (moveCounter%(10f*moveSeed)));
//         if (moveCounter % (10f*moveSeed) == 0)
//         {
//             if(!onEdge)
//                 transform.eulerAngles += new Vector3(0f, Random.Range(0f, 360f), 0f);
//             else
//             {
//                 onEdge = false;
//                 transform.eulerAngles += new Vector3(0f,180f, 0f);
//                 // newMate = true;
//             }
//         }
//         else if (moveCounter % moveSeed == 0)
//         {
//             if(Physics.Raycast(ray, out hit, maxDistance))
//                 {
//                     // Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
//                     // Debug.DrawLine(transform.position, hit.point, Color.green);
//                     if(hit.collider.gameObject.tag == "floor")
//                         transform.position += (transform.forward / 2);
//                     else //other sheep?
//                     {
//                         Debug.Log(gameObject.name + " hits other sheep: " + hit.collider.gameObject.name);
//                         Woodchuck thatSheep = new Woodchuck();
//                         foreach(Woodchuck sheep in EvolutionManager.populationList)
//                         {
//                             // if (sheep.name == gameObject.name)
//                             //     papaDNA = sheep.genes;
//                             if (sheep.name == hit.collider.gameObject.name)
//                                 thatSheep = sheep;
//                         }
//                         Debug.Log(thatSheep.eggsInCarton);
//                         if(thisSheep.eggsInCarton > 0 && thatSheep.eggsInCarton > 0) //only if both are fertile
//                         {
//                             //middle spot
//                             Vector3 loveSpot = Vector3.Lerp(gameObject.transform.position, hit.collider.gameObject.transform.position, 0.5f);
//                             //mutation
//                             // DNA babyDNA = Mutate(gameObject.genes, hit.collider.gameObject.genes);
//                             // List<Woodchuck> pop = EvolutionManager.populationList;
//                             // DNA papaDNA = new DNA();
//                             // DNA papa2DNA = new DNA();
//                             // DNA babyDNA = new DNA();

//                             DNA papaDNA = thisSheep.genes;
//                             DNA papa2DNA = thatSheep.genes;
//                             DNA babyDNA = new DNA();

//                             // // foreach(Woodchuck sheep in populationList)
//                             // foreach(Woodchuck sheep in EvolutionManager.populationList)
//                             // {
//                             //     if (sheep.name == gameObject.name)
//                             //         papaDNA = sheep.genes;
//                             //     if (sheep.name == hit.collider.gameObject.name)
//                             //         papa2DNA = sheep.genes;
//                             // }
//                             // DNA papaDNA = pop[pop.IndexOf(gameObject)].genes;
//                             // DNA papa2DNA = pop[pop.IndexOf(hit.collider.gameObject)].genes;
//                             babyDNA.Mutate(papaDNA, papa2DNA);
//                             // DNA babyDNA = Woodchuck.Mutate(gameObject.genes, hit.collider.gameObject.genes);
//                             //weird, need to eliminate redundancy
//                             EvolutionManager.Birth(loveSpot, babyDNA.colorDNA);
//                             // newMate = false;
//                             thisSheep.eggsInCarton--;
//                             thatSheep.eggsInCarton--;
//                         }
                        
//                     }
//                 }
//             else
//                 onEdge = true;
//                 // Debug.Log(gameObject.name);
//         }
//         // else
//             // Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
//     }
// }
