using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandoverTakeoverController : MonoBehaviour
{
    public GameObject[] AllInstructions;
    public GameObject[] ObjectsToHighlight;
    public Material[] NormalMat;
    public Material highlight;
    public GameObject[] RayInteractionElements;
    public GameObject[] Valves;
    public int ValueCount;
    Text[] Display;
    Text[] RotValue;
    public bool[] valveOpen;
    public bool[] tasks;
    public GameObject[] Lables;
    public GameObject[] Next;
    public GameObject Needle;
    public GameObject[] Trigger;
    public GameObject[] Arrows;

    public GameObject LabelParent;
    public GameObject TriggerParent;
    public GameObject ArrowParent;

    public static HandoverTakeoverController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RayOnOff(false);
        ChangeMater(ObjectsToHighlight[0], NormalMat[0]);
        
        Lables = new GameObject[Valves.Length];
        Display = new Text[Valves.Length];
        RotValue = new Text[Valves.Length];

        Arrows = new GameObject[ArrowParent.transform.childCount];
        Trigger = new GameObject[TriggerParent.transform.childCount];

        for (int i = 0; i < LabelParent.transform.childCount; i++)
        {
            Lables[i] = LabelParent.transform.GetChild(i).gameObject;
            Display[i] = LabelParent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
            RotValue[i] = LabelParent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>();
        }

        for (int i = 0; i < TriggerParent.transform.childCount; i++)
        {
            Trigger[i] = TriggerParent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < ArrowParent.transform.childCount; i++)
        {
            Arrows[i] = ArrowParent.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (!valveOpen[0] && !tasks[0])
        {
            AllInstructions[0].SetActive(false);
            Arrows[0].SetActive(true);
            Arrows[1].SetActive(true);
            Trigger[0].SetActive(true);
            tasks[0] = true;
        }

        if (!valveOpen[1] && !tasks[1])
        {
            AllInstructions[1].SetActive(false);
           // Arrows[0].SetActive(true);
          //  Arrows[1].SetActive(true);
           // Trigger[0].SetActive(true);
            tasks[1] = true;
        }
    }

    public void suction()
    {
        AllInstructions[1].SetActive(true);
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
        Trigger[0].SetActive(false);
        Lables[1].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], NormalMat[0]);
    }
   
    public void updateValve(int n)
    {
        //X
        if (n == 0)
        {
            print(Valves[n].transform.localEulerAngles.x);
            int temp = (int)(Valves[n].transform.localEulerAngles.x);
            temp = temp * 100 / 80;
            ValueCount = 100 - temp;
        }
        //X
        else if(n==1)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.x);
            temp = temp * 100 / 80;
            ValueCount = temp;
        }


        if (ValueCount > 98)
        {
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 98 && ValueCount > 0)
        {
            RotValue[n].text = ValueCount.ToString();
        }
        else
        {
            valveOpen[n] = false;
            RotValue[n].text = "0";
            Display[n].text = "Closed";
        }
    }

    public void Stophigh(int n)
    {
         StopHighlight(ObjectsToHighlight[n], NormalMat[0]);      
    }

    public void StopHighlight(GameObject obj, Material norm)
    {
        StopAllCoroutines();
        obj.GetComponent<Renderer>().material = norm;
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
}
