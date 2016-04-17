using UnityEngine;
using System.Collections;

public class SoldierWeapon : MonoBehaviour
{
    public ParticleSystem chargeParticleEffect;
    public GameObject beamPrefab;
    public Vector2 beamPrefabOffset;
    int beamStrength;
    bool charging;
    public PlatformMovement platformMovement;
    public int maximumBeamStrength;

    private ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve initialRate;
    private Vector3 particleOffset;
    void Start()
    {
        this.emission = chargeParticleEffect.emission;
        this.emission.enabled = false;
        initialRate = new ParticleSystem.MinMaxCurve(emission.rate.constantMax);
        this.particleOffset = chargeParticleEffect.transform.localPosition;
    }

    public void StartCharging()
    {
        if (!charging)
        {
            charging = true;
            chargeParticleEffect.transform.localPosition = new Vector3(this.platformMovement.IsFlipped() ? -particleOffset.x : particleOffset.x, particleOffset.y);

            this.emission.enabled = true;
            this.beamStrength = 0;
            emission.rate = new ParticleSystem.MinMaxCurve(initialRate.constantMax);
            StartCoroutine(BeginCharging());
            StartCoroutine(IncreaseParticleRate());
            StartCoroutine(FixDirection());
        }
    }

    public void Release()
    {
        this.emission.enabled = false;
        this.charging = false;
        StartCoroutine(ReleaseBurst());
    }

    IEnumerator FixDirection()
    {
        while (this.charging)
        {
            chargeParticleEffect.transform.localPosition = new Vector3(this.platformMovement.IsFlipped() ? -particleOffset.x : particleOffset.x, particleOffset.y);
            yield return 1;
        }
    }

    IEnumerator BeginCharging()
    {
        while (this.charging)
        {
            beamStrength++;
            if (beamStrength >= maximumBeamStrength) beamStrength = maximumBeamStrength;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator IncreaseParticleRate()
    {
        while (this.charging)
        {
            chargeParticleEffect.transform.localPosition = new Vector3(this.platformMovement.IsFlipped() ? -particleOffset.x : particleOffset.x, particleOffset.y);
            emission.rate = new ParticleSystem.MinMaxCurve(emission.rate.constantMax + 25);
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator ReleaseBurst()
    {
        platformMovement.BlockFlip();
        var beam = GameObject.Instantiate(beamPrefab);
        beam.transform.SetParent(this.transform, false);
        beam.transform.localPosition = new Vector3(platformMovement.IsFlipped() ? -beamPrefabOffset.x : beamPrefabOffset.x, beamPrefabOffset.y);
        beam.transform.localRotation = Quaternion.Euler(89f, 0, 0);

        float currentVelocity = 0f;
        float dir = platformMovement.IsFlipped() ? -1 : 1;
        for (int i = 0; i < beamStrength; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float beamSize = Mathf.SmoothDamp(1 * dir, beamStrength * 10 * dir, ref currentVelocity, 0.15f);
                beam.transform.localScale = new Vector3(beamSize, beam.transform.localScale.y, beam.transform.localScale.z);
                beam.transform.localRotation = Quaternion.Slerp(beam.transform.localRotation, Quaternion.Euler(0, 0, 0), 0.0015f * (j + 1) * (i + 1));
                yield return 1;
            }
        }
        
        yield return new WaitForSeconds(beamStrength / 5f);

        for (int i = 0; i < beamStrength; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                beam.transform.localRotation = Quaternion.Slerp(beam.transform.localRotation, Quaternion.Euler(89f, 0, 0), 0.0045f * (j + 1) * (i + 1));
                yield return 1;
            }
        }
        platformMovement.UnblockFlip();
        GameObject.Destroy(beam);
    }
}
