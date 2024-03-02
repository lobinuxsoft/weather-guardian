using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    [SerializeField] private EventSystem eventSystem;
    public event EventHandler OnEndGameState;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            Hide();
        });
        Time.timeScale = 0f;
        mainMenuButton.Select();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    
}
