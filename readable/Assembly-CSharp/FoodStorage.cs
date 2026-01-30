using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class FoodStorage : KMonoBehaviour
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x060022E4 RID: 8932 RVA: 0x000CB44B File Offset: 0x000C964B
	// (set) Token: 0x060022E5 RID: 8933 RVA: 0x000CB453 File Offset: 0x000C9653
	public FilteredStorage FilteredStorage { get; set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000CB45C File Offset: 0x000C965C
	// (set) Token: 0x060022E7 RID: 8935 RVA: 0x000CB464 File Offset: 0x000C9664
	public bool SpicedFoodOnly
	{
		get
		{
			return this.onlyStoreSpicedFood;
		}
		set
		{
			this.onlyStoreSpicedFood = value;
			base.Trigger(1163645216, BoxedBools.Box(this.onlyStoreSpicedFood));
			if (this.onlyStoreSpicedFood)
			{
				this.FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				this.storage.DropHasTags(new Tag[]
				{
					GameTags.Edible,
					GameTags.UnspicedFood
				});
				return;
			}
			this.FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
		}
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000CB4E1 File Offset: 0x000C96E1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FoodStorage>(-905833192, FoodStorage.OnCopySettingsDelegate);
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000CB4FA File Offset: 0x000C96FA
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000CB504 File Offset: 0x000C9704
	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			this.SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}

	// Token: 0x04001475 RID: 5237
	[Serialize]
	private bool onlyStoreSpicedFood;

	// Token: 0x04001476 RID: 5238
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001478 RID: 5240
	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});
}
