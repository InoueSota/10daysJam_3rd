using UnityEngine;

public class SplitManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;

    [Header("子オブジェクト取得")]
    [SerializeField] private GameObject overObj;
    private SpriteRenderer overSpriteRenderer;
    private Transform overTransform;
    private Vector3 overOrigin;
    private Vector3 overTarget;
    [SerializeField] private GameObject underObj;
    private SpriteRenderer underSpriteRenderer;

    // 他オブジェクト取得
    private Transform playerTransform;

    // フラグ - 上にオブジェクトがあるか
    private bool isFreeOver;

    [Header("予告避け")]
    [SerializeField] private float floatHeight;
    [SerializeField] private float chasePower;
    [SerializeField] private float flowSpeed;
    [SerializeField] private float range;
    private float angle;

    [Header("避け")]
    [SerializeField] private float avoidHeight;
    [SerializeField] private float avoidTime;
    private float avoidTimer;
    private bool isAvoid;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();

        // 子オブジェクト初期化
        overSpriteRenderer = overObj.GetComponent<SpriteRenderer>();
        overTransform = overObj.transform;
        overOrigin = overObj.transform.position;
        overTarget = overOrigin;
        underSpriteRenderer = underObj.GetComponent<SpriteRenderer>();

        // プレイヤーの位置を取得
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 初期化時は上にオブジェクトがない判定にする
        isFreeOver = true;

        // sin運動フワフワ初期化
        angle = 0;
    }

    void Update()
    {
        if (!isAvoid)
        {
            NoticeFreeOver();
        }
        else
        {
            Avoid();
        }
    }
    void NoticeFreeOver()
    {
        float yBetween = Mathf.Abs(transform.position.y - playerTransform.position.y + 1f);

        if (isFreeOver && yBetween < 0.4f)
        {
            overTarget.y = overOrigin.y + floatHeight;

            // Targetをsin運動させる
            angle += flowSpeed * Time.deltaTime;
            overTarget.y += Mathf.Sin(angle) * range;
        }
        else
        {
            overTarget = overOrigin;
            angle = 0f;
        }

        // Targetに向かって追い続ける
        overTransform.position += (overTarget - overTransform.position) * (chasePower * Time.deltaTime);
    }
    void Avoid()
    {
        avoidTimer -= Time.deltaTime;
        if (avoidTimer <= 0f) { isAvoid = false; }

        // Targetに向かって追い続ける
        overTransform.position += (overTarget - overTransform.position) * (chasePower * Time.deltaTime);
    }

    // 初期化処理
    void Initialize()
    {
        overSpriteRenderer.enabled = true;
        underSpriteRenderer.enabled = true;
        allObjectManager.SetIsActive(overSpriteRenderer.enabled);
        allObjectManager.Initialize();

        angle = 0;
    }

    // 消滅処理
    void Disappear()
    {
        overSpriteRenderer.enabled = false;
        underSpriteRenderer.enabled = false;
        allObjectManager.SetIsActive(false);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
        }
    }
    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            Initialize();
        }
        else
        {
            Disappear();
        }
    }
    public void SetIsFreeOver(bool _isFreeOver)
    {
        isFreeOver = _isFreeOver;
    }

    // Getter
    public bool GetCanHit()
    {
        // trueなら避けフラグをtrueにする
        if (isFreeOver)
        {
            overTarget.y = overOrigin.y + avoidHeight;
            avoidTimer = avoidTime;
            isAvoid = true;
        }
        return !isFreeOver;
    }
}
