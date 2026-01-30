using System;
using UnityEngine;

// Token: 0x020005BB RID: 1467
public class CreaturePoopLoot : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
	// Token: 0x060021AD RID: 8621 RVA: 0x000C3A4C File Offset: 0x000C1C4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Poop, this.roll, null);
		this.roll.Enter(new StateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State.Callback(CreaturePoopLoot.RollForLoot)).GoTo(this.idle);
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000C3AA4 File Offset: 0x000C1CA4
	public static void RollForLoot(CreaturePoopLoot.Instance smi)
	{
		for (int i = 0; i < smi.def.Loot.Length; i++)
		{
			float value = UnityEngine.Random.value;
			CreaturePoopLoot.LootData lootData = smi.def.Loot[i];
			if (lootData.probability > 0f && value <= lootData.probability)
			{
				Tag tag = lootData.tag;
				Vector3 position = smi.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(true);
			}
		}
	}

	// Token: 0x040013A1 RID: 5025
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State idle;

	// Token: 0x040013A2 RID: 5026
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State roll;

	// Token: 0x02001461 RID: 5217
	public struct LootData
	{
		// Token: 0x04006E73 RID: 28275
		public Tag tag;

		// Token: 0x04006E74 RID: 28276
		public float probability;
	}

	// Token: 0x02001462 RID: 5218
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E75 RID: 28277
		public CreaturePoopLoot.LootData[] Loot;
	}

	// Token: 0x02001463 RID: 5219
	public new class Instance : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.GameInstance
	{
		// Token: 0x06008FB1 RID: 36785 RVA: 0x0036C8D1 File Offset: 0x0036AAD1
		public Instance(IStateMachineTarget master, CreaturePoopLoot.Def def) : base(master, def)
		{
		}
	}
}
