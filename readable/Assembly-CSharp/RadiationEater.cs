using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance>
{
	// Token: 0x06005008 RID: 20488 RVA: 0x001D10EE File Offset: 0x001CF2EE
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001C06 RID: 7174
	public class StatesInstance : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater, object>.GameInstance
	{
		// Token: 0x0600AC3F RID: 44095 RVA: 0x003CB86A File Offset: 0x003C9A6A
		public StatesInstance(RadiationEater master) : base(master)
		{
			this.radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id, TRAITS.RADIATION_EATER_RECOVERY, DUPLICANTS.TRAITS.RADIATIONEATER.NAME, false, false, true);
		}

		// Token: 0x0600AC40 RID: 44096 RVA: 0x003CB8A4 File Offset: 0x003C9AA4
		public void OnEatRads(float radsEaten)
		{
			float delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
			base.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
		}

		// Token: 0x040086C3 RID: 34499
		public AttributeModifier radiationEating;
	}

	// Token: 0x02001C07 RID: 7175
	public class States : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater>
	{
		// Token: 0x0600AC41 RID: 44097 RVA: 0x003CB8F0 File Offset: 0x003C9AF0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier("Radiation Eating", (RadiationEater.StatesInstance smi) => smi.radiationEating, null).EventHandler(GameHashes.RadiationRecovery, delegate(RadiationEater.StatesInstance smi, object data)
			{
				float radsEaten = (float)data;
				smi.OnEatRads(radsEaten);
			});
		}
	}
}
