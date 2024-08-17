using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    #region State
    public bool isPause { get => _isPause; private set => _isPause = value; }
    #endregion

    #region Editor State
    [Header("State")]
    [SerializeField] bool _isPause = false;
    #endregion
    #region Components
    [Header("Components")]
    [SerializeField] GameObject containerPauseUI;
    #endregion

    void Start()
    {
        containerPauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPause = !isPause;

            if (!isPause)
            {
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                containerPauseUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        containerPauseUI.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }
}
