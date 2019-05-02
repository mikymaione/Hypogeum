/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;

public class AutoGuida : NetworkBehaviour
{

    public float AngoloMassimoDiSterzata = 30f;
    public float CoppiaMassima = 300f;
    public float CoppiaFrenante = 30000f;

    [Tooltip("m/s")]
    public float VelocitaCritica = 10f;
    public int LimiteInferiore = 5;
    public int LimiteSuperiore = 1;

    public GameObject Ruote;

<<<<<<< HEAD
    public DrivingController drivingController;

    public InputData data = new InputData();

    private void InizializzaCamera()
    {
        var cam = Camera.main.GetComponent<cCamera>();

        var LookHere = GameObject.Find("LookHere");
        var Position = GameObject.Find("Position");

        cam.lookAtTarget = LookHere.transform;
        cam.positionTarget = Position.transform;
    }

    void Start()
=======
    private WheelCollider[] cRuote;
    private cCamera MyCamera;
    private Transform LookHere, Position;


    public override void OnStartLocalPlayer()
>>>>>>> a69381d81f546a014cd029719461ed24e1cfbfeb
    {
        MyCamera = Camera.main.GetComponent<cCamera>();
        cRuote = GetComponentsInChildren<WheelCollider>();
        LookHere = transform.Find("CameraAnchor/LookHere");
        Position = transform.Find("CameraAnchor/Position");

        SetCamera();

        for (var i = 0; i < cRuote.Length; ++i)
        {
            var ruota = cRuote[i];

            // Create wheel shapes only when needed.
            if (Ruote != null)
            {
                var ws = Instantiate(Ruote);
                ws.transform.parent = ruota.transform;
            }
        }
    }

<<<<<<< HEAD
    public void GetData(InputData inputData)
    {
        data = inputData;
=======
    private void SetCamera()
    {
        MyCamera.lookAtTarget = LookHere;
        MyCamera.positionTarget = Position;
>>>>>>> a69381d81f546a014cd029719461ed24e1cfbfeb
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            drivingController.ReadInput(data);
        }
    }


}