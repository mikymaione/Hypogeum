/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: Maione Michele
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.UI;

public class HudScriptManager : MonoBehaviour
{

    //To change the speed in the speedometer
    private Text speedText;
    //To manage the team Health bar
    private Slider healthBar;


    void Start()
    {
        speedText = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        setSpeed(0);
        setHealth(0, 0, 0);
    }

    public void setValues(GeneralCar generalCar)
    {
        if (generalCar != null)
        {
            setHealth(0, generalCar.Max_Health, generalCar.Health);
            setSpeed(generalCar.actualSpeed);
        }
    }

    private void setHealth(int min, int max, int value)
    {
        healthBar.minValue = min;
        healthBar.maxValue = max;
        healthBar.value = value;
    }

    private void setSpeed(float value)
    {
        speedText.text = value.ToString("0");
    }


}