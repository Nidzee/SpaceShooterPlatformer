using System.Collections;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	static bool _ininted = false;
	public static bool disableCameras = false;




	void Awake() 
	{
		disableCameras = true;
		StartCoroutine(launcherAwakeRoutine());
	}

	IEnumerator launcherAwakeRoutine() 
	{
		initGame();
		Destroy(gameObject);
		yield return null;
	}

	


	static void initGame() 
	{
		if(_ininted) 
		{
			return;
		}
		_ininted = true;



		// string cultureName = CultureInfo.CurrentCulture?.Name;
		// Debug.Log($"[Launcher] Culture {cultureName}");
		// CrashReportHandler.SetUserMetadata("SessionId", Guid.NewGuid().ToString());


		// PassiveLauncher.configureAspectRatio();
		

		// Start game scene
		// InitialGameLoader.startInitialLoading();
	}
}