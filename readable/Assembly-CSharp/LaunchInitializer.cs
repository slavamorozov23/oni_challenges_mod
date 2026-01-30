using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x020009DB RID: 2523
public class LaunchInitializer : MonoBehaviour
{
	// Token: 0x06004965 RID: 18789 RVA: 0x001A90B1 File Offset: 0x001A72B1
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x001A90B8 File Offset: 0x001A72B8
	public static int UpdateNumber()
	{
		return 57;
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x001A90BC File Offset: 0x001A72BC
	private void Update()
	{
		if (this.numWaitFrames > Time.renderedFrameCount)
		{
			return;
		}
		PerformanceCaptureMonitor.Initialize();
		if (!DistributionPlatform.Initialized)
		{
			if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				global::Debug.LogError("Machine does not support RGBAFloat32");
			}
			GraphicsOptionsScreen.SetSettingsFromPrefs();
			Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
			global::Debug.Log("Date: " + System.DateTime.Now.ToString());
			global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			KPlayerPrefs.instance.Load();
			DistributionPlatform.Initialize();
		}
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		global::Debug.Log("DistributionPlatform initialized.");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		DebugUtil.LogArgs(new object[]
		{
			"DLC Information"
		});
		foreach (string text in DlcManager.GetOwnedDLCIds())
		{
			global::Debug.Log(string.Format("- {0} loaded: {1}", text, DlcManager.IsContentSubscribed(text)));
		}
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		KFMOD.Initialize();
		for (int i = 0; i < this.SpawnPrefabs.Length; i++)
		{
			GameObject gameObject = this.SpawnPrefabs[i];
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
		LaunchInitializer.DeleteLingeringFiles();
		base.enabled = false;
	}

	// Token: 0x06004968 RID: 18792 RVA: 0x001A9270 File Offset: 0x001A7470
	private static void DeleteLingeringFiles()
	{
		string[] array = new string[]
		{
			"fmod.log",
			"load_stats_0.json",
			"OxygenNotIncluded_Data/output_log.txt"
		};
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		foreach (string path in array)
		{
			string path2 = Path.Combine(directoryName, path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
			}
		}
	}

	// Token: 0x040030DE RID: 12510
	private const string PREFIX = "U";

	// Token: 0x040030DF RID: 12511
	private const int UPDATE_NUMBER = 57;

	// Token: 0x040030E0 RID: 12512
	private static readonly string BUILD_PREFIX = "U" + 57.ToString();

	// Token: 0x040030E1 RID: 12513
	public GameObject[] SpawnPrefabs;

	// Token: 0x040030E2 RID: 12514
	[SerializeField]
	private int numWaitFrames = 1;
}
