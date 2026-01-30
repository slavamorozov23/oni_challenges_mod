using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using ProcGen;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06004D32 RID: 19762 RVA: 0x001C1219 File Offset: 0x001BF419
	public Dictionary<string, MusicManager.SongInfo> SongMap
	{
		get
		{
			return this.songMap;
		}
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x001C1224 File Offset: 0x001BF424
	public void PlaySong(string song_name, bool canWait = false)
	{
		this.Log("Play: " + song_name);
		if (!AudioDebug.Get().musicEnabled)
		{
			return;
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to play duplicate song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.Count == 0)
		{
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array = new object[1];
				int num = 0;
				string str = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array[num] = str + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array);
			}
			int num2 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num2 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num2, false);
			}
			if (songInfo.dynamic)
			{
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
				this.activeDynamicSong = songInfo;
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		if (songInfo.interruptsActiveMusic)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.activeSongs[list[i]].interruptsActiveMusic)
				{
					MusicManager.SongInfo songInfo2 = this.activeSongs[list[i]];
					songInfo2.ev.setParameterByName("interrupted_dimmed", 1f, false);
					this.Log("Dimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					songInfo.songsOnHold.Add(list[i]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array2 = new object[1];
				int num3 = 0;
				string str2 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array2[num3] = str2 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array2);
			}
			songInfo.ev.start();
			songInfo.ev.release();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		int num4 = 0;
		foreach (string key in this.activeSongs.Keys)
		{
			MusicManager.SongInfo songInfo3 = this.activeSongs[key];
			if (!songInfo3.interruptsActiveMusic && songInfo3.priority > num4)
			{
				num4 = songInfo3.priority;
			}
		}
		if (songInfo.priority >= num4)
		{
			for (int j = 0; j < list.Count; j++)
			{
				MusicManager.SongInfo songInfo4 = this.activeSongs[list[j]];
				FMOD.Studio.EventInstance ev = songInfo4.ev;
				if (!songInfo4.interruptsActiveMusic)
				{
					ev.setParameterByName("interrupted_dimmed", 1f, false);
					ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.activeSongs.Remove(list[j]);
					list.Remove(list[j]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array3 = new object[1];
				int num5 = 0;
				string str3 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array3[num5] = str3 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array3);
			}
			int num6 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num6 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num6, false);
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
		}
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001C1634 File Offset: 0x001BF834
	public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (shouldLog)
		{
			this.Log("Stop: " + song_name);
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (!this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to stop a song that isn't playing:",
				song_name
			});
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		ev.stop(stopMode);
		ev.release();
		if (songInfo.dynamic)
		{
			this.activeDynamicSong = null;
		}
		if (songInfo.songsOnHold.Count > 0)
		{
			for (int i = 0; i < songInfo.songsOnHold.Count; i++)
			{
				MusicManager.SongInfo songInfo2;
				if (this.activeSongs.TryGetValue(songInfo.songsOnHold[i], out songInfo2) && songInfo2.ev.isValid())
				{
					FMOD.Studio.EventInstance ev2 = songInfo2.ev;
					this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					ev2.setParameterByName("interrupted_dimmed", 0f, false);
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
				else
				{
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
			}
		}
		this.activeSongs.Remove(song_name);
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001C1798 File Offset: 0x001BF998
	public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		this.Log("Kill All Songs");
		if (this.DynamicMusicIsActive())
		{
			this.StopDynamicMusic(true);
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			this.StopSong(list[i], true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001C17F0 File Offset: 0x001BF9F0
	public void SetSongParameter(string song_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_value));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByName(parameter_name, parameter_value, false);
		}
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001C1848 File Offset: 0x001BFA48
	public void SetSongParameter(string song_name, string parameter_name, string parameter_lable, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_lable));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByNameWithLabel(parameter_name, parameter_lable, false);
		}
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001C189C File Offset: 0x001BFA9C
	public bool SongIsPlaying(string song_name)
	{
		MusicManager.SongInfo songInfo = null;
		return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x001C18C8 File Offset: 0x001BFAC8
	private void Update()
	{
		this.ClearFinishedSongs();
		if (this.DynamicMusicIsActive())
		{
			this.SetDynamicMusicZoomLevel();
			this.SetDynamicMusicTimeSinceLastJob();
			if (this.activeDynamicSong.useTimeOfDay)
			{
				this.SetDynamicMusicTimeOfDay();
			}
			if (GameClock.Instance != null && GameClock.Instance.GetCurrentCycleAsPercentage() >= this.duskTimePercentage / 100f)
			{
				this.StopDynamicMusic(false);
			}
		}
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x001C1930 File Offset: 0x001BFB30
	private void ClearFinishedSongs()
	{
		if (this.activeSongs.Count > 0)
		{
			ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				MusicManager.SongInfo value = keyValuePair.Value;
				FMOD.Studio.EventInstance ev = value.ev;
				ev.getPlaybackState(out value.musicPlaybackState);
				if (value.musicPlaybackState == PLAYBACK_STATE.STOPPED || value.musicPlaybackState == PLAYBACK_STATE.STOPPING)
				{
					pooledList.Add(keyValuePair.Key);
					foreach (string song_name in value.songsOnHold)
					{
						this.SetSongParameter(song_name, "interrupted_dimmed", 0f, true);
					}
					value.songsOnHold.Clear();
				}
			}
			foreach (string key in pooledList)
			{
				this.activeSongs.Remove(key);
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001C1A80 File Offset: 0x001BFC80
	public void OnEscapeMenu(bool paused)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null)
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
			}
		}
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x001C1AF0 File Offset: 0x001BFCF0
	public void OnSupplyClosetMenu(bool paused, float fadeTime)
	{
		bool flag = !paused;
		if (!PauseScreen.Instance.IsNullOrDestroyed() && PauseScreen.Instance.IsActive() && flag && MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.SongInfo songInfo = this.songMap["Music_ESC_Menu"];
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value != songInfo)
				{
					this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
				}
			}
			this.StartFadeToPause(songInfo.ev, false, 0.25f);
			return;
		}
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair2 in this.activeSongs)
		{
			if (keyValuePair2.Value != null)
			{
				this.StartFadeToPause(keyValuePair2.Value.ev, paused, fadeTime);
			}
		}
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001C1C1C File Offset: 0x001BFE1C
	public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
	{
		if (paused)
		{
			base.StartCoroutine(this.FadeToPause(inst, fadeTime));
			return;
		}
		base.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001C1C40 File Offset: 0x001BFE40
	private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 0f;
		float lerpTime = 0f;
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		inst.setPaused(true);
		yield break;
	}

	// Token: 0x06004D3F RID: 19775 RVA: 0x001C1C56 File Offset: 0x001BFE56
	private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 1f;
		float lerpTime = 0f;
		inst.setPaused(false);
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004D40 RID: 19776 RVA: 0x001C1C6C File Offset: 0x001BFE6C
	public void WattsonStartDynamicMusic()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout != null && currentClusterLayout.clusterAudio != null && !string.IsNullOrWhiteSpace(currentClusterLayout.clusterAudio.musicFirst))
		{
			DebugUtil.Assert(this.fullSongPlaylist.songMap.ContainsKey(currentClusterLayout.clusterAudio.musicFirst), "Attempting to play dlc music that isn't in the fullSongPlaylist");
			this.activePlaylist = this.fullSongPlaylist;
			this.PlayDynamicMusic(currentClusterLayout.clusterAudio.musicFirst);
			return;
		}
		this.PlayDynamicMusic();
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001C1CEC File Offset: 0x001BFEEC
	public void PlayDynamicMusic()
	{
		if (this.DynamicMusicIsActive())
		{
			this.Log("Trying to play DynamicMusic when it is already playing.");
			return;
		}
		string nextDynamicSong = this.GetNextDynamicSong();
		this.PlayDynamicMusic(nextDynamicSong);
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001C1D1C File Offset: 0x001BFF1C
	private void PlayDynamicMusic(string song_name)
	{
		if (song_name == "NONE")
		{
			return;
		}
		this.PlaySong(song_name, false);
		MusicManager.SongInfo songInfo;
		if (this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			this.activeDynamicSong = songInfo;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
			if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
			{
				this.SetDynamicMusicPaused();
			}
			if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
			{
				this.SetDynamicMusicOverlayActive();
			}
			this.SetDynamicMusicPlayHook();
			this.SetDynamicMusicKeySigniture();
			string key = "Volume_Music";
			if (KPlayerPrefs.HasKey(key))
			{
				float @float = KPlayerPrefs.GetFloat(key);
				AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", @float, true);
			}
			AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
			return;
		}
		this.Log("DynamicMusic song " + song_name + " did not start.");
		string text = "";
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			text = text + keyValuePair.Key + ", ";
			global::Debug.Log(text);
		}
		DebugUtil.DevAssert(false, "Song failed to play: " + song_name, null);
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x001C1EA0 File Offset: 0x001C00A0
	public void StopDynamicMusic(bool stopImmediate = false)
	{
		if (this.activeDynamicSong != null)
		{
			FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
			this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
			this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
			this.activeDynamicSong = null;
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001C1F14 File Offset: 0x001C0114
	public string GetNextDynamicSong()
	{
		string result = "";
		if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
		{
			while (this.nextMusicType == MusicManager.TypeOfMusic.None)
			{
				this.CycleToNextMusicType();
			}
		}
		switch (this.nextMusicType)
		{
		case MusicManager.TypeOfMusic.DynamicSong:
			result = this.fullSongPlaylist.GetNextSong();
			this.activePlaylist = this.fullSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.MiniSong:
			result = this.miniSongPlaylist.GetNextSong();
			this.activePlaylist = this.miniSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.None:
			result = "NONE";
			this.activePlaylist = null;
			break;
		}
		this.CycleToNextMusicType();
		return result;
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001C1FAC File Offset: 0x001C01AC
	private void CycleToNextMusicType()
	{
		int num = this.musicTypeIterator + 1;
		this.musicTypeIterator = num;
		this.musicTypeIterator = num % this.musicStyleOrder.Length;
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001C1FEC File Offset: 0x001C01EC
	public bool DynamicMusicIsActive()
	{
		return this.activeDynamicSong != null;
	}

	// Token: 0x06004D47 RID: 19783 RVA: 0x001C1FF9 File Offset: 0x001C01F9
	public void SetDynamicMusicPaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
		}
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x001C2024 File Offset: 0x001C0224
	public void SetDynamicMusicUnpaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0f, true);
		}
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x001C2050 File Offset: 0x001C0250
	public void SetDynamicMusicZoomLevel()
	{
		if (CameraController.Instance != null)
		{
			float parameter_value = 100f - Camera.main.orthographicSize / 20f * 100f;
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
		}
	}

	// Token: 0x06004D4A RID: 19786 RVA: 0x001C20A4 File Offset: 0x001C02A4
	public void SetDynamicMusicTimeSinceLastJob()
	{
		this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
	}

	// Token: 0x06004D4B RID: 19787 RVA: 0x001C20D4 File Offset: 0x001C02D4
	public void SetDynamicMusicTimeOfDay()
	{
		if (this.time >= this.timeOfDayUpdateRate)
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
			this.time = 0f;
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x06004D4C RID: 19788 RVA: 0x001C2132 File Offset: 0x001C0332
	public void SetDynamicMusicOverlayActive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
		}
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001C215D File Offset: 0x001C035D
	public void SetDynamicMusicOverlayInactive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0f, true);
		}
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001C2188 File Offset: 0x001C0388
	public void SetDynamicMusicPlayHook()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0f, true);
			this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
		}
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001C2203 File Offset: 0x001C0403
	public bool ShouldPlayDynamicMusicLoadedGame()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() <= this.loadGameCutoffPercentage / 100f;
	}

	// Token: 0x06004D50 RID: 19792 RVA: 0x001C2220 File Offset: 0x001C0420
	public void SetDynamicMusicKeySigniture()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			string musicKeySigniture = this.activePlaylist.songMap[simpleSoundEventName].musicKeySigniture;
			float value;
			if (!(musicKeySigniture == "Ab"))
			{
				if (!(musicKeySigniture == "Bb"))
				{
					if (!(musicKeySigniture == "C"))
					{
						if (!(musicKeySigniture == "D"))
						{
							value = 2f;
						}
						else
						{
							value = 3f;
						}
					}
					else
					{
						value = 2f;
					}
				}
				else
				{
					value = 1f;
				}
			}
			else
			{
				value = 0f;
			}
			RuntimeManager.StudioSystem.setParameterByName("MusicInKey", value, false);
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06004D51 RID: 19793 RVA: 0x001C22D1 File Offset: 0x001C04D1
	public static MusicManager instance
	{
		get
		{
			return MusicManager._instance;
		}
	}

	// Token: 0x06004D52 RID: 19794 RVA: 0x001C22D8 File Offset: 0x001C04D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
		{
			this.alwaysPlayMusic = (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1);
		}
	}

	// Token: 0x06004D53 RID: 19795 RVA: 0x001C2312 File Offset: 0x001C0512
	protected override void OnPrefabInit()
	{
		MusicManager._instance = this;
		this.ConfigureSongs();
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x001C2333 File Offset: 0x001C0533
	protected override void OnCleanUp()
	{
		MusicManager._instance = null;
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x001C233B File Offset: 0x001C053B
	private static bool IsValidForDLCContext(string dlcid)
	{
		if (dlcid == "")
		{
			return true;
		}
		if (SaveLoader.Instance != null)
		{
			return Game.IsDlcActiveForCurrentSave(dlcid);
		}
		return DlcManager.IsContentSubscribed(dlcid);
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001C2368 File Offset: 0x001C0568
	[ContextMenu("Reload")]
	public void ConfigureSongs()
	{
		this.songMap.Clear();
		this.fullSongPlaylist.Clear();
		this.miniSongPlaylist.Clear();
		foreach (MusicManager.DynamicSong dynamicSong in this.fullSongs)
		{
			if (MusicManager.IsValidForDLCContext(dynamicSong.requiredDlcId))
			{
				string simpleSoundEventName = Assets.GetSimpleSoundEventName(dynamicSong.fmodEvent);
				MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
				songInfo.fmodEvent = dynamicSong.fmodEvent;
				songInfo.requiredDlcId = dynamicSong.requiredDlcId;
				songInfo.priority = 100;
				songInfo.interruptsActiveMusic = false;
				songInfo.dynamic = true;
				songInfo.useTimeOfDay = dynamicSong.useTimeOfDay;
				songInfo.numberOfVariations = dynamicSong.numberOfVariations;
				songInfo.musicKeySigniture = dynamicSong.musicKeySigniture;
				songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName] = songInfo;
				this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
			}
		}
		foreach (MusicManager.Minisong minisong in this.miniSongs)
		{
			if (MusicManager.IsValidForDLCContext(minisong.requiredDlcId))
			{
				string simpleSoundEventName2 = Assets.GetSimpleSoundEventName(minisong.fmodEvent);
				MusicManager.SongInfo songInfo2 = new MusicManager.SongInfo();
				songInfo2.fmodEvent = minisong.fmodEvent;
				songInfo2.requiredDlcId = minisong.requiredDlcId;
				songInfo2.priority = 100;
				songInfo2.interruptsActiveMusic = false;
				songInfo2.dynamic = true;
				songInfo2.useTimeOfDay = false;
				songInfo2.numberOfVariations = 5;
				songInfo2.musicKeySigniture = minisong.musicKeySigniture;
				songInfo2.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName2] = songInfo2;
				this.miniSongPlaylist.songMap[simpleSoundEventName2] = songInfo2;
			}
		}
		foreach (MusicManager.Stinger stinger in this.stingers)
		{
			if (MusicManager.IsValidForDLCContext(stinger.requiredDlcId))
			{
				string simpleSoundEventName3 = Assets.GetSimpleSoundEventName(stinger.fmodEvent);
				MusicManager.SongInfo songInfo3 = new MusicManager.SongInfo();
				songInfo3.fmodEvent = stinger.fmodEvent;
				songInfo3.priority = 100;
				songInfo3.interruptsActiveMusic = true;
				songInfo3.dynamic = false;
				songInfo3.useTimeOfDay = false;
				songInfo3.numberOfVariations = 0;
				songInfo3.requiredDlcId = stinger.requiredDlcId;
				this.songMap[simpleSoundEventName3] = songInfo3;
			}
		}
		foreach (MusicManager.MenuSong menuSong in this.menuSongs)
		{
			if (MusicManager.IsValidForDLCContext(menuSong.requiredDlcId))
			{
				string simpleSoundEventName4 = Assets.GetSimpleSoundEventName(menuSong.fmodEvent);
				MusicManager.SongInfo songInfo4 = new MusicManager.SongInfo();
				songInfo4.fmodEvent = menuSong.fmodEvent;
				songInfo4.priority = 100;
				songInfo4.interruptsActiveMusic = true;
				songInfo4.dynamic = false;
				songInfo4.useTimeOfDay = false;
				songInfo4.numberOfVariations = 0;
				songInfo4.requiredDlcId = menuSong.requiredDlcId;
				this.songMap[simpleSoundEventName4] = songInfo4;
			}
		}
		this.fullSongPlaylist.ResetUnplayedSongs();
		this.miniSongPlaylist.ResetUnplayedSongs();
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001C2676 File Offset: 0x001C0876
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001C2678 File Offset: 0x001C0878
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001C267A File Offset: 0x001C087A
	private void Log(string s)
	{
	}

	// Token: 0x04003388 RID: 13192
	private const string VARIATION_ID = "variation";

	// Token: 0x04003389 RID: 13193
	private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";

	// Token: 0x0400338A RID: 13194
	private const string MUSIC_KEY = "MusicInKey";

	// Token: 0x0400338B RID: 13195
	private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;

	// Token: 0x0400338C RID: 13196
	private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;

	// Token: 0x0400338D RID: 13197
	[Header("Song Lists")]
	[Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
	[SerializeField]
	private MusicManager.DynamicSong[] fullSongs;

	// Token: 0x0400338E RID: 13198
	[Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
	[SerializeField]
	private MusicManager.Minisong[] miniSongs;

	// Token: 0x0400338F RID: 13199
	[Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
	[SerializeField]
	private MusicManager.Stinger[] stingers;

	// Token: 0x04003390 RID: 13200
	[Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
	[SerializeField]
	private MusicManager.MenuSong[] menuSongs;

	// Token: 0x04003391 RID: 13201
	private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x04003392 RID: 13202
	public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x04003393 RID: 13203
	[Space]
	[Header("Tuning Values")]
	[Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
	[SerializeField]
	private float duskTimePercentage = 85f;

	// Token: 0x04003394 RID: 13204
	[Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
	[SerializeField]
	private float loadGameCutoffPercentage = 50f;

	// Token: 0x04003395 RID: 13205
	[Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float dynamicMusicSFXAttenuationPercentage = 65f;

	// Token: 0x04003396 RID: 13206
	[Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float miniSongSFXAttenuationPercentage;

	// Token: 0x04003397 RID: 13207
	[SerializeField]
	private MusicManager.TypeOfMusic[] musicStyleOrder;

	// Token: 0x04003398 RID: 13208
	[NonSerialized]
	public bool alwaysPlayMusic;

	// Token: 0x04003399 RID: 13209
	private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x0400339A RID: 13210
	private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x0400339B RID: 13211
	[NonSerialized]
	public MusicManager.SongInfo activeDynamicSong;

	// Token: 0x0400339C RID: 13212
	[NonSerialized]
	public MusicManager.DynamicSongPlaylist activePlaylist;

	// Token: 0x0400339D RID: 13213
	private MusicManager.TypeOfMusic nextMusicType;

	// Token: 0x0400339E RID: 13214
	private int musicTypeIterator;

	// Token: 0x0400339F RID: 13215
	private float time;

	// Token: 0x040033A0 RID: 13216
	private float timeOfDayUpdateRate = 2f;

	// Token: 0x040033A1 RID: 13217
	private static MusicManager _instance;

	// Token: 0x040033A2 RID: 13218
	[NonSerialized]
	public List<string> MusicDebugLog = new List<string>();

	// Token: 0x02001B74 RID: 7028
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class SongInfo
	{
		// Token: 0x04008505 RID: 34053
		public EventReference fmodEvent;

		// Token: 0x04008506 RID: 34054
		[NonSerialized]
		public int priority;

		// Token: 0x04008507 RID: 34055
		[NonSerialized]
		public bool interruptsActiveMusic;

		// Token: 0x04008508 RID: 34056
		[NonSerialized]
		public bool dynamic;

		// Token: 0x04008509 RID: 34057
		[NonSerialized]
		public string requiredDlcId;

		// Token: 0x0400850A RID: 34058
		[NonSerialized]
		public bool useTimeOfDay;

		// Token: 0x0400850B RID: 34059
		[NonSerialized]
		public int numberOfVariations;

		// Token: 0x0400850C RID: 34060
		[NonSerialized]
		public string musicKeySigniture = "C";

		// Token: 0x0400850D RID: 34061
		[NonSerialized]
		public FMOD.Studio.EventInstance ev;

		// Token: 0x0400850E RID: 34062
		[NonSerialized]
		public List<string> songsOnHold = new List<string>();

		// Token: 0x0400850F RID: 34063
		[NonSerialized]
		public PLAYBACK_STATE musicPlaybackState;

		// Token: 0x04008510 RID: 34064
		[NonSerialized]
		public bool playHook = true;

		// Token: 0x04008511 RID: 34065
		[NonSerialized]
		public float sfxAttenuationPercentage = 65f;
	}

	// Token: 0x02001B75 RID: 7029
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class DynamicSong
	{
		// Token: 0x04008512 RID: 34066
		public EventReference fmodEvent;

		// Token: 0x04008513 RID: 34067
		[Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
		[SerializeField]
		public bool useTimeOfDay;

		// Token: 0x04008514 RID: 34068
		[Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
		[SerializeField]
		public int numberOfVariations;

		// Token: 0x04008515 RID: 34069
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04008516 RID: 34070
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x02001B76 RID: 7030
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Stinger
	{
		// Token: 0x04008517 RID: 34071
		public EventReference fmodEvent;

		// Token: 0x04008518 RID: 34072
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x02001B77 RID: 7031
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class MenuSong
	{
		// Token: 0x04008519 RID: 34073
		public EventReference fmodEvent;

		// Token: 0x0400851A RID: 34074
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x02001B78 RID: 7032
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Minisong
	{
		// Token: 0x0400851B RID: 34075
		public EventReference fmodEvent;

		// Token: 0x0400851C RID: 34076
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x0400851D RID: 34077
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x02001B79 RID: 7033
	public enum TypeOfMusic
	{
		// Token: 0x0400851F RID: 34079
		DynamicSong,
		// Token: 0x04008520 RID: 34080
		MiniSong,
		// Token: 0x04008521 RID: 34081
		None
	}

	// Token: 0x02001B7A RID: 7034
	public class DynamicSongPlaylist
	{
		// Token: 0x0600AA1D RID: 43549 RVA: 0x003C3441 File Offset: 0x003C1641
		public void Clear()
		{
			this.songMap.Clear();
			this.unplayedSongs.Clear();
			this.lastSongPlayed = "";
		}

		// Token: 0x0600AA1E RID: 43550 RVA: 0x003C3464 File Offset: 0x003C1664
		public string GetNextSong()
		{
			string text;
			if (this.unplayedSongs.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index];
				this.unplayedSongs.RemoveAt(index);
			}
			else
			{
				this.ResetUnplayedSongs();
				bool flag = this.unplayedSongs.Count > 1;
				if (flag)
				{
					for (int i = 0; i < this.unplayedSongs.Count; i++)
					{
						if (this.unplayedSongs[i] == this.lastSongPlayed)
						{
							this.unplayedSongs.Remove(this.unplayedSongs[i]);
							break;
						}
					}
				}
				int index2 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index2];
				this.unplayedSongs.RemoveAt(index2);
				if (flag)
				{
					this.unplayedSongs.Add(this.lastSongPlayed);
				}
			}
			this.lastSongPlayed = text;
			global::Debug.Assert(this.songMap.ContainsKey(text), "Missing song " + text);
			return Assets.GetSimpleSoundEventName(this.songMap[text].fmodEvent);
		}

		// Token: 0x0600AA1F RID: 43551 RVA: 0x003C3590 File Offset: 0x003C1790
		public void ResetUnplayedSongs()
		{
			this.unplayedSongs.Clear();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.songMap)
			{
				if (MusicManager.IsValidForDLCContext(keyValuePair.Value.requiredDlcId))
				{
					this.unplayedSongs.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x04008522 RID: 34082
		public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

		// Token: 0x04008523 RID: 34083
		public List<string> unplayedSongs = new List<string>();

		// Token: 0x04008524 RID: 34084
		private string lastSongPlayed = "";
	}
}
