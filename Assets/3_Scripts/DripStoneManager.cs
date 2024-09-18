using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DripStoneManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript particle;
    private BoxCollider2D boxCollider2D;

    // 他コンポーネント取得
    private StageObjectManager stageObjectManager;

    // 親オブジェクト
    private Transform parentTransform;

    // 基本情報
    private Vector2 halfSize;

    // 座標系
    private Vector3 originPosition;
    private Vector3 nextPosition;

    // 落下
    private bool isFallActive;
    private bool isFalling;
    [Header("落下")]
    [SerializeField] private float fallPowerMax;
    [SerializeField] private float addFallPower;
    private float fallPower;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleInstantiateScript>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        parentTransform = transform.parent.transform;
        halfSize.x = transform.localScale.x * 0.5f;
        halfSize.y = transform.localScale.y * 0.5f;
        originPosition = transform.position;
    }

    void Update()
    {
        if (isFallActive)
        {
            nextPosition = transform.position;

            Fall();

            transform.position = nextPosition;
        }
    }

    void Fall()
    {
        if (!isFalling)
        {
            // ブロックとの衝突判定
            bool noBlock = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y軸判定
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
                fallPower = 0f;
                isFalling = true;
            }
        }
        else
        {
            fallPower += addFallPower * Time.deltaTime;

            float deltaFallPower = fallPower * Time.deltaTime;
            nextPosition.y -= deltaFallPower;

            // ブロックとの衝突判定
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y軸判定
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.5f;

                    if (xBetween < xDoubleSize && yBetween < yDoubleSize)
                    {
                        if (nextPosition.y > obj.transform.position.y)
                        {
                            nextPosition.y = obj.transform.position.y + 0.5f + halfSize.y;
                            isFalling = false;
                            break;
                        }
                    }
                }
                else if (obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.ITEM)
                {
                    // X軸判定
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y軸判定
                    float yBetween = Mathf.Abs(nextPosition.y - obj.transform.position.y);
                    float yDoubleSize = halfSize.y + 0.3f;

                    if (xBetween < xDoubleSize && yBetween < yDoubleSize)
                    {
                        if (nextPosition.y > obj.transform.position.y)
                        {
                            obj.GetComponent<ItemManager>().Damage();
                            break;
                        }
                    }
                }
            }
        }
    }

    // 初期化処理
    void Initialize()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();

        transform.position = originPosition;
        transform.parent = parentTransform;
        fallPower = 0f;
        isFallActive = false;
        isFalling = false;
        stageObjectManager = null;
    }

    // 消滅処理
    void Disappear()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        particle.RunParticle(0);
        isFallActive = false;
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetHp() <= 0)
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
    public void FallInitialize()
    {
        transform.parent = null;
        isFallActive = true;
        isFalling = true;
        stageObjectManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageObjectManager>();
        stageObjectManager.SetIsMoving(AllObjectManager.ObjectType.DRIPSTONE, true);
    }

    // Getter
    public bool GetIsFalling()
    {
        return isFalling;
    }
}
