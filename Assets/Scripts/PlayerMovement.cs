using UnityEngine;
using UnityEngine.EventSystems;

//public static class TagManager
//{
//    public static readonly string Player = "Player";
//}

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator animator;

    private Camera cam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        // ** 기존 코드
        //rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, playerInput.MoveH * rotationSpeed * Time.fixedDeltaTime, 0f));
        //rb.MovePosition(rb.position + transform.forward * playerInput.MoveV * moveSpeed * Time.fixedDeltaTime);
        Movement();
        Lotate();
    }

    private void Movement()
    {
        // ** 기존 코드
        //rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, playerInput.MoveH * rotationSpeed * Time.fixedDeltaTime, 0f));
        //rb.MovePosition(rb.position + transform.forward * playerInput.MoveV * moveSpeed * Time.fixedDeltaTime);


        // 카메라 기준 좌표계 벡터 가져오기
        Vector3 camForward = cam.transform.forward; // 카메라 Transform 의 -Z 방향 (파란 축)
        Vector3 camRight = cam.transform.right; // X축 방향 (빨간 축)

        // 카메라 벡터 수평면으로 투영
        // 카메라에 롤(삐딱하게 기운 회전)이 있을 때도 정확히 직교하도록 만드는 방법
        camForward.y = 0f;
        camRight.y = 0f;

        // 벡터의 길이가 변해서 이동속도가 들쭉날쭉해지기 때문
        // -> 카메라 각도에 따라 이동속도 달라짐
        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDir = new Vector3(playerInput.MoveH, 0, playerInput.MoveV);
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.sqrMagnitude > 0f)
            moveDir.Normalize();

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        float normalizedSpeed = Mathf.Clamp(rb.linearVelocity.magnitude / moveSpeed, 0f, 1f);

        animator.SetFloat(AnimIds.MoveHash, playerInput.MoveV);

        if (playerInput.MoveV == 0)
            animator.SetFloat(AnimIds.MoveHash, playerInput.MoveH);
    }

    private void Lotate()
    {
        //Vector3 mouseScreenPos = Input.mousePosition;
        //mouseScreenPos.z = cam.WorldToScreenPoint(transform.position).z;

        //Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        //Vector3 lookDirection = mouseWorldPos - transform.position;
        //lookDirection.y = 0f;

        //if (lookDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //    transform.rotation = targetRotation;
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;

            transform.LookAt(targetPos);
        }
    }
}
