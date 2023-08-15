using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public int hitPoints;
    public float movementSpeed = 2f;
    public Transform player;
    public Rigidbody2D knightRb;
    public Animator animator;
    public float direction;
    public AudioSource audiosource;
    public AudioClip hit;
    private bool killed = false;

    //Health Management
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audiosource.PlayOneShot(hit, 1.5f);
        if (collision.gameObject.tag == "Bullet")
        {
            hitPoints -= 2;            
        }
        else if (collision.gameObject.tag == "LargeBullet")
        {
            hitPoints -= 4;
        }
        else if (collision.gameObject.tag == "SmallBullet")
        {
            hitPoints -= 1;            
        }
        if (hitPoints <= 0)
        {
            StartCoroutine(killKnight());            
        }
    }

    IEnumerator killKnight()
    {
        killed = true;
        knightRb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GameController.instance.DefeatEnemy();
        animator.SetBool("deadKnight", true);
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (!killed)
        {
            transform.localRotation = transform.localRotation * Quaternion.Inverse(transform.rotation);
            knightRb.velocity = (player.transform.position - transform.position).normalized * movementSpeed;
            direction = Vector2.SignedAngle(Vector2.up, (player.transform.position - transform.position).normalized);
            animator.SetFloat("Direction", direction);
        }
    }
}
