using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Transform startPos;
    public AIDestinationSetter ai;
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ai.target = other.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ai.target = startPos;
    }
}
