using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using System;

public static class PopUpController
{
    public static void OpenSpecificScene<SceneType>(string sceneName, Action<SceneType> callback) where SceneType : PopUp
    {

        // Create operation to load specific scene
        var openSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);


        // Add callback invoke after scene was loaded
        openSceneOperation.completed += (s) => {

            // Get opened scene
            var openedScene = SceneManager.GetSceneByName(sceneName);
            
            // Try to get specific type from opened scene
            var openedSceneByType = GetTargetSceneType<SceneType>(openedScene);
            
            // If loaded scene can not get specific type -> skip
            if (openedSceneByType == null)
            {
                Debug.LogError("[###] ERROR! Error during scene opening. Fail to load: " + sceneName);
                return;
            }

            openedSceneByType.initSceneData(sceneName);

            // If scene was successfully opened by correct type -> launch callback
            callback.Invoke(openedSceneByType);
        };
    }

    static SceneType GetTargetSceneType<SceneType>(Scene scene)
    {

        // Get scene root objects
        var sceneRootObjects = scene.GetRootGameObjects();


        // Try to get target type of scene
        var rootSceneComponent = sceneRootObjects.FirstOrDefault(i => i.GetComponent<SceneType>() != null);
        if (rootSceneComponent == null) {
            return default;
        }


        // Get target scene type
        var popUpContent = rootSceneComponent.GetComponent<SceneType>();
        return popUpContent;
    }
}