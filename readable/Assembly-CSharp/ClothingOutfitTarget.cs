using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000848 RID: 2120
public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060039FF RID: 14847 RVA: 0x00144553 File Offset: 0x00142753
	public string OutfitId
	{
		get
		{
			return this.impl.OutfitId;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06003A00 RID: 14848 RVA: 0x00144560 File Offset: 0x00142760
	public ClothingOutfitUtility.OutfitType OutfitType
	{
		get
		{
			return this.impl.OutfitType;
		}
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x0014456D File Offset: 0x0014276D
	public string[] ReadItems()
	{
		return this.impl.ReadItems(this.OutfitType).Where(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x00144596 File Offset: 0x00142796
	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		this.impl.WriteItems(outfitType, items);
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06003A03 RID: 14851 RVA: 0x001445A5 File Offset: 0x001427A5
	public bool CanWriteItems
	{
		get
		{
			return this.impl.CanWriteItems;
		}
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x001445B2 File Offset: 0x001427B2
	public string ReadName()
	{
		return this.impl.ReadName();
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x001445BF File Offset: 0x001427BF
	public void WriteName(string name)
	{
		this.impl.WriteName(name);
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06003A06 RID: 14854 RVA: 0x001445CD File Offset: 0x001427CD
	public bool CanWriteName
	{
		get
		{
			return this.impl.CanWriteName;
		}
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x001445DA File Offset: 0x001427DA
	public void Delete()
	{
		this.impl.Delete();
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06003A08 RID: 14856 RVA: 0x001445E7 File Offset: 0x001427E7
	public bool CanDelete
	{
		get
		{
			return this.impl.CanDelete;
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x001445F4 File Offset: 0x001427F4
	public bool DoesExist()
	{
		return this.impl.DoesExist();
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x00144601 File Offset: 0x00142801
	public ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x0014460A File Offset: 0x0014280A
	public bool DoesContainLockedItems()
	{
		return ClothingOutfitTarget.DoesContainLockedItems(this.ReadItems());
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x00144618 File Offset: 0x00142818
	public static bool DoesContainLockedItems(IList<string> itemIds)
	{
		foreach (string id in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(id);
			if (permitResource != null && !permitResource.IsUnlocked())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x0014467C File Offset: 0x0014287C
	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in this.ReadItems()
		select Db.Get().Permits.ClothingItems.Get(i);
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x001446A8 File Offset: 0x001428A8
	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x001446C7 File Offset: 0x001428C7
	public bool Is<T>() where T : ClothingOutfitTarget.Implementation
	{
		return this.impl is T;
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x001446D8 File Offset: 0x001428D8
	public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
	{
		ClothingOutfitTarget.Implementation implementation = this.impl;
		if (implementation is T)
		{
			T t = (T)((object)implementation);
			value = t;
			return true;
		}
		value = default(T);
		return false;
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0014470C File Offset: 0x0014290C
	public bool IsTemplateOutfit()
	{
		return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0014471E File Offset: 0x0014291E
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0014473F File Offset: 0x0014293F
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (ClothingOutfitTarget.DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id));
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x00144770 File Offset: 0x00142970
	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x001447A3 File Offset: 0x001429A3
	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x001447B8 File Offset: 0x001429B8
	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x001447D4 File Offset: 0x001429D4
	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		ClothingOutfitUtility.OutfitType outfitType;
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out customTemplateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0014486D File Offset: 0x00142A6D
	public static bool DoesTemplateExist(string outfitId)
	{
		return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x001448A2 File Offset: 0x00142AA2
	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource outfit in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit));
		}
		List<ClothingOutfitResource>.Enumerator enumerator = default(List<ClothingOutfitResource>.Enumerator);
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			keyValuePair.Deconstruct(out text, out customTemplateOutfitEntry);
			string outfitId = text;
			ClothingOutfitUtility.OutfitType outfitType;
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				yield return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
			}
		}
		Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator enumerator2 = default(Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x001448AB File Offset: 0x00142AAB
	public static ClothingOutfitTarget GetRandom()
	{
		return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x001448B8 File Offset: 0x00142AB8
	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in ClothingOutfitTarget.GetAllTemplates()
		where t.OutfitType == onlyOfType
		select t;
		if (enumerable == null || enumerable.Count<ClothingOutfitTarget>() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0014490C File Offset: 0x00142B0C
	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string a = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string b = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text;
		if (a != b)
		{
			text = UI.OUTFIT_NAME.RESOLVE_CONFLICT;
		}
		else
		{
			text = "{OutfitName} ({ConflictNumber})";
		}
		for (int i = 1; i < 10000; i++)
		{
			string text2 = text.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!ClothingOutfitTarget.DoesTemplateExist(text2))
			{
				return text2;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x001449DA File Offset: 0x00142BDA
	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x001449E4 File Offset: 0x00142BE4
	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x001449F4 File Offset: 0x00142BF4
	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget)
		{
			ClothingOutfitTarget other = (ClothingOutfitTarget)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x00144A19 File Offset: 0x00142C19
	public bool Equals(ClothingOutfitTarget other)
	{
		if (this.impl == null || other.impl == null)
		{
			return this.impl == null == (other.impl == null);
		}
		return this.OutfitId == other.OutfitId;
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x00144A52 File Offset: 0x00142C52
	public override int GetHashCode()
	{
		return Hash.SDBMLower(this.impl.OutfitId);
	}

	// Token: 0x0400236E RID: 9070
	public readonly ClothingOutfitTarget.Implementation impl;

	// Token: 0x0400236F RID: 9071
	public static readonly string[] NO_ITEMS = new string[0];

	// Token: 0x04002370 RID: 9072
	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	// Token: 0x020017F1 RID: 6129
	public interface Implementation
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06009D00 RID: 40192
		string OutfitId { get; }

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06009D01 RID: 40193
		ClothingOutfitUtility.OutfitType OutfitType { get; }

		// Token: 0x06009D02 RID: 40194
		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x06009D03 RID: 40195
		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06009D04 RID: 40196
		bool CanWriteItems { get; }

		// Token: 0x06009D05 RID: 40197
		string ReadName();

		// Token: 0x06009D06 RID: 40198
		void WriteName(string name);

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06009D07 RID: 40199
		bool CanWriteName { get; }

		// Token: 0x06009D08 RID: 40200
		void Delete();

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06009D09 RID: 40201
		bool CanDelete { get; }

		// Token: 0x06009D0A RID: 40202
		bool DoesExist();
	}

	// Token: 0x020017F2 RID: 6130
	public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06009D0B RID: 40203 RVA: 0x0039C042 File Offset: 0x0039A242
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06009D0C RID: 40204 RVA: 0x0039C045 File Offset: 0x0039A245
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06009D0D RID: 40205 RVA: 0x0039C048 File Offset: 0x0039A248
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009D0E RID: 40206 RVA: 0x0039C04B File Offset: 0x0039A24B
		public bool DoesExist()
		{
			return !this.minionInstance.IsNullOrDestroyed();
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06009D0F RID: 40207 RVA: 0x0039C05C File Offset: 0x0039A25C
		public string OutfitId
		{
			get
			{
				return this.minionInstance.GetInstanceID().ToString() + "_outfit";
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06009D10 RID: 40208 RVA: 0x0039C086 File Offset: 0x0039A286
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x0039C08E File Offset: 0x0039A28E
		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.m_outfitType = outfitType;
			this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06009D12 RID: 40210 RVA: 0x0039C0AA File Offset: 0x0039A2AA
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.accessorizer.GetClothingItemsIds(outfitType);
		}

		// Token: 0x06009D13 RID: 40211 RVA: 0x0039C0B8 File Offset: 0x0039A2B8
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			this.accessorizer.ClearClothingItems(new ClothingOutfitUtility.OutfitType?(outfitType));
			this.accessorizer.ApplyClothingItems(outfitType, from i in items
			select Db.Get().Permits.ClothingItems.Get(i));
		}

		// Token: 0x06009D14 RID: 40212 RVA: 0x0039C107 File Offset: 0x0039A307
		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
		}

		// Token: 0x06009D15 RID: 40213 RVA: 0x0039C123 File Offset: 0x0039A323
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		// Token: 0x06009D16 RID: 40214 RVA: 0x0039C12F File Offset: 0x0039A32F
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}

		// Token: 0x04007938 RID: 31032
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		// Token: 0x04007939 RID: 31033
		public readonly GameObject minionInstance;

		// Token: 0x0400793A RID: 31034
		public readonly WearableAccessorizer accessorizer;
	}

	// Token: 0x020017F3 RID: 6131
	public readonly struct UserAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06009D17 RID: 40215 RVA: 0x0039C13B File Offset: 0x0039A33B
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06009D18 RID: 40216 RVA: 0x0039C13E File Offset: 0x0039A33E
		public bool CanWriteName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06009D19 RID: 40217 RVA: 0x0039C141 File Offset: 0x0039A341
		public bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009D1A RID: 40218 RVA: 0x0039C144 File Offset: 0x0039A344
		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06009D1B RID: 40219 RVA: 0x0039C160 File Offset: 0x0039A360
		public string OutfitId
		{
			get
			{
				return this.m_outfitId[0];
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06009D1C RID: 40220 RVA: 0x0039C16A File Offset: 0x0039A36A
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06009D1D RID: 40221 RVA: 0x0039C172 File Offset: 0x0039A372
		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			this.m_outfitId = new string[]
			{
				outfitId
			};
			this.m_outfitType = outfitType;
		}

		// Token: 0x06009D1E RID: 40222 RVA: 0x0039C18C File Offset: 0x0039A38C
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out customTemplateOutfitEntry))
			{
				ClothingOutfitUtility.OutfitType outfitType2;
				global::Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType2) && outfitType2 == this.m_outfitType);
				return customTemplateOutfitEntry.itemIds;
			}
			return ClothingOutfitTarget.NO_ITEMS;
		}

		// Token: 0x06009D1F RID: 40223 RVA: 0x0039C1E4 File Offset: 0x0039A3E4
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
		}

		// Token: 0x06009D20 RID: 40224 RVA: 0x0039C1F8 File Offset: 0x0039A3F8
		public string ReadName()
		{
			return this.OutfitId;
		}

		// Token: 0x06009D21 RID: 40225 RVA: 0x0039C200 File Offset: 0x0039A400
		public void WriteName(string name)
		{
			if (this.OutfitId == name)
			{
				return;
			}
			if (ClothingOutfitTarget.DoesTemplateExist(name))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Can not change outfit name from \"",
					this.OutfitId,
					"\" to \"",
					name,
					"\", \"",
					name,
					"\" already exists"
				}));
			}
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
			{
				CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
			}
			else
			{
				CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
			}
			this.m_outfitId[0] = name;
		}

		// Token: 0x06009D22 RID: 40226 RVA: 0x0039C2BA File Offset: 0x0039A4BA
		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
		}

		// Token: 0x0400793B RID: 31035
		private readonly string[] m_outfitId;

		// Token: 0x0400793C RID: 31036
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}

	// Token: 0x020017F4 RID: 6132
	public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06009D23 RID: 40227 RVA: 0x0039C2D2 File Offset: 0x0039A4D2
		public bool CanWriteItems
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06009D24 RID: 40228 RVA: 0x0039C2D5 File Offset: 0x0039A4D5
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06009D25 RID: 40229 RVA: 0x0039C2D8 File Offset: 0x0039A4D8
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009D26 RID: 40230 RVA: 0x0039C2DB File Offset: 0x0039A4DB
		public bool DoesExist()
		{
			return true;
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06009D27 RID: 40231 RVA: 0x0039C2DE File Offset: 0x0039A4DE
		public string OutfitId
		{
			get
			{
				return this.m_outfitId;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06009D28 RID: 40232 RVA: 0x0039C2E6 File Offset: 0x0039A4E6
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06009D29 RID: 40233 RVA: 0x0039C2EE File Offset: 0x0039A4EE
		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			this.m_outfitId = outfit.Id;
			this.m_outfitType = outfit.outfitType;
			this.resource = outfit;
		}

		// Token: 0x06009D2A RID: 40234 RVA: 0x0039C30F File Offset: 0x0039A50F
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.resource.itemsInOutfit;
		}

		// Token: 0x06009D2B RID: 40235 RVA: 0x0039C31C File Offset: 0x0039A51C
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		// Token: 0x06009D2C RID: 40236 RVA: 0x0039C328 File Offset: 0x0039A528
		public string ReadName()
		{
			return this.resource.Name;
		}

		// Token: 0x06009D2D RID: 40237 RVA: 0x0039C335 File Offset: 0x0039A535
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		// Token: 0x06009D2E RID: 40238 RVA: 0x0039C341 File Offset: 0x0039A541
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}

		// Token: 0x0400793D RID: 31037
		public readonly ClothingOutfitResource resource;

		// Token: 0x0400793E RID: 31038
		private readonly string m_outfitId;

		// Token: 0x0400793F RID: 31039
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}
}
