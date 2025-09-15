using UnityEngine;

/// <summary>
/// Handles ball collision detection to support future scoring logic.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class BallHandler : MonoBehaviour
{
    [HideInInspector] public bool touchedRim, touchedBackboard = false;

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
        }

    }

    public void ResetShot()
    {
        touchedRim = false;
        touchedBackboard = false;
    }
}
