using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartGameMissionCanvas : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] private EventSystem eventSystem;
    public event EventHandler OnStartGame;

    private void Start()
    {

        startGameButton.onClick.AddListener(() =>
        {
            OnStartGame?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1.0f;
            Hide();
        });
        Time.timeScale = 0f;
        startGameButton.Select();

        //pauseUnpause.
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
