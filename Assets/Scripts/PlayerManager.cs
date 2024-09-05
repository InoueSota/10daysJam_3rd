using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerMoveManager moveManager;
    private PlayerHitManager hitManager;

    // �ړ��t���O
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
