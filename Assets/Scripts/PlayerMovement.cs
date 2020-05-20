using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 mousePos;

    [Header("Health")]
    private float maxHP = 100;
    public float hp;
    public Slider hpSlider;

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Camera cam;
    public Animator anim;
    
    public GameObject blood;

    private void Start()
    {
        hp = maxHP;
    }

    private void Update()
    {
        
        if(Pause.isPaused == false)
        {
            Walk();
            FaceMouse();
        }
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        hpSlider.value = hp;

        
        
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    

    private void Walk()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0f);
        if (movement != new Vector3(0, 0, 0))
        {
            anim.SetBool("Walk",true);
            
        }
        else
        {
            anim.SetBool("Walk",false);
        }
        transform.position = transform.position + Vector3.ClampMagnitude(movement,1f) * moveSpeed * Time.deltaTime;
    }

    private void FaceMouse()
    {
        Vector2 mousPos = Input.mousePosition;
        mousPos = Camera.main.ScreenToWorldPoint(mousPos);
        
        Vector2 direction = new Vector2(mousPos.x - transform.position.x,mousPos.y - transform.position.y);

        transform.right = direction;
    }

    public void TakeDamage()
    {
        hp -= 5;
        GameObject bl = Instantiate(blood, transform.position, transform.rotation);
        Destroy(bl,4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MedKit"))
        {
            Destroy(other.gameObject);
            hp += 30;
            if (hp > maxHP)
                hp = maxHP;
        }
        if (other.CompareTag("Fuel"))
        {
            Destroy(other.gameObject);
        }
    }
}
