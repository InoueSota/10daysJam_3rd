using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;
    private SpriteRenderer spriteRenderer;
    private S_BlockDestory blockDestory;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ����������
    void Initialize()
    {
        spriteRenderer.enabled = true;
        allObjectManager.SetIsActive(spriteRenderer.enabled);

        transform.localScale = Vector3.one;
        spriteRenderer.color = Color.white;
    }

    // ���ŏ���
    void Destruction()
    {
        allObjectManager.SetIsActive(false);

        //blockDestory.BlockDestroy(spriteRenderer.enabled);
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
