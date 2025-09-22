using System;
using UnityEngine;


/// <summary>
/// Detects when the basketball enters the hoop 
/// </summary>
public class HoopTriggerHandler : MonoBehaviour
{
    public event Action<int> OnScore;
    [SerializeReference] private EventHandler eventHandler;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            // Check if the collider that entered has the tag "ball"
            Debug.Log("Score!");
            BallHandler ballHandler = other.GetComponent<BallHandler>();
            if(ballHandler != null)
            {
                if (ballHandler.touchedRim) Debug.Log("rimshot!");
                if (ballHandler.touchedBackboard)
                {
                    if (eventHandler.CheckEvent())
                    {

                        //check for active event, then resets
                        eventHandler.ResetEvent();
                    }
                }
            }

            OnScore?.Invoke(ballHandler.GetScore());
        }
    }

}
