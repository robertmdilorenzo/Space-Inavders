using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    GameManager manager;
    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if(collision.gameObject.tag == "Enemy")
        {
            RedirectEnemies();
        }
        
    }

    //When enemies reach the left or right boundary, the game manager must tell them to move down on the next tick and reverse their direction
    public void RedirectEnemies()
    {
      
        manager.SetMoveDown(true);
    }
}
