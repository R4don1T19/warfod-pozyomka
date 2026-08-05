using UnityEngine;

public abstract class AC_CombatSystem : MonoBehaviour
{   
    public abstract void dealDamage();

    public abstract void dealDamageBlink();

    public abstract void takeDamage(int damage);

    public abstract void death();
}
