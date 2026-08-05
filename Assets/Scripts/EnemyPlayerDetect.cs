using System.Text.RegularExpressions;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyPlayerDetect : MonoBehaviour
{
    public Transform Enemy;
    public Transform Player;
    public SpriteRenderer ChaseIcon;
    public bool chasePlayer = false;
    public bool ChasePlayer { get; set; }
    private void Start()
    {
        ChaseIcon = GetComponentInChildren<SpriteRenderer>();
        ChaseIcon.color = new Color(1, 1, 1, 0f);
    }
    internal double rangeCalculate()
    {
        return Math.Sqrt(Math.Pow((Player.position.x - Enemy.position.x), 2) + Math.Pow(Player.position.y - Enemy.position.y, 2));
    }

    internal Vector3 PlayerPos()
    {
        return new Vector3(Player.position.x, Player.position.y, 0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player = collision.transform;
            chasePlayer = true;
            if (rangeCalculate() <= 2)
                chasePlayer = false;
            ChaseIcon.color = new Color(1, 1, 1, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasePlayer = false;
            ChaseIcon.color = new Color(1, 1, 1, 0f);
        }
    }
}
