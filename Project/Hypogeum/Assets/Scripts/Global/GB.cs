/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GB
{

    public enum ECoin
    {
        Reason,
        Instinct
    }

    public enum EGameType
    {
        Driving,
        Shooting
    }

    public enum EAnimal
    {
        Eagle = 0,
        Lion = 1,
        Rhino = 2,
        Shark = 3
    }

    //Mantenere ordine corretto, serve per tornare indietro alla scende precedente con il tasto ESC
    public enum EScenes
    {
        StartTitle,
        PlayerSelection,
        LobbyM,
        Game
    }

    internal static EGameType? GameType;
    internal static EAnimal? Animal;


    internal static void SetCannonsPositions()
    {
        var weapons = GameObject.FindGameObjectsWithTag("Cannon");

        if (weapons.Length > 0)
        {
            var cars = GameObject.FindGameObjectsWithTag("car");

            if (cars.Length > 0)
                foreach (var w in weapons)
                {
                    var razzaCannone = w.name.Replace("Cannon", "");

                    foreach (var c in cars)
                    {
                        var razzaAuto = c.name.Replace("Car", "");

                        if (razzaAuto.Equals(razzaCannone))
                        {
                            var cannonPositionMarker = c.transform.Find("CannonPosition");
                            w.transform.position = cannonPositionMarker.position;
                        }
                    }
                }
        }
    }

    public static void PlayCarEngine(AudioSource carAudioSource, float generalCar_actualSpeed)
    {
        if (carAudioSource != null)
        {
            carAudioSource.volume = Mathf.Min(generalCar_actualSpeed / 40, 1);

            if (!carAudioSource.isPlaying && carAudioSource.volume > 0)
                carAudioSource.Play();
        }
    }

    public static RenderTexture getAnimalRenderTexture(EAnimal animal, EGameType gameType)
    {
        var tipo = (gameType == EGameType.Shooting ? "Shooter" : "");
        var percorso = $"Radar/RadarRenderTexture{animal}s{tipo}";
        var texture = Resources.Load(percorso) as RenderTexture;

        return texture;
    }

    public static void DistruggiOggetti(Object[] oggetti)
    {
        foreach (var o in oggetti)
            Object.Destroy(o);
    }

    public static void AbilitaButton(UnityEngine.UI.Button b, bool abilitato)
    {
        var img = b.GetComponent<UnityEngine.UI.Image>();
        img.color = (abilitato ? Color.white : Color.gray);

        b.enabled = abilitato;
    }

    //da testare
    public static T FindComponentInChildWithTag<T>(GameObject parent, string tag) where T : Component
    {
        var t = parent.transform;

        foreach (Transform tr in t)
            if (tr.CompareTag(tag))
                return tr.GetComponent<T>();

        return null;
    }

    public static double ms_to_kmh(float meters_per_seconds)
    {
        return meters_per_seconds * 3.6;
    }

    public static double ms_to_mph(float meters_per_seconds)
    {
        return meters_per_seconds * 2.237;
    }

    public static T getRandomEnum<T>()
    {
        var values = System.Enum.GetValues(typeof(T));
        var random = Random.Range(0, values.Length);

        return (T)values.GetValue(random);
    }

    public static string GetCarNameInGameFromAnimalValue(GB.EAnimal animal)
    {
        string carName = "";

        switch (animal)
        {
            case EAnimal.Rhino:
                carName = "RhinosCar(Clone)";
                break;
            case EAnimal.Eagle:
                carName = "EaglesCar(Clone)";
                break;
            case EAnimal.Lion:
                carName = "LionsCar(Clone)";
                break;
            case EAnimal.Shark:
                carName = "SharksCar(Clone)";
                break;
            default:
                throw new System.NotImplementedException();
        }

        return carName;
    }

    public static GameObject LoadAnimalCar(EAnimal a)
    {
        var s = "";

        switch (a)
        {
            case EAnimal.Rhino:
                s = "RhinosCar";
                break;
            case EAnimal.Eagle:
                s = "EaglesCar";
                break;
            case EAnimal.Lion:
                s = "LionsCar";
                break;
            case EAnimal.Shark:
                s = "SharksCar";
                break;
            default:
                throw new System.NotImplementedException();
        }

        return Resources.Load($"Cars/{s}") as GameObject;
    }

    public static int GetTeamAttack(EAnimal a)
    {
        switch (a)
        {
            case EAnimal.Rhino:
                return 7;
            case EAnimal.Eagle:
                return 5;
            case EAnimal.Lion:
                return 9;
            case EAnimal.Shark:
                return 7;
            default:
                throw new System.NotImplementedException();
        }
    }

    public static GameObject LoadCannon(EAnimal animal)
    {
        return Resources.Load($"Weapons/{animal.ToString()}sCannon") as GameObject;
    }

    public static GameObject LoadCoin(ECoin coin)
    {
        return Resources.Load($"Coins/Coin{coin.ToString()}") as GameObject;
    }

    public static void SwitchFullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreen = true;
        }
    }

    public static void GoBackToScene()
    {
        var s = SceneManager.GetActiveScene();
        var i = s.buildIndex - 1;

        if (i < 0)
        {
            Application.Quit();
        }
        else
        {
            var e = (EScenes)i;
            GotoScene(e);
        }
    }

    public static void GotoSceneName(string name)
    {
        var LobbyMName = EScenes.LobbyM.ToString();
        var p = (name.Contains("Scenes/") ? name : $"Scenes/{name}");

        if (LobbyManager.s_Singleton != null)
        {
            var MostraLobby = name.Equals(LobbyMName);

            LobbyManager.s_Singleton.Restart(MostraLobby);
        }

        SceneManager.LoadScene(p, LoadSceneMode.Single);
    }

    public static void GotoScene(EScenes scene_name)
    {
        var name = scene_name.ToString();

        GotoSceneName(name);
    }


}