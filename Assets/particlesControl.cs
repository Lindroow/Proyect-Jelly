using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlesControl : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] ParticleSystemForceField[] forceField;
    [SerializeField] Material[] materials;

    [SerializeField] int numFuego, numAgua, numAire, numPlanta, numTotalParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void numParticlesSet()
    {
        numFuego = particles[0].maxParticles;
        numAgua = particles[1].maxParticles;
        numAire = particles[2].maxParticles;
        numPlanta = particles[3].maxParticles;
        numTotalParticles = numAgua+numAire+numFuego+numPlanta;
    }

    public void ActiveHability(ref bool fireHability, ref bool waterHability, ref bool airHability, ref bool plantHability)
    {
        if (numFuego >= 100 && fireHability == false)
        {
            fireHability = true;
            Debug.Log(fireHability);
        }
        if (numAgua >= 100 && waterHability == false)
        {
            waterHability = true;
            Debug.Log(waterHability);
        }
        if (numAire >= 100 && airHability == false)
        {
            airHability = true;
            Debug.Log(airHability);
            Debug.Log(numAire);
        }
        if (numPlanta >= 100 && plantHability == false)
        {
            plantHability = true;
            Debug.Log(plantHability);
        }
        
    }

       public IEnumerator airHability()
    {

        materials[2].DOColor(Color.yellow * 5, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);
        materials[2].DOColor(Color.black * 0, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);
        StopAllCoroutines();
    }

    
}
