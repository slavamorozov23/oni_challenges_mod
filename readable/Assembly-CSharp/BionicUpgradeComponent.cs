using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006E9 RID: 1769
public class BionicUpgradeComponent : Assignable, IGameObjectEffectDescriptor
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x000FEA8E File Offset: 0x000FCC8E
	// (set) Token: 0x06002BA5 RID: 11173 RVA: 0x000FEA85 File Offset: 0x000FCC85
	public BionicUpgradeComponent.IWattageController WattageController { get; private set; }

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000FEA96 File Offset: 0x000FCC96
	public float CurrentWattage
	{
		get
		{
			if (!this.HasWattageController)
			{
				return 0f;
			}
			return this.WattageController.GetCurrentWattageCost();
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000FEAB1 File Offset: 0x000FCCB1
	public string CurrentWattageName
	{
		get
		{
			if (!this.HasWattageController)
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.GetProperName(), GameUtil.GetFormattedWattage(this.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return this.WattageController.GetCurrentWattageCostName();
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000FEAE9 File Offset: 0x000FCCE9
	public bool HasWattageController
	{
		get
		{
			return this.WattageController != null;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06002BAA RID: 11178 RVA: 0x000FEAF4 File Offset: 0x000FCCF4
	public float PotentialWattage
	{
		get
		{
			return this.data.WattageCost;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000FEB01 File Offset: 0x000FCD01
	public BionicUpgradeComponentConfig.BoosterType Booster
	{
		get
		{
			return this.data.Booster;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000FEB0E File Offset: 0x000FCD0E
	public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
	{
		get
		{
			return this.data.stateMachine;
		}
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x000FEB1C File Offset: 0x000FCD1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.data = BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()];
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_HasAvailableSlots));
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x000FEB70 File Offset: 0x000FCD70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.assignmentManager.Remove(this);
		this.customAssignmentUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForBoosterAssignment);
		this.customAssignablesUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForMinionAssigment);
		base.Subscribe(856640610, new Action<object>(this.RefreshStatusItem));
		this.RefreshStatusItem(null);
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x000FEBD8 File Offset: 0x000FCDD8
	private void RefreshStatusItem(object _ = null)
	{
		if (this.assignee == null && !base.gameObject.HasTag(GameTags.Stored))
		{
			if (this.unassignedStatusItem == Guid.Empty)
			{
				this.unassignedStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.UnassignedBionicBooster, null);
				return;
			}
		}
		else if (this.unassignedStatusItem != Guid.Empty)
		{
			this.unassignedStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.unassignedStatusItem, false);
		}
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x000FEC60 File Offset: 0x000FCE60
	public string GetTooltipForMinionAssigment(Assignables assignables)
	{
		MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
		if (component == null)
		{
			return "ERROR N/A";
		}
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject == null)
		{
			return "ERROR N/A";
		}
		BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			return "This Duplicant cannot install boosters";
		}
		int num = smi.CountBoosterAssignments(this.PrefabID());
		string text = (num == 0) ? string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num) : string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num);
		string text2 = string.Format((smi.AssignedSlotCount < smi.UnlockedSlotCount) ? UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.AVAILABLE_SLOTS : UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NO_AVAILABLE_SLOTS, targetGameObject.GetProperName(), smi.AssignedSlotCount, smi.UnlockedSlotCount);
		string text3 = "";
		List<AttributeInstance> list = new List<AttributeInstance>(targetGameObject.GetAttributes().AttributeTable).FindAll((AttributeInstance a) => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill);
		for (int i = 0; i < list.Count; i++)
		{
			string str = UIConstants.ColorPrefixWhite;
			if (list[i].GetTotalValue() > 0f)
			{
				str = UIConstants.ColorPrefixGreen;
			}
			else if (list[i].GetTotalValue() < 0f)
			{
				str = UIConstants.ColorPrefixRed;
			}
			text3 += string.Format("{0}: {1}", list[i].Name, str + list[i].GetFormattedValue() + UIConstants.ColorSuffix);
			if (i != list.Count - 1)
			{
				text3 += "\n";
			}
		}
		return string.Concat(new string[]
		{
			targetGameObject.GetProperName(),
			"\n\n",
			text,
			"\n\n",
			text2,
			"\n\n",
			text3
		});
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x000FEE6C File Offset: 0x000FD06C
	public string GetTooltipForBoosterAssignment(Assignables assignables)
	{
		MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
		if (component == null)
		{
			return "ERROR N/A";
		}
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject == null)
		{
			return "ERROR N/A";
		}
		BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			return "ERROR N/A";
		}
		int num = smi.CountBoosterAssignments(this.PrefabID());
		string str = (num == 0) ? string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num) : string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num);
		return BionicUpgradeComponentConfig.GenerateTooltipForBooster(this) + "\n\n" + str;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000FEF1B File Offset: 0x000FD11B
	public void InformOfWattageChanged()
	{
		System.Action onWattageCostChanged = this.OnWattageCostChanged;
		if (onWattageCostChanged == null)
		{
			return;
		}
		onWattageCostChanged();
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x000FEF2D File Offset: 0x000FD12D
	public void SetWattageController(BionicUpgradeComponent.IWattageController wattageController)
	{
		this.WattageController = wattageController;
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000FEF38 File Offset: 0x000FD138
	public override void Assign(IAssignableIdentity new_assignee)
	{
		AssignableSlotInstance specificSlotInstance = null;
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee != this.assignee && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			Ownables soleOwner = new_assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				BionicUpgradesMonitor.Instance smi = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetSMI<BionicUpgradesMonitor.Instance>();
				if (smi != null)
				{
					BionicUpgradesMonitor.UpgradeComponentSlot firstEmptyAvailableSlot = smi.GetFirstEmptyAvailableSlot();
					if (firstEmptyAvailableSlot != null)
					{
						specificSlotInstance = firstEmptyAvailableSlot.GetAssignableSlotInstance();
					}
				}
			}
		}
		base.Assign(new_assignee, specificSlotInstance);
		base.Trigger(1980521255, null);
		this.RefreshStatusItem(null);
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000FEFC2 File Offset: 0x000FD1C2
	public override void Unassign()
	{
		base.Unassign();
		base.Trigger(1980521255, null);
		this.RefreshStatusItem(null);
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000FEFDD File Offset: 0x000FD1DD
	private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
	{
		return worker.GetMinionModel() == BionicMinionConfig.MODEL;
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x000FEFF0 File Offset: 0x000FD1F0
	private bool AssignablePrecondition_HasAvailableSlots(MinionAssignablesProxy worker)
	{
		if (SelectTool.Instance.selected != null && SelectTool.Instance.selected.gameObject == worker.GetTargetGameObject())
		{
			return true;
		}
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			BionicUpgradesMonitor.Instance smi = minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>();
			if (smi == null)
			{
				return true;
			}
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
			{
				if (!upgradeComponentSlot.IsLocked && (upgradeComponentSlot.assignedUpgradeComponent == null || upgradeComponentSlot.assignedUpgradeComponent == this))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000FF092 File Offset: 0x000FD292
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()].stateMachineDescription, null, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04001A02 RID: 6658
	private BionicUpgradeComponentConfig.BionicUpgradeData data;

	// Token: 0x04001A03 RID: 6659
	public System.Action OnWattageCostChanged;

	// Token: 0x04001A04 RID: 6660
	private Guid unassignedStatusItem = Guid.Empty;

	// Token: 0x020015A1 RID: 5537
	public interface IWattageController
	{
		// Token: 0x060093F3 RID: 37875
		float GetCurrentWattageCost();

		// Token: 0x060093F4 RID: 37876
		string GetCurrentWattageCostName();
	}
}
