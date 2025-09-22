using System;
using UnityEngine;

public class ColliderCameraHandler : MonoBehaviour
{

    public event Action<bool> OnCameraCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            //set "follow" to false
            OnCameraCollider?.Invoke(false);
        }
    }
}
