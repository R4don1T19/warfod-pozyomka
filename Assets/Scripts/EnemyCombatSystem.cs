using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EnemyCombatSystem : AC_CombatSystem
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private PlayerCombatSystem Player;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int hpenemy;
    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownDefault;
    [SerializeField] private float invisible = 0f;
    [SerializeField] private float visibleSpeed;
    [SerializeField] private float timer = 1f;
    [SerializeField] private bool _PlayerInZone = false;
    [SerializeField] private bool _blink = false;
    public int HPENEMY { get { return hpenemy; } set { hpenemy = value; } }
    void Start()
    {
        Enemy = GameObject.Find("Enemy");
        visibleSpeed = 1f;
        sprite = GetComponentInChildren<SpriteRenderer>();
        cooldown = cooldownDefault = 1.3f;
        hpenemy = 5;
    }
    void Update()
    {
        dealDamage();
        if (_blink)
            dealDamageBlink();
    }
    public override void dealDamage()
    {
        if (_PlayerInZone)
        {
            if (cooldown <= 0)
            {
                _blink = true;
                Player.takeDamage(1);
                cooldown = cooldownDefault;
                timer = 1f;
                sprite.color = new Color(1, 1, 1, 1f);
            }
            else
            {
                cooldown -= Time.deltaTime;
            }
        }
    }
    public override void dealDamageBlink()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            Color color = sprite.color;
            color.a = Mathf.MoveTowards(color.a, invisible, visibleSpeed * Time.deltaTime);
            sprite.color = color;
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 0f);
            _blink = false;
        }
    }

    public override void takeDamage(int damage)
    {
        hpenemy -= damage;
        if (hpenemy <= 0)
            death();
    }
    public override void death()
    {
        Destroy(Enemy);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player = collision.GetComponentInChildren<PlayerCombatSystem>();
            _PlayerInZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _PlayerInZone = false;
            cooldown = cooldownDefault;
        }

    }
}
