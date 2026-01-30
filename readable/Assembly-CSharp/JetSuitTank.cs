using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009D1 RID: 2513
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, IDevQuickAction
{
	// Token: 0x060048F1 RID: 18673 RVA: 0x001A64AA File Offset: 0x001A46AA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
		base.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x001A64D4 File Offset: 0x001A46D4
	public float PercentFull()
	{
		return this.amount / 100f;
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x001A64E2 File Offset: 0x001A46E2
	public bool IsEmpty()
	{
		return this.amount <= 0f;
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x001A64F4 File Offset: 0x001A46F4
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x001A6506 File Offset: 0x001A4706
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.2f;
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x001A6518 File Offset: 0x001A4718
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, GameUtil.GetFormattedMass(this.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x001A655C File Offset: 0x001A475C
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.jetSuitMonitor = new JetSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.jetSuitMonitor.StartSM();
		if (this.IsEmpty())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x001A65D4 File Offset: 0x001A47D4
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
			NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
			Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
			if (component && component.CurrentNavType == NavType.Hover)
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}
		if (this.jetSuitMonitor != null)
		{
			this.jetSuitMonitor.StopSM("Removed jetsuit tank");
			this.jetSuitMonitor = null;
		}
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001A6664 File Offset: 0x001A4864
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.amount = 0f;
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x001A6674 File Offset: 0x001A4874
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.amount = 100f;
		if (this.jetSuitMonitor != null && this.jetSuitMonitor.sm.owner.Get(this.jetSuitMonitor) != null)
		{
			this.jetSuitMonitor.sm.owner.Get(this.jetSuitMonitor).RemoveTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x001A66DC File Offset: 0x001A48DC
	public List<DevQuickActionInstruction> GetDevInstructions()
	{
		return new List<DevQuickActionInstruction>
		{
			new DevQuickActionInstruction(IDevQuickAction.CommonMenusNames.Storage, "Fill Fuel", new System.Action(this.FillTank)),
			new DevQuickActionInstruction(IDevQuickAction.CommonMenusNames.Storage, "Empty Fuel", new System.Action(this.Empty))
		};
	}

	// Token: 0x0400307F RID: 12415
	[MyCmpGet]
	private ElementEmitter elementConverter;

	// Token: 0x04003080 RID: 12416
	[Serialize]
	public SimHashes lastFuelUsed = SimHashes.Vacuum;

	// Token: 0x04003081 RID: 12417
	[Serialize]
	public float amount;

	// Token: 0x04003082 RID: 12418
	public const float FUEL_CAPACITY = 100f;

	// Token: 0x04003083 RID: 12419
	public const float FUEL_BURN_RATE = 0.2f;

	// Token: 0x04003084 RID: 12420
	public const float CO2_EMITTED_PER_FUEL_BURNED = 0.25f;

	// Token: 0x04003085 RID: 12421
	public const float EMIT_TEMPERATURE = 373.15f;

	// Token: 0x04003086 RID: 12422
	public const float REFILL_PERCENT = 0.2f;

	// Token: 0x04003087 RID: 12423
	private JetSuitMonitor.Instance jetSuitMonitor;

	// Token: 0x04003088 RID: 12424
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04003089 RID: 12425
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
