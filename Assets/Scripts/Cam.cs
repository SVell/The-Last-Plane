using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public float smoothSpeed = 10f;

    private void Update()
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
