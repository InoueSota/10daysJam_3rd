using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrassParentScript : MonoBehaviour
{
    [SerializeField] private GrassScript grassPrefab = null;

    [Header("草召喚高さ")]
    [SerializeField] private float offSetY = 0.5f;

    [Header("召喚本数")]
    [SerializeField] private int grassCount = 1;

    [Header("召喚時のズレ率の最大(前の距離*ズレ率の分ズレる)")]
    [SerializeField, Range(0.0f, 1.0f)] private float shiftPerMax = 0;
    private float shiftPer = 0;

    [Header("長さの比率")]
    [SerializeField, Range(0.0f, 1.0f)] private float grassLengthPerMax = 0.5f;
    private float grassLengthPer = 0.5f;

    [Header("関節数")]
    [SerializeField] private int jointCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        GrassGrow();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void GrassGrow()
    {

        for (int grassNum = 0; grassNum < grassCount; grassNum++)
        {
            
            GrassScript grass = null;

            for (int jointNum = 0; jointNum < jointCount; jointNum++)
            {
                //召喚位置
                Vector3 growPos = Vector3.zero;
                growPos.y = offSetY;
                // 本数による距離間
                float grassDistance = 1.0f / grassCount;

                //最初を半分ずらしてはやす
                growPos.x = (grassDistance * 0.5f + grassDistance * (grassNum)) - 0.5f - shiftPer;


                //大きさ
                Vector3 grassScale = Vector3.one;

                float oneDot = 1.0f / 16.0f;

                grassScale.x = oneDot * 1.5f;
                grassScale.y = oneDot * 8.0f;


                GameObject parent = this.gameObject;
                //
                if(grass != null)
                {
                    parent = grass.gameObject; 
                }

                grass = Instantiate(grassPrefab, Vector3.zero, Quaternion.identity);

                grass.transform.parent = parent.transform;

                grass.Init(growPos, grassScale);
            }
        }
    }

}
