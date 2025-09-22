using UnityEngine;

public class BackboardEventCommon : GameEvent
{
    private void Awake()
    {
        Name = "Backboard +4";
        Points = 4;
        Rarity = EventRarity.Common;
        Duration = 40f;
    }
}

public class BackboardEventRare : GameEvent
{
    private void Awake()
    {
        Name = "Backboard +6";
        Points = 6;
        Rarity = EventRarity.Rare;
        Duration = 20f;
    }
}

public class BackboardEventSuperRare : GameEvent
{
    private void Awake()
    {
        Name = "Backboard +8";
        Points = 8;
        Rarity = EventRarity.SuperRare;
        Duration = 10f;
    }
}
