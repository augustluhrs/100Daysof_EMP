using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGaussian : MonoBehaviour
{
    [SerializeField] float mean = 0;
    [SerializeField] float stdDev = 0.3413f;
    [SerializeField] int frequency = 20;
    private int counter = 0;

    [SerializeField] GameObject prefab;
    [SerializeField] GameObject chart;
    [SerializeField] GameObject origin;
    private float xScale;
    private float chartCounter = 0;
    // public GaussianRange
    void Start()
    {
        xScale = chart.transform.localScale.x / 2;
        GameObject ori = Instantiate(origin, new Vector3(chart.transform.position.x, chart.transform.position.y, chart.transform.position.z), Quaternion.identity);
        // ori.transform.parent = chart.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        if (counter % frequency == 0)
        {
            chartCounter += 0.02f;
            float value = GetGaussian(mean, stdDev);
            GameObject newData = Instantiate(prefab,chart.transform.position + new Vector3(xScale * value, 1f, chartCounter), Quaternion.identity);
            // newData.transform.parent = chart.transform;
        }
    }

    public float GetGaussian(float _mean, float _stdDev)
    {
        // Debug.Log("first value: " + GaussianRange.NextGaussian());
        Debug.Log("mean: " + _mean + " , stdDev: " + _stdDev);
        float output = GaussianRange.NextGaussian(_mean, _stdDev);
        Debug.Log("output value: " + output);
        return output;
    }
}
