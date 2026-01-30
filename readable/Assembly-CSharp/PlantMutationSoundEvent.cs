using System;
using UnityEngine;

// Token: 0x0200055C RID: 1372
public class PlantMutationSoundEvent : SoundEvent
{
	// Token: 0x06001E7A RID: 7802 RVA: 0x000A5DD4 File Offset: 0x000A3FD4
	public PlantMutationSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000A5DE4 File Offset: 0x000A3FE4
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
		Vector3 position = behaviour.position;
		if (component != null)
		{
			for (int i = 0; i < component.GetSoundEvents().Count; i++)
			{
				SoundEvent.PlayOneShot(component.GetSoundEvents()[i], position, 1f);
			}
		}
	}
}
