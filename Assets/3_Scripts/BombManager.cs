using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;

    // ��{���
    private Vector3 originScale;
    private Quaternion originRotate;

    // Bomb����
    [SerializeField] private float explosionRange;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originScale = transform.localScale;
        originRotate = transform.localRotation;
    }

    // ����
    void Explosion()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

            if (obj != gameObject && hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND)
            {
                // X������
                float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);
                // Y������
                float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                if (xBetween < explosionRange && yBetween < explosionRange)
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
                }
            }
        }
    }

    // ����������
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();

        transform.localScale = originScale;
        transform.localRotation = originRotate;
        spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    // ���ŏ���
    void Destruction()
    {
        spriteRenderer.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetHp() <= 0)
        {
            Explosion();
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
}
