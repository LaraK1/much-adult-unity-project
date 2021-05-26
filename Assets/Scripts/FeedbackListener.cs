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
        Thing.PlayParticle += PlayFeedback;
        Audiomanager.Instance.Play("music");
    }

    private void OnDestroy()
    {
        Thing.PlayParticle -= PlayFeedback;
    }

    /// <summary>Plays particles and sound.</summary>
    /// <param name="good">True: Good / white feedback; False: Bad / red feedback</param>
    /// <param name="position">New Position for the partiles to play..</param>
    private void PlayFeedback(bool good, Vector3 position)
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
