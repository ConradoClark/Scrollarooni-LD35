using UnityEngine;
using System.Collections;

public class StopParticleEmission : MonoBehaviour
{
    public new ParticleSystem particleSystem;
    ParticleSystem.EmissionModule emission;
    public float delayInSeconds;
    void Start()
    {
        this.emission = particleSystem.emission;
        this.StartCoroutine(DelayAndStop());
    }

    IEnumerator DelayAndStop()
    {
        yield return new  WaitForSeconds(delayInSeconds);
        this.emission.enabled = false;
    }
}
