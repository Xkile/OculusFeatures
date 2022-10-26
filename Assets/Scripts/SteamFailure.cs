using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamFailure : MonoBehaviour
{
    [Tooltip("Frist Part")]

    public static SteamFailure instance;
    public GameObject[] AllInst;
    public Transform StopButton;
    public GameObject stopMainObject;
    public Transform startButton;
    public GameObject startMainObject;
    public GameObject Knob;
    public bool knobOnLocalMode;
    public GameObject highLightFristObject;

    [Tooltip("knobDrumLiquidpump Part")]
    public GameObject knobDrumLiquidpump;
    public GameObject HighLight;

    [Tooltip("HF Bottom pump Part")]
    public GameObject knobHFBottompump;
    public GameObject HFBottompumpHighLight;


    [Tooltip("All Trigger Enter Object")]
    public GameObject[] allTriggerEnterObject;


    [Tooltip("Goto Top floor")]
    public GameObject valveRotate;
    public GameObject GotoTopHighLight;


    public Text[] RotValue;
    public Text[] Display;

    public GameObject[] Valves;
 
    public Transform Player;
    public OVRScreenFade screenFade;

    int ValueCount;

    public bool taskOne = false;
    public bool taskTwo = false;
    public bool taskThree = false;
    public bool taskFour = false;

    public GameObject nextButtonOfTskOne;
    public GameObject nextButtonOfTskTwo;

    public GameObject taskFiveHighLight;

    public GameObject oilFuelOpen;

    public bool taskFourbool1, taskFourbool2;

    public Renderer Object1;
    public Renderer Object2;
    public Renderer Object3;
    public Renderer Object4;
    public Renderer Object5;

    public Renderer Object6;
    public Renderer Object7;
    public Renderer Object8;
    public Renderer Object9;
    public Renderer Object10;



    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        startMainObject.GetComponent<PokeInteractable>().enabled = false;
        Knob.GetComponent<BoxCollider>().enabled = false;
        knobDrumLiquidpump.GetComponent<BoxCollider>().enabled = false;
        knobHFBottompump.GetComponent<BoxCollider>().enabled = false;

    }


    public void AllInstTrueFalse(int instIndex)
    {
        foreach(GameObject obje in AllInst)
        {
            obje.SetActive(false);
        }
        AllInst[instIndex].SetActive(true);
    }
    public void AllInstFalse()
    {
        foreach (GameObject obje in AllInst)
        {
            obje.SetActive(false);
        }
     }
    // Update is called once per frame
   public void KnobOnLocalMode()
    {
        if (Knob.transform.localRotation.x < -0.38 && knobOnLocalMode == true)
        {
            print("knob in local mode");
            knobOnLocalMode = false;
            OnKnobLocalMode();
        }
    }

    public void KnobOnclosemode()
    {

        print("knob in local mode"+ knobDrumLiquidpump.transform.localRotation.x);
        if (knobDrumLiquidpump.transform.localRotation.x < -0.29 )
        {
            print("knob in local mode");
            //  OnKnobLocalMode();
            CompleteDrummodule();
        }
    }

    public void OpenOilFuel()
    {

        print("knob in local mode" + oilFuelOpen.transform.localRotation.x);
        if (oilFuelOpen.transform.localRotation.x < -0.29)
        {
            print("knob in local mode");
            taskFourbool1 = true;
            completeTaskFour();

        }
        else
        {
            taskFourbool1=  false;
        }
    }


    public void completeTaskFour()
    {
        if(taskFourbool1 && taskFourbool2)
        {
            AllInstTrueFalse(8);
            oilFuelOpen.GetComponent<BoxCollider>().enabled = false;
            taskFourbool1 = false;
            taskFourbool2 = false;
            taskThree = false;
            RayOnOff(true);

        }
    }

    public void KnobOnHFBottompump()
    {

        print("knob in local mode" + knobHFBottompump.transform.localRotation.x);
        if (knobHFBottompump.transform.localRotation.x < 0.29)
        {
            print("knob in local mode");
            //  OnKnobLocalMode();
            OnCompleteHFBottompump();
        }
    }

    public void OnCompleteHFBottompump()
    {
        AllInstFalse();
        knobHFBottompump.GetComponent<BoxCollider>().enabled = false;
        GotoTopHighLight.SetActive(true);
        allTriggerEnterObject[2].SetActive(true);
        RayOnOff(true);

    }



    public void CompleteDrummodule()
    {
        knobDrumLiquidpump.GetComponent<BoxCollider>().enabled = false;
        AllInstFalse();
        HFBottompumpHighLight.SetActive(true);
        allTriggerEnterObject[1].SetActive(true);
        StopAllCoroutines();
        RayOnOff(true);


    }

    public void OnReleaseButton()
    {
         StopButton.transform.localPosition = new Vector3(StopButton.transform.localPosition.x, StopButton.transform.localPosition.y, 0.118f);
         StartCoroutine(HoldTime(stopMainObject,false));
         AllInstTrueFalse(1);
         Knob.GetComponent<BoxCollider>().enabled = true;
         knobOnLocalMode = true;
         StopAllCoroutines();
         HighLightObject(Object2);

    }
    public void PressStartButton()
    {
          AllInstFalse();
         startButton.transform.localPosition = new Vector3(startButton.transform.localPosition.x, startButton.transform.localPosition.y, 0.07f);
         StartCoroutine(HoldTime(startMainObject, false));
         HighLight.SetActive(true);
         allTriggerEnterObject[0].SetActive(true);
        RayOnOff(true);

    }


    IEnumerator HoldTime(GameObject obj, bool active)
    {
        yield return new WaitForSeconds(1);
        obj.GetComponent<PokeInteractable>().enabled = active;
    }

    public void OnKnobLocalMode()
    {
        Knob.GetComponent<BoxCollider>().enabled = false;
        AllInstTrueFalse(2);
        startMainObject.GetComponent<PokeInteractable>().enabled = true;
        StopAllCoroutines();
        HighLightObject(Object3);
    }

    public void OnTriggerEnterOnDrumLiquidpump()
    {
        knobDrumLiquidpump.GetComponent<BoxCollider>().enabled = true;
        HighLight.SetActive(false);
        AllInstTrueFalse(3);
        RayOnOff(false);
        HighLightObject(Object4);

    }

    public void GotoAnotherModule()
    {
        knobHFBottompump.GetComponent<BoxCollider>().enabled = true;
        HFBottompumpHighLight.SetActive(false);
        AllInstTrueFalse(4);
        RayOnOff(false);
        HighLightObject(Object5);

    }


    public void StartModule()
    {
        AllInstTrueFalse(0);
        highLightFristObject.SetActive(false);
        HighLightObject(Object1);
        RayOnOff(false);
    }



    public void OnTriggerToTopFloor()
    {
        GotoTopHighLight.SetActive(false);
        screenFade.FadeIn();
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
         Player.transform.position = new Vector3(-34.06f, 11.88f, 64.09f);
         StartCoroutine(ControllerOn());
        AllInstTrueFalse(5);
        taskOne = true;
        RayOnOff(false);
        HighLightObject(Object6);


    }

    public void OnPlayerMoveToTaskFive()
    {
        taskFiveHighLight.SetActive(false);
        screenFade.FadeIn();
        StopAllCoroutines();
        HighLightObject(Object8);
        HighLightObject(Object9);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(2.85f, 4.769515f, 40.5f);
        StartCoroutine(ControllerOn());
        AllInstTrueFalse(7);
        RayOnOff(false);
    }


    IEnumerator ControllerOn()
    {
        yield return new WaitForSeconds(1);
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<OVRPlayerController>().enabled = true;
    }

    public void updateValve(int n)
    {
        if(taskOne)
        ValueCount = (int)(Valves[n].transform.eulerAngles.x *10/8);
        else if(taskTwo)
            ValueCount = (int)(Valves[n].transform.eulerAngles.y * 10 / 8);
        else if(taskThree)
            ValueCount = (int)(Valves[n].transform.eulerAngles.z * 10 / 8);
        else if(taskFour)
            ValueCount = (int)(Valves[n].transform.eulerAngles.y * 10 / 8);

        Debug.Log("Angle"+ Valves[n].transform.localEulerAngles);
        if (ValueCount > 98)
        {

            Display[n].text = "Close";
            RotValue[n].text = "100";
            if (taskOne)
            {
                nextButtonOfTskOne.SetActive(true);
                RayOnOff(true);

            }
            else if (taskTwo)
            {
                nextButtonOfTskTwo.SetActive(true);
                RayOnOff(true);

            }
            else if (taskThree)
            {
                taskFourbool2 = true;
                completeTaskFour();
            }
            else if (taskFour)
            {

            }
        }
        else if (ValueCount <= 99 && ValueCount >= 0)
        {
            RotValue[n].text = ValueCount.ToString();
            Display[n].text = "Open";
            taskFourbool2 = false;


        }
        else
        {
            RotValue[n].text = "0";
            Display[n].text = "Open";
            taskFourbool2 = false;
        }
    }



    public void OnCompleteTaskOne()
    {
        taskOne = false;

        screenFade.FadeIn();
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-40.73f, 6.26f, 62.47f);
        StartCoroutine(ControllerOn());
        AllInstTrueFalse(6);
        taskTwo = true;
        HighLightObject(Object7);
        RayOnOff(false);
    }

    public void OnCompleteTskTwo()
    {
        taskTwo = false;
        screenFade.FadeIn();
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-36.55f, 1.61f, 71f);
        StartCoroutine(ControllerOn());
        AllInstFalse();
        taskThree = true;
        taskFiveHighLight.SetActive(true);
        allTriggerEnterObject[3].SetActive(true);
        RayOnOff(true);

    }


    public void OnCompleteTaskFour()
    {
        screenFade.FadeIn();
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(28.022f, 6.124918f, -32.244f);
        StartCoroutine(ControllerOn());
        AllInstTrueFalse(9);
        taskFour = true;
        RayOnOff(false);
        HighLightObject(Object10);

    }


    public void HighLightObject(Renderer obj)
    {
         Material mat = obj.GetComponent<Renderer>().material;
         StartCoroutine(ChangeMaterial(obj, mat));
    }

    public Material highlight;
    IEnumerator ChangeMaterial(Renderer obj, Material norm)
    {
        obj.material = highlight;
        yield return new WaitForSeconds(.5f);
        obj.material = norm;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ChangeMaterial(obj, norm));
    }

    public GameObject[] RayInteractionElements;

    void RayOnOff(bool Active)
    {
        foreach (GameObject i in RayInteractionElements)
        {
            i.SetActive(Active);
        }
    }

}
