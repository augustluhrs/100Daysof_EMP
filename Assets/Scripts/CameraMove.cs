using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // [SerializeField] GameObject ball;
    [SerializeField] GameObject panel;

    private Quaternion cam;
    private float rotation;
    void Start()
    {
        cam = gameObject.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        // cam.eulerAngles += new Vector3(0, 1f, 0);
         // Rotate the cube by converting the angles into a quaternion.
        if (panel.activeSelf == true){
            rotation += .05f;
            
        }
        else
        {
            rotation *= 1.2f;
        }
        Quaternion target = Quaternion.Euler(0, rotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, .5f);
        // Debug.Log(rotation);
        //old ball camera script
        /*
        transform.LookAt(ball.transform.position);
        gameObject.transform.position = ball.transform.position + new Vector3(-20f, 20f, -40f);
        */
    }
}
