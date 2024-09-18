using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BombManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private DestructionManager destructionManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript particle;
    private BoxCollider2D boxCollider2D;

    // ��{���
    private Vector3 originScale;
    private Quaternion originRotate;

    // Bomb����
    [SerializeField] private float explosionRange;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        destructionManager = GetComponent<DestructionManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleInstantiateScript>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        originScale = transform.localScale;
        originRotate = transform.localRotation;
    }

    // ����
    void Explosion()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

            if (obj != gameObject && hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
            {
                // X������
                float xBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);
                // Y������
                float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                if (xBetween < explosionRange && yBetween < explosionRange)
                {
                    destructionManager.Destruction(obj);
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

        transform.localScale = originScale;
        transform.localRotation = originRotate;
        spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    // ���ŏ���
    void Disappear()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        particle.RunParticle(0);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
            Explosion();
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
}
