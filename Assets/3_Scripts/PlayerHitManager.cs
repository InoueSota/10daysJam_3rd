using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private AllObjectManager allObjectManager;

    // ��{���
    private Vector2 halfSize;

    // �A�C�e���J�E���g
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

                // �A�C�e����\������
                hitItemManager.SetIsActive(false);

                // �A�C�e���l���t���O�ύX
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
