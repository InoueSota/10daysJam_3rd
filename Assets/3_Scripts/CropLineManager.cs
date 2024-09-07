using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    // 他コンポーネント取得
    private PlayerMoveManager playerMoveManager;
    
    // 入力
    private InputManager inputManager;
    private bool isTriggerSpecial;
    //カラー
    private SpriteRenderer spriteRenderer;
    private GameObject Player;
    void Start()
    {
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();
        inputManager = GetComponent<InputManager>();

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

                if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND)
                {
                    // Y軸判定
                    float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                    if (yBetween < 0.2f)
                    {
                        switch (hitAllObjectManager.GetObjectType())
                        {
                            case AllObjectManager.ObjectType.BLOCK:

                                obj.GetComponent<BlockManager>().Damage();

                                break;
                            case AllObjectManager.ObjectType.ITEM:

                                obj.GetComponent<ItemManager>().Damage();

                                break;
                            case AllObjectManager.ObjectType.DRIPSTONEBLOCK:

                                obj.transform.GetChild(0).GetComponent<DripStoneManager>().FallInitialize();
                                obj.GetComponent<BlockManager>().Damage();

                                break;
                            case AllObjectManager.ObjectType.DRIPSTONE:

                                obj.GetComponent<DripStoneManager>().Damage();

                                break;
                            case AllObjectManager.ObjectType.BOMB:

                                obj.GetComponent<BombManager>().Damage();

                                break;
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
    }
}
