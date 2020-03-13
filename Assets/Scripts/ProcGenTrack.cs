using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenTrack : MonoBehaviour
{
    public List<GameObject> trackTiles = new List<GameObject>();
    public List<GameObject> trackTemplate = new List<GameObject>();
    [SerializeField] GameObject ball;
    
    void Start()
    {
        StartCoroutine("CreateMap");
    }
    IEnumerator CreateMap()
    {
        int safetyCount = 0;
        while (trackTemplate.Count > 0 && safetyCount < 10)
        {
            for (int i = trackTemplate.Count - 1; i >= 0; i--) //starting with basic 3x3 = 9 tiles
            {
                if(trackTemplate[i] == null)
                {
                    //continue;
                }
                else if (trackTemplate[i].tag != "StartPlatform") //ignore center start platform
                {
                    //huge thanks to Jonas Tyroller and his youtube video "How to Randomly Generate Levels (and Islands)"
                    //make a list of all possible tiles
                    List<GameObject> tilePool = new List<GameObject>();
                    foreach (GameObject segment in trackTiles) //do I need to do this like my previous array issue????
                    {
                        GameObject segmentCopy = segment;
                        tilePool.Add(segment);
                    }
                    print("Placing Tile in Section " + i);
                    AttemptConnection(tilePool, i);
                }
                yield return new WaitForSeconds(0.01f);
            }
            safetyCount++;
        }
        
        //wait, don't need to do that if I just prevent movement until map is generated?
        // mapCompleted = true;
        GameObject.Find("GameEngine").GetComponent<PlatformMove>().enabled = true;
        yield return new WaitForSeconds(1f);
        //move the sphere to the start platform and give it gravity again
        ball.transform.position = GameObject.Find("StartPlatform").transform.position + new Vector3(0, 5f, 0);
        ball.GetComponent<Rigidbody>().useGravity = true;
    }
    void Update()
    {

    }
    private void AttemptConnection(List<GameObject> tiles, int num)
    {
        // spawn from list
        int index = Random.Range(0, tiles.Count);
        GameObject spawnedTile = tiles[index];
        GameObject newTile = Instantiate(spawnedTile, trackTemplate[num].transform.position, Quaternion.identity);
        // newTile.name = "Temporary Checker Tile" + num;
        print(newTile.name);
        // int orient = 0;
        // bool isValid = false;
        for (int c = 0; c < newTile.transform.childCount; c++)
        {
            //check each connector
            Transform bebo = newTile.transform.GetChild(c);
            // print(bebo.tag);
            if (bebo.tag == "validConnection")
            {
                //for each orientation, not including flipping upside down
                for (int o = 0; o < 4; o++)
                {
                    newTile.transform.RotateAround(newTile.transform.position, Vector3.up, 90f);
                    // print(newTile.transform.eulerAngles.y);
                    if (bebo.GetComponent<ConnectCheck>().CheckConnection())
                    {
                        //if it fits, replace the template tile and move on
                        Destroy(trackTemplate[num]);
                        newTile.name = "tile" + num;
                        newTile.transform.parent = gameObject.transform;
                        return;
                    }
                }
            }
        }
        //clear that tile from list and start over
        Destroy(newTile);
        tiles.Remove(tiles[index]);
        if (tiles.Count > 0)
        { //if there are any possible segments left, run it again
            AttemptConnection(tiles, num);
        }
        else
        {
            print("NO VALID SEGMENTS");

        }
    }
}
