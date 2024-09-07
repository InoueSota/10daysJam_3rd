using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GrassScript : MonoBehaviour
{

    [Header("�ő�p�x")]
    [SerializeField] private float droopAngle = 15f;

    [Header("�|���͂̉e����")]
    [SerializeField] private float droopingPer = 1;

    [Header("�v���C���[�ɓ����������̗h�ꗦ")]
    [SerializeField] private float playerHitDroopingPow = 1;

    [Header("�v���C���[�ɓ��������̗h�ꗦ")]
    [SerializeField] private float playerMoveDroopingPow = 1;

    [Header("�߂��")]
    [SerializeField] private float returnPowAcc  = 60;
    [SerializeField] private float returnPow = 0;

    [SerializeField] private float rot = 0;
    private Vector3 eulerAngle_ = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        returnPow += returnPowAcc * Time.deltaTime * droopingPer;

        if (returnPow < rot)
        {
            rot -= returnPow;
        }
        else if (-returnPow > rot)
        {
            rot += returnPow;
        }
        else if (-returnPow <= rot && returnPow >= rot)
        {
            rot = 0;
            returnPow = 0.0f;
        }

        rot = Mathf.Clamp(rot, -droopAngle, droopAngle);

        //rot = rot % 90;



        eulerAngle_.z = rot;
        this.transform.localEulerAngles = eulerAngle_;
    }

    //�|���͂�������
    void Drooping(float pow)
    {
        returnPow = 0.0f;

        float powT = pow * Time.deltaTime;

        powT = Mathf.Clamp(powT, -droopAngle, droopAngle);
  
        rot += powT;
        rot = Mathf.Clamp(rot, -droopAngle, droopAngle);
    }

    //�����������̂̈ʒu�Ǝ����̈ʒu���ׁA�p�x�����߂�B�������獶�E�ǂ���ɓ|��邩�I�� 1 = ��
    int HitAngle(Vector3 targetPos)
    {

        Vector3 thisPos = this.transform.position;
        Vector3 targetVector= targetPos - thisPos;

        float degree = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg - 90;

       // Debug.Log(degree);
       // Debug.Log(rot - degree);

        if(rot - degree < 0)
        {
            return -1;
        }

        return 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMoveManager player = collision.GetComponent<PlayerMoveManager>();
            Drooping(playerHitDroopingPow * HitAngle(collision.gameObject.transform.position));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMoveManager player = collision.GetComponent<PlayerMoveManager>();
            if (player.GetSpeed() > 0)
            {
                Drooping(playerMoveDroopingPow * -player.GetDirection());
            }
        }
    }

    public void Init(Vector3 pos , Vector3 scale)
    {
        this.transform.localPosition = pos;

        Vector3 localScale = scale;
        Vector3 parentLossyScale = this.transform.parent.lossyScale;

        this.transform.localScale  = new Vector3(
         localScale.x / parentLossyScale.x,
         localScale.y / parentLossyScale.y,
         localScale.z / parentLossyScale.z);
    }


}
