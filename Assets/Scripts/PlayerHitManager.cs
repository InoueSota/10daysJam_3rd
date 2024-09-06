using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;

    // 基本情報
    private Vector2 halfSize;

    // アイテムカウント
    private int itemCount;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();

        halfSize.x = transform.localScale.x * 0.5f;
        halfSize.y = transform.localScale.y * 0.5f;

        itemCount = 0;
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
            if (collision.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.ITEM)
            {
                ItemManager hitItemManager = collision.GetComponent<ItemManager>();

                // アイテム非表示処理
                hitItemManager.SetIsActive(false);

                // アイテム獲得フラグ変更
                if (itemCount <= 0)
                {
                    GlobalVariables.isGetItem1 = true;
                }
                else if (itemCount <= 1)
                {
                    GlobalVariables.isGetItem2 = true;
                }
                else if (itemCount <= 2)
                {
                    GlobalVariables.isGetItem3 = true;
                }
                itemCount++;
            }
        }
    }
}
