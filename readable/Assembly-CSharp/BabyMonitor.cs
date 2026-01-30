using System;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000886 RID: 2182
public class BabyMonitor : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>
{
	// Token: 0x06003C17 RID: 15383 RVA: 0x00150838 File Offset: 0x0014EA38
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.baby;
		this.root.Enter(new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State.Callback(BabyMonitor.AddBabyEffect));
		this.baby.Transition(this.spawnadult, new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.Transition.ConditionCallback(BabyMonitor.IsReadyToSpawnAdult), UpdateRate.SIM_4000ms);
		this.spawnadult.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, (BabyMonitor.Instance smi) => true, null);
		this.babyEffect = new Effect("IsABaby", CREATURES.MODIFIERS.BABY.NAME, CREATURES.MODIFIERS.BABY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, CREATURES.MODIFIERS.BABY.NAME, true, false, true));
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, CREATURES.MODIFIERS.BABY.NAME, false, false, true));
	}

	// Token: 0x06003C18 RID: 15384 RVA: 0x0015095E File Offset: 0x0014EB5E
	private static void AddBabyEffect(BabyMonitor.Instance smi)
	{
		smi.Get<Effects>().Add(smi.sm.babyEffect, false);
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x00150978 File Offset: 0x0014EB78
	private static bool IsReadyToSpawnAdult(BabyMonitor.Instance smi)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(smi.gameObject);
		float num = smi.def.adultThreshold;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 0.005f;
		}
		return amountInstance.value > num;
	}

	// Token: 0x04002511 RID: 9489
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State baby;

	// Token: 0x04002512 RID: 9490
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State spawnadult;

	// Token: 0x04002513 RID: 9491
	public Effect babyEffect;

	// Token: 0x0200184E RID: 6222
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007A8D RID: 31373
		public Tag adultPrefab;

		// Token: 0x04007A8E RID: 31374
		public string onGrowDropID;

		// Token: 0x04007A8F RID: 31375
		public float onGrowDropUnits = 1f;

		// Token: 0x04007A90 RID: 31376
		public bool forceAdultNavType;

		// Token: 0x04007A91 RID: 31377
		public float adultThreshold = 5f;

		// Token: 0x04007A92 RID: 31378
		public Action<GameObject> configureAdultOnMaturation;
	}

	// Token: 0x0200184F RID: 6223
	public new class Instance : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.GameInstance
	{
		// Token: 0x06009E83 RID: 40579 RVA: 0x003A3568 File Offset: 0x003A1768
		public Instance(IStateMachineTarget master, BabyMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009E84 RID: 40580 RVA: 0x003A3574 File Offset: 0x003A1774
		public void SpawnAdult()
		{
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.smi.def.adultPrefab), position);
			gameObject.SetActive(true);
			if (!base.smi.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			}
			if (base.smi.def.onGrowDropID != null)
			{
				GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(base.smi.def.onGrowDropID), position);
				gameObject2.GetComponent<PrimaryElement>().Mass *= base.smi.def.onGrowDropUnits;
				gameObject2.SetActive(true);
			}
			foreach (AmountInstance amountInstance in base.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			EffectInstance effectInstance = base.gameObject.GetComponent<Effects>().Get("AteFromFeeder");
			if (effectInstance != null)
			{
				gameObject.GetComponent<Effects>().Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			if (!base.smi.def.forceAdultNavType)
			{
				Navigator component = base.smi.GetComponent<Navigator>();
				gameObject.GetComponent<Navigator>().SetCurrentNavType(component.CurrentNavType);
			}
			gameObject.Trigger(-2027483228, base.gameObject);
			KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component2)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.gameObject.Trigger(663420073, gameObject);
			base.smi.gameObject.DeleteObject();
			if (base.def.configureAdultOnMaturation != null)
			{
				base.def.configureAdultOnMaturation(gameObject);
			}
		}
	}
}
