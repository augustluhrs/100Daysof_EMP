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
        // float output =  mean + NextGaussian() * standard_deviation;
        // return Mathf.Clamp(output, 0f, 1f);
        float gauss = NextGaussian();
        Debug.Log("first value: "  + gauss);
        return mean + gauss * standard_deviation;
    }
    public static float GetGaussian_PointFiveMean()
    {
        return Mathf.Clamp((0.5f + NextGaussian() * 0.3413f), 0f, 1f);
    }
    //clamping just to be safe
    public static float GetGaussian_ZeroMean()
    {
        return Mathf.Clamp((NextGaussian() * 0.3413f), -1f, 1f);
    }

    public static float GetGaussian_SlightZero() //for a muted mutation
    {
        float baseGauss = Mathf.Clamp((NextGaussian() * 0.3413f), -1f, 1f);
        float mutedGauss = Mathf.Abs((baseGauss * GetGaussian_ZeroMean()));

        if (baseGauss < 0) //to keep primary polarity
        {
            mutedGauss *= -1;
        }
        
        return mutedGauss;
    }
}
