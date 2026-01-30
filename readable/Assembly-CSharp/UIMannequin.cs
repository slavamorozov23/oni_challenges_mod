using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000EB8 RID: 3768
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x060078BC RID: 30908 RVA: 0x002E6F2C File Offset: 0x002E512C
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x060078BD RID: 30909 RVA: 0x002E6F48 File Offset: 0x002E5148
	public Option<Personality> Personality
	{
		get
		{
			return default(Option<Personality>);
		}
	}

	// Token: 0x060078BE RID: 30910 RVA: 0x002E6F5E File Offset: 0x002E515E
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060078BF RID: 30911 RVA: 0x002E6F68 File Offset: 0x002E5168
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.LoadAnims();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle", KAnim.PlayMode.Paused, 1f, 0f);
			this.spawn = this.animController.gameObject;
			BaseMinionConfig.ConfigureSymbols(this.spawn, false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	// Token: 0x060078C0 RID: 30912 RVA: 0x002E7030 File Offset: 0x002E5230
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count<ClothingItemResource>() == 0;
		if (this.shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		this.SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides(0);
		BaseMinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
		Accessorizer component = this.SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component2 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ApplyMinionPersonality(this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"), null));
		component2.ClearClothingItems(null);
		component2.ApplyClothingItems(outfitType, outfit);
		List<KAnimHashedString> list = new List<KAnimHashedString>(32);
		if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			list.Add("foot");
			list.Add("hand_paint");
			if (flag)
			{
				list.Add("belt");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeTops))
			{
				list.Add("torso");
				list.Add("neck");
				list.Add("arm_lower");
				list.Add("arm_lower_sleeve");
				list.Add("arm_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeGloves))
			{
				list.Add("arm_lower_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeBottoms))
			{
				list.Add("leg");
				list.Add("pelvis");
			}
		}
		KAnimHashedString[] source = outfit.SelectMany((ClothingItemResource item) => from s in item.AnimFile.GetData().build.symbols
		select s.hash).Concat(list).ToArray<KAnimHashedString>();
		foreach (KAnim.Build.Symbol symbol in this.animController.AnimFiles[0].GetData().build.symbols)
		{
			if (symbol.hash == "mannequin_arm" || symbol.hash == "mannequin_body" || symbol.hash == "mannequin_headshape" || symbol.hash == "mannequin_leg")
			{
				this.animController.SetSymbolVisiblity(symbol.hash, true);
			}
			else
			{
				this.animController.SetSymbolVisiblity(symbol.hash, source.Contains(symbol.hash));
			}
		}
	}

	// Token: 0x060078C1 RID: 30913 RVA: 0x002E7340 File Offset: 0x002E5540
	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			if (clothingItemResource.Category == category)
			{
				return clothingItemResource;
			}
		}
		return null;
	}

	// Token: 0x060078C2 RID: 30914 RVA: 0x002E7394 File Offset: 0x002E5594
	public void React(UIMinionOrMannequinReactSource source)
	{
		this.animController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400542C RID: 21548
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x0400542D RID: 21549
	private KBatchedAnimController animController;

	// Token: 0x0400542E RID: 21550
	private GameObject spawn;

	// Token: 0x0400542F RID: 21551
	public bool shouldShowOutfitWithDefaultItems = true;

	// Token: 0x04005430 RID: 21552
	public Option<Personality> personalityToUseForDefaultClothing;
}
