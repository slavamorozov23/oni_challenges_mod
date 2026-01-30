using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000675 RID: 1653
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002872 RID: 10354 RVA: 0x000E8D53 File Offset: 0x000E6F53
	public UpdatePercentCompleteParameter() : base("percentComplete")
	{
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x000E8D70 File Offset: 0x000E6F70
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdatePercentCompleteParameter.Entry item = new UpdatePercentCompleteParameter.Entry
		{
			worker = sound.transform.GetComponent<WorkerBase>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000E8DCC File Offset: 0x000E6FCC
	public override void Update(float dt)
	{
		foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
		{
			if (!(entry.worker == null))
			{
				Workable workable = entry.worker.GetWorkable();
				if (!(workable == null))
				{
					float percentComplete = workable.GetPercentComplete();
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, percentComplete, false);
				}
			}
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000E8E5C File Offset: 0x000E705C
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

	// Token: 0x040017C8 RID: 6088
	private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

	// Token: 0x0200154C RID: 5452
	private struct Entry
	{
		// Token: 0x0400716B RID: 29035
		public WorkerBase worker;

		// Token: 0x0400716C RID: 29036
		public EventInstance ev;

		// Token: 0x0400716D RID: 29037
		public PARAMETER_ID parameterId;
	}
}
