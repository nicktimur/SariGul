using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anime;

    [Header("Player")]
    [SerializeField] private Player player;

    [Header("HealthBar")]
    [SerializeField] private Transform bar;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anime.SetBool("Walk", false);
    }

    private void Update()
    {
        if (!player.isDead)
        {
            if (movingLeft)
            {
                if(enemy.position.x >= leftEdge.position.x)
                {
                    MoveInDirection(-1);
                }
                else
                {
                    DirectionChange();
                }
            }
            else
            {
                if(enemy.position.x <= rightEdge.position.x)
                {
                    MoveInDirection(1);
                }
                else
                {
                    DirectionChange();
                }

            }
        }
    }

    private void DirectionChange()
    {
        anime.SetBool("Walk", false);

        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration) 
        {
            movingLeft = !movingLeft;
        }

    }

    public void TurnBack()
    {
        movingLeft = !movingLeft;
    }


    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anime.SetBool("Walk", true);

        enemy.localScale = new Vector3(-_direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);

        bar.localScale = new Vector3(-_direction, initScale.y, initScale.z);
    }
}
