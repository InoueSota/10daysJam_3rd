using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    // ���͊Ǘ��I�u�W�F�N�g
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

        // �E�B���h�E�����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
