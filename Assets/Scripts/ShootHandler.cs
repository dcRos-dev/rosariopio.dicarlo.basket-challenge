using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class ShootHandler : MonoBehaviour
{
    [Header("shooting values")]
    public float Angle;
    public float ShotDuration;

    [Space]
    [Header("References")]
    [SerializeField] private Transform[] targetPositions;
    [SerializeField] private Transform[] shootingTransforms;
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform playerContainer;
    [SerializeField] private InputHandler inputHandler;

    private BallHandler ballHandler;
    private Transform targetPosition;
    private Vector3 perfectVel = Vector3.zero;
    private Vector3 currentBallStartingPosition = Vector3.zero;
    private Rigidbody ballRb;
    private int prevIndex = -1;

    private void OnEnable()
    {
        inputHandler.OnSwipeEnded += Shot;
    }

    private void OnDisable()
    {
        inputHandler.OnSwipeEnded -= Shot;
    }

    void Start()
    {
        //Set initial shooting position for testing the perfect shot calculation
        if (shootingTransforms.Length.Equals(0) || targetPositions.Length.Equals(0)) return;

        //ball references
        ballRb = ball.GetComponent<Rigidbody>();
        ballHandler = ball.GetComponent<BallHandler>();

        //initially setting player to first position
        currentBallStartingPosition = ball.transform.position;
        ChangePlayerPos(0);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Sets the player's position to a random spot, avoiding repetition.
    /// </summary>
    public void RandomPlayerPos()
    {
        int randomIndex;

        // Looping to avoid selecting the same position 
        // checking the list length to prevent infinite looping.
        do
        {
            randomIndex = Random.Range(0, shootingTransforms.Length);
        } while (randomIndex == prevIndex && shootingTransforms.Length > 1);
        
        ChangePlayerPos(randomIndex);
    }

    /// <summary>
    /// sets the player's position to a specific spot based on the given index.
    /// </summary>
    public void ChangePlayerPos(int positionIndex)
    {
        if (shootingTransforms[positionIndex] != null)
        {
            ResetBallPosition(); //resetting ball position before changing player shooting spot

            playerContainer.position = shootingTransforms[positionIndex].position;
            playerContainer.rotation = shootingTransforms[positionIndex].rotation;

            //saving the new reset position
            SaveCurrentShootingPosition();
            prevIndex = positionIndex;
        }
        else
        {
            Debug.Log("null");
        }
    }


    /*
    public void Shot()
    {
        //Vector3 force = GetPerfectVelocity(currentShootingTransform);
        if (perfectVel == Vector3.zero) return;

        Vector3 force = perfectVel * ballRb.mass;
        if (force == Vector3.zero) return;
        Debug.Log("force: " + force);

        ballRb.isKinematic = false;
        ballRb.velocity = Vector3.zero; 
        ballRb.AddForce(force, ForceMode.Impulse);
    }
    */

    [Tooltip("shoot the ball")]
    public void Shot(float sliderValue)
    {
        float forceMultiplier = 1f;

        // Get the target position and force multiplier based on the precision value
        Vector3 targetPos = GetTargetByPrecisionValue(sliderValue,out forceMultiplier);

        // Calculate the initial velocity needed to reach the target
        // Changed logic to consider the ball's position based on different shooting spots 
        Vector3 velocity = GetPerfectVelocity(ball.transform.position, targetPos);
        Vector3 finalForce = velocity * ballRb.mass * forceMultiplier;

        // Apply force to shoot the ball
        ballRb.isKinematic = false;
        ballRb.velocity = Vector3.zero;
        ballRb.AddForce(finalForce, ForceMode.Impulse);

        StartCoroutine(ResetShotAfterDelay());
    }

    /// <summary>
    /// reset ball for the next shot
    /// </summary>
    public void ResetBallPosition()
    {
        ballHandler.ResetShot();
        ballRb.isKinematic = true;
        ballRb.velocity = Vector3.zero;
        ball.transform.position = currentBallStartingPosition;
    }

    /// <summary>
    /// Calculates a target position based on the player's shot precision (slider value).
    /// Determines whether the shot is a perfect hit, backboard, rim or a miss.
    /// Applies a randomized offset to simulate shot accuracy.
    /// Also outputs a force multiplier to adjust shot strength based on slider value.
    /// 
    /// The returned position includes a randomized offset based on shot deviation.
    /// </summary>

    private Vector3 GetTargetByPrecisionValue(float sliderValue, out float forceMultiplier)
    {
        //slider value for a perfect shot and backboard shot
        float perfectCenter = 0.5f;
        float backboardCenter = 0.7f;

        //calculating distances and checking the closest one: true for perfect shot, false per backboard shot
        float distPerfect = Mathf.Abs(sliderValue - perfectCenter);
        float distBackboard = Mathf.Abs(sliderValue - backboardCenter);
        bool closestIsPerfect = distPerfect < distBackboard;

        //getting the delta distance from the closest target
        float delta = closestIsPerfect ? distPerfect : distBackboard;

        Transform finalTarget;
        float spreadFactor = 0f;
        forceMultiplier = 1f; 

        if (delta < 0.02f)
        {
            // Very close to the target: either a perfect shot or a backboard shot
            finalTarget = closestIsPerfect ? targetPositions[0] : targetPositions[1];
            delta = 0f;
            Debug.Log("perfect shot or backboard shot");
        }
        else if (delta < 0.05f && closestIsPerfect )
        {
            //rim shot
            delta = 0f;
            finalTarget = targetPositions[2]; 
            Debug.Log("rim shot");
        }
        else
        {
            Debug.Log("miss");
            finalTarget = targetPositions[0];

            // Maximum spread for poor haim
            spreadFactor = 1.5f;

            if (sliderValue < perfectCenter)
            {
                forceMultiplier = 1f - Mathf.Clamp(delta, 0f, 0.5f);
            }
            else if(sliderValue > backboardCenter)
            {
                forceMultiplier = 1f + Mathf.Clamp(delta , 0f, 0.5f);

            }
        }

        //Vector3 offset = Random.insideUnitSphere * delta * spreadFactor;
        Vector3 offset = Random.onUnitSphere*spreadFactor;
        return finalTarget.position + offset;
    }


    [Tooltip("given starting and target positions, return the perfect velocity for that specific shot")]
    private Vector3 GetPerfectVelocity(Vector3 startPos, Vector3 targetPos)
    {
        //Debug.Log("target position: " + targetPos);
        //Debug.Log("start position: " + startPos);

        float g = Mathf.Abs(Physics.gravity.y);
        //Debug.Log("gravity : " + g);

        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarStart = new Vector3(startPos.x, 0, startPos.z);

        //getting the distance and height diff 
        float d = Vector3.Distance(targetPos, startPos);
        float h = targetPos.y - startPos.y; 
        //Debug.Log("distance: " + d);
        //Debug.Log("height difference: " + h);

        float angleRad = Angle * Mathf.Deg2Rad;

        // Compute the initial velocity v0 by using projectile equations with known distance, height diff and starting angle.
        float v0 = d * Mathf.Sqrt(g / (2 * Mathf.Pow(Mathf.Cos(angleRad), 2) * (d * Mathf.Tan(angleRad) - h)));
        //Debug.Log("initial velocity V_zero: " + v0);


        //returning force
        Vector3 dir = (planarTarget - planarStart).normalized;
        Vector3 velocity = dir * Mathf.Cos(angleRad) * v0
                 + Vector3.up * Mathf.Sin(angleRad) * v0;
        //Debug.Log("final velocity: " + velocity);

        return velocity * ballRb.mass;
    }

    /// <summary>
    /// Stores the ball's current transform to reset it after each shot.
    /// </summary>
    private void SaveCurrentShootingPosition()
    {
        currentBallStartingPosition = ball.transform.position;
        //currentBallStartingPosition.rotation = ball.transform.rotation;
    }

    /// <summary>
    /// Change the player position after waiting for fixed time 
    /// </summary>
    private IEnumerator ResetShotAfterDelay()
    {
        yield return new WaitForSeconds(ShotDuration);
        RandomPlayerPos();
    }
}
