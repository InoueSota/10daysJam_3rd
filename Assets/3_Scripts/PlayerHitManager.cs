using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;
    private PlayerMoveManager moveManager;
    UndoManager undoManager;
    // 基本情報
    private Vector2 halfSize;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        moveManager = GetComponent<PlayerMoveManager>();
        undoManager=GameObject.FindWithTag("GameController").GetComponent<UndoManager>();

        halfSize.x = transform.localScale.x * 0.5f;
        halfSize.y = transform.localScale.y * 0.5f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger2D(collision);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigger2D(collision);
    }
    void OnTrigger2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            if (collision.GetComponent<AllObjectManager>().GetIsActive()&&collision.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.ITEM)
            {
                undoManager.UndoSaveItem(moveManager.lastGroundpos);

                ItemManager hitItemManager = collision.GetComponent<ItemManager>();

                // アイテム非表示処理(ゲットしたときの処理)
                hitItemManager.SetIsActive(false);                
                hitItemManager.SetIsGet();
            }
        }
    }
}
