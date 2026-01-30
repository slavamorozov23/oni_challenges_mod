using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
public class BionicMinionStorageExtension : KMonoBehaviour, StoredMinionIdentity.IStoredMinionExtension
{
	// Token: 0x06002BA0 RID: 11168 RVA: 0x000FE848 File Offset: 0x000FCA48
	public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
	{
		Storage[] components = storedMinionGameObject.GetComponents<Storage>();
		using (List<Tag>.Enumerator enumerator = BionicMinionStorageExtension.StoragesTypesToTransfer.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag inventoryType = enumerator.Current;
				if (components == null || !(components.FindFirst((Storage s) => s.storageID == inventoryType) != null))
				{
					Storage storage = storedMinionGameObject.AddComponent<Storage>();
					storage.allowItemRemoval = false;
					storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
					storage.storageID = inventoryType;
				}
			}
		}
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000FE8E8 File Offset: 0x000FCAE8
	void StoredMinionIdentity.IStoredMinionExtension.PullFrom(StoredMinionIdentity source)
	{
		Storage[] components = source.GetComponents<Storage>();
		Storage[] components2 = base.GetComponents<Storage>();
		foreach (Storage storage in components)
		{
			bool test = false;
			foreach (Storage storage2 in components2)
			{
				if (storage2.storageID == storage.storageID)
				{
					storage.Transfer(storage2, false, true);
					test = true;
					break;
				}
			}
			DebugUtil.DevAssert(test, "Missmatched storages on BionicMinionStorageExtension", null);
		}
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000FE968 File Offset: 0x000FCB68
	void StoredMinionIdentity.IStoredMinionExtension.PushTo(StoredMinionIdentity destination)
	{
		GameObject gameObject = destination.gameObject;
		this.AddStoredMinionGameObjectRequirements(gameObject);
		Storage[] components = base.GetComponents<Storage>();
		Storage[] components2 = gameObject.GetComponents<Storage>();
		foreach (Tag b in BionicMinionStorageExtension.StoragesTypesToTransfer)
		{
			Storage storage = null;
			Storage target = null;
			foreach (Storage storage2 in components)
			{
				if (storage2.storageID == b)
				{
					storage = storage2;
					break;
				}
			}
			foreach (Storage storage3 in components2)
			{
				if (storage3.storageID == b)
				{
					target = storage3;
					break;
				}
			}
			storage.Transfer(target, true, true);
		}
	}

	// Token: 0x04001A00 RID: 6656
	private static readonly List<Tag> StoragesTypesToTransfer = new List<Tag>
	{
		GameTags.StoragesIds.BionicBatteryStorage,
		GameTags.StoragesIds.BionicUpgradeStorage,
		GameTags.StoragesIds.BionicOxygenTankStorage
	};
}
