using UnityEngine;
using System;
public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Transform Enemy;

    private bool _inEnemyRange = false;
    private bool _betweenStatus = false;
    private float progress = 0f;
    [SerializeField] private double range;
    
    private Vector3 _midPoint;
    private Vector3 _returnPositionA;
    private Vector3 _base;
    private Vector3 PlayerPos;
    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if (Player == null)
            return;
        else
            PlayerTransform = Player.transform;
            PlayerPos = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y + 1, -10); // навсегда запомнить позицию Капсулы
    }

    void LateUpdate() // здесь начинается сущий кошмар математики
    {
        if (_inEnemyRange == false)
        { 
            transform.position = Vector3.SmoothDamp(transform.position, PlayerPos, ref currentVelocity , 0.25f); // предельно понятно
        }

        if (_inEnemyRange == true) // два состояния - переход на центр "схватки" и возращение на Капсулу
        {
            if (Enemy == null || Player == null)
                return;
            // первый этап - А стоит, Б двигается
            _midPoint = new Vector3((PlayerTransform.position.x + Enemy.position.x) / 2, (PlayerTransform.position.y + Enemy.position.y) / 2, -1); // рассчитывается точка Б
            transform.position = _base = Vector3.Lerp(transform.position, _midPoint, 0.05f); // для t достаточно фиксированного значения из-за идеального случая
            range = Math.Sqrt(Math.Pow((PlayerTransform.position.x - Enemy.position.x), 2) + Math.Pow(PlayerTransform.position.y - Enemy.position.y, 2)); // ну и расстояние для механики

            if (range > 19) 
            {
                // здесь, в промежуточном статусе, мы запоминаем откуда мы будем "передвигаться"
                if (!_betweenStatus)
                    GetPosition(_base);

                // тот самый ебаный прогресс, который будет постоянно увеличиваться
                progress += 0.02f;
                transform.position = Vector3.Lerp(_returnPositionA, PlayerPos, progress); // правильная и грамотная реализация передвижения от фиксированного А до изменчивого Б
                _betweenStatus = true;

                if (progress > 1) // ну и проверка во избежании проблем с закручиванием алгоритма
                {
                    _inEnemyRange = false;
                    _betweenStatus = false;
                }
            }
            else
            {
                // Если снова приблизились к врагу - сбрасываем статус возврата
                _betweenStatus = false;
            }
        }
    }

    private void GetPosition(Vector3 Aposition) // мини-бро
    {
        _returnPositionA = Aposition;
        progress = 0f; // начинаем с начала, блять                                                 АХУЕТЬ, ЭТО ДИПСИК НАПИСАЛ noway
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // тут все предельно понятно
        {
            Enemy = collision.transform;
            _inEnemyRange = true;
            _betweenStatus = false;
            progress = 0f; // новый враг - новый прогресс

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            _inEnemyRange = false;
    }
}