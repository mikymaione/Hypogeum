/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

public struct InputData
{
    //the order depends by your definition inside unity component
    public float[] axis;
    public bool[] buttons;

    public InputData(int axesCount, int buttonsCount)
    {
        axis = new float[axesCount];
        buttons = new bool[buttonsCount];
    }

    public void Reset()
    {
        for (var i = 0; i < axis.Length; i++)
            axis[i] = 0f;

        for (var i = 0; i < buttons.Length; i++)
            buttons[i] = false;
    }
}

public class InputManager : MonoBehaviour
{
    [Range(0, 10)]
    public int axisCount;

    [Range(0, 20)]
    public int buttonsCount;

    public Controller controller;

    public void passInput(InputData data)
    {
        Debug.Log("Movement: " + data.axis[0] + ", " + data.axis[1]);
        controller.ReadInput(data);
    }

    public void refreshTracker()
    {
        var dt = GetComponent<DeviceTracker>();

        if (dt != null)
            dt.Refresh();
    }

}