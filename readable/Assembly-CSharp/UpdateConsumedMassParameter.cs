using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020005B9 RID: 1465
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
	// Token: 0x060021A6 RID: 8614 RVA: 0x000C38A7 File Offset: 0x000C1AA7
	public UpdateConsumedMassParameter() : base("consumedMass")
	{
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000C38C4 File Offset: 0x000C1AC4
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateConsumedMassParameter.Entry item = new UpdateConsumedMassParameter.Entry
		{
			creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000C3920 File Offset: 0x000C1B20
	public override void Update(float dt)
	{
		foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
		{
			if (!entry.creatureCalorieMonitor.IsNullOrStopped())
			{
				float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, fullness, false);
			}
		}
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000C39A4 File Offset: 0x000C1BA4
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

	// Token: 0x0400139D RID: 5021
	private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

	// Token: 0x0200145D RID: 5213
	private struct Entry
	{
		// Token: 0x04006E67 RID: 28263
		public CreatureCalorieMonitor.Instance creatureCalorieMonitor;

		// Token: 0x04006E68 RID: 28264
		public EventInstance ev;

		// Token: 0x04006E69 RID: 28265
		public PARAMETER_ID parameterId;
	}
}
