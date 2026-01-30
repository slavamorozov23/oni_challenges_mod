using System;
using UnityEngine;

// Token: 0x02000B17 RID: 2839
public class RocketConduitStorageAccess : KMonoBehaviour, ISim200ms
{
	// Token: 0x060052B2 RID: 21170 RVA: 0x001E1740 File Offset: 0x001DF940
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001E1760 File Offset: 0x001DF960
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			return;
		}
		float num = this.storage.MassStored();
		if (num < this.targetLevel - 0.01f || num > this.targetLevel + 0.01f)
		{
			if (this.operational != null)
			{
				this.operational.SetActive(true, false);
			}
			float num2 = this.targetLevel - num;
			foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
			{
				CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
				if (component != null && component.storageType == this.cargoType)
				{
					if (num2 > 0f && component.storage.MassStored() > 0f)
					{
						for (int i = component.storage.items.Count - 1; i >= 0; i--)
						{
							GameObject gameObject = component.storage.items[i];
							if (!(this.filterable != null) || !(this.filterable.SelectedTag != GameTags.Void) || !(gameObject.PrefabID() != this.filterable.SelectedTag))
							{
								Pickupable pickupable = gameObject.GetComponent<Pickupable>().Take(num2);
								if (pickupable != null)
								{
									num2 -= pickupable.PrimaryElement.Mass;
									this.storage.Store(pickupable.gameObject, true, false, true, false);
								}
								if (num2 <= 0f)
								{
									break;
								}
							}
						}
						if (num2 <= 0f)
						{
							break;
						}
					}
					if (num2 < 0f && component.storage.RemainingCapacity() > 0f)
					{
						Mathf.Min(-num2, component.storage.RemainingCapacity());
						for (int j = this.storage.items.Count - 1; j >= 0; j--)
						{
							Pickupable pickupable2 = this.storage.items[j].GetComponent<Pickupable>().Take(-num2);
							if (pickupable2 != null)
							{
								num2 += pickupable2.PrimaryElement.Mass;
								component.storage.Store(pickupable2.gameObject, true, false, true, false);
							}
							if (num2 >= 0f)
							{
								break;
							}
						}
						if (num2 >= 0f)
						{
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x040037E2 RID: 14306
	[SerializeField]
	public Storage storage;

	// Token: 0x040037E3 RID: 14307
	[SerializeField]
	public float targetLevel;

	// Token: 0x040037E4 RID: 14308
	[SerializeField]
	public CargoBay.CargoType cargoType;

	// Token: 0x040037E5 RID: 14309
	[MyCmpGet]
	private Filterable filterable;

	// Token: 0x040037E6 RID: 14310
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040037E7 RID: 14311
	private const float TOLERANCE = 0.01f;

	// Token: 0x040037E8 RID: 14312
	private CraftModuleInterface craftModuleInterface;
}
