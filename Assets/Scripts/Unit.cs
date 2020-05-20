using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int hp = 100;
    
    private Rigidbody2D rb;
    public GameObject player;
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;
    public Transform firePoint;

    public bool canShoot;
    public int ammo;
    public int maxAmmo;
    
    private float shootTime;
    public float shootTimeOffset = 1.5f;
    private float attackTime;
    public float attackTimeOffset = 2.5f;
    public bool hasPistol;
    public bool HasRifle;
    public bool hasAxe;

    private Animator anim;

    public LayerMask whatIsPlayer;

    private Vector2 curPos;
    private Vector2 lastPos;

    public GameObject blood;
    public AudioSource shoot;
    public AudioSource hit;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
        //canShoot = true;
        
        if (HasRifle)
        {
            maxAmmo = 30;
            shootTimeOffset = 0.3f;
        }

        if (hasPistol)
        {
            maxAmmo = 7;
            shootTimeOffset = 0.7f;
        }
    }


    private void Update()
    {
        // LookAt Player.
            var dir = player.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            
            curPos = transform.position;
            if(curPos == lastPos) {
               anim.SetBool("Walk",false);
            }
            else if(anim.GetBool("Attack") != true)
            {
                anim.SetBool("Walk", true);
            }
            lastPos = curPos;

            StartCoroutine(ShootStop());
            shootTime -= Time.deltaTime;
            attackTime -= Time.deltaTime;

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
    }


    IEnumerator ShootStop()
    {
        if (ammo >= maxAmmo)
        {
            canShoot = false;
            yield return new WaitForSeconds(3f);
            ammo = 0;
            canShoot = true;
        }
    }
    
    IEnumerator AxeAttack()
    {
        Debug.Log("Hit");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(firePoint.position, 0.83f, whatIsPlayer);
        anim.SetBool("Attack",true);
        foreach (var x in enemies)
        {
            x.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            
        }
        yield return new WaitForSeconds(1f);
        anim.SetBool("Attack",false);
    }
    
    public void Shoot()
    {
        shoot.Play();
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler (0f, 0f, -90f));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                    rb.AddForce(firePoint.right * bulletSpeed,ForceMode2D.Impulse); 
    }
    


    public void TakeDamage(int damage)
    {
        hp -= damage;
        GameObject bl = Instantiate(blood, transform.position, transform.rotation);
        hit.Play();
        Destroy(bl,4f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canShoot && shootTime < 0)
            {
                Shoot();
                ammo++;
                shootTime = shootTimeOffset;
            }

            if (hasAxe && attackTime < 0)
            {
                attackTime = attackTimeOffset;
                StartCoroutine(AxeAttack());
            }
        }
    }
}
