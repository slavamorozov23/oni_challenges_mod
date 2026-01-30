using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020006F8 RID: 1784
public class BuildingComplete : Building
{
	// Token: 0x06002C21 RID: 11297 RVA: 0x00100F95 File Offset: 0x000FF195
	private bool WasReplaced()
	{
		return this.replacingTileLayer != ObjectLayer.NumLayers;
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x00100FA4 File Offset: 0x000FF1A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Vector3 position = base.transform.GetPosition();
		Grid.PosToXY(position);
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		Attributes attributes = this.GetAttributes();
		foreach (Klei.AI.Attribute attribute in this.Def.attributes)
		{
			attributes.Add(attribute);
		}
		foreach (AttributeModifier attributeModifier in this.Def.attributeModifiers)
		{
			Klei.AI.Attribute attribute2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			if (attributes.Get(attribute2) == null)
			{
				attributes.Add(attribute2);
			}
			attributes.Add(attributeModifier);
		}
		foreach (AttributeInstance attributeInstance in attributes)
		{
			AttributeModifier item = new AttributeModifier(attributeInstance.Id, attributeInstance.GetTotalValue(), null, false, false, true);
			this.regionModifiers.Add(item);
		}
		if (this.Def.SelfHeatKilowattsWhenActive != 0f || this.Def.ExhaustKilowattsWhenActive != 0f)
		{
			base.gameObject.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		}
		if (this.Def.UseStructureTemperature)
		{
			GameComps.StructureTemperatures.Add(base.gameObject);
		}
		base.Subscribe<BuildingComplete>(1606648047, BuildingComplete.OnObjectReplacedDelegate);
		if (this.Def.Entombable)
		{
			base.Subscribe<BuildingComplete>(-1089732772, BuildingComplete.OnEntombedChange);
		}
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x00101220 File Offset: 0x000FF420
	private void OnEntombedChanged()
	{
		if (base.gameObject.HasTag(GameTags.Entombed))
		{
			Components.EntombedBuildings.Add(this);
			return;
		}
		Components.EntombedBuildings.Remove(this);
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x0010124B File Offset: 0x000FF44B
	public override void UpdatePosition()
	{
		base.UpdatePosition();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, base.GetExtents());
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x0010126C File Offset: 0x000FF46C
	private void OnObjectReplaced(object data)
	{
		Constructable.ReplaceCallbackParameters value = ((Boxed<Constructable.ReplaceCallbackParameters>)data).value;
		this.replacingTileLayer = value.TileLayer;
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x00101294 File Offset: 0x000FF494
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		if (this.Def.IsFoundation)
		{
			foreach (int num in base.PlacementCells)
			{
				Grid.Foundation[num] = true;
				Game.Instance.roomProber.SolidChangedEvent(num, false);
			}
		}
		if (Grid.IsValidCell(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, this.Def.SceneLayer);
			base.transform.SetPosition(position);
		}
		if (this.primaryElement != null)
		{
			if (this.primaryElement.Mass == 0f)
			{
				this.primaryElement.Mass = this.Def.Mass[0];
			}
			float temperature = this.primaryElement.Temperature;
			if (temperature > 0f && !float.IsNaN(temperature) && !float.IsInfinity(temperature))
			{
				BuildingComplete.MinKelvinSeen = Mathf.Min(BuildingComplete.MinKelvinSeen, temperature);
			}
			PrimaryElement primaryElement = this.primaryElement;
			primaryElement.setTemperatureCallback = (PrimaryElement.SetTemperatureCallback)Delegate.Combine(primaryElement.setTemperatureCallback, new PrimaryElement.SetTemperatureCallback(this.OnSetTemperature));
		}
		if (!base.gameObject.HasTag(GameTags.RocketInSpace))
		{
			this.Def.MarkArea(cell, base.Orientation, this.Def.ObjectLayer, base.gameObject);
			if (this.Def.IsTilePiece)
			{
				this.Def.MarkArea(cell, base.Orientation, this.Def.TileLayer, base.gameObject);
				this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
				});
			}
		}
		base.RegisterBlockTileRenderer();
		if (this.Def.PreventIdleTraversalPastBuilding)
		{
			for (int j = 0; j < base.PlacementCells.Length; j++)
			{
				Grid.PreventIdleTraversal[base.PlacementCells[j]] = true;
			}
		}
		Components.BuildingCompletes.Add(this);
		BuildingConfigManager.Instance.AddBuildingCompleteKComponents(base.gameObject, this.Def.Tag);
		this.hasSpawnedKComponents = true;
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, base.GetExtents(), GameScenePartitioner.Instance.completeBuildings, null);
		if (this.prefabid.HasTag(GameTags.TemplateBuilding))
		{
			Components.TemplateBuildings.Add(this);
		}
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				int k = 1;
				while (k < component.constructionElements.Length)
				{
					Tag tag = component.constructionElements[k];
					Element element = ElementLoader.GetElement(tag);
					if (element != null)
					{
						using (List<AttributeModifier>.Enumerator enumerator = element.attributeModifiers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								AttributeModifier modifier = enumerator.Current;
								attributes.Add(modifier);
							}
							goto IL_341;
						}
						goto IL_2E1;
					}
					goto IL_2E1;
					IL_341:
					k++;
					continue;
					IL_2E1:
					GameObject gameObject = Assets.TryGetPrefab(tag);
					if (!(gameObject != null))
					{
						goto IL_341;
					}
					PrefabAttributeModifiers component2 = gameObject.GetComponent<PrefabAttributeModifiers>();
					if (component2 != null)
					{
						foreach (AttributeModifier modifier2 in component2.descriptors)
						{
							attributes.Add(modifier2);
						}
						goto IL_341;
					}
					goto IL_341;
				}
			}
		}
		BuildingInventory.Instance.RegisterBuilding(this);
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00101620 File Offset: 0x000FF820
	private void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		BuildingComplete.MinKelvinSeen = Mathf.Min(BuildingComplete.MinKelvinSeen, temperature);
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x00101632 File Offset: 0x000FF832
	public void SetCreationTime(float time)
	{
		this.creationTime = time;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x0010163B File Offset: 0x000FF83B
	private string GetInspectSound()
	{
		return GlobalAssets.GetSound("AI_Inspect_" + base.GetComponent<KPrefabID>().PrefabTag.Name, false);
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x00101660 File Offset: 0x000FF860
	protected override void OnCleanUp()
	{
		if (Game.quitting)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		if (this.hasSpawnedKComponents)
		{
			BuildingConfigManager.Instance.DestroyBuildingCompleteKComponents(base.gameObject, this.Def.Tag);
		}
		if (this.Def.UseStructureTemperature)
		{
			GameComps.StructureTemperatures.Remove(base.gameObject);
		}
		base.OnCleanUp();
		if (!this.WasReplaced() && base.gameObject.GetMyWorldId() != 255)
		{
			int cell = Grid.PosToCell(this);
			this.Def.UnmarkArea(cell, base.Orientation, this.Def.ObjectLayer, base.gameObject);
			if (this.Def.IsTilePiece)
			{
				this.Def.UnmarkArea(cell, base.Orientation, this.Def.TileLayer, base.gameObject);
				this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
				});
			}
			if (this.Def.IsFoundation)
			{
				foreach (int num in base.PlacementCells)
				{
					Grid.Foundation[num] = false;
					Game.Instance.roomProber.SolidChangedEvent(num, false);
				}
			}
			if (this.Def.PreventIdleTraversalPastBuilding)
			{
				for (int j = 0; j < base.PlacementCells.Length; j++)
				{
					Grid.PreventIdleTraversal[base.PlacementCells[j]] = false;
				}
			}
		}
		if (this.WasReplaced() && this.Def.IsTilePiece && this.replacingTileLayer != this.Def.TileLayer)
		{
			int cell2 = Grid.PosToCell(this);
			this.Def.UnmarkArea(cell2, base.Orientation, this.Def.TileLayer, base.gameObject);
			this.Def.RunOnArea(cell2, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		Components.BuildingCompletes.Remove(this);
		Components.EntombedBuildings.Remove(this);
		Components.TemplateBuildings.Remove(this);
		base.UnregisterBlockTileRenderer();
		BuildingInventory.Instance.UnregisterBuilding(this);
		base.Trigger(-21016276, this);
	}

	// Token: 0x04001A27 RID: 6695
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x04001A28 RID: 6696
	[MyCmpGet]
	public KPrefabID prefabid;

	// Token: 0x04001A29 RID: 6697
	public bool isManuallyOperated;

	// Token: 0x04001A2A RID: 6698
	public bool isArtable;

	// Token: 0x04001A2B RID: 6699
	public PrimaryElement primaryElement;

	// Token: 0x04001A2C RID: 6700
	[Serialize]
	public float creationTime = -1f;

	// Token: 0x04001A2D RID: 6701
	private bool hasSpawnedKComponents;

	// Token: 0x04001A2E RID: 6702
	private ObjectLayer replacingTileLayer = ObjectLayer.NumLayers;

	// Token: 0x04001A2F RID: 6703
	public List<AttributeModifier> regionModifiers = new List<AttributeModifier>();

	// Token: 0x04001A30 RID: 6704
	private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnEntombedChange = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data)
	{
		component.OnEntombedChanged();
	});

	// Token: 0x04001A31 RID: 6705
	private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data)
	{
		component.OnObjectReplaced(data);
	});

	// Token: 0x04001A32 RID: 6706
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04001A33 RID: 6707
	public static float MinKelvinSeen = float.MaxValue;
}
