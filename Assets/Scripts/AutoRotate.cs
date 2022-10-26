using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AirFinController.instance.StartFanRotation)
        transform.Rotate(0,AirFinController.instance.speed,0);
        
    }
}
