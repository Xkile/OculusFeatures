using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDepressurizingController : MonoBehaviour
{
    public GameObject[] AllInstructions;

    public bool switchHS5301;
    public bool switchHS5305;
    public bool checkSwitches;

    public GameObject[] RayInteractionElements;

    public GameObject Arrow;

    public GameObject Trigger;

    public GameObject button1;
    public GameObject button2;
    public GameObject button1Interactable;
    public GameObject button2Interactable;

    public static EmergencyDepressurizingController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switchHS5301 = true;
        switchHS5305 = true;
        
    }
    private void Update()
    {
        if (!switchHS5301 && !switchHS5305 && checkSwitches)
        {
            AllInstructions[2].SetActive(false);
            //AllInstructions[3].SetActive(true);
            Arrow.SetActive(true);
            Trigger.SetActive(true);
            checkSwitches = false;
        }
    }

    public void switch1off()
    {
        switchHS5301 = false;
        button1.transform.localPosition = new Vector3(button1.transform.localPosition.x, button1.transform.localPosition.y, 0.0371f);
        StartCoroutine(HoldTime(button1Interactable, false));
        


    }

    public void switch2off()
    {
        switchHS5305 = false;
        button2.transform.localPosition = new Vector3(button2.transform.localPosition.x, button2.transform.localPosition.y, 0.0371f);
        StartCoroutine(HoldTime(button2Interactable, false));
        
    }

    public void confirmFromDCS()
    {
        AllInstructions[0].SetActive(false);
        AllInstructions[1].SetActive(true);
    }

    public void takeClearance()
    {
        AllInstructions[1].SetActive(false);
        AllInstructions[2].SetActive(true);
        button1Interactable.GetComponent<PokeInteractable>().enabled = true;
        button2Interactable.GetComponent<PokeInteractable>().enabled = true;
        RayOnOff(false);
        checkSwitches = true;
    }

    public void checkClosed()
    {
        AllInstructions[3].SetActive(true);
        Arrow.SetActive(false);
        Trigger.SetActive(false);
        //AllInstructions[4].SetActive(true);
        RayOnOff(true);
    }

    void RayOnOff(bool Active)
    {
        foreach (GameObject i in RayInteractionElements)
        {
            i.SetActive(Active);
        }
    }

    IEnumerator HoldTime(GameObject obj, bool active)
    {
        yield return new WaitForSeconds(1);
        obj.GetComponent<PokeInteractable>().enabled = active;
    }


}
