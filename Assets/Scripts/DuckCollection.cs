using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCollection : MonoBehaviour
{
    private GameObject playerDuck;
    private DuckMovement duckMovementSC;
    private bool isFollowingDuck = false;
    private bool isShocked;

    [SerializeField] ParticleSystem shockVFX;
    
    private MotherDuckCounter motherDuckSC;
    private Animator ducklingAnimator;
    
    private int ducklingNum;
    private Transform duckToFollow;
    private Vector3 ducklingOffset;

    private AudioSource collectAudio;


    void Start()
    {
        playerDuck = GameObject.FindGameObjectWithTag("Player");
        collectAudio = GetComponent<AudioSource>();
        ducklingAnimator = GetComponent<Animator>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFollowingDuck)
        {
            // increment data on mother duck so all ducklings have the same reference point
            motherDuckSC = other.GetComponent<MotherDuckCounter>();
            motherDuckSC.UpdateDucklingCount(this.transform);
            
            // keep store of personal ducking data as each duckling picked up will need its on number and distance from next unit
            ducklingNum = motherDuckSC.ducklingCount;
            ducklingOffset = motherDuckSC.ducklingDisUnit;
            duckToFollow = motherDuckSC.Ducks[ducklingNum-1];
            
            isFollowingDuck = true;
            collectAudio.Play();
            ducklingAnimator.SetTrigger("Collected");
        }
    }

    private void Update()
    { 
        if (isFollowingDuck) 
        {
            transform.position = Vector3.Lerp(transform.position, duckToFollow.position + ducklingOffset, Time.deltaTime );
            transform.rotation = Quaternion.Lerp(transform.rotation, duckToFollow.rotation, Time.deltaTime );
        }
        if (!isShocked && duckMovementSC.farted)
        {
            isShocked = true;
            //shockVFX.Play();
            StartCoroutine(enableShock());
        }
    }

    IEnumerator enableShock()
    {
        yield return new WaitForSeconds(0.4f);
        isShocked = false;
    }




}
