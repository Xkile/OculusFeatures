using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequentialController : MonoBehaviour
{

    public GameObject[] AllInstructions;
    public GameObject[] ObjectsToHighlight;
    public Material[] NormalMat;
    public Material highlight;
    public GameObject[] RayInteractionElements;
    public GameObject[] Valves;
    public int ValueCount;
    public Text[] Display;
    public Text[] RotValue;
    public bool[] valveOpen;
    public bool[] tasks;
    public GameObject[] Lables;
    public GameObject[] Next;
    public GameObject Needle;
    public GameObject[] Trigger;
    public GameObject[] Arrows;

    public static SequentialController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RayOnOff(false);
        ChangeMater(ObjectsToHighlight[0], NormalMat[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (valveOpen[0] && !tasks[0])
        {
            AllInstructions[1].SetActive(false);
            Arrows[2].SetActive(true);
            Arrows[3].SetActive(true);
            Arrows[4].SetActive(true);
            Trigger[1].SetActive(true);
            tasks[0] = true;
        }

        if (valveOpen[1] && !tasks[1])
        {
            Arrows[5].SetActive(true);
            Arrows[6].SetActive(true);
            Arrows[7].SetActive(true);
            AllInstructions[2].SetActive(false);
            Trigger[2].SetActive(true);
            tasks[1] = true;
        }

        if (!valveOpen[2] && !tasks[2])
        {
            AllInstructions[3].SetActive(false);
            Arrows[8].SetActive(true);
            Trigger[3].SetActive(true);
            tasks[2] = true;
        }

        if (!valveOpen[3] && !tasks[3])
        {
            AllInstructions[4].SetActive(false);
            AllInstructions[5].SetActive(true);
            ChangeMater(ObjectsToHighlight[0], NormalMat[0]);
            tasks[3] = true;
        }

        if(tasks[3] && valveOpen[3] && !tasks[4])
        {
            Arrows[1].SetActive(true);
            Arrows[2].SetActive(true);
            Trigger[2].SetActive(true); 
            AllInstructions[7].SetActive(false);
            tasks[4] = true;
        }

        if (tasks[3]&& valveOpen[2] && !tasks[5])
        {
            AllInstructions[8].SetActive(false);
            Arrows[2].SetActive(true);
            Arrows[3].SetActive(true);
            Trigger[1].SetActive(true);
            tasks[5] = true;
        }

        if (tasks[3] && valveOpen[1]&& !tasks[6])
        {
            Arrows[5].SetActive(true);
            Arrows[6].SetActive(true);
            Arrows[7].SetActive(true);
            Trigger[0].SetActive(true);
            tasks[6] = true;
        }
    }

    public void checkOutletTemp()
    {
        if (!tasks[3])
        {
            Next[0].SetActive(true);
            RayOnOff(true);
        }
        else
        {
            Next[1].SetActive(true);
            RayOnOff(true);
        }       
    }

    public void checkOutletTemp2()
    {
        AllInstructions[0].SetActive(false);
        RayOnOff(false);
        Arrows[0].SetActive(true);
        Arrows[1].SetActive(true);
        Trigger[0].SetActive(true);
    }

    public void openDrain()
    {
        AllInstructions[2].SetActive(true);       
        Arrows[2].SetActive(false);
        Arrows[3].SetActive(false);
        Arrows[4].SetActive(false);
        Trigger[1].SetActive(false);
        Lables[1].SetActive(true);
        ChangeMater(ObjectsToHighlight[2], NormalMat[1]);       
    }

    public void openDrainP2()
    {
        Arrows[2].SetActive(false);
        Arrows[3].SetActive(false);
        Trigger[1].SetActive(false);
        AllInstructions[9].SetActive(true);
        ChangeMater(ObjectsToHighlight[2], NormalMat[1]);
    }
    public void backwashInlet()
    {
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
        Trigger[0].SetActive(false);      
        AllInstructions[1].SetActive(true);
        Lables[0].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
    }

    public void backwashInletP2()
    {
        Arrows[5].SetActive(false);
        Arrows[6].SetActive(false);
        Arrows[7].SetActive(false);
        Trigger[0].SetActive(false);
        ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
    }

    public void saltwater()
    {
        Arrows[5].SetActive(false);
        Arrows[6].SetActive(false);
        Arrows[7].SetActive(false);
        Lables[2].SetActive(true);
        AllInstructions[3].SetActive(true);
        ChangeMater(ObjectsToHighlight[3], NormalMat[1]);
    }

    public void saltwaterP2()
    {
        Arrows[1].SetActive(false);
        Arrows[2].SetActive(false);
        Trigger[2].SetActive(false);
        AllInstructions[8].SetActive(true);
        ChangeMater(ObjectsToHighlight[3], NormalMat[1]);
    }

    public void saltwaterout()
    {
        Arrows[8].SetActive(false);       
        Trigger[3].SetActive(false);
        AllInstructions[4].SetActive(true);
        ChangeMater(ObjectsToHighlight[4], NormalMat[1]);
        Lables[3].SetActive(true);
    }

    public void saltwateroutP2()
    {      
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
        Trigger[3].SetActive(false);
        AllInstructions[7].SetActive(true);
        ChangeMater(ObjectsToHighlight[4], NormalMat[1]);
        Lables[3].SetActive(true);
    }

    public void DecreaseInPressure()
    {
        AllInstructions[5].SetActive(false);
        AllInstructions[6].SetActive(true);
    }

    public void DecreaseInPressure2()
    {
        AllInstructions[6].SetActive(false);
        Arrows[0].SetActive(true);
        Arrows[1].SetActive(true);
        Trigger[3].SetActive(true);
        RayOnOff(false);
    }

    public void updateValve(int n)
    {
        //X
        if (n == 3)
        {
            print(Valves[n].transform.localEulerAngles.x);
            int temp = (int)(Valves[n].transform.localEulerAngles.x);
            temp = temp * 100 / 80;
            ValueCount = temp;
        }
        //Y
        else
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y);
            ValueCount = 99 - temp;
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

        if (n == 0)
        {
            StopHighlight(ObjectsToHighlight[n], NormalMat[0]);
        }
        else if (n == 1)
        {
            StopHighlight(ObjectsToHighlight[n], NormalMat[1]);
        }
        else
        {
            StopHighlight(ObjectsToHighlight[n], NormalMat[1]);
        }
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
