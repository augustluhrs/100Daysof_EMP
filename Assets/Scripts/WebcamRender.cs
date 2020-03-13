using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamRender : MonoBehaviour
{
    //thanks to user Max-Bot on unity forums
    public RawImage rawImage;
    void Start()
    {
        WebCamTexture webcamTexture = new WebCamTexture();
        // Renderer renderer = GetComponent<Renderer>();
        rawImage.texture = webcamTexture;
        rawImage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
