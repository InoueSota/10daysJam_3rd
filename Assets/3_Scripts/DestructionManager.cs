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

                _obj.GetComponent<BlockManager>().Damage();

                if (0 < _obj.transform.childCount && _obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive())
                {
                    _obj.transform.GetChild(0).GetComponent<GrassParentScript>().Damage();
                }
                break;
            case AllObjectManager.ObjectType.ITEM:

                _obj.GetComponent<ItemManager>().Damage();

                break;
            case AllObjectManager.ObjectType.GRASSPARENT:

                _obj.GetComponent<GrassParentScript>().Damage();

                break;
            case AllObjectManager.ObjectType.DRIPSTONEBLOCK:

                if (_obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive())
                {
                    _obj.transform.GetChild(0).GetComponent<DripStoneManager>().FallInitialize();
                }
                _obj.GetComponent<BlockManager>().Damage();

                break;
            case AllObjectManager.ObjectType.DRIPSTONE:

                _obj.GetComponent<DripStoneManager>().Damage();

                break;
            case AllObjectManager.ObjectType.BOMB:

                _obj.GetComponent<BombManager>().Damage();

                break;
            case AllObjectManager.ObjectType.ICICLEBLOCK:

                if (_obj.transform.GetChild(0).GetComponent<AllObjectManager>().GetIsActive())
                {
                    _obj.transform.GetChild(0).GetComponent<IcicleManager>().FallInitialize();
                }
                _obj.GetComponent<BlockManager>().Damage();

                break;
            case AllObjectManager.ObjectType.ICICLE:

                _obj.GetComponent<IcicleManager>().Damage();

                break;
        }
    }
}
