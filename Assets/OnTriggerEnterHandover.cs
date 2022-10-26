using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterHandover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "suction")
        {
            HandoverTakeoverController.instance.suction();
        }
    }
}
