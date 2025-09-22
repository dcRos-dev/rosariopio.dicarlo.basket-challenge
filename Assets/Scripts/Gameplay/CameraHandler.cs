using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private CinemachineVirtualCamera currentPlayerCamera;
    [SerializeReference] private ColliderCameraHandler colliderCameraHandler;
    [Space(2)]
    [Header("ballCamera References")]
    [SerializeField] private Transform ball;
    [SerializeField] private Transform lookAtTarget;


    //Defining blending modes: player -> ball (EaseInOut) && ball -> player (cut)
    private CinemachineBlendDefinition blendEaseInOut = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
    private CinemachineBlendDefinition blendCut = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        colliderCameraHandler.OnCameraCollider += SetBallFollowing;
    }

    private void OnDisable()
    {
        colliderCameraHandler.OnCameraCollider += SetBallFollowing;
    }


    /// <summary>
    /// switch to Ball camera
    /// </summary>
    public void SwitchToBallCamera()
    {
        // EaseInOut
        SetCameraBlend(true);
        ballCamera.Priority++;
        currentPlayerCamera.Priority--;
    }

    public void SwitchToPlayerCamera()
    {
        //cut
        SetCameraBlend(false);
        // restoring 
        SetBallFollowing(true);

        currentPlayerCamera.Priority++;
        ballCamera.Priority--;
    }

    private void SetBallFollowing(bool follow)
    {
        if (follow)
        {
            //resuming ball following
            ballCamera.LookAt = ball;
            ballCamera.Follow = ball;

        }
        else
        {
            //stop ball following
            ballCamera.LookAt = lookAtTarget;
            ballCamera.Follow = null;
        }
    }


    public void SetCameraBlend(bool isShooting)
    {
        brain.m_DefaultBlend = isShooting ? blendEaseInOut : blendCut;
    }

}
