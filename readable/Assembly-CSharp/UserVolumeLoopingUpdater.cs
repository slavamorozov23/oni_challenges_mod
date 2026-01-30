using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200056C RID: 1388
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x06001F0F RID: 7951 RVA: 0x000A8F94 File Offset: 0x000A7194
	public UserVolumeLoopingUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000A8FB4 File Offset: 0x000A71B4
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UserVolumeLoopingUpdater.Entry item = new UserVolumeLoopingUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000A9000 File Offset: 0x000A7200
	public override void Update(float dt)
	{
		if (string.IsNullOrEmpty(this.playerPref))
		{
			return;
		}
		float @float = KPlayerPrefs.GetFloat(this.playerPref);
		foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, @float, false);
		}
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000A9080 File Offset: 0x000A7280
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04001223 RID: 4643
	private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();

	// Token: 0x04001224 RID: 4644
	private string playerPref;

	// Token: 0x02001404 RID: 5124
	private struct Entry
	{
		// Token: 0x04006D1C RID: 27932
		public EventInstance ev;

		// Token: 0x04006D1D RID: 27933
		public PARAMETER_ID parameterId;
	}
}
