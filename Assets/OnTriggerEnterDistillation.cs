using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDistillation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "stair")
        {
            distillationController.instance.Stair();
        }else if (other.gameObject.tag == "Player" && gameObject.name == "stair2")
        {
            distillationController.instance.Stair2();
        }
    }
}
