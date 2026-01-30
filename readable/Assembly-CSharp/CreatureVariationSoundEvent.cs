using System;

// Token: 0x02000548 RID: 1352
public class CreatureVariationSoundEvent : SoundEvent
{
	// Token: 0x06001D45 RID: 7493 RVA: 0x0009F6DF File Offset: 0x0009D8DF
	public CreatureVariationSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
	{
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x0009F6F4 File Offset: 0x0009D8F4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = base.sound;
		CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
		if (component != null && !string.IsNullOrEmpty(component.symbolPrefix))
		{
			string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, base.name), false);
			if (!string.IsNullOrEmpty(sound2))
			{
				sound = sound2;
			}
		}
		base.PlaySound(behaviour, sound);
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x0009F750 File Offset: 0x0009D950
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				string asset = base.sound;
				CreatureBrain component2 = behaviour.GetComponent<CreatureBrain>();
				if (component2 != null && !string.IsNullOrEmpty(component2.symbolPrefix))
				{
					string sound = GlobalAssets.GetSound(StringFormatter.Combine(component2.symbolPrefix, base.name), false);
					if (!string.IsNullOrEmpty(sound))
					{
						asset = sound;
					}
				}
				component.StopSound(asset);
			}
		}
	}
}
