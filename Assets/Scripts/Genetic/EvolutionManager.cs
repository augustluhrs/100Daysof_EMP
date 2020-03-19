using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class EvolutionManager : MonoBehaviour
{
    //socket stuff
    private SocketIOComponent socket;

    //global state variables
    [Range(0f, 1f)][SerializeField] public float mutationDefault = 0.3f;
    [Range(0f, 2f)][SerializeField] public float populationControlModifier = 0.9f;


    public int numAnimals = 0; //not just counting population children because dont want name overlap
    public GameObject animalPrefab;
    public GameObject population;

    public GameObject world;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;

    //DNA defaults
    public Dictionary<string, float> defaultReproductionTraits = new Dictionary<string, float>();
    public float defaultMoveSeed;

    //score stuff
    public GameObject player1background;
    public GameObject player2background;
    public Text player1text;
    public Text player2text;
    public Text player1scoreText;
    public Text player2scoreText;
    public float player1score;
    public float player2score;
    public float[] defaultArray1 = new float[2];
    public float[] defaultArray2 = new float[2];


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
        socket.On("spawn2", Spawn2);


        //DNA defaults
        defaultReproductionTraits.Add("eggsInCarton", 3f);
        defaultReproductionTraits.Add("fertility", .5f);
        defaultReproductionTraits.Add("refractoryPeriod", 200f);
        defaultReproductionTraits.Add("lifeSpan", 3000f);
        defaultMoveSeed = 15f;

        defaultArray1[0] = 1f;
        defaultArray1[1] = 0f;
        defaultArray2[0] = 0f;
        defaultArray2[1] = 1f;
    }

    void Update()
    {
        player1scoreText.text = player1score.ToString();
        player2scoreText.text = player2score.ToString();
    }
    
    public void Spawn(SocketIOEvent e)
    {
        // Debug.Log("local Scale: " + world.transform.localScale.x + " , " + world.transform.localScale.z);
        // float xScale = world.transform.localScale.x / e.data.GetField("width").f;
        // float yScale = world.transform.localScale.z / e.data.GetField("height").f; //negative? or change origin?
        // Debug.Log("world 1" + xScale + " " + yScale);
        // Vector3 thisSpot = new Vector3(e.data.GetField("x").f * xScale, .5f, e.data.GetField("y").f * -yScale);
        
        //player color
        Color socketColor = new Color(e.data.GetField("r").f/255f, e.data.GetField("g").f/255f, e.data.GetField("b").f/255f);
        Color inverseColor = new Color((255 - e.data.GetField("r").f)/255f, (255 - e.data.GetField("g").f)/255f, (255 - e.data.GetField("b").f)/255f);
        player1text.material.SetColor("_Color", inverseColor);
        player1scoreText.material.SetColor("_Color", inverseColor);
        player1background.GetComponent<Image>().color = socketColor;
        
        //spawn info
        Vector3 thisSpot = spawnPoint1.transform.position;
        // Color socketColor = new Color(e.data.GetField("r").f/255f, e.data.GetField("g").f/255f, e.data.GetField("b").f/255f);
        DNA animal = new DNA(socketColor, mutationDefault, defaultReproductionTraits, defaultMoveSeed, defaultArray1);
        animal.moveSeed = e.data.GetField("speed").f;
        animal.fertility = e.data.GetField("fertility").f;
        animal.refractoryPeriod = e.data.GetField("reload").f;
        animal.lifeSpan = e.data.GetField("life").f;

        
        //same as Birth() but immaculate
        GameObject newAnimal = Instantiate(animalPrefab, thisSpot, Quaternion.identity);
        newAnimal.transform.eulerAngles += new Vector3(0f, Random.Range(0f, 360f), 0f);
        newAnimal.transform.parent = population.transform;
        newAnimal.name = "animal: " + numAnimals;
        numAnimals++;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = animal.moveSeed;
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = animal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", animal.colorDNA);
        player1score += 1f;

    }

    public void Spawn2(SocketIOEvent e)
    {
        // Debug.Log("local Scale: " + world.transform.localScale.x + " , " + world.transform.localScale.z);
        // float xScale = world2.transform.localScale.x / e.data.GetField("width").f;
        // float yScale = world2.transform.localScale.z / e.data.GetField("height").f; //negative? or change origin?
        // Debug.Log("world2" + xScale + " " + yScale);
        // Vector3 thisSpot = new Vector3((e.data.GetField("x").f * xScale) + 15f, .5f, e.data.GetField("y").f * -yScale);

        //player color
        Color socketColor = new Color(e.data.GetField("r").f/255f, e.data.GetField("g").f/255f, e.data.GetField("b").f/255f);
        Color inverseColor = new Color((255 - e.data.GetField("r").f)/255f, (255 - e.data.GetField("g").f)/255f, (255 - e.data.GetField("b").f)/255f);
        player2text.material.SetColor("_Color", inverseColor);
        player2scoreText.material.SetColor("_Color", inverseColor);
        player2background.GetComponent<Image>().color = socketColor;

        //spawn info
        Vector3 thisSpot = spawnPoint2.transform.position;
        // Color socketColor = new Color(e.data.GetField("r").f/255f, e.data.GetField("g").f/255f, e.data.GetField("b").f/255f);
        DNA animal = new DNA(socketColor, mutationDefault, defaultReproductionTraits, defaultMoveSeed, defaultArray2);
        GameObject newAnimal = Instantiate(animalPrefab, thisSpot, Quaternion.identity);
        newAnimal.transform.eulerAngles += new Vector3(0f, Random.Range(0f, 360f), 0f);
        newAnimal.transform.parent = population.transform;
        newAnimal.name = "animal: " + numAnimals;
        numAnimals++;
        newAnimal.tag = "animal";
        newAnimal.AddComponent<MoveAnimal>();
        newAnimal.GetComponent<MoveAnimal>().moveSeed = animal.moveSeed;
        newAnimal.AddComponent<AnimalCore>();
        newAnimal.GetComponent<AnimalCore>().thisAnimal = animal;
        newAnimal.GetComponent<Renderer>().material.SetColor("_Color", animal.colorDNA);
        player2score += 1f;
        
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
