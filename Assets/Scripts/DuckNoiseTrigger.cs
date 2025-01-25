using UnityEngine;

public class DuckNoiseTrigger : MonoBehaviour
{
    // Quack quack quack
    [SerializeField] AudioSource audioSource;
    [SerializeField] TransitionTrigger transitionTrigger;
    
    [SerializeField] AudioClip DuckClip;
    [SerializeField] AudioClip BubbleClip;
    
    void Start()
    {
        // Quack quacky quack
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        audioSource.clip = DuckClip;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerDuckNoise()
    {
        audioSource.clip = DuckClip;
        audioSource.Play();
    }
    
    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerBubbleNoise()
    {
        audioSource.clip = BubbleClip;
        audioSource.Play();
    }

    public void MoveToTransition()
    {
        transitionTrigger.TriggerTransition();
    }
}
