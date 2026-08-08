using UnityEngine;
using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
public class PlayerMovement : MonoBehaviour
{
    // wow no way
    [SerializeField] internal float speed = 6.66f;
    [SerializeField] private float _ladderSpeed = 3.33f;
    [SerializeField] private float stamina = 6f;
    [SerializeField] private float maxStamina = 6f;
    [SerializeField] private float staminaTimer = 2f;
    [SerializeField] private Animator anime;

    private PlayerSurfaceDetect PSD;

    [SerializeField] private bool _isFlipped = false;
    public bool IsFlipped { get { return _isFlipped; } set { _isFlipped = value; } }

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveVector;

    void Start()
    {
        anime = GetComponent<Animator>();
        PSD = GetComponentInChildren<PlayerSurfaceDetect>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 10f;

    }
    private void Update()
    {
        LadderTeleport();
        Rotate();
        Stamina();
    }
    private void FixedUpdate()
    {
        Walk();
    }
    private void Walk()
    {
        if (PSD.IsOnLadder)
        {
            moveVector.y = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveVector.y * _ladderSpeed);
            anime.SetFloat("moveSpeed", rb.linearVelocity.x);
        }
        else if (PSD.IsStairs)
        {
            moveVector.x = Input.GetAxisRaw("Horizontal");
            moveVector.y = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
            anime.SetFloat("moveSpeed", rb.linearVelocity.x);
        }
        else if (PSD.IsGrounded || PSD.NearLadder)  // _NearLadder очень важен
        {
            moveVector.x = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
            anime.SetFloat("moveSpeed", rb.linearVelocity.x);
        }
    }

    internal void StopMovement()
    {
        enabled = false;
        rb.linearVelocity = Vector2.zero;
    }

    internal void StartMovement()
    {
        enabled = true;
    }
    private void Stamina()
    {
        // Это условие проверяет нажатие на шифт при наличии стамины
        if (Input.GetKey(KeyCode.LeftShift) && stamina >= 0)
        {
            staminaTimer = 2f;     // установка таймера
            speed = 10f;           // увеличение скорости 
            stamina -= 0.05f;      // ну и пока бежим тратится стамина
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {    // Отпустил шифт?
            speed = 6.6f;              // возвращаем прежнюю скорость
            stamina += 0.025f;         // восстанавливаем стамину
            if (stamina > maxStamina)  // если значение стамины превысило допустимое, то возвращаем граничное значение
                stamina = maxStamina;
        }
        if (stamina <= 0)
        {     // стамина кончилась?
            speed = 6.6f;                      // возвращаю скорость
            if (staminaTimer >= 0)
                staminaTimer -= Time.deltaTime; // таймер истекает
            else
                stamina = 6f; // и стамина полностью восстанавливается
        }
    }

    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            sr.flipX = false;
            IsFlipped = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            sr.flipX = true;
            IsFlipped = true;
        }
    }
    private void LadderTeleport()
    {
        if (PSD.TPDN == null || PSD.TPUP == null) // обязательно, чтобы было ХОТЬ ЧТО-ТО, а то откуда лезть то?
        {
            return;
        }
        double firstRange = Math.Sqrt(Math.Pow((rb.position.x - PSD.TPUP.position.x), 2) + Math.Pow(rb.position.y - PSD.TPUP.position.y, 2));
        double secondRange = Math.Sqrt(Math.Pow((rb.position.x - PSD.TPDN.position.x), 2) + Math.Pow(rb.position.y - PSD.TPDN.position.y, 2));
        // тут короче расстояния от персонажа к двум телепортам(который вверху и внизу) 
        if (PSD.NearLadder && Input.GetKeyUp(KeyCode.E)) // если мы вблизи лестницы и нажмем на У, то
        {
            PSD.IsOnLadder = true; // 1) мы на лестнице! а значит можем карабкаться
            PSD.IsGrounded = false;
            PSD.IsStairs = false;
            if (firstRange < secondRange) // если телепорт вверх ближе к нам
            {
                transform.position = PSD.TPUP.position; // телепорт к точке вверх
            }
            else
            {
                transform.position = PSD.TPDN.position; // телепорт к нижней точке
            }
            rb.gravityScale = 0; //отключаем гравитацию для успешного продвижения вверх-вниз
            gameObject.layer = LayerMask.NameToLayer("Ladder"); //меняем слой для передвижения сквозь объекты
        }
    }
}