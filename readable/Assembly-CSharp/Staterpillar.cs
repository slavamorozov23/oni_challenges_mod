using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class Staterpillar : KMonoBehaviour
{
	// Token: 0x06003BFE RID: 15358 RVA: 0x001501F0 File Offset: 0x0014E3F0
	protected override void OnPrefabInit()
	{
		this.dummyElement = new List<Tag>
		{
			SimHashes.Unobtanium.CreateTag()
		};
		this.connectorDef = Assets.GetBuildingDef(this.connectorDefId);
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x0015021E File Offset: 0x0014E41E
	public void SpawnConnectorBuilding(int targetCell)
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.SpawnGenerator(targetCell);
			return;
		}
		this.SpawnConduitConnector(targetCell);
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x0015023C File Offset: 0x0014E43C
	public void DestroyOrphanedConnectorBuilding()
	{
		KPrefabID building = this.GetConnectorBuilding();
		if (building != null)
		{
			this.connectorRef.Set(null);
			this.cachedGenerator = null;
			this.cachedConduitDispenser = null;
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", delegate(object o)
			{
				if (building != null)
				{
					Util.KDestroyGameObject(building.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x001502A1 File Offset: 0x0014E4A1
	public void EnableConnector()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.EnableGenerator();
			return;
		}
		this.EnableConduitConnector();
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x001502BA File Offset: 0x0014E4BA
	public bool IsConnectorBuildingSpawned()
	{
		return this.GetConnectorBuilding() != null;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x001502C8 File Offset: 0x0014E4C8
	public bool IsConnected()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			return this.GetGenerator().CircuitID != ushort.MaxValue;
		}
		return this.GetConduitDispenser().IsConnected;
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x001502F5 File Offset: 0x0014E4F5
	public KPrefabID GetConnectorBuilding()
	{
		return this.connectorRef.Get();
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x00150304 File Offset: 0x0014E504
	private void SpawnConduitConnector(int targetCell)
	{
		if (this.GetConduitDispenser() == null)
		{
			GameObject gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			this.connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
		}
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0015036E File Offset: 0x0014E56E
	private void EnableConduitConnector()
	{
		ConduitDispenser conduitDispenser = this.GetConduitDispenser();
		conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
		conduitDispenser.storage = base.GetComponent<Storage>();
		conduitDispenser.SetOnState(true);
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x00150394 File Offset: 0x0014E594
	public ConduitDispenser GetConduitDispenser()
	{
		if (this.cachedConduitDispenser == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedConduitDispenser = kprefabID.GetComponent<ConduitDispenser>();
			}
		}
		return this.cachedConduitDispenser;
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x001503D8 File Offset: 0x0014E5D8
	private void DestroyOrphanedConduitDispenserBuilding()
	{
		ConduitDispenser dispenser = this.GetConduitDispenser();
		if (dispenser != null)
		{
			this.connectorRef.Set(null);
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", delegate(object o)
			{
				if (dispenser != null)
				{
					Util.KDestroyGameObject(dispenser.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x00150430 File Offset: 0x0014E630
	private void SpawnGenerator(int targetCell)
	{
		StaterpillarGenerator generator = this.GetGenerator();
		GameObject gameObject = null;
		if (generator != null)
		{
			gameObject = generator.gameObject;
		}
		if (!gameObject)
		{
			gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
			component.parent = new Ref<Staterpillar>(this);
			this.connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
			component.enabled = false;
		}
		Attributes attributes = gameObject.gameObject.GetAttributes();
		bool flag = base.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0f;
		if (flag)
		{
			attributes.Add(this.wildMod);
		}
		bool flag2 = base.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
		CreatureCalorieMonitor.Instance smi = base.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi.IsHungry() || flag2)
		{
			float calories0to = smi.GetCalories0to1();
			float num = 1f;
			if (calories0to <= 0f)
			{
				num = (flag ? 0.1f : 0.025f);
			}
			else if (calories0to <= 0.3f)
			{
				num = 0.5f;
			}
			else if (calories0to <= 0.5f)
			{
				num = 0.75f;
			}
			if (num < 1f)
			{
				float num2;
				if (flag)
				{
					num2 = Mathf.Lerp(0f, 25f, 1f - num);
				}
				else
				{
					num2 = (1f - num) * 100f;
				}
				AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -num2, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, false, false, true);
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x001505EE File Offset: 0x0014E7EE
	private void EnableGenerator()
	{
		StaterpillarGenerator generator = this.GetGenerator();
		generator.enabled = true;
		generator.GetComponent<BuildingCellVisualizer>().enabled = true;
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00150608 File Offset: 0x0014E808
	public StaterpillarGenerator GetGenerator()
	{
		if (this.cachedGenerator == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedGenerator = kprefabID.GetComponent<StaterpillarGenerator>();
			}
		}
		return this.cachedGenerator;
	}

	// Token: 0x04002506 RID: 9478
	public ObjectLayer conduitLayer;

	// Token: 0x04002507 RID: 9479
	public string connectorDefId;

	// Token: 0x04002508 RID: 9480
	private IList<Tag> dummyElement;

	// Token: 0x04002509 RID: 9481
	private BuildingDef connectorDef;

	// Token: 0x0400250A RID: 9482
	[Serialize]
	private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

	// Token: 0x0400250B RID: 9483
	private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD, false, false, true);

	// Token: 0x0400250C RID: 9484
	private ConduitDispenser cachedConduitDispenser;

	// Token: 0x0400250D RID: 9485
	private StaterpillarGenerator cachedGenerator;
}
