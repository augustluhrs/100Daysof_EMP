using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    //huge thanks to Jonas Tyroller and his youtube video "How to Randomly Generate Levels (and Islands)"

    //game objects -- the maze holder, the ball, and the start platform
    [SerializeField] GameObject perplexus;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject start;
    //the possible segments to choose from (should I create categories later?)
    public List<GameObject> allSegments = new List<GameObject>();
    // script variables
    private int segmentCount = 0; //current number of segments
    private int segmentMax = 5; //max number of segments in the maze
    void Start()
    {
        StartCoroutine("CreateMap");
    }
    IEnumerator CreateMap()
    {
        //make the spawnStack with a fixed size equal to maximum number of segments
        GameObject[] spawnStack = new GameObject[segmentMax];
        while (segmentCount < segmentMax)
        { 
            //attempt connection at the spawnpoint
            //base rotation
            //for each connection (in order)
            
            //if connection, add it to the spawnStack and repeat
            
            //if no connection, rotate until fits old spawn, then check each connection again


            //if connection fails, eliminate the last spawned object and return to previous spawnPoint, starting at old connector
           
            
        }

        //- - - - then once reached end of maze
        //prevent phone movement until map is generated
        GameObject.Find("GameEngine").GetComponent<PlatformMove>().enabled = true;
        yield return new WaitForSeconds(1f);
        //move the sphere to the start platform and give it gravity again
        ball.transform.position = GameObject.Find("StartPlatform").transform.position + new Vector3(0, 5f, 0);
        ball.GetComponent<Rigidbody>().useGravity = true;
    }
    private void AttemptConnection(List<GameObject> segment, GameObject spawnPoint)
    { 
        //make a list of all possible segments
        //pick one
        //rotate it until a connector fits
        //if no connector fits, eliminate it from the list
        //repeat until it finds one, return it and add it to the stack
        

    }
}
