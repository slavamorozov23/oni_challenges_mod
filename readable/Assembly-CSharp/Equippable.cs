using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000932 RID: 2354
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
	// Token: 0x060041C5 RID: 16837 RVA: 0x001736D5 File Offset: 0x001718D5
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x001736DD File Offset: 0x001718DD
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x060041C7 RID: 16839 RVA: 0x001736E6 File Offset: 0x001718E6
	// (set) Token: 0x060041C8 RID: 16840 RVA: 0x001736F3 File Offset: 0x001718F3
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x060041C9 RID: 16841 RVA: 0x00173704 File Offset: 0x00171904
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x060041CA RID: 16842 RVA: 0x00173754 File Offset: 0x00171954
	protected override void OnSpawn()
	{
		Components.AssignableItems.Add(this);
		if (this.isEquipped)
		{
			if (this.assignee != null && this.assignee is MinionIdentity)
			{
				this.assignee = (this.assignee as MinionIdentity).assignableProxy.Get();
				this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
			}
			if (this.assignee == null && this.assignee_identityRef.Get() != null)
			{
				this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
			}
			if (this.assignee != null)
			{
				Equipment component = this.assignee.GetSoleOwner().GetComponent<Equipment>();
				bool flag = true;
				UnityEngine.Object component2 = component.GetComponent<MinionAssignablesProxy>();
				GameObject gameObject = null;
				if (component2 != null)
				{
					gameObject = component.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					if (gameObject != null)
					{
						flag = gameObject.GetComponent<KPrefabID>().isSpawned;
					}
				}
				if (flag)
				{
					this.EquipToAssignable();
				}
				else
				{
					gameObject.Subscribe(1589886948, new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip));
				}
			}
			else
			{
				global::Debug.LogWarning("Equippable trying to be equipped to missing prefab");
				this.isEquipped = false;
			}
		}
		base.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
	}

	// Token: 0x060041CB RID: 16843 RVA: 0x00173881 File Offset: 0x00171A81
	private void EquipToAssignable()
	{
		if (this.assignee != null)
		{
			this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
		}
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x001738A1 File Offset: 0x00171AA1
	private void OnAsigneeSpawnedAndReadyForEquip(object o)
	{
		GameObject go = (GameObject)o;
		this.EquipToAssignable();
		go.Unsubscribe(1589886948, new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip));
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x001738C8 File Offset: 0x00171AC8
	public KAnimFile GetBuildOverride()
	{
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component == null || component.BuildOverride == null)
		{
			return this.def.BuildOverride;
		}
		return Assets.GetAnim(component.BuildOverride);
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x0017390C File Offset: 0x00171B0C
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x001739A8 File Offset: 0x00171BA8
	public override void Unassign()
	{
		if (this.isEquipped)
		{
			((this.assignee is MinionIdentity) ? ((MinionIdentity)this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((KMonoBehaviour)this.assignee).GetComponent<Equipment>()).Unequip(this);
			this.OnUnequip();
		}
		base.Unassign();
	}

	// Token: 0x060041D0 RID: 16848 RVA: 0x00173A08 File Offset: 0x00171C08
	public void OnEquip(AssignableSlotInstance slot)
	{
		this.isEquipped = true;
		if (SelectTool.Instance.selected == this.selectable)
		{
			SelectTool.Instance.Select(null, false);
		}
		base.GetComponent<KBatchedAnimController>().enabled = false;
		base.GetComponent<KSelectable>().IsSelectable = false;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		GameObject targetGameObject = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		Effects component = targetGameObject.GetComponent<Effects>();
		if (component != null)
		{
			foreach (Effect effect in this.def.EffectImmunites)
			{
				component.AddImmunity(effect, name, true);
			}
		}
		if (this.def.OnEquipCallBack != null)
		{
			this.def.OnEquipCallBack(this);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
		targetGameObject.Trigger(-210173199, this);
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x00173B14 File Offset: 0x00171D14
	public void OnUnequip()
	{
		this.isEquipped = false;
		if (this.destroyed)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
		base.GetComponent<KBatchedAnimController>().enabled = true;
		base.GetComponent<KSelectable>().IsSelectable = true;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		if (this.assignee != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					Effects component = targetGameObject.GetComponent<Effects>();
					if (component != null)
					{
						foreach (Effect effect in this.def.EffectImmunites)
						{
							component.RemoveImmunity(effect, name);
						}
					}
				}
			}
		}
		if (this.def.OnUnequipCallBack != null)
		{
			this.def.OnUnequipCallBack(this);
		}
		if (this.assignee != null)
		{
			Ownables soleOwner2 = this.assignee.GetSoleOwner();
			if (soleOwner2)
			{
				GameObject targetGameObject2 = soleOwner2.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject2)
				{
					targetGameObject2.Trigger(-1841406856, this);
				}
			}
		}
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x00173C5C File Offset: 0x00171E5C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.def != null)
		{
			List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
			if (this.def.additionalDescriptors != null)
			{
				foreach (Descriptor item in this.def.additionalDescriptors)
				{
					equipmentEffects.Add(item);
				}
			}
			return equipmentEffects;
		}
		return new List<Descriptor>();
	}

	// Token: 0x04002919 RID: 10521
	private global::QualityLevel quality;

	// Token: 0x0400291A RID: 10522
	[MyCmpAdd]
	private EquippableWorkable equippableWorkable;

	// Token: 0x0400291B RID: 10523
	[MyCmpAdd]
	private EquippableFacade facade;

	// Token: 0x0400291C RID: 10524
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400291D RID: 10525
	public DefHandle defHandle;

	// Token: 0x0400291E RID: 10526
	[Serialize]
	public bool isEquipped;

	// Token: 0x0400291F RID: 10527
	private bool destroyed;

	// Token: 0x04002920 RID: 10528
	[Serialize]
	public bool unequippable = true;

	// Token: 0x04002921 RID: 10529
	[Serialize]
	public bool hideInCodex;

	// Token: 0x04002922 RID: 10530
	private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>(delegate(Equippable component, object data)
	{
		component.destroyed = true;
	});
}
