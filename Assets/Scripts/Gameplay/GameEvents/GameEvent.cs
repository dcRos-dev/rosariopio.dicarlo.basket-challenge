using UnityEngine;

/// <summary>
/// basic GameEvent
/// </summary>
public abstract class GameEvent : MonoBehaviour
{
    public string Name { get; protected set; }
    public int Points { get; protected set; }
    public EventRarity Rarity { get; protected set; }

}

public enum EventRarity
{
    Common,
    Rare,
    SuperRare
}

