using System;

// Token: 0x02000AA4 RID: 2724
public class PlantBranchGrowerBase<StateMachineType, StateMachineInstanceType, MasterType, DefType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance where MasterType : IStateMachineTarget where DefType : PlantBranchGrowerBase<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantBranchGrowerBaseDef
{
	// Token: 0x02001BCD RID: 7117
	public class PlantBranchGrowerBaseDef : StateMachine.BaseDef, IPlantBranchGrower
	{
		// Token: 0x0600AB36 RID: 43830 RVA: 0x003C7F47 File Offset: 0x003C6147
		public string GetPlantBranchPrefabName()
		{
			return this.BRANCH_PREFAB_NAME;
		}

		// Token: 0x0600AB37 RID: 43831 RVA: 0x003C7F4F File Offset: 0x003C614F
		public int GetMaxBranchCount()
		{
			return this.MAX_BRANCH_COUNT;
		}

		// Token: 0x040085C1 RID: 34241
		public int MAX_BRANCH_COUNT;

		// Token: 0x040085C2 RID: 34242
		public string BRANCH_PREFAB_NAME;
	}
}
