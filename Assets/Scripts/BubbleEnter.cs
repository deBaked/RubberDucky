using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEnter : MonoBehaviour
{
    private bool enteredBubble;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!enteredBubble && other.CompareTag("Player"))
        {
            other.GetComponent<DuckMovement>().canFly(true);
            enteredBubble = true;
        }
    }
}
