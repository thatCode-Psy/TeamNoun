using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepingLightScript : LightSourceScript
{

    public float sweepingAngle;

    public float sweepingTime;

    float startTime;
    
    float endTime;

    bool isSweepCounterClockwise; //Sweeps counterclockwise then clockwise.

    float originalStartAngle;
    float originalEndAngle;
    // Start is called before the first frame update
    void Start() {
        startTime = Time.time;
        endTime = Time.time + sweepingTime;
        isSweepCounterClockwise = true;

        originalStartAngle = startAngle;
        originalEndAngle = endAngle;
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        float currentTime = Time.time;
        float t;
        if(currentTime > endTime){
            isSweepCounterClockwise = !isSweepCounterClockwise;
            t = (currentTime - startTime)/(endTime - startTime) - 1;
            startTime = currentTime;
            endTime = currentTime + sweepingTime;
            
        }
        else{
            t = (currentTime - startTime)/(endTime - startTime);
        } 
        if(isSweepCounterClockwise){
            startAngle = Mathf.Lerp(originalStartAngle, originalStartAngle - sweepingAngle, t);
            endAngle = Mathf.Lerp(originalEndAngle, originalEndAngle - sweepingAngle, t);
        }
        else{
            startAngle = Mathf.Lerp(originalStartAngle - sweepingAngle, originalStartAngle, t);
            endAngle = Mathf.Lerp(originalEndAngle - sweepingAngle, originalEndAngle, t);
        }
        base.Update();
    }
}
