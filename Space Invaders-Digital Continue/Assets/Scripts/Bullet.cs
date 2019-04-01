using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager manager;
    Player player;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(0.0f, 0.1f, 0.0f);
         
    }

    //When the bullet goes off screen, it deletes itself
    private void OnBecameInvisible()
    {
        player.canShoot = true;
        Destroy(gameObject); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        manager.HandleCollision(gameObject, collision.gameObject);
    }

}
