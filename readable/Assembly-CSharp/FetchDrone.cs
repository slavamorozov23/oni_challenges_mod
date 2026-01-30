using System;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class FetchDrone : KMonoBehaviour
{
	// Token: 0x06005286 RID: 21126 RVA: 0x001E0118 File Offset: 0x001DE318
	protected override void OnSpawn()
	{
		ChoreGroup[] array = new ChoreGroup[]
		{
			Db.Get().ChoreGroups.Build,
			Db.Get().ChoreGroups.Basekeeping,
			Db.Get().ChoreGroups.Cook,
			Db.Get().ChoreGroups.Art,
			Db.Get().ChoreGroups.Dig,
			Db.Get().ChoreGroups.Research,
			Db.Get().ChoreGroups.Farming,
			Db.Get().ChoreGroups.Ranching,
			Db.Get().ChoreGroups.MachineOperating,
			Db.Get().ChoreGroups.MedicalAid,
			Db.Get().ChoreGroups.Combat,
			Db.Get().ChoreGroups.LifeSupport,
			Db.Get().ChoreGroups.Recreation,
			Db.Get().ChoreGroups.Toggle,
			Db.Get().ChoreGroups.Rocketry
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.choreConsumer.SetPermittedByUser(array[i], false);
			}
		}
		foreach (Storage storage in base.GetComponents<Storage>())
		{
			if (storage.storageID != GameTags.ChargedPortableBattery)
			{
				this.pickupableStorage = storage;
				break;
			}
		}
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.pickupableStorage.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		base.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x001E02DB File Offset: 0x001DE4DB
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1697596308);
		base.Unsubscribe(-1582839653);
		base.OnCleanUp();
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001E02FC File Offset: 0x001DE4FC
	private void OnTagsChanged(object data)
	{
		TagChangedEventData value = ((Boxed<TagChangedEventData>)data).value;
		if (value.added && value.tag == GameTags.Creatures.Die)
		{
			Brain component = base.GetComponent<Brain>();
			if (component != null && !component.IsRunning())
			{
				component.Resume("death");
			}
		}
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x001E0354 File Offset: 0x001DE554
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		this.ShowPickupSymbol(gameObject);
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001E0378 File Offset: 0x001DE578
	private void ShowPickupSymbol(GameObject pickupable)
	{
		bool flag = this.pickupableStorage.items.Contains(pickupable);
		if (flag)
		{
			this.AddAnimTracker(pickupable);
		}
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM, !flag);
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM_CARRY, flag);
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x001E03D0 File Offset: 0x001DE5D0
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.GetComponent<KBatchedAnimTracker>();
			if (kbatchedAnimTracker != null && kbatchedAnimTracker.controller == this.animController)
			{
				return;
			}
			kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = ((go.GetComponent<Brain>() != null) ? new HashedString("snapTo_pivot") : new HashedString("snapTo_thing"));
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x001E048C File Offset: 0x001DE68C
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker kbatchedAnimTracker = (go != null) ? go.GetComponent<KBatchedAnimTracker>() : null;
		if (kbatchedAnimTracker != null && kbatchedAnimTracker.controller == this.animController)
		{
			UnityEngine.Object.Destroy(kbatchedAnimTracker);
		}
	}

	// Token: 0x040037C0 RID: 14272
	private static string BOTTOM = "bottom";

	// Token: 0x040037C1 RID: 14273
	private static string BOTTOM_CARRY = "bottom_carry";

	// Token: 0x040037C2 RID: 14274
	private KBatchedAnimController animController;

	// Token: 0x040037C3 RID: 14275
	private Storage pickupableStorage;

	// Token: 0x040037C4 RID: 14276
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;
}
