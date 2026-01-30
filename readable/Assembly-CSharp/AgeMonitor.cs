using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000884 RID: 2180
public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
	// Token: 0x06003C0D RID: 15373 RVA: 0x0015069C File Offset: 0x0014E89C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		this.alive.ToggleAttributeModifier("Aging", (AgeMonitor.Instance smi) => this.aging, null).Transition(this.time_to_die, new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie), UpdateRate.SIM_1000ms).Update(new Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem), UpdateRate.SIM_1000ms, false);
		this.time_to_die.Enter(new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die));
		this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f, CREATURES.MODIFIERS.AGE.NAME, false, false, true);
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x00150748 File Offset: 0x0014E948
	private static void Die(AgeMonitor.Instance smi)
	{
		smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x00150764 File Offset: 0x0014E964
	private static bool TimeToDie(AgeMonitor.Instance smi)
	{
		return smi.age.value >= smi.age.GetMax();
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00150784 File Offset: 0x0014E984
	private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
	{
		bool show = smi.age.value > smi.age.GetMax() * 0.9f;
		smi.oldStatusGuid = smi.kselectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Old, smi.oldStatusGuid, show, smi);
	}

	// Token: 0x0400250E RID: 9486
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;

	// Token: 0x0400250F RID: 9487
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;

	// Token: 0x04002510 RID: 9488
	private AttributeModifier aging;

	// Token: 0x0200184A RID: 6218
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009E78 RID: 40568 RVA: 0x003A33AD File Offset: 0x003A15AD
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
		}

		// Token: 0x04007A87 RID: 31367
		public float minAgePercentOnSpawn;

		// Token: 0x04007A88 RID: 31368
		public float maxAgePercentOnSpawn = 0.75f;
	}

	// Token: 0x0200184B RID: 6219
	public new class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
	{
		// Token: 0x06009E7A RID: 40570 RVA: 0x003A33E8 File Offset: 0x003A15E8
		public Instance(IStateMachineTarget master, AgeMonitor.Def def) : base(master, def)
		{
			this.age = Db.Get().Amounts.Age.Lookup(base.gameObject);
			base.Subscribe(1119167081, delegate(object data)
			{
				this.RandomizeAge();
			});
		}

		// Token: 0x06009E7B RID: 40571 RVA: 0x003A3438 File Offset: 0x003A1638
		public void RandomizeAge()
		{
			this.age.value = Mathf.Lerp(this.age.GetMax() * base.def.minAgePercentOnSpawn, this.age.GetMax() * base.def.maxAgePercentOnSpawn, UnityEngine.Random.value);
			AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = this.age.value / this.age.GetMax() * amountInstance.GetMax() * 1.75f;
				amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06009E7C RID: 40572 RVA: 0x003A34ED File Offset: 0x003A16ED
		public float CyclesUntilDeath
		{
			get
			{
				return this.age.GetMax() - this.age.value;
			}
		}

		// Token: 0x04007A89 RID: 31369
		public AmountInstance age;

		// Token: 0x04007A8A RID: 31370
		public Guid oldStatusGuid;

		// Token: 0x04007A8B RID: 31371
		[MyCmpReq]
		public KSelectable kselectable;
	}
}
