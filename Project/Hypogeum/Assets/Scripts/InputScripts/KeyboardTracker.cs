/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using System;

//easier managing the axis with a keyboard
//can be seen in the inspector
[Serializable]  
public struct AxisKeys
{
    //the positive button of the axis
    public KeyCode positive;
    //negative one
    public KeyCode negative;

}

public class KeyboardTracker : DeviceTracker
{
    //keycode array of the buttons the keyboard has to keep track of
    public KeyCode[] buttonKeys;
    public AxisKeys[] axisKeys;

    /* this is the unity built-in reset feature (in transform),
     * we use it to match the input manager  axes and buttons count,
     * awake() is not an option because it is called only whe the game's already started */
    void Reset()
    {
        im = GetComponent<InputManager>();
        axisKeys = new AxisKeys[im.axisCount];
        buttonKeys = new KeyCode[im.buttonsCount];
    }

    public override void Refresh()
    {
        im = GetComponent<InputManager>();

        //create 2 temp arrays to save the old axisKeys and buttonKeys
        var newButtons = new KeyCode[im.buttonsCount];
        var newAxis = new AxisKeys[im.axisCount];

        if (buttonKeys != null)
            for (var i = 0; i < Math.Min(newButtons.Length, buttonKeys.Length); i++)
                newButtons[i] = buttonKeys[i];

        buttonKeys = newButtons;

        if (axisKeys != null)
            for (var i = 0; i < Math.Min(newAxis.Length, axisKeys.Length); i++)
                newAxis[i] = axisKeys[i];

        axisKeys = newAxis;
    }

    // Update is called once per frame
    void Update()
    {
        //check for inputs, if inputs detected, set newData to true
        //populate the InputData to pass to the InputManager

        //checking if axiskey are pressed
        for (var i = 0; i < axisKeys.Length; i++)
        {
            var val = 0f;

            if (Input.GetKey(axisKeys[i].positive))
            {
                val += 1f;
                newData = true;
            }

            if (Input.GetKey(axisKeys[i].negative))
            {
                val -= 1f;
                newData = true;
            }

            data.axis[i] = val;
        }

        //checking if buttons are pressed
        for (var i = 0; i < buttonKeys.Length; i++)
            if (Input.GetKey(buttonKeys[i]))
            {
                data.buttons[i] = true;
                newData = true;
            }

        //if player has given a new input pass it to the IM
        if (newData)
        {
            im.passInput(data);
            newData = false;
            data.Reset();
        }
    }

}