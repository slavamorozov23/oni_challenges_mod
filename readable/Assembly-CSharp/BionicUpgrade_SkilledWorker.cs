using System;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
public class BionicUpgrade_SkilledWorker : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>
{
	// Token: 0x06002BD7 RID: 11223 RVA: 0x000FF7C4 File Offset: 0x000FD9C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.root.Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplySkillPerks)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveSkillPerks)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyModifiers)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveModifiers)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyHats)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveHats));
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.StartWork, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive, null).EventTransition(GameHashes.StopWork, this.Inactive, null).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.CreateFX)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ClearFX));
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000FF95D File Offset: 0x000FDB5D
	public static void ApplySkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.resume.ApplyAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def)smi.def).SkillPerksIds);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000FF97A File Offset: 0x000FDB7A
	public static void RemoveSkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.resume.RemoveAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def)smi.def).SkillPerksIds);
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000FF997 File Offset: 0x000FDB97
	public static void ApplyModifiers(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplyModifiers();
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000FF99F File Offset: 0x000FDB9F
	public static void RemoveModifiers(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveModifiers();
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000FF9A7 File Offset: 0x000FDBA7
	public static void ApplyHats(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplyHats();
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000FF9AF File Offset: 0x000FDBAF
	public static void RemoveHats(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveHats();
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x000FF9B7 File Offset: 0x000FDBB7
	public static bool IsMinionWorkingOnlineAndNotInBatterySaveMode(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore(smi) && BionicUpgrade_SkilledWorker.IsMinionWorkingWithAttribute(smi);
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000FF9D4 File Offset: 0x000FDBD4
	public static bool IsMinionWorkingWithAttribute(BionicUpgrade_SkilledWorker.Instance smi)
	{
		Workable workable = smi.worker.GetWorkable();
		return workable != null && smi.worker.GetState() == WorkerBase.State.Working && workable.GetWorkAttribute() != null && workable.GetWorkAttribute().Id == ((BionicUpgrade_SkilledWorker.Def)smi.def).AttributeId;
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x000FFA2E File Offset: 0x000FDC2E
	public static void CreateFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		BionicUpgrade_SkilledWorker.CreateAndReturnFX(smi);
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000FFA38 File Offset: 0x000FDC38
	public static BionicAttributeUseFx.Instance CreateAndReturnFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new BionicAttributeUseFx.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x000FFA87 File Offset: 0x000FDC87
	public static void ClearFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.fx.sm.destroyFX.Trigger(smi.fx);
		smi.fx = null;
	}

	// Token: 0x020015B2 RID: 5554
	public new class Def : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def
	{
		// Token: 0x0600942D RID: 37933 RVA: 0x00377B97 File Offset: 0x00375D97
		public Def(string upgradeID, string attributeID, AttributeModifier[] modifiers = null, SkillPerk[] skillPerks = null, string[] hats = null) : base(upgradeID)
		{
			this.AttributeId = attributeID;
			this.modifiers = modifiers;
			this.SkillPerksIds = skillPerks;
			this.hats = hats;
		}

		// Token: 0x0600942E RID: 37934 RVA: 0x00377BC0 File Offset: 0x00375DC0
		public override string GetDescription()
		{
			string text = "";
			if (this.SkillPerksIds.Length != 0)
			{
				text += UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_PERKS;
				for (int i = 0; i < this.SkillPerksIds.Length; i++)
				{
					text += "\n";
					text += SkillPerk.GetDescription(this.SkillPerksIds[i].Id);
				}
				if (this.modifiers.Length != 0)
				{
					text += "\n\n";
				}
			}
			if (this.modifiers.Length != 0)
			{
				text += UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_ATTRIBUTES;
				for (int j = 0; j < this.modifiers.Length; j++)
				{
					text += "\n";
					text = text + this.modifiers[j].GetName() + ": " + this.modifiers[j].GetFormattedString();
				}
			}
			return text;
		}

		// Token: 0x04007266 RID: 29286
		public SkillPerk[] SkillPerksIds;

		// Token: 0x04007267 RID: 29287
		public string AttributeId;

		// Token: 0x04007268 RID: 29288
		public AttributeModifier[] modifiers;

		// Token: 0x04007269 RID: 29289
		public string[] hats;
	}

	// Token: 0x020015B3 RID: 5555
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance
	{
		// Token: 0x0600942F RID: 37935 RVA: 0x00377C9A File Offset: 0x00375E9A
		public Instance(IStateMachineTarget master, BionicUpgrade_SkilledWorker.Def def) : base(master, def)
		{
		}

		// Token: 0x06009430 RID: 37936 RVA: 0x00377CA4 File Offset: 0x00375EA4
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06009431 RID: 37937 RVA: 0x00377CCC File Offset: 0x00375ECC
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x06009432 RID: 37938 RVA: 0x00377D6C File Offset: 0x00375F6C
		public void ApplyModifiers()
		{
			Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
			foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)base.smi.def).modifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x06009433 RID: 37939 RVA: 0x00377DBC File Offset: 0x00375FBC
		public void RemoveModifiers()
		{
			Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
			foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)base.smi.def).modifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x06009434 RID: 37940 RVA: 0x00377E0C File Offset: 0x0037600C
		public void ApplyHats()
		{
			string[] hats = ((BionicUpgrade_SkilledWorker.Def)base.smi.def).hats;
			if (hats == null)
			{
				return;
			}
			MinionResume component = base.GetComponent<MinionResume>();
			string properName = Assets.GetPrefab(base.smi.def.UpgradeID).GetProperName();
			foreach (string hat in hats)
			{
				component.AddAdditionalHat(properName, hat);
			}
		}

		// Token: 0x06009435 RID: 37941 RVA: 0x00377E80 File Offset: 0x00376080
		public void RemoveHats()
		{
			string[] hats = ((BionicUpgrade_SkilledWorker.Def)base.smi.def).hats;
			if (hats == null)
			{
				return;
			}
			MinionResume component = base.GetComponent<MinionResume>();
			string properName = Assets.GetPrefab(base.smi.def.UpgradeID).GetProperName();
			foreach (string hat in hats)
			{
				component.RemoveAdditionalHat(properName, hat);
			}
		}

		// Token: 0x0400726A RID: 29290
		[MyCmpGet]
		public WorkerBase worker;

		// Token: 0x0400726B RID: 29291
		[MyCmpGet]
		public MinionResume resume;

		// Token: 0x0400726C RID: 29292
		public BionicAttributeUseFx.Instance fx;
	}
}
