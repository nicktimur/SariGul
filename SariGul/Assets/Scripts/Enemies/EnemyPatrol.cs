using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Animator anime;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
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

    private void DirectionChange()
    {
        anime.SetBool("moving", false);
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        anime.SetBool("Walk", true);

        enemy.localScale = new Vector3(-_direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
