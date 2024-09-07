using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private DestructionManager destructionManager;
    private InputManager inputManager;
    private bool isTriggerSpecial;

    // ���R���|�[�l���g�擾
    private PlayerMoveManager playerMoveManager;
    
    //�J���[
    private SpriteRenderer spriteRenderer;
    private GameObject Player;
    void Start()
    {
        destructionManager = GetComponent<DestructionManager>();
        inputManager = GetComponent<InputManager>();
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();

        //�v���C���[�̃J���[�擾�����C���̐F���v���C���[�Ɠ�����
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
                    // Y������
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
