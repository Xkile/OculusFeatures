using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine.UI;

public class AirFinController : MonoBehaviour
{
    public GameObject [] AllInstructions;
    public GameObject StartButton;
    public GameObject IncreaseSpeedButton;
    public GameObject DecreaseSpeedButton;

    public GameObject Knob;

    public GameObject[] Valves;

    public bool [] valveOpen;

    public bool StartFanRotation;
    public bool knobOnLocalMode;
    public bool knobOnRemoteMode;
    
    public bool part2Complete;
    public bool part22ndActComplete;

    public bool secondTimeRemote;
    public bool remoteModebool;

    public GameObject [] Arrows;
    public GameObject[] Triggers;

    public GameObject buttonVisStart;
    public GameObject buttonVisIncrease;
   

    public static AirFinController instance;

    public GameObject Player;

    public float speed;

    public Text [] RotValue;
    public Text [] Display;    

    int ValueCount;

    public GameObject[] RayInteractionElements;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartButton.GetComponent<PokeInteractable>().enabled = false;
        IncreaseSpeedButton.GetComponent<PokeInteractable>().enabled = false;
        DecreaseSpeedButton.GetComponent<PokeInteractable>().enabled = false;

        RayOnOff(false);

        knobOnLocalMode = true;
        remoteModebool = true;
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[5], normalMat[2]);       
    }

    // Update is called once per frame
    void Update()
    {    
        if (Knob.transform.localRotation.x < -0.35 && knobOnLocalMode == true)
        {
            print("knob in local mode");
            knobOnLocalMode = false;
            OnKnobLocalMode();
        }
        if (Knob.transform.localRotation.x > 0.34 && knobOnRemoteMode == true)
        {         
            OnKnobRemoteMode();     
            knobOnRemoteMode = false;       
        }

       
        if (valveOpen[0] && valveOpen[1] && !part2Complete)
        {
            AllInstructions[8].SetActive(true);
            RayOnOff(true);
            part2Complete = true;
        }

        if(valveOpen[2] && !part22ndActComplete)
        {
            AllInstructions[9].SetActive(true);
            RayOnOff(true);
            part22ndActComplete = true;
        }
    }

    void RayOnOff(bool Active)
    {
        foreach (GameObject i in RayInteractionElements)
        {
            i.SetActive(Active);
        }
    }

    public void updateValve(int n)
    {

        ValueCount = (int)(Valves[n].transform.localEulerAngles.z);

        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 99 && ValueCount >= 0)
        {
            RotValue[n].text = ValueCount.ToString();
            Display[n].text = "Closed";
        }
        else
        {
           RotValue[n].text = "0";
           Display[n].text = "Closed";
        }
    }

    IEnumerator HoldTime(GameObject obj, bool active)
    {
        yield return new WaitForSeconds(1);
        obj.GetComponent<PokeInteractable>().enabled = active;
    }

   public void OnKnobLocalMode()
    {
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[3], normalMat[3]);
        AllInstructions[0].SetActive(false);       
        StartCoroutine(HoldTime(StartButton, true));
        AllInstructions[1].SetActive(true);
    }

   public void OnFanStart()
    {
        AllInstructions[1].SetActive(false);
        StartFanRotation = true;
        speed = 5;
        AllInstructions[2].SetActive(true);
        StartCoroutine(HoldTime(StartButton, false));
        //0.0371f
        buttonVisStart.transform.localPosition = new Vector3(buttonVisStart.transform.localPosition.x, buttonVisStart.transform.localPosition.y, 0.07f);
        RayOnOff(true);
    }

   public void CheckFanRotation()
    {
        AllInstructions[2].SetActive(false);
        AllInstructions[3].SetActive(true);
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[4], normalMat[3]);
        StartCoroutine(HoldTime(IncreaseSpeedButton, true));
        RayOnOff(false);
    }

    public void RaiseFanSpeed()
    {
        AllInstructions[3].SetActive(false);
        speed = 10;
        StartCoroutine(HoldTime(IncreaseSpeedButton, false));
        buttonVisIncrease.transform.localPosition = new Vector3(buttonVisIncrease.transform.localPosition.x, buttonVisIncrease.transform.localPosition.y, 0.07f);
        AllInstructions[4].SetActive(true);
        RayOnOff(true);
    }

   public void OnRPMCheck()
    {
        AllInstructions[4].SetActive(false);
        AllInstructions[5].SetActive(true);
        ObjectsToHighlight[5].material = normalMat[2];
        HighlightObjects[0].SetActive(true);
        ChangeMater(ObjectsToHighlight[5], normalMat[2]);

        if (remoteModebool)
        {
            knobOnRemoteMode = true;
            remoteModebool = false;
        }
        else
        {
            knobOnRemoteMode = true;
            secondTimeRemote = true;
        }
        RayOnOff(false);
    }

    void OnKnobRemoteMode()
    {
        
        if (secondTimeRemote)
        {
            AllInstructions[9].SetActive(true);
            StartFanRotation = false;
        }
        else
        {
            AllInstructions[5].SetActive(false);
           // AllInstructions[6].SetActive(true);
            StartFanRotation = false;
            Arrows[0].SetActive(true);
            Triggers[0].SetActive(true);
            AllInstructions[10].SetActive(true);
        }
    }

    public void MoveToTheNextPart()
    {
        Triggers[0].SetActive(false);
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(true);
        FadeIN();
        //AllInstructions[6].SetActive(false);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        //Player.transform.localEulerAngles = new Vector3(0, 0, 0);
        Player.transform.position = new Vector3(-5.63f, 0.369f, 10.12f);
        //Player.transform.localEulerAngles = new Vector3(0, 135, 0);
        StartCoroutine(ControllerOn());
        Triggers[1].SetActive(true);
        Triggers[2].SetActive(true);
    }

    public void TurnOnInsSecondPart()
    {
        AllInstructions[10].SetActive(false);
        AllInstructions[7].SetActive(true);
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[0],normalMat[0]);
        ChangeMater(ObjectsToHighlight[1],normalMat[0]);
        Triggers[1].SetActive(false);
        Triggers[2].SetActive(false);
        Arrows[1].SetActive(false);
    }



    IEnumerator ControllerOn()
    {
        yield return new WaitForSeconds(2);
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<OVRPlayerController>().enabled = true;
    }


    public void MoveToSteamValve()
    {
        FadeIN();
        RayOnOff(false);
        AllInstructions[8].SetActive(false);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        ChangeMater(ObjectsToHighlight[2], normalMat[1]);
        Player.transform.position = new Vector3(-5.587f, 5.813f , 15.872f);
        //Player.transform.localEulerAngles = new Vector3(0, 0, 0);
        //StopAllCoroutines();
        StartCoroutine(ControllerOn());
    }

    public void MoveToTheSwitch()
    {
        FadeIN();
        StopAllCoroutines();
        foreach (GameObject i in HighlightObjects)
        {
            i.SetActive(true);
        }
        ObjectsToHighlight[3].material = normalMat[3];
        ObjectsToHighlight[4].material = normalMat[3];
        ObjectsToHighlight[5].material = normalMat[2];



        buttonVisIncrease.transform.localPosition = new Vector3(buttonVisIncrease.transform.localPosition.x, buttonVisIncrease.transform.localPosition.y, 0.0371f);
        buttonVisStart.transform.localPosition = new Vector3(buttonVisStart.transform.localPosition.x, buttonVisStart.transform.localPosition.y, 0.0371f);
        RayOnOff(false);
        AllInstructions[9].SetActive(false);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        //Player.transform.localEulerAngles = new Vector3(0, 0, 0);
        Player.transform.position = new Vector3(-0.55f,-1.088f ,-1.606f);
        //Player.transform.localEulerAngles = new Vector3(0, 0, 0);
        AllInstructions[0].SetActive(true);
        knobOnLocalMode = true;
        StartCoroutine(ControllerOn());
        ChangeMater(ObjectsToHighlight[5], normalMat[2]);
    }

    public Material highlight;
    public Material [] normalMat;
    public Renderer [] ObjectsToHighlight;
    public GameObject [] HighlightObjects;

     public void ChangeMater(Renderer obj, Material norm)
    {
        StartCoroutine(ChangeMaterial(obj,norm));       
    }

    IEnumerator ChangeMaterial(Renderer obj, Material norm)
    {
        obj.material = highlight;
        yield return new WaitForSeconds(.5f);
        obj.material = norm;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ChangeMaterial(obj,norm));
    }

    public OVRScreenFade fadeInOut;

    public void FadeIN()
    {
        fadeInOut.FadeIn();
    }

}
