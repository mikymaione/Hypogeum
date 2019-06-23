/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: Maione Michele
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScriptManager : MonoBehaviour
{

    //To change the speed in the speedometer
    private Text speedText;

    //To manage the team Health bar
    private Slider healthBar, hypeBar;

    private GameObject controlsClosed, controlsOpen, win, loss;

    private bool GameControlsVisibile = false;
    private bool HoAvutoIlTempoDiLeggere = true;

    private int NumeroMassimoTeamVistiInCampo = 0;

    internal GeneralCar generalCar;


    void Start()
    {
        //HUD
        controlsClosed = GameObject.Find("ControlsClosed");
        controlsOpen = GameObject.Find("ControlsOpen");
        controlsOpen.SetActive(GameControlsVisibile);

        speedText = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        hypeBar = GameObject.Find("HypeBar").GetComponent<Slider>();

        win = GameObject.FindGameObjectWithTag("Win");
        loss = GameObject.FindGameObjectWithTag("Loss");

        win.SetActive(false);
        loss.SetActive(false);

        setSpeed(0);
        setHealth(0, 0, 0);
        setHype(0);
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

        GestioneTeamRimastiVivi();
    }

    private void GestioneTeamRimastiVivi()
    {
        var TeamRimastiVivi = new HashSet<GB.EAnimal>();

        var cars = GameObject.FindGameObjectsWithTag("car");
        var cannons = GameObject.FindGameObjectsWithTag("Cannon");

        foreach (var car in cars)
        {
            var gc = car.GetComponent<GeneralCar>();

            if (!TeamRimastiVivi.Contains(gc.AnimalType))
                TeamRimastiVivi.Add(gc.AnimalType);
        }

        foreach (var cannon in cannons)
        {
            var sh = cannon.GetComponent<Shooting>();

            if (!TeamRimastiVivi.Contains(sh.TipoDiArma))
                TeamRimastiVivi.Add(sh.TipoDiArma);
        }

        if (TeamRimastiVivi.Count > NumeroMassimoTeamVistiInCampo)
            NumeroMassimoTeamVistiInCampo = TeamRimastiVivi.Count;

        if (NumeroMassimoTeamVistiInCampo > 1)
        {
            if (TeamRimastiVivi.Count == 1)
                win.SetActive(true);

            if (TeamRimastiVivi.Count < 2)
                StartCoroutine(EsciDalGioco());
        }
    }

    private IEnumerator EsciDalGioco()
    {
        yield return new WaitForSeconds(3);
        StopCoroutine(EsciDalGioco());

        Cursor.visible = true;
        GB.GotoScene(GB.EScenes.StartTitle);
    }

    private IEnumerator TempoDiLettura()
    {
        yield return new WaitForSeconds(0.999f);
        StopCoroutine(TempoDiLettura());
        HoAvutoIlTempoDiLeggere = true;
    }

    public void setValues()
    {
        if (generalCar != null)
        {
            setHype(generalCar.Hype);
            setHealth(0, generalCar.Max_Health, generalCar.Health);
            setSpeed(generalCar.actualSpeed);

            var vivo = (generalCar.Health > 0);

            if (!vivo)
                if (NumeroMassimoTeamVistiInCampo == 1)
                    NumeroMassimoTeamVistiInCampo = int.MaxValue;

            loss.SetActive(!vivo);
        }
    }

    private void setHype(float value)
    {
        hypeBar.minValue = 0;
        hypeBar.maxValue = 1000;
        hypeBar.value = value;
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