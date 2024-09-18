using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropLineManager : MonoBehaviour
{
    //soundeffect�v���n�u
    [SerializeField] GameObject soundPrefab;
    // ���R���|�[�l���g�擾
    private DestructionManager destructionManager;
    private SpriteRenderer spriteRenderer;
    private InputManager inputManager;
    private bool isTriggerSpecial;
    private CropSound cropsound;


    // ���R���|�[�l���g�擾
    private PlayerMoveManager playerMoveManager;
    private UndoManager undoManager;
    // [SerializeField] private CropEffect cropEffect;
    // �J�����֌W
    private Transform cameraTransform;
    private float cameraHalfSizeX;

    public bool isCroping;
    public bool isBlockBreak;
    float coolTime;

    void Start()
    {
        undoManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UndoManager>();

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
        /* �u���b�N����ꂽ�����ꊇ�ł͂Ȃ��A���Ԃɖ炷�̂ɕK�v�ȏ���
            int i = 0;
            coolTime = 0.04f;
            i++;
            if (obj.GetComponent<BlockManager>())
            {
                obj.GetComponent<BlockManager>().SetNum(i * 0.025867f);
            }
        */
        if (isTriggerSpecial && playerMoveManager.GetIsGround())
        {
            isCroping = true;
            
            //Undo�p
            undoManager.UndoSave();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager hitAllObjectManager = obj.GetComponent<AllObjectManager>();

                if (hitAllObjectManager.GetObjectType() != AllObjectManager.ObjectType.GROUND && hitAllObjectManager.GetIsActive())
                {
                    // Y������
                    float yBetween = Mathf.Abs(transform.position.y - obj.transform.position.y);

                    if (yBetween < 0.2f)
                    {
                        

                        // Crop���ɓ����邩�ǂ������肪����I�u�W�F�N�g
                        if (hitAllObjectManager.GetIsJudgeObject())
                        {
                            
                            if (obj.GetComponent<SplitManager>() && obj.GetComponent<SplitManager>().GetCanHit())
                            {
                                
                                // �u���b�N�󂵂���
                                isBlockBreak = true;
                                // �_���[�W��^����
                                obj.GetComponent<SplitManager>().Damage();
                                // Sounds
                                cropsound.SoundCrop();
                            }
                        }
                        else
                        {
                            // �u���b�N�󂵂���
                            isBlockBreak = true;
                            // �u���b�N�̎�ނɉ����ď������e��ω�������
                            destructionManager.Destruction(obj);
                            // Sounds
                            cropsound.SoundCrop();
                        }
                    }
                }
            }

            playerMoveManager.SetCropJumpTimer();
        }
    }

    void GetInput()
    {
        isTriggerSpecial = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.SPECIAL))
        {
            isTriggerSpecial = true;
        }
        else
        {
            isCroping = false;
            isBlockBreak = false;
        }
    }

    public bool GetIsCropping()
    {
        return isCroping;
    }
}
