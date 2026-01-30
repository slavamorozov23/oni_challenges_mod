using System;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x0200095E RID: 2398
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
	// Token: 0x06004350 RID: 17232 RVA: 0x0017DABD File Offset: 0x0017BCBD
	public static void DestroyInstance()
	{
		GameClock.Instance = null;
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x0017DAC5 File Offset: 0x0017BCC5
	protected override void OnPrefabInit()
	{
		GameClock.Instance = this;
		this.timeSinceStartOfCycle = 50f;
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0017DAD8 File Offset: 0x0017BCD8
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.time != 0f)
		{
			this.cycle = (int)(this.time / 600f);
			this.timeSinceStartOfCycle = Mathf.Max(this.time - (float)this.cycle * 600f, 0f);
			this.time = 0f;
		}
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x0017DB34 File Offset: 0x0017BD34
	public void Sim33ms(float dt)
	{
		this.AddTime(dt);
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x0017DB3D File Offset: 0x0017BD3D
	public void Render1000ms(float dt)
	{
		this.timePlayed += dt;
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x0017DB4D File Offset: 0x0017BD4D
	private void LateUpdate()
	{
		this.frame++;
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x0017DB60 File Offset: 0x0017BD60
	private void AddTime(float dt)
	{
		this.timeSinceStartOfCycle += dt;
		bool flag = false;
		while (this.timeSinceStartOfCycle >= 600f)
		{
			this.cycle++;
			this.timeSinceStartOfCycle -= 600f;
			base.Trigger(631075836, null);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.Trigger(631075836, null);
			}
			flag = true;
		}
		if (!this.isNight && this.IsNighttime())
		{
			this.isNight = true;
			base.Trigger(-722330267, null);
		}
		if (this.isNight && !this.IsNighttime())
		{
			this.isNight = false;
		}
		if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
		{
			this.DoAutoSave(this.cycle);
		}
		int num = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
		int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
		if (num != num2)
		{
			GameClock.GameClockBlockEventData gameClockBlockEventData;
			using (GameClock.GameClockBlockEventData.Pool.Get(out gameClockBlockEventData))
			{
				gameClockBlockEventData.block = num2;
				base.Trigger(-1215042067, gameClockBlockEventData);
			}
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x0017DCD8 File Offset: 0x0017BED8
	public float GetTimeSinceStartOfReport()
	{
		if (this.IsNighttime())
		{
			return 525f - this.GetTimeSinceStartOfCycle();
		}
		return this.GetTimeSinceStartOfCycle() + 75f;
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x0017DCFB File Offset: 0x0017BEFB
	public float GetTimeSinceStartOfCycle()
	{
		return this.timeSinceStartOfCycle;
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x0017DD03 File Offset: 0x0017BF03
	public float GetCurrentCycleAsPercentage()
	{
		return this.timeSinceStartOfCycle / 600f;
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x0017DD11 File Offset: 0x0017BF11
	public float GetTime()
	{
		return this.timeSinceStartOfCycle + (float)this.cycle * 600f;
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x0017DD27 File Offset: 0x0017BF27
	public float GetTimeInCycles()
	{
		return (float)this.cycle + this.GetCurrentCycleAsPercentage();
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x0017DD37 File Offset: 0x0017BF37
	public int GetFrame()
	{
		return this.frame;
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x0017DD3F File Offset: 0x0017BF3F
	public int GetCycle()
	{
		return this.cycle;
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x0017DD47 File Offset: 0x0017BF47
	public bool IsNighttime()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875f;
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x0017DD5D File Offset: 0x0017BF5D
	public float GetDaytimeDurationInPercentage()
	{
		return 0.875f;
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x0017DD64 File Offset: 0x0017BF64
	public void SetTime(float new_time)
	{
		float dt = Mathf.Max(new_time - this.GetTime(), 0f);
		this.AddTime(dt);
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x0017DD8B File Offset: 0x0017BF8B
	public float GetTimePlayedInSeconds()
	{
		return this.timePlayed;
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x0017DD94 File Offset: 0x0017BF94
	private void DoAutoSave(int day)
	{
		if (GenericGameSettings.instance.disableAutosave)
		{
			return;
		}
		day++;
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, day);
		OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "DoAutoSave");
		string text = SaveLoader.GetActiveSaveFilePath();
		if (text == null)
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		int num = text.LastIndexOf("\\");
		if (num > 0)
		{
			int num2 = text.IndexOf(" Cycle ", num);
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
		}
		text = Path.ChangeExtension(text, null);
		text = text + " Cycle " + day.ToString();
		text = SaveScreen.GetValidSaveFilename(text);
		text = Path.Combine(SaveLoader.GetActiveAutoSavePath(), Path.GetFileName(text));
		string text2 = text;
		int num3 = 1;
		while (File.Exists(text))
		{
			text = text2.Replace(".sav", "");
			text = SaveScreen.GetValidSaveFilename(text2 + " (" + num3.ToString() + ")");
			num3++;
		}
		Game.Instance.StartDelayedSave(text, true, false);
	}

	// Token: 0x04002A9B RID: 10907
	public static GameClock Instance;

	// Token: 0x04002A9C RID: 10908
	[Serialize]
	private int frame;

	// Token: 0x04002A9D RID: 10909
	[Serialize]
	private float time;

	// Token: 0x04002A9E RID: 10910
	[Serialize]
	private float timeSinceStartOfCycle;

	// Token: 0x04002A9F RID: 10911
	[Serialize]
	private int cycle;

	// Token: 0x04002AA0 RID: 10912
	[Serialize]
	private float timePlayed;

	// Token: 0x04002AA1 RID: 10913
	[Serialize]
	private bool isNight;

	// Token: 0x04002AA2 RID: 10914
	public static readonly string NewCycleKey = "NewCycle";

	// Token: 0x02001967 RID: 6503
	private class GameClockBlockEventData
	{
		// Token: 0x04007DD1 RID: 32209
		public int block = -1;

		// Token: 0x04007DD2 RID: 32210
		public static ObjectPool<GameClock.GameClockBlockEventData> Pool = new ObjectPool<GameClock.GameClockBlockEventData>(() => new GameClock.GameClockBlockEventData(), null, null, null, false, 4, 4);
	}
}
