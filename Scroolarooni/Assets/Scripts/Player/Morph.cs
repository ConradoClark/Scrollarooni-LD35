using UnityEngine;
using System.Collections;

public class Morph : MonoBehaviour
{
    //temp
    int morphStatus = 0;
    private Soldier soldierMorph;
    private Ship shipMorph;
    public ParticleSystem morphingParticleEffect;
    private ParticleSystem.EmissionModule emissionModule;
    private bool morphing;

    void Start()
    {
        this.soldierMorph = this.GetComponentInChildren<Soldier>(true);
        this.shipMorph = this.GetComponentInChildren<Ship>(true);
        this.shipMorph.MorphInto();

        this.morphingParticleEffect = this.GetComponent<ParticleSystem>();
        emissionModule = this.morphingParticleEffect.emission;
        emissionModule.enabled = false;
    }

    void Update()
    {
        // Remove, just for testing
        if (Input.GetKeyDown(KeyCode.L) && !morphing)
        {
            if (morphStatus == 0)
            {
                //remove this too
                StartCoroutine(this.MorphIntoSoldier());
                morphStatus = 1;
            }
            else
            {
                StartCoroutine(this.MorphIntoShip());
                morphStatus = 0;
            }
        }
    }

    IEnumerator MorphIntoSoldier()
    {
        morphing = true;
        emissionModule.enabled = true;
        yield return new WaitForSeconds(morphingParticleEffect.duration);
        emissionModule.enabled = false;
        shipMorph.Deactivate();
        soldierMorph.MorphInto();
        morphing = false;
    }

    IEnumerator MorphIntoShip()
    {
        morphing = true;
        emissionModule.enabled = true;
        yield return new WaitForSeconds(morphingParticleEffect.duration);
        emissionModule.enabled = false;
        soldierMorph.Deactivate();
        shipMorph.MorphInto();
        morphing = false;
    }
}
