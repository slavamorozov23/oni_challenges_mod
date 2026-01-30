using System;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class IdleCellSensor : Sensor
{
	// Token: 0x06001C67 RID: 7271 RVA: 0x0009C580 File Offset: 0x0009A780
	public IdleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x0009C5B0 File Offset: 0x0009A7B0
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60));
		this.navigator.RunQuery(idleCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = idleCellQuery.GetResultCell();
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x0009C62A File Offset: 0x0009A82A
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x040010C0 RID: 4288
	private MinionBrain brain;

	// Token: 0x040010C1 RID: 4289
	private Navigator navigator;

	// Token: 0x040010C2 RID: 4290
	private KPrefabID prefabid;

	// Token: 0x040010C3 RID: 4291
	private int cell;
}
