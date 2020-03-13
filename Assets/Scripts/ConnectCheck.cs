using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectCheck : MonoBehaviour
{

    [SerializeField] int rayDistance = 5;

    Vector3 ray = Vector3.zero;
    float len = 0;
    void Start()
    {
        
    }

    void Update(){
        // Debug.DrawRay(transform.position, ray, Color.red);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public bool CheckConnection()
    {
        //checking presence of other connector
        //just gonna send a check out in cardinal directions...
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
            // Physics.Raycast(transform.position, Vector3.back, out hit, rayDistance)||
            // Physics.Raycast(transform.position, Vector3.left, out hit, rayDistance)||
            // Physics.Raycast(transform.position, Vector3.right, out hit, rayDistance))
        {
            //wait so if it detects anything it's good... no need for fancy
            // Debug.DrawLine(transform.position, hit.point);
            ray = hit.point - transform.position;
            print("WE'VE GOT A HIT");
            print(hit.collider.gameObject.name);

            print("PARENT");    
            print(hit.collider.gameObject.transform.parent.name);
            return (true);
        }
        else
        {
            //print("no hit");
            return (false);
        }
    }
}
