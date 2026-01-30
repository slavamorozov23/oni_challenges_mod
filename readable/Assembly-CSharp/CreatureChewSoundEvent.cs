using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class CreatureChewSoundEvent : SoundEvent
{
	// Token: 0x06001D41 RID: 7489 RVA: 0x0009F5B7 File Offset: 0x0009D7B7
	public CreatureChewSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x0009F5C8 File Offset: 0x0009D7C8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = GlobalAssets.GetSound(StringFormatter.Combine(base.name, "_", CreatureChewSoundEvent.GetChewSound(behaviour)), false);
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic))
		{
			Vector3 vector = behaviour.position;
			vector.z = 0f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			EventInstance instance = SoundEvent.BeginOneShot(sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
			if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
			{
				instance.setParameterByName("isBaby", 1f, false);
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x0009F690 File Offset: 0x0009D890
	private static string GetChewSound(AnimEventManager.EventPlayerData behaviour)
	{
		string result = CreatureChewSoundEvent.DEFAULT_CHEW_SOUND;
		EatStates.Instance smi = behaviour.controller.GetSMI<EatStates.Instance>();
		if (smi != null)
		{
			Element latestMealElement = smi.GetLatestMealElement();
			if (latestMealElement != null)
			{
				string creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
				if (!string.IsNullOrEmpty(creatureChewSound))
				{
					result = creatureChewSound;
				}
			}
		}
		return result;
	}

	// Token: 0x0400112E RID: 4398
	private static string DEFAULT_CHEW_SOUND = "Rock";

	// Token: 0x0400112F RID: 4399
	private const string FMOD_PARAM_IS_BABY_ID = "isBaby";
}
