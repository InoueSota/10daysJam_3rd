using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BlockDestory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlockDestroy(bool isActive)
    {
        //Sequence�̃C���X�^���X���쐬
        var sequence = DOTween.Sequence();

        //Append�œ����ǉ����Ă���
        sequence.Append(transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.OutBack));
        //Join�͂ЂƂO�̓���Ɠ����Ɏ��s�����
        sequence.Join(this.transform.DORotate(Vector3.forward * 360, 0.35f,RotateMode.LocalAxisAdd).SetEase(Ease.InSine));
        sequence.Join(this.GetComponent<SpriteRenderer>().DOFade(endValue: 0, duration: 0.35f).SetEase(Ease.InQuad));

        sequence.Play();
        
        //sequence.Play().OnComplete(() =>
        //{
        //    isActive = false;
        //    Debug.Log("OnComplete!");
        //});
    }

}
