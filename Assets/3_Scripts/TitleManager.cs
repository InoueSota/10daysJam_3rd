using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // 自コンポーネント取得
    private ParticleInstantiateScript particle;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;

    // スタンプ
    [SerializeField] private GameObject stampObj;

    // 花火
    [SerializeField] float fireWorksTime = 1, fireWorksTimeMax = 1/*, fireWorksTimeMin = 0.4f*/;
    private bool isAllClear;

    // 他コンポーネント取得
    private ClearData clearData;
    private S_Transition transition;

    // 遷移シーン先名
    [SerializeField] private string nextScene;

    //音関係
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    void Start()
    {
        particle = GetComponent<ParticleInstantiateScript>();
        inputManager = GetComponent<InputManager>();
        clearData = new ClearData();
        clearData = clearData.LoadClearData(clearData);

        // 全クリか判断する
        if (clearData.GetAllClear(clearData))
        {
            isAllClear = true;
            stampObj.SetActive(isAllClear);
        }

        transition = GameObject.FindWithTag("trans").GetComponent<S_Transition>();
        transition.SetColor(4);
    }

    void Update()
    {
        GetInput();

        if (isAllClear)
        {
            AllClear();
        }

        ChangeScene();
    }

    void AllClear()
    {
        if (fireWorksTime <= 0)
        {
            particle.RunParticle(0, new Vector3(Random.Range(-9f, 9f), Random.Range(-4f, 4f), 0.0f));
            fireWorksTime += fireWorksTimeMax;
        }
        else
        {
            fireWorksTime -= Time.deltaTime;
        }
    }
    void ChangeScene()
    {
        if (isTriggerCancel)
        {
            clearData.ResetClearFlag(clearData);
            clearData.Save(clearData);
        }

        if (isTriggerJump && !transition.isTransNow)
        {
            //トランジション処理
            transition.SetTransition(nextScene);
            audioSource.PlayOneShot(clip);
        }
    }

    void GetInput()
    {
        isTriggerJump = false;
        isTriggerCancel = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.CANCEL))
        {
            isTriggerCancel = true;
        }
    }
}
