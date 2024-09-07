using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    // Setter
    public void Initialize()
    {
        dripStone.isMoving = false;
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
}
