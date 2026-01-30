using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000879 RID: 2169
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06003BB1 RID: 15281 RVA: 0x0014DD9E File Offset: 0x0014BF9E
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06003BB2 RID: 15282 RVA: 0x0014DDA8 File Offset: 0x0014BFA8
	// (remove) Token: 0x06003BB3 RID: 15283 RVA: 0x0014DDE0 File Offset: 0x0014BFE0
	public event Action<Tag> OnDiscover;

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x0014DE15 File Offset: 0x0014C015
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x0014DE20 File Offset: 0x0014C020
	public List<Tag> StandardDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				list.Add(go.PrefabID());
			}
			list.Add(ConsumerManager.OXYGEN_TANK_ID);
			return list;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06003BB6 RID: 15286 RVA: 0x0014DE94 File Offset: 0x0014C094
	public List<Tag> BionicDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Edible))
			{
				list.Add(go.PrefabID());
			}
			Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
			GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
			foreach (Tag item in array)
			{
				list.Add(item);
			}
			return list;
		}
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0014DF3C File Offset: 0x0014C13C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0014DF8E File Offset: 0x0014C18E
	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0014DF9F File Offset: 0x0014C19F
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0014DFB8 File Offset: 0x0014C1B8
	public void RefreshDiscovered(object data = null)
	{
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (!this.ShouldBeDiscovered(foodInfo.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Add(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover("UndiscoveredSomething".ToTag());
				}
			}
			else if (this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()) && this.ShouldBeDiscovered(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Remove(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover(foodInfo.Id.ToTag());
				}
				if (!DiscoveredResources.Instance.IsDiscovered(foodInfo.Id.ToTag()))
				{
					if (foodInfo.CaloriesPerUnit == 0f)
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.CookingIngredient);
					}
					else
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.Edible);
					}
				}
			}
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0014E13C File Offset: 0x0014C33C
	private bool ShouldBeDiscovered(Tag food_id)
	{
		if (DiscoveredResources.Instance.IsDiscovered(food_id))
		{
			return true;
		}
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			if (recipe.Result == food_id)
			{
				foreach (string id in recipe.fabricators)
				{
					if (Db.Get().TechItems.IsTechItemComplete(id))
					{
						return true;
					}
				}
			}
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040024E9 RID: 9449
	public static ConsumerManager instance;

	// Token: 0x040024EB RID: 9451
	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	// Token: 0x040024EC RID: 9452
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x040024ED RID: 9453
	public static string OXYGEN_TANK_ID = ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag.ToString();
}
