using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 15;
    //Damage is called by Enemy when collided
    public int Damage;

    void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    //Destroy the Bullet once is outside the Screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}