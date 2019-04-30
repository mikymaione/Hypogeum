using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Range(0,10)]
    public int axisCount;
    [Range(0,20)]
    public int buttonsCount;

    public Controller controller;

    public void passInput(InputData data) {
        Debug.Log("Movement: " + data.axis[0] + ", " + data.axis[1]);
        controller.ReadInput(data);
    }

    public void refreshTracker() {
        
        DeviceTracker dt = (DeviceTracker)GetComponent(typeof(DeviceTracker));
        if (dt != null)
        {
            dt.Refresh();
        }
    }
}

public struct InputData {

    //the order depends by your definition inside unity component
    public float[] axis;
    public bool[] buttons;

    public InputData(int axesCount, int buttonsCount) {
        axis = new float[axesCount];
        buttons = new bool[buttonsCount];
    }

    public void Reset() {

        int i;
        for (i = 0; i < axis.Length; i++) {
            axis[i] = 0f;
        }
        for (i = 0; i < buttons.Length; i++) {
            buttons[i] = false;
        }
    }
}
