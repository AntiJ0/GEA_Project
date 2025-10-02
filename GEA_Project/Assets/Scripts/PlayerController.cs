using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;  // 달리기 속도
    public float jumpPower = 5f;
    public float gravity = -9.81f;

    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 10f;
    private CinemachinePOV pov;
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;

    public int maxHp = 10;
    private int currentHp;

    // 달리기 체크용 변수
    private bool isSprinting;

    // FreeLook 카메라 줌 조정
    public CinemachineFreeLook freeLookCam;
    public float zoomOutFOV = 60f; // 달릴 때 줌 아웃되는 FOV 값
    private float defaultFOV;

    public CinemachineSwitcher camSwitcher;

    // CinemachineSwitcher 스크립트와 연결할 변수
    public bool usingFreeLook = false;

    public Slider hpSlider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();

        // FreeLook 카메라의 기본 FOV 저장
        defaultFOV = freeLookCam.m_Lens.FieldOfView;
        currentHp = maxHp;
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pov.m_HorizontalAxis.Value = transform.eulerAngles.y;
            pov.m_VerticalAxis.Value = 0f;
        }

        if (camSwitcher != null)
            usingFreeLook = camSwitcher.usingFreeLook;

        if (usingFreeLook)
            return;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * z + camRight * x).normalized;
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        Vector3 moveDirection = move * currentSpeed;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = moveDirection;
        finalMove.y = velocity.y;

        controller.Move(finalMove * Time.deltaTime);

        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        freeLookCam.m_Lens.FieldOfView = isSprinting ? zoomOutFOV : defaultFOV;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = (float)currentHp / maxHp;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}