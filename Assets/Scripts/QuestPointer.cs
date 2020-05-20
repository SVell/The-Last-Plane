using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointer : MonoBehaviour
{

    private bool isActive = true;
    public Transform target;
    public GameObject arrow;
    private void Awake()
    {
        
    }

    private void Update()
    {
        var dir = target.position - transform.position;

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isActive)
            {
                arrow.SetActive(false);
                isActive = false;
            }
            else
            {
                arrow.SetActive(true);
                isActive = true;
            }
            
        }
        
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
