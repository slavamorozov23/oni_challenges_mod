using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x0200092F RID: 2351
public class EquipmentDef : Def
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x060041BC RID: 16828 RVA: 0x001735B3 File Offset: 0x001717B3
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x060041BD RID: 16829 RVA: 0x001735D9 File Offset: 0x001717D9
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".DESC");
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x060041BE RID: 16830 RVA: 0x001735FF File Offset: 0x001717FF
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x060041BF RID: 16831 RVA: 0x00173625 File Offset: 0x00171825
	public string GenericName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x060041C0 RID: 16832 RVA: 0x0017364B File Offset: 0x0017184B
	public string WornName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_NAME");
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x060041C1 RID: 16833 RVA: 0x00173671 File Offset: 0x00171871
	public string WornDesc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_DESC");
		}
	}

	// Token: 0x04002900 RID: 10496
	public string Id;

	// Token: 0x04002901 RID: 10497
	public string Slot;

	// Token: 0x04002902 RID: 10498
	public string FabricatorId;

	// Token: 0x04002903 RID: 10499
	public float FabricationTime;

	// Token: 0x04002904 RID: 10500
	public string RecipeTechUnlock;

	// Token: 0x04002905 RID: 10501
	public SimHashes OutputElement;

	// Token: 0x04002906 RID: 10502
	public Dictionary<string, float> InputElementMassMap;

	// Token: 0x04002907 RID: 10503
	public float Mass;

	// Token: 0x04002908 RID: 10504
	public KAnimFile Anim;

	// Token: 0x04002909 RID: 10505
	public string SnapOn;

	// Token: 0x0400290A RID: 10506
	public string SnapOn1;

	// Token: 0x0400290B RID: 10507
	public KAnimFile BuildOverride;

	// Token: 0x0400290C RID: 10508
	public int BuildOverridePriority;

	// Token: 0x0400290D RID: 10509
	public bool IsBody;

	// Token: 0x0400290E RID: 10510
	public List<AttributeModifier> AttributeModifiers;

	// Token: 0x0400290F RID: 10511
	public string RecipeDescription;

	// Token: 0x04002910 RID: 10512
	public List<Effect> EffectImmunites = new List<Effect>();

	// Token: 0x04002911 RID: 10513
	public Action<Equippable> OnEquipCallBack;

	// Token: 0x04002912 RID: 10514
	public Action<Equippable> OnUnequipCallBack;

	// Token: 0x04002913 RID: 10515
	public EntityTemplates.CollisionShape CollisionShape;

	// Token: 0x04002914 RID: 10516
	public float width;

	// Token: 0x04002915 RID: 10517
	public float height = 0.325f;

	// Token: 0x04002916 RID: 10518
	public Tag[] AdditionalTags;

	// Token: 0x04002917 RID: 10519
	public string wornID;

	// Token: 0x04002918 RID: 10520
	public List<Descriptor> additionalDescriptors = new List<Descriptor>();
}
