using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public Player player;
    public Transform enemyContainer;
    public Canvas UI;
    public Vector2 randomFire;
    public Vector2 randomUFOSpawnTime;
    public float fireDelay = 0.0f;
    public float ufoDelay = 8.0f;

    //Container for enemies that are at the bottom of the formation and are able to fire bullets
    public List<Enemy> fireReadyEnemies;

    public Transform ufoSpawnPoint;

    long score;
    int enemyCount, enemyDirection;
    float startTime, moveRate;
    bool shouldMoveEnemiesDown;
    Text currentScoreText, finalScoreText;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        enemyDirection = 1;
        score = 0;
        enemyCount = 55;
        moveRate = 3.0f;
        Invoke("MoveInvaders", moveRate);
        shouldMoveEnemiesDown = false;
        currentScoreText = UI.transform.Find("ActualScore").GetComponent<Text>();
        finalScoreText = UI.transform.Find("GameOverPanel").Find("FinalScore").GetComponent<Text>();
        fireReadyEnemies.Capacity = 11;

        Transform initialFiringEnemies = enemyContainer.GetChild(0);

        for (int i = 0; i < initialFiringEnemies.childCount; i++)
        {
            fireReadyEnemies.Add(initialFiringEnemies.GetChild(i).gameObject.GetComponent<Enemy>());
        }
    }

    // Update is called once per second
    void FixedUpdate()
    {
        //Handles the victory condition for the game
        if(enemyCount == 0)
        {
            GameObject endGamePanel = UI.transform.Find("GameOverPanel").gameObject;
            endGamePanel.SetActive(true);
            Text victoryMessage = endGamePanel.transform.Find("GameOverText").gameObject.GetComponent<Text>();
            finalScoreText.text = score.ToString(); 
            victoryMessage.text = "You Win!";
            victoryMessage.color = Color.green;
            

        }
        
        currentScoreText.text = score.ToString();

        //Selects enemies at random to fire bullets at random times
        if (Time.time > fireDelay)
        {
            fireDelay = GetFutureTimeInRange(randomFire.x, randomFire.y);
            int nextFiringEnemyLocation = Random.Range(0, fireReadyEnemies.Capacity);
            if (fireReadyEnemies[nextFiringEnemyLocation] != null)
            {
                fireReadyEnemies[nextFiringEnemyLocation].Shoot();
            }
        }
        //Randomly creates UFOs
        if (Time.time > ufoDelay)
        {
            ufoDelay = GetFutureTimeInRange(randomUFOSpawnTime.x, randomUFOSpawnTime.y);
            SpawnUFO();
        }
    }

    public void GameOver()
    {
        UI.transform.Find("GameOverPanel").gameObject.SetActive(true);
        finalScoreText.text = score.ToString();
        
    }

    //Moves the invader formation in a given direction. Once they meet a boundary they go down and change direction.
    void MoveInvaders()
    {
        if (enemyContainer != null)
        {
            if (!shouldMoveEnemiesDown)
            {
                enemyContainer.transform.Translate(0.5f * enemyDirection, 0.0f, 0.0f);
            }
            else
            {
                shouldMoveEnemiesDown = false;
                enemyContainer.transform.Translate(0.0f, -0.5f, 0.0f);
                enemyDirection *= -1;
            }
        }
        if ((Time.time - startTime) > 15.0f)
        {
            startTime = Time.time;
            moveRate = moveRate - 0.5f;
        }

        if (moveRate <= 0)
        {
            moveRate = 0.1f;
        }
        Invoke("MoveInvaders", moveRate);
    }

    public void SetMoveDown(bool moveDown)
    {
        shouldMoveEnemiesDown = moveDown;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    void SpawnUFO()
    {
        GameObject newUFO = Instantiate(Resources.Load("Prefabs/UFO")) as GameObject;
        newUFO.transform.position = ufoSpawnPoint.position;
    }

    //Takes in any two objects that have collided, then selects which handling function to call based on the first object's tag
    public void HandleCollision(GameObject firstObject, GameObject secondObject)
    {
        string collisionType = firstObject.tag;

        switch (collisionType)
        {
            case "Bullet":
                HandleBulletCollision(firstObject.GetComponent<Bullet>(), secondObject);
                break;
            case "Enemy":
                HandleEnemyCollision(firstObject.GetComponent<Enemy>(), secondObject);
                break;
            case "EnemyBullet":
                HandleEnemyBulletCollision(firstObject.GetComponent<EnemyBullet>(), secondObject);
                break;
            default:
                break;
        }
    }

    //This method handles collisions for when a bullet hits something in the game
    void HandleBulletCollision(Bullet bullet, GameObject target)
    {
        string targetTag = target.tag;
        Destroy(bullet.gameObject);
        switch (targetTag)
        {
            case "Enemy":
                Enemy enemy = target.GetComponent<Enemy>();
                
                AddScore(enemy.myPointValue);
                enemyCount--;
                if (fireReadyEnemies[target.transform.GetSiblingIndex()] == enemy && (enemy.RowIndex() + 1 < enemyContainer.childCount))
                {
                   
                    fireReadyEnemies[target.transform.GetSiblingIndex()] = GetNextFiringEnemy(enemy);
                   
                    
                }
                Destroy(target);
                break;
            case "BarrierBlock":
                target.GetComponent<BarrierBlock>().TakeDamage();
                break;
            case "UFO":
                AddScore(target.GetComponent<UFO>().myPointValue);
                Destroy(target);
                break;
            default:
                break;
        }
        
    }

    //After an enemy is destroyed, this function updates the list of enemies that are able to shoot with the enemy above the one just killed
    Enemy GetNextFiringEnemy(Enemy deadEnemy)
    {
        
        int rowIndex = deadEnemy.RowIndex() + 1;
        int colIndex = deadEnemy.gameObject.transform.GetSiblingIndex();
        Enemy replacementEnemy;
        while ((rowIndex < enemyContainer.childCount) && (enemyContainer.transform.GetChild(rowIndex).GetChild(colIndex).gameObject == null) )
        {
            rowIndex++;
        }
        replacementEnemy = enemyContainer.GetChild(rowIndex).GetChild(colIndex).gameObject.GetComponent<Enemy>();
       
        return replacementEnemy;
    }

    //This method handles scenarios when an enemy collides with something in the game
    void HandleEnemyCollision(Enemy enemy, GameObject otherObject)
    {
        string otherTag = otherObject.tag; 

        if(otherObject.tag == "Player")
        {
            otherObject.gameObject.GetComponent<Player>().TakeDamage();
        }
    }

    //This method handles times when an enemy bullet collides with something in the game
    void HandleEnemyBulletCollision(EnemyBullet enemyBullet, GameObject target)
    {
        string targetTag = target.tag;
        Destroy(enemyBullet.gameObject);

        switch (targetTag)
        {
            case "Player":
                Debug.LogError("Player hit");
                target.GetComponent<Player>().TakeDamage();
                break;
            case "BarrierBlock":
                target.GetComponent<BarrierBlock>().TakeDamage();
                break;
            default:
                break;
        }
    }

    //This method returns a time between the current time + min and the current time + max
    float GetFutureTimeInRange(float min, float max)
    { 

        return Time.time + (float) Random.Range(min, max);
    }
}
