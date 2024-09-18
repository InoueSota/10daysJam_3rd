using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private ParticleInstantiateScript particle;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isTriggerCancel;

    // �X�^���v
    [SerializeField] private GameObject stampObj;

    // �ԉ�
    [SerializeField] float fireWorksTime = 1, fireWorksTimeMax = 1/*, fireWorksTimeMin = 0.4f*/;
    private bool isAllClear;

    // ���R���|�[�l���g�擾
    private ClearData clearData;
    private S_Transition transition;

    // �J�ڃV�[���於
    [SerializeField] private string nextScene;

    //���֌W
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    void Start()
    {
        particle = GetComponent<ParticleInstantiateScript>();
        inputManager = GetComponent<InputManager>();
        clearData = new ClearData();
        clearData = clearData.LoadClearData(clearData);

        // �S�N�������f����
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
            //�g�����W�V��������
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
