using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerMoveManager moveManager;
    private PlayerHitManager hitManager;

    // 移動フラグ
    private bool isActive;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
        hitManager = GetComponent<PlayerHitManager>();
    }

    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }
    public void Initialize()
    {
        moveManager.Initialize();
    }

    public bool GetIsActive()
    {
        return isActive;
    }
}
