using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GrassScript : MonoBehaviour
{

    [Header("最大角度")]
    [SerializeField] private float droopAngle = 15f;

    private float droopingPer = 1;

    [Header("プレイヤーに当たった時の揺れ率")]
    [SerializeField] private float playerHitDroopingPow = 1;

    [Header("プレイヤーに動いた時の揺れ率")]
    [SerializeField] private float playerMoveDroopingPow = 1;

    [Header("戻る力")]
    [SerializeField] private float returnPowAcc  = 60;
    [SerializeField] private float returnPow = 0;

    [SerializeField] private float rot = 0,totalRot;
    private Vector3 eulerAngle_ = Vector3.zero;

    GrassScript parent;
    GrassParentScript baseParent;
    SpriteRenderer sprite = null;
    BoxCollider2D collider=null;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.GetComponent<GrassScript>();
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        sprite.enabled = baseParent.GetActive();


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


        if (parent != null)
        {
            totalRot = parent.GetTotalRot();
            totalRot += rot;
        }
        else
        {
            totalRot = rot;
        }

        eulerAngle_.z = totalRot;
        this.transform.eulerAngles = eulerAngle_;
    }

    //倒れる力を加える
    void Drooping(float pow)
    {
        returnPow = 0.0f;

        float powT = pow * Time.deltaTime;

        powT = Mathf.Clamp(powT, -droopAngle, droopAngle);
  
        rot += powT;
        rot = Mathf.Clamp(rot, -droopAngle, droopAngle);
    }

    //当たった物体の位置と自分の位置を比べ、角度を求める。そこから左右どちらに倒れるか選択 1 = 左
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

    public void Init(Vector3 pos , Vector3 scale, float droopingPer_, GrassParentScript baseParent_)
    {

        baseParent = baseParent_;

        droopingPer = droopingPer_;

        this.transform.localPosition = pos;

        sprite = this.GetComponent<SpriteRenderer>();
        collider = this.GetComponent<BoxCollider2D>();

        sprite.size = scale;
        collider.size = scale;
        scale.x = 0.0f;
        collider.offset = scale * 0.5f;

    }

    public float GetTotalRot()
    {
        return totalRot;
    }

    public float GetLength()
    {
        return sprite.size.y;
    }

    public float GetWide()
    {
        return sprite.size.x;
    }
}

    
