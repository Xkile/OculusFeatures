using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Oculus.Interaction.OneGrabRotateTransformer;

public class LeakandFireController : MonoBehaviour
{
    public GameObject[] AllInstructions;
    public Renderer[] highlightingObjects;
    public GameObject[] rotatableValves;

    public GameObject[] switches;
    public GameObject[] voiceOverDisplay;

    public bool firstPanelShout;
    public bool secondPanelShout;
    public bool moveToValve;
    public bool firstTimeVoSwitch1;
    public bool firstTimeVoSwitch2;

    public bool[] valveRotated;

    public Text[] Status;
    public Text[] rotValue;

    public Material normalMat;
    public Material highlight;

    public GameObject fireSmall;
    public GameObject fireLarge;

    // Start is called before the first frame update
    void Start()
    {
        firstPanelShout = true;
        //secondPanelShout = true;
        firstTimeVoSwitch1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(valveRotated[0] && valveRotated[1] && !moveToValve)
        {
            AllInstructions[3].SetActive(false);
            AllInstructions[4].SetActive(true);
            fireLarge.SetActive(false);
            fireSmall.SetActive(false);
            moveToValve = true;
        }
        
    }

    public void MoveTowardTheFire()
    {
        AllInstructions[0].SetActive(false);
        AllInstructions[1].SetActive(true);
        //Show arrows
        StartCoroutine(ChangeMaterial(highlightingObjects[0]));
        StartCoroutine(ChangeMaterial(highlightingObjects[1]));

    }

    public void TurnOnSecondSwitch()
    {
        secondPanelShout = true;
        firstTimeVoSwitch2 = true;
    }

    public void SwitchReset(GameObject obj)
    {
       
        if (firstPanelShout)
        {
            AllInstructions[1].SetActive(false);
            AllInstructions[2].SetActive(true);
            fireLarge.SetActive(true);
            firstPanelShout = false;

        }else if (secondPanelShout)
        {
            AllInstructions[2].SetActive(false);
            AllInstructions[3].SetActive(true);
            AllInstructions[5].SetActive(true);
            AllInstructions[6].SetActive(true);
            //Show arrows
            StartCoroutine(ChangeMaterial(highlightingObjects[2]));
            StartCoroutine(ChangeMaterial(highlightingObjects[3]));         
            secondPanelShout = false;
        }
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);       
    }

    public void updateRotatingValves(int n, string Initial, string Final)
    {
        int valueCount;
        valueCount = (int)(rotatableValves[n].transform.eulerAngles.y *10/16);
        

        //print("inlet value: " + inletValueCount);
        //print("rotation: " + valve.transform.localRotation.y);
        if (valueCount > 98)
        {
            Status[n].text = Final;
            rotValue[n].text = "100";
            rotatableValves[n].GetComponent<MeshCollider>().enabled = false;
            valveRotated[n] = true;
        }
        else if (valueCount <= 99 && valueCount >= 0)
        {
            rotValue[n].text = valueCount.ToString();
            Status[n].text = Initial;
        }
        else
        {
            rotValue[n].text = "0";
            Status[n].text = Initial;
        }
    }

    public void updateValve(int n)
    {
        updateRotatingValves(n, "Closed", "Open");
    }

    public void updateSwitch(int n)
    {
        print("Switch 1 Angle: " + switches[n].transform.eulerAngles.x);
        if (switches[n].transform.eulerAngles.x > 23)
        {
            print("turn on fire voice over");
            voiceOverDisplay[n].SetActive(true);
            if (firstTimeVoSwitch1 || firstTimeVoSwitch2)
            {
                VoiceManager.instance.PlayAudio(3);
                firstTimeVoSwitch1 = false;
                firstTimeVoSwitch2 = false;
            }          
        }
        else
        {
            print("turn off fire voice over");
            //voiceOverDisplay[n].SetActive(false);
        }
    }



    IEnumerator ChangeMaterial(Renderer obj)
    {
        obj.material = highlight;
        yield return new WaitForSeconds(.5f);
        obj.material = normalMat;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ChangeMaterial(obj));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
