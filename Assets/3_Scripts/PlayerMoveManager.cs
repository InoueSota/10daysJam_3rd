using DG.Tweening;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerManager playerManager;
    private InputManager inputManager;
    private PlayerAnimationManager playerAnimationManager;
    private AudioSource audioSource;
    private bool isPushLeft;
    private bool isPushRight;
    private bool isTriggerJump;

    //Undo用
    public Vector3 lastGroundpos;

    // 基本情報
    private Vector2 halfSize;
    private Vector2 cameraHalfSize;

    // 座標系
    private Vector3 originPosition;
    private Vector3 nextPosition;

    [Header("横移動")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float accelerationTime;
    private float moveSpeed;
    private float acceleration;
    private float accelerationTimer;
    private bool canAcceleration;
    private Vector3 moveDirection;

    [Header("ジャンプ")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float cropJumpCoolTime;
    private bool isJumping;
    private float jumpTarget;
    private float cropJumpCoolTimer;

    [Header("滞空")]
    [SerializeField] private float hangTime;
    [SerializeField] private float hitHeadHangTime;
    private bool isHovering;
    private float hangTimer;

    [Header("重力")]
    [SerializeField] private float gravityMax;
    [SerializeField] private float addGravity;
    private bool isGravity;
    private float gravityPower;

    [Header("サボテン吹っ飛び")]
    private Vector3 cactusTarget;
    private Vector3 cactusDirection;
    private bool isCactus;
    [Header("１秒で吹っ飛ぶマス数")]
    [SerializeField] private float cactusAmount;

    [Header("１秒で移動するマス数")]
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
            // 最初にワープする
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

            // CropJump対策
            cropJumpCoolTimer -= Time.deltaTime;

            //ここが設置判定か・・・？？
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
        // 移動方向の修正
        CheckDirection();

        // 加速
        Acceleration();

        // 移動
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        nextPosition += deltaMoveSpeed * moveDirection;

        // ブロックとの衝突判定
        if (isPushLeft || isPushRight)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X軸判定
                float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                float xDoubleSize = halfSize.x + 0.5f;

                // Y軸判定
                float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                float yDoubleSize = halfSize.y + 0.25f;

                // 衝突対象がサボテンだったら吹っ飛ぶようにする
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
        // ジャンプ開始と初期化
        if (playerManager.GetCanJump() && cropJumpCoolTimer <= 0f && !isWarping && !isCactus && !isJumping && !isHovering && !isGravity && isTriggerJump)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (nextPosition.y > obj.transform.position.y)
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.5f;

                    // Y軸判定
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

        // ジャンプ処理
        if (isJumping)
        {
            float deltaJumpSpeed = jumpSpeed * Time.deltaTime;
            nextPosition.y += (jumpTarget - nextPosition.y) * deltaJumpSpeed;

            // ジャンプ終了処理
            if (Mathf.Abs(jumpTarget - nextPosition.y) < 0.03f)
            {
                nextPosition.y = jumpTarget;

                hangTimer = hangTime;
                isHovering = true;
                isJumping = false;
            }

            // ブロックとの衝突判定
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.26f;

                    // Y軸判定
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
                // ブロックとの衝突判定
                bool noBlock = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
                {
                    if (nextPosition.y > obj.transform.position.y)
                    {
                        // X軸判定
                        float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                        float xDoubleSize = halfSize.x + 0.25f;

                        // Y軸判定
                        float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                        float yDoubleSize = halfSize.y + 0.51f;

                        if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                        {
                            // 衝突対象がサボテンだったら吹っ飛ぶようにする
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

            // ブロックとの衝突判定
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (nextPosition.y > obj.transform.position.y)
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y軸判定
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.51f;

                    // 衝突対象がサボテンだったら吹っ飛ぶようにする
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
            // ブロックがなかったら次に進める
            cactusTarget += cactusDirection;

            // 吹っ飛びの終着点等を取得する
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X軸判定
                float xBetween = Mathf.Abs(cactusTarget.x - obj.transform.position.x);
                float xDoubleSize = halfSize.x + 0.25f;

                // Y軸判定
                float yBetween = Mathf.Abs(cactusTarget.y - obj.transform.position.y);
                float yDoubleSize = halfSize.y + 0.25f;

                if (yBetween < yDoubleSize && xBetween < xDoubleSize)
                {
                    // ブロックがあったら１つ手前で止める
                    if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetIsHitObject())
                    {
                        cactusTarget = obj.transform.position;
                        cactusTarget -= cactusDirection;
                        isCactus = true;
                        break;
                    }

                }
            }

            // もし画面内にオブジェクトがないのなら画面端で止める

            // 左端を超えたか
            float thisLeftX = cactusTarget.x - halfSize.x;
            if (thisLeftX < -cameraHalfSize.x)
            {
                cactusTarget.x = -cameraHalfSize.x + halfSize.x;
                isCactus = true;
                break;
            }

            // 右端を超えたか
            float thisRightX = cactusTarget.x + halfSize.x;
            if (thisRightX > cameraHalfSize.x)
            {
                cactusTarget.x = cameraHalfSize.x - halfSize.x;
                isCactus = true;
                break;
            }

            // 上端を超えたか
            float thisTopY = cactusTarget.y - halfSize.y;
            if (thisTopY > cameraHalfSize.y)
            {
                cactusTarget.y = cameraHalfSize.y - halfSize.y;
                isCactus = true;
                break;
            }

            // 下端を超えたか
            float thisBottomY = cactusTarget.y + halfSize.y;
            if (thisBottomY < -cameraHalfSize.y)
            {
                cactusTarget.y = -cameraHalfSize.y + halfSize.y;
                isCactus = true;
                break;
            }
        }

        // １マス隙間の場合は吹っ飛ばないようにする
        if (Vector3.Distance(nextPosition, cactusTarget) < 0.5f)
        {
            nextPosition = transform.position;
            isCactus = false;
        }
        else
        {
            transform.position = nextPosition;

            // サボテンに当たった
            cactusManager.SetHit();

            // 距離によって移動速度が変わらないように調整
            float cactusTime = Vector3.Distance(nextPosition, cactusTarget) / cactusAmount;

            // 吹っ飛び開始
            transform.DOMove(cactusTarget, cactusTime).SetEase(Ease.OutSine).OnComplete(FinishCactus);

            // 吹っ飛び以外のフラグ類を初期化する
            isJumping = false;
            isHovering = false;
            isGravity = false;
        }
    }
    void FinishCactus()
    {
        // ブロックとプレイヤーの間座標
        Vector3 betweenPosition = transform.position + cactusDirection * 0.5f;

        //
        // betweenPositionを使ってやれや！
        // ちなみに「cactusDirection」は、吹っ飛んでる方向です
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
            // ワープ以外のフラグ類を初期化する
            isJumping = false;
            isHovering = false;
            isGravity = false;

            // 距離によって移動速度が変わらないように調整
            float warpTime = Vector3.Distance(nextPosition, warpPosition) / warpAmount;
            // ワープ開始
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
        // 左端を超えたか
        float thisLeftX = nextPosition.x - halfSize.x;
        if (thisLeftX < -cameraHalfSize.x)
        {
            nextPosition.x = -cameraHalfSize.x + halfSize.x;
        }

        // 右端を超えたか
        float thisRightX = nextPosition.x + halfSize.x;
        if (thisRightX > cameraHalfSize.x)
        {
            nextPosition.x = cameraHalfSize.x - halfSize.x;
        }

        // 上端を超えたか
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

            // 今のままの方が遠いなら新ワープ座標を取得する
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
        //向きを取得
        // 左-1 右 1 

        //右
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
        //移動速度を取得 
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
