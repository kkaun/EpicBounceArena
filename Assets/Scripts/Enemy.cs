using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    public float speed = 5f;

    private Rigidbody enemyRb;

    private GameObject player;

    private GameManager gameManager;

    private static float destroyPosY = -8;

    void Start()
    {
        player = GameObject.Find("Player");

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyRb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        CheckDeath();

        PursuePlayer();
    }

    private void CheckDeath()
    {
        if (transform.position.y < destroyPosY)
        {
            Destroy(gameObject);

            switch (enemyType)
            {
                case EnemyType.Enemy:
                    gameManager.IncreaseScore(2);
                    break;
                case EnemyType.EliteEnemy:
                    gameManager.IncreaseScore(5);
                    break;
                case EnemyType.Boss:
                    gameManager.IncreaseScore(15);
                    break;
                default:
                    break;
            }
        }
    }

    void PursuePlayer()
    {
        if (!player.GetComponent<PlayerController>().HasForceAura())
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
        } else
        {
            Vector3 lookDirection = (transform.position - player.transform.position).normalized;
            enemyRb.AddForce(lookDirection * PlayerController.forceAuraPushFactor);
        }
    }
}
