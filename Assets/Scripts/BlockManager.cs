using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // 自コンポーネント取得
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
        // 消えているとき
        if (!spriteRenderer.enabled)
        {

        }
    }

    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            spriteRenderer.enabled = true;
            allObjectManager.SetIsActive(spriteRenderer.enabled);

            transform.localScale = Vector3.one;
            spriteRenderer.color = Color.white;
        }
        else
        {
            allObjectManager.SetIsActive(false);

            //
            // ここが消された瞬間
            //blockDestory.BlockDestroy(spriteRenderer.enabled);
            //Sequenceのインスタンスを作成
            var sequence = DOTween.Sequence();

            //Appendで動作を追加していく
            sequence.Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
            //Joinはひとつ前の動作と同時に実行される
            sequence.Join(this.transform.DORotate(Vector3.forward * 200, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutElastic));
            sequence.Join(this.GetComponent<SpriteRenderer>().DOFade(endValue: 0, duration: 0.5f).SetEase(Ease.InQuad));

            sequence.Play().OnComplete(() =>
            {
                spriteRenderer.enabled = false;
            });
            //
        }
    }
}
