using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public abstract class DeviceTracker : MonoBehaviour
{
    protected InputManager im;
    protected InputData data;
    protected bool newData;

    void  Awake() {
        im = (InputManager)GetComponent(typeof(InputManager));
        data = new InputData(im.axisCount, im.buttonsCount);
    }

}