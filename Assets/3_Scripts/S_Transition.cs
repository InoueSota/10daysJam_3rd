using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class S_Transition : MonoBehaviour
{
    //�v���C���[�擾
    GameObject player;
    // �I�u�W�F�N�g
    [SerializeField] GameObject transObj;

    public Camera secondaryCamera;
    // �ǂ�����n�܂邩�|�W�V����
    [Header("����ɓ���")]
    [SerializeField] Vector3 SPos_In;
    [SerializeField] Vector3 EPos_Out;
    [Header("���͂��Ȃ���_��")]
    [SerializeField] Vector3 SScale_In;
    [SerializeField] Vector3 EScale_Out;
    //�C�[�W���O
    [SerializeField] Ease easeIn;
    [SerializeField] Ease easeOut;
    [SerializeField] float easeInTime;
    [SerializeField] float easeOutTime;
    // ��ʑJ��
    [SerializeField] public bool isTrans;
    [SerializeField] public bool isTransNow;

    // �J�ڐ�
    public string NextSceneName;

    private static S_Transition instance;


    void Awake()
    {
        //�V�[�����؂�ւ���Ă��I�u�W�F�N�g��ێ�����v���O����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializePositions();
        if (GameObject.FindWithTag("trans"))
        {
            if (GameObject.FindWithTag("trans") != this.gameObject)
            {
                Destroy(this);
            }
        }
        transform.position = new Vector3(500, 0, 0);
    }

    void InitializePositions()
    {
        isTrans = false;
    }

    void OnEnable()
    {
        //�V�[���ǂݍ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //�V�[���ǂݍ���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�[�������[�h���ꂽ��̓�����ĊJ
        if (isTrans)
        {
            Trans();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrans)
        {
            Trans();
        }
        if (Camera.main != null && Camera.main.GetUniversalAdditionalCameraData().cameraStack.Count == 0)
        {
            //�J�����ǉ�����
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(secondaryCamera);
        }

        //�v���C���[�̉������ʑJ�ڂ�����
        if (player == null && GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player");
        }
        if (player == null)
        {
            SPos_In = new Vector3(secondaryCamera.transform.position.x, 0, 0);
            EPos_Out = new Vector3(secondaryCamera.transform.position.x, 0, 0);
        }
        else if (player != null)
        {
            //�v���C���[��������|�W�V�����擾
            SPos_In = new Vector3(secondaryCamera.transform.position.x, player.transform.position.y - 1.5f, 0);
            EPos_Out = new Vector3(secondaryCamera.transform.position.x, player.transform.position.y - 1.5f, 0);
        }


        //if (SceneManager.GetActiveScene().name == "TitleScene")
        //{
        //    NextSceneName = "GameScene";
        //}
        //else if (SceneManager.GetActiveScene().name == "GameScene")
        //{
        //    NextSceneName = "ScoreScene";
        //}
        //else if (SceneManager.GetActiveScene().name == "ScoreScene")
        //{
        //    NextSceneName = "TitleScene";
        //}
    }

    private void Trans()
    {
        isTransNow = true;
        transObj.transform.position = SPos_In;
        transObj.transform.DOScale(SScale_In, easeInTime).SetEase(easeIn).OnComplete(() =>
        {
            //�����ŉ�ʐ؂�ւ�鏈��(��ʑJ�ڂŉ�ʂ������ĂȂ��ꏊ)

            SceneManager.LoadScene(NextSceneName);
            transObj.transform.position = EPos_Out;
            transObj.transform.DOScale(EScale_Out, easeOutTime).SetEase(easeOut).OnComplete(() =>
            {
                //��ʑJ�ڂ�����
                isTransNow = false;
            });
        });



        isTrans = false;
    }
    public void SetTransition(string name)
    {
        isTrans = true;
        NextSceneName = name;
    }

}
