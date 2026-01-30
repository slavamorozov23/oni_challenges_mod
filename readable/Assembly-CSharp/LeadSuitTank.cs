using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009DD RID: 2525
[SerializationConfig(MemberSerialization.OptIn)]
public class LeadSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004975 RID: 18805 RVA: 0x001A9880 File Offset: 0x001A7A80
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LeadSuitTank>(-1617557748, LeadSuitTank.OnEquippedDelegate);
		base.Subscribe<LeadSuitTank>(-170173755, LeadSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x001A98AA File Offset: 0x001A7AAA
	public float PercentFull()
	{
		return this.batteryCharge;
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x001A98B2 File Offset: 0x001A7AB2
	public bool IsEmpty()
	{
		return this.batteryCharge <= 0f;
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x001A98C4 File Offset: 0x001A7AC4
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06004979 RID: 18809 RVA: 0x001A98D6 File Offset: 0x001A7AD6
	public bool NeedsRecharging()
	{
		return this.PercentFull() <= 0.25f;
	}

	// Token: 0x0600497A RID: 18810 RVA: 0x001A98E8 File Offset: 0x001A7AE8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.LEADSUIT_BATTERY, GameUtil.GetFormattedPercent(this.PercentFull() * 100f, GameUtil.TimeSlice.None));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x0600497B RID: 18811 RVA: 0x001A992C File Offset: 0x001A7B2C
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.leadSuitMonitor = new LeadSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.leadSuitMonitor.StartSM();
		if (this.NeedsRecharging())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.SuitBatteryLow);
		}
	}

	// Token: 0x0600497C RID: 18812 RVA: 0x001A99A4 File Offset: 0x001A7BA4
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryLow);
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryOut);
			NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
		}
		if (this.leadSuitMonitor != null)
		{
			this.leadSuitMonitor.StopSM("Removed leadsuit tank");
			this.leadSuitMonitor = null;
		}
	}

	// Token: 0x040030E8 RID: 12520
	[Serialize]
	public float batteryCharge = 1f;

	// Token: 0x040030E9 RID: 12521
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040030EA RID: 12522
	public float batteryDuration = 200f;

	// Token: 0x040030EB RID: 12523
	public float coolingOperationalTemperature = 333.15f;

	// Token: 0x040030EC RID: 12524
	public Tag coolantTag;

	// Token: 0x040030ED RID: 12525
	private LeadSuitMonitor.Instance leadSuitMonitor;

	// Token: 0x040030EE RID: 12526
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040030EF RID: 12527
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
