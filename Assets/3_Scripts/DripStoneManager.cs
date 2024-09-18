using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DripStoneManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript particle;
    private BoxCollider2D boxCollider2D;

    // ���R���|�[�l���g�擾
    private StageObjectManager stageObjectManager;

    // �e�I�u�W�F�N�g
    private Transform parentTransform;

    // ��{���
    private Vector2 halfSize;

    // ���W�n
    private Vector3 originPosition;
    private Vector3 nextPosition;

    // ����
    private bool isFallActive;
    private bool isFalling;
    [Header("����")]
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
            // �u���b�N�Ƃ̏Փ˔���
            bool noBlock = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

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
                fallPower = 0f;
                isFalling = true;
            }
        }
        else
        {
            fallPower += addFallPower * Time.deltaTime;

            float deltaFallPower = fallPower * Time.deltaTime;
            nextPosition.y -= deltaFallPower;

            // �u���b�N�Ƃ̏Փ˔���
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                if (obj != gameObject && obj.GetComponent<AllObjectManager>().GetIsActive() && obj.GetComponent<AllObjectManager>().GetObjectType() != AllObjectManager.ObjectType.ITEM)
                {
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y������
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
                    // X������
                    float xBetween = Mathf.Abs(nextPosition.x - obj.transform.position.x);
                    float xDoubleSize = halfSize.x + 0.25f;

                    // Y������
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

    // ����������
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

    // ���ŏ���
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
