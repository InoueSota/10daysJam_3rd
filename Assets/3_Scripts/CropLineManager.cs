using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    //soundeffect�v���n�u
    GameObject soundPrefab;
    // ���R���|�[�l���g�擾
    private DestructionManager destructionManager;
    private SpriteRenderer spriteRenderer;
    private InputManager inputManager;
    private bool isTriggerSpecial;
    private CropSound cropsound;
    

    // ���R���|�[�l���g�擾
    private PlayerMoveManager playerMoveManager;
    // [SerializeField] private CropEffect cropEffect;
    // �J�����֌W
    private Transform cameraTransform;
    private float cameraHalfSizeX;

    public bool isCroping;
    public bool isBlockBreak;
    float coolTime;

    void Start()
    {
        cropsound = GetComponent<CropSound>();
        destructionManager = GetComponent<DestructionManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        playerMoveManager = transform.parent.GetComponent<PlayerMoveManager>();
        coolTime = 0;

        // �J�����֌W
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraHalfSizeX = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;

        //�v���C���[�̃J���[�擾�����C���̐F���v���C���[�Ɠ�����
        spriteRenderer.color = transform.parent.GetComponent<SpriteRenderer>().color;
    }

    void LateUpdate()
    {
        GetInput();

        transform.localPosition = new(0f, -1f, 0f);
        transform.position = new(cameraTransform.position.x, transform.position.y, transform.position.z);
        coolTime -= Time.deltaTime;

        Destruction();
    }



    void Destruction()
    {
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {
            isCroping = true;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                // X������
                float xCameraBetween = Mathf.Abs(transform.position.x - obj.transform.position.x);
                coolTime = 0.01f;
                coolTime -= 0.02f;
                if (xCameraBetween < cameraHalfSizeX)
                {
                    AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                    if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                    {

                        // Y������
                        float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                        if (yBetween < 0.2f&&coolTime<=0)
                        {
                            Debug.Log("takusan");
                            //�u���b�N�󂵂���
                            isBlockBreak = true;
                            destructionManager.Destruction(obj);
                            //sounds
                            cropsound.SoundCrop();
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
