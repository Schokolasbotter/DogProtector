using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDeleter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kitty1" || collision.gameObject.tag == "Kitty2" || collision.gameObject.tag == "Kitty3")
        {
            GameController.instance.LoseKitty();
        }
        Destroy(collision.gameObject,0.5f);
    }
}