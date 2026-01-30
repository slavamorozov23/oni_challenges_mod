using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020005FF RID: 1535
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSoundManager")]
public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060023A8 RID: 9128 RVA: 0x000CDE26 File Offset: 0x000CC026
	public static void DestroyInstance()
	{
		LoopingSoundManager.instance = null;
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000CDE2E File Offset: 0x000CC02E
	protected override void OnPrefabInit()
	{
		LoopingSoundManager.instance = this;
		this.CollectParameterUpdaters();
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000CDE3C File Offset: 0x000CC03C
	protected override void OnSpawn()
	{
		if (SpeedControlScreen.Instance != null && Game.Instance != null)
		{
			Game.Instance.Subscribe(-1788536802, new Action<object>(LoopingSoundManager.instance.OnPauseChanged));
		}
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			this.OnActiveWorldChanged();
		});
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000CDE9F File Offset: 0x000CC09F
	private void OnActiveWorldChanged()
	{
		this.StopAllSounds();
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x000CDEA8 File Offset: 0x000CC0A8
	private void CollectParameterUpdaters()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (!type.IsAbstract)
			{
				bool flag = false;
				Type baseType = type.BaseType;
				while (baseType != null)
				{
					if (baseType == typeof(LoopingSoundParameterUpdater))
					{
						flag = true;
						break;
					}
					baseType = baseType.BaseType;
				}
				if (flag)
				{
					LoopingSoundParameterUpdater loopingSoundParameterUpdater = (LoopingSoundParameterUpdater)Activator.CreateInstance(type);
					DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(loopingSoundParameterUpdater.parameter));
					this.parameterUpdaters[loopingSoundParameterUpdater.parameter] = loopingSoundParameterUpdater;
				}
			}
		}
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000CDF70 File Offset: 0x000CC170
	public void UpdateFirstParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.firstParameterValue = value;
		data.firstParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000CDFD4 File Offset: 0x000CC1D4
	public void UpdateSecondParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.secondParameterValue = value;
		data.secondParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000CE038 File Offset: 0x000CC238
	public void UpdateObjectSelection(HandleVector<int>.Handle handle, Vector3 sound_pos, float vol, bool objectIsSelectedAndVisible)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.pos = sound_pos;
		data.vol = vol;
		data.objectIsSelectedAndVisible = objectIsSelectedAndVisible;
		ATTRIBUTES_3D attributes = sound_pos.To3DAttributes();
		if (data.IsPlaying)
		{
			data.ev.set3DAttributes(attributes);
			data.ev.setVolume(vol);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000CE0A4 File Offset: 0x000CC2A4
	public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.velocity = velocity;
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000CE0D4 File Offset: 0x000CC2D4
	public void RenderEveryTick(float dt)
	{
		ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
		List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
		bool flag = Time.timeScale == 0f;
		SoundCuller soundCuller = CameraController.Instance.soundCuller;
		for (int i = 0; i < dataList.Count; i++)
		{
			LoopingSoundManager.Sound sound = dataList[i];
			if (sound.objectIsSelectedAndVisible)
			{
				sound.pos = SoundEvent.AudioHighlightListenerPosition(sound.transform.GetPosition());
				sound.vol = 1f;
			}
			else if (sound.transform != null)
			{
				sound.pos = sound.transform.GetPosition();
				sound.pos.z = 0f;
			}
			if (sound.animController != null)
			{
				Vector3 offset = sound.animController.Offset;
				sound.pos.x = sound.pos.x + offset.x;
				sound.pos.y = sound.pos.y + offset.y;
			}
			bool flag2 = !sound.IsCullingEnabled || (sound.ShouldCameraScalePosition && soundCuller.IsAudible(sound.pos, sound.falloffDistanceSq)) || soundCuller.IsAudibleNoCameraScaling(sound.pos, sound.falloffDistanceSq);
			bool isPlaying = sound.IsPlaying;
			if (flag2)
			{
				pooledList.Add(sound);
				if (!isPlaying)
				{
					SoundDescription soundDescription = this.GetSoundDescription(sound.path);
					sound.ev = KFMOD.CreateInstance(soundDescription.path);
					dataList[i] = sound;
					pooledList2.Add(i);
				}
			}
			else if (isPlaying)
			{
				pooledList3.Add(i);
			}
		}
		foreach (int index in pooledList2)
		{
			LoopingSoundManager.Sound sound2 = dataList[index];
			SoundDescription soundDescription2 = this.GetSoundDescription(sound2.path);
			sound2.ev.setPaused(flag && sound2.ShouldPauseOnGamePaused);
			sound2.pos.z = 0f;
			Vector3 pos = sound2.pos;
			if (sound2.objectIsSelectedAndVisible)
			{
				sound2.pos = SoundEvent.AudioHighlightListenerPosition(sound2.transform.GetPosition());
				sound2.vol = 1f;
			}
			else if (sound2.transform != null)
			{
				sound2.pos = sound2.transform.GetPosition();
			}
			sound2.ev.set3DAttributes(pos.To3DAttributes());
			sound2.ev.setVolume(sound2.vol);
			sound2.ev.start();
			sound2.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
			if (sound2.firstParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.firstParameter), sound2.firstParameterValue, false);
			}
			if (sound2.secondParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.secondParameter), sound2.secondParameterValue, false);
			}
			LoopingSoundParameterUpdater.Sound sound3 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound2.ev,
				path = sound2.path,
				description = soundDescription2,
				transform = sound2.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter in soundDescription2.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (this.parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					loopingSoundParameterUpdater.Add(sound3);
				}
			}
			dataList[index] = sound2;
		}
		pooledList2.Recycle();
		foreach (int index2 in pooledList3)
		{
			LoopingSoundManager.Sound sound4 = dataList[index2];
			SoundDescription soundDescription3 = this.GetSoundDescription(sound4.path);
			LoopingSoundParameterUpdater.Sound sound5 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound4.ev,
				path = sound4.path,
				description = soundDescription3,
				transform = sound4.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter2 in soundDescription3.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater2 = null;
				if (this.parameterUpdaters.TryGetValue(parameter2.name, out loopingSoundParameterUpdater2))
				{
					loopingSoundParameterUpdater2.Remove(sound5);
				}
			}
			if (sound4.ShouldCameraScalePosition)
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			sound4.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
			sound4.ev.release();
			dataList[index2] = sound4;
		}
		pooledList3.Recycle();
		float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
		foreach (LoopingSoundManager.Sound sound6 in pooledList)
		{
			ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition(sound6.pos, sound6.objectIsSelectedAndVisible).To3DAttributes();
			attributes.velocity = (sound6.velocity * velocityScale).ToFMODVector();
			EventInstance ev = sound6.ev;
			ev.set3DAttributes(attributes);
		}
		foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> keyValuePair in this.parameterUpdaters)
		{
			keyValuePair.Value.Update(dt);
		}
		pooledList.Recycle();
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000CE710 File Offset: 0x000CC910
	public static LoopingSoundManager Get()
	{
		return LoopingSoundManager.instance;
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000CE718 File Offset: 0x000CC918
	public void StopAllSounds()
	{
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				ev = sound.ev;
				ev.release();
			}
		}
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000CE794 File Offset: 0x000CC994
	private SoundDescription GetSoundDescription(HashedString path)
	{
		return KFMOD.GetSoundEventDescription(path);
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000CE79C File Offset: 0x000CC99C
	public HandleVector<int>.Handle Add(string path, Vector3 pos, Transform transform = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true, float vol = 1f, bool objectIsSelectedAndVisible = false)
	{
		SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(path);
		LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags)0;
		if (pause_on_game_pause)
		{
			flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
		}
		if (enable_culling)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
		}
		if (enable_camera_scaled_position)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
		}
		KBatchedAnimController animController = null;
		if (transform != null)
		{
			animController = transform.GetComponent<KBatchedAnimController>();
		}
		LoopingSoundManager.Sound initial_data = new LoopingSoundManager.Sound
		{
			transform = transform,
			animController = animController,
			falloffDistanceSq = soundEventDescription.falloffDistanceSq,
			path = path,
			pos = pos,
			flags = flags,
			firstParameter = HashedString.Invalid,
			secondParameter = HashedString.Invalid,
			vol = vol,
			objectIsSelectedAndVisible = objectIsSelectedAndVisible
		};
		return this.sounds.Allocate(initial_data);
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x000CE85C File Offset: 0x000CCA5C
	public static HandleVector<int>.Handle StartSound(EventReference event_ref, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		return LoopingSoundManager.StartSound(KFMOD.GetEventReferencePath(event_ref), pos, pause_on_game_pause, enable_culling);
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000CE86C File Offset: 0x000CCA6C
	public static HandleVector<int>.Handle StartSound(string path, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogWarning("Missing sound");
			return HandleVector<int>.InvalidHandle;
		}
		return LoopingSoundManager.Get().Add(path, pos, null, pause_on_game_pause, enable_culling, true, 1f, false);
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000CE8A8 File Offset: 0x000CCAA8
	public static void StopSound(HandleVector<int>.Handle handle)
	{
		if (LoopingSoundManager.Get() == null)
		{
			return;
		}
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.stop(LoopingSoundManager.Get().GameIsPaused ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			data.ev.release();
			SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(data.path);
			foreach (SoundDescription.Parameter parameter in soundEventDescription.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound
					{
						ev = data.ev,
						path = data.path,
						description = soundEventDescription,
						transform = data.transform,
						objectIsSelectedAndVisible = false
					};
					loopingSoundParameterUpdater.Remove(sound);
				}
			}
		}
		LoopingSoundManager.Get().sounds.Free(handle);
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000CE9B0 File Offset: 0x000CCBB0
	public static void PauseSound(HandleVector<int>.Handle handle, bool paused)
	{
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.setPaused(paused);
		}
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000CE9E8 File Offset: 0x000CCBE8
	private void OnPauseChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		this.GameIsPaused = value;
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.setPaused(value && sound.ShouldPauseOnGamePaused);
			}
		}
	}

	// Token: 0x040014C7 RID: 5319
	private static LoopingSoundManager instance;

	// Token: 0x040014C8 RID: 5320
	private bool GameIsPaused;

	// Token: 0x040014C9 RID: 5321
	private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();

	// Token: 0x040014CA RID: 5322
	private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>(0);

	// Token: 0x020014D5 RID: 5333
	public class Tuning : TuningData<LoopingSoundManager.Tuning>
	{
		// Token: 0x04006FBB RID: 28603
		public float velocityScale;
	}

	// Token: 0x020014D6 RID: 5334
	public struct Sound
	{
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06009142 RID: 37186 RVA: 0x00370CDC File Offset: 0x0036EEDC
		public bool IsPlaying
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06009143 RID: 37187 RVA: 0x00370CE9 File Offset: 0x0036EEE9
		public bool ShouldPauseOnGamePaused
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06009144 RID: 37188 RVA: 0x00370CF6 File Offset: 0x0036EEF6
		public bool IsCullingEnabled
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06009145 RID: 37189 RVA: 0x00370D03 File Offset: 0x0036EF03
		public bool ShouldCameraScalePosition
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x04006FBC RID: 28604
		public EventInstance ev;

		// Token: 0x04006FBD RID: 28605
		public Transform transform;

		// Token: 0x04006FBE RID: 28606
		public KBatchedAnimController animController;

		// Token: 0x04006FBF RID: 28607
		public float falloffDistanceSq;

		// Token: 0x04006FC0 RID: 28608
		public HashedString path;

		// Token: 0x04006FC1 RID: 28609
		public Vector3 pos;

		// Token: 0x04006FC2 RID: 28610
		public Vector2 velocity;

		// Token: 0x04006FC3 RID: 28611
		public HashedString firstParameter;

		// Token: 0x04006FC4 RID: 28612
		public HashedString secondParameter;

		// Token: 0x04006FC5 RID: 28613
		public float firstParameterValue;

		// Token: 0x04006FC6 RID: 28614
		public float secondParameterValue;

		// Token: 0x04006FC7 RID: 28615
		public float vol;

		// Token: 0x04006FC8 RID: 28616
		public bool objectIsSelectedAndVisible;

		// Token: 0x04006FC9 RID: 28617
		public LoopingSoundManager.Sound.Flags flags;

		// Token: 0x020028A4 RID: 10404
		[Flags]
		public enum Flags
		{
			// Token: 0x0400B306 RID: 45830
			PLAYING = 1,
			// Token: 0x0400B307 RID: 45831
			PAUSE_ON_GAME_PAUSED = 2,
			// Token: 0x0400B308 RID: 45832
			ENABLE_CULLING = 4,
			// Token: 0x0400B309 RID: 45833
			ENABLE_CAMERA_SCALED_POSITION = 8
		}
	}
}
