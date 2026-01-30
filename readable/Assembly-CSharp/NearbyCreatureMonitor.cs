using System;
using System.Collections.Generic;

// Token: 0x020008AF RID: 2223
public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06003D49 RID: 15689 RVA: 0x00156258 File Offset: 0x00154458
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateNearbyCreatures", delegate(NearbyCreatureMonitor.Instance smi, float dt)
		{
			smi.UpdateNearbyCreatures(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020018B0 RID: 6320
	public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06009FE0 RID: 40928 RVA: 0x003A7F7C File Offset: 0x003A617C
		// (remove) Token: 0x06009FE1 RID: 40929 RVA: 0x003A7FB4 File Offset: 0x003A61B4
		public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

		// Token: 0x06009FE2 RID: 40930 RVA: 0x003A7FE9 File Offset: 0x003A61E9
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009FE3 RID: 40931 RVA: 0x003A7FF4 File Offset: 0x003A61F4
		public void UpdateNearbyCreatures(float dt)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
			if (cavityForCell != null)
			{
				this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
			}
		}
	}
}
