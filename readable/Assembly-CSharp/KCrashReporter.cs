using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Klei;
using KMod;
using Newtonsoft.Json;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x020009D7 RID: 2519
public class KCrashReporter : MonoBehaviour
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x0600491A RID: 18714 RVA: 0x001A6EB4 File Offset: 0x001A50B4
	// (remove) Token: 0x0600491B RID: 18715 RVA: 0x001A6EE8 File Offset: 0x001A50E8
	public static event Action<bool> onCrashReported;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x0600491C RID: 18716 RVA: 0x001A6F1C File Offset: 0x001A511C
	// (remove) Token: 0x0600491D RID: 18717 RVA: 0x001A6F50 File Offset: 0x001A5150
	public static event Action<float> onCrashUploadProgress;

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x0600491E RID: 18718 RVA: 0x001A6F83 File Offset: 0x001A5183
	// (set) Token: 0x0600491F RID: 18719 RVA: 0x001A6F8A File Offset: 0x001A518A
	public static bool hasReportedError { get; private set; }

	// Token: 0x06004920 RID: 18720 RVA: 0x001A6F94 File Offset: 0x001A5194
	private void OnEnable()
	{
		KCrashReporter.dataRoot = Application.dataPath;
		Application.logMessageReceived += this.HandleLog;
		KCrashReporter.ignoreAll = true;
		string path = Path.Combine(KCrashReporter.dataRoot, "hashes.json");
		if (File.Exists(path))
		{
			StringBuilder stringBuilder = new StringBuilder();
			MD5 md = MD5.Create();
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
			if (dictionary.Count > 0)
			{
				bool flag = true;
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					string value = keyValuePair.Value;
					stringBuilder.Length = 0;
					using (FileStream fileStream = new FileStream(Path.Combine(KCrashReporter.dataRoot, key), FileMode.Open, FileAccess.Read))
					{
						foreach (byte b in md.ComputeHash(fileStream))
						{
							stringBuilder.AppendFormat("{0:x2}", b);
						}
						if (stringBuilder.ToString() != value)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					KCrashReporter.ignoreAll = false;
				}
			}
			else
			{
				KCrashReporter.ignoreAll = false;
			}
		}
		else
		{
			KCrashReporter.ignoreAll = false;
		}
		if (KCrashReporter.ignoreAll)
		{
			global::Debug.Log("Ignoring crash due to mismatched hashes.json entries.");
		}
		if (File.Exists("ignorekcrashreporter.txt"))
		{
			KCrashReporter.ignoreAll = true;
			global::Debug.Log("Ignoring crash due to ignorekcrashreporter.txt");
		}
		if (Application.isEditor && !GenericGameSettings.instance.enableEditorCrashReporting)
		{
			KCrashReporter.terminateOnError = false;
		}
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x001A713C File Offset: 0x001A533C
	private void OnDisable()
	{
		Application.logMessageReceived -= this.HandleLog;
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x001A7150 File Offset: 0x001A5350
	private void HandleLog(string msg, string stack_trace, LogType type)
	{
		if ((KCrashReporter.logCount += 1U) == 10000000U)
		{
			DebugUtil.DevLogError("Turning off logging to avoid increasing the file to an unreasonable size, please review the logs as they probably contain spam");
			global::Debug.DisableLogging();
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		if (msg != null && msg.StartsWith(DebugUtil.START_CALLSTACK))
		{
			string text = msg;
			msg = text.Substring(text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) + DebugUtil.END_CALLSTACK.Length);
			stack_trace = text.Substring(DebugUtil.START_CALLSTACK.Length, text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) - DebugUtil.START_CALLSTACK.Length);
		}
		if (Array.IndexOf<string>(KCrashReporter.IgnoreStrings, msg) != -1)
		{
			return;
		}
		if (msg != null && msg.StartsWith("<RI.Hid>"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to load cursor"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to save a temporary cursor"))
		{
			return;
		}
		if (type == LogType.Exception)
		{
			RestartWarning.ShouldWarn = true;
		}
		if (this.errorScreen == null && (type == LogType.Exception || type == LogType.Error))
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			if (SpeedControlScreen.Instance != null)
			{
				SpeedControlScreen.Instance.Pause(true, true);
			}
			string text2 = msg;
			string text3 = stack_trace;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = new StackTrace(5, true).ToString();
			}
			if (App.isLoading)
			{
				if (!SceneInitializerLoader.deferred_error.IsValid)
				{
					SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError
					{
						msg = text2,
						stack_trace = text3
					};
					return;
				}
			}
			else
			{
				this.ShowDialog(text2, text3, true, null, null);
			}
		}
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x001A72C8 File Offset: 0x001A54C8
	public bool ShowDialog(string error, string stack_trace, bool includeSaveFile = true, string[] extraCategories = null, string[] extraFiles = null)
	{
		GenericGameSettings instance = GenericGameSettings.instance;
		if (instance.devBootSmoke || !string.IsNullOrEmpty(instance.scriptedProfile.saveGame))
		{
			KCrashReporter.hasCrash = true;
			global::Debug.Log("Automated test resulted in a crash. Return exit code 1.");
			App.QuitCode(1);
			return false;
		}
		if (this.errorScreen != null)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find(KCrashReporter.error_canvas_name);
		if (gameObject == null)
		{
			gameObject = new GameObject();
			gameObject.name = KCrashReporter.error_canvas_name;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 32767;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		this.errorScreen = UnityEngine.Object.Instantiate<GameObject>(this.reportErrorPrefab, Vector3.zero, Quaternion.identity);
		this.errorScreen.transform.SetParent(gameObject.transform, false);
		ReportErrorDialog errorDialog = this.errorScreen.GetComponentInChildren<ReportErrorDialog>();
		string stackTrace = error + "\n\n" + stack_trace;
		if (Global.Instance != null && Global.Instance.modManager != null && Global.Instance.modManager.HasCrashableMods())
		{
			Exception ex = DebugUtil.RetrieveLastExceptionLogged();
			StackTrace stackTrace2 = (ex != null) ? new StackTrace(ex) : new StackTrace(5, true);
			Global.Instance.modManager.SearchForModsInStackTrace(stackTrace2);
			Global.Instance.modManager.SearchForModsInStackTrace(stack_trace);
			errorDialog.PopupDisableModsDialog(stackTrace, new System.Action(this.OnQuitToDesktopCrashed), (Global.Instance.modManager.IsInDevMode() || !KCrashReporter.terminateOnError) ? new System.Action(this.OnCloseErrorDialog) : null);
		}
		else
		{
			errorDialog.PopupSubmitErrorDialog(stackTrace, delegate
			{
				KCrashReporter.ReportError(error, stack_trace, this.confirmDialogPrefab, this.errorScreen, errorDialog.UserMessage(), includeSaveFile, extraCategories, extraFiles);
			}, new System.Action(this.OnQuitToDesktopCrashed), KCrashReporter.terminateOnError ? null : new System.Action(this.OnCloseErrorDialog));
		}
		return true;
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x001A74EB File Offset: 0x001A56EB
	private void OnCloseErrorDialog()
	{
		UnityEngine.Object.Destroy(this.errorScreen);
		this.errorScreen = null;
		KCrashReporter.hasCrash = false;
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x001A751D File Offset: 0x001A571D
	private void OnQuitToDesktopCrashed()
	{
		App.QuitCode(1);
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x001A7528 File Offset: 0x001A5728
	private static string GetUserID()
	{
		if (DistributionPlatform.Initialized)
		{
			string[] array = new string[5];
			array[0] = DistributionPlatform.Inst.Name;
			array[1] = "ID_";
			array[2] = DistributionPlatform.Inst.LocalUser.Name;
			array[3] = "_";
			int num = 4;
			DistributionPlatform.UserId id = DistributionPlatform.Inst.LocalUser.Id;
			array[num] = ((id != null) ? id.ToString() : null);
			return string.Concat(array);
		}
		return "LocalUser_" + Environment.UserName;
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x001A75A4 File Offset: 0x001A57A4
	private static string GetLogContents()
	{
		string path = Util.LogFilePath();
		if (File.Exists(path))
		{
			using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
		return "";
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x001A7610 File Offset: 0x001A5810
	public static void ReportDevNotification(string notification_name, string stack_trace, string details = "", bool includeSaveFile = false, string[] extraCategories = null)
	{
		if (KCrashReporter.previouslyReportedDevNotifications == null)
		{
			KCrashReporter.previouslyReportedDevNotifications = new HashSet<int>();
		}
		details = notification_name + " - " + details;
		global::Debug.Log(details);
		int hashValue = new HashedString(notification_name).HashValue;
		bool hasReportedError = KCrashReporter.hasReportedError;
		if (!KCrashReporter.previouslyReportedDevNotifications.Contains(hashValue))
		{
			KCrashReporter.previouslyReportedDevNotifications.Add(hashValue);
			if (extraCategories != null)
			{
				Array.Resize<string>(ref extraCategories, extraCategories.Length + 1);
				extraCategories[extraCategories.Length - 1] = KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION;
			}
			else
			{
				extraCategories = new string[]
				{
					KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION
				};
			}
			KCrashReporter.ReportError("DevNotification: " + notification_name, stack_trace, null, null, details, includeSaveFile, extraCategories, null);
		}
		KCrashReporter.hasReportedError = hasReportedError;
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x001A76C0 File Offset: 0x001A58C0
	public static void ReportError(string msg, string stack_trace, ConfirmDialogScreen confirm_prefab, GameObject confirm_parent, string userMessage = "", bool includeSaveFile = true, string[] extraCategories = null, string[] extraFiles = null)
	{
		if (KPrivacyPrefs.instance.disableDataCollection)
		{
			return;
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		global::Debug.Log("Reporting error.\n");
		if (msg != null)
		{
			global::Debug.Log(msg);
		}
		if (stack_trace != null)
		{
			global::Debug.Log(stack_trace);
		}
		KCrashReporter.hasReportedError = true;
		if (string.IsNullOrEmpty(msg))
		{
			msg = "No message";
		}
		Match match = KCrashReporter.failedToLoadModuleRegEx.Match(msg);
		if (match.Success)
		{
			string path = match.Groups[1].ToString();
			string text = match.Groups[2].ToString();
			string fileName = Path.GetFileName(path);
			msg = string.Concat(new string[]
			{
				"Failed to load '",
				fileName,
				"' with error '",
				text,
				"'."
			});
		}
		if (string.IsNullOrEmpty(stack_trace))
		{
			string buildText = BuildWatermark.GetBuildText();
			stack_trace = string.Format("No stack trace {0}\n\n{1}", buildText, msg);
		}
		List<string> list = new List<string>();
		if (KCrashReporter.debugWasUsed)
		{
			list.Add("(Debug Used)");
		}
		if (KCrashReporter.haveActiveMods)
		{
			list.Add("(Mods Active)");
		}
		list.Add(msg);
		string[] array = new string[]
		{
			"Debug:LogError",
			"UnityEngine.Debug",
			"Output:LogError",
			"DebugUtil:Assert",
			"System.Array",
			"System.Collections",
			"KCrashReporter.Assert",
			"No stack trace."
		};
		foreach (string text2 in stack_trace.Split('\n', StringSplitOptions.None))
		{
			if (list.Count >= 5)
			{
				break;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				bool flag = false;
				foreach (string value in array)
				{
					if (text2.StartsWith(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(text2);
				}
			}
		}
		if (userMessage == UI.CRASHSCREEN.BODY.text || userMessage.IsNullOrWhiteSpace())
		{
			userMessage = "";
		}
		else
		{
			userMessage = "[" + BuildWatermark.GetBuildText() + "] " + userMessage;
		}
		userMessage = userMessage.Replace(stack_trace, "");
		KCrashReporter.Error error = new KCrashReporter.Error();
		if (extraCategories != null)
		{
			error.categories.AddRange(extraCategories);
		}
		error.callstack = stack_trace;
		if (KCrashReporter.disableDeduping)
		{
			error.callstack = error.callstack + "\n" + Guid.NewGuid().ToString();
		}
		error.fullstack = string.Format("{0}\n\n{1}", msg, stack_trace);
		error.summaryline = string.Join("\n", list.ToArray());
		error.userMessage = userMessage;
		List<string> list2 = new List<string>();
		if (includeSaveFile && KCrashReporter.MOST_RECENT_SAVEFILE != null)
		{
			list2.Add(KCrashReporter.MOST_RECENT_SAVEFILE);
			error.saveFilename = Path.GetFileName(KCrashReporter.MOST_RECENT_SAVEFILE);
		}
		if (extraFiles != null)
		{
			foreach (string text3 in extraFiles)
			{
				list2.Add(text3);
				error.extraFilenames.Add(Path.GetFileName(text3));
			}
		}
		string jsonString = JsonConvert.SerializeObject(error);
		byte[] archiveData = KCrashReporter.CreateArchiveZip(KCrashReporter.GetLogContents(), list2);
		System.Action successCallback = delegate()
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(UI.CRASHSCREEN.REPORTEDERROR_SUCCESS, null, null, null, null, null, null, null, null);
			}
		};
		Action<long> failureCallback = delegate(long errorCode)
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				string text4 = (errorCode == 413L) ? UI.CRASHSCREEN.REPORTEDERROR_FAILURE_TOO_LARGE : UI.CRASHSCREEN.REPORTEDERROR_FAILURE;
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(text4, null, null, null, null, null, null, null, null);
			}
		};
		KCrashReporter.pendingCrash = new KCrashReporter.PendingCrash
		{
			jsonString = jsonString,
			archiveData = archiveData,
			successCallback = successCallback,
			failureCallback = failureCallback
		};
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x001A7A3B File Offset: 0x001A5C3B
	private static IEnumerator SubmitCrashAsync(string jsonString, byte[] archiveData, System.Action successCallback, Action<long> failureCallback)
	{
		bool success = false;
		Uri uri = new Uri("https://games-feedback.klei.com/submit");
		List<IMultipartFormSection> list = new List<IMultipartFormSection>
		{
			new MultipartFormDataSection("metadata", jsonString),
			new MultipartFormFileSection("archiveFile", archiveData, "Archive.zip", "application/octet-stream")
		};
		if (KleiAccount.KleiToken != null)
		{
			list.Add(new MultipartFormDataSection("loginToken", KleiAccount.KleiToken));
		}
		using (UnityWebRequest w = UnityWebRequest.Post(uri, list))
		{
			w.SendWebRequest();
			while (!w.isDone)
			{
				yield return null;
				if (KCrashReporter.onCrashUploadProgress != null)
				{
					KCrashReporter.onCrashUploadProgress(w.uploadProgress);
				}
			}
			if (w.result == UnityWebRequest.Result.Success)
			{
				UnityEngine.Debug.Log("Submitted crash!");
				if (successCallback != null)
				{
					successCallback();
				}
				success = true;
			}
			else
			{
				UnityEngine.Debug.Log("CrashReporter: Could not submit crash " + w.result.ToString());
				if (failureCallback != null)
				{
					failureCallback(w.responseCode);
				}
			}
		}
		UnityWebRequest w = null;
		if (KCrashReporter.onCrashReported != null)
		{
			KCrashReporter.onCrashReported(success);
		}
		yield break;
		yield break;
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x001A7A60 File Offset: 0x001A5C60
	public static void ReportBug(string msg, GameObject confirmParent)
	{
		string stack_trace = "Bug Report From: " + KCrashReporter.GetUserID() + " at " + System.DateTime.Now.ToString();
		KCrashReporter.ReportError(msg, stack_trace, ScreenPrefabs.Instance.ConfirmDialogScreen, confirmParent, "", true, null, null);
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x001A7AAC File Offset: 0x001A5CAC
	public static void Assert(bool condition, string message, string[] extraCategories = null)
	{
		if (!condition && !KCrashReporter.hasReportedError)
		{
			StackTrace stackTrace = new StackTrace(1, true);
			KCrashReporter.ReportError("ASSERT: " + message, stackTrace.ToString(), null, null, null, true, extraCategories, null);
		}
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x001A7AE7 File Offset: 0x001A5CE7
	public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
	{
		if (KCrashReporter.hasReportedError)
		{
			return;
		}
		KCrashReporter.pendingReport = new KCrashReporter.PendingReport(msg, stack_trace, dmp_filename);
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x001A7B00 File Offset: 0x001A5D00
	private static byte[] CreateArchiveZip(string log, List<string> files)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				if (files != null)
				{
					foreach (string text in files)
					{
						try
						{
							if (!File.Exists(text))
							{
								UnityEngine.Debug.Log("CrashReporter: file does not exist to include: " + text);
							}
							else
							{
								using (Stream stream = zipArchive.CreateEntry(Path.GetFileName(text), System.IO.Compression.CompressionLevel.Fastest).Open())
								{
									byte[] array = File.ReadAllBytes(text);
									stream.Write(array, 0, array.Length);
								}
							}
						}
						catch (Exception ex)
						{
							string str = "CrashReporter: Could not add file '";
							string str2 = text;
							string str3 = "' to archive: ";
							Exception ex2 = ex;
							UnityEngine.Debug.Log(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
					using (Stream stream2 = zipArchive.CreateEntry("Player.log", System.IO.Compression.CompressionLevel.Fastest).Open())
					{
						byte[] bytes = Encoding.UTF8.GetBytes(log);
						stream2.Write(bytes, 0, bytes.Length);
					}
				}
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x001A7CC0 File Offset: 0x001A5EC0
	private void Update()
	{
		if (KCrashReporter.pendingReport != null)
		{
			KCrashReporter.PendingReport pendingReport = KCrashReporter.pendingReport;
			KCrashReporter.pendingReport = null;
			if (KCrashReporter.hasReportedError)
			{
				return;
			}
			KCrashReporter component = Global.Instance.GetComponent<KCrashReporter>();
			if (component != null)
			{
				component.ShowDialog(pendingReport.message, pendingReport.stack_trace, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.SIM
				}, new string[]
				{
					pendingReport.additional_filename
				});
			}
			else
			{
				KCrashReporter.ReportError(pendingReport.message, pendingReport.stack_trace, null, null, "", true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.SIM
				}, new string[]
				{
					pendingReport.additional_filename
				});
			}
		}
		if (KCrashReporter.pendingCrash != null)
		{
			KCrashReporter.PendingCrash pendingCrash = KCrashReporter.pendingCrash;
			KCrashReporter.pendingCrash = null;
			global::Debug.Log("Submitting crash...");
			base.StartCoroutine(KCrashReporter.SubmitCrashAsync(pendingCrash.jsonString, pendingCrash.archiveData, pendingCrash.successCallback, pendingCrash.failureCallback));
		}
	}

	// Token: 0x04003097 RID: 12439
	public static string MOST_RECENT_SAVEFILE = null;

	// Token: 0x04003098 RID: 12440
	public const string CRASH_REPORTER_SERVER = "https://games-feedback.klei.com";

	// Token: 0x04003099 RID: 12441
	public const uint MAX_LOGS = 10000000U;

	// Token: 0x0400309C RID: 12444
	public static bool ignoreAll = false;

	// Token: 0x0400309D RID: 12445
	public static bool debugWasUsed = false;

	// Token: 0x0400309E RID: 12446
	public static bool haveActiveMods = false;

	// Token: 0x0400309F RID: 12447
	public static uint logCount = 0U;

	// Token: 0x040030A0 RID: 12448
	public static string error_canvas_name = "ErrorCanvas";

	// Token: 0x040030A1 RID: 12449
	public static bool disableDeduping = false;

	// Token: 0x040030A3 RID: 12451
	public static bool hasCrash = false;

	// Token: 0x040030A4 RID: 12452
	private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);

	// Token: 0x040030A5 RID: 12453
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x040030A6 RID: 12454
	[SerializeField]
	private GameObject reportErrorPrefab;

	// Token: 0x040030A7 RID: 12455
	[SerializeField]
	private ConfirmDialogScreen confirmDialogPrefab;

	// Token: 0x040030A8 RID: 12456
	private GameObject errorScreen;

	// Token: 0x040030A9 RID: 12457
	public static bool terminateOnError = true;

	// Token: 0x040030AA RID: 12458
	private static string dataRoot;

	// Token: 0x040030AB RID: 12459
	private static readonly string[] IgnoreStrings = new string[]
	{
		"Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!",
		"The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame",
		"Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.",
		"Texture has out of range width / height",
		"<I> Failed to get cursor position:\r\nSuccess.\r\n"
	};

	// Token: 0x040030AC RID: 12460
	private static HashSet<int> previouslyReportedDevNotifications;

	// Token: 0x040030AD RID: 12461
	private static KCrashReporter.PendingReport pendingReport;

	// Token: 0x040030AE RID: 12462
	private static KCrashReporter.PendingCrash pendingCrash;

	// Token: 0x02001A33 RID: 6707
	public class CRASH_CATEGORY
	{
		// Token: 0x040080B6 RID: 32950
		public static string DEVNOTIFICATION = "DevNotification";

		// Token: 0x040080B7 RID: 32951
		public static string VANILLA = "Vanilla";

		// Token: 0x040080B8 RID: 32952
		public static string SPACEDOUT = "SpacedOut";

		// Token: 0x040080B9 RID: 32953
		public static string MODDED = "Modded";

		// Token: 0x040080BA RID: 32954
		public static string DEBUGUSED = "DebugUsed";

		// Token: 0x040080BB RID: 32955
		public static string SANDBOX = "Sandbox";

		// Token: 0x040080BC RID: 32956
		public static string STEAMDECK = "SteamDeck";

		// Token: 0x040080BD RID: 32957
		public static string SIM = "SimDll";

		// Token: 0x040080BE RID: 32958
		public static string FILEIO = "FileIO";

		// Token: 0x040080BF RID: 32959
		public static string MODSYSTEM = "ModSystem";

		// Token: 0x040080C0 RID: 32960
		public static string WORLDGENFAILURE = "WorldgenFailure";
	}

	// Token: 0x02001A34 RID: 6708
	private class Error
	{
		// Token: 0x0600A488 RID: 42120 RVA: 0x003B4AEC File Offset: 0x003B2CEC
		public Error()
		{
			this.userName = KCrashReporter.GetUserID();
			this.platform = Util.GetOperatingSystem();
			this.InitDefaultCategories();
			this.InitSku();
			this.InitSlackSummary();
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool flag = !SteamApps.GetCurrentBetaName(out a, 100);
				this.branch = a;
				if (a == "public_playtest")
				{
					this.branch = "public_testing";
				}
				if (flag || (a == "public_testing" && !UnityEngine.Debug.isDebugBuild))
				{
					this.branch = "default";
				}
			}
		}

		// Token: 0x0600A489 RID: 42121 RVA: 0x003B4C38 File Offset: 0x003B2E38
		private void InitDefaultCategories()
		{
			if (DlcManager.IsPureVanilla())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.VANILLA);
			}
			if (DlcManager.IsExpansion1Active())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.SPACEDOUT);
			}
			foreach (string text in DlcManager.GetActiveDLCIds())
			{
				if (!(text == "EXPANSION1_ID"))
				{
					this.categories.Add(text);
				}
			}
			if (KCrashReporter.debugWasUsed)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.DEBUGUSED);
			}
			if (KCrashReporter.haveActiveMods)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.MODDED);
			}
			if (SaveGame.Instance != null && SaveGame.Instance.sandboxEnabled)
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.SANDBOX);
			}
			if (DistributionPlatform.Inst.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.categories.Add(KCrashReporter.CRASH_CATEGORY.STEAMDECK);
			}
		}

		// Token: 0x0600A48A RID: 42122 RVA: 0x003B4D44 File Offset: 0x003B2F44
		private void InitSku()
		{
			this.sku = "steam";
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool flag = !SteamApps.GetCurrentBetaName(out a, 100);
				if (a == "public_testing" || a == "preview" || a == "public_playtest" || a == "playtest")
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						this.sku = "steam-public-testing";
					}
					else
					{
						this.sku = "steam-release";
					}
				}
				if (flag || a == "release")
				{
					this.sku = "steam-release";
				}
			}
		}

		// Token: 0x0600A48B RID: 42123 RVA: 0x003B4DE4 File Offset: 0x003B2FE4
		private void InitSlackSummary()
		{
			string buildText = BuildWatermark.GetBuildText();
			string text = (GameClock.Instance != null) ? string.Format(" - Cycle {0}", GameClock.Instance.GetCycle() + 1) : "";
			int num;
			if (!(Global.Instance != null) || Global.Instance.modManager == null)
			{
				num = 0;
			}
			else
			{
				num = Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc());
			}
			int num2 = num;
			string text2 = (num2 > 0) ? string.Format(" - {0} active mods", num2) : "";
			this.slackSummary = string.Concat(new string[]
			{
				buildText,
				" ",
				this.platform,
				text,
				text2
			});
		}

		// Token: 0x040080C1 RID: 32961
		public string game = "ONI";

		// Token: 0x040080C2 RID: 32962
		public string userName;

		// Token: 0x040080C3 RID: 32963
		public string platform;

		// Token: 0x040080C4 RID: 32964
		public string version = LaunchInitializer.BuildPrefix();

		// Token: 0x040080C5 RID: 32965
		public string branch = "default";

		// Token: 0x040080C6 RID: 32966
		public string sku = "";

		// Token: 0x040080C7 RID: 32967
		public int build = 706793;

		// Token: 0x040080C8 RID: 32968
		public string callstack = "";

		// Token: 0x040080C9 RID: 32969
		public string fullstack = "";

		// Token: 0x040080CA RID: 32970
		public string summaryline = "";

		// Token: 0x040080CB RID: 32971
		public string userMessage = "";

		// Token: 0x040080CC RID: 32972
		public List<string> categories = new List<string>();

		// Token: 0x040080CD RID: 32973
		public string slackSummary;

		// Token: 0x040080CE RID: 32974
		public string logFilename = "Player.log";

		// Token: 0x040080CF RID: 32975
		public string saveFilename = "";

		// Token: 0x040080D0 RID: 32976
		public string screenshotFilename = "";

		// Token: 0x040080D1 RID: 32977
		public List<string> extraFilenames = new List<string>();

		// Token: 0x040080D2 RID: 32978
		public string title = "";

		// Token: 0x040080D3 RID: 32979
		public bool isServer;

		// Token: 0x040080D4 RID: 32980
		public bool isDedicated;

		// Token: 0x040080D5 RID: 32981
		public bool isError = true;

		// Token: 0x040080D6 RID: 32982
		public string emote = "";
	}

	// Token: 0x02001A35 RID: 6709
	public class PendingCrash
	{
		// Token: 0x040080D7 RID: 32983
		public string jsonString;

		// Token: 0x040080D8 RID: 32984
		public byte[] archiveData;

		// Token: 0x040080D9 RID: 32985
		public System.Action successCallback;

		// Token: 0x040080DA RID: 32986
		public Action<long> failureCallback;
	}

	// Token: 0x02001A36 RID: 6710
	public class PendingReport
	{
		// Token: 0x0600A48D RID: 42125 RVA: 0x003B4EC9 File Offset: 0x003B30C9
		public PendingReport(string msg, string stack_trace, string filename)
		{
			this.message = msg;
			this.stack_trace = stack_trace;
			this.additional_filename = filename;
		}

		// Token: 0x040080DB RID: 32987
		public string message;

		// Token: 0x040080DC RID: 32988
		public string stack_trace;

		// Token: 0x040080DD RID: 32989
		public string additional_filename;
	}
}
