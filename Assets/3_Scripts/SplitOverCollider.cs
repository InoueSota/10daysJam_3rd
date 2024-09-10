using UnityEngine;

public class SplitOverCollider : MonoBehaviour
{
    // 親オブジェクトのコンポーネント取得
    private SplitManager splitManager;

    void Start()
    {
        splitManager = transform.parent.GetComponent<SplitManager>();
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
        // 上部にオブジェクトがあるか
        if (collision.CompareTag("Object"))
        {
            AllObjectManager allObjectManager = collision.GetComponent<AllObjectManager>();

            if (allObjectManager.GetObjectType() != AllObjectManager.ObjectType.ITEM && allObjectManager.GetIsActive())
            {
                splitManager.SetIsFreeOver(false);
            }
            else if (!allObjectManager.GetIsActive())
            {
                splitManager.SetIsFreeOver(true);
            }
        }
        // 上部にプレイヤーがいるか
        if (collision.CompareTag("Player"))
        {
            splitManager.SetIsFreeOver(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 上部にプレイヤーがいるか
        if (collision.CompareTag("Player"))
        {
            splitManager.SetIsFreeOver(true);
        }
    }
}
