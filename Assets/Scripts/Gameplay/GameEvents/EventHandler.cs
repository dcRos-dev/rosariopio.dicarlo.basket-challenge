using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Randomly triggers a GameEvent
/// </summary>
public class EventHandler : MonoBehaviour
{
    [SerializeField] private float globalProbability = 0.3f;


    private List<GameEvent> availableEvents = new List<GameEvent>();
    private GameEvent currentEvent = null;

    //timer values
    private float timer = 0f;
    private float checkInterval = 4f;

    private void Start()
    {
        // Saving events
        availableEvents.Add(gameObject.AddComponent<BackboardEventCommon>());
        availableEvents.Add(gameObject.AddComponent<BackboardEventRare>());
        availableEvents.Add(gameObject.AddComponent<BackboardEventSuperRare>());
        Debug.Log("event: " + availableEvents[0].Name);
    }



    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            TryTriggerEvent();
        }
    }

    private void TryTriggerEvent()
    {
        if (availableEvents.Count == 0 || currentEvent != null) return;

        if (Random.value <= globalProbability)
        {
            currentEvent = PickEventByRarity();
            Debug.Log("Triggered event: " + currentEvent.Name);
        }
    }

    private GameEvent PickEventByRarity()
    {
        float roll = Random.value;
        if (roll < 0.6f)
        {
            Debug.Log("Rarity: " + EventRarity.Common.ToString());
            return availableEvents[0];
        }
        else if (roll < 0.9f)
        {
            Debug.Log("Rarity: " + EventRarity.Rare.ToString());
            return availableEvents[1];
        }
        else
        {
            Debug.Log("Rarity: " + EventRarity.SuperRare.ToString());
            return availableEvents[2];
        }
    }


    public void ResetEvent()
    {
        currentEvent = null;
    }


    /// <summary>
    /// Returns the points of the active event, if any; otherwise returns 0.
    /// </summary>
    public int GetEventPoints()
    {
        if (currentEvent != null)
        {
            int points = currentEvent.Points;
            return points;
        }
        return 0;
    }


    public bool CheckEvent()
    {
        if (currentEvent == null) return false;
        return true;
    }
}


