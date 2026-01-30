using System;
using UnityEngine;

// Token: 0x02000DCB RID: 3531
public readonly struct OutfitBrowserScreenConfig
{
	// Token: 0x06006E56 RID: 28246 RVA: 0x0029C120 File Offset: 0x0029A320
	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		this.isPickingOutfitForDupe = (minionPersonality.HasValue || minionInstance.HasValue);
		this.targetMinionInstance = minionInstance;
		this.isValid = true;
		if (minionPersonality.IsSome() || this.targetMinionInstance.IsSome())
		{
			global::Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	// Token: 0x06006E57 RID: 28247 RVA: 0x0029C191 File Offset: 0x0029A391
	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x0029C1AB File Offset: 0x0029A3AB
	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x06006E59 RID: 28249 RVA: 0x0029C1C8 File Offset: 0x0029A3C8
	public string GetMinionName()
	{
		if (this.targetMinionInstance.HasValue)
		{
			return this.targetMinionInstance.Value.GetProperName();
		}
		if (this.minionPersonality.HasValue)
		{
			return this.minionPersonality.Value.Name;
		}
		return "-";
	}

	// Token: 0x06006E5A RID: 28250 RVA: 0x0029C216 File Offset: 0x0029A416
	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	// Token: 0x06006E5B RID: 28251 RVA: 0x0029C245 File Offset: 0x0029A445
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	// Token: 0x06006E5C RID: 28252 RVA: 0x0029C26C File Offset: 0x0029A46C
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality value = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), value, minionInstance);
	}

	// Token: 0x06006E5D RID: 28253 RVA: 0x0029C2B8 File Offset: 0x0029A4B8
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06006E5E RID: 28254 RVA: 0x0029C2F8 File Offset: 0x0029A4F8
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen, null);
	}

	// Token: 0x04004B61 RID: 19297
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	// Token: 0x04004B62 RID: 19298
	public readonly Option<ClothingOutfitTarget> selectedTarget;

	// Token: 0x04004B63 RID: 19299
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04004B64 RID: 19300
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04004B65 RID: 19301
	public readonly bool isValid;

	// Token: 0x04004B66 RID: 19302
	public readonly bool isPickingOutfitForDupe;
}
