/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GestioneMenu : MonoBehaviour
{

    public Button button_fullscreen, button_go;

    public Button button_sparo, button_guida;
    public Button button_lion, button_shark, button_rhino, button_eagle;

    public GameObject car_lion, car_rhino, car_shark, car_eagle;

    private List<sAnimalMapping> buttons_animal_mapping;


    private struct sAnimalMapping
    {
        internal GameObject car;
        internal Button button;
        internal GB.EAnimal animal;
    }


    void Start()
    {        
        GB.Animal = null;
        GB.GameType = null;

        buttons_animal_mapping = new List<sAnimalMapping>() {
            new sAnimalMapping() {
                car = car_lion,
                button = button_lion,
                animal = GB.EAnimal.Lion
            },
            new sAnimalMapping() {
                car = car_rhino,
                button = button_rhino,
                animal = GB.EAnimal.Rhino
            },
            new sAnimalMapping() {
                car = car_shark,
                button = button_shark,
                animal = GB.EAnimal.Shark
            },
            new sAnimalMapping() {
                car = car_eagle,
                button = button_eagle,
                animal = GB.EAnimal.Eagle
            }
        };


        SelezionaAnimale();

        button_fullscreen?.onClick.AddListener(() =>
        {
            Screen.fullScreen = !Screen.fullScreen;
        });


        button_eagle?.onClick.AddListener(() =>
        {
            SelezionaAnimale(GB.EAnimal.Eagle);
        });
        button_rhino?.onClick.AddListener(() =>
        {
            SelezionaAnimale(GB.EAnimal.Rhino);
        });
        button_shark?.onClick.AddListener(() =>
        {
            SelezionaAnimale(GB.EAnimal.Shark);
        });
        button_lion?.onClick.AddListener(() =>
        {
            SelezionaAnimale(GB.EAnimal.Lion);
        });


        button_sparo?.onClick.AddListener(() =>
        {
            SelezionaModalitaGioco(GB.EGameType.Shooting);
        });

        button_guida?.onClick.AddListener(() =>
        {
            SelezionaModalitaGioco(GB.EGameType.Driving);
        });


        button_go?.onClick.AddListener(() =>
        {
            if (GB.Animal.HasValue && GB.GameType.HasValue)
                GB.GotoScene(GB.EScenes.LobbyM);
        });
    }

    private void SelezionaModalitaGioco(GB.EGameType m)
    {
        GB.AbilitaButton(button_guida, m != GB.EGameType.Driving);
        GB.AbilitaButton(button_sparo, m != GB.EGameType.Shooting);

        GB.GameType = m;

        AbilitaButtonGo();
    }

    private void AbilitaButtonGo()
    {
        var abilitato = (GB.Animal.HasValue && GB.GameType.HasValue);

        GB.AbilitaButton(button_go, abilitato);
    }

    private void SelezionaAnimale(GB.EAnimal? selectedAnimal = null)
    {
        foreach (var el in buttons_animal_mapping)
        {
            var selezionato = (selectedAnimal == el.animal);

            GB.AbilitaButton(el.button, !selezionato);

            el.car.active = selezionato;
        }

        GB.Animal = selectedAnimal;
        AbilitaButtonGo();
    }


}