using UnityEngine;

public class BubbleNoiseTrigger : MonoBehaviour
{
    // Quack quack quack
    [SerializeField] AudioSource audioSource;
    private Animator _animator;
    
    void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        // Quack quacky quack
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerBubbleNoise()
    {
        audioSource.Play();
    }
    
    public void TriggerAnim()
    {
        _animator.SetBool("Start", true);
    }
}
