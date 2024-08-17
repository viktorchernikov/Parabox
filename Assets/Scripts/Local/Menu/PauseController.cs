using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;

    #region State
    public bool isPaused { get => _isPaused; private set => _isPaused = value; }
    #endregion

    #region Editor State
    [Header("State")]
    [SerializeField] bool _isPaused = false;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        isPaused = false;
    }
}
