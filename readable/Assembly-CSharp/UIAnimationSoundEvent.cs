using System;

// Token: 0x02000565 RID: 1381
public class UIAnimationSoundEvent : SoundEvent
{
	// Token: 0x06001ECA RID: 7882 RVA: 0x000A758C File Offset: 0x000A578C
	public UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000A75A1 File Offset: 0x000A57A1
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x000A75AC File Offset: 0x000A57AC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			try
			{
				if (SoundListenerController.Instance == null)
				{
					KFMOD.PlayUISound(base.sound);
				}
				else
				{
					KFMOD.PlayOneShot(base.sound, SoundListenerController.Instance.transform.GetPosition(), 1f);
				}
			}
			catch
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"AUDIOERROR: Missing [" + base.sound + "]"
				});
			}
		}
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x000A7698 File Offset: 0x000A5898
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}
}
