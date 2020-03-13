using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class EvolutionManager : MonoBehaviour
{
    //socket stuff
    private SocketIOComponent socket;

    //global state variables
    public int numAnimals = 0; //not just counting population children because dont want name overlap
    public GameObject animalPrefab;
    public GameObject population;
    public GameObject world;
    
    void Start()
    {
        //socket stuff
        GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);

        socket.On("spawn", Spawn);
    }

    void Update()
    {
        
    }
    
    
    public void Spawn(SocketIOEvent e)
    {
        // Debug.Log("local Scale: " + world.transform.localScale.x + " , " + world.transform.localScale.z);
        float xScale = world.transform.localScale.x / e.data.GetField("width").f;
        float yScale = world.transform.localScale.z / e.data.GetField("height").f; //negative? or change origin?

        Vector3 thisSpot = new Vector3(e.data.GetField("x").f * xScale, .5f, e.data.GetField("y").f * -yScale);
        Color socketColor = new Color(e.data.GetField("r").f/255f, e.data.GetField("g").f/255f, e.data.GetField("b").f/255f);
        Animal animal = new Animal("animal: " + numAnimals, new DNA(socketColor));
        numAnimals++;
        //same as Birth() but immaculate
        GameObject newAnimal = Instantiate(animalPrefab, thisSpot, Quaternion.identity);
        newAnimal.transform.parent = population.transform;
        newAnimal.name = animal.animalName;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = animal.moveSeed;
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = animal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", animal.genes.colorDNA);
    }

    public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log(
			"#####################################################" +
			"THIS: " + e.data.GetField("this").str +
			"#####################################################"
		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
    
}
