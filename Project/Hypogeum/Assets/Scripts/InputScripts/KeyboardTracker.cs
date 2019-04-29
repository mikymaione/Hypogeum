using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyboardTracker : DeviceTracker
{
    //keycode array of the buttons the keyboard has to keep track of
    public KeyCode[] buttonKeys;
    public AxisKeys[] axisKeys;

    /* this is the unity built-in reset feature (in transform),
     * we use it to match the input manager  axes and buttons count,
     * awake() is not an option because it is called only whe the game's already started */ 
    void Reset() {
        im = (InputManager)GetComponent(typeof(InputManager));
        axisKeys = new AxisKeys[im.axisCount];
        buttonKeys = new KeyCode[im.buttonsCount];
    }

    public override void Refresh() {
        im = (InputManager)GetComponent(typeof(InputManager));

        //create 2 temp arrays to save the old axisKeys and buttonKeys
        KeyCode[] newButtons = new KeyCode[im.buttonsCount];
        AxisKeys[] newAxis = new AxisKeys[im.axisCount];

        if (buttonKeys != null) {
            for (int i = 0; i < Math.Min(newButtons.Length, buttonKeys.Length); i++)
            {
                newButtons[i] = buttonKeys[i];
            }
        }
        buttonKeys = newButtons;

        if (axisKeys != null) {
            for (int i = 0; i < Math.Min(newAxis.Length, axisKeys.Length); i++)
            {
                newAxis[i] = axisKeys[i];
            }
        }
        axisKeys = newAxis;
    }

    // Update is called once per frame
    void Update()
    {
        //check for inputs, if inputs detected, set newData to true
        //populate the InputData to pass to the InputManager

        //checking if axiskey are pressed
        for (int i = 0; i < axisKeys.Length; i++)
        {
            float val = 0f;
            if (Input.GetKey(axisKeys[i].positive)) {
                val += 1f;
                newData = true;
            }
            if (Input.GetKey(axisKeys[i].negative)) {
                val -= 1f;
                newData = true;
            }
            data.axis[i] = val;
        }
        //checking if buttons are pressed
        for (int i = 0; i < buttonKeys.Length; i++)
        {
            if (Input.GetKey(buttonKeys[i])) {
                data.buttons[i] = true;
                newData = true;
            }
        }
        //if player has given a new input pass it to the IM
        if (newData) {
            im.passInput(data);
            newData = false;
            data.Reset();
        }

    }
}

//easier managing the axis with a keyboard
[System.Serializable]
//can be seen in the inspector
public struct AxisKeys {

    //the positive button of the axis
    public KeyCode positive;
    //negative one
    public KeyCode negative;
}
