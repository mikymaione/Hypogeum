using System.Collections;
using System.Collections.Generic;
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
        bool coolDownComplete = (Time.time > nextReadyTime);

        if (coolDownComplete)
        {
            AbilityReady();
            if (Input.GetButtonDown(abilityButtonAxisName))
                ButtonTriggered();
        }
        //if ability cooldown is not complete
        else
        {
            CoolDown();
        }
        
    }
}
