using System;

// Token: 0x02000E9F RID: 3743
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x060077C2 RID: 30658 RVA: 0x002DECC1 File Offset: 0x002DCEC1
	public SpeedOneShotUpdater() : base("Speed")
	{
	}

	// Token: 0x060077C3 RID: 30659 RVA: 0x002DECD3 File Offset: 0x002DCED3
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue(), false);
	}
}
