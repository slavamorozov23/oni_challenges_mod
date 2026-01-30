using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
[Serializable]
public class BuildingDef : Def, IHasDlcRestrictions
{
	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0015E8AB File Offset: 0x0015CAAB
	public IReadOnlyList<string> SearchTerms
	{
		get
		{
			return this.searchTerms;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x0015E8B3 File Offset: 0x0015CAB3
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".NAME");
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x0015E8D9 File Offset: 0x0015CAD9
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".DESC");
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x0015E8FF File Offset: 0x0015CAFF
	public string Flavor
	{
		get
		{
			return "\"" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".FLAVOR") + "\"";
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x0015E934 File Offset: 0x0015CB34
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x0015E95A File Offset: 0x0015CB5A
	public bool IsTilePiece
	{
		get
		{
			return this.TileLayer != ObjectLayer.NumLayers;
		}
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x0015E969 File Offset: 0x0015CB69
	public bool CanReplace(GameObject go)
	{
		return this.ReplacementTags != null && go.GetComponent<KPrefabID>().HasAnyTags(this.ReplacementTags);
	}

	// Token: 0x06003EB6 RID: 16054 RVA: 0x0015E986 File Offset: 0x0015CB86
	public bool IsAvailable()
	{
		return !this.Deprecated && (!this.DebugOnly || Game.Instance.DebugOnlyBuildingsAllowed);
	}

	// Token: 0x06003EB7 RID: 16055 RVA: 0x0015E9A6 File Offset: 0x0015CBA6
	public bool ShouldShowInBuildMenu()
	{
		return this.ShowInBuildMenu;
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x0015E9B0 File Offset: 0x0015CBB0
	public bool IsReplacementLayerOccupied(int cell)
	{
		if (Grid.Objects[cell, (int)this.ReplacementLayer] != null)
		{
			return true;
		}
		if (this.EquivalentReplacementLayers != null)
		{
			foreach (ObjectLayer layer in this.EquivalentReplacementLayers)
			{
				if (Grid.Objects[cell, (int)layer] != null)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0015EA3C File Offset: 0x0015CC3C
	public GameObject GetReplacementCandidate(int cell)
	{
		if (this.ReplacementCandidateLayers != null)
		{
			using (List<ObjectLayer>.Enumerator enumerator = this.ReplacementCandidateLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ObjectLayer objectLayer = enumerator.Current;
					if (Grid.ObjectLayers[(int)objectLayer].ContainsKey(cell))
					{
						GameObject gameObject = Grid.ObjectLayers[(int)objectLayer][cell];
						if (gameObject != null && gameObject.GetComponent<BuildingComplete>() != null)
						{
							return gameObject;
						}
					}
				}
				goto IL_96;
			}
		}
		if (Grid.ObjectLayers[(int)this.TileLayer].ContainsKey(cell))
		{
			return Grid.ObjectLayers[(int)this.TileLayer][cell];
		}
		IL_96:
		return null;
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x0015EAF4 File Offset: 0x0015CCF4
	public GameObject Create(Vector3 pos, Storage resource_storage, IList<Tag> selected_elements, Recipe recipe, float temperature, GameObject obj)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		if (resource_storage != null)
		{
			Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
			if (allIngredients != null)
			{
				foreach (Recipe.Ingredient ingredient in allIngredients)
				{
					float num;
					SimUtil.DiseaseInfo b;
					float num2;
					resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out b, out num2);
					diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
				}
			}
		}
		GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer, null, 0);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.ElementID = element.id;
		component.Temperature = temperature;
		component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "BuildingDef.Create");
		gameObject.name = obj.name;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x0015EBC4 File Offset: 0x0015CDC4
	public List<Tag> DefaultElements()
	{
		List<Tag> list = new List<Tag>();
		string[] materialCategory = this.MaterialCategory;
		for (int i = 0; i < materialCategory.Length; i++)
		{
			List<Tag> validMaterials = MaterialSelector.GetValidMaterials(materialCategory[i], false);
			if (validMaterials.Count != 0)
			{
				list.Add(validMaterials[0]);
			}
		}
		return list;
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x0015EC14 File Offset: 0x0015CE14
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, string facadeID, bool playsound = true, float timeBuilt = -1f)
	{
		GameObject gameObject = this.Build(cell, orientation, resource_storage, selected_elements, temperature, playsound, timeBuilt);
		if (facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
		}
		return gameObject;
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x0015EC60 File Offset: 0x0015CE60
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, bool playsound = true, float timeBuilt = -1f)
	{
		Vector3 pos = Grid.CellToPosCBC(cell, this.SceneLayer);
		GameObject gameObject = this.Create(pos, resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
		Rotatable component = gameObject.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(orientation);
		}
		this.MarkArea(cell, orientation, this.ObjectLayer, gameObject);
		if (this.IsTilePiece)
		{
			this.MarkArea(cell, orientation, this.TileLayer, gameObject);
			this.RunOnArea(cell, orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer);
			});
		}
		if (this.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize, false);
			if (playsound && sound != null)
			{
				Vector3 position = gameObject.transform.GetPosition();
				position.z = 0f;
				KFMOD.PlayOneShot(sound, position, 1f);
			}
		}
		Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[selected_elements.Count];
			for (int i = 0; i < selected_elements.Count; i++)
			{
				component2.constructionElements[i] = selected_elements[i];
			}
		}
		BuildingComplete component3 = gameObject.GetComponent<BuildingComplete>();
		if (component3)
		{
			component3.SetCreationTime(timeBuilt);
		}
		Game.Instance.Trigger(-1661515756, gameObject);
		gameObject.Trigger(-1661515756, gameObject);
		return gameObject;
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x0015EDB8 File Offset: 0x0015CFB8
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, null, 0);
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x0015EDC7 File Offset: 0x0015CFC7
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, facadeID, true, layer);
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0015EDDC File Offset: 0x0015CFDC
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, bool restrictToActiveWorld, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, Grid.PosToCell(pos), orientation, false, out text, restrictToActiveWorld))
		{
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		if (gameObject != null && facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			gameObject.GetComponent<KBatchedAnimController>().Play("place", KAnim.PlayMode.Once, 1f, 0f);
		}
		return gameObject;
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0015EE7C File Offset: 0x0015D07C
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out text))
		{
			Constructable component = this.BuildingUnderConstruction.GetComponent<Constructable>();
			component.IsReplacementTile = true;
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			component.IsReplacementTile = false;
			if (orientation != Orientation.Neutral)
			{
				Rotatable component2 = gameObject.GetComponent<Rotatable>();
				if (component2 != null)
				{
					component2.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x0015EEDC File Offset: 0x0015D0DC
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		GameObject gameObject = this.TryReplaceTile(src_go, pos, orientation, selected_elements, layer);
		if (gameObject != null)
		{
			if (facadeID != null && facadeID != "DEFAULT_FACADE")
			{
				gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			}
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x0015EF48 File Offset: 0x0015D148
	public GameObject Instantiate(Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		float num = -0.15f;
		pos.z += num;
		GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, null, layer);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null, "Missing primary element for BuildingDef");
		gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
		gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0015EFBC File Offset: 0x0015D1BC
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, out string fail_reason)
	{
		return this.IsAreaClear(source_go, cell, orientation, layer, tile_layer, replace_tile, true, out fail_reason, true);
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x0015EFDC File Offset: 0x0015D1DC
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, bool restrictToActiveWorld, out string fail_reason, bool permitUproots = true)
	{
		bool flag = true;
		fail_reason = null;
		int i = 0;
		BuildLocationRule buildLocationRule;
		while (i < this.PlacementOffsets.Length)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (restrictToActiveWorld && (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				flag = false;
				break;
			}
			bool flag2 = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit || this.BuildLocationRule == BuildLocationRule.WireBridge;
			GameObject x = null;
			if (replace_tile)
			{
				x = this.GetReplacementCandidate(num);
			}
			if (!flag2)
			{
				GameObject gameObject = Grid.Objects[num, (int)layer];
				bool flag3 = false;
				if (gameObject != null)
				{
					Building component = gameObject.GetComponent<Building>();
					if (component != null)
					{
						buildLocationRule = component.Def.BuildLocationRule;
						if (buildLocationRule - BuildLocationRule.Conduit <= 2)
						{
							flag3 = true;
						}
					}
				}
				if (!flag3 && (!permitUproots || !Uprootable.CanUproot(gameObject)))
				{
					if (gameObject != null && gameObject != source_go && (x == null || x != gameObject) && (gameObject.GetComponent<Wire>() == null || this.BuildingComplete.GetComponent<Wire>() == null))
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
					if (tile_layer != ObjectLayer.NumLayers && (x == null || x == source_go) && Grid.Objects[num, (int)tile_layer] != null && Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == null)
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
				}
			}
			if (layer == ObjectLayer.Building && this.AttachmentSlotTag != GameTags.Rocket && Grid.Objects[num, 39] != null)
			{
				if (this.BuildingComplete.GetComponent<Wire>() == null)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
					flag = false;
					break;
				}
				break;
			}
			else
			{
				if (layer == ObjectLayer.Gantry)
				{
					bool flag4 = false;
					MakeBaseSolid.Def def = source_go.GetDef<MakeBaseSolid.Def>();
					for (int j = 0; j < def.solidOffsets.Length; j++)
					{
						CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(def.solidOffsets[j], orientation);
						flag4 |= (rotatedCellOffset2 == rotatedCellOffset);
					}
					if (flag4 && !this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
					GameObject gameObject2 = Grid.Objects[num, 1];
					if (gameObject2 != null && gameObject2.GetComponent<BuildingPreview>() == null)
					{
						Building component2 = gameObject2.GetComponent<Building>();
						if (flag4 || component2 == null || component2.Def.AttachmentSlotTag != GameTags.Rocket)
						{
							fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
							flag = false;
							break;
						}
					}
				}
				if (this.BuildLocationRule == BuildLocationRule.Tile)
				{
					if (!this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
				}
				else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) != SubWorld.ZoneType.Space)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
					flag = false;
					break;
				}
				i++;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (layer == ObjectLayer.LiquidConduit)
		{
			GameObject gameObject3 = Grid.Objects[cell, 19];
			if (gameObject3 != null)
			{
				Building component3 = gameObject3.GetComponent<Building>();
				if (component3 != null && component3.Def.BuildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin && component3.GetCell() == cell)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN;
					return false;
				}
			}
		}
		buildLocationRule = this.BuildLocationRule;
		switch (buildLocationRule)
		{
		case BuildLocationRule.NotInTiles:
		{
			GameObject x2 = Grid.Objects[cell, 9];
			if (!replace_tile && x2 != null && x2 != source_go)
			{
				flag = false;
			}
			else if (Grid.HasDoor[cell])
			{
				flag = false;
			}
			else
			{
				GameObject gameObject4 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject4 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						if (gameObject4 != source_go)
						{
							flag = false;
						}
					}
					else
					{
						Building component4 = gameObject4.GetComponent<Building>();
						if (component4 != null && component4.Def.ReplacementLayer != this.ReplacementLayer)
						{
							flag = false;
						}
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			}
			break;
		}
		case BuildLocationRule.Conduit:
		case BuildLocationRule.LogicBridge:
			break;
		case BuildLocationRule.WireBridge:
			return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
		case BuildLocationRule.HighWattBridgeTile:
			flag = (this.IsValidTileLocation(source_go, cell, replace_tile, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason));
			break;
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num2 = 0;
			while (num2 < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int k = 0; k < Components.BuildingAttachPoints[num2].points.Length; k++)
				{
					if (Components.BuildingAttachPoints[num2].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num2++;
			}
			if (!flag)
			{
				fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			}
			break;
		}
		default:
			if (buildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin)
			{
				flag = (Grid.Objects[cell, 16] == null && (Grid.Objects[cell, 19] == null || Grid.Objects[cell, 19] == source_go));
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		flag = (flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0015F5E0 File Offset: 0x0015D7E0
	private bool IsValidTileLocation(GameObject source_go, int cell, bool replacement_tile, ref string fail_reason)
	{
		GameObject gameObject = Grid.Objects[cell, 27];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 29];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 2];
		if (gameObject != null && gameObject != source_go)
		{
			Building component = gameObject.GetComponent<Building>();
			if (!replacement_tile && component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0015F6C4 File Offset: 0x0015D8C4
	public void RunOnArea(int cell, Orientation orientation, Action<int> callback)
	{
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int obj = Grid.OffsetCell(cell, rotatedCellOffset);
			callback(obj);
		}
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0015F708 File Offset: 0x0015D908
	public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
		{
			for (int i = 0; i < this.PlacementOffsets.Length; i++)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
				int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
				GameObject gameObject = Grid.Objects[cell2, (int)layer];
				if (Uprootable.CanUproot(gameObject))
				{
					Grid.Objects[cell2, 5] = gameObject;
				}
				Grid.Objects[cell2, (int)layer] = go;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell3, (int)objectLayerForConduitType], go);
			Grid.Objects[cell3, (int)objectLayerForConduitType] = go;
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell4, (int)objectLayerForConduitType2], go);
			Grid.Objects[cell4, (int)objectLayerForConduitType2] = go;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			this.MarkOverlappingPorts(Grid.Objects[cell5, 29], go);
			Grid.Objects[cell5, 29] = go;
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			this.MarkOverlappingPorts(Grid.Objects[cell6, 29], go);
			Grid.Objects[cell6, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			this.MarkOverlappingPorts(Grid.Objects[cell7, 29], go);
			this.MarkOverlappingPorts(Grid.Objects[cell8, 29], go);
			Grid.Objects[cell7, 29] = go;
			Grid.Objects[cell8, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
		{
			LogicPorts component = go.GetComponent<LogicPorts>();
			if (component != null && component.inputPortInfo != null)
			{
				LogicPorts.Port[] inputPortInfo = component.inputPortInfo;
				for (int j = 0; j < inputPortInfo.Length; j++)
				{
					CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(inputPortInfo[j].cellOffset, orientation);
					int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
					this.MarkOverlappingLogicPorts(Grid.Objects[cell9, (int)layer], go, cell9);
					Grid.Objects[cell9, (int)layer] = go;
				}
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						this.MarkOverlappingPorts(Grid.Objects[cell10, (int)objectLayerForConduitType3], go);
						Grid.Objects[cell10, (int)objectLayerForConduitType3] = go;
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset8 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell11 = Grid.OffsetCell(cell, rotatedCellOffset8);
						this.MarkOverlappingPorts(Grid.Objects[cell11, (int)objectLayerForConduitType4], go);
						Grid.Objects[cell11, (int)objectLayerForConduitType4] = go;
					}
				}
			}
		}
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0015FB01 File Offset: 0x0015DD01
	public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			existing.AddTag(GameTags.HasInvalidPorts);
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0015FB38 File Offset: 0x0015DD38
	public void MarkOverlappingLogicPorts(GameObject existing, GameObject replaced, int cell)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			LogicGate component = existing.GetComponent<LogicGate>();
			LogicPorts component2 = existing.GetComponent<LogicPorts>();
			LogicPorts.Port port;
			bool flag;
			LogicGateBase.PortId portId;
			if ((component2 != null && component2.TryGetPortAtCell(cell, out port, out flag)) || (component != null && component.TryGetPortAtCell(cell, out portId)))
			{
				existing.AddTag(GameTags.HasInvalidPorts);
			}
		}
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0015FBB0 File Offset: 0x0015DDB0
	public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (cell == Grid.InvalidCell)
		{
			return;
		}
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (Grid.Objects[cell2, (int)layer] == go)
			{
				Grid.Objects[cell2, (int)layer] = null;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			if (Grid.Objects[cell3, (int)objectLayerForConduitType] == go)
			{
				Grid.Objects[cell3, (int)objectLayerForConduitType] = null;
			}
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			if (Grid.Objects[cell4, (int)objectLayerForConduitType2] == go)
			{
				Grid.Objects[cell4, (int)objectLayerForConduitType2] = null;
			}
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			if (Grid.Objects[cell5, 29] == go)
			{
				Grid.Objects[cell5, 29] = null;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			if (Grid.Objects[cell6, 29] == go)
			{
				Grid.Objects[cell6, 29] = null;
			}
		}
		if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			if (Grid.Objects[cell7, 29] == go)
			{
				Grid.Objects[cell7, 29] = null;
			}
			if (Grid.Objects[cell8, 29] == go)
			{
				Grid.Objects[cell8, 29] = null;
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
						if (Grid.Objects[cell9, (int)objectLayerForConduitType3] == go)
						{
							Grid.Objects[cell9, (int)objectLayerForConduitType3] = null;
						}
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						if (Grid.Objects[cell10, (int)objectLayerForConduitType4] == go)
						{
							Grid.Objects[cell10, (int)objectLayerForConduitType4] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0015FEF1 File Offset: 0x0015E0F1
	public int GetBuildingCell(int cell)
	{
		return cell + (this.WidthInCells - 1) / 2;
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0015FEFF File Offset: 0x0015E0FF
	public Vector3 GetVisualizerOffset()
	{
		return Vector3.right * (0.5f * (float)((this.WidthInCells + 1) % 2));
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x0015FF1C File Offset: 0x0015E11C
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x0015FF3C File Offset: 0x0015E13C
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason);
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x0015FF5D File Offset: 0x0015E15D
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0015FF6B File Offset: 0x0015E16B
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason, false);
	}

	// Token: 0x06003ED2 RID: 16082 RVA: 0x0015FF7C File Offset: 0x0015E17C
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason, bool restrictToActiveWorld)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (restrictToActiveWorld && (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (this.BuildLocationRule == BuildLocationRule.OnRocketEnvelope)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.OnBackWall)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL_REQUIRED;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.OnWall)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.InCorner)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.WallFloor)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.BelowRocketCeiling)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
			if ((float)(Grid.CellToXY(cell).y + 35 + source_go.GetComponent<Building>().Def.HeightInCells) >= world.maximumBounds.y - (float)Grid.TopBorderHeight)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BELOWROCKETCEILING;
				return false;
			}
		}
		return this.IsAreaClear(source_go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, restrictToActiveWorld, out fail_reason, true);
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x00160178 File Offset: 0x0015E378
	public bool IsValidReplaceLocation(Vector3 pos, Orientation orientation, ObjectLayer replace_layer, ObjectLayer obj_layer)
	{
		if (replace_layer == ObjectLayer.NumLayers)
		{
			return false;
		}
		bool result = true;
		int cell = Grid.PosToCell(pos);
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(cell2))
			{
				return false;
			}
			if (Grid.Objects[cell2, (int)obj_layer] == null || Grid.Objects[cell2, (int)replace_layer] != null)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x00160200 File Offset: 0x0015E400
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile = false)
	{
		string text = "";
		return this.IsValidBuildLocation(source_go, pos, orientation, out text, replace_tile);
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x00160220 File Offset: 0x0015E420
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string reason, bool replace_tile = false)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidBuildLocation(source_go, cell, orientation, replace_tile, out reason);
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x00160244 File Offset: 0x0015E444
	public bool IsValidBuildLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (!this.IsAreaValid(cell, orientation, out fail_reason))
		{
			return false;
		}
		bool flag = true;
		fail_reason = null;
		switch (this.BuildLocationRule)
		{
		case BuildLocationRule.Anywhere:
		case BuildLocationRule.Conduit:
		case BuildLocationRule.OnFloorOrBuildingAttachPoint:
			flag = true;
			break;
		case BuildLocationRule.OnFloor:
		case BuildLocationRule.OnCeiling:
		case BuildLocationRule.OnFoundationRotatable:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			break;
		case BuildLocationRule.OnFloorOverSpace:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			else if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (int check_cell) => global::World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
			}
			break;
		case BuildLocationRule.OnWall:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
			}
			break;
		case BuildLocationRule.InCorner:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
			}
			break;
		case BuildLocationRule.Tile:
		{
			flag = true;
			GameObject gameObject = Grid.Objects[cell, 27];
			if (gameObject != null)
			{
				Building component = gameObject.GetComponent<Building>();
				if (component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = false;
				}
			}
			gameObject = Grid.Objects[cell, 2];
			if (gameObject != null)
			{
				Building component2 = gameObject.GetComponent<Building>();
				if (component2 != null && component2.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = replace_tile;
				}
			}
			break;
		}
		case BuildLocationRule.NotInTiles:
		{
			GameObject x = Grid.Objects[cell, 9];
			flag = (replace_tile || x == null || x == source_go);
			flag = (flag && !Grid.HasDoor[cell]);
			if (flag)
			{
				GameObject gameObject2 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject2 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						flag = (flag && (gameObject2 == null || gameObject2 == source_go));
					}
					else
					{
						Building component3 = gameObject2.GetComponent<Building>();
						flag = (component3 == null || component3.Def.ReplacementLayer == this.ReplacementLayer);
					}
				}
			}
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			break;
		}
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num = 0;
			while (num < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int i = 0; i < Components.BuildingAttachPoints[num].points.Length; i++)
				{
					if (Components.BuildingAttachPoints[num].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num++;
			}
			fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			break;
		}
		case BuildLocationRule.OnRocketEnvelope:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
			}
			break;
		case BuildLocationRule.WallFloor:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x00160678 File Offset: 0x0015E878
	private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
	{
		bool result = true;
		fail_reason = null;
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x00160704 File Offset: 0x0015E904
	private bool ArePowerPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			GameObject x = Grid.Objects[cell2, 29];
			if (x != null && x != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			GameObject x2 = Grid.Objects[cell3, 29];
			if (x2 != null && x2 != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x001607C0 File Offset: 0x0015E9C0
	private bool AreConduitPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		bool flag = true;
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
			flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
		}
		if (flag && this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int utility_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
			flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell2, ref fail_reason);
		}
		Building component = source_go.GetComponent<Building>();
		if (flag && component)
		{
			ISecondaryInput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryInput>();
			if (components != null)
			{
				foreach (ISecondaryInput secondaryInput in components)
				{
					for (int j = 0; j < 4; j++)
					{
						ConduitType conduitType = (ConduitType)j;
						if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
						{
							CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
							int utility_cell3 = Grid.OffsetCell(cell, rotatedCellOffset3);
							flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell3, ref fail_reason);
						}
					}
				}
			}
		}
		if (flag)
		{
			ISecondaryOutput[] components2 = component.Def.BuildingComplete.GetComponents<ISecondaryOutput>();
			if (components2 != null)
			{
				foreach (ISecondaryOutput secondaryOutput in components2)
				{
					for (int k = 0; k < 4; k++)
					{
						ConduitType conduitType2 = (ConduitType)k;
						if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
						{
							CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
							int utility_cell4 = Grid.OffsetCell(cell, rotatedCellOffset4);
							flag = this.IsValidConduitConnection(source_go, conduitType2, utility_cell4, ref fail_reason);
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x00160960 File Offset: 0x0015EB60
	private bool IsValidWireBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			int cell2;
			int cell3;
			component.GetCells(out cell2, out cell3);
			if (Grid.Objects[cell2, 29] != null || Grid.Objects[cell3, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x001609D4 File Offset: 0x0015EBD4
	private bool IsValidHighWattBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			if (!component.AreCellsValid(cell, orientation))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			int num;
			int num2;
			component.GetCells(out num, out num2);
			if (Grid.Objects[num, 29] != null || Grid.Objects[num2, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
			if (Grid.Objects[num, 9] != null || Grid.Objects[num2, 9] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			if (Grid.HasDoor[num] || Grid.HasDoor[num2])
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			GameObject gameObject = Grid.Objects[num, 1];
			GameObject gameObject2 = Grid.Objects[num2, 1];
			if (gameObject != null || gameObject2 != null)
			{
				BuildingUnderConstruction buildingUnderConstruction = gameObject ? gameObject.GetComponent<BuildingUnderConstruction>() : null;
				BuildingUnderConstruction buildingUnderConstruction2 = gameObject2 ? gameObject2.GetComponent<BuildingUnderConstruction>() : null;
				if ((buildingUnderConstruction && buildingUnderConstruction.Def.BuildingComplete.GetComponent<Door>()) || (buildingUnderConstruction2 && buildingUnderConstruction2.Def.BuildingComplete.GetComponent<Door>()))
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
					return false;
				}
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06003EDC RID: 16092 RVA: 0x00160B70 File Offset: 0x0015ED70
	private bool AreLogicPortsInValidPositions(GameObject source_go, int cell, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		List<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
		LogicPorts component = source_go.GetComponent<LogicPorts>();
		if (component != null)
		{
			component.HackRefreshVisualizers();
			if (this.DoLogicPortsConflict(component.inputPorts, visElements) || this.DoLogicPortsConflict(component.outputPorts, visElements))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		else
		{
			LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
			if (component2 != null && (this.IsLogicPortObstructed(component2.InputCellOne, visElements) || this.IsLogicPortObstructed(component2.OutputCellOne, visElements) || ((component2.RequiresTwoInputs || component2.RequiresFourInputs) && this.IsLogicPortObstructed(component2.InputCellTwo, visElements)) || (component2.RequiresFourInputs && (this.IsLogicPortObstructed(component2.InputCellThree, visElements) || this.IsLogicPortObstructed(component2.InputCellFour, visElements))) || (component2.RequiresFourOutputs && (this.IsLogicPortObstructed(component2.OutputCellTwo, visElements) || this.IsLogicPortObstructed(component2.OutputCellThree, visElements) || this.IsLogicPortObstructed(component2.OutputCellFour, visElements))) || (component2.RequiresControlInputs && (this.IsLogicPortObstructed(component2.ControlCellOne, visElements) || this.IsLogicPortObstructed(component2.ControlCellTwo, visElements)))))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x00160CCC File Offset: 0x0015EECC
	private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
	{
		if (ports_a == null || ports_b == null)
		{
			return false;
		}
		foreach (ILogicUIElement logicUIElement in ports_a)
		{
			int logicUICell = logicUIElement.GetLogicUICell();
			foreach (ILogicUIElement logicUIElement2 in ports_b)
			{
				if (logicUIElement != logicUIElement2 && logicUICell == logicUIElement2.GetLogicUICell())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x00160D68 File Offset: 0x0015EF68
	private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
	{
		int num = 0;
		using (IEnumerator<ILogicUIElement> enumerator = ports.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetLogicUICell() == cell)
				{
					num++;
				}
			}
		}
		return num > 0;
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x00160DBC File Offset: 0x0015EFBC
	private bool IsValidConduitConnection(GameObject source_go, ConduitType conduit_type, int utility_cell, ref string fail_reason)
	{
		bool result = true;
		switch (conduit_type)
		{
		case ConduitType.Gas:
		{
			GameObject x = Grid.Objects[utility_cell, 15];
			if (x != null && x != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Liquid:
		{
			GameObject x2 = Grid.Objects[utility_cell, 19];
			if (x2 != null && x2 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Solid:
		{
			GameObject x3 = Grid.Objects[utility_cell, 23];
			if (x3 != null && x3 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
			}
			break;
		}
		}
		return result;
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x00160E76 File Offset: 0x0015F076
	public static int GetXOffset(int width)
	{
		return -(width - 1) / 2;
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x00160E80 File Offset: 0x0015F080
	public static bool CheckFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		if (location_rule == BuildLocationRule.OnBackWall)
		{
			return BuildingDef.CheckBackWallFoundation(cell, width, height, orientation);
		}
		if (location_rule == BuildLocationRule.OnWall)
		{
			return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.InCorner)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.WallFloor)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height, optionalFoundationRequiredTag);
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x00160F0C File Offset: 0x0015F10C
	public static bool CheckBaseFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		for (int i = num; i <= num2; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset((location_rule == BuildLocationRule.OnCeiling) ? new CellOffset(i, height) : new CellOffset(i, -1), orientation);
			int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num3) || !Grid.Solid[num3])
			{
				return false;
			}
			if (optionalFoundationRequiredTag.IsValid && (!Grid.ObjectLayers[9].ContainsKey(num3) || !Grid.ObjectLayers[9][num3].HasTag(optionalFoundationRequiredTag)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x00160F9C File Offset: 0x0015F19C
	public static bool CheckBackWallFoundation(int cell, int width, int height, Orientation orientation)
	{
		for (int i = 0; i < height; i++)
		{
			int num = -(width - 1) / 2;
			int num2 = width / 2;
			for (int j = num; j <= num2; j++)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(j, i, orientation);
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
				if (Grid.Solid[num3] || !Grid.ObjectLayers[2].ContainsKey(num3) || Grid.ObjectLayers[2][num3] == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x00161018 File Offset: 0x0015F218
	public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
	{
		for (int i = 0; i < height; i++)
		{
			CellOffset offset = new CellOffset(leftWall ? (-(width - 1) / 2 - 1) : (width / 2 + 1), i);
			int num = Grid.OffsetCell(cell, offset);
			GameObject gameObject = Grid.Objects[num, 1];
			bool flag = false;
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && component.Def.IsFoundation)
				{
					flag = true;
				}
			}
			if (!Grid.IsValidBuildingCell(num) || (!Grid.Solid[num] && !flag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x001610B4 File Offset: 0x0015F2B4
	public static bool AreAllCellsValid(int base_cell, Orientation orientation, int width, int height, Func<int, bool> valid_cell_check)
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		if (orientation == Orientation.FlipH)
		{
			int num3 = num;
			num = -num2;
			num2 = -num3;
		}
		for (int i = 0; i < height; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				int arg = Grid.OffsetCell(base_cell, j, i);
				if (!valid_cell_check(arg))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x00161106 File Offset: 0x0015F306
	public Sprite GetUISprite(string animName = "ui", bool centered = false)
	{
		return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered, "");
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0016111C File Offset: 0x0015F31C
	public void GenerateOffsets()
	{
		this.GenerateOffsets(this.WidthInCells, this.HeightInCells);
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x00161130 File Offset: 0x0015F330
	public void GenerateOffsets(int width, int height)
	{
		if (!BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
		{
			int num = width / 2 - width + 1;
			this.PlacementOffsets = new CellOffset[width * height];
			for (int num2 = 0; num2 != height; num2++)
			{
				int num3 = num2 * width;
				for (int num4 = 0; num4 != width; num4++)
				{
					int num5 = num3 + num4;
					this.PlacementOffsets[num5].x = num4 + num;
					this.PlacementOffsets[num5].y = num2;
				}
			}
			BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
		}
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x001611CC File Offset: 0x0015F3CC
	public void PostProcess()
	{
		this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, 1f, (SimHashes)0, this.Name, null, 0);
		this.CraftRecipe.Icon = this.UISprite;
		for (int i = 0; i < this.MaterialCategory.Length; i++)
		{
			TagManager.Create(this.MaterialCategory[i], MATERIALS.GetMaterialString(this.MaterialCategory[i]));
			Recipe.Ingredient item = new Recipe.Ingredient(this.MaterialCategory[i], (float)((int)this.Mass[i]));
			this.CraftRecipe.Ingredients.Add(item);
		}
		if (this.DecorBlockTileInfo != null)
		{
			this.DecorBlockTileInfo.PostProcess();
		}
		if (this.DecorPlaceBlockTileInfo != null)
		{
			this.DecorPlaceBlockTileInfo.PostProcess();
		}
		if (!this.Deprecated)
		{
			TechItem techItem = Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite), this.RequiredDlcIds, this.ForbiddenDlcIds, this.POIUnlockable);
			if (techItem != null)
			{
				techItem.AddSearchTerms(this.searchTerms);
			}
		}
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x001612F8 File Offset: 0x0015F4F8
	public bool MaterialsAvailable(IList<Tag> selected_elements, WorldContainer world)
	{
		bool result = true;
		foreach (Recipe.Ingredient ingredient in this.CraftRecipe.GetAllIngredients(selected_elements))
		{
			if (world.worldInventory.GetAmount(ingredient.tag, true) < ingredient.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x00161348 File Offset: 0x0015F548
	public bool CheckRequiresBuildingCellVisualizer()
	{
		return this.CheckRequiresPowerInput() || this.CheckRequiresPowerOutput() || this.CheckRequiresGasInput() || this.CheckRequiresGasOutput() || this.CheckRequiresLiquidInput() || this.CheckRequiresLiquidOutput() || this.CheckRequiresSolidInput() || this.CheckRequiresSolidOutput() || this.CheckRequiresHighEnergyParticleInput() || this.CheckRequiresHighEnergyParticleOutput() || this.SelfHeatKilowattsWhenActive != 0f || this.ExhaustKilowattsWhenActive != 0f || this.DiseaseCellVisName != null;
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x001613CA File Offset: 0x0015F5CA
	public bool CheckRequiresPowerInput()
	{
		return this.RequiresPowerInput;
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x001613D2 File Offset: 0x0015F5D2
	public bool CheckRequiresPowerOutput()
	{
		return this.RequiresPowerOutput;
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x001613DA File Offset: 0x0015F5DA
	public bool CheckRequiresGasInput()
	{
		return this.InputConduitType == ConduitType.Gas;
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x001613E5 File Offset: 0x0015F5E5
	public bool CheckRequiresGasOutput()
	{
		return this.OutputConduitType == ConduitType.Gas;
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x001613F0 File Offset: 0x0015F5F0
	public bool CheckRequiresLiquidInput()
	{
		return this.InputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x001613FB File Offset: 0x0015F5FB
	public bool CheckRequiresLiquidOutput()
	{
		return this.OutputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x00161406 File Offset: 0x0015F606
	public bool CheckRequiresSolidInput()
	{
		return this.InputConduitType == ConduitType.Solid;
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x00161411 File Offset: 0x0015F611
	public bool CheckRequiresSolidOutput()
	{
		return this.OutputConduitType == ConduitType.Solid;
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0016141C File Offset: 0x0015F61C
	public bool CheckRequiresHighEnergyParticleInput()
	{
		return this.UseHighEnergyParticleInputPort;
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x00161424 File Offset: 0x0015F624
	public bool CheckRequiresHighEnergyParticleOutput()
	{
		return this.UseHighEnergyParticleOutputPort;
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0016142C File Offset: 0x0015F62C
	public void AddFacade(string db_facade_id)
	{
		if (this.AvailableFacades == null)
		{
			this.AvailableFacades = new List<string>();
		}
		if (!this.AvailableFacades.Contains(db_facade_id))
		{
			this.AvailableFacades.Add(db_facade_id);
		}
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0016145B File Offset: 0x0015F65B
	[Obsolete]
	public bool IsValidDLC()
	{
		return Game.IsCorrectDlcActiveForCurrentSave(this);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x00161463 File Offset: 0x0015F663
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x00161474 File Offset: 0x0015F674
	public static void CollectFabricationRecipes(Tag fabricatorId, List<ComplexRecipe> recipes)
	{
		foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
		{
			if (complexRecipe.fabricators.Contains(fabricatorId))
			{
				recipes.Add(complexRecipe);
			}
		}
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x001614DC File Offset: 0x0015F6DC
	public string[] GetRequiredDlcIds()
	{
		return this.RequiredDlcIds;
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x001614E4 File Offset: 0x0015F6E4
	public string[] GetForbiddenDlcIds()
	{
		return this.ForbiddenDlcIds;
	}

	// Token: 0x040026BF RID: 9919
	public string[] RequiredDlcIds;

	// Token: 0x040026C0 RID: 9920
	public string[] ForbiddenDlcIds;

	// Token: 0x040026C1 RID: 9921
	public float EnergyConsumptionWhenActive;

	// Token: 0x040026C2 RID: 9922
	public float GeneratorWattageRating;

	// Token: 0x040026C3 RID: 9923
	public float GeneratorBaseCapacity;

	// Token: 0x040026C4 RID: 9924
	public float MassForTemperatureModification;

	// Token: 0x040026C5 RID: 9925
	public float ExhaustKilowattsWhenActive;

	// Token: 0x040026C6 RID: 9926
	public float SelfHeatKilowattsWhenActive;

	// Token: 0x040026C7 RID: 9927
	public float BaseMeltingPoint;

	// Token: 0x040026C8 RID: 9928
	public float ConstructionTime;

	// Token: 0x040026C9 RID: 9929
	public float WorkTime;

	// Token: 0x040026CA RID: 9930
	public float ThermalConductivity = 1f;

	// Token: 0x040026CB RID: 9931
	public int WidthInCells;

	// Token: 0x040026CC RID: 9932
	public int HeightInCells;

	// Token: 0x040026CD RID: 9933
	public int HitPoints;

	// Token: 0x040026CE RID: 9934
	public float Temperature = 293.15f;

	// Token: 0x040026CF RID: 9935
	public bool RequiresPowerInput;

	// Token: 0x040026D0 RID: 9936
	public bool AddLogicPowerPort = true;

	// Token: 0x040026D1 RID: 9937
	public bool RequiresPowerOutput;

	// Token: 0x040026D2 RID: 9938
	public bool UseWhitePowerOutputConnectorColour;

	// Token: 0x040026D3 RID: 9939
	public CellOffset ElectricalArrowOffset;

	// Token: 0x040026D4 RID: 9940
	public ConduitType InputConduitType;

	// Token: 0x040026D5 RID: 9941
	public ConduitType OutputConduitType;

	// Token: 0x040026D6 RID: 9942
	public bool ModifiesTemperature;

	// Token: 0x040026D7 RID: 9943
	public bool Floodable = true;

	// Token: 0x040026D8 RID: 9944
	public bool Disinfectable = true;

	// Token: 0x040026D9 RID: 9945
	public bool Entombable = true;

	// Token: 0x040026DA RID: 9946
	public bool Replaceable = true;

	// Token: 0x040026DB RID: 9947
	public bool Invincible;

	// Token: 0x040026DC RID: 9948
	public bool Overheatable = true;

	// Token: 0x040026DD RID: 9949
	public bool Repairable = true;

	// Token: 0x040026DE RID: 9950
	public float OverheatTemperature = 348.15f;

	// Token: 0x040026DF RID: 9951
	public float FatalHot = 533.15f;

	// Token: 0x040026E0 RID: 9952
	public bool Breakable;

	// Token: 0x040026E1 RID: 9953
	public bool ContinuouslyCheckFoundation;

	// Token: 0x040026E2 RID: 9954
	public bool IsFoundation;

	// Token: 0x040026E3 RID: 9955
	[Obsolete]
	public bool isSolidTile;

	// Token: 0x040026E4 RID: 9956
	public bool DragBuild;

	// Token: 0x040026E5 RID: 9957
	public bool UseStructureTemperature = true;

	// Token: 0x040026E6 RID: 9958
	public global::Action HotKey = global::Action.NumActions;

	// Token: 0x040026E7 RID: 9959
	public CellOffset attachablePosition = new CellOffset(0, 0);

	// Token: 0x040026E8 RID: 9960
	public bool CanMove;

	// Token: 0x040026E9 RID: 9961
	public bool Cancellable = true;

	// Token: 0x040026EA RID: 9962
	public bool OnePerWorld;

	// Token: 0x040026EB RID: 9963
	public bool PlayConstructionSounds = true;

	// Token: 0x040026EC RID: 9964
	public Func<CodexEntry, CodexEntry> ExtendCodexEntry;

	// Token: 0x040026ED RID: 9965
	public bool POIUnlockable;

	// Token: 0x040026EE RID: 9966
	public List<Tag> ReplacementTags;

	// Token: 0x040026EF RID: 9967
	private readonly List<string> searchTerms = new List<string>();

	// Token: 0x040026F0 RID: 9968
	public List<ObjectLayer> ReplacementCandidateLayers;

	// Token: 0x040026F1 RID: 9969
	public List<ObjectLayer> EquivalentReplacementLayers;

	// Token: 0x040026F2 RID: 9970
	[HashedEnum]
	[NonSerialized]
	public HashedString ViewMode = OverlayModes.None.ID;

	// Token: 0x040026F3 RID: 9971
	public BuildLocationRule BuildLocationRule;

	// Token: 0x040026F4 RID: 9972
	public ObjectLayer ObjectLayer = ObjectLayer.Building;

	// Token: 0x040026F5 RID: 9973
	public ObjectLayer TileLayer = ObjectLayer.NumLayers;

	// Token: 0x040026F6 RID: 9974
	public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;

	// Token: 0x040026F7 RID: 9975
	public string DiseaseCellVisName;

	// Token: 0x040026F8 RID: 9976
	public string[] MaterialCategory;

	// Token: 0x040026F9 RID: 9977
	public string AudioCategory = "Metal";

	// Token: 0x040026FA RID: 9978
	public string AudioSize = "medium";

	// Token: 0x040026FB RID: 9979
	public float[] Mass;

	// Token: 0x040026FC RID: 9980
	public bool AlwaysOperational;

	// Token: 0x040026FD RID: 9981
	public List<LogicPorts.Port> LogicInputPorts;

	// Token: 0x040026FE RID: 9982
	public List<LogicPorts.Port> LogicOutputPorts;

	// Token: 0x040026FF RID: 9983
	public bool Upgradeable;

	// Token: 0x04002700 RID: 9984
	public float BaseTimeUntilRepair = 600f;

	// Token: 0x04002701 RID: 9985
	public bool ShowInBuildMenu = true;

	// Token: 0x04002702 RID: 9986
	public bool DebugOnly;

	// Token: 0x04002703 RID: 9987
	public PermittedRotations PermittedRotations;

	// Token: 0x04002704 RID: 9988
	public Orientation InitialOrientation;

	// Token: 0x04002705 RID: 9989
	public bool Deprecated;

	// Token: 0x04002706 RID: 9990
	public bool UseHighEnergyParticleInputPort;

	// Token: 0x04002707 RID: 9991
	public bool UseHighEnergyParticleOutputPort;

	// Token: 0x04002708 RID: 9992
	public CellOffset HighEnergyParticleInputOffset;

	// Token: 0x04002709 RID: 9993
	public CellOffset HighEnergyParticleOutputOffset;

	// Token: 0x0400270A RID: 9994
	public CellOffset PowerInputOffset;

	// Token: 0x0400270B RID: 9995
	public CellOffset PowerOutputOffset;

	// Token: 0x0400270C RID: 9996
	public CellOffset UtilityInputOffset = new CellOffset(0, 1);

	// Token: 0x0400270D RID: 9997
	public CellOffset UtilityOutputOffset = new CellOffset(1, 0);

	// Token: 0x0400270E RID: 9998
	public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;

	// Token: 0x0400270F RID: 9999
	public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04002710 RID: 10000
	public string RequiredAttribute = "";

	// Token: 0x04002711 RID: 10001
	public int RequiredAttributeLevel;

	// Token: 0x04002712 RID: 10002
	public List<Descriptor> EffectDescription;

	// Token: 0x04002713 RID: 10003
	public float MassTier;

	// Token: 0x04002714 RID: 10004
	public float HeatTier;

	// Token: 0x04002715 RID: 10005
	public float ConstructionTimeTier;

	// Token: 0x04002716 RID: 10006
	public string PrimaryUse;

	// Token: 0x04002717 RID: 10007
	public string SecondaryUse;

	// Token: 0x04002718 RID: 10008
	public string PrimarySideEffect;

	// Token: 0x04002719 RID: 10009
	public string SecondarySideEffect;

	// Token: 0x0400271A RID: 10010
	public Recipe CraftRecipe;

	// Token: 0x0400271B RID: 10011
	public Sprite UISprite;

	// Token: 0x0400271C RID: 10012
	public bool isKAnimTile;

	// Token: 0x0400271D RID: 10013
	public bool isUtility;

	// Token: 0x0400271E RID: 10014
	public KAnimFile[] AnimFiles;

	// Token: 0x0400271F RID: 10015
	public string DefaultAnimState = "off";

	// Token: 0x04002720 RID: 10016
	public bool BlockTileIsTransparent;

	// Token: 0x04002721 RID: 10017
	public TextureAtlas BlockTileAtlas;

	// Token: 0x04002722 RID: 10018
	public TextureAtlas BlockTilePlaceAtlas;

	// Token: 0x04002723 RID: 10019
	public TextureAtlas BlockTileShineAtlas;

	// Token: 0x04002724 RID: 10020
	public Material BlockTileMaterial;

	// Token: 0x04002725 RID: 10021
	public BlockTileDecorInfo DecorBlockTileInfo;

	// Token: 0x04002726 RID: 10022
	public BlockTileDecorInfo DecorPlaceBlockTileInfo;

	// Token: 0x04002727 RID: 10023
	public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();

	// Token: 0x04002728 RID: 10024
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04002729 RID: 10025
	public Tag AttachmentSlotTag;

	// Token: 0x0400272A RID: 10026
	public bool PreventIdleTraversalPastBuilding;

	// Token: 0x0400272B RID: 10027
	public GameObject BuildingComplete;

	// Token: 0x0400272C RID: 10028
	public GameObject BuildingPreview;

	// Token: 0x0400272D RID: 10029
	public GameObject BuildingUnderConstruction;

	// Token: 0x0400272E RID: 10030
	public CellOffset[] PlacementOffsets;

	// Token: 0x0400272F RID: 10031
	public CellOffset[] ConstructionOffsetFilter;

	// Token: 0x04002730 RID: 10032
	public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04002731 RID: 10033
	public float BaseDecor;

	// Token: 0x04002732 RID: 10034
	public float BaseDecorRadius;

	// Token: 0x04002733 RID: 10035
	public int BaseNoisePollution;

	// Token: 0x04002734 RID: 10036
	public int BaseNoisePollutionRadius;

	// Token: 0x04002735 RID: 10037
	public List<string> AvailableFacades = new List<string>();

	// Token: 0x04002736 RID: 10038
	public string RequiredSkillPerkID;

	// Token: 0x04002737 RID: 10039
	private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();
}
