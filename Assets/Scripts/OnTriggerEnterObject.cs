using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterObject : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "Upclimb")
        {
            AirFinController.instance.MoveToTheNextPart();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "ValveRotateTrigger")
        {
            AirFinController.instance.TurnOnInsSecondPart();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "TriggerEDS")
        {
            EmergencyDepressurizingController.instance.checkClosed();
        }

    }
}
