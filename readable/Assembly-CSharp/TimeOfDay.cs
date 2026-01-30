using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06005184 RID: 20868 RVA: 0x001D929C File Offset: 0x001D749C
	public static bool IsMilestoneApproaching
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 2 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle + 1);
			}
			return false;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06005185 RID: 20869 RVA: 0x001D92F4 File Offset: 0x001D74F4
	public static bool IsMilestoneDay
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 1 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle);
			}
			return false;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06005187 RID: 20871 RVA: 0x001D9352 File Offset: 0x001D7552
	// (set) Token: 0x06005186 RID: 20870 RVA: 0x001D9349 File Offset: 0x001D7549
	public TimeOfDay.TimeRegion timeRegion { get; private set; }

	// Token: 0x06005188 RID: 20872 RVA: 0x001D935A File Offset: 0x001D755A
	public static void DestroyInstance()
	{
		TimeOfDay.Instance = null;
	}

	// Token: 0x06005189 RID: 20873 RVA: 0x001D9362 File Offset: 0x001D7562
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		TimeOfDay.Instance = this;
	}

	// Token: 0x0600518A RID: 20874 RVA: 0x001D9370 File Offset: 0x001D7570
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		TimeOfDay.Instance = null;
	}

	// Token: 0x0600518B RID: 20875 RVA: 0x001D9380 File Offset: 0x001D7580
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeRegion = this.GetCurrentTimeRegion();
		string clusterId = SaveLoader.Instance.GameInfo.clusterId;
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay))
		{
			this.stingerDay = clusterData.clusterAudio.stingerDay;
		}
		else
		{
			this.stingerDay = "Stinger_Day";
		}
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight))
		{
			this.stingerNight = clusterData.clusterAudio.stingerNight;
		}
		else
		{
			this.stingerNight = "Stinger_Loop_Night";
		}
		if (!MusicManager.instance.SongIsPlaying(this.stingerNight) && this.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
		{
			MusicManager.instance.PlaySong(this.stingerNight, false);
			MusicManager.instance.SetSongParameter(this.stingerNight, "Music_PlayStinger", 0f, true);
		}
		this.UpdateSunlightIntensity();
	}

	// Token: 0x0600518C RID: 20876 RVA: 0x001D946F File Offset: 0x001D766F
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.UpdateVisuals();
	}

	// Token: 0x0600518D RID: 20877 RVA: 0x001D9477 File Offset: 0x001D7677
	public TimeOfDay.TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeOfDay.TimeRegion.Night;
		}
		return TimeOfDay.TimeRegion.Day;
	}

	// Token: 0x0600518E RID: 20878 RVA: 0x001D9488 File Offset: 0x001D7688
	private void Update()
	{
		this.UpdateVisuals();
		TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
		Boxed<int> boxed = Boxed<int>.Get(GameClock.Instance.GetCycle());
		if (currentTimeRegion != this.timeRegion)
		{
			if (TimeOfDay.IsMilestoneApproaching)
			{
				Game.Instance.Trigger(-720092972, boxed);
			}
			if (TimeOfDay.IsMilestoneDay)
			{
				Game.Instance.Trigger(2070437606, boxed);
			}
			this.TriggerSoundChange(currentTimeRegion, TimeOfDay.IsMilestoneDay);
			this.timeRegion = currentTimeRegion;
			base.Trigger(1791086652, null);
		}
		Boxed<int>.Release(boxed);
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x001D9510 File Offset: 0x001D7710
	private void UpdateVisuals()
	{
		float num = 0.875f;
		float num2 = 0.2f;
		float num3 = 1f;
		float b = 0f;
		if (GameClock.Instance.GetCurrentCycleAsPercentage() >= num)
		{
			b = num3;
		}
		this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
		float y = this.UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, y, 0f, 0f));
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x001D9586 File Offset: 0x001D7786
	public void Sim4000ms(float dt)
	{
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06005191 RID: 20881 RVA: 0x001D958F File Offset: 0x001D778F
	public void SetEclipse(bool eclipse)
	{
		this.isEclipse = eclipse;
	}

	// Token: 0x06005192 RID: 20882 RVA: 0x001D9598 File Offset: 0x001D7798
	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || this.isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * 3.1415927f);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = (float)worldContainer.cosmicRadiation;
		}
		return num2;
	}

	// Token: 0x06005193 RID: 20883 RVA: 0x001D9658 File Offset: 0x001D7858
	private void TriggerSoundChange(TimeOfDay.TimeRegion new_region, bool milestoneReached)
	{
		if (new_region == TimeOfDay.TimeRegion.Day)
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying(this.stingerNight))
			{
				MusicManager.instance.StopSong(this.stingerNight, true, STOP_MODE.ALLOWFADEOUT);
			}
			if (milestoneReached)
			{
				MusicManager.instance.PlaySong("Stinger_Day_Celebrate", false);
			}
			else
			{
				MusicManager.instance.PlaySong(this.stingerDay, false);
			}
			MusicManager.instance.PlayDynamicMusic();
			return;
		}
		if (new_region != TimeOfDay.TimeRegion.Night)
		{
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
		MusicManager.instance.PlaySong(this.stingerNight, false);
	}

	// Token: 0x06005194 RID: 20884 RVA: 0x001D96FF File Offset: 0x001D78FF
	public void SetScale(float new_scale)
	{
		this.scale = new_scale;
	}

	// Token: 0x04003728 RID: 14120
	private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";

	// Token: 0x04003729 RID: 14121
	public static List<int> MILESTONE_CYCLES = new List<int>(2)
	{
		99,
		999
	};

	// Token: 0x0400372A RID: 14122
	[Serialize]
	private float scale;

	// Token: 0x0400372C RID: 14124
	private EventInstance nightLPEvent;

	// Token: 0x0400372D RID: 14125
	public static TimeOfDay Instance;

	// Token: 0x0400372E RID: 14126
	public string stingerDay;

	// Token: 0x0400372F RID: 14127
	public string stingerNight;

	// Token: 0x04003730 RID: 14128
	private bool isEclipse;

	// Token: 0x02001C2F RID: 7215
	public enum TimeRegion
	{
		// Token: 0x04008724 RID: 34596
		Invalid,
		// Token: 0x04008725 RID: 34597
		Day,
		// Token: 0x04008726 RID: 34598
		Night
	}
}
