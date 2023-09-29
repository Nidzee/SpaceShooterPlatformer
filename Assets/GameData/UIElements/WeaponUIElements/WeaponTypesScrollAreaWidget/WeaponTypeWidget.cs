using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class WeaponTypeWidget : MonoBehaviour
{
    [SerializeField] GameObject _selectedOutline;
    [SerializeField] TMP_Text _weaponName;
    [SerializeField] BasicButton _button;
    [SerializeField] UniversalButton _unlockButton;


    WeaponType_GameData _data;
    public WeaponType_GameData WidgetData => _data;

    public UnityEvent<WeaponTypeWidget> OnWidgetClick = new UnityEvent<WeaponTypeWidget>();
    public UnityEvent<WeaponTypeWidget> OnUnlockClick = new UnityEvent<WeaponTypeWidget>();


    public void init(WeaponType_GameData data)
    {
        _data = data;
        _button.BaseButton.onClick.AddListener(() => OnWidgetClick.Invoke(this));
        _unlockButton.BaseButton.onClick.AddListener(() => OnUnlockClick.Invoke(this));

        _unlockButton.SetLabel(data.UnlockPrice.ToString());

        RefreshWidget();
    }

    public void RefreshWidget()
    {
        // Check if weapon is unlocked
        bool isUnlocked = false;
        foreach (var save in PlayerDataManager.Instance.PlayerData.WeaponData.WeaponsSavesCollection)
        {
            if (save.WeaponType == _data.WeaponType)
            {
                isUnlocked = save.IsUnlocked;
            }

            continue;
        }



        if (isUnlocked)
        {
            _weaponName.text = _data.WeaponName;
            _unlockButton.gameObject.SetActive(false);
        } 
        else
        {
            _weaponName.text = "LOCKED";
            _unlockButton.gameObject.SetActive(true);
        }
    }


    



    public void UpdateOutline(bool isSelected)
    {
        _selectedOutline.gameObject.SetActive(isSelected);
    }
}