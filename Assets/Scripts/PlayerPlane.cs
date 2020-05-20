using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerPlane : MonoBehaviour
{
    [Header("Fuel")]
    public int fuel;
    public int fuelMax;
    public GameObject fuelGameObject;
    public Slider fuelSlider;
    public GameObject quest;

    public GameObject[] enemies;
    
    private void Start()
    {
        fuel = 0;
        fuelSlider.maxValue = fuelMax;
    }

    private void FixedUpdate()
    {
        if (fuel >= fuelMax)
        {
            foreach (var VARIABLE in enemies)
            {
                VARIABLE.SetActive(true);
            }
            quest.SetActive(true);
        }
    }

    IEnumerator ShowFuel()
    {
        fuel += 20;
        fuelGameObject.SetActive(true);
        fuelSlider.value = fuel;
        yield return new WaitForSeconds(3f);
        if (fuel < fuelMax)
        {
            fuelGameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fuel"))
        {
            Destroy(other.gameObject);
            StartCoroutine(ShowFuel());
        }
    }
}
