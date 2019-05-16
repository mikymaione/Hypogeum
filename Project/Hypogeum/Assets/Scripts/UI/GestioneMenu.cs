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

    public Button button_sparo, button_guida, button_lion, button_shark, button_rhino, button_eagle;


    void Start()
    {
        Screen.fullScreen = false;

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

    private void SelezionaAnimale(Button b_animale)
    {
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

            coloraButton(s, Color.green);
        }

        selezionaButton(b_animale);

        if (b_animale.Equals(button_eagle))
            GB.Animal = GB.EAnimal.Eagle;
        else if (b_animale.Equals(button_rhino))
            GB.Animal = GB.EAnimal.Rhino;
        else if (b_animale.Equals(button_lion))
            GB.Animal = GB.EAnimal.Lion;
        else if (b_animale.Equals(button_shark))
            GB.Animal = GB.EAnimal.Shark;
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