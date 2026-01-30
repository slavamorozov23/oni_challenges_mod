using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000D11 RID: 3345
public class FullBodyUIMinionWidget : KMonoBehaviour
{
	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x0600677B RID: 26491 RVA: 0x002703D1 File Offset: 0x0026E5D1
	// (set) Token: 0x0600677C RID: 26492 RVA: 0x002703D9 File Offset: 0x0026E5D9
	public KBatchedAnimController animController { get; private set; }

	// Token: 0x0600677D RID: 26493 RVA: 0x002703E2 File Offset: 0x0026E5E2
	protected override void OnSpawn()
	{
		this.TrySpawnDisplayMinion();
	}

	// Token: 0x0600677E RID: 26494 RVA: 0x002703EC File Offset: 0x0026E5EC
	private void TrySpawnDisplayMinion()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
		}
	}

	// Token: 0x0600677F RID: 26495 RVA: 0x00270454 File Offset: 0x0026E654
	private void InitializeAnimator()
	{
		this.TrySpawnDisplayMinion();
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		for (int i = component.GetAccessories().Count - 1; i >= 0; i--)
		{
			component.RemoveAccessory(component.GetAccessories()[i].Get());
		}
	}

	// Token: 0x06006780 RID: 26496 RVA: 0x002704C4 File Offset: 0x0026E6C4
	public void SetDefaultPortraitAnimator()
	{
		MinionIdentity minionIdentity = (Components.MinionIdentities.Count > 0) ? Components.MinionIdentities[0] : null;
		HashedString id = (minionIdentity != null) ? minionIdentity.personalityResourceId : Db.Get().Personalities.resources.GetRandom<Personality>().Id;
		this.InitializeAnimator();
		this.animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
		Accessorizer accessorizer = (minionIdentity != null) ? minionIdentity.GetComponent<Accessorizer>() : null;
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (accessorizer)
		{
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(null, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, null);
	}

	// Token: 0x06006781 RID: 26497 RVA: 0x002705EC File Offset: 0x0026E7EC
	public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
	{
		if (assignableIdentity == null || assignableIdentity.IsNull())
		{
			this.SetDefaultPortraitAnimator();
			return;
		}
		this.InitializeAnimator();
		string current_hat = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(assignableIdentity, out minionIdentity, out storedMinionIdentity);
		Accessorizer accessorizer = null;
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (minionIdentity != null)
		{
			accessorizer = minionIdentity.GetComponent<Accessorizer>();
			foreach (ResourceRef<Accessory> resourceRef in accessorizer.GetAccessories())
			{
				component.AddAccessory(resourceRef.Get());
			}
			current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		else if (storedMinionIdentity != null)
		{
			foreach (ResourceRef<Accessory> resourceRef2 in storedMinionIdentity.accessories)
			{
				component.AddAccessory(resourceRef2.Get());
			}
			current_hat = storedMinionIdentity.currentHat;
			hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
	}

	// Token: 0x06006782 RID: 26498 RVA: 0x002707FC File Offset: 0x0026E9FC
	private void UpdateHatOverride(string current_hat, KAnim.Build.Symbol hair_symbol, KAnim.Build.Symbol hat_hair_symbol)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		this.animController.SetSymbolVisiblity(hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		SymbolOverrideController component = this.animController.GetComponent<SymbolOverrideController>();
		if (hair_symbol != null)
		{
			component.AddSymbolOverride("snapto_hair_always", hair_symbol, 1);
		}
		if (hat_hair_symbol != null)
		{
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
		}
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x002708D4 File Offset: 0x0026EAD4
	private void UpdateClothingOverride(SymbolOverrideController symbolOverrideController, MinionIdentity identity, StoredMinionIdentity storedMinionIdentity)
	{
		string[] array = null;
		if (identity != null)
		{
			array = identity.GetComponent<WearableAccessorizer>().GetClothingItemsIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		else if (storedMinionIdentity != null)
		{
			array = storedMinionIdentity.GetClothingItemIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		if (array != null)
		{
			this.animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in array
			select Db.Get().Permits.ClothingItems.Get(i));
		}
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x00270945 File Offset: 0x0026EB45
	public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
	{
		this.animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
	}

	// Token: 0x06006785 RID: 26501 RVA: 0x00270959 File Offset: 0x0026EB59
	public void RemoveEquipment(Equippable equippable)
	{
		this.animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x0027096C File Offset: 0x0026EB6C
	private void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x040046E4 RID: 18148
	[SerializeField]
	private GameObject duplicantAnimAnchor;

	// Token: 0x040046E6 RID: 18150
	public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;
}
