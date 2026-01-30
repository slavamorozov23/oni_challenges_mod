using System;
using UnityEngine;

// Token: 0x02000544 RID: 1348
[Serializable]
public class BuildingDamageSoundEvent : SoundEvent
{
	// Token: 0x06001D36 RID: 7478 RVA: 0x0009F017 File Offset: 0x0009D217
	public BuildingDamageSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, false, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x0009F02C File Offset: 0x0009D22C
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 sound_pos = behaviour.position;
		sound_pos.z = 0f;
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible)
		{
			sound_pos = SoundEvent.AudioHighlightListenerPosition(sound_pos);
		}
		WorkerBase component = behaviour.GetComponent<WorkerBase>();
		if (component == null)
		{
			string sound = GlobalAssets.GetSound("Building_Dmg_Metal", false);
			if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic))
			{
				SoundEvent.PlayOneShot(base.sound, sound_pos, SoundEvent.GetVolume(base.objectIsSelectedAndVisible));
				return;
			}
		}
		Workable workable = component.GetWorkable();
		if (workable != null)
		{
			Building component2 = workable.GetComponent<Building>();
			if (component2 != null)
			{
				BuildingDef def = component2.Def;
				string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(base.name, "_", def.AudioCategory), false);
				if (sound2 == null)
				{
					sound2 = GlobalAssets.GetSound("Building_Dmg_Metal", false);
				}
				if (sound2 != null && (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound2, base.looping, this.isDynamic)))
				{
					SoundEvent.PlayOneShot(sound2, sound_pos, SoundEvent.GetVolume(base.objectIsSelectedAndVisible));
				}
			}
		}
	}
}
