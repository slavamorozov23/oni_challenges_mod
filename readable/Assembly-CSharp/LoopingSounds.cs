using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000600 RID: 1536
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSounds")]
public class LoopingSounds : KMonoBehaviour
{
	// Token: 0x060023BD RID: 9149 RVA: 0x000CEA9C File Offset: 0x000CCC9C
	public bool IsSoundPlaying(string path)
	{
		using (List<LoopingSounds.LoopingSoundEvent>.Enumerator enumerator = this.loopingSounds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.asset == path)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000CEAFC File Offset: 0x000CCCFC
	public bool StartSound(string asset, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, bool ignore_pause = false, bool enable_camera_scaled_position = true)
	{
		if (asset == null || asset == "")
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = behaviour.position;
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, !ignore_pause, true, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000CEBE4 File Offset: 0x000CCDE4
	public bool StartSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.StartSound(eventReferencePath);
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000CEC00 File Offset: 0x000CCE00
	public bool StartSound(string asset)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, true, true, true, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000CECE0 File Offset: 0x000CCEE0
	public bool StartSound(string asset, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000CEDC0 File Offset: 0x000CCFC0
	public void UpdateVelocity(string asset, Vector2 value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateVelocity(loopingSoundEvent.handle, value);
				break;
			}
		}
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000CEE30 File Offset: 0x000CD030
	public void UpdateFirstParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000CEEA0 File Offset: 0x000CD0A0
	public void UpdateSecondParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateSecondParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000CEF10 File Offset: 0x000CD110
	private void StopSoundAtIndex(int i)
	{
		LoopingSoundManager.StopSound(this.loopingSounds[i].handle);
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000CEF28 File Offset: 0x000CD128
	public void StopSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.StopSound(eventReferencePath);
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000CEF44 File Offset: 0x000CD144
	public void StopSound(string asset)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				this.StopSoundAtIndex(i);
				this.loopingSounds.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000CEF94 File Offset: 0x000CD194
	public void PauseSound(string asset, bool paused)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				LoopingSoundManager.PauseSound(this.loopingSounds[i].handle, paused);
				return;
			}
		}
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000CEFE8 File Offset: 0x000CD1E8
	public void StopAllSounds()
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			this.StopSoundAtIndex(i);
		}
		this.loopingSounds.Clear();
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000CF01D File Offset: 0x000CD21D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopAllSounds();
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000CF02C File Offset: 0x000CD22C
	public void SetParameter(EventReference event_ref, HashedString parameter, float value)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.SetParameter(eventReferencePath, parameter, value);
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x000CF04C File Offset: 0x000CD24C
	public void SetParameter(string path, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == path)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000CF0BC File Offset: 0x000CD2BC
	public void PlayEvent(GameSoundEvents.Event ev)
	{
		if (AudioDebug.Get().debugGameEventSounds)
		{
			string str = "GameSoundEvent: ";
			HashedString name = ev.Name;
			global::Debug.Log(str + name.ToString());
		}
		List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
		if (events == null)
		{
			return;
		}
		Vector2 v = base.transform.GetPosition();
		for (int i = 0; i < events.Count; i++)
		{
			SoundEvent soundEvent = events[i] as SoundEvent;
			if (soundEvent == null || soundEvent.sound == null)
			{
				return;
			}
			if (CameraController.Instance.IsAudibleSound(v, soundEvent.sound))
			{
				if (AudioDebug.Get().debugGameEventSounds)
				{
					global::Debug.Log("GameSound: " + soundEvent.sound);
				}
				float num = 0f;
				if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
				{
					if (Time.time - num > soundEvent.minInterval)
					{
						SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
					}
				}
				else
				{
					SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
				}
				this.lastTimePlayed[soundEvent.soundHash] = Time.time;
			}
		}
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000CF20C File Offset: 0x000CD40C
	public void UpdateObjectSelection(bool selected)
	{
		GameObject gameObject = base.gameObject;
		if (selected && gameObject != null && CameraController.Instance.IsVisiblePos(gameObject.transform.position))
		{
			this.objectIsSelectedAndVisible = true;
			this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.sound_pos);
			this.vol = 1f;
		}
		else
		{
			this.objectIsSelectedAndVisible = false;
			this.sound_pos = base.transform.GetPosition();
			this.sound_pos.z = 0f;
			this.vol = 1f;
		}
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			LoopingSoundManager.Get().UpdateObjectSelection(this.loopingSounds[i].handle, this.sound_pos, this.vol, this.objectIsSelectedAndVisible);
		}
	}

	// Token: 0x040014CB RID: 5323
	private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();

	// Token: 0x040014CC RID: 5324
	private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();

	// Token: 0x040014CD RID: 5325
	[SerializeField]
	public bool updatePosition;

	// Token: 0x040014CE RID: 5326
	public float vol = 1f;

	// Token: 0x040014CF RID: 5327
	public bool objectIsSelectedAndVisible;

	// Token: 0x040014D0 RID: 5328
	public Vector3 sound_pos;

	// Token: 0x020014D7 RID: 5335
	private struct LoopingSoundEvent
	{
		// Token: 0x04006FCA RID: 28618
		public string asset;

		// Token: 0x04006FCB RID: 28619
		public HandleVector<int>.Handle handle;
	}
}
