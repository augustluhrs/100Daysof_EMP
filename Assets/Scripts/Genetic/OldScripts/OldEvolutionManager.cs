// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using SocketIO;

// public class OldEvolutionManager : MonoBehaviour
// {
//     private SocketIOComponent socket;
//     [SerializeField] GameObject woodchuck; //the creature prefab
//     // [SerializeField] int populationSize = 1; //initial pop count
//     [SerializeField] int generationLength; //how long before repop
//     // [SerializeField] List<GameObject> spawnPoints; //how to iterate through list?
//     // [SerializeField] GameObject[] spawnPoints; //locations for initial spawn -- later might need to raycast for spots?
//     // GameObject[] validSpawns = new GameObject[1]; //annoying, change to list?

//     public List<Woodchuck> populationList = new List<Woodchuck>();
//     // private int sheepNum = 0;

//     //scripts?
    

//     void Start()
//     {
//         //socket stuff
//         GameObject go = GameObject.Find("SocketIO");
// 		socket = go.GetComponent<SocketIOComponent>();

// 		socket.On("open", TestOpen);
// 		socket.On("boop", TestBoop);
// 		socket.On("error", TestError);
// 		socket.On("close", TestClose);

//         socket.On("spawn", Spawn);
        
//         // validSpawns = spawnPoints; //will i need to do this element by element?
//         // for(int i = 0; i < spawnPoints.Length; i++)
//         // {
//         //     validSpawns[i] = spawnPoints[i];
//         // }
//         // InitPopulation(populationSize); //removed b/c chaning population list
//         // Genesis(population);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //when spawning babies, should pop out up top and fall down
//     }
//     /*
//     private void InitPopulation(int popSize)
//     {
//         for(int i = 0; i < popSize; i++)
//         {
//             population.Add(new Woodchuck());
//         }
//         // Debug.Log(population);
//     }

//     private void Genesis(List<Woodchuck> pop)
//     {
//         // for (int i = 0; i < pop.Count; i++)
//         // {
            
//         // }
//         foreach(Woodchuck sheep in pop) //might be doing this backwards since going to need to add to pop
//         {
//             Vector3 popSpot = spawnPoints[0].transform.position + new Vector3(0, 1f, 0); //placeholder
//             Birth(sheepNum, popSpot);
//             // Debug.Log(pop.IndexOf(sheep));
//             // GameObject newChuck = Instantiate(woodchuck, spawnPoints[0].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
//             // newChuck.AddComponent<MoveWoodchuck>();
//             // float seed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
//             // newChuck.GetComponent<MoveWoodchuck>().moveSeed = seed;//semi-unique move seed?
//             // Debug.Log(seed);
//             // newChuck.name = "sheep " + pop.IndexOf(sheep);
//             // newChuck.GetComponentInChildren<MeshRenderer>().material.color = new Color(sheep.genes.red, sheep.genes.green, sheep.genes.blue);
//             // Debug.Log(sheep.genes.red);
            
//         }
//     }
//     */

//     // public void Birth(int num, Vector3 spot, float r, float g, float b)
//     public void Birth(Vector3 spot, float r, float g, float b)
//     {
//         //make new sheep gameobject
//         GameObject newChuck = Instantiate(woodchuck, spot, Quaternion.identity);
//         //give it a unique name;
//         // string name = "sheep " + num;
//         // newChuck.name = name;
//         // sheepNum++;
//         string name = "sheep " + populationList.Count;
//         newChuck.name = name;
//         //add the movement script and establish its movement factor
//         newChuck.AddComponent<MoveWoodchuck>();
//         float seed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
//         newChuck.GetComponent<MoveWoodchuck>().moveSeed = seed;//semi-unique move seed?
//         //give it DNA???? so confus
//         Woodchuck baby = new Woodchuck(name, r, g, b);
//         populationList.Add(baby);
//         //change the name of all the gameobjects in the sheep (need to make it one model)
//         // GameObject[] parts;
//         // parts = newChuck.Transform.child
//         foreach(Transform child in newChuck.transform)
//         {
//             child.name = name;
//             foreach(Transform grandchild in child)
//             {
//                 grandchild.name = name;
//             }
//         }
//         //change the color of the whole sheep
//         Renderer[] children;
//         children = newChuck.GetComponentsInChildren<Renderer>();
//         foreach(Renderer rend in children)
//         {
//             DNA thisDNA = baby.genes;
//             rend.material.SetColor("_Color", baby.genes.colorDNA);
//             // Debug.Log(rend.material.color);
//         }
//     }
//     public void Birth(Vector3 _spot, Color _color) //for color
//     {
//         //make new sheep gameobject
//         GameObject newChuck = Instantiate(woodchuck, _spot, Quaternion.identity);
//         //give it a unique name;
//         // string name = "sheep " + num;
//         // newChuck.name = name;
//         // sheepNum++;
//         string name = "sheep " + populationList.Count;
//         newChuck.name = name;
//         //add the movement script and establish its movement factor
//         newChuck.AddComponent<MoveWoodchuck>();
//         float seed = (float)Mathf.FloorToInt(UnityEngine.Random.Range(5f, 30f));
//         newChuck.GetComponent<MoveWoodchuck>().moveSeed = seed;//semi-unique move seed?
//         //give it DNA???? so confus
//         Woodchuck baby = new Woodchuck(name, _color);
//         populationList.Add(baby);
//         //change the color of the whole sheep
//         Renderer[] children;
//         children = newChuck.GetComponentsInChildren<Renderer>();
//         foreach(Renderer rend in children)
//         {
//             DNA thisDNA = baby.genes;
//             rend.material.SetColor("_Color", baby.genes.colorDNA);
//             // Debug.Log(rend.material.color);
//         }
//     }


//     public void Spawn(SocketIOEvent e)
//     {
//         // Debug.Log("new sheep " + sheepNum + " at: X" + e.data.GetField("x") + ", Y:" + e.data.GetField("y"));
//         Vector3 thisSpot = new Vector3(e.data.GetField("x").f, 1f, e.data.GetField("y").f);
//         Birth(thisSpot, e.data.GetField("r").f, e.data.GetField("g").f, e.data.GetField("b").f );
//     }

//     public void TestOpen(SocketIOEvent e)
// 	{
// 		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
// 	}
	
// 	public void TestBoop(SocketIOEvent e)
// 	{
// 		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

// 		if (e.data == null) { return; }

// 		Debug.Log(
// 			"#####################################################" +
// 			"THIS: " + e.data.GetField("this").str +
// 			"#####################################################"
// 		);
// 	}
	
// 	public void TestError(SocketIOEvent e)
// 	{
// 		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
// 	}
	
// 	public void TestClose(SocketIOEvent e)
// 	{	
// 		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
// 	}
// }
