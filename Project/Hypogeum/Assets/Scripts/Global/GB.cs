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

    public enum EGameType
    {
        Driving,
        Shooting
    }

    public enum EAnimal
    {
        Eagle,
        Lion,
        Rhino,
        Shark
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


    //da testare
    public static T FindComponentInChildWithTag<T>(GameObject parent, string tag) where T : Component
    {
        var t = parent.transform;

        foreach (Transform tr in t)
            if (tr.CompareTag(tag))
                return tr.GetComponent<T>();

        return null;
    }

    public static EAnimal getRandomAnimal()
    {
        var values = System.Enum.GetValues(typeof(EAnimal));
        var random = Random.Range(0, values.Length);

        return (EAnimal)values.GetValue(random);
    }

    public static GameObject LoadAnimalCar(EAnimal? animal)
    {
        var s = "";

        switch (animal)
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
        }

        return Resources.Load($"Cars/{s}") as GameObject;
    }

    public static GameObject LoadCannon(EAnimal animal)
    {
        return Resources.Load($"Weapons/{animal.ToString()}sCannon") as GameObject;
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
        var s = SceneManager.GetActiveScene();
        var p = (name.Contains("Scenes/") ? name : $"Scenes/{name}");

        SceneManager.LoadScene(p, LoadSceneMode.Single);

        if (s.name.Equals(LobbyMName))
        {
            LobbyManager.s_Singleton.StopClient();
            LobbyManager.s_Singleton.StopServer();

            UnityEngine.Networking.NetworkServer.DisconnectAll();

            Object.Destroy(LobbyManager.s_Singleton.gameObject);
        }
    }

    public static void GotoScene(EScenes scene_name)
    {
        var name = scene_name.ToString();

        GotoSceneName(name);
    }
}