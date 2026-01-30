using System;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class MingleCellSensor : Sensor
{
	// Token: 0x06001C6A RID: 7274 RVA: 0x0009C632 File Offset: 0x0009A832
	public MingleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.scheduable = base.GetComponent<Schedulable>();
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x0009C65F File Offset: 0x0009A85F
	public bool IsAllowed()
	{
		return ScheduleManager.Instance.IsAllowed(this.scheduable, Db.Get().ScheduleBlockTypes.Recreation);
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x0009C680 File Offset: 0x0009A880
	public override void Update()
	{
		if (!this.IsAllowed())
		{
			return;
		}
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int, MingleCellSensor>.PooledList pooledList = ListPool<int, MingleCellSensor>.Allocate();
		int num2 = 50;
		foreach (int num3 in Game.Instance.mingleCellTracker.mingleCells)
		{
			if (this.brain.IsCellClear(num3))
			{
				int navigationCost = this.navigator.GetNavigationCost(num3);
				if (navigationCost != -1)
				{
					if (num3 == Grid.InvalidCell || navigationCost < num)
					{
						this.cell = num3;
						num = navigationCost;
					}
					if (navigationCost < num2)
					{
						pooledList.Add(num3);
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			this.cell = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
		}
		pooledList.Recycle();
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x0009C76C File Offset: 0x0009A96C
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x040010C4 RID: 4292
	private MinionBrain brain;

	// Token: 0x040010C5 RID: 4293
	private Navigator navigator;

	// Token: 0x040010C6 RID: 4294
	private Schedulable scheduable;

	// Token: 0x040010C7 RID: 4295
	private int cell;
}
