using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerPumpOperation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "jtc")
        {
            PumpOperationController.instance.jtc();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "suction")
        {
            PumpOperationController.instance.suction();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "suction 2")
        {
            PumpOperationController.instance.suction2();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "turbine")
        {
            PumpOperationController.instance.turbineCasing();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "drain")
        {
            PumpOperationController.instance.DrainCondensatefromLP();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "steam")
        {
            PumpOperationController.instance.steam();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "steam2")
        {
            PumpOperationController.instance.steam2();
        }else if (other.gameObject.tag == "Player" && gameObject.name == "stopcheck")
        {
            PumpOperationController.instance.stopCheck();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "dcs")
        {
            PumpOperationController.instance.dcs();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "final")
        {
            PumpOperationController.instance.final();
        }
        else if (other.gameObject.tag == "Player" && gameObject.name == "final2")
        {
            PumpOperationController.instance.final2();
        }
    }
}
