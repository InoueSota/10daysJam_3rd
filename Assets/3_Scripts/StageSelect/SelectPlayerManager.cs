using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerManager : MonoBehaviour
{
    [SerializeField] private SelectManager selectManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StageGate"))
        {
            selectManager.SetEnterStage(collision.GetComponent<StageGateManager>().GetStageName());
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("StageGate"))
        {
            selectManager.SetLeaveStage();
        }
    }
}
