using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameTestPanel : EditorWindow
{
    [MenuItem ("IceShardInc / GameTestPanel")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameTestPanel), false, "GameTestPanel");
    }
}
