using System;
using UnityEngine;

// Token: 0x02000DCF RID: 3535
public readonly struct OutfitDesignerScreenConfig
{
	// Token: 0x06006E84 RID: 28292 RVA: 0x0029D9D8 File Offset: 0x0029BBD8
	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		this.outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : Option.None);
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		this.isValid = true;
		ClothingOutfitTarget.MinionInstance minionInstance;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
		{
			global::Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
		}
	}

	// Token: 0x06006E85 RID: 28293 RVA: 0x0029DA52 File Offset: 0x0029BC52
	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
	}

	// Token: 0x06006E86 RID: 28294 RVA: 0x0029DA6C File Offset: 0x0029BC6C
	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
	}

	// Token: 0x06006E87 RID: 28295 RVA: 0x0029DA86 File Offset: 0x0029BC86
	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None, null);
	}

	// Token: 0x06006E88 RID: 28296 RVA: 0x0029DAA3 File Offset: 0x0029BCA3
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None, null);
	}

	// Token: 0x06006E89 RID: 28297 RVA: 0x0029DABC File Offset: 0x0029BCBC
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality value = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		ClothingOutfitTarget.MinionInstance minionInstance;
		global::Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
		global::Debug.Assert(minionInstance.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, value, targetMinionInstance, null);
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x0029DB18 File Offset: 0x0029BD18
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06006E8B RID: 28299 RVA: 0x0029DB58 File Offset: 0x0029BD58
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen, null);
	}

	// Token: 0x04004B8F RID: 19343
	public readonly ClothingOutfitTarget sourceTarget;

	// Token: 0x04004B90 RID: 19344
	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	// Token: 0x04004B91 RID: 19345
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04004B92 RID: 19346
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04004B93 RID: 19347
	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	// Token: 0x04004B94 RID: 19348
	public readonly bool isValid;
}
