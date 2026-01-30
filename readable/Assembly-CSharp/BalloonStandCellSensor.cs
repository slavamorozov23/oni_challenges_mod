using System;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class BalloonStandCellSensor : Sensor
{
	// Token: 0x06001C51 RID: 7249 RVA: 0x0009BCEE File Offset: 0x00099EEE
	public BalloonStandCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.scheduable = base.GetComponent<Schedulable>();
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x0009BD1B File Offset: 0x00099F1B
	public bool IsAllowed()
	{
		return ScheduleManager.Instance.IsAllowed(this.scheduable, Db.Get().ScheduleBlockTypes.Recreation);
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x0009BD3C File Offset: 0x00099F3C
	public override void Update()
	{
		if (!this.IsAllowed())
		{
			return;
		}
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int[], BalloonStandCellSensor>.PooledList pooledList = ListPool<int[], BalloonStandCellSensor>.Allocate();
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
						int num4 = Grid.CellRight(num3);
						int num5 = Grid.CellRight(num4);
						int num6 = Grid.CellLeft(num3);
						int num7 = Grid.CellLeft(num6);
						CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cell);
						CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(num7);
						CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(num5);
						if (cavityForCell != null)
						{
							if (cavityForCell3 != null && cavityForCell3.handle == cavityForCell.handle && this.navigator.NavGrid.NavTable.IsValid(num4, NavType.Floor) && this.navigator.NavGrid.NavTable.IsValid(num5, NavType.Floor))
							{
								pooledList.Add(new int[]
								{
									num3,
									num5
								});
							}
							if (cavityForCell2 != null && cavityForCell2.handle == cavityForCell.handle && this.navigator.NavGrid.NavTable.IsValid(num6, NavType.Floor) && this.navigator.NavGrid.NavTable.IsValid(num7, NavType.Floor))
							{
								pooledList.Add(new int[]
								{
									num3,
									num7
								});
							}
						}
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			int[] array = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
			this.cell = array[0];
			this.standCell = array[1];
		}
		else if (Components.Telepads.Count > 0)
		{
			Telepad telepad = Components.Telepads.Items[0];
			if (telepad == null || !telepad.GetComponent<Operational>().IsOperational)
			{
				return;
			}
			int num8 = Grid.PosToCell(telepad.transform.GetPosition());
			num8 = Grid.CellLeft(num8);
			int num9 = Grid.CellRight(num8);
			int num10 = Grid.CellRight(num9);
			bool cavityForCell4 = Game.Instance.roomProber.GetCavityForCell(num8) != null;
			CavityInfo cavityForCell5 = Game.Instance.roomProber.GetCavityForCell(num10);
			if (cavityForCell4 && cavityForCell5 != null && this.navigator.GetNavigationCost(num8) != -1 && this.navigator.GetNavigationCost(num9) != -1 && this.navigator.GetNavigationCost(num10) != -1)
			{
				this.cell = num8;
				this.standCell = num10;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x0009C050 File Offset: 0x0009A250
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x0009C058 File Offset: 0x0009A258
	public int GetStandCell()
	{
		return this.standCell;
	}

	// Token: 0x040010AA RID: 4266
	private MinionBrain brain;

	// Token: 0x040010AB RID: 4267
	private Navigator navigator;

	// Token: 0x040010AC RID: 4268
	private Schedulable scheduable;

	// Token: 0x040010AD RID: 4269
	private int cell;

	// Token: 0x040010AE RID: 4270
	private int standCell;
}
