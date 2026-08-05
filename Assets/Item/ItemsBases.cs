using UnityEngine;
using UnityEngine.UI;
public enum Type
{
    Weapon, Heal, Quest
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items")]
public class ItemsBases : ScriptableObject
{
    public string ItemName;
    public int maxAmount;
    public Sprite image;
}
