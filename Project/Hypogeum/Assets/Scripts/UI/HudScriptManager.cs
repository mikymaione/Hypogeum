/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: Maione Michele
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudScriptManager : MonoBehaviour
{

    //To change the speed in the speedometer
    private Text speedText;

    //To manage the team Health bar
    private Slider healthBar;

    private GameObject controlsClosed;
    private GameObject controlsOpen;

    private bool GameControlsVisibile = false;
    private bool HoAvutoIlTempoDiLeggere = true;


    void Start()
    {
        //HUD
        controlsClosed = GameObject.Find("ControlsClosed");
        controlsOpen = GameObject.Find("ControlsOpen");
        controlsOpen.SetActive(GameControlsVisibile);

        speedText = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        setSpeed(0);
        setHealth(0, 0, 0);
    }

    void Update()
    {
        //HUD
        if (Input.GetKey(KeyCode.F1) && HoAvutoIlTempoDiLeggere)
        {
            HoAvutoIlTempoDiLeggere = false;

            controlsOpen.SetActive(!GameControlsVisibile);
            controlsClosed.SetActive(GameControlsVisibile);
            GameControlsVisibile = !GameControlsVisibile;

            StartCoroutine(TempoDiLettura());
        }
    }

    private IEnumerator TempoDiLettura()
    {
        yield return new WaitForSeconds(0.999f);
        StopCoroutine(TempoDiLettura());
        HoAvutoIlTempoDiLeggere = true;
    }

    public void setValues(GeneralCar generalCar)
    {
        if (generalCar != null)
        {
            setHealth(0, generalCar.Max_Health, generalCar.Health);
            setSpeed(generalCar.actualSpeed);
        }
    }

    private void setHealth(int min, int max, float value)
    {
        healthBar.minValue = min;
        healthBar.maxValue = max;
        healthBar.value = value;
    }

    private void setSpeed(float value)
    {
        //value: m/s
        var realspeed = System.Convert.ToInt16(GB.ms_to_mph(value));

        if (realspeed == 0 || realspeed % 2 == 0)
            speedText.text = realspeed.ToString("0");
    }


}