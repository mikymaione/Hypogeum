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

    public GameObject Ruote, LookHere, Position;

    [Tooltip("m/s")]
    public float VelocitaCritica = 5f;
    public int LimiteInferiore = 5;
    public int LimiteSuperiore = 1;

    private WheelCollider[] cRuote;
    private cCamera MyCamera;

    public override void OnStartLocalPlayer()
    {
        MyCamera = Camera.main.GetComponent<cCamera>();
        cRuote = GetComponentsInChildren<WheelCollider>();
        LookHere = GameObject.Find("LookHere");
        Position = GameObject.Find("Position");

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

    private void SetCamera()
    {
        MyCamera.lookAtTarget = LookHere.transform;
        MyCamera.positionTarget = Position.transform;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            cRuote[0].ConfigureVehicleSubsteps(VelocitaCritica, LimiteInferiore, LimiteSuperiore);

            var angolo = AngoloMassimoDiSterzata * Input.GetAxis("Horizontal");
            var momentoTorcente = CoppiaMassima * Input.GetAxis("Vertical");

            var frenoAMano = Input.GetKey(KeyCode.X) ? CoppiaFrenante : 0;

            foreach (var ruota in cRuote)
            {
                if (ruota.transform.localPosition.z > 0)
                    ruota.steerAngle = angolo;

                if (ruota.transform.localPosition.z < 0)
                    ruota.brakeTorque = frenoAMano;

                ruota.motorTorque = momentoTorcente;

                if (Ruote)
                {
                    Quaternion q;
                    Vector3 p;
                    ruota.GetWorldPose(out p, out q);

                    var t = ruota.transform.GetChild(0);
                    t.position = p;
                    t.rotation = q;
                }
            }

            SetCamera();
        }
    }


}