using System;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class UIAnimationVoiceSoundEvent : SoundEvent
{
	// Token: 0x06001ECE RID: 7886 RVA: 0x000A76CA File Offset: 0x000A58CA
	public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, false, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
		this.actualSoundName = sound_name;
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x000A76E6 File Offset: 0x000A58E6
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x000A76F0 File Offset: 0x000A58F0
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string soundPath = MinionVoice.ByObject(behaviour.controller).UnwrapOr(MinionVoice.Random(), string.Format("Couldn't find MinionVoice on UI {0}, falling back to random voice", behaviour.controller)).GetSoundPath(this.actualSoundName);
		if (this.actualSoundName.Contains(":"))
		{
			float num = float.Parse(this.actualSoundName.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return;
			}
		}
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(soundPath, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", soundPath, behaviour.name)
				});
			}
			this.lastPlayedLoopingSoundPath = soundPath;
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x000A7834 File Offset: 0x000A5A34
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null && this.lastPlayedLoopingSoundPath != null)
			{
				component.StopSound(this.lastPlayedLoopingSoundPath);
			}
		}
		this.lastPlayedLoopingSoundPath = null;
	}

	// Token: 0x040011F2 RID: 4594
	private string actualSoundName;

	// Token: 0x040011F3 RID: 4595
	private string lastPlayedLoopingSoundPath;
}
