using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Weapons Config")]
    private float shootTimer;
    public float shootTimerOffset = 0.1f;
    public int ammo;
    public int maxAmmo;
    public bool hasPistol;
    public bool hasRifle;
    public bool hasAxe;
    private int[] weaponNum = new int[3];
    private int weaponCur = 0;
    public int pistolAmmoStock = 21;
    public int rifleAmmoStock = 90;
    private int pistolAmmo = 20;
    private int rifleAmmo = 30;
    public bool canShoot = true;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Text bulletText;
    public GameObject bullteImage;
    public float bulletSpeed = 20f;

    [Header("AxeAttack")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask whatIsEnemy;
    public int damage = 20;
    private float attackTimer = 0;
    public float attackTimerOffset = 0.25f;

    public Animator anim;

    public AudioSource shoot;
    public AudioSource reload;

    private void Start()
    {
        if (hasPistol)
        {
            maxAmmo = 20;
        }
        else if (hasRifle)
        {
            maxAmmo = 30;
        }

        ammo = maxAmmo;
    }

    private void Update()
    {
        Animate();
        ShowUIBullets();
        if(Pause.isPaused == false)
        {
            
            ChangeWeapons();
            
            if (Input.GetButtonDown("Fire1") && hasPistol && ammo > 0 && canShoot)
            {
                Shoot();
                pistolAmmo--;
            }
        
            if (Input.GetButton("Fire1") && hasRifle && ammo > 0 && canShoot)
            {
                if (shootTimer <= 0)
                {
                    shootTimer = shootTimerOffset;
                    Shoot();
                
                }
                else
                {
                    shootTimer -= Time.deltaTime;
                }
            }

            if (Input.GetButton("Fire1") && hasAxe && attackTimer < 0)
            {
                attackTimer = attackTimerOffset;
                StartCoroutine(AxeAttack());
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        
            shootTimer -= Time.deltaTime;
            attackTimer -= Time.deltaTime;
        }
        
        
    }

    private void ShowUIBullets()
    {
        if (hasPistol)
        {
            bullteImage.SetActive(true);
            bulletText.enabled = true;
            bulletText.text = pistolAmmo + "/" + pistolAmmoStock;
        }
        else if (hasRifle)
        {
            bullteImage.SetActive(true);
            bulletText.enabled = true;
            bulletText.text = rifleAmmo + "/" + rifleAmmoStock;
        }
        else
        {
            bullteImage.SetActive(false);
            bulletText.enabled = false;
        }
    }

    private void Shoot()
    {
        shoot.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler (0f, 0f, -90f));
        bullet.GetComponent<Bullet>().PlayerBullet = true;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(firePoint.right * bulletSpeed,ForceMode2D.Impulse);
        ammo--;
        if (hasRifle)
        {
            rifleAmmo--;
        }
    }

    IEnumerator AxeAttack()
    {
        Debug.Log("Hit");
        anim.SetBool("Attack",true);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemy);
        foreach (var x in enemies)
        {
            if (x.isTrigger == false)
            {
                x.gameObject.GetComponent<Unit>().TakeDamage(80);
            }
            
        }
        yield return new WaitForSeconds(0.23f);
        anim.SetBool("Attack",false);
    }

    public void Animate()
    {
        if (hasPistol)
        {
            maxAmmo = 20;
            ammo = pistolAmmo;
            anim.SetBool("Pistol",true);
            anim.SetBool("Rifle",false);
            anim.SetBool("Axe",false);
        }
        else if (hasRifle)
        {
            maxAmmo = 30;
            ammo = rifleAmmo;
            anim.SetBool("Rifle",true);
            anim.SetBool("Pistol", false);
            anim.SetBool("Axe",false);
        }
        else if (hasAxe)
        {
            maxAmmo = 0;
            anim.SetBool("Axe",true);
            anim.SetBool("Pistol", false);
            anim.SetBool("Rifle",false);
        }
        else
        {
            maxAmmo = 0;
            anim.SetBool("Pistol", false);
            anim.SetBool("Rifle",false);
            anim.SetBool("Axe",false);
        }
    }

    IEnumerator Reload()
    {
        ammo = maxAmmo;
        
        if (hasRifle)
        {
            if (rifleAmmoStock> 0)
            {
                reload.Play();
                canShoot = false;
            }
            yield return new WaitForSeconds(1f);
            canShoot = true;
            rifleAmmoStock += rifleAmmo;
            if (rifleAmmoStock > 30)
            {
                rifleAmmo = maxAmmo;
                rifleAmmoStock -= 30;
            }
            else if(rifleAmmoStock <= 30 && rifleAmmoStock > 0) 
            {
                
                if (rifleAmmoStock > 30)
                {
                    rifleAmmo = 30;
                    rifleAmmoStock -= 30;
                }
                else
                {
                    rifleAmmo = rifleAmmoStock;
                    rifleAmmoStock = 0;
                }
            }
            else
            {
                rifleAmmo = rifleAmmo;
            }
        }
        else if (hasPistol)
        {
            if (pistolAmmoStock> 0)
            {
                reload.Play();
                canShoot = false;
            }
            yield return new WaitForSeconds(1f);
            canShoot = true;
            pistolAmmoStock += pistolAmmo;
            if (pistolAmmoStock > 20)
            {
                pistolAmmo = maxAmmo;
                pistolAmmoStock -= 20;
            }
            else if(pistolAmmoStock <= 20 && pistolAmmoStock > 0)
            {
                if (pistolAmmoStock > 20)
                {
                    pistolAmmo = 20;
                    pistolAmmoStock -= 20;
                }
                else
                {
                    pistolAmmo = pistolAmmoStock;
                    pistolAmmoStock = 0;
                }
                    

            }
            else
            {
                pistolAmmo = pistolAmmo;
            }
        }
    }
    
    
    

    private void ChangeWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (weaponCur >= weaponNum.Length -1)
            {
                weaponCur = 0;
            }
            else
            {
                weaponCur++;
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (weaponCur < 0)
            {
                weaponCur = weaponNum.Length-1;
            }
            else
            {
                weaponCur--;
            }
        }*/

        switch (weaponCur)
        {
            case 0:
                hasAxe = true;
                hasPistol = false;
                hasRifle = false;
                break;
            case 1:
                hasAxe = false;
                hasPistol = true;
                hasRifle = false;
                break;
            case 2:
                hasAxe = false;
                hasPistol = false;
                hasRifle = true;
                break;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            hasAxe = true;
            hasPistol = false;
            hasRifle = false;
            weaponCur = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasAxe = false;
            hasPistol = true;
            hasRifle = false;
            weaponCur = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasAxe = false;
            hasPistol = false;
            hasRifle = true;
            weaponCur = 2;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PistolAmmo"))
        {
            Destroy(other.gameObject);
            pistolAmmoStock += 20;
        }
        if (other.CompareTag("RifleAmmo"))
        {
            Destroy(other.gameObject);
            rifleAmmoStock += 30;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        Gizmos.color = Color.red;
    }
}
