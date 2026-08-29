using UnityEngine;

public class DoorID : MonoBehaviour
{
    /* 
   У двери будет два ID, вход и выход 
   При переходе с одной сцены в другую, в LE записываются значения string входа и выхода
   Перед загрузкой сцены, в Awake () будет сделан перебор, в котором будет искать на сцене объект Door, данные входа.
   Найдя нужные данные, просто будет телепорт Player на transform.position у объекта двери.
*/
    [SerializeField] internal string DoorIDTo;
    [SerializeField] internal string DoorIDFrom;
    [SerializeField] internal string SceneName;
}
