using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCollection : MonoBehaviour
{
    private GameObject PlayerDuck;
    private bool isFollowingDuck = false;

    [SerializeField] private Vector3 offset;
    
    void Start()
    {
        PlayerDuck = GameObject.FindGameObjectWithTag("Player");
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFollowingDuck)
        {
            isFollowingDuck = true;
        }
    }

    private void Update()
    { 
        if (isFollowingDuck) 
        {
            transform.position = Vector3.Lerp(transform.position, PlayerDuck.transform.position + offset, Time.deltaTime );
            transform.rotation = Quaternion.Lerp(transform.rotation, PlayerDuck.transform.rotation, Time.deltaTime );
        }
    }
    
    
    
}
