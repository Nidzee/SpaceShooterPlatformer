using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image _blackOverlay;
    [SerializeField] RectTransform _contentHolder;

    [SerializeField] UniversalButton _closeButton;

    string _sceneName;


    public void initSceneData(string sceneName)
    {
        _sceneName = sceneName;
    }
    
    public void showPopUp()
    {
        Debug.Log("[###] SHOW POPUP");
        _closeButton.OnClick.AddListener(ClosePopUp);
    }

    void ClosePopUp()
    {
        SceneManager.UnloadSceneAsync(_sceneName);
    }
}
