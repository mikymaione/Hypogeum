using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{

    public string abilityButtonAxisName = "Bite";
    public Image darkMask;
    public Text coolDownTextDisplay;

    [SerializeField] private Ability ability;
    //it should be the gameObject from where the ability comes from
    [SerializeField] private GameObject ilMezzo;

    private Image myButtonImage;
    private AudioSource abilityAudioSource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        Initialize(ability, ilMezzo);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
