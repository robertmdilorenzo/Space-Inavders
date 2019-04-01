using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBlock : MonoBehaviour
{
    // Start is called before the first frame update
    int health;
    Color myColor;

    void Start()
    {
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
//Alters block color to represent taking damage. Green is 3 health, Yellow is 2 health and Red is 1 health
    Color SetColor(int health)
    {
        switch (health)
        {
            case 3:
                myColor = Color.green;
                return myColor;
            case 2:
                myColor = Color.yellow;
                return myColor;
            case 1:
                myColor = Color.red;
                return myColor;
            default:
                myColor = Color.green;
                return myColor;
        }
    }

    public void TakeDamage()
    {
        health--;
        gameObject.GetComponent<SpriteRenderer>().color = SetColor(health);
    }
}
