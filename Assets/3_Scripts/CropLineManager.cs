using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    // 自コンポーネント取得
    private DestructionManager destructionManager;
    private InputManager inputManager;
    private bool isTriggerSpecial;

    // 他コンポーネント取得
    private PlayerMoveManager playerMoveManager;
    
    //カラー
    private SpriteRenderer spriteRenderer;
    private GameObject Player;
    void Start()
    {
        destructionManager = GetComponent<DestructionManager>();
        inputManager = GetComponent<InputManager>();
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();

        //プレイヤーのカラー取得しラインの色をプレイヤーと同じに
        Player = GameObject.FindWithTag("Player");

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Player.GetComponent<SpriteRenderer>().color;
    }

    void LateUpdate()
    {
        GetInput();

        transform.localPosition = new(0f, -1f, 0f);
        transform.position = new(0f, transform.position.y, transform.position.z);

        Destruction();
    }

    void Destruction()
    {
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {


            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                {
                    // Y軸判定
                    float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                    if (yBetween < 0.2f)
                    {
                        destructionManager.Destruction(obj);
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
    }
}
