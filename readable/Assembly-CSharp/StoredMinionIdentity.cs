using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000644 RID: 1604
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StoredMinionIdentity")]
public class StoredMinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, IPersonalPriorityManager
{
	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000E0207 File Offset: 0x000DE407
	// (set) Token: 0x060026EA RID: 9962 RVA: 0x000E020F File Offset: 0x000DE40F
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060026EB RID: 9963 RVA: 0x000E0218 File Offset: 0x000DE418
	// (set) Token: 0x060026EC RID: 9964 RVA: 0x000E0220 File Offset: 0x000DE420
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060026ED RID: 9965 RVA: 0x000E0229 File Offset: 0x000DE429
	// (set) Token: 0x060026EE RID: 9966 RVA: 0x000E0231 File Offset: 0x000DE431
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x060026EF RID: 9967 RVA: 0x000E023C File Offset: 0x000DE43C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					num++;
				}
			}
			this.TotalExperienceGained = MinionResume.CalculatePreviousExperienceBar(num);
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
		if (!this.model.IsValid)
		{
			this.model = MinionConfig.MODEL;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
		}
		List<ResourceRef<Accessory>> list = this.accessories.FindAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		if (list.Count > 0)
		{
			List<ClothingItemResource> list2 = new List<ClothingItemResource>();
			foreach (ResourceRef<Accessory> resourceRef in list)
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryResolveAccessoryResource(resourceRef.Guid);
				if (clothingItemResource != null && !list2.Contains(clothingItemResource))
				{
					list2.Add(clothingItemResource);
					this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing].Add(new ResourceRef<ClothingItemResource>(clothingItemResource));
				}
			}
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		this.OnDeserializeModifiers();
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x000E04AC File Offset: 0x000DE6AC
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).perks.Contains(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x000E052C File Offset: 0x000DE72C
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x000E054A File Offset: 0x000DE74A
	protected override void OnPrefabInit()
	{
		this.assignableProxy = new Ref<MinionAssignablesProxy>();
		this.minionModifiers = base.GetComponent<MinionModifiers>();
		this.savedAttributeValues = new Dictionary<string, float>();
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x000E0570 File Offset: 0x000DE770
	[OnSerializing]
	private void OnSerialize()
	{
		this.savedAttributeValues.Clear();
		foreach (AttributeInstance attributeInstance in this.minionModifiers.attributes)
		{
			this.savedAttributeValues.Add(attributeInstance.Attribute.Id, attributeInstance.GetTotalValue());
		}
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000E05E4 File Offset: 0x000DE7E4
	protected override void OnSpawn()
	{
		string[] attributes = MinionConfig.GetAttributes();
		string[] amounts = MinionConfig.GetAmounts();
		AttributeModifier[] traits = MinionConfig.GetTraits();
		if (this.model == BionicMinionConfig.MODEL)
		{
			attributes = BionicMinionConfig.GetAttributes();
			amounts = BionicMinionConfig.GetAmounts();
			traits = BionicMinionConfig.GetTraits();
		}
		BaseMinionConfig.AddMinionAttributes(this.minionModifiers, attributes);
		BaseMinionConfig.AddMinionAmounts(this.minionModifiers, amounts);
		BaseMinionConfig.AddMinionTraits(BaseMinionConfig.GetMinionNameForModel(this.model), BaseMinionConfig.GetMinionBaseTraitIDForModel(this.model), this.minionModifiers, traits);
		this.ValidateProxy();
		this.CleanupLimboMinions();
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x000E066D File Offset: 0x000DE86D
	public void OnHardDelete()
	{
		if (this.assignableProxy.Get() != null)
		{
			Util.KDestroyGameObject(this.assignableProxy.Get().gameObject);
		}
		ScheduleManager.Instance.OnStoredDupeDestroyed(this);
		Components.StoredMinionIdentities.Remove(this);
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000E06B0 File Offset: 0x000DE8B0
	private void OnDeserializeModifiers()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.savedAttributeValues)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(keyValuePair.Key);
			if (attribute == null)
			{
				attribute = Db.Get().BuildingAttributes.TryGet(keyValuePair.Key);
			}
			if (attribute != null)
			{
				if (this.minionModifiers.attributes.Get(attribute.Id) != null)
				{
					this.minionModifiers.attributes.Get(attribute.Id).Modifiers.Clear();
					this.minionModifiers.attributes.Get(attribute.Id).ClearModifiers();
				}
				else
				{
					this.minionModifiers.attributes.Add(attribute);
				}
				this.minionModifiers.attributes.Add(new AttributeModifier(attribute.Id, keyValuePair.Value, () => DUPLICANTS.ATTRIBUTES.STORED_VALUE, false, false));
			}
		}
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x000E07E4 File Offset: 0x000DE9E4
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x000E07F8 File Offset: 0x000DE9F8
	public string[] GetClothingItemIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customClothingItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customClothingItems[outfitType].Count];
			for (int i = 0; i < this.customClothingItems[outfitType].Count; i++)
			{
				array[i] = this.customClothingItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return null;
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x000E0868 File Offset: 0x000DEA68
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = false;
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Stored minion with an invalid kpid! Attempting to recover...",
				this.storedName
			});
			flag = true;
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.storedName
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
		bool flag2 = false;
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
			for (int i = 0; i < storedMinionInfo.Count; i++)
			{
				MinionStorage.Info info = storedMinionInfo[i];
				if (flag && info.serializedMinion != null && info.serializedMinion.GetId() == -1 && info.name == this.storedName)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Found a minion storage with an invalid ref, rebinding.",
						component.InstanceID,
						this.storedName,
						minionStorage.gameObject.name
					});
					info = new MinionStorage.Info(this.storedName, new Ref<KPrefabID>(component));
					storedMinionInfo[i] = info;
					minionStorage.GetComponent<Assignable>().Assign(this);
					flag2 = true;
					break;
				}
				if (info.serializedMinion != null && info.serializedMinion.Get() == component)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (!flag2)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Found a stored minion that wasn't in any minion storage. Respawning them at the portal.",
				component.InstanceID,
				this.storedName
			});
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				MinionStorage.DeserializeMinion(component.gameObject, activeTelepad.transform.GetPosition());
			}
		}
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000E0B48 File Offset: 0x000DED48
	public string GetProperName()
	{
		return this.storedName;
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000E0B50 File Offset: 0x000DED50
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000E0B62 File Offset: 0x000DED62
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000E0B74 File Offset: 0x000DED74
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x000E0B87 File Offset: 0x000DED87
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x000E0B94 File Offset: 0x000DED94
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x000E0BF6 File Offset: 0x000DEDF6
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000E0C00 File Offset: 0x000DEE00
	public string GetStorageReason()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			using (List<MinionStorage.Info>.Enumerator enumerator2 = minionStorage.GetStoredMinionInfo().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.serializedMinion.Get() == component)
					{
						return minionStorage.GetProperName();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000E0CB8 File Offset: 0x000DEEB8
	public bool IsPermittedToConsume(string consumable)
	{
		return !this.forbiddenTagSet.Contains(consumable);
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000E0CD0 File Offset: 0x000DEED0
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		foreach (string id in this.traitIDs)
		{
			if (Db.Get().traits.Exists(id))
			{
				Trait trait = Db.Get().traits.Get(id);
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == chore_group.IdHash)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000E0D80 File Offset: 0x000DEF80
	public int GetPersonalPriority(ChoreGroup chore_group)
	{
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(chore_group.IdHash, out priorityInfo))
		{
			return priorityInfo.priority;
		}
		return 0;
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000E0DAA File Offset: 0x000DEFAA
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000E0DAD File Offset: 0x000DEFAD
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000E0DAF File Offset: 0x000DEFAF
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x040016F3 RID: 5875
	[Serialize]
	public string storedName;

	// Token: 0x040016F4 RID: 5876
	[Serialize]
	public Tag model;

	// Token: 0x040016F5 RID: 5877
	[Serialize]
	public string gender;

	// Token: 0x040016F9 RID: 5881
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x040016FA RID: 5882
	[Serialize]
	public int voiceIdx;

	// Token: 0x040016FB RID: 5883
	[Serialize]
	public KCompBuilder.BodyData bodyData;

	// Token: 0x040016FC RID: 5884
	[Serialize]
	public List<Ref<KPrefabID>> assignedItems;

	// Token: 0x040016FD RID: 5885
	[Serialize]
	public List<Ref<KPrefabID>> equippedItems;

	// Token: 0x040016FE RID: 5886
	[Serialize]
	public List<string> traitIDs;

	// Token: 0x040016FF RID: 5887
	[Serialize]
	public List<ResourceRef<Accessory>> accessories;

	// Token: 0x04001700 RID: 5888
	[Obsolete("Deprecated, use customClothingItems")]
	[Serialize]
	public List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x04001701 RID: 5889
	[Serialize]
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customClothingItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x04001702 RID: 5890
	[Serialize]
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x04001703 RID: 5891
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public List<Tag> forbiddenTags;

	// Token: 0x04001704 RID: 5892
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x04001705 RID: 5893
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x04001706 RID: 5894
	[Serialize]
	public List<Effects.SaveLoadEffect> saveLoadEffects;

	// Token: 0x04001707 RID: 5895
	[Serialize]
	public List<Effects.SaveLoadImmunities> saveLoadImmunities;

	// Token: 0x04001708 RID: 5896
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x04001709 RID: 5897
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x0400170A RID: 5898
	[Serialize]
	public List<string> grantedSkillIDs = new List<string>();

	// Token: 0x0400170B RID: 5899
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x0400170C RID: 5900
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x0400170D RID: 5901
	[Serialize]
	public float TotalExperienceGained;

	// Token: 0x0400170E RID: 5902
	[Serialize]
	public string currentHat;

	// Token: 0x0400170F RID: 5903
	[Serialize]
	public string targetHat;

	// Token: 0x04001710 RID: 5904
	[Serialize]
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x04001711 RID: 5905
	[Serialize]
	public List<AttributeLevels.LevelSaveLoad> attributeLevels;

	// Token: 0x04001712 RID: 5906
	[Serialize]
	public Dictionary<string, float> savedAttributeValues;

	// Token: 0x04001713 RID: 5907
	public MinionModifiers minionModifiers;

	// Token: 0x02001524 RID: 5412
	public interface IStoredMinionExtension
	{
		// Token: 0x06009250 RID: 37456
		void PushTo(StoredMinionIdentity destination);

		// Token: 0x06009251 RID: 37457
		void PullFrom(StoredMinionIdentity source);

		// Token: 0x06009252 RID: 37458
		void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject);
	}
}
