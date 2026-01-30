using System;
using System.Collections.Generic;

// Token: 0x02000845 RID: 2117
public class CustomClothingOutfits
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060039EA RID: 14826 RVA: 0x0014398A File Offset: 0x00141B8A
	public static CustomClothingOutfits Instance
	{
		get
		{
			if (CustomClothingOutfits._instance == null)
			{
				CustomClothingOutfits._instance = new CustomClothingOutfits();
			}
			return CustomClothingOutfits._instance;
		}
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x001439A2 File Offset: 0x00141BA2
	public SerializableOutfitData.Version2 Internal_GetOutfitData()
	{
		return this.serializableOutfitData;
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x001439AA File Offset: 0x00141BAA
	public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
	{
		this.serializableOutfitData = data;
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x001439B4 File Offset: 0x00141BB4
	public void Internal_EditOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name, string[] outfit_items)
	{
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out customTemplateOutfitEntry))
		{
			customTemplateOutfitEntry = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry();
			customTemplateOutfitEntry.outfitType = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			customTemplateOutfitEntry.itemIds = outfit_items;
			this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = customTemplateOutfitEntry;
		}
		else
		{
			ClothingOutfitUtility.OutfitType outfitType;
			if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"Cannot edit outfit \"",
					outfit_name,
					"\" of unknown outfit type \"",
					customTemplateOutfitEntry.outfitType,
					"\""
				}));
			}
			if (outfitType != outfit_type)
			{
				throw new NotSupportedException(string.Format("Cannot edit outfit \"{0}\" of outfit type \"{1}\" to be an outfit of type \"{2}\"", outfit_name, customTemplateOutfitEntry.outfitType, outfit_type));
			}
			customTemplateOutfitEntry.itemIds = outfit_items;
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x00143A88 File Offset: 0x00141C88
	public void Internal_RenameOutfit(ClothingOutfitUtility.OutfitType outfit_type, string old_outfit_name, string new_outfit_name)
	{
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": missing \"",
				old_outfit_name,
				"\" entry"
			}));
		}
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": entry \"",
				new_outfit_name,
				"\" already exists"
			}));
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
		foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			keyValuePair.Deconstruct(out text, out dictionary);
			Dictionary<string, string> dictionary2 = dictionary;
			if (dictionary2 != null)
			{
				using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
					{
						string a;
						keyValuePair2.Deconstruct(out text, out a);
						string item = text;
						if (a == old_outfit_name)
						{
							pooledList.Add(item);
						}
					}
					foreach (string key in pooledList)
					{
						dictionary2[key] = new_outfit_name;
					}
				}
			}
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x00143C88 File Offset: 0x00141E88
	public void Internal_RemoveOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name)
	{
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
			{
				string text;
				Dictionary<string, string> dictionary;
				keyValuePair.Deconstruct(out text, out dictionary);
				Dictionary<string, string> dictionary2 = dictionary;
				if (dictionary2 != null)
				{
					using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
					{
						foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
						{
							string a;
							keyValuePair2.Deconstruct(out text, out a);
							string item = text;
							if (a == outfit_name)
							{
								pooledList.Add(item);
							}
						}
						foreach (string key in pooledList)
						{
							dictionary2.Remove(key);
						}
					}
				}
			}
			ClothingOutfitUtility.SaveClothingOutfitData();
		}
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x00143DCC File Offset: 0x00141FCC
	public bool Internal_TryGetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, out string outfitId)
	{
		if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
		{
			string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			if (this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId].ContainsKey(name))
			{
				outfitId = this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name];
				return true;
			}
		}
		outfitId = null;
		return false;
	}

	// Token: 0x060039F1 RID: 14833 RVA: 0x00143E3C File Offset: 0x0014203C
	public void Internal_SetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, Option<string> outfit_id)
	{
		string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
		Dictionary<string, string> dictionary;
		if (outfit_id.HasValue)
		{
			if (!this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
			}
			this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
		}
		else if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out dictionary))
		{
			dictionary.Remove(name);
			if (dictionary.Count == 0)
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x04002365 RID: 9061
	private static CustomClothingOutfits _instance;

	// Token: 0x04002366 RID: 9062
	private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();
}
