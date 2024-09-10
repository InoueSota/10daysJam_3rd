using UnityEngine;

public class SplitOverCollider : MonoBehaviour
{
    // �e�I�u�W�F�N�g�̃R���|�[�l���g�擾
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
        // �㕔�ɃI�u�W�F�N�g�����邩
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
        // �㕔�Ƀv���C���[�����邩
        if (collision.CompareTag("Player"))
        {
            splitManager.SetIsFreeOver(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // �㕔�Ƀv���C���[�����邩
        if (collision.CompareTag("Player"))
        {
            splitManager.SetIsFreeOver(true);
        }
    }
}
