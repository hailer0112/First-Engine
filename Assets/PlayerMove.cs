using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f; // 점프 힘

    private Vector2 inputVector;
    private Rigidbody rb;
    private bool isGrounded; // 바닥 체크용

    void Start()
    {
        // 내 오브젝트에 붙어있는 Rigidbody 부품을 가져옴
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // 유니티 물리 엔진이 바닥에 닿았다고 판정했을 때만 점프
        if (isGrounded)
        {
            // Rigidbody에 위쪽 방향으로 순간적인 힘(VelocityChange)을 줍니다.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    void Update()
    {
        // 3D 이동 방향 계산 (Y축은 Rigidbody가 알아서 하므로 0으로 둠)
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDirection.magnitude > 0)
        {
            moveDirection = moveDirection.normalized;
        }

        // X, Z축 이동은 transform으로 처리
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // [Unity 6 물리 흐름] 바닥 Collider와 부딪히고 있는 동안에는 땅 위에 있다고 판정
    private void OnCollisionStay(Collision collision)
    {
        // 바닥 오브젝트의 태그가 Ground 거나, 그냥 부딪히고 있으면 땅으로 인정
        isGrounded = true;
    }
}
