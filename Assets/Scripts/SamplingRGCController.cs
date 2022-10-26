using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SamplingRGCController : MonoBehaviour
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

    public void EnsureN1Close()
    {
        AllInstructions[0].SetActive(false);
        StopHighlight(ObjectsToHighlight[0], NormalMat[0]);
        Lables[0].SetActive(false);
        AllInstructions[1].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
        Lables[1].SetActive(true);
    }

    public void EnsureBV3Closed()
    {
        AllInstructions[1].SetActive(false);
        StopHighlight(ObjectsToHighlight[1], NormalMat[1]);
        Lables[1].SetActive(false);
        AllInstructions[2].SetActive(true);
        ChangeMater(ObjectsToHighlight[2], NormalMat[1]);
        Lables[2].SetActive(true);
    }

    public void EnsureBV2Open()
    {
        AllInstructions[2].SetActive(false);
        StopHighlight(ObjectsToHighlight[2], NormalMat[1]);
        Lables[2].SetActive(false);
        AllInstructions[3].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
        Lables[1].SetActive(true);
        tasks[0] = true;
    }

    public void UpdatePressure(int n)
    {
        if (!tasks[12])
        {
            if (n == 0)
            {
                Needle.transform.DORotate(new Vector3(0, 0, 277.8f), 4).OnComplete(() => Next[2].SetActive(true));
            }
        }
        else
        {
            Needle.transform.DORotate(new Vector3(0, 0, 277.8f), 4).OnComplete(() => Next[4].SetActive(true));
        }
       
    }

    public void PressureUpdated()
    {
        AllInstructions[4].SetActive(false);
        AllInstructions[5].SetActive(true);
        Lables[1].SetActive(false);
        Lables[2].SetActive(true);
        ChangeMater(ObjectsToHighlight[2], NormalMat[1]);
        tasks[2] = true;
    }

    public void PressureZero()
    {
        AllInstructions[18].SetActive(false);
        AllInstructions[19].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {      
        ChangeMater(ObjectsToHighlight[0], NormalMat[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if(valveOpen[1] && tasks[0] && !tasks[1])
        {
            ChangeMater(ObjectsToHighlight[3], NormalMat[2]);
            AllInstructions[3].SetActive(false);
            AllInstructions[4].SetActive(true);
            tasks[1] = true;
        }

        if (!valveOpen[2] && tasks[2] && !tasks[3]){
            AllInstructions[5].SetActive(false);
            AllInstructions[6].SetActive(true);
            Lables[2].SetActive(false);
            ChangeMater(ObjectsToHighlight[4], NormalMat[2]);
            Lables[3].SetActive(true);
            tasks[3] = true;
        }

        if (tasks[3] && valveOpen[3] && !tasks[4])
        {
            AllInstructions[6].SetActive(false);
            AllInstructions[7].SetActive(true);
            ChangeMater(ObjectsToHighlight[0], NormalMat[0]);
            Lables[0].SetActive(true);
            tasks[4] = true;
        }

        if (tasks[4] && valveOpen[0] && !tasks[5])
        {
            AllInstructions[7].SetActive(false);
            AllInstructions[8].SetActive(true);
            Needle.transform.DORotate(new Vector3(0, 0, 300f), 5).OnComplete(() => Next[3].SetActive(true));
            tasks[5] = true;
        }

        if (tasks[5] && !valveOpen[3] && !tasks[6])
        {
            AllInstructions[10].SetActive(false);
            AllInstructions[11].SetActive(true);
            tasks[6] = true;
        }

        if (tasks[6] && !valveOpen[1] && !tasks[7])
        {
            AllInstructions[12].SetActive(false);
            AllInstructions[13].SetActive(true);
            ChangeMater(ObjectsToHighlight[5], NormalMat[3]);
            Lables[4].SetActive(true);
            tasks[7] = true;
        }

        if (tasks[7] && !valveOpen[4] && !tasks[8])
        {
            AllInstructions[13].SetActive(false);
            AllInstructions[14].SetActive(true);
            ChangeMater(ObjectsToHighlight[6], NormalMat[3]);
            Lables[4].SetActive(false);
            Lables[5].SetActive(true);
            tasks[8] = true;
        }

        if (tasks[8] && valveOpen[3] && !tasks[9])
        {
            AllInstructions[14].SetActive(false);
            AllInstructions[15].SetActive(true);
            Lables[5].SetActive(false);
            Lables[3].SetActive(true);
            ChangeMater(ObjectsToHighlight[4], NormalMat[1]);
            tasks[9] = true;
        }

        if (tasks[9] && valveOpen[2] &&  !tasks[10])
        {
            AllInstructions[15].SetActive(false);
            AllInstructions[16].SetActive(true);
            Lables[3].SetActive(false);
            Lables[2].SetActive(true);
            ChangeMater(ObjectsToHighlight[2], NormalMat[1]);
            tasks[10] = true;
        }

        if (tasks[10] && valveOpen[2] && !tasks[11])
        {
            AllInstructions[16].SetActive(false);
            AllInstructions[17].SetActive(true);
            Lables[2].SetActive(false);
            Lables[1].SetActive(true);          
            ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
            tasks[11] = true;
        }

        if (tasks[11] && valveOpen[1] && !tasks[12])
        {
            AllInstructions[17].SetActive(false);
            AllInstructions[18].SetActive(true);
            ChangeMater(ObjectsToHighlight[3], NormalMat[2]);
            tasks[12] = true;
        }

    }

    public void PressureIncreased()
    {
        AllInstructions[8].SetActive(false);
        AllInstructions[9].SetActive(true);
        Lables[0].SetActive(false);
    }

    public void FlushingDone()
    {
        AllInstructions[9].SetActive(false);
        AllInstructions[10].SetActive(true);
        ChangeMater(ObjectsToHighlight[4], NormalMat[1]);
        Lables[3].SetActive(true);
    }

    public void PressureReduced()
    {
        AllInstructions[11].SetActive(false);
        AllInstructions[12].SetActive(true);
        Lables[3].SetActive(false);
        ChangeMater(ObjectsToHighlight[1], NormalMat[1]);
        Lables[1].SetActive(true);
    }

    public void updateValve(int n)
    {
        if (n == 0)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.z);
            print(temp);
            ValueCount = 99 - temp;
            if (ValueCount > 0)
            {
                Next[0].SetActive(false);
            }else
            {
                Next[0].SetActive(true);
            }
        }else if (n ==1 || n==2 || n ==3)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.z);
            ValueCount = temp * 10/9;          
            if (n == 1 && ValueCount > 1)
            {
                Next[1].SetActive(false);
            }else if (n == 2&& ValueCount < 100)
            {
                Next[2].SetActive(false);
            }else if (n == 1 && ValueCount == 0)
            {
                Next[1].SetActive(true);
            }else if (n == 2 && ValueCount == 100)
            {
                Next[2].SetActive(true);
            }
        }
        else if (n == 4)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.z);
            ValueCount = temp * 10 / 18;
        }

        print("n is " + n + " value is " + ValueCount);    

        if (ValueCount > 98)
        {         
            Display[n].text = "Open";
            RotValue[n].text = "100";
            valveOpen[n] = true;
        }
        else if (ValueCount <= 98 && ValueCount > 0)
        {          
            //valveOpen[n] = false;
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
        }else if (n == 3)
        {
            StopHighlight(ObjectsToHighlight[n], NormalMat[2]);
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

