using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianController : MonoBehaviour
{
    public int hitPoints;
    public float movementSpeed;
    private GameObject kitty;
    private Vector3 target;
    private bool HasCapturedKitty;
    private bool targetKittyCaptured;
    public Animator bubbleAnimator;
    public Animator magicianAnimator;
    public Rigidbody2D magicianrb;
    public float direction;
    public AudioSource audiosource;
    public AudioClip hit;
    public AudioClip meow;
    private bool killed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hitPoints -= 2;
            audiosource.PlayOneShot(hit, 1.5f);
        }
        else if (collision.gameObject.tag == "LargeBullet")
        {
            hitPoints -= 4;
            audiosource.PlayOneShot(hit, 1.5f);
        }
        else if (collision.gameObject.tag == "SmallBullet")
        {
            hitPoints -= 1;
            audiosource.PlayOneShot(hit, 1.5f);
        }
        if (collision.gameObject.tag == "Kitty1" || collision.gameObject.tag == "Kitty2" || collision.gameObject.tag == "Kitty3")
        {
            HasCapturedKitty = true;
            bubbleAnimator.SetBool("caughtKitty", true);
            audiosource.PlayOneShot(meow);
        }
        if (hitPoints <= 0)
        {
            GameController.instance.DefeatEnemy();
            StartCoroutine(killMagician());
        }
    }

    IEnumerator killMagician()
    {
        killed = true;
        magicianrb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GameController.instance.DefeatEnemy();
        magicianAnimator.SetBool("deadMagician", true);
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void Start()
    {
        HasCapturedKitty = false;
        chooseKitty();
        target = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f),0f);
    }

    private void Update()
    {
        if (!HasCapturedKitty)
        {
            if (targetKittyCaptured)
            {                
                chooseKitty();
                moveToTarget();
            }
            else if (!targetKittyCaptured)
            { 
                if(kitty != null)
                {
                    magicianrb.velocity = (kitty.transform.position - transform.position).normalized * movementSpeed;
                    direction = Vector2.SignedAngle(Vector2.up, (kitty.transform.position - transform.position).normalized);
                    magicianAnimator.SetFloat("Direction", direction);
                    targetKittyCaptured = kitty.GetComponent<KittyMovement>().captured;
                }
                else
                {
                    moveToTarget();
                }                
            }                        
        }
        else if (HasCapturedKitty)
        {            
            moveToTarget();
        }
        else if (killed)
        {
            magicianrb.velocity = Vector2.zero;
        }
    }
    private void chooseKitty()
    {
        while (!kitty)
        {
            kitty = GameObject.FindGameObjectWithTag("Kitty" + Random.Range(1, 4));
        }
        targetKittyCaptured = kitty.GetComponent<KittyMovement>().captured;
    }
    private void moveToTarget()
    {
        magicianrb.velocity = target.normalized * movementSpeed;
        direction = Vector2.SignedAngle(Vector2.up, target.normalized);
        magicianAnimator.SetFloat("Direction", direction);
    }
}
