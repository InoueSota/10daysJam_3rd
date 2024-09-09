using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    // 自コンポーネント取得
    private DestructionManager destructionManager;
    private SpriteRenderer spriteRenderer;
    private InputManager inputManager;
    private bool isTriggerSpecial;

    // 他コンポーネント取得
    private PlayerMoveManager playerMoveManager;
   // [SerializeField] private CropEffect cropEffect;

    // カメラ関係
    private Transform cameraTransform;
    private float cameraHalfSizeX;

    public bool isCroping;
    public bool isBlockBreak;

    void Start()
    {
        destructionManager = GetComponent<DestructionManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();

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

        Destruction();
    }

    void Destruction()
    {
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {
            isCroping = true;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X軸判定
                float xCameraBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);

                if (xCameraBetween < cameraHalfSizeX)
                {
                    AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                    if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                    {

                        // Y軸判定
                        float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                        if (yBetween < 0.2f)
                        {
                            //ブロック壊したら
                            isBlockBreak = true;
                            destructionManager.Destruction(obj);
                            
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
        else {
            isCroping = false;
            isBlockBreak = false;
        }
    }
}
