using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class FartChore : Chore<FartChore.StatesInstance>
{
	// Token: 0x06001928 RID: 6440 RVA: 0x0008C5FC File Offset: 0x0008A7FC
	public FartChore(IStateMachineTarget target, ChoreType chore_type, float mass, SimHashes element_id, byte disease_idx, int disease_count, float overpressureThreshold) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FartChore.StatesInstance(this, target.gameObject);
		this.mass = mass;
		this.element_id = element_id;
		this.disease_idx = disease_idx;
		this.disease_count = disease_count;
		this.overpressureThreshold = overpressureThreshold;
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x0008C65C File Offset: 0x0008A85C
	private bool CheckIsOverpressure(int cell)
	{
		return Grid.Mass[cell] > this.overpressureThreshold;
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x0008C671 File Offset: 0x0008A871
	public static void CreateEmission(FartChore.StatesInstance smi)
	{
		smi.master.DoFart();
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0008C680 File Offset: 0x0008A880
	public void DoFart()
	{
		if (this.mass <= 0f)
		{
			return;
		}
		Element element = ElementLoader.FindElementByHash(this.element_id);
		float temperature = base.smi.master.GetComponent<PrimaryElement>().Temperature;
		if (element.IsGas || element.IsLiquid)
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			if (this.CheckIsOverpressure(num))
			{
				return;
			}
			SimMessages.AddRemoveSubstance(num, this.element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, this.mass, temperature, this.disease_idx, this.disease_count, true, -1);
		}
		else if (element.IsSolid)
		{
			element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), this.mass, temperature, this.disease_idx, this.disease_count, false, true, false);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, this.gameObject.transform, 1.5f, false);
	}

	// Token: 0x04000EAE RID: 3758
	private float mass;

	// Token: 0x04000EAF RID: 3759
	private SimHashes element_id;

	// Token: 0x04000EB0 RID: 3760
	private byte disease_idx;

	// Token: 0x04000EB1 RID: 3761
	private int disease_count;

	// Token: 0x04000EB2 RID: 3762
	private float overpressureThreshold;

	// Token: 0x020012D9 RID: 4825
	public class StatesInstance : GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.GameInstance
	{
		// Token: 0x060089D4 RID: 35284 RVA: 0x00354C67 File Offset: 0x00352E67
		public StatesInstance(FartChore master, GameObject farter) : base(master)
		{
			base.sm.farter.Set(farter, base.smi, false);
		}
	}

	// Token: 0x020012DA RID: 4826
	public class States : GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore>
	{
		// Token: 0x060089D5 RID: 35285 RVA: 0x00354C8C File Offset: 0x00352E8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.farter);
			this.root.PlayAnim("fart").ScheduleGoTo(10f, this.finish).OnAnimQueueComplete(this.finish);
			this.finish.Enter(new StateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.State.Callback(FartChore.CreateEmission)).ReturnSuccess();
		}

		// Token: 0x04006956 RID: 26966
		public StateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.TargetParameter farter;

		// Token: 0x04006957 RID: 26967
		public GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.State finish;
	}
}
