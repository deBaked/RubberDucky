using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEnter : MonoBehaviour
{
    private bool enteredBubble;
    public GameObject fakeBubble;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!enteredBubble && other.CompareTag("Player"))
        {
            other.GetComponent<DuckMovement>().canFly(true);
            other.GetComponent<DuckMovement>().Bubble.SetActive(true);
            fakeBubble.SetActive(false);
            enteredBubble = true;
        }
    }
}
