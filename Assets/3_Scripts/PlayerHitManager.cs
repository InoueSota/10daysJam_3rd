using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;

    // ��{���
    private Vector2 halfSize;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();

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
            if (collision.GetComponent<AllObjectManager>().GetObjectType() == AllObjectManager.ObjectType.ITEM)
            {
                ItemManager hitItemManager = collision.GetComponent<ItemManager>();

                // �A�C�e����\������
                hitItemManager.SetIsActive(false);
            }
        }
    }
}
