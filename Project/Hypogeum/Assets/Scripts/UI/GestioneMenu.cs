/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.UI;

public class GestioneMenu : MonoBehaviour
{

    public Button button_fullscreen, button_sparo, button_guida, button_lion, button_shark, button_rhino, button_eagle;
    public GameObject car_lion, car_rhino, car_shark, car_eagle;

    private GameObject[] cars;

    void Start()
    {
        cars = new GameObject[] { car_lion, car_rhino, car_shark, car_eagle };
        HideCars(null);

        button_fullscreen?.onClick.AddListener(() =>
        {
            Screen.fullScreen = !Screen.fullScreen;
        });


        button_eagle?.onClick.AddListener(() =>
        {
            SelezionaAnimale(button_eagle);
        });
        button_rhino?.onClick.AddListener(() =>
        {
            SelezionaAnimale(button_rhino);
        });
        button_shark?.onClick.AddListener(() =>
        {
            SelezionaAnimale(button_shark);
        });
        button_lion?.onClick.AddListener(() =>
        {
            SelezionaAnimale(button_lion);
        });


        button_sparo?.onClick.AddListener(() =>
        {
            Play(GB.EGameType.Shooting);
        });

        button_guida?.onClick.AddListener(() =>
        {
            Play(GB.EGameType.Driving);
        });
    }

    private void HideCars(GameObject tranne)
    {
        car_eagle.active = (car_eagle.Equals(tranne));
        car_rhino.active = (car_rhino.Equals(tranne));
        car_shark.active = (car_shark.Equals(tranne));
        car_lion.active = (car_lion.Equals(tranne));
    }

    private void SelezionaAnimale(Button b_animale)
    {
        //inner function
        void coloraButton(Button b, Color c)
        {
            b.GetComponent<Image>().color = c;
        }

        void selezionaButton(Button s)
        {
            coloraButton(button_eagle, Color.white);
            coloraButton(button_lion, Color.white);
            coloraButton(button_rhino, Color.white);
            coloraButton(button_shark, Color.white);

            coloraButton(s, Color.gray);
        }
        //inner function

        selezionaButton(b_animale);

        if (b_animale.Equals(button_eagle))
        {
            GB.Animal = GB.EAnimal.Eagle;
            HideCars(car_eagle);
        }
        else if (b_animale.Equals(button_rhino))
        {
            GB.Animal = GB.EAnimal.Rhino;
            HideCars(car_rhino);
        }
        else if (b_animale.Equals(button_lion))
        {
            GB.Animal = GB.EAnimal.Lion;
            HideCars(car_lion);
        }
        else if (b_animale.Equals(button_shark))
        {
            GB.Animal = GB.EAnimal.Shark;
            HideCars(car_shark);
        }
    }

    private void Play(GB.EGameType gt)
    {
        if (GB.Animal.HasValue)
        {
            GB.GameType = gt;
            GB.GotoScene("LobbyM");
        }
    }


}