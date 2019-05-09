/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{
    //hardcoded input key
    public string abilityButtonAxisName = "Fire2";
    public Image darkMask;
    public Text coolDownTextDisplay;

    private Ability ability;
    //it should be the gameObject from where the ability comes from
    private GameObject ilMezzo;

    private Image myButtonImage;
    private AudioSource abilityAudioSource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize(ability, ilMezzo);
    }

    public void Initialize(Ability selectedAbility, GameObject ilMezzo)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilityAudioSource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability.aSprite;
        darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        ability.Initialize(ilMezzo);
        AbilityReady();
    }

    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCD.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        abilityAudioSource.clip = ability.aSound;
        abilityAudioSource.Play();
        ability.TriggerAbility();
    }

    // Update is called once per frame
    void Update()
    {
        var coolDownComplete = (Time.time > nextReadyTime);

        if (coolDownComplete)
        {
            AbilityReady();

            if (Input.GetButtonDown(abilityButtonAxisName))
                ButtonTriggered();
        }
        else //if ability cooldown is not complete
        {            
            CoolDown();
        }
    }

}