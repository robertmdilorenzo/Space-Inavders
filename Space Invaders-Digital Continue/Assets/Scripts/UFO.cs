using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    // Start is called before the first frame update
    public int myPointValue = 200;
    void Start()
    {
        InvokeRepeating("MoveUFO", 0.03f, 0.03f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveUFO()
    {
        gameObject.transform.position += new Vector3(0.1f, 0.0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "RightBound")
        {
            Destroy(gameObject);
        }
    }
       
}

