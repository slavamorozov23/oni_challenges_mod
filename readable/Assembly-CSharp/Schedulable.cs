using System;
using UnityEngine;

// Token: 0x02000B32 RID: 2866
[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
	// Token: 0x06005464 RID: 21604 RVA: 0x001ED3B4 File Offset: 0x001EB5B4
	public Schedule GetSchedule()
	{
		return ScheduleManager.Instance.GetSchedule(this);
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x001ED3C4 File Offset: 0x001EB5C4
	public bool IsAllowed(ScheduleBlockType schedule_block_type)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Trying to schedule {0} but {1} is not on a valid world. Grid cell: {2}", schedule_block_type.Id, base.gameObject.name, Grid.PosToCell(base.gameObject.GetComponent<KPrefabID>()))
			});
			return false;
		}
		return myWorld.AlertManager.IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x001ED441 File Offset: 0x001EB641
	public void OnScheduleChanged(Schedule schedule)
	{
		base.Trigger(467134493, schedule);
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x001ED44F File Offset: 0x001EB64F
	public void OnScheduleBlocksTick(Schedule schedule)
	{
		base.Trigger(1714332666, schedule);
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x001ED45D File Offset: 0x001EB65D
	public void OnScheduleBlocksChanged(Schedule schedule)
	{
		base.Trigger(-894023145, schedule);
	}
}
