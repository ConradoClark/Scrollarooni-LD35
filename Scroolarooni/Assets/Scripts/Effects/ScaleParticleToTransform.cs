using UnityEngine;
using System.Collections;

public class ScaleParticleToTransform : MonoBehaviour
{
    public Transform reference;
    public ParticleSystem ParticleSystem;

    void Start()
    {
    }

    void Update()
    {
        var module = this.ParticleSystem.velocityOverLifetime;
                
        ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();
        rate.constantMax = reference.lossyScale.magnitude * 10f * reference.lossyScale.normalized.x;
        module.x = rate;
    }
}
