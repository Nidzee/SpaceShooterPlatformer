using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public enum TabButtonType
{
    ArsenalTab = 0,
    HealthArmourTab = 1,
}

public class TabButton : MonoBehaviour
{
    bool _isClicked;
    [SerializeField] BasicButton _button;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _iconImage;
    [SerializeField] TabButtonType _tabButtonType;


    [Header("Resources")]
    [SerializeField] Sprite _activeBackground;
    [SerializeField] Sprite _passiveBackground;
    [SerializeField] Sprite _activeIcon;
    [SerializeField] Sprite _passiveIcon;

    public UnityEvent OnTabButtonActivated = new UnityEvent();

    public TabButtonType TabButtonType => _tabButtonType;



    public void Init()
    {
        DeactivateButton();

        _button.BaseButton.onClick.AddListener(triggerButtonClick);
    }

    public void DeactivateButton()
    {
        _isClicked = false;
        _backgroundImage.sprite = _passiveBackground;
        _iconImage.sprite = _passiveIcon;
    }

    public void ActivateButton()
    {
        _isClicked = true;
        _backgroundImage.sprite = _activeBackground;
        _iconImage.sprite = _activeIcon;
    }

    void triggerButtonClick()
    {
        if (_isClicked)
        {
            Debug.LogWarning("[###] Click on same tab button");
            return;
        }

        OnTabButtonActivated.Invoke();
    }
}