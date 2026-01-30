using System;
using System.Collections.Generic;

// Token: 0x0200055F RID: 1375
public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000A6299 File Offset: 0x000A4499
	public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x000A62A0 File Offset: 0x000A44A0
	public void AddVolume(string animFile, string eventName, EffectorValues vals)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			this.volumeCache.Add(key, vals);
			return;
		}
		this.volumeCache[key] = vals;
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x000A62EC File Offset: 0x000A44EC
	public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

	// Token: 0x040011DC RID: 4572
	public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
