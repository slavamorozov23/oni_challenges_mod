using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class MainMenuSoundEvent : SoundEvent
{
	// Token: 0x06001E71 RID: 7793 RVA: 0x000A5A21 File Offset: 0x000A3C21
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000A5A38 File Offset: 0x000A3C38
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
		if (instance.isValid())
		{
			instance.setParameterByName("frame", (float)base.frame, false);
			KFMOD.EndOneShot(instance);
		}
	}
}
