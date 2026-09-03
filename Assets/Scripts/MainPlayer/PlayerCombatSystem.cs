
using UnityEngine;

public class PlayerCombatSystem : AC_CombatSystem
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private Transform TriggerZone;
    [SerializeField] private EnemyCombatSystem enemy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int hpPlayer;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _blink;
    [SerializeField] private float cooldown;
    [SerializeField] private float invisibleSpeed;
    [SerializeField] private float timer;
    [SerializeField] private float invisible;
    public int HPPlayer { get { return hpPlayer; } set { hpPlayer = value; } }

    void Start()
    {
        invisibleSpeed = 2f;
        invisible = 0f;
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        TriggerZone = GetComponentInChildren<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpPlayer = 5;
        cooldown = 1f;
    }

    void Update()
    {
        dealDamage();
        if (_blink)
            dealDamageBlink();
        flip();
    }
    public override void dealDamage()
    {
        if (cooldown <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_canAttack)
                    enemy.takeDamage(1);
                _blink = true;
                cooldown = 1f;
                spriteRenderer.color = new Color(1, 1, 1, 1f);
                _blink = true;
                timer = 0.5f;
            }
        }
        else
            cooldown -= Time.deltaTime;
    }

    public override void dealDamageBlink()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            Color color = spriteRenderer.color;
            color.a = Mathf.MoveTowards(color.a, invisible, invisibleSpeed * Time.deltaTime);
            spriteRenderer.color = color;
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 0f);
            _blink = false;
        } 
    }

    public override void takeDamage(int damage)
    {
        hpPlayer -= damage;
        if (hpPlayer <= 0)
            death();
    }

    public override void death()
    {
        Destroy(GameObject.Find("MainPlayer"));
    }
    private void flip()
    {
        if (PlayerMovement.IsFlipped == false)
        {
            TriggerZone.localPosition = new Vector2(1f, 0f);
        }
        else
        {
            TriggerZone.localPosition = new Vector2(-1f, 0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponentInChildren<EnemyCombatSystem>();
            _canAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
            _canAttack = false;
    }
}
