using UnityEngine;

public class SplitManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;

    [Header("�q�I�u�W�F�N�g�擾")]
    [SerializeField] private GameObject overObj;
    private SpriteRenderer overSpriteRenderer;
    private Transform overTransform;
    private Vector3 overOrigin;
    private Vector3 overTarget;
    [SerializeField] private GameObject underObj;
    private SpriteRenderer underSpriteRenderer;

    // ���I�u�W�F�N�g�擾
    private Transform playerTransform;

    // �t���O - ��ɃI�u�W�F�N�g�����邩
    private bool isFreeOver;

    [Header("�\������")]
    [SerializeField] private float floatHeight;
    [SerializeField] private float chasePower;
    [SerializeField] private float flowSpeed;
    [SerializeField] private float range;
    private float angle;

    [Header("����")]
    [SerializeField] private float avoidHeight;
    [SerializeField] private float avoidTime;
    private float avoidTimer;
    private bool isAvoid;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();

        // �q�I�u�W�F�N�g������
        overSpriteRenderer = overObj.GetComponent<SpriteRenderer>();
        overTransform = overObj.transform;
        overOrigin = overObj.transform.position;
        overTarget = overOrigin;
        underSpriteRenderer = underObj.GetComponent<SpriteRenderer>();

        // �v���C���[�̈ʒu���擾
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // ���������͏�ɃI�u�W�F�N�g���Ȃ�����ɂ���
        isFreeOver = true;

        // sin�^���t���t��������
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

            // Target��sin�^��������
            angle += flowSpeed * Time.deltaTime;
            overTarget.y += Mathf.Sin(angle) * range;
        }
        else
        {
            overTarget = overOrigin;
            angle = 0f;
        }

        // Target�Ɍ������Ēǂ�������
        overTransform.position += (overTarget - overTransform.position) * (chasePower * Time.deltaTime);
    }
    void Avoid()
    {
        avoidTimer -= Time.deltaTime;
        if (avoidTimer <= 0f) { isAvoid = false; }

        // Target�Ɍ������Ēǂ�������
        overTransform.position += (overTarget - overTransform.position) * (chasePower * Time.deltaTime);
    }

    // ����������
    void Initialize()
    {
        overSpriteRenderer.enabled = true;
        underSpriteRenderer.enabled = true;
        allObjectManager.SetIsActive(overSpriteRenderer.enabled);
        allObjectManager.Initialize();

        angle = 0;
    }

    // ���ŏ���
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
        // true�Ȃ�����t���O��true�ɂ���
        if (isFreeOver)
        {
            overTarget.y = overOrigin.y + avoidHeight;
            avoidTimer = avoidTime;
            isAvoid = true;
        }
        return !isFreeOver;
    }
}
