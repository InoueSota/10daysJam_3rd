using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StageObjectManager : MonoBehaviour
{
    // 全体管理フラグ
    private bool canCheck;
    private bool noMovingObjects;

    enum ObjectType
    {
        DRIPSTONE,
        ICICLE
    }

    class StageObject
    {
        public bool isMoving;

        public void CheckMoving(StageObject _stageObject, ObjectType _objectType)
        {
            if (_stageObject.isMoving)
            {
                bool isFinishMoving = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
                {
                    AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

                    // DripStone
                    if (_objectType == ObjectType.DRIPSTONE && allObjectManager.GetObjectType() == AllObjectManager.ObjectType.DRIPSTONE)
                    {
                        if (obj.GetComponent<DripStoneManager>().GetIsFalling())
                        {
                            isFinishMoving = false;
                            break;
                        }
                    }
                    // ICICLE
                    else if (_objectType == ObjectType.ICICLE && allObjectManager.GetObjectType() == AllObjectManager.ObjectType.ICICLE)
                    {
                        if (obj.GetComponent<IcicleManager>().GetIsFalling())
                        {
                            isFinishMoving = false;
                            break;
                        }
                    }
                }

                if (isFinishMoving)
                {
                    _stageObject.isMoving = false;
                }
            }
        }
    }
    private StageObject dripStone;
    private StageObject icicle;

    private PlayerManager playerManager;

    void Start()
    {
        dripStone = new StageObject();
        icicle = new StageObject();
       
    }

    void Update()
    {
        if (canCheck)
        {
            // 動作中のオブジェクトをチェックする
            dripStone.CheckMoving(dripStone, ObjectType.DRIPSTONE);
            icicle.CheckMoving(icicle, ObjectType.ICICLE);

            noMovingObjects = !dripStone.isMoving;
            noMovingObjects = !icicle.isMoving;
            playerManager.SetIsActive(noMovingObjects);

            //ブロックの隙間を埋める用の関数
            BlockGapFillerLoad();
            BlockGapFiller();
        }
    }

    // Setter
    public void Initialize()
    {
        dripStone.isMoving = false;
        BlockGapFillerLoad();
        BlockGapFiller();
    }
    public void SetIsMoving(AllObjectManager.ObjectType _objectType, bool _isMoving)
    {
        switch (_objectType)
        {
            case AllObjectManager.ObjectType.DRIPSTONE:

                dripStone.isMoving = _isMoving;

                break;
        }
    }
    public void SetCanCheck(bool _canCheck)
    {
        canCheck = _canCheck;
    }
    public void SetPlayerManager(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    // Getter
    public bool GetIsFinishMoving()
    {
        return noMovingObjects;
    }

    [SerializeField] Vector2Int mapSize;
    BlockGapFillerScript[,] blocks = null;
    private void BlockGapFillerLoad()
    {

        blocks = new BlockGapFillerScript[mapSize.x,mapSize.y];

        if (blocks != null)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
            {
                AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

               

                if (allObjectManager.GetObjectType() == AllObjectManager.ObjectType.BLOCK)
                {
                   

                    if (Mathf.Abs(obj.transform.position.x) <= 8.5f 
                        && Mathf.Abs(obj.transform.position.y) <= 4.5f){

                        BlockGapFillerScript gapFiller = obj.GetComponentInChildren<BlockGapFillerScript>();

                    Vector2Int pos = Vector2Int.zero;

                        pos.x = (int)(obj.transform.position.x + 8.5f);
                        pos.y = (int)(obj.transform.position.y + 4.5f);

                        if (allObjectManager.GetIsActive() == false)
                        {
                            blocks[pos.x, pos.y] = null;
                        }
                        else
                        {
                            blocks[pos.x, pos.y] = gapFiller;
                        }
                    }

                }
            }
        }
    }

    private void BlockGapFiller()
    {

        if (blocks != null)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {

                    if(blocks[x, y] != null)
                    {
                        int[] blockCheck = new int[8];

                        //[0][1][2]//1=ブロック//0=他
                        //[3][B][4]
                        //[5][6][7]
                        
                        blockCheck[0] = CheckBlock(x - 1, y + 1);  
                        blockCheck[1] = CheckBlock(x, y + 1); 
                        blockCheck[2] = CheckBlock(x + 1, y + 1);

                        blockCheck[3] = CheckBlock(x - 1, y);
                        blockCheck[4] = CheckBlock(x + 1, y);

                        blockCheck[5] = CheckBlock(x-1, y-1);
                        blockCheck[6] = CheckBlock(x, y-1);
                        blockCheck[7] = CheckBlock(x+1, y-1);

                        if (
                            blockCheck[0] == 1 &&
                            blockCheck[1] == 1 &&
                            blockCheck[2] == 1 &&
                            blockCheck[3] == 1 &&
                            blockCheck[4] == 1 &&
                            blockCheck[5] == 1 &&
                            blockCheck[6] == 1 &&
                            blockCheck[7] == 1 
                            )
                        {
                            blocks[x, y].SetType(14);
                        }
                        else if (
                            blockCheck[0] == 0 && blockCheck[1] == 1 && blockCheck[2] == 1 &&
                            blockCheck[3] == 1 &&                                                      blockCheck[4] == 1 &&
                            blockCheck[5] == 1 &&blockCheck[6] == 1 && blockCheck[7] == 1
                            )
                        {
                            blocks[x, y].SetType(8);
                        }
                        else if (
                            blockCheck[0] == 1 && blockCheck[1] == 1 && blockCheck[2] == 1 &&
                            blockCheck[3] == 1 && blockCheck[4] == 1 &&
                            blockCheck[5] == 0 && blockCheck[6] == 1 && blockCheck[7] == 1
                            )
                        {
                            blocks[x, y].SetType(9);
                        }
                        else if (
                            blockCheck[0] == 1 && blockCheck[1] == 1 && blockCheck[2] == 1 &&
                            blockCheck[3] == 1 && blockCheck[4] == 1 &&
                            blockCheck[5] == 1 && blockCheck[6] == 1 && blockCheck[7] == 0
                            )
                        {
                            blocks[x, y].SetType(10);
                        }
                        else if (
                            blockCheck[0] == 1 && blockCheck[1] == 1 && blockCheck[2] == 0 &&
                            blockCheck[3] == 1 && blockCheck[4] == 1 &&
                            blockCheck[5] == 1 && blockCheck[6] == 1 && blockCheck[7] == 1
                            )
                        {
                            blocks[x, y].SetType(11);
                        }
                        else if (
                            blockCheck[0] == 1 && blockCheck[1] == 1 && blockCheck[2] == 0 &&
                            blockCheck[3] == 1 && blockCheck[4] == 1 &&
                            blockCheck[5] == 0 && blockCheck[6] == 1 && blockCheck[7] == 1
                            )
                        {
                            blocks[x, y].SetType(12);
                        }
                        else if (
                            blockCheck[0] == 0 && blockCheck[1] == 1 && blockCheck[2] == 1&&
                            blockCheck[3] == 1 && blockCheck[4] == 1 &&
                            blockCheck[5] == 1 && blockCheck[6] == 1 && blockCheck[7] == 0
                            )
                        {
                            blocks[x, y].SetType(13);
                        }
                        else if (
                             blockCheck[1] == 1 && blockCheck[2] == 1 &&
                             blockCheck[4] == 1 &&
                             blockCheck[6] == 1 && blockCheck[7] == 1
                            )
                        {
                            blocks[x, y].SetType(4);
                        }
                        else if (
                            blockCheck[0] == 1 && blockCheck[1] == 1 && blockCheck[2] == 1 &&
                            blockCheck[3] == 1 && blockCheck[4] == 1 
                            )
                        {
                            blocks[x, y].SetType(5);
                        }
                        else if (
                           blockCheck[0] == 1 && blockCheck[1] == 1  &&
                           blockCheck[3] == 1 &&
                           blockCheck[5] == 1 && blockCheck[6] == 1 
                           )
                        {
                            blocks[x, y].SetType(6);
                        }
                        else if (
                           blockCheck[3] == 1 && blockCheck[4] == 1 &&
                           blockCheck[5] == 1 && blockCheck[6] == 1 && blockCheck[7] == 1
                           )
                        {
                            blocks[x, y].SetType(7);
                        }
                        else if (
                           blockCheck[4] == 1 &&
                           blockCheck[6] == 1 && blockCheck[7] == 1
                          )
                        {
                            blocks[x, y].SetType(0);
                        }
                        else if (
                            blockCheck[1] == 1 && blockCheck[2] == 1 &&
                            blockCheck[4] == 1 
                            )
                        {
                            blocks[x, y].SetType(1);
                        }
                        else if (
                           blockCheck[0] == 1 && blockCheck[1] == 1 &&
                           blockCheck[3] == 1
                           )
                        {
                            blocks[x, y].SetType(2);
                        }
                        else if (
                           blockCheck[3] == 1 &&
                           blockCheck[5] == 1 && blockCheck[6] == 1 
                           )
                        {
                            blocks[x, y].SetType(3);
                        }
                        else 
                        {
                            blocks[x, y].SetType(-1);
                        }

                    }
                }
            }

            
        }
    }

    private int CheckBlock(int x,int y)
    {

        if(x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {
            return 0;
        }

        if (blocks[x, y] == null)
        {
            return 0;
        }

        return 1;
    }
}
