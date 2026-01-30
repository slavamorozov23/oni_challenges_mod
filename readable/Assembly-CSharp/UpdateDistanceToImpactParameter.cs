using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200086A RID: 2154
internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06003B2E RID: 15150 RVA: 0x0014AD01 File Offset: 0x00148F01
	public UpdateDistanceToImpactParameter() : base("distanceToImpact")
	{
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x0014AD20 File Offset: 0x00148F20
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateDistanceToImpactParameter.Entry item = new UpdateDistanceToImpactParameter.Entry
		{
			comet = sound.transform.GetComponent<Comet>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0014AD7C File Offset: 0x00148F7C
	public override void Update(float dt)
	{
		foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
		{
			if (!(entry.comet == null))
			{
				float soundDistance = entry.comet.GetSoundDistance();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, soundDistance, false);
			}
		}
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0014ADFC File Offset: 0x00148FFC
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

	// Token: 0x04002423 RID: 9251
	private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

	// Token: 0x02001825 RID: 6181
	private struct Entry
	{
		// Token: 0x040079EC RID: 31212
		public Comet comet;

		// Token: 0x040079ED RID: 31213
		public EventInstance ev;

		// Token: 0x040079EE RID: 31214
		public PARAMETER_ID parameterId;
	}
}
