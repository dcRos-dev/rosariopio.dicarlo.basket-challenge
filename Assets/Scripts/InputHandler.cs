using System;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    //invoked when swipe ends
    public event Action<float> OnSwipeEnded;


    [Header("UI Elements")]
    [SerializeField] private Slider precisionSlider;
    [SerializeField] private float maxSwipeDistance;


    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    // saving the highest Swipe Distance in the current shot to avoid lowering the slider
    private float highestSwipeDistance = 0f;


    void Update()
    {

        /*
        
        if (Application.isEditor )
        {
            // PC/Editor input
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("PC/Editor input detection");
                startTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endTouchPos = Input.mousePosition;
                SwipeEnded();
            }
        }

        if (Application.isMobilePlatform)
        {
            // Mobile input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        Debug.Log("Mobile input detection");
                        startTouchPos = touch.position;
                        break;

                    case TouchPhase.Ended:
                        endTouchPos = touch.position;
                        SwipeEnded();
                        break;
                }
            }
        }
        */


        //implementation with processor directives

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPos = touch.position;

                    //resetting
                    precisionSlider.value = 0f;
                    highestSwipeDistance = 0f;

                    break;

                case TouchPhase.Moved:
                    endTouchPos = touch.position;
                    //Debug.Log("end pos: " + endTouchPos);
                    //Debug.Log("swipeDistance:" + swipeDistance);
                    float swipeDistance = endTouchPos.y - startTouchPos.y;
                    if (swipeDistance > highestSwipeDistance)
                    {
                        highestSwipeDistance = swipeDistance;
                        UpdateSlider(swipeDistance);
                    }

                    break;

                case TouchPhase.Ended:
                    endTouchPos = touch.position;
                    SwipeEnded();
                    break;
            }
        }


#elif UNITY_EDITOR
     //PC/EDITOR INPUT DETECTION

    if (Input.GetMouseButtonDown(0))
    {
        startTouchPos = Input.mousePosition;

        // resetting
        precisionSlider.value = 0f;
        highestSwipeDistance = 0f;
    }
    else if (Input.GetMouseButton(0)) 
    {
        endTouchPos = Input.mousePosition;
        float swipeDistance = endTouchPos.y - startTouchPos.y;
        //Debug.Log("end pos: " + endTouchPos);
        //Debug.Log("swipeDistance:" + swipeDistance);
        if (swipeDistance > highestSwipeDistance)
        {
            highestSwipeDistance = swipeDistance;
            UpdateSlider(swipeDistance);
        }
    }
    else if (Input.GetMouseButtonUp(0))
    {
        endTouchPos = Input.mousePosition;
        SwipeEnded();
    }
#endif

    }


    /// <summary>
    /// Updates the slider value based on the swipe distance.
    /// </summary>
    /// <param name="swipeDistance">The swipe distance measured from the initial touch position.</param>
    /// <remarks>
    /// The distance is normalized against a predefined maximum swipe length, the resulting value is clamped between 0 and 1.
    /// </remarks>
    private void UpdateSlider(float distance)
    {
        float clampedDistance = Mathf.Clamp01(distance / maxSwipeDistance);
        precisionSlider.value = clampedDistance;
    }

    /// <summary>
    /// checks if the input qualifies as an upward swipe and, if valid,
    /// invokes "OnSwipeEnded" with the current precision slider value.
    /// </summary>
    private void SwipeEnded()
    {
        Vector2 swipeVector = endTouchPos - startTouchPos;
        if (swipeVector.magnitude < 10f) return;

        if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
        {
            if (swipeVector.y > 0)
            {
                Debug.Log("Swipe");
                OnSwipeEnded?.Invoke(precisionSlider.value);
            }
        }

    }
}



