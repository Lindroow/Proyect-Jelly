
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
    [SerializeField] ParticleSystemForceField forceField;
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

    public int SetParticles()
    {
        particleSystem.maxParticles = 0;
        StartCoroutine("Destroy");
        return numParticles;
        
    }

    public string setType()
    {
        return particleType.ToString();
    }

    IEnumerator Destroy()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
            pointLight.DOIntensity(0, 0.5f);
        forceField.enabled = false;
            yield return new WaitForSeconds(11f);
            gameObject.SetActive(false);
        
    }
}
