using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlValveBypassController : MonoBehaviour
{

    public GameObject[] AllInstructions;
    public GameObject[] RayInteractionElements;
    public Text[] RotValue;
    public Text[] Display;
    public bool[] valveOpen;
    public GameObject[] Valves;
    public GameObject[] ObjectsToHighlight;
    public Material highlight;
    public Material[] normalMat;
    public GameObject[] Labels;
    public bool inletValveCompletelyClosed;
    public bool outletValveCompletelyClosed;
    public bool task1;
    public bool task2;
    public bool task3;
    public bool task4;
    public bool task5;
    public bool task6;

    int ValueCount;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMater(ObjectsToHighlight[0], normalMat[0]);
        RayOnOff(false);
        valveOpen[1] = true;
        outletValveCompletelyClosed = true;
        task3 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (valveOpen[0] && !valveOpen[1] && !task1)
        {
            AllInstructions[1].SetActive(false);
            AllInstructions[2].SetActive(true);
            RayOnOff(true);
            task1 = true;
            

        }

        if (inletValveCompletelyClosed && !task2)
        {
            AllInstructions[3].SetActive(false);
            AllInstructions[4].SetActive(true);
            ObjectsToHighlight[1].GetComponent<Renderer>().material = normalMat[1];
            ObjectsToHighlight[1].transform.localEulerAngles = new Vector3(0, 0, 0);
            ObjectsToHighlight[1].SetActive(true);
            ChangeMater(ObjectsToHighlight[1], normalMat[1]);
            outletValveCompletelyClosed = false;
            task2 = true;
            task3 = false;
            
        }

        if (outletValveCompletelyClosed && !task3)
        {
            AllInstructions[4].SetActive(false);
            AllInstructions[5].SetActive(true);
            Labels[0].SetActive(false);
            Labels[1].SetActive(false);
            Labels[2].SetActive(true);
            ChangeMater(ObjectsToHighlight[3], normalMat[1]);
            task3 = true;
        }

        if (valveOpen[2] && !task4)
        {
            AllInstructions[5].SetActive(false);
            AllInstructions[6].SetActive(true);
            Labels[2].SetActive(false);
            Labels[3].SetActive(true);
            Labels[4].SetActive(true);
            ChangeMater(ObjectsToHighlight[4], normalMat[1]);
            ChangeMater(ObjectsToHighlight[5], normalMat[1]);
            task4 = true;
        }

        if (valveOpen[3] && valveOpen[4] && !task5)
        {
            AllInstructions[6].SetActive(false);
            AllInstructions[7].SetActive(true);
            RayOnOff(true);
            task5 = true;
           
        }

        if (task5 && !valveOpen[3] && !valveOpen[4] && !task6)
        {
            AllInstructions[8].SetActive(false);
            AllInstructions[9].SetActive(true);
            Labels[3].SetActive(false);
            Labels[4].SetActive(false);
            task6 = true;
        }
    }

    public void CheckControlValve()
    {
        RayOnOff(false);
        AllInstructions[0].SetActive(false);
        AllInstructions[1].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], normalMat[1]);
        ChangeMater(ObjectsToHighlight[2], normalMat[1]);
        Labels[0].SetActive(true);
        Labels[1].SetActive(true);

    }
    public void ChangeMater(GameObject obj, Material norm)
    {
        StartCoroutine(ChangeMaterial(obj, norm));
    }

    IEnumerator ChangeMaterial(GameObject obj, Material norm)
    {       
        obj.GetComponent<Renderer>().material = highlight;
        yield return new WaitForSeconds(.5f);
        obj.GetComponent<Renderer>().material = norm;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ChangeMaterial(obj, norm));
    }
    public void RayOnOff(bool Active)
    {
        foreach (GameObject i in RayInteractionElements)
        {
            i.SetActive(Active);
        }
    }

    public void updateValveZ(int n)
    {
        int temp = (int)(Valves[n].transform.localEulerAngles.z);
        ValueCount = 100 - temp;
       print(ValueCount);
        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 99 && ValueCount > 1)
        {           
            RotValue[n].text = ValueCount.ToString();
            //Display[n].text = "Closed";
        }
        else
        {
            RotValue[n].text = "0";
            Display[n].text = "Closed";
            if (!outletValveCompletelyClosed)
            {
                outletValveCompletelyClosed = true;
            }
        }
    }

    public void updateValveY(int n)
    {
       // print(Valves[n].transform.localEulerAngles);
        int temp = (int)(Valves[n].transform.localEulerAngles.y);
        ValueCount = 100 - temp;
      //  print(ValueCount);
        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 98 && ValueCount > 31)
        {
            RotValue[n].text = ValueCount.ToString();
            Display[n].text = "Closed";
            //valveOpen[n] = false;
        }
        else
        {
            RotValue[n].text = "30";
            Display[n].text = "Closed";
            valveOpen[n] = false;
            //if (!inletValveCompletelyClosed)
           // {
               // inletValveCompletelyClosed = true;
          //  }
        }
    }

    public void updateValveY3(int n)
    {
        print(Valves[n].transform.localEulerAngles);
        int temp = (int)(Valves[n].transform.localEulerAngles.y);
        ValueCount = 100 - temp;
        print(ValueCount);
        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 98 && ValueCount > 1)
        {
            RotValue[n].text = ValueCount.ToString();
            Display[n].text = "Closed";
            valveOpen[n] = false;
        }
        else
        {
            RotValue[n].text = "0";
            Display[n].text = "Closed";
            valveOpen[n] = false;
            if (!inletValveCompletelyClosed)
            {
                inletValveCompletelyClosed = true;
            }
        }
    }

    public void updateValveZ2(int n)
    {
        ValueCount = (int)(Valves[n].transform.localEulerAngles.z);
        // print(ValueCount);
        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 99 && ValueCount > 0)
        {
            RotValue[n].text = ValueCount.ToString();
            //Display[n].text = "Open";
        }
        else
        {
            RotValue[n].text = "0";
            Display[n].text = "Closed";        
        }
    }

    public void updateValveY2(int n)
    {     
        ValueCount = (int)(Valves[n].transform.localEulerAngles.y);
        print(ValueCount);
        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 99 && ValueCount > 0)
        {
            RotValue[n].text = ValueCount.ToString();
            Display[n].text = "Closed";
           // valveOpen[n] = false;
        }
        else
        {
            RotValue[n].text = "0";
            Display[n].text = "Closed";
            valveOpen[n] = false;        
        }
    }

    public void CoordinateWithDCS()
    {
        AllInstructions[2].SetActive(false); 
        AllInstructions[3].SetActive(true);
        Valves[1].SetActive(false);
        Labels[1].SetActive(false);
        Labels[5].SetActive(true);
        Valves[5].SetActive(true);
        RayOnOff(false);
        ObjectsToHighlight[2].GetComponent<Renderer>().material = normalMat[1];
        ObjectsToHighlight[2].transform.localEulerAngles = new Vector3(-90, 0, 0);
        ObjectsToHighlight[2].SetActive(true);
        ChangeMater(ObjectsToHighlight[2], normalMat[1]);
    }

    public void CheckTemprature()
    {
        RayOnOff(false);
        AllInstructions[7].SetActive(false);
        AllInstructions[8].SetActive(true);
        ObjectsToHighlight[4].GetComponent<Renderer>().material = normalMat[1];
        ObjectsToHighlight[4].transform.localEulerAngles = new Vector3(-89.98f, 100, 0);
        ObjectsToHighlight[4].SetActive(true);
       ObjectsToHighlight[5].transform.localEulerAngles = new Vector3(-89.98f, 100, 0);
        ObjectsToHighlight[5].GetComponent<Renderer>().material = normalMat[1];
        ObjectsToHighlight[5].SetActive(true);
        ChangeMater(ObjectsToHighlight[4], normalMat[1]);
        ChangeMater(ObjectsToHighlight[5], normalMat[1]);
    }
}
