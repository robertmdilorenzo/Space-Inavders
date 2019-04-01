using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameManager manager;
     void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        gameObject.transform.Translate(0.0f, -0.1f, 0.0f);
        
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        manager.HandleCollision(gameObject, collision.gameObject);
        
        
    }

   
}
