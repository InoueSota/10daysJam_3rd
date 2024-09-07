using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerManager playerManager;

    // ����
    private InputManager inputManager;
    private bool isPushLeft;
    private bool isPushRight;
    private bool isTriggerJump;

    // ��{���
    private Vector2 halfSize;
    private Vector2 cameraHalfSize;

    // ���W�n
    private Vector3 originPosition;
    private Vector3 nextPosition;

    // ���ړ�
    [Header("���ړ�")]
    [SerializeField] private float maxSpeed;
    private float moveSpeed;
    private Vector3 moveDirection;

    // �W�����v
    private bool isJumping;
    [Header("�W�����v")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    private float jumpTarget;

    // �؋�
    private bool isHovering;
    [Header("�؋�")]
    [SerializeField] private float hangTime;
    private float hangTimer;

    // �d��
    private bool isGravity;
    [Header("�d��")]
    [SerializeField] private float gravityMax;
    [SerializeField] private float addGravity;
    private float gravityPower;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();

        halfSize.x = transform.localScale.x * 0.5f;
        halfSize.y = transform.localScale.y * 0.5f;
        cameraHalfSize.x = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        cameraHalfSize.y = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;
        originPosition = transform.position;
    }
    public void Initialize()
    {
        transform.position = originPosition;
        isJumping = false;
        isHovering = false;
        isGravity = false;
    }

    void Update()
    {
        if (playerManager.GetIsActive())
        {
            GetInput();

            nextPosition = transform.position;

            Move();
            Jump();
            Hovering();
            Gravity();
            ClampInCamera();

            transform.position = nextPosition;
        }
    }

    void CheckDirection()
    {
        if (isPushLeft || isPushRight)
        {
            moveSpeed = maxSpeed;

            if (isPushLeft)
            {
                moveDirection = Vector3.left;
            }
            else if (isPushRight)
            {
                moveDirection = Vector3.right;
            }
        }
        else
        {
            moveSpeed = 0f;
        }
    }
    void Move()
    {
        // �ړ������̏C��
        CheckDirection();

        // �ړ�
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        nextPosition += deltaMoveSpeed * moveDirection;

        // �u���b�N�Ƃ̏Փ˔���
        if (isPushLeft || isPushRight)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.5f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.45f;

                    if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                    {
                        if (obj.transform.position.x < nextPosition.x)
                        {
                            nextPosition.x = obj.transform.position.x + 0.5f + halfSize.x;
                            break;
                        }
                        else
                        {
                            nextPosition.x = obj.transform.position.x - 0.5f - halfSize.x;
                            break;
                        }
                    }
                }
            }
        }
    }
    void Jump()
    {
        // �W�����v�J�n�Ə�����
        if (!isJumping && !isHovering && !isGravity && isTriggerJump)
        {
            jumpTarget = nextPosition.y + jumpDistance;
            isJumping = true;
        }

        // �W�����v����
        if (isJumping)
        {
            float deltaJumpSpeed = jumpSpeed * Time.deltaTime;
            nextPosition.y += (jumpTarget - nextPosition.y) * deltaJumpSpeed;

            // �W�����v�I������
            if (Mathf.Abs(jumpTarget - nextPosition.y) < 0.03f)
            {
                nextPosition.y = jumpTarget;

                hangTimer = hangTime;
                isHovering = true;
                isJumping = false;
            }

            // �u���b�N�Ƃ̏Փ˔���
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.45f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.5f;

                    if (xBetween < xDoubleSize && yBetween < yDoubleSize)
                    {
                        if (nextPosition.y < obj.transform.position.y)
                        {
                            nextPosition.y = obj.transform.position.y - 0.5f - halfSize.y;
                            isJumping = false;
                            break;
                        }
                    }
                }
            }
        }
    }
    void Hovering()
    {
        if (isHovering)
        {
            hangTimer -= Time.deltaTime;
            if (hangTimer <= 0f) { isHovering = false; }
        }
    }
    void Gravity()
    {
        if (!isGravity)
        {
            if (!isJumping && !isHovering)
            {
                // �u���b�N�Ƃ̏Փ˔���
                bool noBlock = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
                {
                    if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                    {
                        // X������
                        float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                        float xDoubleSize = halfSize.x + 0.45f;

                        // Y������
                        float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                        float yDoubleSize = halfSize.y + 0.5f;

                        if (yBetween <= yDoubleSize && xBetween < xDoubleSize)
                        {
                            if (nextPosition.y > obj.transform.position.y)
                            {
                                noBlock = false;
                                break;
                            }
                        }
                    }
                }

                if (noBlock)
                {
                    gravityPower = 0f;
                    isGravity = true;
                }
            }
        }
        else
        {
            gravityPower += addGravity * Time.deltaTime;

            float deltaGravityPower = gravityPower * Time.deltaTime;
            nextPosition.y -= deltaGravityPower;

            // �u���b�N�Ƃ̏Փ˔���
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.45f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.5f;

                    if (xBetween < xDoubleSize && yBetween < yDoubleSize)
                    {
                        if (nextPosition.y > obj.transform.position.y)
                        {
                            nextPosition.y = obj.transform.position.y + 0.5f + halfSize.y;
                            isGravity = false;
                            break;
                        }
                    }
                }
            }
        }
    }
    void ClampInCamera()
    {
        // ���[�𒴂�����
        float thisLeftX = nextPosition.x - halfSize.x;
        if (thisLeftX < -cameraHalfSize.x)
        {
            nextPosition.x = -cameraHalfSize.x + halfSize.x;
        }

        // �E�[�𒴂�����
        float thisRightX = nextPosition.x + halfSize.x;
        if (thisRightX > cameraHalfSize.x)
        {
            nextPosition.x = cameraHalfSize.x - halfSize.x;
        }
    }

    void GetInput()
    {
        isPushLeft = false;
        isPushRight = false;
        isTriggerJump = false;

        if (inputManager.IsPush(InputManager.INPUTPATTERN.HORIZONTAL))
        {
            if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.HORIZONTAL) < -0.1f)
            {
                isPushLeft = true;
            }
            else if (inputManager.ReturnInputValue(InputManager.INPUTPATTERN.HORIZONTAL) > 0.1f)
            {
                isPushRight = true;
            }
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
    }
    
    public bool GetIsGround()
    {
        if (isJumping || isHovering || isGravity)
        {
            return false;
        }
        return true;
    }

    public int GetDirection()
    {
        //�������擾
        // ��-1 �E 1 

        //�E
        if (moveDirection.x > 0 )
        {
            return 1;
        }
        else if (moveDirection.x < 0)
        {
            return -1;
        }

        return 0;
    }

    public float GetSpeed()
    {
        //�ړ����x���擾 

        return moveSpeed;
    }
}
