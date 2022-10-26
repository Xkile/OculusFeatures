using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PumpOperationController : MonoBehaviour
{

    public GameObject[] AllIntructions;
    public GameObject[] ObjectsToHighlight;
    public GameObject[] RayInteractionElements;
    public Material highlight;
    public Material[] normalMat;
    public GameObject Lever;
    public GameObject Needle;
    public int increment;
    public GameObject[] Arrows;
    public GameObject[] Trigger;
    public int ValueCount;
    public GameObject[] Valves;
    public Text[] Display;
    public Text[] RotValue;

    public bool[] tasks;
    public bool[] valveOpen;
    public bool warmUpDone;
    public GameObject[] Lables;

    public GameObject Player;

    public GameObject [] rotateBut;

    public GameObject[] Display2;

    public static PumpOperationController instance;

    public GameObject[] Next;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        increment = 0;
        RayOnOff(false);
        ChangeMater(ObjectsToHighlight[0], normalMat[0]);
    }

    // Update is called once per frame
    void Update()
    {
        //print(Needle.transform.rotation);
        /*  print("Lever: " + Lever.transform.localEulerAngles);
          print("Lever Max Angle: "+Lever.GetComponent<OneGrabRotateTransformer>().Constraints.MaxAngle.Value);

         if( Lever.transform.localEulerAngles.z == Lever.GetComponent<OneGrabRotateTransformer>().Constraints.MaxAngle.Value)
          {
              print("Needle should move");
              increment += -33;
              Needle.transform.rotation = Quaternion.Euler(increment, 0, 0);
          }*/

        if (valveOpen[0] && !tasks[1])
        {
            AllIntructions[2].SetActive(false);
            AllIntructions[3].SetActive(true);
            StopAllCoroutines();
            ChangeMater(ObjectsToHighlight[3], normalMat[2]);
            rotateBut[0].SetActive(true);
            RayOnOff(true);
            Lables[1].SetActive(true);
            tasks[1] = true;
        }

        if(valveOpen[1] && !tasks[2])
        {
            AllIntructions[3].SetActive(false);
            AllIntructions[4].SetActive(true);
            StopAllCoroutines();
            ChangeMater(ObjectsToHighlight[4], normalMat[2]);
            ChangeMater(ObjectsToHighlight[5], normalMat[2]);
            Lables[0].SetActive(false);           
            Lables[2].SetActive(true);
            Lables[3].SetActive(true);
            RayOnOff(false);
            tasks[2] = true;
            
        }
        if (valveOpen[2] && valveOpen[3] && !tasks[3])
        {
            AllIntructions[4].SetActive(false);
            AllIntructions[5].SetActive(true);
            StopAllCoroutines();
            ChangeMater(ObjectsToHighlight[6], normalMat[2]);
            ChangeMater(ObjectsToHighlight[7], normalMat[2]);
            Lables[4].SetActive(true);
            Lables[5].SetActive(true);           
            rotateBut[1].SetActive(true);         
            RayOnOff(true);
            tasks[3] = true;
        }

        if (valveOpen[4] && valveOpen[5] && !tasks[4])
        {
            StartCoroutine(lineUp());
            RayOnOff(false);
            tasks[4] = true;

        }

        if (valveOpen[6] && valveOpen[7] && !tasks[5])
        {
            AllIntructions[6].SetActive(false);
            AllIntructions[7].SetActive(true);
            Arrows[2].SetActive(true);

            ChangeMater(ObjectsToHighlight[10], normalMat[2]);
            ChangeMater(ObjectsToHighlight[11], normalMat[2]);
            ChangeMater(ObjectsToHighlight[12], normalMat[2]);
            ChangeMater(ObjectsToHighlight[13], normalMat[2]);

            Lables[6].SetActive(false);
            Lables[7].SetActive(false);

            Lables[8].SetActive(true);
            Lables[9].SetActive(true);
            Lables[10].SetActive(true);
            Lables[11].SetActive(true);
            tasks[5] = true;
        }

        if (valveOpen[8] && valveOpen[9] && valveOpen[10] && valveOpen[11] && !tasks[6])
        {
            Lables[8].SetActive(false);
            Lables[9].SetActive(false);
            Lables[10].SetActive(false);
            Lables[11].SetActive(false);
            AllIntructions[7].SetActive(false);
            Arrows[2].SetActive(false);
            Arrows[3].SetActive(true);
            Arrows[4].SetActive(true);
            Trigger[1].SetActive(true);
            tasks[6] = true;
        }

        if(valveOpen [12] && !tasks[7])
        {
            AllIntructions[8].SetActive(false);
            AllIntructions[9].SetActive(true);
            ChangeMater(ObjectsToHighlight[23], normalMat[2]);
            ChangeMater(ObjectsToHighlight[24], normalMat[2]);
            Lables[13].SetActive(true);
            Lables[14].SetActive(true);
            tasks[7] = true;
        }

        if (valveOpen[13] && valveOpen [14] && !tasks[8])
        {
            AllIntructions[9].SetActive(false);
            AllIntructions[10].SetActive(true);
            RayOnOff(true);
            tasks[8] = true;
            
        }

        if (valveOpen[15] && !tasks[9])
        {
            AllIntructions[11].SetActive(false);
            ObjectsToHighlight[15].SetActive(false);
            Lables[15].SetActive(false);
            Arrows[6].SetActive(true);
            Arrows[7].SetActive(true);
            Arrows[11].SetActive(true);
            Trigger[3].SetActive(true);
            RayOnOff(false);
            tasks[9] = true;
        }

        if (valveOpen[16] && valveOpen[17] && valveOpen[18] && valveOpen[19] && !tasks[10])
        {
            Lables[16].SetActive(false);
            Lables[17].SetActive(false);
            Lables[18].SetActive(false);
            Lables[19].SetActive(false);        
            ObjectsToHighlight[16].SetActive(false);
            ObjectsToHighlight[17].SetActive(false);
            ObjectsToHighlight[18].SetActive(false);
            ObjectsToHighlight[19].SetActive(false);        
            Lables[20].SetActive(true);
            AllIntructions[12].SetActive(false);
            AllIntructions[13].SetActive(true);
            ChangeMater(ObjectsToHighlight[20], normalMat[2]);
            tasks[10] = true;          
        }

        if (valveOpen[20] && !tasks[11])
        {
            AllIntructions[13].SetActive(false);
            Arrows[8].SetActive(true);
            Arrows[9].SetActive(true);
            Arrows[10].SetActive(true);            
            Trigger[4].SetActive(true);           
            tasks[11] = true;
        }

        if (valveOpen[21] && valveOpen[22] && !tasks[12])
        {
            AllIntructions[14].SetActive(false); 
            AllIntructions[15].SetActive(true);
            Lables[23].SetActive(true);
            Lables[21].SetActive(false); 
            Lables[22].SetActive(false);
            ObjectsToHighlight[21].SetActive(false);
            ObjectsToHighlight[22].SetActive(false);
            ChangeMater(ObjectsToHighlight[25], normalMat[2]);
            tasks[12] = true;
        }

        if (valveOpen[23] && !tasks[13])
        {
            AllIntructions[15].SetActive(false);
            AllIntructions[16].SetActive(true);
            ChangeMater(ObjectsToHighlight[26], normalMat[2]);
            Lables[24].SetActive(true);
            tasks[13] = true;
        }

        if (warmUpDone && !tasks[14])
        {
            AllIntructions[16].SetActive(false);          
            Arrows[12].SetActive(true);
            Trigger[5].SetActive(true);
            tasks[14] = true;
        }

        if (valveOpen[25] && valveOpen[26] && !tasks[15])
        {
            AllIntructions[17].SetActive(false);
            AllIntructions[18].SetActive(true);
            StopAllCoroutines();
            ChangeMater(ObjectsToHighlight[29], normalMat[2]);
            ChangeMater(ObjectsToHighlight[30], normalMat[2]);
            Lables[27].SetActive(true);
            Lables[28].SetActive(true);
            tasks[15] = true;
        }

        if (valveOpen[27] && valveOpen[28] && !tasks[16])
        {
            AllIntructions[18].SetActive(false);
            AllIntructions[19].SetActive(true);
            StopAllCoroutines();
            ChangeMater(ObjectsToHighlight[31], normalMat[2]);
            ChangeMater(ObjectsToHighlight[32], normalMat[2]);
            Lables[29].SetActive(true);
            Lables[30].SetActive(true);
            tasks[16] = true;
        }

        if(valveOpen[29] && valveOpen [30] && !tasks[17])
        {
            Next[0].SetActive(true);
            tasks[17] = true;
        }

        if(valveOpen[31] && !valveOpen[25] && !tasks[18])
        {
            Arrows[14].SetActive(true);
            Arrows[15].SetActive(true);
            Arrows[16].SetActive(true);
            Trigger[7].SetActive(true);
            tasks[18] = false;
        }

        if(valveOpen[32] && !tasks[19])
        {
            Arrows[17].SetActive(true);
            Arrows[18].SetActive(true);
            Trigger[9].SetActive(true);
            tasks[19] = true;
        }

        if(!valveOpen[33] && !valveOpen[34] && !tasks[20])
        {
            ChangeMater(ObjectsToHighlight[22], normalMat[2]);
            ChangeMater(ObjectsToHighlight[23], normalMat[2]);
            //ObjectsToHighlight[22].GetComponent<Renderer>().material = normalMat[2];
            // ObjectsToHighlight[23].GetComponent<Renderer>().material = normalMat[2];
            AllIntructions[21].SetActive(false);
            AllIntructions[22].SetActive(true);
            ObjectsToHighlight[22].SetActive(true);
            ObjectsToHighlight[23].SetActive(true);
            Lables[13].SetActive(true);
            Lables[13].SetActive(true);
            tasks[20] = true;
        }

        if(tasks[20] && !valveOpen[13] && !valveOpen[14] && !tasks[21])
        {
            ChangeMater(ObjectsToHighlight[35], normalMat[2]);
            ChangeMater(ObjectsToHighlight[36], normalMat[2]);
            //ObjectsToHighlight[22].GetComponent<Renderer>().material = normalMat[2];
            // ObjectsToHighlight[23].GetComponent<Renderer>().material = normalMat[2];
            AllIntructions[22].SetActive(false);
            AllIntructions[23].SetActive(true);
            ObjectsToHighlight[35].SetActive(true);
            ObjectsToHighlight[36].SetActive(true);
            
            tasks[21] = true;
        }

        if(!valveOpen[35] && !tasks[22])
        {
            Arrows[19].SetActive(false);
            Arrows[20].SetActive(true);
            Trigger[9].SetActive(true);
        }
    }

    public void forcedValveOpen1()
    {
        valveOpen[1] = true;
    }

    public void forcedValveOpen2()
    {
        
        valveOpen[4] = true;
        valveOpen[5] = true;
    }

    public void forcedValveOpen3()
    {
        valveOpen[15] = true;      
    }

    public void forcedValveOpen4()
    {
        valveOpen[16] = true;
        valveOpen[17] = true;
        valveOpen[18] = true;
        valveOpen[19] = true;
    }

    public void forcedValveOpen5()
    {
        valveOpen[21] = true;
        valveOpen[22] = true;       
    }

    public void MPStop()
    {
        AllIntructions[24].SetActive(false);
        AllIntructions[25].SetActive(true);

        Arrows[19].SetActive(true);
        ObjectsToHighlight[35].SetActive(false);
        ObjectsToHighlight[36].SetActive(false);
        Lables[33].SetActive(false);
        Lables[34].SetActive(false);
        Lables[35].SetActive(true);
        ChangeMater(ObjectsToHighlight[37], normalMat[2]);

    }

    public void CheckSealOil()
    {
        StopAllCoroutines();
        AllIntructions[0].SetActive(false);
        AllIntructions[1].SetActive(true);
        ChangeMater(ObjectsToHighlight[1], normalMat[1]);
    }

    public void raisePressure()
    {
        Arrows[0].SetActive(true);
        Arrows[1].SetActive(true);
        Trigger[0].SetActive(true);
        AllIntructions[1].SetActive(false);
    }

    public void DrainCondensatefromMP()
    {
        StopAllCoroutines();
        //RayOnOff(false);
        ChangeMater(ObjectsToHighlight[15], normalMat[2]);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-9.88f, 5.248f, 18.69f);
        Lables[15].SetActive(true);
        AllIntructions[11].SetActive(true);
        StartCoroutine(ControllerOn());

    }

    public void DrainCondensatefromLP()
    {
        AllIntructions[14].SetActive(true);
        StopAllCoroutines();       
        ChangeMater(ObjectsToHighlight[21], normalMat[2]);
        ChangeMater(ObjectsToHighlight[22], normalMat[2]);
        Arrows[8].SetActive(false);
        Arrows[9].SetActive(false);
        Arrows[10].SetActive(false);
        RayOnOff(true);
        Lables[21].SetActive(true);
        Lables[22].SetActive(true);
    }

    public void jtc()
    {
        AllIntructions[2].SetActive(true);
        Lables[0].SetActive(true);
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[2], normalMat[2]);
        Trigger[0].SetActive(false);
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
    }

    public void suction()
    {
        Trigger[1].SetActive(false);
        Arrows[3].SetActive(false);
        Arrows[4].SetActive(false);
        Arrows[5].SetActive(true);
        Trigger[2].SetActive(true);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-0.88f, 11.2f, 2.76f);
        StartCoroutine(ControllerOn());
    }

    public void suction2()
    {
        Arrows[5].SetActive(false);
        Trigger[2].SetActive(false);
        Lables[12].SetActive(true);
        AllIntructions[8].SetActive(true);
        //StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[14], normalMat[2]);
    }

    public void turbineCasing()
    {
        RayOnOff(true);
        Arrows[6].SetActive(false);
        Arrows[7].SetActive(false);
        Arrows[11].SetActive(false);
        Trigger[3].SetActive(false);
        Lables[16].SetActive(true);
        Lables[17].SetActive(true);
        Lables[18].SetActive(true);
        Lables[19].SetActive(true);
        AllIntructions[12].SetActive(true);
        StopAllCoroutines();
        ChangeMater(ObjectsToHighlight[16], normalMat[2]);
        ChangeMater(ObjectsToHighlight[17], normalMat[2]);
        ChangeMater(ObjectsToHighlight[18], normalMat[2]);
        ChangeMater(ObjectsToHighlight[19], normalMat[2]);
    }

    public void steam()
    {
        Trigger[5].SetActive(false);
        Arrows[12].SetActive(false);
        Arrows[13].SetActive(true);
        Trigger[6].SetActive(true);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-0.88f, 11.2f, 2.76f);
        StartCoroutine(ControllerOn());
    }

    public void steam2()
    {
        Arrows[13].SetActive(false);
        Trigger[6].SetActive(false);
        Lables[25].SetActive(true);
        Lables[26].SetActive(true);
        AllIntructions[17].SetActive(true);       
        ChangeMater(ObjectsToHighlight[27], normalMat[2]);
        ChangeMater(ObjectsToHighlight[28], normalMat[2]);
    }

    public void jumpToSteamVent()
    {
        AllIntructions[20].SetActive(true);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-1.42f, 5.248f, -5.88f);
        ObjectsToHighlight[25].GetComponent<Renderer>().material = normalMat[2];
        ObjectsToHighlight[25].SetActive(true);
        ChangeMater(ObjectsToHighlight[25], normalMat[2]);
        ChangeMater(ObjectsToHighlight[33], normalMat[2]);
        Lables[23].SetActive(true);
        Lables[31].SetActive(true);
        StartCoroutine(ControllerOn());
    }

    public void stopCheck()
    {
        Arrows[14].SetActive(false);
        Arrows[15].SetActive(false);
        Arrows[16].SetActive(false);
        Trigger[7].SetActive(false);
        Lables[32].SetActive(true);
        ChangeMater(ObjectsToHighlight[32], normalMat[2]);
    }

    public void dcs()
    {
        Arrows[17].SetActive(false);
        Arrows[18].SetActive(false);
        Trigger[9].SetActive(false);
        AllIntructions[21].SetActive(true);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<OVRPlayerController>().enabled = false;
        Player.transform.position = new Vector3(-0.25f, 10.97f, 11.964f);
        StartCoroutine(ControllerOn());
        Lables[33].SetActive(true);
        Lables[34].SetActive(true);
        ChangeMater(ObjectsToHighlight[35], normalMat[2]);
        ChangeMater(ObjectsToHighlight[36], normalMat[2]);
    }

    public void final()
    {
        Arrows[21].SetActive(true);
        Arrows[22].SetActive(true);
        Arrows[23].SetActive(true);
        Trigger[10].SetActive(true);
    }
    public void final2()
    {
        Arrows[21].SetActive(false);
        Arrows[22].SetActive(false);
        Arrows[23].SetActive(false);
        Trigger[10].SetActive(false);
        AllIntructions[25].SetActive(true);
        ChangeMater(ObjectsToHighlight[38], normalMat[1]);
    }

    IEnumerator lineUp()
    {
        yield return new WaitForSeconds(4);
        AllIntructions[5].SetActive(false);
        Lables[0].SetActive(false);
        Lables[1].SetActive(false);
        Lables[2].SetActive(false);
        Lables[3].SetActive(false);
        Lables[4].SetActive(false);
        Lables[5].SetActive(false);
        AllIntructions[6].SetActive(true);
        ChangeMater(ObjectsToHighlight[8], normalMat[2]);
        ChangeMater(ObjectsToHighlight[9], normalMat[2]);
        Lables[6].SetActive(true);
        Lables[7].SetActive(true);
    }

    IEnumerator ControllerOn()
    {
        yield return new WaitForSeconds(2);
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<OVRPlayerController>().enabled = true;
    }

    public void updateValve(int n)
    {
        if (n == 0 || n==25 || n==26 || n == 29 || n ==30 || n ==33 || n ==34)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y);
            ValueCount = 100 - temp;
        }
        else if (n == 1 || n == 8 || n ==9 || n ==10 || n==12 || n == 20){            
            ValueCount = (int)(Valves[n].transform.localEulerAngles.x *10/8);                   
        }else if (n == 2 || n == 3 || n == 27 || n ==28)
        {
            ValueCount = (int)(Valves[n].transform.localEulerAngles.z);
        }else if (n == 4 )
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y *10/12);
            ValueCount = 100 - temp;
           // print("temp inlet: " + temp);
           // print("drain valve inlet: " + ValueCount);
            if(ValueCount > 2)
            {
                Display[n].text = "Draining";
                valveOpen[n] = true;
            }
            else
            {
                Display[n].text = "Closed";
            }
        }else if (n==5)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y * 10 / 12);
           // print("temp outlet: " + temp);
            ValueCount = 100 - temp;
           // print("drain valve outlet: " + ValueCount);
            if (ValueCount > 2)
            {
                Display[n].text = "Draining";
                valveOpen[n] = true;
            }
            else
            {
                Display[n].text = "Closed";
            }
        }else if (n==6 || n == 7)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y * 10 / 12);
            //print("temp outlet: " + temp);
            ValueCount = 100 - temp;
            //print("drain valve outlet: " + ValueCount);
            RotValue[n].text = ValueCount.ToString();
            if (ValueCount > 98)
            {
                Display[n].text = "Lined Up";
                valveOpen[n] = true;
            }
            else
            {
                Display[n].text = "Closed";
            }
        }else if (n ==11 || n > 12 && n < 20 || n == 21 || n == 22)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.y * 10 / 12);
            ValueCount = 100 - temp;
        }else if (n == 24)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.x * 10 / 8);
            ValueCount = 100 - temp;
            if (valveOpen[n] && !warmUpDone && ValueCount <1)
            {
                Display2[0].SetActive(false);
                warmUpDone = true;
            }else if (!valveOpen[n] && ValueCount> 98)
            {
                Display2[0].SetActive(true);
            }
        }else if (n == 23)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.z);
            ValueCount = 100 - temp;
        }else if (n == 32)
        {
            int temp = (int)(Valves[n].transform.localEulerAngles.x);
            ValueCount = 100 - temp;
        }
            
        if (ValueCount > 98)
        {
            if (n == 8 || n == 9 || n == 10 || n == 11)
            {
                Display[n].text = "Lined Up";
            }
            else
            {
                Display[n].text = "Open";
            }
            
            
            RotValue[n].text = "100";


            valveOpen[n] = true;
        }
        else if (ValueCount <= 99 && ValueCount > 0)
        {
            if(n== 33 || n == 34)
            {

            }
            else
            {
                valveOpen[n] = false;
            }
            
            RotValue[n].text = ValueCount.ToString();
        }
        else
        {
            if (ValueCount == 0)
            {
                valveOpen[n] = false;
            }
            RotValue[n].text = "0";
            Display[n].text = "Closed";
        }
    }

    public void updateSwitch(int n)
    {
        ValueCount = (int)(Valves[n].transform.localEulerAngles.z);
        if (ValueCount == 45)
        {
            AllIntructions[25].SetActive(false);
        }
    }
    public void ResetLever()
    {
        Lever.transform.rotation = Quaternion.identity;
        Lever.GetComponent<OneGrabRotateTransformer>().Constraints.MinAngle.Value += 35;
        Lever.GetComponent<OneGrabRotateTransformer>().Constraints.MaxAngle.Value += 35;
        if (!tasks[0])
        {
            raisePressure();
            tasks[0] = true;
        }
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
