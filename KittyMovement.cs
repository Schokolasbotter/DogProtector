using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyMovement : MonoBehaviour
{
    private float timer;
    public int stopAfter;
    public int moveAfter;
    public float kittySpeed;
    private float newX;
    private float newY;
    private Vector2 m;
    public Animator Animator;
    public SpriteRenderer spriteRenderer;
    public Collider2D Kittycollider;
    private Transform capturer;
    public bool captured;
    public Rigidbody2D rb;

    private void Start()
    {
        newX = Random.Range(-1, 2);
        newY = Random.Range(-1, 2);
        timer = 0f;
        kittySpeed = Random.Range(0f, 2f);
        stopAfter = Random.Range(1, 11);
        moveAfter = Random.Range(1, 6);
    }
    private void FixedUpdate()
    {
        if (capturer)
        {           
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = capturer.transform.position;
            spriteRenderer.enabled = false;
            Kittycollider.enabled = false;
            captured = true;
        }
        else if (!capturer)
        {
            spriteRenderer.enabled = true;
            Kittycollider.enabled = true;
            captured = false;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            timer += Time.deltaTime;            
            if (timer <= stopAfter)
            {
                m = new Vector2(newX, newY) * kittySpeed * Time.deltaTime;
                //transform.position = transform.position + m;
                rb.MovePosition(rb.position + m);
            }
            else if(timer > stopAfter && timer < stopAfter + moveAfter)
            {
                rb.velocity = Vector2.zero;
            }
            else if (timer >= stopAfter + moveAfter)
            {
                newX = Random.Range(-1, 2);
                newY = Random.Range(-1, 2);
                timer = 0;
                kittySpeed = Random.Range(0f, 5f);
                stopAfter = Random.Range(1, 11);
                moveAfter = Random.Range(1, 6);
            }
            setDirection();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            newX = -newX;
            newY = -newY;
        }
        if (collision.gameObject.tag == "Magician")
        {
            capturer = collision.gameObject.GetComponent<Transform>();            
        }        
    }
    private void setDirection()
    {
        if(newX == 1)
        {            
            Animator.SetBool("Left", false);
            Animator.SetBool("Up", false);
            Animator.SetBool("Down", false);
            Animator.SetBool("Standing", false);
            Animator.SetBool("Right", true);
        }
        if(newX == -1)
        {            
            Animator.SetBool("Right", false);
            Animator.SetBool("Up", false);
            Animator.SetBool("Down", false);
            Animator.SetBool("Standing", false);
            Animator.SetBool("Left", true);
        }
        if(newY == 1 && newX == 0)
        {
            Animator.SetBool("Right", false);
            Animator.SetBool("Left", false);
            Animator.SetBool("Down", false);
            Animator.SetBool("Standing", false);
            Animator.SetBool("Up", true);
        }
        if (newY == -1 && newX == 0)
        {
            Animator.SetBool("Right", false);
            Animator.SetBool("Left", false);
            Animator.SetBool("Up", false);
            Animator.SetBool("Standing", false);
            Animator.SetBool("Down", true);
        }
        if (timer >= stopAfter)
        {
            Animator.SetBool("Right", false);
            Animator.SetBool("Left", false);
            Animator.SetBool("Up", false);
            Animator.SetBool("Down", false);
            Animator.SetBool("Standing", true);
        }
    }

}

