using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropEffect : MonoBehaviour
{
    //Crop取得
    [SerializeField] GameObject Player;
    [SerializeField] CropLineManager lineManager;
    private CropSound cropSound;
    //スプライト
    [SerializeField] GameObject Front;
    [SerializeField] GameObject Middle;
    [SerializeField] GameObject Back;

    //イージング
    [SerializeField] Ease frontEase_In;
    [SerializeField] Ease frontEase_Out;
    [SerializeField] Ease middleEase_In;
    [SerializeField] Ease middleEase_Out;
    [SerializeField] Ease backEase_In;
    [SerializeField] Ease backEase_Out;
    [SerializeField] Vector3 frontEaseScale;
    [SerializeField] Vector3 middlEeaseScale;
    [SerializeField] Vector3 backEaseScale;
    [SerializeField] float frontEaseTime;
    [SerializeField] float frontEaseOutTime;
    [SerializeField] float middleEaseTime;
    [SerializeField] float middleEaseOutTime;
    [SerializeField] float backEaseTime;
    [SerializeField] float backEaseOutTime;

    public bool isOne;

    // Start is called before the first frame update
    void Start()
    {
        Front.transform.localScale = new Vector3(1.5f, 0, 0);
        Middle.transform.localScale = new Vector3(1.5f, 0, 0);
        Back.transform.localScale = new Vector3(1.5f, 0, 0);
        cropSound = GetComponent<CropSound>();

    }

    // Update is called once per frame
    void Update()
    {

        CropBreak();
    }

    public void CropBreak()
    {
        if (lineManager.isCroping/* && !isOne*/)
        {
            //生成
            GameObject middle = Instantiate(Middle);
            middle.transform.position = new Vector3(Player.transform.position.x, lineManager.transform.position.y, middle.transform.position.z);

            isOne = true;
            Debug.Log("CropingNow");
            if (lineManager.isBlockBreak)
            {
                //生成
                GameObject front = Instantiate(Front);
                GameObject back = Instantiate(Back);
                front.transform.position = new Vector3(Player.transform.position.x, lineManager.transform.position.y, front.transform.position.z);
                back.transform.position = new Vector3(Player.transform.position.x, lineManager.transform.position.y, back.transform.position.z);


                Debug.Log("isBreakBrock");
                //front
                cropSound.SoundCrop();
                front.transform.DOScale(frontEaseScale, frontEaseTime).SetEase(frontEase_In).OnComplete(() =>
                {
                    //Debug.Log("complate");
                    front.transform.DOScale(new Vector3(1.3f, 0, 0), frontEaseOutTime).SetEase(frontEase_Out).OnComplete(() =>
                    {
                        isOne = false;
                        Destroy(front);
                    });
                });

                //back
                back.transform.DOScale(backEaseScale, backEaseTime).SetEase(backEase_In).OnComplete(() =>
                {
                    back.transform.DOScale(new Vector3(1.3f, 0, 0), backEaseOutTime).SetEase(backEase_Out).OnComplete(() =>
                    {
                        isOne = false;
                        Destroy(back);
                    });
                    //isOne = false;
                });

            }
            //middle
            cropSound.SoundCropMiss();
            middle.transform.DOScale(middlEeaseScale, middleEaseTime).SetEase(middleEase_In).OnComplete(() =>
            {
                middle.transform.DOScale(new Vector3(1.3f, 0, 0), middleEaseOutTime).SetEase(middleEase_Out).OnComplete(() =>
                {
                    isOne = false;
                    Destroy(middle);
                });
            });


        }
    }


}
