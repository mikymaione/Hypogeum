/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestioneMenu : MonoBehaviour
{

    public Button button_sparo, button_guida;


    void Start()
    {
        void button_sparo_click()
        {
            Play(GB.eGameType.Shooting);
        }

        void button_guida_click()
        {
            Play(GB.eGameType.Driving);
        }

        button_sparo.onClick.AddListener(button_sparo_click);
        button_guida.onClick.AddListener(button_guida_click);
    }

    private void Play(GB.eGameType gt)
    {
        GB.GameType = gt;
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
    }

}