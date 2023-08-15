using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    private Vector2 movement;
    private Vector2 rotation;
    public float angle;
    public float movementAngle;
    public float playerSpeed;
    public Rigidbody2D rb;
    public GameObject laserBlastPrefab;
    public Transform firePoint;
    public float timeBetweenShots;
    private bool canShoot;
    private bool trigger;
    public Animator Animator;
    public Animator bubbleAnimator;
    private Vector2 m;
    public float timeKO;
    private bool stunned;
    public AudioSource audiosource;
    public AudioClip bark;

    // Start is called before the first frame update
    void Awake()
    {
        // Get Controller Input
        controls = new PlayerControls();
        // Movement
        controls.GamePlay.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        //Rotation
        controls.GamePlay.Rotate.performed += ctx => rotation = ctx.ReadValue<Vector2>();

        //Get Rigidbody2D attached to Character
        rb = GetComponent<Rigidbody2D>();

        //Shoot when button is pressed
        canShoot = true;
        controls.GamePlay.Bark.started += ctx => trigger = true;
        controls.GamePlay.Bark.canceled += ctx => trigger = false;       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameController.instance.gamePlaying && !stunned)
        {
            OnEnable();
        }
        else if (!GameController.instance.gamePlaying)
        {
            OnDisable();
        }
        //Movement
        m = new Vector2(movement.x, movement.y) * playerSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + m);      
        movementAngle = Vector2.SignedAngle(Vector2.up, movement);
        //Rotation
        if (rotation.magnitude > 0f)
        {
            angle = Vector2.SignedAngle(Vector2.up, rotation);
            rb.SetRotation(angle);
        }
        //Shooting
        if (trigger && canShoot)
        {
                Fire();
        }     
        //Set the animation correctly
        setAnimation();
    }
    private void Fire()
    {
        if (canShoot)
        {
            canShoot = false;          
            Instantiate(laserBlastPrefab, firePoint.position, transform.rotation);
            audiosource.PlayOneShot(bark);
            StartCoroutine(ShotCooldown());
        }
    }
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }   
    private void OnEnable()
   {
        controls.GamePlay.Enable();
   }

   private void OnDisable()
   {
        controls.GamePlay.Disable();
   }    

    private void setAnimation()
    {
        if (m != Vector2.zero)
        {
            Animator.SetBool("movement", true);
            Animator.SetFloat("movementAngle", movementAngle);
        }
        if (m == Vector2.zero)
        {
            Animator.SetBool("movement", false);
        }
        if (rotation != Vector2.zero)
        {
            Animator.SetBool("rotation",true);
            Animator.SetFloat("rotationAngle", angle);
        }
        if (rotation == Vector2.zero)
        {
            Animator.SetBool("rotation", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knight" || collision.gameObject.tag == "Magician")
        {
           StartCoroutine(StopMovement());
        }
    }

    IEnumerator StopMovement()
    {
        stunned = true;
        controls.GamePlay.Disable();
        bubbleAnimator.SetBool("confused", true);
        movement = Vector2.zero;
        rotation = Vector2.zero;
        Animator.SetBool("movement", false);
        Animator.SetBool("rotation", false);
        Animator.SetBool("stunned", true);
        yield return new WaitForSeconds(timeKO);
        controls.GamePlay.Enable();
        bubbleAnimator.SetBool("confused", false);
        Animator.SetBool("stunned", false);
        stunned = false;
    }
}


