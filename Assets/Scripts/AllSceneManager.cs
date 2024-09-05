using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    // 入力管理オブジェクト
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        Scene();
    }

    void Scene()
    {
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.RESET))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // ウィンドウを閉じる
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
