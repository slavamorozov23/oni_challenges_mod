using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000632 RID: 1586
[AddComponentMenu("KMonoBehaviour/scripts/SaveManager")]
public class SaveManager : KMonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060025DE RID: 9694 RVA: 0x000D9C00 File Offset: 0x000D7E00
	// (remove) Token: 0x060025DF RID: 9695 RVA: 0x000D9C38 File Offset: 0x000D7E38
	public event Action<SaveLoadRoot> onRegister;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060025E0 RID: 9696 RVA: 0x000D9C70 File Offset: 0x000D7E70
	// (remove) Token: 0x060025E1 RID: 9697 RVA: 0x000D9CA8 File Offset: 0x000D7EA8
	public event Action<SaveLoadRoot> onUnregister;

	// Token: 0x060025E2 RID: 9698 RVA: 0x000D9CDD File Offset: 0x000D7EDD
	protected override void OnPrefabInit()
	{
		Assets.RegisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000D9CF0 File Offset: 0x000D7EF0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Assets.UnregisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x000D9D0C File Offset: 0x000D7F0C
	private void OnAddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		Tag saveLoadTag = prefab.GetSaveLoadTag();
		this.prefabMap[saveLoadTag] = prefab.gameObject;
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x000D9D3C File Offset: 0x000D7F3C
	public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
	{
		return this.sceneObjects;
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x000D9D44 File Offset: 0x000D7F44
	private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
	{
		KPrefabID component = saver.GetComponent<KPrefabID>();
		if (component == null)
		{
			DebugUtil.LogErrorArgs(saver.gameObject, new object[]
			{
				"All savers must also have a KPrefabID on them but",
				saver.gameObject.name,
				"does not have one."
			});
			return null;
		}
		List<SaveLoadRoot> list;
		if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out list))
		{
			list = new List<SaveLoadRoot>();
			this.sceneObjects[component.GetSaveLoadTag()] = list;
		}
		return list;
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x000D9DC0 File Offset: 0x000D7FC0
	public void Register(SaveLoadRoot root)
	{
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Add(root);
		if (this.onRegister != null)
		{
			this.onRegister(root);
		}
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x000D9DF4 File Offset: 0x000D7FF4
	public void Unregister(SaveLoadRoot root)
	{
		if (this.onRegister != null)
		{
			this.onUnregister(root);
		}
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Remove(root);
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x000D9E2C File Offset: 0x000D802C
	public GameObject GetPrefab(Tag tag)
	{
		GameObject result = null;
		if (this.prefabMap.TryGetValue(tag, out result))
		{
			return result;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Item not found in prefabMap",
			"[" + tag.Name + "]"
		});
		return null;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000D9E7C File Offset: 0x000D807C
	private void SortAssociatedObjects(ref List<Tag> objectTags, List<Tag> associatedTags)
	{
		int num = objectTags.FindIndex((Tag t) => associatedTags.Contains(t));
		if (num >= 0)
		{
			Tag b = objectTags[num];
			foreach (Tag tag in associatedTags)
			{
				if (tag != b && objectTags.Contains(tag))
				{
					objectTags.Remove(tag);
					objectTags.Insert(num + 1, tag);
				}
			}
		}
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x000D9F24 File Offset: 0x000D8124
	public void Save(BinaryWriter writer)
	{
		writer.Write(SaveManager.SAVE_HEADER);
		writer.Write(7);
		writer.Write(37);
		int num = 0;
		Dictionary<Tag, List<Tag>> dictionary = new Dictionary<Tag, List<Tag>>();
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
				if (keyValuePair.Value[0].associatedTag != Tag.Invalid)
				{
					if (!dictionary.ContainsKey(keyValuePair.Value[0].associatedTag))
					{
						dictionary.Add(keyValuePair.Value[0].associatedTag, new List<Tag>());
					}
					dictionary[keyValuePair.Value[0].associatedTag].Add(keyValuePair.Key);
				}
			}
		}
		writer.Write(num);
		this.orderedKeys.Clear();
		this.orderedKeys.AddRange(this.sceneObjects.Keys);
		this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name == "StickerBomb"
		select a).ToList<Tag>();
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name.Contains("UnderConstruction")
		select a).ToList<Tag>();
		foreach (KeyValuePair<Tag, List<Tag>> keyValuePair2 in dictionary)
		{
			this.SortAssociatedObjects(ref this.orderedKeys, keyValuePair2.Value);
		}
		this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>(new SaveLoadRoot[]
		{
			SaveGame.Instance.GetComponent<SaveLoadRoot>()
		}), writer);
		foreach (Tag key in this.orderedKeys)
		{
			List<SaveLoadRoot> list = this.sceneObjects[key];
			if (list.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot in list)
				{
					if (!(saveLoadRoot == null) && saveLoadRoot.GetComponent<SimCellOccupier>() != null)
					{
						this.Write(key, list, writer);
						break;
					}
				}
			}
		}
		foreach (Tag key2 in this.orderedKeys)
		{
			List<SaveLoadRoot> list2 = this.sceneObjects[key2];
			if (list2.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot2 in list2)
				{
					if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<SimCellOccupier>() == null)
					{
						this.Write(key2, list2, writer);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x000DA2AC File Offset: 0x000D84AC
	private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
	{
		int count = value.Count;
		Tag tag = key;
		writer.WriteKleiString(tag.Name);
		writer.Write(count);
		long position = writer.BaseStream.Position;
		int value2 = -1;
		writer.Write(value2);
		long position2 = writer.BaseStream.Position;
		foreach (SaveLoadRoot saveLoadRoot in value)
		{
			if (saveLoadRoot != null)
			{
				saveLoadRoot.Save(writer);
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Null game object when saving"
				});
			}
		}
		long position3 = writer.BaseStream.Position;
		long num = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x000DA394 File Offset: 0x000D8594
	public bool Load(IReader reader)
	{
		char[] array = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
		if (array == null || array.Length != SaveManager.SAVE_HEADER.Length)
		{
			return false;
		}
		for (int i = 0; i < SaveManager.SAVE_HEADER.Length; i++)
		{
			if (array[i] != SaveManager.SAVE_HEADER[i])
			{
				return false;
			}
		}
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		if (num != 7 || num2 > 37)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", new object[]
				{
					7,
					37,
					num,
					num2
				})
			});
			return false;
		}
		this.ClearScene();
		try
		{
			int num3 = reader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadKleiString();
				int num4 = reader.ReadInt32();
				int length = reader.ReadInt32();
				Tag key = TagManager.Create(text);
				GameObject prefab;
				if (!this.prefabMap.TryGetValue(key, out prefab))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not find prefab '" + text + "'"
					});
					reader.SkipBytes(length);
				}
				else
				{
					List<SaveLoadRoot> value = new List<SaveLoadRoot>(num4);
					this.sceneObjects[key] = value;
					for (int k = 0; k < num4; k++)
					{
						SaveLoadRoot x = SaveLoadRoot.Load(prefab, reader);
						if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && x == null)
						{
							global::Debug.LogError("Error loading data [" + text + "]");
							return false;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Error deserializing prefabs\n\n",
				ex.ToString()
			});
			throw ex;
		}
		return true;
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x000DA558 File Offset: 0x000D8758
	private void ClearScene()
	{
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			foreach (SaveLoadRoot saveLoadRoot in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(saveLoadRoot.gameObject);
			}
		}
		this.sceneObjects.Clear();
	}

	// Token: 0x04001635 RID: 5685
	public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x04001636 RID: 5686
	public const int SAVE_MAJOR_VERSION = 7;

	// Token: 0x04001637 RID: 5687
	public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;

	// Token: 0x04001638 RID: 5688
	public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x04001639 RID: 5689
	public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;

	// Token: 0x0400163A RID: 5690
	public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;

	// Token: 0x0400163B RID: 5691
	public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;

	// Token: 0x0400163C RID: 5692
	public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;

	// Token: 0x0400163D RID: 5693
	public const int SAVE_MINOR_VERSION_EXPANDED_WORLD_INFO = 12;

	// Token: 0x0400163E RID: 5694
	public const int SAVE_MINOR_VERSION_BASIC_COMFORTS_FIX = 13;

	// Token: 0x0400163F RID: 5695
	public const int SAVE_MINOR_VERSION_PLATFORM_TRAIT_NAMES = 14;

	// Token: 0x04001640 RID: 5696
	public const int SAVE_MINOR_VERSION_ADD_JOY_REACTIONS = 15;

	// Token: 0x04001641 RID: 5697
	public const int SAVE_MINOR_VERSION_NEW_AUTOMATION_WARNING = 16;

	// Token: 0x04001642 RID: 5698
	public const int SAVE_MINOR_VERSION_ADD_GUID_TO_HEADER = 17;

	// Token: 0x04001643 RID: 5699
	public const int SAVE_MINOR_VERSION_EXPANSION_1_INTRODUCED = 20;

	// Token: 0x04001644 RID: 5700
	public const int SAVE_MINOR_VERSION_CONTENT_SETTINGS = 21;

	// Token: 0x04001645 RID: 5701
	public const int SAVE_MINOR_VERSION_COLONY_REQ_REMOVE_SERIALIZATION = 22;

	// Token: 0x04001646 RID: 5702
	public const int SAVE_MINOR_VERSION_ROTTABLE_TUNING = 23;

	// Token: 0x04001647 RID: 5703
	public const int SAVE_MINOR_VERSION_LAUNCH_PAD_SOLIDITY = 24;

	// Token: 0x04001648 RID: 5704
	public const int SAVE_MINOR_VERSION_BASE_GAME_MERGEDOWN = 25;

	// Token: 0x04001649 RID: 5705
	public const int SAVE_MINOR_VERSION_FALLING_WATER_WORLDIDX_SERIALIZATION = 26;

	// Token: 0x0400164A RID: 5706
	public const int SAVE_MINOR_VERSION_ROCKET_RANGE_REBALANCE = 27;

	// Token: 0x0400164B RID: 5707
	public const int SAVE_MINOR_VERSION_ENTITIES_WRONG_LAYER = 28;

	// Token: 0x0400164C RID: 5708
	public const int SAVE_MINOR_VERSION_TAGBITS_REWORK = 29;

	// Token: 0x0400164D RID: 5709
	public const int SAVE_MINOR_VERSION_ACCESSORY_SLOT_UPGRADE = 30;

	// Token: 0x0400164E RID: 5710
	public const int SAVE_MINOR_VERSION_GEYSER_CAN_BE_RENAMED = 31;

	// Token: 0x0400164F RID: 5711
	public const int SAVE_MINOR_VERSION_SPACE_SCANNERS_TELESCOPES = 32;

	// Token: 0x04001650 RID: 5712
	public const int SAVE_MINOR_VERSION_U50_CRITTERS = 33;

	// Token: 0x04001651 RID: 5713
	public const int SAVE_MINOR_VERSION_DLC_ADD_ONS = 34;

	// Token: 0x04001652 RID: 5714
	public const int SAVE_MINOR_VERSION_U53_SCHEDULES = 35;

	// Token: 0x04001653 RID: 5715
	public const int SAVE_MINOR_VERSION_POKESHELL_MOLTS = 36;

	// Token: 0x04001654 RID: 5716
	public const int SAVE_MINOR_VERSION_TECH_COMPONENTS = 37;

	// Token: 0x04001655 RID: 5717
	public const int SAVE_MINOR_VERSION = 37;

	// Token: 0x04001656 RID: 5718
	private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();

	// Token: 0x04001657 RID: 5719
	private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();

	// Token: 0x0400165A RID: 5722
	public static int DEBUG_OnlyLoadThisCellsObjects = -1;

	// Token: 0x0400165B RID: 5723
	private static readonly char[] SAVE_HEADER = new char[]
	{
		'K',
		'S',
		'A',
		'V'
	};

	// Token: 0x0400165C RID: 5724
	private List<Tag> orderedKeys = new List<Tag>();

	// Token: 0x02001510 RID: 5392
	private enum BoundaryTag : uint
	{
		// Token: 0x04007097 RID: 28823
		Component = 3735928559U,
		// Token: 0x04007098 RID: 28824
		Prefab = 3131961357U,
		// Token: 0x04007099 RID: 28825
		Complete = 3735929054U
	}
}
