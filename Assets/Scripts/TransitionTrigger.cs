using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] string sceneToLoad;
    
    private Animator animator;
    private GameSingleton gameSingleton;
    private bool hasStartedPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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

        gameSingleton = GameSingleton.GetInstance();
    }

    public void TriggerAudio()
    {
        audioSource.Play();
    }

    public void TriggerTransition()
    {
        animator.SetBool("Start", true);
    }

    public void TriggerSceneChange()
    {
        gameSingleton.SceneChange(sceneToLoad);
    }
}
