using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class GuageGlassController : MonoBehaviour
    {
        public GameObject[] AllInstructions;
        public Material highlight;
        public Material isolationNormalMat;
        public Material gaugeGlassNormalMat;
        public Material glassCBDNormalMat;

        public Renderer[] isoValvesRender;

        public Renderer GaugeGlass;
        public Renderer GlassCBD;
        public Renderer GlassCommonDrain;
        public Renderer GlassDrain;

        public bool[] isoValveOpen;

        public int[] isoValveValueCount;

        public Text[] isoDisplay;

        public Text[] isoValue;

        public GameObject[] isoValves;

        public bool valvesIsolated;
        public bool gaugeGlassOpen;
        public bool glassCBDOpen;
        public bool glassCommonDrain;
        public bool glassDrain;

        public GameObject Player;

        public bool GuageGlassVent;
        public bool GlassDrainFinalZ;

        public static GuageGlassController instance;

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ChangeMaterial(isoValvesRender[0], isolationNormalMat));
            StartCoroutine(ChangeMaterial(isoValvesRender[1], isolationNormalMat));
            StartCoroutine(ChangeMaterial(isoValvesRender[2], isolationNormalMat));
            StartCoroutine(ChangeMaterial(isoValvesRender[3], isolationNormalMat));
        }

        // Update is called once per frame
        void Update()
        {
            if (isoValveOpen[0] && isoValveOpen[1] && isoValveOpen[2] && isoValveOpen[3] && !valvesIsolated)
            {
                AllInstructions[0].SetActive(false);
                AllInstructions[1].SetActive(true);
                openGaugeGlassVent();
                valvesIsolated = true;
                
            }

            if (isoValveOpen[4] && !gaugeGlassOpen)
            {
                AllInstructions[1].SetActive(false);
                AllInstructions[2].SetActive(true);
                gaugeGlassOpen = true;
                GuageGlassVent = false;
             }

            if (isoValveOpen[5] && !glassCBDOpen)
            {
                AllInstructions[3].SetActive(false);
                AllInstructions[4].SetActive(true);
                glassCBDOpen = true;
                StartCoroutine(ChangeMaterial(GlassCommonDrain, glassCBDNormalMat));
        }

        if (isoValveOpen[6] && !glassCommonDrain)
        {
            AllInstructions[4].SetActive(false);
            AllInstructions[5].SetActive(true);
            glassCommonDrain = true;
            StartCoroutine(ChangeMaterial(GlassDrain, gaugeGlassNormalMat));
        }

        if (isoValveOpen[7] && !glassDrain)
        {
            AllInstructions[5].SetActive(false);
            AllInstructions[6].SetActive(true);
            glassDrain = true;
            StartCoroutine(ChangeMaterial(GlassCommonDrain, glassCBDNormalMat));
        }
    }

        IEnumerator ChangeMaterial(Renderer obj, Material NormalMat)
        {
            obj.material = highlight;
            yield return new WaitForSeconds(.5f);
            obj.material = NormalMat;
            yield return new WaitForSeconds(.5f);
            StartCoroutine(ChangeMaterial(obj, NormalMat));
        }

    public void updateIsolationValve(int valueCount, GameObject valve, Text DisplayText, Text ValueText, int n, string Initial, string Final)
    {
        /*if (GuageGlassVent)
        {
            print("glass");
            valueCount = (int)(valve.transform.localEulerAngles.x);
            valueCount = valueCount * -1;
            print(valueCount);
            if (valueCount > 98)
            {
                DisplayText.text = Final;
                ValueText.text = "100";
                isoValveOpen[n] = true;
            }
            else if (valueCount <= 99 && valueCount >= 0)
            {
                ValueText.text = valueCount.ToString();
                DisplayText.text = Initial;
            }
            else
            {
                ValueText.text = "0";
                DisplayText.text = Initial;
            }
        }*/
           if (GlassDrainFinalZ)
            {
                print("glassZ");
                valueCount = (int)(valve.transform.localEulerAngles.z);

                if (valueCount > 98)
                {
                    DisplayText.text = Final;
                    ValueText.text = "100";
                    isoValveOpen[n] = true;
                }
                else if (valueCount <= 99 && valueCount >= 0)
                {
                    ValueText.text = valueCount.ToString();
                    DisplayText.text = Initial;
                }
                else
                {
                    ValueText.text = "0";
                    DisplayText.text = Initial;
                }
            }
            else
            {
                print("not glass");
                valueCount = (int)(valve.transform.localEulerAngles.y);
                print("value count: " + valve.gameObject.name + ": " + valueCount);
                if (valueCount > 98)
                {
                    DisplayText.text = Final;
                    ValueText.text = "100";
                    isoValveOpen[n] = true;
                }
                else if (valueCount <= 99 && valueCount >= 0)
                {
                    print("should update");
                    ValueText.text = valueCount.ToString();
                    DisplayText.text = Initial;
                }
                else
                {
                    ValueText.text = "0";
                    DisplayText.text = Initial;
                }
            }
        }
    

        public void updateIsolationVale1(int n)
        {
            updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Open", "Isolated");
        }

        public void updateIsolationVale2(int n)
        {
            updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Open", "Isolated");
        }

        public void updateIsolationVale3(int n)
        {
            updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Open", "Isolated");
        }

        public void updateIsolationVale4(int n)
        {
            updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Open", "Isolated");
        }


        void openGaugeGlassVent()
        {
            StopAllCoroutines();
            StartCoroutine(ChangeMaterial(GaugeGlass, gaugeGlassNormalMat));
        }
        
        public void guageGlassActive()
         {
        print("Glass vent can be moved now");
                GuageGlassVent = true;
         }

        public void GlassDrainFinal()
        {
            GlassDrainFinalZ = true;
        }
        public void updateGaugeGlassVent(int n)
        {
           updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Close", "Open");
        }

        public void updateGlassCommonDrain(int n)
        {
        updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Close", "Open");
        }

        public void updateGlassDrain(int n)
        {
        updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Close", "Open");
        }

        public void gaugeCBDvalveOpen()
        {
            Player.GetComponent<CharacterController>().enabled = false;
            Player.GetComponent<OVRPlayerController>().enabled = false;
            AllInstructions[2].SetActive(false);
            AllInstructions[3].SetActive(true);
             Player.transform.position = new Vector3(1.9f, -18.51016f, 2f);
            StopAllCoroutines();
            StartCoroutine(ControllerOn());
            StartCoroutine(ChangeMaterial(GlassCBD, glassCBDNormalMat));
        }
        public void updateGlassCBD(int n)
        {
            updateIsolationValve(isoValveValueCount[n], isoValves[n], isoDisplay[n], isoValue[n], n, "Closed", "Open");
        }

        IEnumerator ControllerOn()
        {
            yield return new WaitForSeconds(2);
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<OVRPlayerController>().enabled = true;
        }
        public void Quit()
        {
            Application.Quit();
        }

    }
