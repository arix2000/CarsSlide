using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerstart : MonoBehaviour
{
    private float StartTime;
    private float ElapsedTime;
    private bool hasStartedLap = false;

    public GameObject lapText;

 
    void Update()
    {
        if (hasStartedLap)
        {
            ElapsedTime = Time.time - StartTime;

            lapText.GetComponent<Text>().text = "Lap Time: " + ElapsedTime.ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        hasStartedLap = true;

        StartTime = Time.time;
    }
}
