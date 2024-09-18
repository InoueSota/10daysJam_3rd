using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerMoveManager moveManager;
    private PlayerHitManager hitManager;
    [SerializeField] private UICropScript ui_Crop;
    // 移動フラグ
    private bool isActive;
    private bool canGetInput;
    private bool canJump = true;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
        hitManager = GetComponent<PlayerHitManager>();
        canGetInput = false;
    }

    // Setter
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }
    public void SetCanGetInput(bool _canGetInput)
    {
        canGetInput = _canGetInput;
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
    public bool GetCanGetInput()
    {
        return canGetInput;
    }
    public bool GetCanJump()
    {
        return canJump;
    }
}
