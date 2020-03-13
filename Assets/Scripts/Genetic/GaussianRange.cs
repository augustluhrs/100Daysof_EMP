using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//thanks Alan Zucconi! 
// https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
public static class GaussianRange
{
    public static float NextGaussian()
    {
        float v1, v2, s;
        do 
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1f; //can't do -1, 1?
            v2 = 2.0f * Random.Range(0f, 1f) - 1f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f); //so within unit circle and not origin?

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) /s); //radius/angle polar coordinate thing?
        return v1 * s;
    }

    public static float NextGaussian(float mean, float standard_deviation) //mean is .5 and sd is 1?
    {
        float output =  mean + NextGaussian() * standard_deviation;
        return Mathf.Clamp(output, 0f, 1f);
        
        // return mean + NextGaussian() * standard_deviation;
    }
}
