using System;

// Token: 0x02000571 RID: 1393
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06001F17 RID: 7959 RVA: 0x000A9120 File Offset: 0x000A7320
	public UserVolumeOneShotUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x000A9138 File Offset: 0x000A7338
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		if (!string.IsNullOrEmpty(this.playerPref))
		{
			float @float = KPlayerPrefs.GetFloat(this.playerPref);
			sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), @float, false);
		}
	}

	// Token: 0x04001225 RID: 4645
	private string playerPref;
}
