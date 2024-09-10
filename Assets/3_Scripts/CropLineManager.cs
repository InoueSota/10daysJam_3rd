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
    // [SerializeField] private CropEffect cropEffect;
    // カメラ関係
    private Transform cameraTransform;
    private float cameraHalfSizeX;

    public bool isCroping;
    public bool isBlockBreak;
    float coolTime;

    void Start()
    {
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
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {
            isCroping = true;
            int i = 0;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X軸判定
                float xCameraBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);


                //coolTime = 0.04f;                
                if (xCameraBetween < cameraHalfSizeX)
                {
                    AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                    if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                    {

                        // Y軸判定
                        float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                        if (yBetween < 0.2f)
                        {
                            i++;
                            //ブロック壊したら
                            isBlockBreak = true;
                            //if (obj.GetComponent<BlockManager>())
                            //{
                            //Debug.Log(i);
                            //    obj.GetComponent<BlockManager>().SetNum(i * 0.025867f);
                            //}
                            destructionManager.Destruction(obj);

                            //sounds
                            cropsound.SoundCrop();
                            //GameObject brockSound= Instantiate(soundPrefab);


                        }
                    }
                }
            }
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
        return isCroping ;
    }
}
