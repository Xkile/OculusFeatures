using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterSequential : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "backInlet")
        {
            
            if (!SequentialController.instance.tasks[3])
            {
                SequentialController.instance.backwashInlet();
            }
            else
            {
                SequentialController.instance.backwashInletP2();
            }
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "backdrain")
        {
            
            if (!SequentialController.instance.tasks[3])
            {
                SequentialController.instance.openDrain();
            }
            else
            {
                SequentialController.instance.openDrainP2();
            }
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "saltwaterInlet")
        {
            
            if (!SequentialController.instance.tasks[3])
            {
                SequentialController.instance.saltwater();
            }
            else
            {
                SequentialController.instance.saltwaterP2();

            }
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "saltwateroutlet")
        {
            if (!SequentialController.instance.tasks[3])
            {
                SequentialController.instance.saltwaterout();
            }            
            else
            {
                SequentialController.instance.saltwateroutP2();
            }
        }
    }
}
