// using System;
// using System.Threading.Tasks;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// namespace Sources.scenes {
// 	public class LoadingScene : MonoBehaviour {
		
// 		public enum Style 
// 		{
// 			Loading,
// 			LoadingNoProgress
// 		}
	

// 		static LoadingScene _instance;

// 		[SerializeField] LoadingBar _loadingBar = null;
	
// 		[SerializeField] Image _gameLogo = null;
// 		[SerializeField] Sprite _logoEn = null;
// 		[SerializeField] Sprite _logoCn = null;
	
// 		[SerializeField] Canvas _rootCanvas;
// 		[SerializeField] CanvasGroup _rootCanvasGroup;
	
// 		static bool _isInteractiveLoadingActive = false;
// 		static CallbackVoid _pendingCallbacks = null;
	
// 		public static LoadingTaskRunner LatestTask = null;

// 		Tweener _fadeTween;
		
// 		const float FADE_DURATION = 0.3f;
	



// 		void Awake() {
// 			_instance = this;
// 			// _gameLogo.sprite = GameRegionConfig.isRegion(GameRegion.China) ? _logoCn : _logoEn;
// 		}
	
// 		void setStyle(Style style) {
// 			if (style == Style.Loading) {
// 				_loadingBar.gameObject.SetActive(true);
// 			} else if (style == Style.LoadingNoProgress) {
// 				_loadingBar.gameObject.SetActive(false);
// 			}
// 		}

// 		async Task runLoadingTask(ILoadingTaskV2 task) {
			
// 			show();


// 			LoadingTaskRunner taskRunner = new LoadingTaskRunner("Load", task);
// 			LatestTask = taskRunner;

// 			var context = new ManualSubjectContext();
// 			taskRunner.onProgressPercents.subscribe(context, onPercentsChanged);

// 			await taskRunner.execute();
		
// 			if (UBuild.isDebugBuild) {
// 				Debug.Log("Load Result:");
// 				Debug.Log(taskRunner.mainTrace.toStringTrace());
// 			}
// 			context.unsubscribeAll();


// 			hide();
// 		}
		



// 		void show() {
// 			_rootCanvas.gameObject.SetActive(true);
// 			_rootCanvasGroup.blocksRaycasts = true;
// 			_fadeTween = _rootCanvasGroup.DOFade(1, FADE_DURATION);
// 		}

// 		void hide() {
// 			_fadeTween = _rootCanvasGroup
// 				.DOFade(0, FADE_DURATION)
// 				.OnComplete(() => {
// 					_rootCanvasGroup.blocksRaycasts = false;
// 					_rootCanvas.gameObject.SetActive(false);
// 				});
// 		}
		




// 		void onPercentsChanged(int percents) {
// 			if (_loadingBar != null) {
// 				_loadingBar.updatePercents(percents);
// 			}
// 		}
		
// 		public static void planLoadingTaskForNowOrLater(string loadingTag, Style style, ILoadingTaskV2 task, CallbackVoid originalCallback = null) {
// 			if (_isInteractiveLoadingActive) {
// 				_pendingCallbacks += delegate {
// 					planLoadingTaskForNowOrLater(loadingTag, style, task, originalCallback);
// 				};
// 			} else {
// 				executeLoadingTaskInteractive(loadingTag, style, task)
// 					.Callback(() => originalCallback?.Invoke());
// 			}
// 		}

// 		public static async UniTask<TScene> openSceneAsync<TScene>(Action<DiContainer> bindings = null) 
// 			where TScene : GameScene {
// 			TScene scene = null;
// 			var task = GameScene.getOpenSceneLoadingTask<TScene>(
// 				loadedScene => scene = loadedScene,
// 				bindings);
// 			await executeLoadingTaskInteractive($"OpenScene.{typeof(TScene).Name}", Style.Loading, task);
// 			return scene;
// 		}

// 		/// <summary>
// 		/// Opens the loading scene and executes the given LoadingTask while showing progress
// 		/// </summary>
// 		/// <param name="loadingTag"></param>
// 		/// <param name="style"></param>
// 		/// <param name="task"></param>
// 		/// <param name="originalCallback">Callback is called when the loading task has finished. 
// 		/// If the loading task contains scene openening, than the scene callback will be called first.</param>
// 		public static async UniTask executeLoadingTaskInteractive(string loadingTag, Style style, ILoadingTaskV2 task) {
// 			if (_isInteractiveLoadingActive) {
// 				// Report exception, so we can see from where the query to task is comming
// 				Debug.LogException(new System.Exception(loadingTag + ": LoadingScene multiple interactive tasks"));
// 				return;
// 			}

// 			_isInteractiveLoadingActive = true;
		
// 			BackendTimeManager.pauseTimeChecks();

// 			await executeTask(style, task);
		
// 			_isInteractiveLoadingActive = false;
// 			BackendTimeManager.resumeTimeChecks();

// 			CallbackVoid pending = _pendingCallbacks;
// 			_pendingCallbacks = null;
			
// 			pending?.Invoke();
// 		}

// 		static async UniTask executeTask(Style style, ILoadingTaskV2 task) {
// 			if (_instance == null) {
// 				await openLoadingSceneAndExecute(style, task);
// 			} else {
// 				_instance.setStyle(style);
// 				await _instance.runLoadingTask(task);
// 			}
// 		}

// 		static async UniTask openLoadingSceneAndExecute(Style style, ILoadingTaskV2 task) {
// 			var loadingSceneTask = new AsyncLoadingTaskV2(
// 				async () => {
// 					await SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
// 					await UniTask.Yield();
// 					if (_instance != null) {
// 						_instance.setStyle(style);
// 					}
// 				});
			
			
		
// 			LoadingTaskRunner mainRunner = new LoadingTaskRunner("LoadingScene.Open", loadingSceneTask);

// 			await mainRunner.execute();

// 			if (UBuild.isDebugBuild) {
// 				Debug.Log("Loading Scene:");
// 				Debug.Log(mainRunner.mainTrace.toStringTrace());
// 			}
		
// 			if (_instance != null) {
// 				await _instance.runLoadingTask(task);
// 			} else {
// 				Debug.LogException(new System.Exception("Failed to open Loading Scene"));
// 				//Perfom loading in background
// 				LoadingTaskRunner secondRunner = new LoadingTaskRunner("LoadingScene.Background", task);
// 				await secondRunner.execute();
			
// 				if (UBuild.isDebugBuild) {
// 					Debug.Log("Loading Scene2:");
// 					Debug.Log(secondRunner.mainTrace.toStringTrace());
// 				}
// 			}
// 		}
// 	}
// }
