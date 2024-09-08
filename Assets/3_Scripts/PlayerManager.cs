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
    private bool canJump = true;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
        hitManager = GetComponent<PlayerHitManager>();
    }

    // Setter
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }
    public void SetCanJump(bool _canJump)
    {
        canJump = _canJump;
    }
    public void Initialize()
    {
        moveManager.Initialize();
    }

    // Getter
    public bool GetIsActive()
    {
        return isActive;
    }
    public bool GetCanJump()
    {
        return canJump;
    }
}
