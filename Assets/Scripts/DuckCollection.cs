using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCollection : MonoBehaviour
{
    private GameObject playerDuck;
    private bool isFollowingDuck = false;
    
    private MotherDuckCounter motherDuckSC;
    
    private int ducklingNum;
    private Transform duckToFollow;
    private Vector3 ducklingOffset;
    
    void Start()
    {
        playerDuck = GameObject.FindGameObjectWithTag("Player");
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
        }
    }

    private void Update()
    { 
        if (isFollowingDuck) 
        {
            transform.position = Vector3.Lerp(transform.position, duckToFollow.position + ducklingOffset, Time.deltaTime );
            transform.rotation = Quaternion.Lerp(transform.rotation, duckToFollow.rotation, Time.deltaTime );
        }
    }
    
    
    
}
