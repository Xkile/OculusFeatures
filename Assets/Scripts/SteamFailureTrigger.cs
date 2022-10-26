using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamFailureTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {



        
             if (other.tag == "Player" && gameObject.name == "Start")
        {
            SteamFailure.instance.StartModule();
            gameObject.SetActive(false);
            
        }
           else if (other.tag=="Player" && gameObject.name == "knobDrumLiquidpump")
        {
            SteamFailure.instance.OnTriggerEnterOnDrumLiquidpump();
            gameObject.SetActive(false);

        }else if(other.tag == "Player" && gameObject.name == "HFBottompump")
        {
            SteamFailure.instance.GotoAnotherModule();
            gameObject.SetActive(false);

        }
        else if (other.tag == "Player" && gameObject.name == "GotoTop")
        {
            SteamFailure.instance.OnTriggerToTopFloor();
            gameObject.SetActive(false);

        }
        else if (other.tag == "Player" && gameObject.name == "GotoTop_task5")
        {
            SteamFailure.instance.OnPlayerMoveToTaskFive();
            gameObject.SetActive(false);

        }
        
    }
}
