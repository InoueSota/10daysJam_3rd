using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    //soundeffectプレハブ
    [SerializeField] GameObject soundPrefab;
    // 自コンポーネント取得
    private DestructionManager destructionManager;
    private SpriteRenderer spriteRenderer;
    private InputManager inputManager;
    private bool isTriggerSpecial;
    private CropSound cropsound;


    // 他コンポーネント取得
    private PlayerMoveManager playerMoveManager;
    private UndoManager undoManager;
    // [SerializeField] private CropEffect cropEffect;
    // カメラ関係
    private Transform cameraTransform;
    private float cameraHalfSizeX;

    public bool isCroping;
    public bool isBlockBreak;
    float coolTime;

    void Start()
    {
        undoManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UndoManager>();

        cropsound = GetComponent<CropSound>();
        destructionManager = GetComponent<DestructionManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();
        coolTime = 0;

        // カメラ関係
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraHalfSizeX = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;

        //プレイヤーのカラー取得しラインの色をプレイヤーと同じに
        spriteRenderer.color = transform.parent.GetComponent<SpriteRenderer>().color;
    }

    void LateUpdate()
    {
        GetInput();

        transform.localPosition = new(0f, -1f, 0f);
        transform.position = new(cameraTransform.position.x, transform.position.y, transform.position.z);
        coolTime -= Time.deltaTime;

        Destruction();
    }

    void Destruction()
    {
        /* ブロックが壊れた音を一括ではなく、順番に鳴らすのに必要な処理
            int i = 0;
            coolTime = 0.04f;
            i++;
            if (obj.GetComponent<BlockManager>())
            {
                obj.GetComponent<BlockManager>().SetNum(i * 0.025867f);
            }
        */
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {
            isCroping = true;
            
            //Undo用
            undoManager.UndoSave();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                {
                    // Y軸判定
                    float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                    if (yBetween < 0.2f)
                    {
                        

                        // Crop時に当たるかどうか判定が入るオブジェクト
                        if (hitAllObjectManager.GetIsJudgeObject())
                        {
                            
                            if (obj.GetComponent<SplitManager>() && obj.GetComponent<SplitManager>().GetCanHit())
                            {
                                
                                // ブロック壊したら
                                isBlockBreak = true;
                                // ダメージを与える
                                obj.GetComponent<SplitManager>().Damage();
                                // Sounds
                                cropsound.SoundCrop();
                            }
                        }
                        else
                        {
                            // ブロック壊したら
                            isBlockBreak = true;
                            // ブロックの種類に応じて処理内容を変化させる
                            destructionManager.Destruction(obj);
                            // Sounds
                            cropsound.SoundCrop();
                        }
                    }
                }
            }

            playerMoveManager.SetCropJumpTimer();
        }
    }

    void GetInput()
    {
        isTriggerSpecial = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.SPECIAL))
        {
            isTriggerSpecial = true;
        }
        else
        {
            isCroping = false;
            isBlockBreak = false;
        }
    }

    public bool GetIsCropping()
    {
        return isCroping;
    }
}
