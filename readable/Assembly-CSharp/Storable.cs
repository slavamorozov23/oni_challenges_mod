using System;

// Token: 0x02000642 RID: 1602
public class Storable : KMonoBehaviour
{
	// Token: 0x06002684 RID: 9860 RVA: 0x000DD61A File Offset: 0x000DB81A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Storable>(856640610, Storable.OnStoreDelegate);
		base.Subscribe<Storable>(-778359855, Storable.RefreshStorageTagsDelegate);
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000DD644 File Offset: 0x000DB844
	public void OnStore(object data)
	{
		this.RefreshStorageTags(data);
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000DD650 File Offset: 0x000DB850
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && ((Boxed<bool>)data).value);
		Storage storage = (Storage)data;
		if (storage != null && storage.gameObject == base.gameObject)
		{
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		SaveLoadRoot component2 = base.GetComponent<SaveLoadRoot>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (component3)
		{
			component3.IsSelectable = !flag;
		}
		if (flag)
		{
			component.AddTag(GameTags.Stored, false);
			if (storage == null || !storage.allowItemRemoval)
			{
				component.AddTag(GameTags.StoredPrivate, false);
			}
			else
			{
				component.RemoveTag(GameTags.StoredPrivate);
			}
			if (component2 != null)
			{
				component2.SetRegistered(false);
				return;
			}
		}
		else
		{
			component.RemoveTag(GameTags.Stored);
			component.RemoveTag(GameTags.StoredPrivate);
			if (component2 != null)
			{
				component2.SetRegistered(true);
			}
		}
	}

	// Token: 0x040016BD RID: 5821
	private static readonly EventSystem.IntraObjectHandler<Storable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040016BE RID: 5822
	private static readonly EventSystem.IntraObjectHandler<Storable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.RefreshStorageTags(data);
	});
}
