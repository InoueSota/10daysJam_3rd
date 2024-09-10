using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class S_Transition : MonoBehaviour
{
    //プレイヤー取得
    GameObject player;
    // オブジェクト
    [SerializeField] GameObject transObj;

    public Camera secondaryCamera;
    // どこから始まるかポジション
    [Header("勝手に入る")]
    [SerializeField] Vector3 SPos_In;
    [SerializeField] Vector3 EPos_Out;
    [Header("入力しなきゃダメ")]
    [SerializeField] Vector3 SScale_In;
    [SerializeField] Vector3 EScale_Out;
    //イージング
    [SerializeField] Ease easeIn;
    [SerializeField] Ease easeOut;
    [SerializeField] float easeInTime;
    [SerializeField] float easeOutTime;
    // 画面遷移
    [SerializeField] public bool isTrans;
    [SerializeField] public bool isTransNow;

    // 遷移先
    public string NextSceneName;

    private static S_Transition instance;


    void Awake()
    {
        //シーンが切り替わってもオブジェクトを保持するプログラム
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
        //シーン読み込み
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //シーン読み込み
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンがロードされた後の動作を再開
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
            //カメラ追加処理
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(secondaryCamera);
        }

        //プレイヤーの下から画面遷移させる
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
            //プレイヤーがいたらポジション取得
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
            //ここで画面切り替わる処理(画面遷移で画面が見えてない場所)

            SceneManager.LoadScene(NextSceneName);
            transObj.transform.position = EPos_Out;
            transObj.transform.DOScale(EScale_Out, easeOutTime).SetEase(easeOut).OnComplete(() =>
            {
                //画面遷移が閉じる
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
