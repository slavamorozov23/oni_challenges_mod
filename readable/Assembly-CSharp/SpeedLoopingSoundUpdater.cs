using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000E9E RID: 3742
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x060077BD RID: 30653 RVA: 0x002DEB85 File Offset: 0x002DCD85
	public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

	// Token: 0x060077BE RID: 30654 RVA: 0x002DEBA4 File Offset: 0x002DCDA4
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x060077BF RID: 30655 RVA: 0x002DEBF0 File Offset: 0x002DCDF0
	public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

	// Token: 0x060077C0 RID: 30656 RVA: 0x002DEC5C File Offset: 0x002DCE5C
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

	// Token: 0x060077C1 RID: 30657 RVA: 0x002DECB4 File Offset: 0x002DCEB4
	public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

	// Token: 0x04005341 RID: 21313
	private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

	// Token: 0x02002109 RID: 8457
	private struct Entry
	{
		// Token: 0x040097E3 RID: 38883
		public EventInstance ev;

		// Token: 0x040097E4 RID: 38884
		public PARAMETER_ID parameterId;
	}
}
