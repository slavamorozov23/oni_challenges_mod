using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000B94 RID: 2964
internal class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600587E RID: 22654 RVA: 0x00201DE0 File Offset: 0x001FFFE0
	public UpdateRocketSpeedParameter() : base("rocketSpeed")
	{
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x00201E00 File Offset: 0x00200000
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketSpeedParameter.Entry item = new UpdateRocketSpeedParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06005880 RID: 22656 RVA: 0x00201E5C File Offset: 0x0020005C
	public override void Update(float dt)
	{
		foreach (UpdateRocketSpeedParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						EventInstance ev = entry.ev;
						ev.setParameterByID(entry.parameterId, component.rocketSpeed, false);
					}
				}
			}
		}
	}

	// Token: 0x06005881 RID: 22657 RVA: 0x00201EF4 File Offset: 0x002000F4
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

	// Token: 0x04003B66 RID: 15206
	private List<UpdateRocketSpeedParameter.Entry> entries = new List<UpdateRocketSpeedParameter.Entry>();

	// Token: 0x02001D1A RID: 7450
	private struct Entry
	{
		// Token: 0x04008A56 RID: 35414
		public RocketModule rocketModule;

		// Token: 0x04008A57 RID: 35415
		public EventInstance ev;

		// Token: 0x04008A58 RID: 35416
		public PARAMETER_ID parameterId;
	}
}
