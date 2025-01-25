using UnityEngine;
using UnityEngine.Serialization;

public class DuckNoiseTrigger : MonoBehaviour
{
    // Quack quack quack
    [SerializeField] AudioSource audioSource;
    [SerializeField] TransitionTrigger transitionTrigger;
    
    [SerializeField] AudioClip duckClip;
    [SerializeField] AudioClip bubbleClip;
    
    void Start()
    {
        // Quack quacky quack
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        audioSource.clip = duckClip;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerDuckNoise()
    {
        audioSource.clip = duckClip;
        audioSource.Play();
    }
    
    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerBubbleNoise()
    {
        audioSource.clip = bubbleClip;
        audioSource.Play();
    }

    public void MoveToTransition()
    {
        transitionTrigger.TriggerTransition();
    }
}
