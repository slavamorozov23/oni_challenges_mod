using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200070B RID: 1803
[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
	// Token: 0x06002CAB RID: 11435 RVA: 0x00103ED5 File Offset: 0x001020D5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.chore = this.CreateChore();
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x00103EEC File Offset: 0x001020EC
	private Chore CreateChore()
	{
		return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, this, null, true, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x00103F1F File Offset: 0x0010211F
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x00103F35 File Offset: 0x00102135
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker == null;
		return true;
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x00103F40 File Offset: 0x00102140
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.chore != null && !this.chore.isComplete)
		{
			this.chore.Cancel("completed but not complete??");
		}
		this.chore = this.CreateChore();
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x00103F7A File Offset: 0x0010217A
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x00103F90 File Offset: 0x00102190
	public override float GetPercentComplete()
	{
		base.worker == null;
		return 0f;
	}

	// Token: 0x04001A8F RID: 6799
	public float daysToMasterRole;

	// Token: 0x04001A90 RID: 6800
	private Chore chore;

	// Token: 0x04001A91 RID: 6801
	public static Chore.Precondition IsNotMarkedForDeconstruction = new Chore.Precondition
	{
		id = "IsNotMarkedForDeconstruction",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		}
	};
}
