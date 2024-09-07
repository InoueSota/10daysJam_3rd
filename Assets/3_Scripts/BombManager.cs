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

    void LateUpdate()
    {
        if (allObjectManager.GetHp() >= 2)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
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
