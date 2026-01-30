using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Klei;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class PerformanceCaptureMonitor
{
	// Token: 0x060017C6 RID: 6086 RVA: 0x00086A44 File Offset: 0x00084C44
	public static void WritePerformanceCaptureData()
	{
		PerformanceCaptureMonitor.Data.SWAverageFrameTimeMs = (float)(PerformanceCaptureMonitor.captureTimer.Elapsed.TotalMilliseconds / (double)GenericGameSettings.instance.scriptedProfile.frameCount);
		PerformanceCaptureMonitor.Data.Revision = 706793U;
		PerformanceCaptureMonitor.Data.Branch = "release";
		PerformanceCaptureMonitor.Data.IsBaseGame = !DlcManager.IsExpansion1Active();
		PerformanceCaptureMonitor.Data.LoadedDlcs = DlcManager.GetActiveDLCIds();
		if (SaveLoader.Instance != null)
		{
			PerformanceCaptureMonitor.Data.ActiveDlcsInSave = SaveLoader.Instance.GameInfo.dlcIds.ToArray();
			PerformanceCaptureMonitor.Data.PerfMonAverageFrameTimeMs = SaveLoader.Instance.GetFrameTime() * 1000f;
		}
		if (Game.Instance != null)
		{
			PerformanceCaptureMonitor.Data.Cycle = GameUtil.GetCurrentCycle();
			PerformanceCaptureMonitor.Data.Brains = new List<PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo>();
			foreach (BrainScheduler.BrainGroup brainGroup in Game.BrainScheduler.debugGetBrainGroups())
			{
				PerformanceCaptureMonitor.Data.Brains.Add(new PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo
				{
					name = brainGroup.tag.ToString(),
					count = brainGroup.BrainCount
				});
			}
		}
		PerformanceCaptureMonitor.Data.Patch = "";
		PerformanceCaptureMonitor.Data.BuildTags = new List<string>();
		PerformanceCaptureMonitor.Data.BuildTags.Add("release");
		PerformanceCaptureMonitor.Data.BuildConfig = string.Join("_", PerformanceCaptureMonitor.Data.BuildTags);
		if (!PerformanceCaptureMonitor.Data.Patch.IsNullOrWhiteSpace())
		{
			PerformanceCaptureMonitor.Data.BuildTags.Add("PatchedBuild");
		}
		if (SpeedControlScreen.Instance != null)
		{
			PerformanceCaptureMonitor.Data.GameSpeed = SpeedControlScreen.Instance.GetSpeed() + 1;
		}
		PerformanceCaptureMonitor.Data.EndMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
		string contents = JsonUtility.ToJson(PerformanceCaptureMonitor.Data);
		File.WriteAllText("PerformanceCaptureData.json", contents);
		DebugUtil.LogArgs(new object[]
		{
			"Written PerformanceCaptureData.json"
		});
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x00086C80 File Offset: 0x00084E80
	public static bool IsCapturingPerformance()
	{
		return !GenericGameSettings.instance.scriptedProfile.saveGame.IsNullOrWhiteSpace();
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x00086C9C File Offset: 0x00084E9C
	public static void Initialize()
	{
		if (PerformanceCaptureMonitor.IsCapturingPerformance())
		{
			Application.targetFrameRate = -1;
			QualitySettings.vSyncCount = 0;
			KProfilerBegin.OnStopCapture = (System.Action)Delegate.Combine(KProfilerBegin.OnStopCapture, new System.Action(PerformanceCaptureMonitor.WritePerformanceCaptureData));
			KProfilerBegin.OnStartCapture = (System.Action)Delegate.Combine(KProfilerBegin.OnStartCapture, new System.Action(PerformanceCaptureMonitor.StartCapture));
			PerformanceCaptureMonitor.systemUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory", 1, ProfilerRecorderOptions.Default);
		}
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x00086D13 File Offset: 0x00084F13
	public static void StartCapture()
	{
		PerformanceCaptureMonitor.captureTimer.Restart();
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x00086D1F File Offset: 0x00084F1F
	public static void StartLoadingSave()
	{
		PerformanceCaptureMonitor.loadTimer.Restart();
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x00086D2B File Offset: 0x00084F2B
	public static IEnumerator FinishedLoadingSave()
	{
		yield return null;
		PerformanceCaptureMonitor.Data.SaveLoadMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
		PerformanceCaptureMonitor.loadTimer.Stop();
		PerformanceCaptureMonitor.Data.SaveLoadTimeSec = (float)PerformanceCaptureMonitor.loadTimer.Elapsed.TotalSeconds;
		yield break;
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x00086D33 File Offset: 0x00084F33
	public static void TryRecordMainMenuStats()
	{
		if (PerformanceCaptureMonitor.Data.MainMenuMemoryMegs == 0f)
		{
			PerformanceCaptureMonitor.Data.MainMenuMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
			PerformanceCaptureMonitor.Data.MainMenuLoadTimeSec = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x00086D64 File Offset: 0x00084F64
	public static float GetMemoryUsed()
	{
		if (!PerformanceCaptureMonitor.systemUsedMemory.Valid)
		{
			return 0f;
		}
		return (float)PerformanceCaptureMonitor.systemUsedMemory.CurrentValue / 1048576f;
	}

	// Token: 0x04000DF0 RID: 3568
	public static PerformanceCaptureMonitor.PerformanceCaptureData Data = new PerformanceCaptureMonitor.PerformanceCaptureData();

	// Token: 0x04000DF1 RID: 3569
	private static ProfilerRecorder systemUsedMemory;

	// Token: 0x04000DF2 RID: 3570
	private static Stopwatch loadTimer = new Stopwatch();

	// Token: 0x04000DF3 RID: 3571
	private static Stopwatch captureTimer = new Stopwatch();

	// Token: 0x02001280 RID: 4736
	public class PerformanceCaptureData
	{
		// Token: 0x0400680D RID: 26637
		public uint Revision;

		// Token: 0x0400680E RID: 26638
		public string Patch;

		// Token: 0x0400680F RID: 26639
		public string Branch;

		// Token: 0x04006810 RID: 26640
		public bool IsDevelopmentBuild;

		// Token: 0x04006811 RID: 26641
		public bool IsBaseGame;

		// Token: 0x04006812 RID: 26642
		public string[] ActiveDlcsInSave;

		// Token: 0x04006813 RID: 26643
		public List<string> LoadedDlcs;

		// Token: 0x04006814 RID: 26644
		public List<string> BuildTags;

		// Token: 0x04006815 RID: 26645
		public List<PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo> Brains;

		// Token: 0x04006816 RID: 26646
		public int Cycle;

		// Token: 0x04006817 RID: 26647
		public float MainMenuLoadTimeSec;

		// Token: 0x04006818 RID: 26648
		public float MainMenuMemoryMegs;

		// Token: 0x04006819 RID: 26649
		public float SaveLoadTimeSec;

		// Token: 0x0400681A RID: 26650
		public float SaveLoadMemoryMegs;

		// Token: 0x0400681B RID: 26651
		public float EndMemoryMegs;

		// Token: 0x0400681C RID: 26652
		public float PerfMonAverageFrameTimeMs;

		// Token: 0x0400681D RID: 26653
		public float SWAverageFrameTimeMs;

		// Token: 0x0400681E RID: 26654
		public string BuildConfig;

		// Token: 0x0400681F RID: 26655
		public int GameSpeed;

		// Token: 0x02002787 RID: 10119
		[Serializable]
		public class BrainInfo
		{
			// Token: 0x0400AF64 RID: 44900
			public string name;

			// Token: 0x0400AF65 RID: 44901
			public int count;
		}
	}
}
