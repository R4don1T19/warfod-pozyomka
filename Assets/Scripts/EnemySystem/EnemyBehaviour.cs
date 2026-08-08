using UnityEngine;
using System;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyPlayerDetect enemyPlayerDetect;
    private EnemySurfaceDetect enemySurfaceDetect;

    [SerializeField] private Transform Enemy;
    [SerializeField] private GameObject Player;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isOnStairs;
    public float speed;
    public double range;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Vector3 PlayerPos;

    private void Start()
    {
        enemyPlayerDetect = GetComponentInChildren<EnemyPlayerDetect>();
        enemySurfaceDetect = GetComponentInChildren<EnemySurfaceDetect>();

        rb.gravityScale = 10f;
        speed = 0.1f;

        if (Player == null)
            Player = GameObject.Find("MainPlayer");
    }
    void Update()
    {
        Walk();
        if (enemySurfaceDetect != null)
        {
            _isOnStairs = enemySurfaceDetect.IsStairs;
            _isGrounded = enemySurfaceDetect.IsGrounded;
        }
    }
    void FixedUpdate()
    { 
        if (Player == null)
            return;

        if (enemyPlayerDetect.chasePlayer && (_isOnStairs || _isGrounded))
        {
            PlayerPos = enemyPlayerDetect.PlayerPos();
            transform.position = Vector3.MoveTowards(transform.position, PlayerPos, speed);
            //transform.position = Vector3.SmoothDamp(transform.position, PlayerPos, ref currentVelocity, 0.25f); // предельно понятн

            if (PlayerPos.x > transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
            
            if (enemyPlayerDetect.rangeCalculate() < 5)
                enemyPlayerDetect.chasePlayer = false;
        }
    }
    private void Walk()
    {
        if (_isGrounded || _isOnStairs || enemyPlayerDetect.chasePlayer)
            rb.freezeRotation = true;
        else 
            rb.freezeRotation = false;
    }
}
