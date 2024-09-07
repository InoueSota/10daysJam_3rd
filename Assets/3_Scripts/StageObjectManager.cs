using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    // ëSëÃä«óùÉtÉâÉO
    private bool canCheck;
    private bool noMovingObjects;

    class StageObject
    {
        public bool isMoving;
    }
    private StageObject dripStone;
    private StageObject icicle;

    private PlayerManager playerManager;

    void Start()
    {
        dripStone = new StageObject();
    }

    void Update()
    {
        if (canCheck)
        {
            if (dripStone.isMoving)
            {
                bool isFinishMoving = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
                {
                    AllObjectManager allObjectManager = obj.GetComponent<AllObjectManager>();

                    switch (allObjectManager.GetObjectType())
                    {
                        case AllObjectManager.ObjectType.DRIPSTONE:

                            if (obj.GetComponent<DripStoneManager>().GetIsFalling())
                            {
                                isFinishMoving = false;
                                break;
                            }
                            break;
                    }
                }

                if (isFinishMoving)
                {
                    dripStone.isMoving = false;
                }
            }

            noMovingObjects = !dripStone.isMoving;
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
