using System;

// Token: 0x02000557 RID: 1367
public class LaserSoundEvent : SoundEvent
{
	// Token: 0x06001E70 RID: 7792 RVA: 0x000A59FB File Offset: 0x000A3BFB
	public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("LaserSoundEvent", sound_name);
	}
}
