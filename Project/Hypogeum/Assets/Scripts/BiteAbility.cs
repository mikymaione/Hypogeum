using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/BiteAbility")]
public class BiteAbility : Ability
{
    public int damage = 1;
    public float range = 30f;
    //in Newtons
    public float hitForce = 100f;

    private BiteTriggerable biteTriggerable;

    public override void Initialize(GameObject obj)
    {
        biteTriggerable = obj.GetComponent<BiteTriggerable>();
        biteTriggerable.Initialize();

        biteTriggerable.biteDamage = damage;
        biteTriggerable.range = range;
        biteTriggerable.hitForce = hitForce;
    }

    public override void TriggerAbility()
    {
        biteTriggerable.Bite();
    }
}
