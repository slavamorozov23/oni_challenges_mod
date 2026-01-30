using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000782 RID: 1922
[AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
public class ItemPedestal : KMonoBehaviour
{
	// Token: 0x06003108 RID: 12552 RVA: 0x0011AF9C File Offset: 0x0011919C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
		if (this.receptacle.Occupant)
		{
			KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
			if (component)
			{
				component.enabled = true;
				component.sceneLayer = Grid.SceneLayer.Move;
			}
			this.OnOccupantChanged(this.receptacle.Occupant);
		}
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x0011B00C File Offset: 0x0011920C
	private void OnOccupantChanged(object data)
	{
		Attributes attributes = this.GetAttributes();
		if (this.decorModifier != null)
		{
			attributes.Remove(this.decorModifier);
			attributes.Remove(this.decorRadiusModifier);
			this.decorModifier = null;
			this.decorRadiusModifier = null;
		}
		if (data != null)
		{
			GameObject gameObject = (GameObject)data;
			UnityEngine.Object component = gameObject.GetComponent<DecorProvider>();
			float value = 5f;
			float value2 = 3f;
			if (component != null)
			{
				value = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(gameObject).GetTotalValue() * 2f, 5f);
				value2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(gameObject).GetTotalValue() + 2f;
			}
			string description = string.Format(BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, gameObject.GetComponent<KPrefabID>().PrefabTag.ProperName());
			this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, value, description, false, false, true);
			this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, value2, description, false, false, true);
			attributes.Add(this.decorModifier);
			attributes.Add(this.decorRadiusModifier);
		}
	}

	// Token: 0x04001D58 RID: 7512
	[MyCmpReq]
	protected SingleEntityReceptacle receptacle;

	// Token: 0x04001D59 RID: 7513
	[MyCmpReq]
	private DecorProvider decorProvider;

	// Token: 0x04001D5A RID: 7514
	private const float MINIMUM_DECOR = 5f;

	// Token: 0x04001D5B RID: 7515
	private const float STORED_DECOR_MODIFIER = 2f;

	// Token: 0x04001D5C RID: 7516
	private const int RADIUS_BONUS = 2;

	// Token: 0x04001D5D RID: 7517
	private AttributeModifier decorModifier;

	// Token: 0x04001D5E RID: 7518
	private AttributeModifier decorRadiusModifier;

	// Token: 0x04001D5F RID: 7519
	private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>(delegate(ItemPedestal component, object data)
	{
		component.OnOccupantChanged(data);
	});
}
