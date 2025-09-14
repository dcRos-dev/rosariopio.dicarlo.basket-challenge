using UnityEngine;


/// <summary>
/// Detects when the basketball enters the hoop 
/// </summary>
public class HoopTriggerHandler : MonoBehaviour
{


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
        }
    }
}
