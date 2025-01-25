using UnityEngine;
using VHierarchy.Libs;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] string sceneToLoad;
    
    [SerializeField] bool isTransitionOut = true;
    [SerializeField] bool canSceneChange = false;
    
    private Animator _animator;
    private float _transitionPolarity = 1f;
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

        _transitionPolarity = isTransitionOut ? 1f : -1f;

        _animator.speed = _transitionPolarity;
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        _gameSingleton = GameSingleton.GetInstance();
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
