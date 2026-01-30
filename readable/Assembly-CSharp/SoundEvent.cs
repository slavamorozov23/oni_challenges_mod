using System;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000560 RID: 1376
[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000A6343 File Offset: 0x000A4543
	// (set) Token: 0x06001E87 RID: 7815 RVA: 0x000A634B File Offset: 0x000A454B
	public string sound { get; private set; }

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000A6354 File Offset: 0x000A4554
	// (set) Token: 0x06001E89 RID: 7817 RVA: 0x000A635C File Offset: 0x000A455C
	public HashedString soundHash { get; private set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000A6365 File Offset: 0x000A4565
	// (set) Token: 0x06001E8B RID: 7819 RVA: 0x000A636D File Offset: 0x000A456D
	public bool looping { get; private set; }

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000A6376 File Offset: 0x000A4576
	// (set) Token: 0x06001E8D RID: 7821 RVA: 0x000A637E File Offset: 0x000A457E
	public bool ignorePause { get; set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000A6387 File Offset: 0x000A4587
	// (set) Token: 0x06001E8F RID: 7823 RVA: 0x000A638F File Offset: 0x000A458F
	public bool shouldCameraScalePosition { get; set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000A6398 File Offset: 0x000A4598
	// (set) Token: 0x06001E91 RID: 7825 RVA: 0x000A63A0 File Offset: 0x000A45A0
	public float minInterval { get; private set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06001E92 RID: 7826 RVA: 0x000A63A9 File Offset: 0x000A45A9
	// (set) Token: 0x06001E93 RID: 7827 RVA: 0x000A63B1 File Offset: 0x000A45B1
	public bool objectIsSelectedAndVisible { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000A63BA File Offset: 0x000A45BA
	// (set) Token: 0x06001E95 RID: 7829 RVA: 0x000A63C2 File Offset: 0x000A45C2
	public EffectorValues noiseValues { get; set; }

	// Token: 0x06001E96 RID: 7830 RVA: 0x000A63CB File Offset: 0x000A45CB
	public SoundEvent()
	{
	}

	// Token: 0x06001E97 RID: 7831 RVA: 0x000A63D4 File Offset: 0x000A45D4
	public SoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame)
	{
		this.shouldCameraScalePosition = true;
		if (do_load)
		{
			this.sound = GlobalAssets.GetSound(sound_name, false);
			this.soundHash = new HashedString(this.sound);
			string.IsNullOrEmpty(this.sound);
		}
		this.minInterval = min_interval;
		this.looping = is_looping;
		this.isDynamic = is_dynamic;
		this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000A6449 File Offset: 0x000A4649
	public static bool ObjectIsSelectedAndVisible(GameObject go)
	{
		return false;
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x000A644C File Offset: 0x000A464C
	public static Vector3 AudioHighlightListenerPosition(Vector3 sound_pos)
	{
		Vector3 position = SoundListenerController.Instance.transform.position;
		float x = 1f * sound_pos.x + 0f * position.x;
		float y = 1f * sound_pos.y + 0f * position.y;
		float z = 0f * position.z;
		return new Vector3(x, y, z);
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000A64B4 File Offset: 0x000A46B4
	public static float GetVolume(bool objectIsSelectedAndVisible)
	{
		float result = 1f;
		if (objectIsSelectedAndVisible)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000A64D1 File Offset: 0x000A46D1
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, bool is_looping, bool is_dynamic)
	{
		return SoundEvent.ShouldPlaySound(controller, sound, sound, is_looping, is_dynamic);
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x000A64E4 File Offset: 0x000A46E4
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, HashedString soundHash, bool is_looping, bool is_dynamic)
	{
		CameraController instance = CameraController.Instance;
		if (instance == null)
		{
			return true;
		}
		Vector3 position = controller.transform.GetPosition();
		Vector3 offset = controller.Offset;
		position.x += offset.x;
		position.y += offset.y;
		if (!SoundCuller.IsAudibleWorld(position))
		{
			return false;
		}
		SpeedControlScreen instance2 = SpeedControlScreen.Instance;
		if (is_dynamic)
		{
			return (!(instance2 != null) || !instance2.IsPaused) && instance.IsAudibleSound(position);
		}
		if (sound == null || SoundEvent.IsLowPrioritySound(sound))
		{
			return false;
		}
		if (!instance.IsAudibleSound(position, soundHash))
		{
			if (!is_looping && !GlobalAssets.IsHighPriority(sound))
			{
				return false;
			}
		}
		else if (instance2 != null && instance2.IsPaused)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000A65B0 File Offset: 0x000A47B0
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000A660C File Offset: 0x000A480C
	protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
	{
		Vector3 vector = behaviour.controller.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		KBatchedAnimController controller = behaviour.controller;
		if (controller != null)
		{
			Vector3 offset = controller.Offset;
			vector.x += offset.x;
			vector.y += offset.y;
		}
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector2 = vector;
			array[num] = vector2.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			if (this.looping)
			{
				LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
				if (component == null)
				{
					global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
				}
				else if (!component.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
					});
				}
			}
			else if (!SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues, SoundEvent.GetVolume(this.objectIsSelectedAndVisible), this.objectIsSelectedAndVisible))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
				});
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound != null) ? sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x000A680C File Offset: 0x000A4A0C
	public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour, this.sound);
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000A681C File Offset: 0x000A4A1C
	public static Vector3 GetCameraScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		Vector3 result = Vector3.zero;
		if (CameraController.Instance != null)
		{
			result = CameraController.Instance.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
		}
		return result;
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000A684A File Offset: 0x000A4A4A
	public static FMOD.Studio.EventInstance BeginOneShot(EventReference event_ref, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return KFMOD.BeginOneShot(event_ref, SoundEvent.GetCameraScaledPosition(pos, objectIsSelectedAndVisible), volume);
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000A685A File Offset: 0x000A4A5A
	public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return SoundEvent.BeginOneShot(RuntimeManager.PathToEventReference(ev), pos, volume, false);
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000A686A File Offset: 0x000A4A6A
	public static bool EndOneShot(FMOD.Studio.EventInstance instance)
	{
		return KFMOD.EndOneShot(instance);
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000A6874 File Offset: 0x000A4A74
	public static bool PlayOneShot(EventReference event_ref, Vector3 sound_pos, float volume = 1f)
	{
		bool result = false;
		if (!event_ref.IsNull)
		{
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(event_ref, sound_pos, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000A68A7 File Offset: 0x000A4AA7
	public static bool PlayOneShot(string sound, Vector3 sound_pos, float volume = 1f)
	{
		return SoundEvent.PlayOneShot(RuntimeManager.PathToEventReference(sound), sound_pos, volume);
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000A68B8 File Offset: 0x000A4AB8
	public static bool PlayOneShot(string sound, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(sound))
		{
			Vector3 vector = behaviour.controller.transform.GetPosition();
			vector.z = 0f;
			if (objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000A6914 File Offset: 0x000A4B14
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(this.sound);
			}
		}
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000A6946 File Offset: 0x000A4B46
	protected static bool IsLowPrioritySound(string sound)
	{
		return sound != null && Game.Instance != null && Game.MainCamera.orthographicSize > AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && !AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound);
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000A6980 File Offset: 0x000A4B80
	protected void PrintSoundDebug(string anim_name, string sound, string sound_name, Vector3 sound_pos)
	{
		if (sound != null)
		{
			string[] array = new string[7];
			array[0] = anim_name;
			array[1] = ", ";
			array[2] = sound_name;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = sound_pos;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
			return;
		}
		global::Debug.Log("Missing sound: " + anim_name + ", " + sound_name);
	}

	// Token: 0x040011DD RID: 4573
	public static int IGNORE_INTERVAL = -1;

	// Token: 0x040011E6 RID: 4582
	protected bool isDynamic;
}
