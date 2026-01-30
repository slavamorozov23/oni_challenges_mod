using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000BEE RID: 3054
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
	// Token: 0x06005B9F RID: 23455 RVA: 0x002127A4 File Offset: 0x002109A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
		base.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x002127D0 File Offset: 0x002109D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.amount != 0f)
		{
			this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			this.amount = 0f;
		}
		this.equippable = base.GetComponent<Equippable>();
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00212831 File Offset: 0x00210A31
	public float GetTankAmount()
	{
		if (this.storage == null)
		{
			this.storage = base.GetComponent<Storage>();
		}
		return this.storage.GetMassAvailable(this.elementTag);
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x0021285E File Offset: 0x00210A5E
	public float PercentFull()
	{
		return this.GetTankAmount() / this.capacity;
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x0021286D File Offset: 0x00210A6D
	public bool IsEmpty()
	{
		return this.GetTankAmount() <= 0f;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x0021287F File Offset: 0x00210A7F
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x00212891 File Offset: 0x00210A91
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06005BA6 RID: 23462 RVA: 0x002128A0 File Offset: 0x00210AA0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.elementTag == GameTags.Breathable)
		{
			string text = this.underwaterSupport ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06005BA7 RID: 23463 RVA: 0x00212924 File Offset: 0x00210B24
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
		if (component != null)
		{
			component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().AddIgnoredFlagsSet("SuitTank", this.SafeCellFlagsToIgnoreOnEquipped);
			component.AddGasProvider(this);
		}
		targetGameObject.AddTag(GameTags.HasSuitTank);
	}

	// Token: 0x06005BA8 RID: 23464 RVA: 0x002129A4 File Offset: 0x00210BA4
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
			GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
			if (component != null)
			{
				component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().RemoveIgnoredFlagsSet("SuitTank");
				component.RemoveGasProvider(this);
			}
			targetGameObject.RemoveTag(GameTags.HasSuitTank);
		}
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x00212A24 File Offset: 0x00210C24
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x00212A26 File Offset: 0x00210C26
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06005BAB RID: 23467 RVA: 0x00212A28 File Offset: 0x00210C28
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.IsEmpty())
		{
			return false;
		}
		float temperature = 0f;
		SimHashes elementConsumed = SimHashes.Vacuum;
		float massConsumed;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(this.elementTag, amount, out massConsumed, out diseaseInfo, out temperature, out elementConsumed);
		OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, massConsumed, temperature, diseaseInfo.idx, diseaseInfo.count);
		base.Trigger(608245985, base.gameObject);
		return true;
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x00212A8C File Offset: 0x00210C8C
	public bool ShouldEmitCO2()
	{
		bool flag = base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
		if (flag)
		{
			return false;
		}
		bool flag2 = this.IsOwnerBionic();
		return !flag && !flag2;
	}

	// Token: 0x06005BAD RID: 23469 RVA: 0x00212AC0 File Offset: 0x00210CC0
	public bool ShouldStoreCO2()
	{
		bool flag = base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
		if (!flag)
		{
			return false;
		}
		bool flag2 = this.IsOwnerBionic();
		return flag && !flag2;
	}

	// Token: 0x06005BAE RID: 23470 RVA: 0x00212AF4 File Offset: 0x00210CF4
	public bool IsOwnerBionic()
	{
		bool result = false;
		if (this.equippable != null && this.equippable.IsAssigned() && this.equippable.isEquipped)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					result = (targetGameObject.PrefabID() == BionicMinionConfig.ID);
				}
			}
		}
		return result;
	}

	// Token: 0x06005BAF RID: 23471 RVA: 0x00212B70 File Offset: 0x00210D70
	public bool IsLowOxygen()
	{
		return this.NeedsRecharging();
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x00212B78 File Offset: 0x00210D78
	[ContextMenu("SetToRefillAmount")]
	public void SetToRefillAmount()
	{
		float tankAmount = this.GetTankAmount();
		float num = 0.25f * this.capacity;
		if (tankAmount > num)
		{
			this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
		}
	}

	// Token: 0x06005BB1 RID: 23473 RVA: 0x00212BB1 File Offset: 0x00210DB1
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x00212BCA File Offset: 0x00210DCA
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.Empty();
		this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, 0, 0, false, false);
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x00212BF2 File Offset: 0x00210DF2
	public bool HasOxygen()
	{
		return !this.IsEmpty();
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x00212BFD File Offset: 0x00210DFD
	public bool IsBlocked()
	{
		return false;
	}

	// Token: 0x04003D04 RID: 15620
	public SafeCellQuery.SafeFlags SafeCellFlagsToIgnoreOnEquipped = (SafeCellQuery.SafeFlags)464;

	// Token: 0x04003D05 RID: 15621
	[Serialize]
	public string element;

	// Token: 0x04003D06 RID: 15622
	[Serialize]
	public float amount;

	// Token: 0x04003D07 RID: 15623
	public Tag elementTag;

	// Token: 0x04003D08 RID: 15624
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04003D09 RID: 15625
	public float capacity;

	// Token: 0x04003D0A RID: 15626
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04003D0B RID: 15627
	public bool underwaterSupport;

	// Token: 0x04003D0C RID: 15628
	private Equippable equippable;

	// Token: 0x04003D0D RID: 15629
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04003D0E RID: 15630
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
