using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UniversalButton : BasicButton
{

    public enum ButtonStyle
    {
        Green = 0,
        Blue = 1,
        Yellow = 3,
        Red = 4,
        Gray = 5,
        Custom = 100,
    }

    public enum LabelStyle
    {
        Title = 0,
        TitleBold = 1,
        TitleCoins = 2,
        TitleCrystals = 3,
        Custom = 100,
    }
    
    [SerializeField] ButtonStyle _buttonStyle;
    [SerializeField] LabelStyle _labelStyle;


    [SerializeField] Image _buttonImage;
    [SerializeField] TMP_Text _buttonLabel;
    [SerializeField] string _buttonText = "Button";
    [SerializeField] UniversalButtonConfigAsset _asset;


    public void SetButtonPrice(int price)
    {
        _buttonLabel.text = price.ToString() + "$";
    }

    public void SetLabel(string label)
    {
        _buttonLabel.text = label;
    }



#if UNITY_EDITOR
    public void OnValidate()
    {
        ApplyButtonStyle();
    }
#endif  

    public void Awake()
    {
        ApplyButtonStyle();
        BaseButton.onClick.AddListener(() => { OnClick?.Invoke(); });
    }

    void ApplyButtonStyle()
    {
        // Skip if button is custom
        if (_buttonStyle == ButtonStyle.Custom)
        {
            return;
        }

        var data = _asset.GetButtonConfig(_buttonStyle);
        if (data == null)
        {
            Debug.LogError("Error! No config for: " + _buttonStyle);
            return;
        }


        // Apply button label material
        var targetMat = data.Font_Bold;
        _buttonLabel.fontSharedMaterial = targetMat;


        // Apply button image
        _buttonImage.sprite = data.ButtonSprite;


        _buttonLabel.text = _buttonText;
    }


    [System.Serializable]
    public class ButtonVisualsConfig
    {
        public Material Font_Slim;
        public Material Font_Bold;
        public Sprite ButtonSprite;
    }

    [System.Serializable]
    public class ButtonStyleConfig
    {
        public ButtonStyle ButtonStyle;
        public ButtonVisualsConfig VisualsConfig;
    }

}