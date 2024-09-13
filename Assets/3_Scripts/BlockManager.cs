using DG.Tweening;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //[SerializeField] GameObject soundEffect;
    [SerializeField] float coolTime;
    bool isOne;
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript particle;

    // ��{���
    private Vector3 originScale;
    private Quaternion originRotate;

    public enum BlockType
    {
        NORMAL,
        GRASS,
        DRIPSTONE,
        ICICLE
    }
    [SerializeField] private BlockType blockType = BlockType.NORMAL;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleInstantiateScript>();

        originScale = transform.localScale;
        originRotate = transform.localRotation;
    }

    void LateUpdate()
    {
        coolTime -= Time.deltaTime;
        if (coolTime <= 0 && !isOne && !allObjectManager.GetIsActive())
        {
            //GameObject sound = Instantiate(soundEffect);
            isOne = true;
        }
    }
    // ����������
    void Initialize()
    {

        //�S�Ă̎��s���~�߂���@
        //DOTween.KillAll();
        //DOTween.Kill(this);
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();

        isOne = false;
        transform.localScale = originScale;
        transform.localRotation = originRotate;
        spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    // ���ŏ���
    void Disappear()
    {


        allObjectManager.SetIsActive(false);

        //�j��p�[�e�B�N������
        if (particle != null)
        {
            particle.RunParticle(0);
        }

        //Sequence�̃C���X�^���X���쐬
        var sequence = DOTween.Sequence();

        //Append�œ����ǉ����Ă���
        sequence.Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
        //Join�͂ЂƂO�̓���Ɠ����Ɏ��s�����
        sequence.Join(this.transform.DORotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InBack));
        //sequence.Join(this.GetComponent<SpriteRenderer>().DOFade(endValue: 0, duration: 0.5f).SetEase(Ease.InQuad));

        sequence.Play().OnComplete(() =>
        {
            spriteRenderer.enabled = false;
        });

    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
        }
        else
        {
            particle.RunParticle(1);
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

    // Getter
    public BlockType GetBlockType()
    {
        return blockType;
    }

    public void SetNum(float cooltime)
    {
        coolTime = cooltime;
    }
}
