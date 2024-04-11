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
        forceField[2].gravityFocus = 0.03f;
        forceField[2].startRange = 0f;
        //forceField[2].directionY = 50f;

        materials[2].DOColor(Color.yellow * 5, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);

        forceField[2].startRange = 1f;
        FocusSize();
        //forceField[2].directionY = 0f;
        materials[2].DOColor(Color.black * 0, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);
        StopAllCoroutines();
    }

    public IEnumerator FireHability()
    {
        forceField[0].gravityFocus = 0.03f;
        forceField[0].startRange = 0f;
        //forceField[2].directionY = 50f;

        materials[0].DOColor(Color.red * 5, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);

        forceField[0].startRange = 1f;
        FocusSize();
        //forceField[2].directionY = 0f;
        materials[0].DOColor(Color.black * 0, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);
        StopAllCoroutines();
    }

    public void WaterHabilityActive()
    {
            forceField[1].gravityFocus = 0f;
            forceField[1].startRange = 0f;
            forceField[1].directionY = -50f;

            materials[1].DOColor(Color.blue * 5, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
        
    }

    public void WaterHabilityInactive()
    {
        forceField[1].startRange = 1f;
        FocusSize();
        forceField[1].directionY = 0f;
        materials[1].DOColor(Color.black * 0, "_EmissionColor", .5f).SetEase(Ease.InOutSine);
    }

    public void FocusSize()
    {
        for (int i = 0; i < forceField.Length; i++) 
        {
            forceField[i].gravityFocus = (numTotalParticles + i * 80) * 0.0003f;
        }
    }
}
