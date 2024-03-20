
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLot : MonoBehaviour
{
    [SerializeField] int numParticles;
    private enum ParticleType { Fuego, Agua, Aire, Planta}
    [SerializeField] Mesh[] particleMesh;
    [SerializeField] Material[] particleMaterial;

    [SerializeField] ParticleType particleType;

    private ParticleSystem particleSystem;
    [SerializeField] Light pointLight;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        numParticles = Random.RandomRange(10, 30);
        particleSystem.maxParticles = numParticles;

        switch(particleType)
        {
            case ParticleType.Fuego:
                particleSystem.GetComponent<ParticleSystemRenderer>().mesh = particleMesh[0];
                particleSystem.GetComponent<ParticleSystemRenderer>().material = particleMaterial[0];
                pointLight.color = Color.red;
                break;
            case ParticleType.Agua:
                particleSystem.GetComponent<ParticleSystemRenderer>().mesh = particleMesh[1];
                particleSystem.GetComponent<ParticleSystemRenderer>().material = particleMaterial[1];
                pointLight.color = Color.blue;
                break;
            case ParticleType.Aire:
                particleSystem.GetComponent<ParticleSystemRenderer>().mesh = particleMesh[2];
                particleSystem.GetComponent<ParticleSystemRenderer>().material = particleMaterial[2];
                pointLight.color = Color.yellow;
                break;
            case ParticleType.Planta:
                particleSystem.GetComponent<ParticleSystemRenderer>().mesh = particleMesh[3];
                particleSystem.GetComponent<ParticleSystemRenderer>().material = particleMaterial[3];
                pointLight.color = Color.green;
                break;
        }
    }

    private void Update()
    {
        if (particleSystem.maxParticles == 0)
        {
            StartCoroutine("Destroy");
        }
    }

    public int SetParticles()
    {
        particleSystem.maxParticles = 0;
        return numParticles;
        
    }

    public string setType()
    {
        return particleType.ToString();
    }

    IEnumerator Destroy()
    {
            particleSystem.startLifetime = 0.5f;
            pointLight.DOIntensity(0, 0.5f);
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        
    }
}
