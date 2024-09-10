using UnityEngine;

public class SplitManager : MonoBehaviour
{
    // 自コンポーネント取得
    private AllObjectManager allObjectManager;

    // 子オブジェクト取得
    [SerializeField] private GameObject overObj;
    private SpriteRenderer overSpriteRenderer;
    [SerializeField] private GameObject underObj;
    private SpriteRenderer underSpriteRenderer;

    // フラグ - 上にオブジェクトがあるか
    private bool isFreeOver;

    void Start()
    {
        allObjectManager = GetComponent<AllObjectManager>();
        overSpriteRenderer = overObj.GetComponent<SpriteRenderer>();
        underSpriteRenderer = underObj.GetComponent<SpriteRenderer>();
        isFreeOver = true;
    }

    void Update()
    {
        Debug.Log(isFreeOver);
    }

    // 初期化処理
    void Initialize()
    {
        overSpriteRenderer.enabled = true;
        underSpriteRenderer.enabled = true;
        allObjectManager.SetIsActive(overSpriteRenderer.enabled);
        allObjectManager.Initialize();
        isFreeOver = true;
    }

    // 消滅処理
    void Disappear()
    {
        overSpriteRenderer.enabled = false;
        underSpriteRenderer.enabled = false;
        allObjectManager.SetIsActive(false);
    }

    // Setter
    public void Damage()
    {
        allObjectManager.Damage();

        if (allObjectManager.GetIsActive() && allObjectManager.GetHp() <= 0)
        {
            SetIsActive(false);
        }
    }
    public void SetIsActive(bool _isActive)
    {
        if (_isActive)
        {
            Initialize();
        }
        else
        {
            Disappear();
        }
    }
    public void SetIsFreeOver(bool _isFreeOver)
    {
        isFreeOver = _isFreeOver;
    }

    // Getter
    public bool GetCanHit()
    {
        return !isFreeOver;
    }
}
