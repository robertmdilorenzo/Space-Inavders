using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public int lives, shootDelay;
    public GameObject managerObj;
    public bool canShoot;
    public GameObject UILives;

    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        manager = managerObj.GetComponent<GameManager>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("left"))
        {
            gameObject.transform.Translate(-0.1f, 0.0f, 0.0f);
        }
        if(Input.GetKey("right"))
        {
            gameObject.transform.Translate(0.1f, 0.0f, 0.0f);
        }
        if(Input.GetKeyDown("space"))
        {
            if (canShoot)
            {
                Shoot();
                canShoot = false;
            }
            
            
        }
        if(lives == 0)
        {
            manager.GameOver();
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerBullet"));

        //Makes sure that when the bullet is spawned, it does not collide with the player
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);

        bullet.transform.position = gameObject.transform.position;
    }

    public void TakeDamage()
    {
        Destroy(UILives.transform.GetChild(0).gameObject);
        lives--;
    }
}
