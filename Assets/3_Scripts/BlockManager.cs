using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private ParticleInstantiateScript  particle;

    // 基本情報
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

    }

    // 初期化処理
    void Initialize()
    {

        //全ての実行を止める方法
        DOTween.KillAll();
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);
        allObjectManager.Initialize();

        transform.localScale = originScale;
        transform.localRotation = originRotate;
        spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    // 消滅処理
    void Disappear()
    {
        allObjectManager.SetIsActive(false);

        //破壊パーティクル生成
        if (particle != null)
        {
            particle.RunParticle(0);
        }

        //Sequenceのインスタンスを作成
        var sequence = DOTween.Sequence();

        //Appendで動作を追加していく
        sequence.Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
        //Joinはひとつ前の動作と同時に実行される
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
}
