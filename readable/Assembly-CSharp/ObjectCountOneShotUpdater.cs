using System;
using System.Collections.Generic;

// Token: 0x02000562 RID: 1378
internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06001EB3 RID: 7859 RVA: 0x000A6D1F File Offset: 0x000A4F1F
	public ObjectCountOneShotUpdater() : base("objectCount")
	{
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000A6D3C File Offset: 0x000A4F3C
	public override void Update(float dt)
	{
		this.soundCounts.Clear();
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000A6D4C File Offset: 0x000A4F4C
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		int num = 0;
		this.soundCounts.TryGetValue(sound.path, out num);
		num = (this.soundCounts[sound.path] = num + 1);
		UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
	}

	// Token: 0x040011EA RID: 4586
	private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
}
