using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] string sceneToLoad;
    
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
            sceneToLoad = "MainMenu";
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
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
        _gameSingleton.SceneChange(sceneToLoad);
    }
}
