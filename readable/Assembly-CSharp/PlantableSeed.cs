using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008B2 RID: 2226
[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06003D59 RID: 15705 RVA: 0x00156883 File Offset: 0x00154A83
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x0015688B File Offset: 0x00154A8B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x001568A8 File Offset: 0x00154AA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.PlantableSeeds.Add(this);
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x001568BB File Offset: 0x00154ABB
	protected override void OnCleanUp()
	{
		Components.PlantableSeeds.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x001568D0 File Offset: 0x00154AD0
	public void TryPlant(bool allow_plant_from_storage = false)
	{
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
		if (!allow_plant_from_storage && base.gameObject.HasTag(GameTags.Stored))
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject);
		if (this.TestSuitableGround(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			if (component != null)
			{
				base.GetComponent<MutantPlant>().CopyMutationsTo(component);
			}
			gameObject.SetActive(true);
			Pickupable pickupable = this.pickupable.TakeUnit(1f);
			if (pickupable != null)
			{
				gameObject.GetComponent<Crop>() != null;
				Util.KDestroyGameObject(pickupable.gameObject);
				return;
			}
			KCrashReporter.Assert(false, "Seed has fractional total amount < 1f", null);
		}
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x001569A4 File Offset: 0x00154BA4
	public bool TestSuitableGround(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (this.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.Foundation[num])
		{
			return false;
		}
		if (Grid.Element[num].hardness >= 150)
		{
			return false;
		}
		if (this.replantGroundTag.IsValid && !Grid.Element[num].HasTag(this.replantGroundTag))
		{
			return false;
		}
		GameObject prefab = Assets.GetPrefab(this.PlantID);
		EntombVulnerable component = prefab.GetComponent<EntombVulnerable>();
		if (component != null && !component.IsCellSafe(cell))
		{
			return false;
		}
		DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
		if (component2 != null && !component2.IsCellSafe(cell))
		{
			return false;
		}
		TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
		if (component3 != null && !component3.IsCellSafe(cell) && Grid.Element[cell].id != SimHashes.Vacuum)
		{
			return false;
		}
		UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
		if (component4 != null && !component4.IsSuitableFoundation(cell))
		{
			return false;
		}
		OccupyArea component5 = prefab.GetComponent<OccupyArea>();
		return !(component5 != null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00156AD8 File Offset: 0x00154CD8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			Descriptor item = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
		}
		else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
		{
			Descriptor item2 = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x040025DA RID: 9690
	public Tag PlantID;

	// Token: 0x040025DB RID: 9691
	public Tag PreviewID;

	// Token: 0x040025DC RID: 9692
	[Serialize]
	public float timeUntilSelfPlant;

	// Token: 0x040025DD RID: 9693
	public Tag replantGroundTag;

	// Token: 0x040025DE RID: 9694
	public string domesticatedDescription;

	// Token: 0x040025DF RID: 9695
	public SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x040025E0 RID: 9696
	[MyCmpGet]
	private Pickupable pickupable;
}
