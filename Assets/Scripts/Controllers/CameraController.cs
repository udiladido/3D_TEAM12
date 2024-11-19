using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : BaseController
{
    private bool canRotate;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;
    private CinemachineTransposer transposer;
    private Vector2 mouseDelta;
	// body의 시작 FollowOffset
    [SerializeField] private Vector3 defaultFollowOffset;
    // 줌 속도
    [SerializeField] private float zoomSpeed = 1f;
    // composer에서 살짝 기울기
    [SerializeField] private Vector3 trackedObjectOffset = new Vector3(0, 0.6f, 0);

    private void Start()
    {
        InitCamera();
    }

    private void InitCamera()
    {
    	// 여기서는 VirtualCamera만 생성해주기 때문에 MainCamera에 CinemachineBrain을 수동으로 달아줘야 한다.
        // 동적으로 추가해도 됨..
        GameObject virtualCameraGameObject = new GameObject("VirtualCamera");
        virtualCamera = virtualCameraGameObject.AddComponent<CinemachineVirtualCamera>();
        // 따라다닐 대상
        virtualCamera.Follow = transform;
        // 주시할 대상
        virtualCamera.LookAt = transform;
        transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = defaultFollowOffset;
        // WorldSpace = 카메라가 월드 공간에서 타겟의 위치만 따라가고 회전은 영향을 받지 않습니다.
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        composer = virtualCamera.AddCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = trackedObjectOffset;
        // 밑에 값을 모두 0으로 설정하면 캐릭터가 떨리는 걸 방지할 수 있다.
        transposer.m_XDamping = 0f;
        transposer.m_YDamping = 0f;
        transposer.m_ZDamping = 0f;
        composer.m_DeadZoneWidth = 0f;
        composer.m_DeadZoneHeight = 0f;
        composer.m_HorizontalDamping = 0f;
        composer.m_VerticalDamping = 0f;

        Camera camera = Camera.main;
        CinemachineBrain brain = camera.GetOrAddComponent<CinemachineBrain>();
    }
    
    public void Zoom(float delta)
    {
        float zoomY = transposer.m_FollowOffset.y - (delta * zoomSpeed * Time.deltaTime);
        float zoomX = transposer.m_FollowOffset.x + (delta * zoomSpeed * Time.deltaTime);
        float zoomZ = transposer.m_FollowOffset.z - (delta * zoomSpeed * Time.deltaTime);
        Vector3 zoom = new Vector3(
            Mathf.Clamp(zoomX, defaultFollowOffset.x, -5),
            Mathf.Clamp(zoomY, 2, defaultFollowOffset.y),
            Mathf.Clamp(zoomZ, 5, defaultFollowOffset.z)
        );
        transposer.m_FollowOffset = zoom;
    }
}