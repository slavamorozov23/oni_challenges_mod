using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;

// Token: 0x02000EBC RID: 3772
public static class UIMinionOrMannequinITargetExtensions
{
	// Token: 0x060078D7 RID: 30935 RVA: 0x002E790B File Offset: 0x002E5B0B
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitResource outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.itemsInOutfit
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x060078D8 RID: 30936 RVA: 0x002E7943 File Offset: 0x002E5B43
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, OutfitDesignerScreen_OutfitState outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.GetItems()
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x060078D9 RID: 30937 RVA: 0x002E797B File Offset: 0x002E5B7B
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitTarget outfit)
	{
		self.SetOutfit(outfit.OutfitType, outfit.ReadItemValues());
	}

	// Token: 0x060078DA RID: 30938 RVA: 0x002E7991 File Offset: 0x002E5B91
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType, Option<ClothingOutfitTarget> outfit)
	{
		if (outfit.HasValue)
		{
			self.SetOutfit(outfit.Value);
			return;
		}
		self.ClearOutfit(outfitType);
	}

	// Token: 0x060078DB RID: 30939 RVA: 0x002E79B1 File Offset: 0x002E5BB1
	public static void ClearOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType)
	{
		self.SetOutfit(outfitType, UIMinionOrMannequinITargetExtensions.EMPTY_OUTFIT);
	}

	// Token: 0x060078DC RID: 30940 RVA: 0x002E79BF File Offset: 0x002E5BBF
	public static void React(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.None);
	}

	// Token: 0x060078DD RID: 30941 RVA: 0x002E79C8 File Offset: 0x002E5BC8
	public static void ReactToClothingItemChange(this UIMinionOrMannequin.ITarget self, PermitCategory clothingChangedCategory)
	{
		self.React(UIMinionOrMannequinITargetExtensions.<ReactToClothingItemChange>g__GetSource|7_0(clothingChangedCategory));
	}

	// Token: 0x060078DE RID: 30942 RVA: 0x002E79D6 File Offset: 0x002E5BD6
	public static void ReactToPersonalityChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnPersonalityChanged);
	}

	// Token: 0x060078DF RID: 30943 RVA: 0x002E79DF File Offset: 0x002E5BDF
	public static void ReactToFullOutfitChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnWholeOutfitChanged);
	}

	// Token: 0x060078E0 RID: 30944 RVA: 0x002E79E8 File Offset: 0x002E5BE8
	public static IEnumerable<ClothingItemResource> GetOutfitWithDefaultItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return outfit;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
			{
				foreach (ClothingItemResource clothingItemResource in outfit)
				{
					DebugUtil.DevAssert(!pooledDictionary.ContainsKey(clothingItemResource.Category), "Duplicate item for category", null);
					pooledDictionary[clothingItemResource.Category] = clothingItemResource;
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitHelmet))
				{
					pooledDictionary[PermitCategory.AtmoSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoHelmetClear");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBody))
				{
					pooledDictionary[PermitCategory.AtmoSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoSuitBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitGloves))
				{
					pooledDictionary[PermitCategory.AtmoSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoGlovesBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBelt))
				{
					pooledDictionary[PermitCategory.AtmoSuitBelt] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoBeltBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitShoes))
				{
					pooledDictionary[PermitCategory.AtmoSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoShoesBasicBlack");
				}
				return pooledDictionary.Values.ToArray<ClothingItemResource>();
			}
			break;
		case ClothingOutfitUtility.OutfitType.JetSuit:
			break;
		default:
			goto IL_289;
		}
		using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary2 = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
		{
			foreach (ClothingItemResource clothingItemResource2 in outfit)
			{
				DebugUtil.DevAssert(!pooledDictionary2.ContainsKey(clothingItemResource2.Category), "Duplicate item for category", null);
				pooledDictionary2[clothingItemResource2.Category] = clothingItemResource2;
			}
			if (!pooledDictionary2.ContainsKey(PermitCategory.JetSuitHelmet))
			{
				pooledDictionary2[PermitCategory.JetSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_JetHelmetClear");
			}
			if (!pooledDictionary2.ContainsKey(PermitCategory.JetSuitBody))
			{
				pooledDictionary2[PermitCategory.JetSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_JetSuitBasic");
			}
			if (!pooledDictionary2.ContainsKey(PermitCategory.JetSuitGloves))
			{
				pooledDictionary2[PermitCategory.JetSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_JetGlovesBasic");
			}
			if (!pooledDictionary2.ContainsKey(PermitCategory.JetSuitShoes))
			{
				pooledDictionary2[PermitCategory.JetSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_JetShoesBasic");
			}
			return pooledDictionary2.Values.ToArray<ClothingItemResource>();
		}
		IL_289:
		throw new NotImplementedException();
	}

	// Token: 0x060078E2 RID: 30946 RVA: 0x002E7CFC File Offset: 0x002E5EFC
	[CompilerGenerated]
	internal static UIMinionOrMannequinReactSource <ReactToClothingItemChange>g__GetSource|7_0(PermitCategory clothingChangedCategory)
	{
		switch (clothingChangedCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.JetSuitBody:
			return UIMinionOrMannequinReactSource.OnTopChanged;
		case PermitCategory.DupeBottoms:
			return UIMinionOrMannequinReactSource.OnBottomChanged;
		case PermitCategory.DupeGloves:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.JetSuitGloves:
			return UIMinionOrMannequinReactSource.OnGlovesChanged;
		case PermitCategory.DupeShoes:
		case PermitCategory.AtmoSuitShoes:
		case PermitCategory.JetSuitShoes:
			return UIMinionOrMannequinReactSource.OnShoesChanged;
		case PermitCategory.DupeHats:
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.JetSuitHelmet:
			return UIMinionOrMannequinReactSource.OnHatChanged;
		}
		DebugUtil.DevAssert(false, string.Format("Couldn't find a reaction for \"{0}\" clothing item category being changed", clothingChangedCategory), null);
		return UIMinionOrMannequinReactSource.None;
	}

	// Token: 0x04005442 RID: 21570
	public static readonly ClothingItemResource[] EMPTY_OUTFIT = new ClothingItemResource[0];
}
