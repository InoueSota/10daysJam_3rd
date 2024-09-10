using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerMoveManager moveManager;
    private PlayerHitManager hitManager;
    [SerializeField] private UICropScript ui_Crop;
    // �ړ��t���O
    [SerializeField] private bool isActive;
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
        ui_Crop.Initialize();
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
