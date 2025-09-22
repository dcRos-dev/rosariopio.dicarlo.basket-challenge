using UnityEngine;

/// <summary>
/// Handles ball collision detection to support future scoring logic.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class BallHandler : MonoBehaviour
{
    [SerializeReference] EventHandler eventHandler;

    [HideInInspector] public bool touchedRim, touchedBackboard = false;

    private int eventScore = 0;

    private void OnCollisionEnter(Collision collision)
    {
        //check if its a backboard shot or rim shot
        if (collision.gameObject.CompareTag("rim"))
        {
            touchedRim = true;
            Debug.Log("the ball touched the rim");
        }
        if (collision.gameObject.CompareTag("backboard"))
        {
            touchedBackboard = true;
            Debug.Log("the ball touched the backboard");
            eventScore = eventHandler.GetEventPoints();
           
        }

    }



    /// <summary>
    /// Calculates and returns the score based on whether the ball touched the rim or backboard.
    /// </summary>
    public int GetScore()
    {
        return GetNormalScore() + GetEventScore();
    }

    public void ResetShot()
    {
        touchedRim = false;
        touchedBackboard = false;
        eventScore = 0;
        
    }


    private int GetNormalScore()
    {
        if (touchedBackboard || touchedRim)
        {
            return 2;
        }
        return 3;
    }

    private int GetEventScore()
    {
        if (eventScore == 0) return 0;
        return eventScore;
    }
}
