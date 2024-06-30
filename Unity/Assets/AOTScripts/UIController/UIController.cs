using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas menu;
    [SerializeField]
    private Canvas touchZone;

    private EventSystem eventSystem;
    private InputActionAsset _inputActionAsset;
    private InputActionMap _player;
    private InputAction _point;
    private InputAction _click;
    private InputAction _esc;
    private InputAction _alt;

    public bool _isPause = false;
    public bool _isInputEnable = true;
    public bool _isPressingEsc = false;
    public bool _isPressingAlt = false;

    [SerializeField]
    private List<string> ShowCursorScene;
    [SerializeField]
    private string EventSystemGOName;
    [SerializeField]
    private int vSyncSettings;

    public int VSyncSettings
    {
        get => vSyncSettings;
        set => vSyncSettings = value;
    }

    private void Awake()
    {
        Debug.Log("UIController Init Done.");

        Application.targetFrameRate = m_FrameRate;
        Time.timeScale = m_GameSpeed;
        Application.runInBackground = m_RunInBackground;
        Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
    }

    private void Start()
    {
        eventSystem = transform.Find(EventSystemGOName).GetComponent<EventSystem>();
        _inputActionAsset = eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset;
        _player = _inputActionAsset.FindActionMap("Player");
        _esc = _inputActionAsset.FindActionMap("UI").FindAction("Menu");
        _alt = _inputActionAsset.FindActionMap("UI").FindAction("Alt");

        _esc.Enable();
        _alt.Enable();


#if UNITY_ANDROID
        if (touchZone != null)
        {
            touchZone.gameObject.SetActive(true);
        }

#else
        if (touchZone != null)
        {
            touchZone.gameObject.SetActive(false);
        }
#endif
    }

    bool esc = false;
    bool alt = false;
    private void Update()
    {
        OnValueChangedVSync(vSyncSettings);

        if (_esc != null)
        {
            esc = Convert.ToBoolean(_esc.ReadValue<float>());
        }
        if (_alt != null)
        {
            alt = Convert.ToBoolean(_alt.ReadValue<float>());
        }

        if (esc && !_isPressingEsc)
        {
            //_isPause = !_isPause;
            _isPressingEsc = true;
        }
        else if (!esc)
        {
            _isPressingEsc = false;
        }

        if (alt && !_isPressingAlt)
        {
            _isPressingAlt = true;
        }
        else if (!alt)
        {
            _isPressingAlt = false;
        }

        //Time.timeScale = _isPause ? 0.0f : 1.0f;
        AudioListener.pause = _isPause;
        if (menu != null)
        {
            menu.gameObject.SetActive(_isPause);
        }
#if UNITY_ANDROID
        if (touchZone != null)
        {
            touchZone.gameObject.SetActive(!_isPause);
        }
#endif

        if (_isPause || _isPressingAlt || GetCursorLockModeState())
        {
            //_player.Disable();
#if !UNITY_ANDROID
            Cursor.lockState = CursorLockMode.None;
#endif
        }
        else
        {
            //_player.Enable();
#if !UNITY_ANDROID
            Cursor.lockState = CursorLockMode.Locked;
#endif
        }
    }

    private bool GetCursorLockModeState()
    {
        return ShowCursorScene.Any(item => item == GetCurrentSceneName());
    }

    public string GetCurrentSceneName()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene != null ? currentScene.name : null;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
#if !UNITY_ANDROID
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private void OnValueChangedVSync(Int32 value)
    {
        QualitySettings.vSyncCount = value;
    }

    public void OnClickExit()
    {
        Debug.Log("Exit");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    //Common

    [SerializeField]
    private int m_FrameRate = 60;

    [SerializeField]
    private float m_GameSpeed = 1f;

    [SerializeField]
    private bool m_RunInBackground = true;

    [SerializeField]
    private bool m_NeverSleep = true;

    private float m_GameSpeedBeforePause = 1f;

    /// <summary>
    /// 获取或设置游戏帧率。
    /// </summary>
    public int FrameRate
    {
        get => m_FrameRate;
        set => Application.targetFrameRate = m_FrameRate = value;
    }

    /// <summary>
    /// 获取或设置游戏速度。
    /// </summary>
    public float GameSpeed
    {
        get => m_GameSpeed;
        set => Time.timeScale = m_GameSpeed = value >= 0f ? value : 0f;
    }

    /// <summary>
    /// 获取游戏是否暂停。
    /// </summary>
    public bool IsGamePaused => m_GameSpeed <= 0f;

    /// <summary>
    /// 获取是否正常游戏速度。
    /// </summary>
    public bool IsNormalGameSpeed => Math.Abs(m_GameSpeed - 1f) < 0.01f;

    /// <summary>
    /// 获取或设置是否允许后台运行。
    /// </summary>
    public bool RunInBackground
    {
        get => m_RunInBackground;
        set => Application.runInBackground = m_RunInBackground = value;
    }

    /// <summary>
    /// 获取或设置是否禁止休眠。
    /// </summary>
    public bool NeverSleep
    {
        get => m_NeverSleep;
        set
        {
            m_NeverSleep = value;
            Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
        }
    }

    /// <summary>
    /// 暂停游戏。
    /// </summary>
    public void PauseGame()
    {
        if (IsGamePaused)
        {
            return;
        }

        m_GameSpeedBeforePause = GameSpeed;
        GameSpeed = 0f;
    }

    /// <summary>
    /// 恢复游戏。
    /// </summary>
    public void ResumeGame()
    {
        if (!IsGamePaused)
        {
            return;
        }

        GameSpeed = m_GameSpeedBeforePause;
    }

    /// <summary>
    /// 重置为正常游戏速度。
    /// </summary>
    public void ResetNormalGameSpeed()
    {
        if (IsNormalGameSpeed)
        {
            return;
        }

        GameSpeed = 1f;
    }

}
#endif
