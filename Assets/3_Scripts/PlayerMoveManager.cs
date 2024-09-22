using DG.Tweening;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerManager playerManager;
    private InputManager inputManager;
    private PlayerAnimationManager playerAnimationManager;
    private AudioSource audioSource;
    private bool isPushLeft;
    private bool isPushRight;
    private bool isTriggerJump;

    //Undo�p
    public Vector3 lastGroundpos;

    // ��{���
    private Vector2 halfSize;
    private Vector2 cameraHalfSize;

    // ���W�n
    private Vector3 originPosition;
    private Vector3 nextPosition;

    [Header("���ړ�")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float accelerationTime;
    private float moveSpeed;
    private float acceleration;
    private float accelerationTimer;
    private bool canAcceleration;
    private Vector3 moveDirection;

    [Header("�W�����v")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float cropJumpCoolTime;
    private bool isJumping;
    private float jumpTarget;
    private float cropJumpCoolTimer;

    [Header("�؋�")]
    [SerializeField] private float hangTime;
    [SerializeField] private float hitHeadHangTime;
    private bool isHovering;
    private float hangTimer;

    [Header("�d��")]
    [SerializeField] private float gravityMax;
    [SerializeField] private float addGravity;
    private bool isGravity;
    private float gravityPower;

    [Header("�T�{�e���������")]
    private Vector3 cactusTarget;
    private Vector3 cactusDirection;
    private bool isCactus;
    [Header("�P�b�Ő�����ԃ}�X��")]
    [SerializeField] private float cactusAmount;

    [Header("�P�b�ňړ�����}�X��")]
    [SerializeField] private float warpAmount;
    private bool isWarp;
    private bool isWarping;
    [SerializeField] Ease warpEase;
    private Vector3 warpPosition;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        audioSource = GetComponent<AudioSource>();

        halfSize.x = transform.localScale.x * 0.5f;
        halfSize.y = transform.localScale.y * 0.5f;
        cameraHalfSize.x = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        cameraHalfSize.y = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;
        originPosition = transform.position;

        isCactus = false;

        isWarp = false;
        warpPosition = Vector3.zero;
    }
    public void Initialize()
    {
        transform.position = originPosition;
        isJumping = false;
        isHovering = false;
        isGravity = false;
        isCactus = false;
        isWarp = false;
        isWarping = false;
        warpPosition = Vector3.zero;
        transform.DOKill();
    }

    void Update()
    {
        if (playerManager.GetIsActive())
        {
            // �ŏ��Ƀ��[�v����
            Warp();

            isPushLeft = false;
            isPushRight = false;
            isTriggerJump = false;

            if (playerManager.GetCanGetInput())
            {
                GetInput();
            }

            nextPosition = transform.position;

            Move();
            Jump();
            Hovering();
            Gravity();

            ClampInCamera();

            transform.position = nextPosition;

            // CropJump�΍�
            cropJumpCoolTimer -= Time.deltaTime;

            //�������ݒu���肩�E�E�E�H�H
            if (GetIsGround())
            {
                lastGroundpos = transform.position;

            }
        }
    }

    void CheckDirection()
    {
        if (!isWarping && !isCactus && (isPushLeft || isPushRight))
        {
            moveSpeed = maxSpeed + acceleration;

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
            acceleration = 0f;
            canAcceleration = false;
            moveSpeed = 0f;
        }
    }
    void Acceleration()
    {
        if (!canAcceleration && GetIsGround())
        {
            accelerationTimer = accelerationTime;
            canAcceleration = true;
        }
        else if (canAcceleration && !GetIsGround())
        {
            acceleration = 0f;
            canAcceleration = false;
        }

        if (canAcceleration)
        {
            accelerationTimer -= Time.deltaTime;
            accelerationTimer = Mathf.Clamp(accelerationTimer, 0f, accelerationTime);
            acceleration = Mathf.Lerp(maxAcceleration, 0f, accelerationTimer / accelerationTime);
        }
    }
    void Move()
    {
        // �ړ������̏C��
        CheckDirection();

        // ����
        Acceleration();

        // �ړ�
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        nextPosition += deltaMoveSpeed * moveDirection;

        // �u���b�N�Ƃ̏Փ˔���
        if (isPushLeft || isPushRight)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X������
                float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                float xDoubleSize = halfSize.x + 0.5f;

                // Y������
                float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                float yDoubleSize = halfSize.y + 0.25f;

                // �ՓˑΏۂ��T�{�e���������琁����Ԃ悤�ɂ���
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.CACTUS)
                {
                    if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                    {
                        if (nextPosition.x > obj.transform.position.x)
                        {
                            nextPosition.x = obj.transform.position.x + 0.5f + halfSize.x;
                            cactusDirection = Vector3.right;
                        }
                        else
                        {
                            nextPosition.x = obj.transform.position.x - 0.5f - halfSize.x;
                            cactusDirection = Vector3.left;
                        }

                        cactusTarget = obj.transform.position;
                        transform.position = nextPosition;
                        nextPosition.y = cactusTarget.y;

                        CactusInitialize(obj.GetComponent<CactusManager>());
                        break;
                    }
                }
                else if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                {
                    if (!isJumping)
                    {
                        yDoubleSize = halfSize.y + 0.15f;
                    }

                    if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                    {
                        if (nextPosition.x > obj.transform.position.x)
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
        if (playerManager.GetCanJump() && cropJumpCoolTimer <= 0f && !isWarping && !isCactus && !isJumping && !isHovering && !isGravity && isTriggerJump)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (nextPosition.y > obj.transform.position.y)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.5f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.51f;

                    if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                    {
                        audioSource.Play();
                        jumpTarget = nextPosition.y + jumpDistance;
                        isJumping = true;
                        break;
                    }
                }
            }
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
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.26f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.5f;

                    if (xBetween < xDoubleSize && yBetween < yDoubleSize)
                    {
                        if (nextPosition.y < obj.transform.position.y)
                        {
                            nextPosition.y = obj.transform.position.y - 0.5f - halfSize.y;
                            hangTimer = hitHeadHangTime;
                            isHovering = true;
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
            if (!isWarping && !isCactus && !isJumping && !isHovering)
            {
                // �u���b�N�Ƃ̏Փ˔���
                bool noBlock = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
                {
                    if (nextPosition.y > obj.transform.position.y)
                    {
                        // X������
                        float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                        float xDoubleSize = halfSize.x + 0.25f;

                        // Y������
                        float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                        float yDoubleSize = halfSize.y + 0.51f;

                        if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                        {
                            // �ՓˑΏۂ��T�{�e���������琁����Ԃ悤�ɂ���
                            if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.CACTUS)
                            {
                                cactusTarget = obj.transform.position;
                                transform.position = nextPosition;
                                nextPosition.x = cactusTarget.x;
                                cactusDirection = Vector3.up;
                                CactusInitialize(obj.GetComponent<CactusManager>());
                                noBlock = false;
                                break;
                            }
                            else if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
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
                if (nextPosition.y > obj.transform.position.y)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y������
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.51f;

                    // �ՓˑΏۂ��T�{�e���������琁����Ԃ悤�ɂ���
                    if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.CACTUS)
                    {
                        if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                        {
                            cactusTarget = obj.transform.position;
                            transform.position = nextPosition;
                            nextPosition.x = cactusTarget.x;
                            cactusDirection = Vector3.up;
                            CactusInitialize(obj.GetComponent<CactusManager>());
                            break;
                        }
                    }
                    else if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                    {
                        xDoubleSize = halfSize.x + 0.25f;

                        if (yBetween <= yDoubleSize && xBetween < xDoubleSize)
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
    void CactusInitialize(CactusManager cactusManager)
    {
        while (!isCactus)
        {
            // �u���b�N���Ȃ������玟�ɐi�߂�
            cactusTarget += cactusDirection;

            // ������т̏I���_�����擾����
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X������
                float xBetween = Mathf.Abs(cactusTarget.x - obj.transform.position.x);
                float xDoubleSize = halfSize.x + 0.25f;

                // Y������
                float yBetween = Mathf.Abs(cactusTarget.y - obj.transform.position.y);
                float yDoubleSize = halfSize.y + 0.25f;

                if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                {
                    // �u���b�N����������P��O�Ŏ~�߂�
                    if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                    {
                        cactusTarget = obj.transform.position;
                        cactusTarget -= cactusDirection;
                        isCactus = true;
                        break;
                    }

                }
            }

            // ������ʓ��ɃI�u�W�F�N�g���Ȃ��̂Ȃ��ʒ[�Ŏ~�߂�

            // ���[�𒴂�����
            float thisLeftX = cactusTarget.x - halfSize.x;
            if (thisLeftX < -cameraHalfSize.x)
            {
                cactusTarget.x = -cameraHalfSize.x + halfSize.x;
                isCactus = true;
                break;
            }

            // �E�[�𒴂�����
            float thisRightX = cactusTarget.x + halfSize.x;
            if (thisRightX > cameraHalfSize.x)
            {
                cactusTarget.x = cameraHalfSize.x - halfSize.x;
                isCactus = true;
                break;
            }

            // ��[�𒴂�����
            float thisTopY = cactusTarget.y - halfSize.y;
            if (thisTopY > cameraHalfSize.y)
            {
                cactusTarget.y = cameraHalfSize.y - halfSize.y;
                isCactus = true;
                break;
            }

            // ���[�𒴂�����
            float thisBottomY = cactusTarget.y + halfSize.y;
            if (thisBottomY < -cameraHalfSize.y)
            {
                cactusTarget.y = -cameraHalfSize.y + halfSize.y;
                isCactus = true;
                break;
            }
        }

        // �P�}�X���Ԃ̏ꍇ�͐�����΂Ȃ��悤�ɂ���
        if (Vector3.Distance(nextPosition, cactusTarget) < 0.5f)
        {
            nextPosition = transform.position;
            isCactus = false;
        }
        else
        {
            transform.position = nextPosition;

            // �T�{�e���ɓ�������
            cactusManager.SetHit();

            // �����ɂ���Ĉړ����x���ς��Ȃ��悤�ɒ���
            float cactusTime = Vector3.Distance(nextPosition, cactusTarget) / cactusAmount;

            // ������ъJ�n
            transform.DOMove(cactusTarget, cactusTime).SetEase(Ease.OutSine).OnComplete(FinishCactus);

            // ������шȊO�̃t���O�ނ�����������
            isJumping = false;
            isHovering = false;
            isGravity = false;
        }
    }
    void FinishCactus()
    {
        // �u���b�N�ƃv���C���[�̊ԍ��W
        Vector3 betweenPosition = transform.position + cactusDirection * 0.5f;

        //
        // betweenPosition���g���Ă���I
        // ���Ȃ݂ɁucactusDirection�v�́A�������ł�����ł�
        //

        float rot = Mathf.Atan2(cactusDirection.y, cactusDirection.x) * Mathf.Rad2Deg;

        playerAnimationManager.RunWallHitParticle(betweenPosition, rot);

        gravityPower = 0f;
        isGravity = true;
        isCactus = false;
    }
    void Warp()
    {
        if (isWarp)
        {
            // ���[�v�ȊO�̃t���O�ނ�����������
            isJumping = false;
            isHovering = false;
            isGravity = false;

            // �����ɂ���Ĉړ����x���ς��Ȃ��悤�ɒ���
            float warpTime = Vector3.Distance(nextPosition, warpPosition) / warpAmount;
            // ���[�v�J�n
            transform.DOMove(warpPosition, warpTime).SetEase(warpEase).OnComplete(FinishWarp);
            isWarping = true;
            isWarp = false;
        }
    }
    void FinishWarp()
    {
        warpPosition = Vector3.zero;
        isWarping = false;
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

        // ��[�𒴂�����
        if (nextPosition.y > cameraHalfSize.y)
        {
            nextPosition.y = cameraHalfSize.y;
            hangTimer = hangTime;
            isHovering = true;
            isJumping = false;
        }
    }

    // Setter
    public void SetDeathWarp(Vector3 _warpPosition)
    {
        if (warpPosition == Vector3.zero)
        {
            warpPosition = _warpPosition;
            isWarp = true;
        }
        else
        {
            float nowDistance = Vector3.Distance(warpPosition, nextPosition);
            float nextDistance = Vector3.Distance(_warpPosition, nextPosition);

            // ���̂܂܂̕��������Ȃ�V���[�v���W���擾����
            if (nowDistance > nextDistance)
            {
                warpPosition = _warpPosition;
            }
        }
    }
    public void SetCropJumpTimer()
    {
        cropJumpCoolTimer = cropJumpCoolTime;
    }

    // Getter
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
        if (isJumping || isHovering || isGravity || isCactus || isWarp || isWarping)
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
        if (moveDirection.x > 0)
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
    public bool GetIsMoving()
    {
        if (isPushLeft || isPushRight)
        {
            return true;
        }
        return false;
    }
    public bool GetIsJump()
    {
        return isJumping;
    }
    public bool GetIsHovering()
    {
        return isHovering;
    }
    public bool GetIsGravity()
    {
        return isGravity;
    }
    public bool GetIsCactus()
    {
        return isCactus;
    }

    public bool GetIsWarping()
    {
        return isWarping;
    }
}
