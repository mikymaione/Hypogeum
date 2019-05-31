/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform lookAtTarget, positionTarget, AimPosition;

    public float smoothing = 6;
    public float mouseSensitivity = 100;
    public float clampAngle = 80;

    internal float rotY = 0;
    internal float rotX = 0;


    void Start()
    {
        switch (GB.GameType)
        {
            case GB.EGameType.Shooting:
                StartCameraAim();
                break;
            case GB.EGameType.Driving:
                break;
        }
    }

    void Update()
    {
        switch (GB.GameType)
        {
            case GB.EGameType.Shooting:
                UpdateCameraAim();
                break;
            case GB.EGameType.Driving:
                UpdateCameraCar();
                break;
        }
    }


    //////////////////////CAR//////////////////////
    private void UpdateCameraCar()
    {
        if (lookAtTarget != null && positionTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
            transform.LookAt(lookAtTarget);
        }
    }
    //////////////////////CAR//////////////////////


    //////////////////////AIM//////////////////////
    private void StartCameraAim()
    {
        var rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    private void UpdateCameraAim()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        var localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

        if (AimPosition != null)
            transform.position = AimPosition.position;

        transform.rotation = localRotation;
    }
    //////////////////////AIM//////////////////////

}