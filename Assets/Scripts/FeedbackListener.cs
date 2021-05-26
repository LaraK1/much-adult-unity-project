using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackListener : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem goodParticle;
    [SerializeField]
    private ParticleSystem badParticle;

    void Start()
    {
        Thing.PlayParticle += PlayParticle;
        Audiomanager.Instance.Play("music");
    }

    private void OnDestroy()
    {
        Thing.PlayParticle -= PlayParticle;
    }

    private void PlayParticle(bool good, Vector3 position)
    {
        if (good)
        {
            goodParticle.transform.position = position;
            goodParticle.Play();
            Audiomanager.Instance.Play("good");
        }
        
        if (!good)
        {
            badParticle.transform.position = position;
            badParticle.Play();
            Audiomanager.Instance.Play("bad");
        }
    }
}
