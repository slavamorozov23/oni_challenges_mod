using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TemplateClasses;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor, IManageGrowingStates
{
	// Token: 0x06003CF9 RID: 15609 RVA: 0x00154770 File Offset: 0x00152970
	protected override void OnPrefabInit()
	{
		Amounts amounts = base.gameObject.GetAmounts();
		this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
		this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
		this.oldAge.maxAttribute.ClearModifiers();
		this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, null, false, false, true));
		base.OnPrefabInit();
		base.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
		base.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x0015483A File Offset: 0x00152A3A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.gameObject.AddTag(GameTags.GrowingPlant);
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x00154860 File Offset: 0x00152A60
	private void OnNewGameSpawn(object data)
	{
		Prefab prefab = (Prefab)data;
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				if (template_amount_value.id == this.maturity.amount.Id && template_amount_value.value == this.GetMaxMaturity())
				{
					return;
				}
			}
		}
		if (this.maturity == null)
		{
			KCrashReporter.ReportDevNotification("Maturity.OnNewGameSpawn", Environment.StackTrace, "", false, null);
		}
		this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * this.MaxMaturityValuePercentageToSpawnWith * UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x00154914 File Offset: 0x00152B14
	public void OverrideMaturityLevel(float percent)
	{
		float value = this.maturity.GetMax() * percent;
		this.maturity.SetValue(value);
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x0015493C File Offset: 0x00152B3C
	public bool ReachedNextHarvest()
	{
		return this.PercentOfCurrentHarvest() >= 1f;
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x0015494E File Offset: 0x00152B4E
	public bool IsGrown()
	{
		return this.maturity.value == this.maturity.GetMax();
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00154968 File Offset: 0x00152B68
	public bool CanGrow()
	{
		return !this.IsGrown();
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00154973 File Offset: 0x00152B73
	public bool IsGrowing()
	{
		return this.maturity.GetDelta() > 0f;
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00154987 File Offset: 0x00152B87
	public void ClampGrowthToHarvest()
	{
		this.maturity.value = this.maturity.GetMax();
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x0015499F File Offset: 0x00152B9F
	public float GetMaxMaturity()
	{
		return this.maturity.GetMax();
	}

	// Token: 0x06003D03 RID: 15619 RVA: 0x001549AC File Offset: 0x00152BAC
	public float PercentOfCurrentHarvest()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x001549C5 File Offset: 0x00152BC5
	public float TimeUntilNextHarvest()
	{
		return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x001549EA File Offset: 0x00152BEA
	public float DomesticGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x00154A08 File Offset: 0x00152C08
	public float WildGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x00154A26 File Offset: 0x00152C26
	public float PercentGrown()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x00154A3F File Offset: 0x00152C3F
	public void ResetGrowth(object data = null)
	{
		this.maturity.value = 0f;
	}

	// Token: 0x06003D09 RID: 15625 RVA: 0x00154A51 File Offset: 0x00152C51
	public float PercentOldAge()
	{
		if (!this.shouldGrowOld)
		{
			return 0f;
		}
		return this.oldAge.value / this.oldAge.GetMax();
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x00154A78 File Offset: 0x00152C78
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
		list.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x00154AC4 File Offset: 0x00152CC4
	public void ConsumeMass(float mass_to_consume)
	{
		float value = this.maturity.value;
		mass_to_consume = Mathf.Min(mass_to_consume, value);
		this.maturity.value = this.maturity.value - mass_to_consume;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06003D0C RID: 15628 RVA: 0x00154B10 File Offset: 0x00152D10
	public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
	{
		float num = units_to_consume / unit_maturity_ratio;
		global::Debug.Assert(num <= this.maturity.value);
		this.maturity.value -= num;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06003D0D RID: 15629 RVA: 0x00154B5B File Offset: 0x00152D5B
	public Crop GetCropComponent()
	{
		return base.GetComponent<Crop>();
	}

	// Token: 0x06003D0E RID: 15630 RVA: 0x00154B63 File Offset: 0x00152D63
	public bool IsWildPlanted()
	{
		return !this.rm.Replanted;
	}

	// Token: 0x0400259F RID: 9631
	public Func<GameObject, bool> CustomGrowStallCondition_IsStalled;

	// Token: 0x040025A0 RID: 9632
	public float MaxMaturityValuePercentageToSpawnWith = 1f;

	// Token: 0x040025A1 RID: 9633
	public float GROWTH_RATE = 0.0016666667f;

	// Token: 0x040025A2 RID: 9634
	public float WILD_GROWTH_RATE = 0.00041666668f;

	// Token: 0x040025A3 RID: 9635
	public bool shouldGrowOld = true;

	// Token: 0x040025A4 RID: 9636
	public float maxAge = 2400f;

	// Token: 0x040025A5 RID: 9637
	private AmountInstance maturity;

	// Token: 0x040025A6 RID: 9638
	private AmountInstance oldAge;

	// Token: 0x040025A7 RID: 9639
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x040025A8 RID: 9640
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040025A9 RID: 9641
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x040025AA RID: 9642
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x040025AB RID: 9643
	private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x040025AC RID: 9644
	private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.ResetGrowth(data);
	});

	// Token: 0x02001897 RID: 6295
	public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
	{
		// Token: 0x06009F83 RID: 40835 RVA: 0x003A6920 File Offset: 0x003A4B20
		public StatesInstance(Growing master) : base(master)
		{
			this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0f, null, false, false, true);
			this.harvestable = base.GetComponent<Harvestable>();
		}

		// Token: 0x06009F84 RID: 40836 RVA: 0x003A69CF File Offset: 0x003A4BCF
		public bool IsGrown()
		{
			return base.master.IsGrown();
		}

		// Token: 0x06009F85 RID: 40837 RVA: 0x003A69DC File Offset: 0x003A4BDC
		public bool ReachedNextHarvest()
		{
			return base.master.ReachedNextHarvest();
		}

		// Token: 0x06009F86 RID: 40838 RVA: 0x003A69E9 File Offset: 0x003A4BE9
		public void ClampGrowthToHarvest()
		{
			base.master.ClampGrowthToHarvest();
		}

		// Token: 0x06009F87 RID: 40839 RVA: 0x003A69F6 File Offset: 0x003A4BF6
		public bool IsWilting()
		{
			return base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}

		// Token: 0x06009F88 RID: 40840 RVA: 0x003A6A20 File Offset: 0x003A4C20
		public bool IsStalledByCustomCondition()
		{
			bool result = false;
			if (base.master.CustomGrowStallCondition_IsStalled != null)
			{
				result = base.master.CustomGrowStallCondition_IsStalled(base.master.gameObject);
			}
			return result;
		}

		// Token: 0x06009F89 RID: 40841 RVA: 0x003A6A59 File Offset: 0x003A4C59
		public bool CanExitStalled()
		{
			return !this.IsWilting() && (base.master.CustomGrowStallCondition_IsStalled == null || !base.master.CustomGrowStallCondition_IsStalled(base.master.gameObject));
		}

		// Token: 0x04007B66 RID: 31590
		public AttributeModifier baseGrowingRate;

		// Token: 0x04007B67 RID: 31591
		public AttributeModifier wildGrowingRate;

		// Token: 0x04007B68 RID: 31592
		public AttributeModifier getOldRate;

		// Token: 0x04007B69 RID: 31593
		public Harvestable harvestable;
	}

	// Token: 0x02001898 RID: 6296
	public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
	{
		// Token: 0x06009F8A RID: 40842 RVA: 0x003A6A94 File Offset: 0x003A4C94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.growing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.growing.EventTransition(GameHashes.Wilt, this.stalled, (Growing.StatesInstance smi) => smi.IsWilting()).EventTransition(GameHashes.CropSleep, this.stalled, (Growing.StatesInstance smi) => smi.IsStalledByCustomCondition()).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.planted, (Growing.StatesInstance smi) => !smi.master.IsWildPlanted()).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.wild, (Growing.StatesInstance smi) => smi.master.IsWildPlanted()).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (Growing.StatesInstance smi) => !smi.master.IsWildPlanted()).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (Growing.StatesInstance smi) => smi.master.IsWildPlanted()).TriggerOnEnter(GameHashes.Grow, null).Update("CheckGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.ReachedNextHarvest())
				{
					smi.GoTo(this.grown);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Growing.StatesInstance smi) => smi.master.GetComponent<IManageGrowingStates>()).Enter(delegate(Growing.StatesInstance smi)
			{
				GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.IsWildPlanted() ? this.growing.wild : this.growing.planted;
				smi.GoTo(state);
			});
			this.growing.wild.ToggleAttributeModifier("GrowingWild", (Growing.StatesInstance smi) => smi.wildGrowingRate, null);
			this.growing.planted.ToggleAttributeModifier("Growing", (Growing.StatesInstance smi) => smi.baseGrowingRate, null);
			this.stalled.EventTransition(GameHashes.WiltRecover, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled()).EventTransition(GameHashes.CropWakeUp, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled());
			this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, null).Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (!smi.ReachedNextHarvest())
				{
					smi.GoTo(this.growing);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Growing.StatesInstance smi) => smi.getOldRate, null).Enter(delegate(Growing.StatesInstance smi)
			{
				smi.ClampGrowthToHarvest();
			}).Exit(delegate(Growing.StatesInstance smi)
			{
				smi.master.oldAge.SetValue(0f);
			});
			this.grown.idle.Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.master.shouldGrowOld && smi.master.oldAge.value >= smi.master.oldAge.GetMax() && smi.harvestable && smi.harvestable.CanBeHarvested)
				{
					if (smi.harvestable.harvestDesignatable != null)
					{
						bool harvestWhenReady = smi.harvestable.harvestDesignatable.HarvestWhenReady;
						smi.harvestable.ForceCancelHarvest(null);
						smi.harvestable.Harvest();
						if (harvestWhenReady && smi.harvestable != null)
						{
							smi.harvestable.harvestDesignatable.SetHarvestWhenReady(true);
						}
					}
					else
					{
						smi.harvestable.ForceCancelHarvest(null);
						smi.harvestable.Harvest();
					}
					smi.master.maturity.SetValue(0f);
					smi.master.oldAge.SetValue(0f);
				}
			}, UpdateRate.SIM_4000ms, false);
		}

		// Token: 0x04007B6A RID: 31594
		public Growing.States.GrowingStates growing;

		// Token: 0x04007B6B RID: 31595
		public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;

		// Token: 0x04007B6C RID: 31596
		public Growing.States.GrownStates grown;

		// Token: 0x0200298B RID: 10635
		public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x0400B7AE RID: 47022
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;

			// Token: 0x0400B7AF RID: 47023
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
		}

		// Token: 0x0200298C RID: 10636
		public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x0400B7B0 RID: 47024
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;
		}
	}
}
