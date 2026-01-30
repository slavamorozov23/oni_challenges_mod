using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200055D RID: 1373
[Serializable]
public class RemoteSoundEvent : SoundEvent
{
	// Token: 0x06001E7C RID: 7804 RVA: 0x000A5E41 File Offset: 0x000A4041
	public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x000A5E54 File Offset: 0x000A4054
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = behaviour.position;
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		Workable workable = behaviour.GetComponent<WorkerBase>().GetWorkable();
		if (workable != null)
		{
			Toggleable component = workable.GetComponent<Toggleable>();
			if (component != null)
			{
				IToggleHandler toggleHandlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<WorkerBase>());
				float value = 1f;
				if (toggleHandlerForWorker != null && toggleHandlerForWorker.IsHandlerOn())
				{
					value = 0f;
				}
				if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
				{
					EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
					instance.setParameterByName("State", value, false);
					SoundEvent.EndOneShot(instance);
				}
			}
		}
	}

	// Token: 0x040011D8 RID: 4568
	private const string STATE_PARAMETER = "State";
}
