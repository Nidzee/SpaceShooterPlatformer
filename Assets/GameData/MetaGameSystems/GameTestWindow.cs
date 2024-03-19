using UnityEditor;
using UnityEngine;

public class GameTestWindow : EditorWindow
{

    const int TEST_ADD_CURRENCY_AMOUNT = 100;
    const int TEST_REMOVE_CURRENCY_AMOUNT = 50;




    [MenuItem ("IceShardInc/Game Test Window")]
    static void Init () 
    {
        // Get existing open window or if none, make a new one:
        GameTestWindow window = (GameTestWindow)EditorWindow.GetWindow (typeof (GameTestWindow));
    }


    void OnGUI () {

        // Skip if game is not running
        if (!Application.isPlaying)
        {
            return;
        }



        GUILayout.Label("[Currency game-test]");
        GUILayout.Space(10);

        GUILayout.Label("[Coins]");
        if (GUILayout.Button("+" + TEST_ADD_CURRENCY_AMOUNT + " coins"))
        {
            CurrencyDataManager.Instance.AddCurrency(CurrencyType.Coins, TEST_ADD_CURRENCY_AMOUNT);
        }
        if (GUILayout.Button("-" + TEST_REMOVE_CURRENCY_AMOUNT + " coins"))
        {
            CurrencyDataManager.Instance.RemoveCurrency(CurrencyType.Coins, TEST_REMOVE_CURRENCY_AMOUNT);
        }


        GUILayout.Space(10);
        GUILayout.Label("[Crystals]");
        if (GUILayout.Button("+" + TEST_ADD_CURRENCY_AMOUNT + " crystals"))
        {
            CurrencyDataManager.Instance.AddCurrency(CurrencyType.Crystals, TEST_ADD_CURRENCY_AMOUNT);
        }
        if (GUILayout.Button("-" + TEST_REMOVE_CURRENCY_AMOUNT + " crystals"))
        {
            CurrencyDataManager.Instance.RemoveCurrency(CurrencyType.Crystals, TEST_REMOVE_CURRENCY_AMOUNT);
        }
    }
}