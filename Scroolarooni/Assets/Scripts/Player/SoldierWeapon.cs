using UnityEngine;
using System.Collections;

public class SoldierWeapon : MonoBehaviour
{
    public ParticleSystem chargeParticleEffect;
    int beamStrength;
    bool charging;

    private ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve initialRate;

    void Start()
    {
        this.emission = chargeParticleEffect.emission;
        this.emission.enabled = false;
        initialRate = new ParticleSystem.MinMaxCurve(emission.rate.constantMax);
    }

    public void StartCharging()
    {
        if (!charging)
        {
            charging = true;
            this.emission.enabled = true;
            this.beamStrength = 0;
            emission.rate = new ParticleSystem.MinMaxCurve(initialRate.constantMax);
            StartCoroutine(BeginCharging());
            StartCoroutine(IncreaseParticleRate());
        }
    }

    public void Release()
    {
        this.emission.enabled = false;
        this.charging = false;
        StartCoroutine(ReleaseBurst());
    }

    IEnumerator BeginCharging()
    {
        while (this.charging)
        {
            beamStrength++;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator IncreaseParticleRate()
    {
        while (this.charging)
        {
            emission.rate = new ParticleSystem.MinMaxCurve(emission.rate.constantMax + 25);
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator ReleaseBurst()
    {
        yield break;
    }
}
