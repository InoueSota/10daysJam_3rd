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

    void LateUpdate()
    {
        // �����Ă���Ƃ�
        if (!spriteRenderer.enabled)
        {

        }
    }

    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            //
            // �����������ꂽ�u��
            //blockDestory.BlockDestroy(spriteRenderer.enabled);
            //Sequence�̃C���X�^���X���쐬
            var sequence = DOTween.Sequence();

            //Append�œ����ǉ����Ă���
            sequence.Append(transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.OutBack));
            //Join�͂ЂƂO�̓���Ɠ����Ɏ��s�����
            sequence.Join(this.transform.DORotate(Vector3.forward * 360, 0.35f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine));
            sequence.Join(this.GetComponent<SpriteRenderer>().DOFade(endValue: 0, duration: 0.35f).SetEase(Ease.InQuad));

            sequence.Play().OnComplete(() =>
            {
                spriteRenderer.enabled = false;
            });
            //

        }
        allObjectManager.SetIsActive(spriteRenderer.enabled);
    }
}
