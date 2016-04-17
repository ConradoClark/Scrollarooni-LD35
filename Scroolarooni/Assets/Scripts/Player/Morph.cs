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

    public delegate void MorphDelegate(MorphType morph);
    public event MorphDelegate OnMorph;

    void Start()
    {
        this.soldierMorph = this.GetComponentInChildren<Soldier>(true);
        this.shipMorph = this.GetComponentInChildren<Ship>(true);
        StartCoroutine(MorphIntoSoldier(true));

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
                StartCoroutine(this.MorphIntoShip());
                morphStatus = 1;
            }
            else
            {
                StartCoroutine(this.MorphIntoSoldier());
                morphStatus = 0;
            }
        }
    }

    IEnumerator MorphIntoSoldier(bool playEffect=true)
    {
        morphing = true;
        if (OnMorph != null)
        {
            OnMorph(MorphType.Soldier); // change before the effect, or after??
        }
        if (playEffect)
        {
            emissionModule.enabled = true;
            yield return new WaitForSeconds(morphingParticleEffect.duration);
            emissionModule.enabled = false;
        }
        shipMorph.Deactivate();
        soldierMorph.MorphInto();
        
        morphing = false;        
    }

    IEnumerator MorphIntoShip(bool playEffect = true)
    {
        morphing = true;
        if (OnMorph != null)
        {
            OnMorph(MorphType.Ship);
        }
        if (playEffect)
        {
            emissionModule.enabled = true;
            yield return new WaitForSeconds(morphingParticleEffect.duration);
            emissionModule.enabled = false;
        }
        soldierMorph.Deactivate();
        shipMorph.MorphInto();        
        morphing = false;
    }
}
