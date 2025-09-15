using UnityEngine;

public class InputHandler : MonoBehaviour
{

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    void Start()
    {
    }

    void Update()
    {

        
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

        /*Implementation using preprocessor directives..
         

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("mobile input detection");
                    startTouchPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    endTouchPos = touch.position;
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
            Debug.Log("PC/EDITOR input detection");
            startTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            SwipeEnded();
        }
#endif

        */
    }


    private void SwipeEnded()
    {
        Vector2 swipeVector = endTouchPos - startTouchPos;
        if (swipeVector.magnitude < 10f) return;

        if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
        {
            if (swipeVector.y > 0)
                Debug.Log("Swipe");
        }

    }
}



