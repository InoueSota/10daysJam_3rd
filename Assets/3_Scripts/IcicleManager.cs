using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

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
        if (isFalling)
        {
            fallPower += addFallPower * Time.deltaTime;

            float deltaFallPower = fallPower * Time.deltaTime;
            nextPosition.y -= deltaFallPower;

            // ブロックとの衝突判定
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                if (obj != gameObject && hitAllObjectManager.GetIsActive() && hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.ITEM)
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
                            switch (hitAllObjectManager.GetObjectType())
                            {
                                case AllObjectManager.ObjectType.BLOCK:

                                    obj.GetComponent<BlockManager>().Damage();

                                    break;
                                case AllObjectManager.ObjectType.ITEM:

                                    obj.GetComponent<ItemManager>().Damage();

                                    break;
                                case AllObjectManager.ObjectType.DRIPSTONEBLOCK:

                                    obj.transform.GetChild(0).GetComponent<DripStoneManager>().FallInitialize();
                                    obj.GetComponent<BlockManager>().Damage();

                                    break;
                                case AllObjectManager.ObjectType.DRIPSTONE:

                                    obj.GetComponent<DripStoneManager>().Damage();

                                    break;
                                case AllObjectManager.ObjectType.BOMB:

                                    obj.GetComponent<BombManager>().Damage();

                                    break;
                            }
                            Damage();
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
    void Destruction()
    {
        spriteRenderer.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
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
            Destruction();
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
