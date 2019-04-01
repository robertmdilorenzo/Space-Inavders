using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    
    Transform myRow;
    public int myPointValue;
    GameManager manager;
    public bool canShoot;

    void Start()
    {
       
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        manager.HandleCollision(gameObject, collision.gameObject);
        
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(Resources.Load("Prefabs/EnemyBullet")) as GameObject;
        Physics2D.IgnoreCollision(bullet.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        bullet.transform.position = gameObject.transform.position;
    }

    public int RowIndex()
    {
        return gameObject.transform.parent.GetSiblingIndex();
    }

    public int ColIndex()
    {
        return gameObject.transform.GetSiblingIndex();
    }

    public string ToString()
    {
        return "Enemy: [Row: " + RowIndex() + ", Col: " + ColIndex() + "]";
    }



}
