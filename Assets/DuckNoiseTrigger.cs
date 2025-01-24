using UnityEngine;

public class DuckNoiseTrigger : MonoBehaviour
{
    // Quack quack quack
    [SerializeField] AudioSource audioSource;
    
    void Start()
    {
        // Quack quacky quack
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.loop = false;
    }

    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerDuckNoise()
    {
        audioSource.Play();
    }
}
