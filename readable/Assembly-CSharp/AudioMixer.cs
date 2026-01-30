using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class AudioMixer
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x000A8569 File Offset: 0x000A6769
	public static AudioMixer instance
	{
		get
		{
			return AudioMixer._instance;
		}
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x000A8570 File Offset: 0x000A6770
	public static AudioMixer Create()
	{
		AudioMixer._instance = new AudioMixer();
		AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
		if (audioMixerSnapshots != null)
		{
			audioMixerSnapshots.ReloadSnapshots();
		}
		return AudioMixer._instance;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000A85A1 File Offset: 0x000A67A1
	public static void Destroy()
	{
		AudioMixer._instance.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AudioMixer._instance = null;
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000A85B4 File Offset: 0x000A67B4
	public EventInstance Start(EventReference event_ref)
	{
		string snapshot;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot);
		return this.Start(snapshot);
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000A85E0 File Offset: 0x000A67E0
	public EventInstance Start(string snapshot)
	{
		EventInstance eventInstance;
		if (!this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			if (RuntimeManager.IsInitialized)
			{
				eventInstance = KFMOD.CreateInstance(snapshot);
				this.activeSnapshots[snapshot] = eventInstance;
				eventInstance.start();
				eventInstance.setParameterByName("snapshotActive", 1f, false);
			}
			else
			{
				eventInstance = default(EventInstance);
			}
		}
		AudioMixer.instance.Log("Start Snapshot: " + snapshot);
		return eventInstance;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000A8660 File Offset: 0x000A6860
	public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.Stop(s, stop_mode);
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x000A8690 File Offset: 0x000A6890
	public bool Stop(HashedString snapshot, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		bool result = false;
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			eventInstance.setParameterByName("snapshotActive", 0f, false);
			eventInstance.stop(stop_mode);
			eventInstance.release();
			this.activeSnapshots.Remove(snapshot);
			result = true;
			AudioMixer instance = AudioMixer.instance;
			string[] array = new string[5];
			array[0] = "Stop Snapshot: [";
			int num = 1;
			HashedString hashedString = snapshot;
			array[num] = hashedString.ToString();
			array[2] = "] with fadeout mode: [";
			array[3] = stop_mode.ToString();
			array[4] = "]";
			instance.Log(string.Concat(array));
		}
		else
		{
			AudioMixer instance2 = AudioMixer.instance;
			string str = "Tried to stop snapshot: [";
			HashedString hashedString = snapshot;
			instance2.Log(str + hashedString.ToString() + "] but it wasn't active.");
		}
		return result;
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x000A875F File Offset: 0x000A695F
	public void Reset()
	{
		this.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x000A8768 File Offset: 0x000A6968
	public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, EventInstance> keyValuePair in this.activeSnapshots)
		{
			if (keyValuePair.Key != AudioMixer.UserVolumeSettingsHash)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Stop(list[i], stop_mode);
		}
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x000A87FC File Offset: 0x000A69FC
	public bool SnapshotIsActive(EventReference event_ref)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.SnapshotIsActive(s);
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000A882B File Offset: 0x000A6A2B
	public bool SnapshotIsActive(HashedString snapshot_name)
	{
		return this.activeSnapshots.ContainsKey(snapshot_name);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000A8840 File Offset: 0x000A6A40
	public void SetSnapshotParameter(EventReference event_ref, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		string snapshot_name;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot_name);
		this.SetSnapshotParameter(snapshot_name, parameter_name, parameter_value, shouldLog);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x000A8870 File Offset: 0x000A6A70
	public void SetSnapshotParameter(string snapshot_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", snapshot_name, parameter_name, parameter_value));
		}
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot_name, out eventInstance))
		{
			eventInstance.setParameterByName(parameter_name, parameter_value, false);
			return;
		}
		this.Log(string.Concat(new string[]
		{
			"Tried to set [",
			parameter_name,
			"] to [",
			parameter_value.ToString(),
			"] but [",
			snapshot_name,
			"] is not active."
		}));
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x000A8900 File Offset: 0x000A6B00
	public void StartPersistentSnapshots()
	{
		this.persistentSnapshotsActive = true;
		this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
		this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
		this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x000A8984 File Offset: 0x000A6B84
	public void StopPersistentSnapshots()
	{
		this.persistentSnapshotsActive = false;
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().PulseSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x000A8A04 File Offset: 0x000A6C04
	private string GetSnapshotName(EventReference event_ref)
	{
		string result;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out result);
		return result;
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x000A8A28 File Offset: 0x000A6C28
	public void UpdatePersistentSnapshotParameters()
	{
		this.SetVisibleDuplicants();
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName, out this.duplicantCountMovingInst))
		{
			this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName2 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName2, out this.duplicantCountSleepingInst))
		{
			this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName3 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		if (this.activeSnapshots.TryGetValue(snapshotName3, out this.duplicantCountInst))
		{
			this.duplicantCountInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName4 = this.GetSnapshotName(AudioMixerSnapshots.Get().PulseSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName4, out this.pulseInst))
		{
			float num = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
			int speed = SpeedControlScreen.Instance.GetSpeed();
			if (speed == 1)
			{
				num /= 2f;
			}
			else if (speed == 2)
			{
				num /= 3f;
			}
			float value = Mathf.Abs(Mathf.Sin(Time.time * 3.1415927f * num));
			this.pulseInst.setParameterByName("Pulse", value, false);
		}
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x000A8BD7 File Offset: 0x000A6DD7
	public void UpdateSpaceVisibleSnapshot(float percent)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", percent, false);
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x000A8BEC File Offset: 0x000A6DEC
	public void PauseSpaceVisibleSnapshot(bool pause)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", 0f, true);
		this.spaceVisibleInst.setPaused(pause);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x000A8C12 File Offset: 0x000A6E12
	public void UpdateFacilityVisibleSnapshot(float percent)
	{
		this.facilityVisibleInst.setParameterByName("facilityVisible", percent, false);
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x000A8C28 File Offset: 0x000A6E28
	private void SetVisibleDuplicants()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			Vector3 position = Components.LiveMinionIdentities[i].transform.GetPosition();
			if (CameraController.Instance.IsVisiblePos(position))
			{
				num++;
				Navigator component = Components.LiveMinionIdentities[i].GetComponent<Navigator>();
				if (component != null && component.IsMoving())
				{
					num2++;
				}
				else
				{
					StaminaMonitor.Instance smi = Components.LiveMinionIdentities[i].GetComponent<WorkerBase>().GetSMI<StaminaMonitor.Instance>();
					if (smi != null && smi.IsSleeping())
					{
						num3++;
					}
				}
			}
		}
		this.visibleDupes["visible"] = num;
		this.visibleDupes["moving"] = num2;
		this.visibleDupes["sleeping"] = num3;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x000A8D08 File Offset: 0x000A6F08
	public void StartUserVolumesSnapshot()
	{
		this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			EventDescription eventDescription;
			eventInstance.getDescription(out eventDescription);
			USER_PROPERTY user_PROPERTY;
			eventDescription.getUserProperty("buses", out user_PROPERTY);
			string text = user_PROPERTY.stringValue();
			char separator = '-';
			string[] array = text.Split(separator, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				float busLevel = 1f;
				string key = "Volume_" + array[i];
				if (KPlayerPrefs.HasKey(key))
				{
					busLevel = KPlayerPrefs.GetFloat(key);
				}
				AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
				userVolumeBus.busLevel = busLevel;
				userVolumeBus.labelString = Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + array[i].ToUpper());
				this.userVolumeSettings.Add(array[i], userVolumeBus);
				this.SetUserVolume(array[i], userVolumeBus.busLevel);
			}
		}
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000A8E1C File Offset: 0x000A701C
	public void SetUserVolume(string bus, float value)
	{
		if (!this.userVolumeSettings.ContainsKey(bus))
		{
			global::Debug.LogError("The provided bus doesn't exist. Check yo'self fool!");
			return;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		else if (value < 0f)
		{
			value = 0f;
		}
		this.userVolumeSettings[bus].busLevel = value;
		KPlayerPrefs.SetFloat("Volume_" + bus, value);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel, false);
		}
		else
		{
			this.Log(string.Concat(new string[]
			{
				"Tried to set [",
				bus,
				"] to [",
				value.ToString(),
				"] but UserVolumeSettingsSnapshot is not active."
			}));
		}
		if (bus == "Music")
		{
			this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000A8F2D File Offset: 0x000A712D
	private void Log(string s)
	{
	}

	// Token: 0x0400120C RID: 4620
	private static AudioMixer _instance = null;

	// Token: 0x0400120D RID: 4621
	private const string DUPLICANT_COUNT_ID = "duplicantCount";

	// Token: 0x0400120E RID: 4622
	private const string PULSE_ID = "Pulse";

	// Token: 0x0400120F RID: 4623
	private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";

	// Token: 0x04001210 RID: 4624
	private const string SPACE_VISIBLE_ID = "spaceVisible";

	// Token: 0x04001211 RID: 4625
	private const string FACILITY_VISIBLE_ID = "facilityVisible";

	// Token: 0x04001212 RID: 4626
	private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";

	// Token: 0x04001213 RID: 4627
	public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();

	// Token: 0x04001214 RID: 4628
	public List<HashedString> SnapshotDebugLog = new List<HashedString>();

	// Token: 0x04001215 RID: 4629
	public bool activeNIS;

	// Token: 0x04001216 RID: 4630
	public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;

	// Token: 0x04001217 RID: 4631
	public static float PULSE_SNAPSHOT_BPM = 120f;

	// Token: 0x04001218 RID: 4632
	public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;

	// Token: 0x04001219 RID: 4633
	private EventInstance duplicantCountInst;

	// Token: 0x0400121A RID: 4634
	private EventInstance pulseInst;

	// Token: 0x0400121B RID: 4635
	private EventInstance duplicantCountMovingInst;

	// Token: 0x0400121C RID: 4636
	private EventInstance duplicantCountSleepingInst;

	// Token: 0x0400121D RID: 4637
	private EventInstance spaceVisibleInst;

	// Token: 0x0400121E RID: 4638
	private EventInstance facilityVisibleInst;

	// Token: 0x0400121F RID: 4639
	private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");

	// Token: 0x04001220 RID: 4640
	public bool persistentSnapshotsActive;

	// Token: 0x04001221 RID: 4641
	private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();

	// Token: 0x04001222 RID: 4642
	public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

	// Token: 0x02001403 RID: 5123
	public class UserVolumeBus
	{
		// Token: 0x04006D1A RID: 27930
		public string labelString;

		// Token: 0x04006D1B RID: 27931
		public float busLevel;
	}
}
