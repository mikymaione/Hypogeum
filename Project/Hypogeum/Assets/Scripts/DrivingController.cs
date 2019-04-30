using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingController : Controller
{
    public float AngoloMassimoDiSterzata = 30f;
    public float CoppiaMassima = 300f;
    public float CoppiaFrenante = 30000f;

    public GameObject Ruote;

    [Tooltip("m/s")]
    public float VelocitaCritica = 10f;
    public int LimiteInferiore = 5;
    public int LimiteSuperiore = 1;

    private WheelCollider[] cRuote;

    public override void ReadInput(InputData data)
    {
        cRuote[0].ConfigureVehicleSubsteps(VelocitaCritica, LimiteInferiore, LimiteSuperiore);

        //horizontal
        var angolo = AngoloMassimoDiSterzata * data.axis[1];
        //vertical
        var momentoTorcente = CoppiaMassima * data.axis[0];

        var frenoAMano = data.buttons[0] ? CoppiaFrenante : 0;

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
    }
}
