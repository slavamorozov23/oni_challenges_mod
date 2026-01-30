using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class ChoreConsumerState
{
	// Token: 0x06001A58 RID: 6744 RVA: 0x000915AC File Offset: 0x0008F7AC
	public ChoreConsumerState(ChoreConsumer consumer)
	{
		this.consumer = consumer;
		this.navigator = consumer.GetComponent<Navigator>();
		this.prefabid = consumer.GetComponent<KPrefabID>();
		this.ownable = consumer.GetComponent<Ownable>();
		this.gameObject = consumer.gameObject;
		this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
		this.hasSolidTransferArm = (this.solidTransferArm != null);
		this.resume = consumer.GetComponent<MinionResume>();
		this.choreDriver = consumer.GetComponent<ChoreDriver>();
		this.schedulable = consumer.GetComponent<Schedulable>();
		this.traits = consumer.GetComponent<Traits>();
		this.choreProvider = consumer.GetComponent<ChoreProvider>();
		MinionIdentity component = consumer.GetComponent<MinionIdentity>();
		if (component != null)
		{
			if (component.assignableProxy == null)
			{
				component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, component);
			}
			this.assignables = component.GetSoleOwner();
			this.equipment = component.GetEquipment();
		}
		else
		{
			this.assignables = consumer.GetComponent<Assignables>();
			this.equipment = consumer.GetComponent<Equipment>();
		}
		this.storage = consumer.GetComponent<Storage>();
		this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
		this.worker = consumer.GetComponent<WorkerBase>();
		this.selectable = consumer.GetComponent<KSelectable>();
		if (this.schedulable != null)
		{
			this.scheduleBlock = this.schedulable.GetSchedule().GetCurrentScheduleBlock();
		}
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x00091700 File Offset: 0x0008F900
	public void Refresh()
	{
		if (this.schedulable != null)
		{
			Schedule schedule = this.schedulable.GetSchedule();
			if (schedule != null)
			{
				this.scheduleBlock = schedule.GetCurrentScheduleBlock();
			}
		}
	}

	// Token: 0x04000F1A RID: 3866
	public KPrefabID prefabid;

	// Token: 0x04000F1B RID: 3867
	public GameObject gameObject;

	// Token: 0x04000F1C RID: 3868
	public ChoreConsumer consumer;

	// Token: 0x04000F1D RID: 3869
	public ChoreProvider choreProvider;

	// Token: 0x04000F1E RID: 3870
	public Navigator navigator;

	// Token: 0x04000F1F RID: 3871
	public Ownable ownable;

	// Token: 0x04000F20 RID: 3872
	public Assignables assignables;

	// Token: 0x04000F21 RID: 3873
	public MinionResume resume;

	// Token: 0x04000F22 RID: 3874
	public ChoreDriver choreDriver;

	// Token: 0x04000F23 RID: 3875
	public Schedulable schedulable;

	// Token: 0x04000F24 RID: 3876
	public Traits traits;

	// Token: 0x04000F25 RID: 3877
	public Equipment equipment;

	// Token: 0x04000F26 RID: 3878
	public Storage storage;

	// Token: 0x04000F27 RID: 3879
	public ConsumableConsumer consumableConsumer;

	// Token: 0x04000F28 RID: 3880
	public KSelectable selectable;

	// Token: 0x04000F29 RID: 3881
	public WorkerBase worker;

	// Token: 0x04000F2A RID: 3882
	public SolidTransferArm solidTransferArm;

	// Token: 0x04000F2B RID: 3883
	public bool hasSolidTransferArm;

	// Token: 0x04000F2C RID: 3884
	public ScheduleBlock scheduleBlock;
}
