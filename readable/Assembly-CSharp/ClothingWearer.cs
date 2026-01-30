using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000849 RID: 2121
[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
	// Token: 0x06003A23 RID: 14883 RVA: 0x00144A7C File Offset: 0x00142C7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decorProvider = base.GetComponent<DecorProvider>();
		if (this.decorModifier == null)
		{
			this.decorModifier = new AttributeModifier("Decor", 0f, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
		}
		if (this.conductivityModifier == null)
		{
			AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
			this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
			attributeInstance.Add(this.conductivityModifier);
		}
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x00144B14 File Offset: 0x00142D14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.decorProvider.decor.Add(this.decorModifier);
		this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f, null, false, false, true));
		Traits component = base.GetComponent<Traits>();
		string format = UI.OVERLAYS.DECOR.CLOTHING;
		if (component != null)
		{
			if (component.HasTrait("DecorUp"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORUP;
			}
			else if (component.HasTrait("DecorDown"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORDOWN;
			}
		}
		this.decorProvider.overrideName = string.Format(format, base.gameObject.GetProperName());
		if (this.currentClothing == null)
		{
			this.ChangeToDefaultClothes();
		}
		else
		{
			this.ChangeClothes(this.currentClothing);
		}
		this.spawnApplyClothesHandle = GameScheduler.Instance.Schedule("ApplySpawnClothes", 2f, delegate(object obj)
		{
			base.GetComponent<CreatureSimTemperatureTransfer>().RefreshRegistration();
		}, null, null);
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x00144C1C File Offset: 0x00142E1C
	protected override void OnCleanUp()
	{
		this.spawnApplyClothesHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x00144C30 File Offset: 0x00142E30
	public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
	{
		this.decorProvider.baseRadius = 3f;
		this.currentClothing = clothingInfo;
		this.conductivityModifier.Description = clothingInfo.name;
		this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
		this.decorModifier.SetValue((float)this.currentClothing.decorMod);
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x00144C92 File Offset: 0x00142E92
	public void ChangeToDefaultClothes()
	{
		this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));
	}

	// Token: 0x04002371 RID: 9073
	private DecorProvider decorProvider;

	// Token: 0x04002372 RID: 9074
	private SchedulerHandle spawnApplyClothesHandle;

	// Token: 0x04002373 RID: 9075
	private AttributeModifier decorModifier;

	// Token: 0x04002374 RID: 9076
	private AttributeModifier conductivityModifier;

	// Token: 0x04002375 RID: 9077
	[Serialize]
	public ClothingWearer.ClothingInfo currentClothing;

	// Token: 0x020017F8 RID: 6136
	public class ClothingInfo
	{
		// Token: 0x06009D3E RID: 40254 RVA: 0x0039C61F File Offset: 0x0039A81F
		public ClothingInfo(string _name, int _decor, float _temperature, float _homeostasisEfficiencyMultiplier)
		{
			this.name = _name;
			this.decorMod = _decor;
			this.conductivityMod = _temperature;
			this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
		}

		// Token: 0x06009D3F RID: 40255 RVA: 0x0039C650 File Offset: 0x0039A850
		public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner == null)
			{
				return;
			}
			ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
			if (component != null)
			{
				component.ChangeClothes(clothingInfo);
				return;
			}
			global::Debug.LogWarning("Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x0039C6B8 File Offset: 0x0039A8B8
		public static void OnUnequipVest(Equippable eq)
		{
			if (eq != null && eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return;
				}
				MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
				if (component == null)
				{
					return;
				}
				GameObject targetGameObject = component.GetTargetGameObject();
				if (targetGameObject == null)
				{
					return;
				}
				ClothingWearer component2 = targetGameObject.GetComponent<ClothingWearer>();
				if (component2 == null)
				{
					return;
				}
				component2.ChangeToDefaultClothes();
			}
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x0039C728 File Offset: 0x0039A928
		public static void SetupVest(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
			Equippable equippable = go.GetComponent<Equippable>();
			if (equippable == null)
			{
				equippable = go.AddComponent<Equippable>();
			}
			equippable.SetQuality(global::QualityLevel.Poor);
			go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
		}

		// Token: 0x04007948 RID: 31048
		[Serialize]
		public string name = "";

		// Token: 0x04007949 RID: 31049
		[Serialize]
		public int decorMod;

		// Token: 0x0400794A RID: 31050
		[Serialize]
		public float conductivityMod;

		// Token: 0x0400794B RID: 31051
		[Serialize]
		public float homeostasisEfficiencyMultiplier;

		// Token: 0x0400794C RID: 31052
		public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 0.0025f, -1.25f);

		// Token: 0x0400794D RID: 31053
		public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.WARM_VEST.NAME, 0, 0.008f, -1.25f);

		// Token: 0x0400794E RID: 31054
		public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0f);

		// Token: 0x0400794F RID: 31055
		public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 0.0025f, -1.25f);

		// Token: 0x04007950 RID: 31056
		public static readonly ClothingWearer.ClothingInfo CUSTOM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);

		// Token: 0x04007951 RID: 31057
		public static readonly ClothingWearer.ClothingInfo SLEEP_CLINIC_PAJAMAS = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);
	}
}
