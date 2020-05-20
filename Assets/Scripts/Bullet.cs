using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool PlayerBullet;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
        
        else if (other.gameObject.CompareTag("Enemy") && PlayerBullet)
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(20);
        }
        Destroy(gameObject);

        }
}
