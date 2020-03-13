using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimal : MonoBehaviour
{
    //movement variables
    private float moveCounter = 0;
    public float moveSeed; //unique added later
    private bool onEdge = false;
    private float floorDistance = 2f;
    private float animalDistance = 1f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Ray floorRay = new Ray(transform.position + transform.forward / 2, (transform.forward - transform.up).normalized);
        RaycastHit floorHit;
        moveCounter ++;
        Debug.DrawRay(floorRay.origin, floorRay.direction, Color.blue);
        Ray animalRay = new Ray (transform.position, transform.forward);
        RaycastHit animalHit;
        Debug.DrawRay(animalRay.origin, animalRay.direction, Color.red);
                        
        if (moveCounter % (10f*moveSeed) == 0)
        {
            if(!onEdge)
                transform.eulerAngles += new Vector3(0f, Random.Range(0f, 360f), 0f);
            else
            {
                onEdge = false;
                transform.eulerAngles += new Vector3(0f, Random.Range(160f, 200f), 0f); //fix getting stuck in corner
            }
        }
        else if (moveCounter % moveSeed == 0)
        {
            if (Physics.Raycast(floorRay, out floorHit, floorDistance)) //if floor ray hits something
                {
                    // if (floorHit.collider.gameObject.tag == "world") //need?
                    if (gameObject.GetComponent<AnimalCore>().thisAnimal.eggsInCarton > 0 &&
                        gameObject.GetComponent<AnimalCore>().thisAnimal.refractoryPeriod <= 0)
                    {
                        if (Physics.Raycast(animalRay, out animalHit, animalDistance))
                        {
                            Debug.Log(gameObject.name + " hit something: " + animalHit.collider.name);
                            if(animalHit.collider.gameObject.tag == "animal")
                            {
                                GameObject otherAnimal = animalHit.collider.gameObject;
                                Debug.Log("Their eggs: " + otherAnimal.GetComponent<AnimalCore>().thisAnimal.eggsInCarton);
                                Debug.Log("Their refractory: " + otherAnimal.GetComponent<AnimalCore>().thisAnimal.refractoryPeriod);

                                if (otherAnimal.GetComponent<AnimalCore>().thisAnimal.eggsInCarton > 0 && 
                                    otherAnimal.GetComponent<AnimalCore>().thisAnimal.refractoryPeriod <= 0) //if they have eggs left and are ready to go
                                    gameObject.GetComponent<AnimalCore>().Diddle(gameObject, otherAnimal);
                            }
                        }
                        else //no edge and no other animal, move
                            transform.position += (transform.forward / 2);
                    }
                    else //no edge and no eggs left, move
                        transform.position += (transform.forward / 2);
                }
            else //no hit detected
                onEdge = true;
        }
    }

}
