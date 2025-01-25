using UnityEngine;
using VHierarchy.Libs;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] string sceneToLoad;
    
    [SerializeField] bool canSceneChange = false;
    [SerializeField] bool runOnStart = false;
    
    private Animator _animator;
    private GameSingleton _gameSingleton;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (sceneToLoad.IsNullOrEmpty())
        {
            Debug.LogWarning("No scene target set in inspector!");
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        _gameSingleton = GameSingleton.GetInstance();

        if (runOnStart)
        {
            TriggerTransition();
        }
    }

    // Called from anim event
    public void TriggerAudio()
    {
        audioSource.Play();
    }

    public void TriggerTransition()
    {
        _animator.SetBool("Start", true);
    }
    
    // Called from anim event
    public void TriggerSceneChange()
    {
        if (canSceneChange)
            _gameSingleton.SceneChange(sceneToLoad);
    }
}
