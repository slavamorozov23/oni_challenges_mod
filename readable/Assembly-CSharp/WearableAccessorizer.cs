using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000673 RID: 1651
[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
	// Token: 0x06002811 RID: 10257 RVA: 0x000E69D7 File Offset: 0x000E4BD7
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
	{
		return this.customOutfitItems;
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06002812 RID: 10258 RVA: 0x000E69DF File Offset: 0x000E4BDF
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> Wearables
	{
		get
		{
			return this.wearables;
		}
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x000E69E8 File Offset: 0x000E4BE8
	public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customOutfitItems[outfitType].Count];
			for (int i = 0; i < this.customOutfitItems[outfitType].Count; i++)
			{
				array[i] = this.customOutfitItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return new string[0];
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x000E6A5D File Offset: 0x000E4C5D
	public Option<string> GetJoyResponseId()
	{
		return this.joyResponsePermitId;
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000E6A6A File Offset: 0x000E4C6A
	public void SetJoyResponseId(Option<string> joyResponsePermitId)
	{
		this.joyResponsePermitId = joyResponsePermitId.UnwrapOr(null, null);
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000E6A7C File Offset: 0x000E4C7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		base.Subscribe(-448952673, new Action<object>(this.EquippedItem));
		base.Subscribe(-1285462312, new Action<object>(this.UnequippedItem));
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000E6ADC File Offset: 0x000E4CDC
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		List<WearableAccessorizer.WearableType> list = new List<WearableAccessorizer.WearableType>();
		foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
		{
			keyValuePair.Value.Deserialize();
			if (keyValuePair.Value.BuildAnims == null || keyValuePair.Value.BuildAnims.Count == 0)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (WearableAccessorizer.WearableType key in list)
		{
			this.wearables.Remove(key);
		}
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair2 in this.customOutfitItems)
		{
			ClothingOutfitUtility.OutfitType outfitType;
			List<ResourceRef<ClothingItemResource>> list2;
			keyValuePair2.Deconstruct(out outfitType, out list2);
			List<ResourceRef<ClothingItemResource>> list3 = list2;
			if (list3 != null && list3.Count != 0)
			{
				for (int num = list3.Count - 1; num != -1; num--)
				{
					if (list3[num].Get() == null)
					{
						list3.RemoveAt(num);
					}
				}
			}
		}
		if (this.clothingItems.Count > 0)
		{
			this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
			if (!this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomClothing))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
				{
					this.Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, resourceRef.Get());
				}
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000E6CCC File Offset: 0x000E4ECC
	public void EquippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.ApplyEquipment(component, component.GetBuildOverride());
		}
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000E6D00 File Offset: 0x000E4F00
	public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && animFile != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(this.wearables[key].BuildAnims[0], this.wearables[key].buildOverridePriority);
			}
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2))
			{
				this.wearables[WearableAccessorizer.WearableType.CustomSuit] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
				this.wearables[WearableAccessorizer.WearableType.CustomSuit].AddCustomItems(this.customOutfitItems[key2]);
			}
			else
			{
				this.wearables[key] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x000E6DF5 File Offset: 0x000E4FF5
	private bool TryGetEquippableClothingType(EquipmentDef equipment, out ClothingOutfitUtility.OutfitType outfitType)
	{
		if (equipment.Id == "Atmo_Suit")
		{
			outfitType = ClothingOutfitUtility.OutfitType.AtmoSuit;
			return true;
		}
		if (equipment.Id == "Jet_Suit")
		{
			outfitType = ClothingOutfitUtility.OutfitType.JetSuit;
			return true;
		}
		outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
		return false;
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x000E6E2C File Offset: 0x000E502C
	private Equippable GetSuitEquippable()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null && component.assignableProxy != null && component.assignableProxy.Get() != null)
		{
			Equipment equipment = component.GetEquipment();
			Assignable assignable = (equipment != null) ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : null;
			if (assignable != null)
			{
				return assignable.GetComponent<Equippable>();
			}
		}
		return null;
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000E6EA0 File Offset: 0x000E50A0
	private WearableAccessorizer.WearableType GetHighestAccessory()
	{
		WearableAccessorizer.WearableType wearableType = WearableAccessorizer.WearableType.Basic;
		foreach (WearableAccessorizer.WearableType wearableType2 in this.wearables.Keys)
		{
			if (wearableType2 > wearableType)
			{
				wearableType = wearableType2;
			}
		}
		return wearableType;
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000E6EFC File Offset: 0x000E50FC
	private void ApplyWearable()
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
			if (this.animController == null)
			{
				global::Debug.LogWarning("Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
				return;
			}
		}
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		WearableAccessorizer.WearableType highestAccessory = this.GetHighestAccessory();
		foreach (object obj in Enum.GetValues(typeof(WearableAccessorizer.WearableType)))
		{
			WearableAccessorizer.WearableType wearableType = (WearableAccessorizer.WearableType)obj;
			if (this.wearables.ContainsKey(wearableType))
			{
				WearableAccessorizer.Wearable wearable = this.wearables[wearableType];
				int buildOverridePriority = wearable.buildOverridePriority;
				foreach (KAnimFile kanimFile in wearable.BuildAnims)
				{
					KAnim.Build build = kanimFile.GetData().build;
					if (build != null)
					{
						for (int i = 0; i < build.symbols.Length; i++)
						{
							string text = HashCache.Get().Get(build.symbols[i].hash);
							if (wearableType == highestAccessory)
							{
								component.AddSymbolOverride(text, build.symbols[i], buildOverridePriority);
								this.animController.SetSymbolVisiblity(text, true);
							}
							else
							{
								component.RemoveSymbolOverride(text, buildOverridePriority);
							}
						}
					}
				}
			}
		}
		this.UpdateVisibleSymbols(highestAccessory);
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000E7094 File Offset: 0x000E5294
	public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		this.UpdateVisibleSymbols(this.ConvertOutfitTypeToWearableType(outfitType));
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x000E70C0 File Offset: 0x000E52C0
	private void UpdateVisibleSymbols(WearableAccessorizer.WearableType wearableType)
	{
		bool flag = wearableType == WearableAccessorizer.WearableType.Basic;
		bool hasHat = base.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
		bool flag2 = false;
		bool is_visible = false;
		bool is_visible2 = true;
		bool is_visible3 = wearableType == WearableAccessorizer.WearableType.Basic;
		bool is_visible4 = wearableType == WearableAccessorizer.WearableType.Basic;
		if (this.wearables.ContainsKey(wearableType))
		{
			List<KAnimHashedString> list = this.wearables[wearableType].BuildAnims.SelectMany((KAnimFile x) => from s in x.GetData().build.symbols
			select s.hash).ToList<KAnimHashedString>();
			flag = (flag || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId));
			flag2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
			is_visible = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
			is_visible2 = (list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible3 = (list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible4 = (list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms)));
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, flag);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible2);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible3);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, is_visible4);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, flag2);
		if (flag2 || flag)
		{
			this.SkirtHACK(wearableType);
		}
		WearableAccessorizer.UpdateHairBasedOnHat(this.animController, hasHat);
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x000E7320 File Offset: 0x000E5520
	private void SkirtHACK(WearableAccessorizer.WearableType wearable_type)
	{
		if (this.wearables.ContainsKey(wearable_type))
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			WearableAccessorizer.Wearable wearable = this.wearables[wearable_type];
			int buildOverridePriority = wearable.buildOverridePriority;
			foreach (KAnimFile kanimFile in wearable.BuildAnims)
			{
				foreach (KAnim.Build.Symbol symbol in kanimFile.GetData().build.symbols)
				{
					if (HashCache.Get().Get(symbol.hash).EndsWith(WearableAccessorizer.cropped))
					{
						component.AddSymbolOverride(WearableAccessorizer.torso, symbol, buildOverridePriority);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000E73F0 File Offset: 0x000E55F0
	public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
	{
		if (hasHat)
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
			return;
		}
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000E74A3 File Offset: 0x000E56A3
	public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
	{
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x000E74E0 File Offset: 0x000E56E0
	private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
	{
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		KAnim.Build build = (animFile != null) ? animFile.GetData().build : null;
		if (build != null)
		{
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string s = HashCache.Get().Get(build.symbols[i].hash);
				component.RemoveSymbolOverride(s, override_priority);
			}
		}
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x000E7548 File Offset: 0x000E5748
	private void UnequippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.RemoveEquipment(component);
		}
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x000E7574 File Offset: 0x000E5774
	public void RemoveEquipment(Equippable equippable)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2) && this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomSuit))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[key2])
				{
					this.RemoveAnimBuild(resourceRef.Get().AnimFile, this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				}
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				this.wearables.Remove(WearableAccessorizer.WearableType.CustomSuit);
			}
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[key].buildOverridePriority);
				this.wearables.Remove(key);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x000E76A8 File Offset: 0x000E58A8
	public void ClearClothingItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair in this.customOutfitItems)
		{
			ClothingOutfitUtility.OutfitType outfitType;
			List<ResourceRef<ClothingItemResource>> list;
			keyValuePair.Deconstruct(out outfitType, out list);
			ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
			if (forOutfitType != null)
			{
				ClothingOutfitUtility.OutfitType? outfitType3 = forOutfitType;
				outfitType = outfitType2;
				if (!(outfitType3.GetValueOrDefault() == outfitType & outfitType3 != null))
				{
					continue;
				}
			}
			this.ApplyClothingItems(outfitType2, Enumerable.Empty<ClothingItemResource>());
		}
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x000E7730 File Offset: 0x000E5930
	public void ApplyClothingItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> items)
	{
		items = items.StableSort(delegate(ClothingItemResource resource)
		{
			if (resource.Category == PermitCategory.DupeTops)
			{
				return 10;
			}
			if (resource.Category == PermitCategory.DupeGloves)
			{
				return 8;
			}
			if (resource.Category == PermitCategory.DupeBottoms)
			{
				return 7;
			}
			if (resource.Category == PermitCategory.DupeShoes)
			{
				return 6;
			}
			return 1;
		});
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].Clear();
		}
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.wearables.ContainsKey(key))
		{
			foreach (KAnimFile animFile in this.wearables[key].BuildAnims)
			{
				this.RemoveAnimBuild(animFile, this.wearables[key].buildOverridePriority);
			}
			this.wearables[key].ClearAnims();
			if (items.Count<ClothingItemResource>() <= 0)
			{
				this.wearables.Remove(key);
			}
		}
		foreach (ClothingItemResource clothingItem in items)
		{
			this.Internal_ApplyClothingItem(outfitType, clothingItem);
		}
		this.ApplyWearable();
		Equippable suitEquippable = this.GetSuitEquippable();
		bool flag;
		ClothingOutfitUtility.OutfitType outfitType2;
		if (suitEquippable == null && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			flag = true;
		}
		else if (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2))
		{
			this.ApplyEquipment(suitEquippable, suitEquippable.GetBuildOverride());
			flag = (outfitType2 == outfitType);
		}
		else
		{
			flag = false;
		}
		if (!base.GetComponent<MinionIdentity>().IsNullOrDestroyed() && this.animController.materialType != KAnimBatchGroup.MaterialType.UI && flag)
		{
			this.QueueOutfitChangedFX();
		}
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x000E78E0 File Offset: 0x000E5AE0
	private void Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothingItem)
	{
		WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
		if (!this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
		}
		if (!this.customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
		{
			if (this.wearables.ContainsKey(wearableType))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
				{
					this.Internal_RemoveClothingItem(outfitType, resourceRef.Get());
				}
			}
			this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
		}
		bool flag;
		if (base.GetComponent<MinionIdentity>().IsNullOrDestroyed() || this.animController.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			flag = true;
		}
		else if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			flag = true;
		}
		else
		{
			Equippable suitEquippable = this.GetSuitEquippable();
			ClothingOutfitUtility.OutfitType outfitType2;
			flag = (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		}
		if (flag)
		{
			if (!this.wearables.ContainsKey(wearableType))
			{
				int buildOverridePriority = (wearableType == WearableAccessorizer.WearableType.CustomClothing) ? 4 : 6;
				this.wearables[wearableType] = new WearableAccessorizer.Wearable(new List<KAnimFile>(), buildOverridePriority);
			}
			this.wearables[wearableType].AddAnim(clothingItem.AnimFile);
		}
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x000E7A78 File Offset: 0x000E5C78
	private void Internal_RemoveClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothing_item)
	{
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].RemoveAll((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothing_item.IdHash);
		}
		if (this.wearables.ContainsKey(key))
		{
			if (this.wearables[key].RemoveAnim(clothing_item.AnimFile))
			{
				this.RemoveAnimBuild(clothing_item.AnimFile, this.wearables[key].buildOverridePriority);
			}
			if (this.wearables[key].BuildAnims.Count <= 0)
			{
				this.wearables.Remove(key);
			}
		}
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x000E7B3A File Offset: 0x000E5D3A
	private WearableAccessorizer.WearableType ConvertOutfitTypeToWearableType(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			return WearableAccessorizer.WearableType.CustomClothing;
		}
		if (outfitType - ClothingOutfitUtility.OutfitType.AtmoSuit > 1)
		{
			global::Debug.LogWarning("Add a wearable type for clothing outfit type " + outfitType.ToString());
			return WearableAccessorizer.WearableType.Basic;
		}
		return WearableAccessorizer.WearableType.CustomSuit;
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x000E7B68 File Offset: 0x000E5D68
	public void RestoreWearables(Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> stored_wearables, Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
	{
		if (stored_wearables != null)
		{
			this.wearables = stored_wearables;
			foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
			{
				keyValuePair.Value.Deserialize();
			}
		}
		if (clothing != null)
		{
			foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair2 in clothing)
			{
				this.ApplyClothingItems(keyValuePair2.Key, from i in keyValuePair2.Value
				select i.Get());
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x000E7C44 File Offset: 0x000E5E44
	public bool HasPermitCategoryItem(ClothingOutfitUtility.OutfitType wearable_type, PermitCategory category)
	{
		bool result = false;
		if (this.customOutfitItems.ContainsKey(wearable_type))
		{
			result = this.customOutfitItems[wearable_type].Exists((ResourceRef<ClothingItemResource> resource) => resource.Get().Category == category);
		}
		return result;
	}

	// Token: 0x0600282D RID: 10285 RVA: 0x000E7C8D File Offset: 0x000E5E8D
	private void QueueOutfitChangedFX()
	{
		this.waitingForOutfitChangeFX = true;
	}

	// Token: 0x0600282E RID: 10286 RVA: 0x000E7C98 File Offset: 0x000E5E98
	private void Update()
	{
		if (this.waitingForOutfitChangeFX && !LockerNavigator.Instance.gameObject.activeInHierarchy)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MinionOutfitChanged, new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.FXFront)), 0f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Changed Clothes", base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			KFMOD.PlayOneShot(GlobalAssets.GetSound("SupplyCloset_Dupe_Clothing_Change", false), base.transform.position, 1f);
			this.waitingForOutfitChangeFX = false;
		}
	}

	// Token: 0x0400178E RID: 6030
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x0400178F RID: 6031
	[Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x04001790 RID: 6032
	[Serialize]
	private string joyResponsePermitId;

	// Token: 0x04001791 RID: 6033
	[Serialize]
	private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x04001792 RID: 6034
	private bool waitingForOutfitChangeFX;

	// Token: 0x04001793 RID: 6035
	[Serialize]
	private Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x04001794 RID: 6036
	private static string torso = "torso";

	// Token: 0x04001795 RID: 6037
	private static string cropped = "_cropped";

	// Token: 0x02001543 RID: 5443
	public enum WearableType
	{
		// Token: 0x04007154 RID: 29012
		Basic,
		// Token: 0x04007155 RID: 29013
		CustomClothing,
		// Token: 0x04007156 RID: 29014
		Outfit,
		// Token: 0x04007157 RID: 29015
		Suit,
		// Token: 0x04007158 RID: 29016
		CustomSuit
	}

	// Token: 0x02001544 RID: 5444
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Wearable
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060092C9 RID: 37577 RVA: 0x003748C6 File Offset: 0x00372AC6
		public List<KAnimFile> BuildAnims
		{
			get
			{
				return this.buildAnims;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060092CA RID: 37578 RVA: 0x003748CE File Offset: 0x00372ACE
		public List<string> AnimNames
		{
			get
			{
				return this.animNames;
			}
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x003748D8 File Offset: 0x00372AD8
		public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
		{
			this.buildAnims = buildAnims;
			this.animNames = (from animFile in buildAnims
			select animFile.name).ToList<string>();
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x00374929 File Offset: 0x00372B29
		public Wearable(KAnimFile buildAnim, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>
			{
				buildAnim
			};
			this.animNames = new List<string>
			{
				buildAnim.name
			};
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x060092CD RID: 37581 RVA: 0x00374964 File Offset: 0x00372B64
		public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>();
			this.animNames = new List<string>();
			this.buildOverridePriority = buildOverridePriority;
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x060092CE RID: 37582 RVA: 0x003749F8 File Offset: 0x00372BF8
		public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
		{
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x060092CF RID: 37583 RVA: 0x00374A68 File Offset: 0x00372C68
		public void Deserialize()
		{
			if (this.animNames != null)
			{
				this.buildAnims = new List<KAnimFile>();
				for (int i = 0; i < this.animNames.Count; i++)
				{
					KAnimFile item = null;
					if (Assets.TryGetAnim(this.animNames[i], out item))
					{
						this.buildAnims.Add(item);
					}
				}
			}
		}

		// Token: 0x060092D0 RID: 37584 RVA: 0x00374AC6 File Offset: 0x00372CC6
		public void AddAnim(KAnimFile animFile)
		{
			this.buildAnims.Add(animFile);
			this.animNames.Add(animFile.name);
		}

		// Token: 0x060092D1 RID: 37585 RVA: 0x00374AE5 File Offset: 0x00372CE5
		public bool RemoveAnim(KAnimFile animFile)
		{
			return this.buildAnims.Remove(animFile) | this.animNames.Remove(animFile.name);
		}

		// Token: 0x060092D2 RID: 37586 RVA: 0x00374B05 File Offset: 0x00372D05
		public void ClearAnims()
		{
			this.buildAnims.Clear();
			this.animNames.Clear();
		}

		// Token: 0x04007159 RID: 29017
		private List<KAnimFile> buildAnims;

		// Token: 0x0400715A RID: 29018
		[Serialize]
		private List<string> animNames;

		// Token: 0x0400715B RID: 29019
		[Serialize]
		public int buildOverridePriority;
	}
}
