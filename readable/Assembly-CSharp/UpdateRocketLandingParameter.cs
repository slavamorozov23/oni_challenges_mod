using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000B93 RID: 2963
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600587A RID: 22650 RVA: 0x00201C53 File Offset: 0x001FFE53
	public UpdateRocketLandingParameter() : base("rocketLanding")
	{
	}

	// Token: 0x0600587B RID: 22651 RVA: 0x00201C70 File Offset: 0x001FFE70
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketLandingParameter.Entry item = new UpdateRocketLandingParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x00201CCC File Offset: 0x001FFECC
	public override void Update(float dt)
	{
		foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						if (component.isLanding)
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 1f, false);
						}
						else
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 0f, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x00201D88 File Offset: 0x001FFF88
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

	// Token: 0x04003B65 RID: 15205
	private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

	// Token: 0x02001D19 RID: 7449
	private struct Entry
	{
		// Token: 0x04008A53 RID: 35411
		public RocketModule rocketModule;

		// Token: 0x04008A54 RID: 35412
		public EventInstance ev;

		// Token: 0x04008A55 RID: 35413
		public PARAMETER_ID parameterId;
	}
}
