using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000DD1 RID: 3537
public class OutfitDesignerScreen_OutfitState
{
	// Token: 0x06006EB6 RID: 28342 RVA: 0x0029EEA8 File Offset: 0x0029D0A8
	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		this.name = sourceTarget.ReadName();
		this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			this.ApplyItem(item);
		}
	}

	// Token: 0x06006EB7 RID: 28343 RVA: 0x0029EF2C File Offset: 0x0029D12C
	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		global::Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	// Token: 0x06006EB8 RID: 28344 RVA: 0x0029EF48 File Offset: 0x0029D148
	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x0029EF64 File Offset: 0x0029D164
	public unsafe void ApplyItem(ClothingItemResource item)
	{
		*this.slots.GetItemSlotForCategory(item.Category) = item;
	}

	// Token: 0x06006EBA RID: 28346 RVA: 0x0029EF82 File Offset: 0x0029D182
	public unsafe Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return *this.slots.GetItemSlotForCategory(category);
	}

	// Token: 0x06006EBB RID: 28347 RVA: 0x0029EF98 File Offset: 0x0029D198
	public unsafe void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, string.Format("Tried to set clothing item with outfit type \"{0}\" to outfit of type \"{1}\"", item.Unwrap().outfitType, this.outfitType), null);
			DebugUtil.DevAssert(item.Unwrap().Category == category, string.Format("Tried to set clothing item with category \"{0}\" to slot with type \"{1}\"", item.Unwrap().Category, category), null);
		}
		*this.slots.GetItemSlotForCategory(category) = item;
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x0029F038 File Offset: 0x0029D238
	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				clothingItems.Add(ptr.Unwrap());
			}
		}
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x0029F084 File Offset: 0x0029D284
	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				itemIds.Add(ptr.Unwrap().Id);
			}
		}
	}

	// Token: 0x06006EBE RID: 28350 RVA: 0x0029F0D4 File Offset: 0x0029D2D4
	public string[] GetItems()
	{
		List<string> list = new List<string>();
		this.AddItemsTo(list);
		return list.ToArray();
	}

	// Token: 0x06006EBF RID: 28351 RVA: 0x0029F0F4 File Offset: 0x0029D2F4
	public bool DoesContainLockedItems()
	{
		bool result;
		using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList pooledList = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
		{
			this.AddItemsTo(pooledList);
			result = ClothingOutfitTarget.DoesContainLockedItems(pooledList);
		}
		return result;
	}

	// Token: 0x06006EC0 RID: 28352 RVA: 0x0029F134 File Offset: 0x0029D334
	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			this.AddItemsTo(pooledHashSet);
			string[] array = this.destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			foreach (string item in array)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04004BB3 RID: 19379
	public string name;

	// Token: 0x04004BB4 RID: 19380
	private OutfitDesignerScreen_OutfitState.Slots slots;

	// Token: 0x04004BB5 RID: 19381
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x04004BB6 RID: 19382
	public ClothingOutfitTarget sourceTarget;

	// Token: 0x04004BB7 RID: 19383
	public ClothingOutfitTarget destinationTarget;

	// Token: 0x02002033 RID: 8243
	public abstract class Slots
	{
		// Token: 0x0600B88C RID: 47244 RVA: 0x003F63B0 File Offset: 0x003F45B0
		private Slots(int slotsCount)
		{
			this.array = new Option<ClothingItemResource>[slotsCount];
		}

		// Token: 0x0600B88D RID: 47245 RVA: 0x003F63C4 File Offset: 0x003F45C4
		public static OutfitDesignerScreen_OutfitState.Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				return new OutfitDesignerScreen_OutfitState.Slots.Clothing();
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
			case ClothingOutfitUtility.OutfitType.JetSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Jetsuit();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600B88E RID: 47246
		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		// Token: 0x0600B88F RID: 47247 RVA: 0x003F6400 File Offset: 0x003F4600
		private ref Option<ClothingItemResource> FallbackSlot(OutfitDesignerScreen_OutfitState.Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", new object[]
			{
				"Option",
				"ClothingItemResource",
				"PermitCategory",
				category,
				"Slots",
				self.GetType().Name
			}), null);
			return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
		}

		// Token: 0x0400953F RID: 38207
		public Option<ClothingItemResource>[] array;

		// Token: 0x04009540 RID: 38208
		private static Option<ClothingItemResource> dummySlot;

		// Token: 0x02002A7E RID: 10878
		public class Clothing : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600D4DE RID: 54494 RVA: 0x0043D96B File Offset: 0x0043BB6B
			public Clothing() : base(6)
			{
			}

			// Token: 0x17000D87 RID: 3463
			// (get) Token: 0x0600D4DF RID: 54495 RVA: 0x0043D974 File Offset: 0x0043BB74
			public ref Option<ClothingItemResource> hatSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000D88 RID: 3464
			// (get) Token: 0x0600D4E0 RID: 54496 RVA: 0x0043D982 File Offset: 0x0043BB82
			public ref Option<ClothingItemResource> topSlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000D89 RID: 3465
			// (get) Token: 0x0600D4E1 RID: 54497 RVA: 0x0043D990 File Offset: 0x0043BB90
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000D8A RID: 3466
			// (get) Token: 0x0600D4E2 RID: 54498 RVA: 0x0043D99E File Offset: 0x0043BB9E
			public ref Option<ClothingItemResource> bottomSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000D8B RID: 3467
			// (get) Token: 0x0600D4E3 RID: 54499 RVA: 0x0043D9AC File Offset: 0x0043BBAC
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x17000D8C RID: 3468
			// (get) Token: 0x0600D4E4 RID: 54500 RVA: 0x0043D9BA File Offset: 0x0043BBBA
			public ref Option<ClothingItemResource> accessorySlot
			{
				get
				{
					return ref this.array[5];
				}
			}

			// Token: 0x0600D4E5 RID: 54501 RVA: 0x0043D9C8 File Offset: 0x0043BBC8
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.DupeHats)
				{
					return this.hatSlot;
				}
				if (category == PermitCategory.DupeTops)
				{
					return this.topSlot;
				}
				if (category == PermitCategory.DupeGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.DupeBottoms)
				{
					return this.bottomSlot;
				}
				if (category == PermitCategory.DupeShoes)
				{
					return this.shoesSlot;
				}
				if (category == PermitCategory.DupeAccessories)
				{
					return this.accessorySlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x02002A7F RID: 10879
		public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600D4E6 RID: 54502 RVA: 0x0043DA1F File Offset: 0x0043BC1F
			public Atmosuit() : base(5)
			{
			}

			// Token: 0x17000D8D RID: 3469
			// (get) Token: 0x0600D4E7 RID: 54503 RVA: 0x0043DA28 File Offset: 0x0043BC28
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000D8E RID: 3470
			// (get) Token: 0x0600D4E8 RID: 54504 RVA: 0x0043DA36 File Offset: 0x0043BC36
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000D8F RID: 3471
			// (get) Token: 0x0600D4E9 RID: 54505 RVA: 0x0043DA44 File Offset: 0x0043BC44
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000D90 RID: 3472
			// (get) Token: 0x0600D4EA RID: 54506 RVA: 0x0043DA52 File Offset: 0x0043BC52
			public ref Option<ClothingItemResource> beltSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000D91 RID: 3473
			// (get) Token: 0x0600D4EB RID: 54507 RVA: 0x0043DA60 File Offset: 0x0043BC60
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x0600D4EC RID: 54508 RVA: 0x0043DA70 File Offset: 0x0043BC70
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.AtmoSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.AtmoSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.AtmoSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.AtmoSuitBelt)
				{
					return this.beltSlot;
				}
				if (category == PermitCategory.AtmoSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x02002A80 RID: 10880
		public class Jetsuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600D4ED RID: 54509 RVA: 0x0043DABF File Offset: 0x0043BCBF
			public Jetsuit() : base(4)
			{
			}

			// Token: 0x17000D92 RID: 3474
			// (get) Token: 0x0600D4EE RID: 54510 RVA: 0x0043DAC8 File Offset: 0x0043BCC8
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000D93 RID: 3475
			// (get) Token: 0x0600D4EF RID: 54511 RVA: 0x0043DAD6 File Offset: 0x0043BCD6
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000D94 RID: 3476
			// (get) Token: 0x0600D4F0 RID: 54512 RVA: 0x0043DAE4 File Offset: 0x0043BCE4
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000D95 RID: 3477
			// (get) Token: 0x0600D4F1 RID: 54513 RVA: 0x0043DAF2 File Offset: 0x0043BCF2
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x0600D4F2 RID: 54514 RVA: 0x0043DB00 File Offset: 0x0043BD00
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.JetSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.JetSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.JetSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.JetSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}
	}
}
