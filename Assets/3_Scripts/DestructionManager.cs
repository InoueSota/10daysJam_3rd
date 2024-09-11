using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    public void Destruction(GameObject _obj)
    {
        switch (_obj.GetComponent<AllObjectManager>().GetObjectType())
        {
            case AllObjectManager.ObjectType.BLOCK:

                BlockManager blockManager = _obj.GetComponent<BlockManager>();

                switch (blockManager.GetBlockType())
                {
                    case BlockManager.BlockType.NORMAL:
                        _obj.GetComponent<BlockManager>().Damage();
                        break;
                    case BlockManager.BlockType.GRASS:
                        if (_obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive()) { _obj.transform.GetChild(0).GetComponent<GrassParentScript>().Damage(); }
                        _obj.GetComponent<BlockManager>().Damage();
                        break;
                    case BlockManager.BlockType.DRIPSTONE:
                        if (_obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive()) { _obj.transform.GetChild(0).GetComponent<DripStoneManager>().FallInitialize(); }
                        _obj.GetComponent<BlockManager>().Damage();
                        break;
                    case BlockManager.BlockType.ICICLE:
                        if (_obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive()) { _obj.transform.GetChild(0).GetComponent<IcicleManager>().FallInitialize(); }
                        _obj.GetComponent<BlockManager>().Damage();
                        break;
                }

                break;
            case AllObjectManager.ObjectType.ITEM:

                _obj.GetComponent<ItemManager>().Damage();

                break;
            case AllObjectManager.ObjectType.GRASSPARENT:

                _obj.GetComponent<GrassParentScript>().Damage();

                break;
            case AllObjectManager.ObjectType.DRIPSTONE:

                _obj.GetComponent<DripStoneManager>().Damage();

                break;
            case AllObjectManager.ObjectType.BOMB:

                _obj.GetComponent<BombManager>().Damage();

                break;
            case AllObjectManager.ObjectType.SPLIT:

                _obj.GetComponent<SplitManager>().Damage();

                break;
            case AllObjectManager.ObjectType.CACTUS:

                _obj.GetComponent<CactusManager>().Damage();

                break;
            case AllObjectManager.ObjectType.ICICLE:

                _obj.GetComponent<IcicleManager>().Damage();

                break;
        }
    }
}
