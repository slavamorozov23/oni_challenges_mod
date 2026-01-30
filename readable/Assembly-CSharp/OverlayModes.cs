using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6A RID: 3178
public abstract class OverlayModes
{
	// Token: 0x02001E24 RID: 7716
	public class GasConduits : OverlayModes.ConduitMode
	{
		// Token: 0x0600B332 RID: 45874 RVA: 0x003E2E4C File Offset: 0x003E104C
		public override HashedString ViewMode()
		{
			return OverlayModes.GasConduits.ID;
		}

		// Token: 0x0600B333 RID: 45875 RVA: 0x003E2E53 File Offset: 0x003E1053
		public override string GetSoundName()
		{
			return "GasVent";
		}

		// Token: 0x0600B334 RID: 45876 RVA: 0x003E2E5A File Offset: 0x003E105A
		public GasConduits() : base(OverlayScreen.GasVentIDs)
		{
		}

		// Token: 0x04008D81 RID: 36225
		public static readonly HashedString ID = "GasConduit";
	}

	// Token: 0x02001E25 RID: 7717
	public class LiquidConduits : OverlayModes.ConduitMode
	{
		// Token: 0x0600B336 RID: 45878 RVA: 0x003E2E78 File Offset: 0x003E1078
		public override HashedString ViewMode()
		{
			return OverlayModes.LiquidConduits.ID;
		}

		// Token: 0x0600B337 RID: 45879 RVA: 0x003E2E7F File Offset: 0x003E107F
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x0600B338 RID: 45880 RVA: 0x003E2E86 File Offset: 0x003E1086
		public LiquidConduits() : base(OverlayScreen.LiquidVentIDs)
		{
		}

		// Token: 0x04008D82 RID: 36226
		public static readonly HashedString ID = "LiquidConduit";
	}

	// Token: 0x02001E26 RID: 7718
	public abstract class ConduitMode : OverlayModes.Mode
	{
		// Token: 0x0600B33A RID: 45882 RVA: 0x003E2EA4 File Offset: 0x003E10A4
		public ConduitMode(ICollection<Tag> ids)
		{
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = ids;
		}

		// Token: 0x0600B33B RID: 45883 RVA: 0x003E2F2C File Offset: 0x003E112C
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600B33C RID: 45884 RVA: 0x003E2F88 File Offset: 0x003E1188
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600B33D RID: 45885 RVA: 0x003E2FBC File Offset: 0x003E11BC
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600B33E RID: 45886 RVA: 0x003E3008 File Offset: 0x003E1208
		public override void Disable()
		{
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = saveLoadRoot.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600B33F RID: 45887 RVA: 0x003E30F8 File Offset: 0x003E12F8
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
				Vector3 position = root.transform.GetPosition();
				position.z = defaultDepth;
				root.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			});
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
				if (saveLoadRoot.GetComponent<Conduit>() != null)
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.conduitTargetLayer, null, null);
				}
				else
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
					{
						Vector3 position = root.transform.GetPosition();
						float z = position.z;
						KPrefabID component3 = root.GetComponent<KPrefabID>();
						if (component3 != null)
						{
							if (component3.HasTag(GameTags.OverlayInFrontOfConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) - 0.2f;
							}
							else if (component3.HasTag(GameTags.OverlayBehindConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) + 0.2f;
							}
						}
						position.z = z;
						root.transform.SetPosition(position);
						KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							this.TriggerResorting(componentsInChildren[i]);
						}
					}, null);
				}
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component != null)
				{
					int networkCell = component.GetNetworkCell();
					UtilityNetworkManager<FlowUtilityNetwork, Vent> mgr = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitSystem : Game.Instance.gasConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, mgr, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			Game.ConduitVisInfo conduitVisInfo = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitVisInfo : Game.Instance.gasConduitVisInfo;
			foreach (SaveLoadRoot saveLoadRoot2 in this.layerTargets)
			{
				if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<IBridgedNetworkItem>() != null)
				{
					BuildingDef def = saveLoadRoot2.GetComponent<Building>().Def;
					Color32 colorByName;
					if (def.ThermalConductivity == 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayTintName);
					}
					else if (def.ThermalConductivity < 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayInsulatedTintName);
					}
					else
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayRadiantTintName);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component2 = saveLoadRoot2.GetComponent<IBridgedNetworkItem>();
						if (component2 != null && component2.IsConnectedToNetworks(this.connectedNetworks))
						{
							colorByName.r = (byte)((float)colorByName.r * num);
							colorByName.g = (byte)((float)colorByName.g * num);
							colorByName.b = (byte)((float)colorByName.b * num);
						}
					}
					saveLoadRoot2.GetComponent<KBatchedAnimController>().TintColour = colorByName;
				}
			}
		}

		// Token: 0x0600B340 RID: 45888 RVA: 0x003E342C File Offset: 0x003E162C
		private void TriggerResorting(KBatchedAnimController kbac)
		{
			if (kbac.enabled)
			{
				kbac.enabled = false;
				kbac.enabled = true;
			}
		}

		// Token: 0x0600B341 RID: 45889 RVA: 0x003E3444 File Offset: 0x003E1644
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						IBridgedNetworkItem component = networkItem.GameObject.GetComponent<IBridgedNetworkItem>();
						if (component != null)
						{
							component.AddNetworks(networks);
						}
					}
				}
			}
		}

		// Token: 0x04008D83 RID: 36227
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04008D84 RID: 36228
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008D85 RID: 36229
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04008D86 RID: 36230
		private List<int> visited = new List<int>();

		// Token: 0x04008D87 RID: 36231
		private ICollection<Tag> targetIDs;

		// Token: 0x04008D88 RID: 36232
		private int objectTargetLayer;

		// Token: 0x04008D89 RID: 36233
		private int conduitTargetLayer;

		// Token: 0x04008D8A RID: 36234
		private int cameraLayerMask;

		// Token: 0x04008D8B RID: 36235
		private int selectionMask;
	}

	// Token: 0x02001E27 RID: 7719
	public class Crop : OverlayModes.BasePlantMode
	{
		// Token: 0x0600B344 RID: 45892 RVA: 0x003E3628 File Offset: 0x003E1828
		public override HashedString ViewMode()
		{
			return OverlayModes.Crop.ID;
		}

		// Token: 0x0600B345 RID: 45893 RVA: 0x003E362F File Offset: 0x003E182F
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x0600B346 RID: 45894 RVA: 0x003E3638 File Offset: 0x003E1838
		public Crop(Canvas ui_root, GameObject harvestable_notification_prefab)
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[3];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropHalted, delegate(KMonoBehaviour h)
			{
				WiltCondition component = h.GetComponent<WiltCondition>();
				return component != null && component.IsWilting();
			});
			array[1] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrowing, (KMonoBehaviour h) => !(h as HarvestDesignatable).CanBeHarvested());
			array[2] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrown, (KMonoBehaviour h) => (h as HarvestDesignatable).CanBeHarvested());
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
			this.uiRoot = ui_root;
			this.harvestableNotificationPrefab = harvestable_notification_prefab;
		}

		// Token: 0x0600B347 RID: 45895 RVA: 0x003E3754 File Offset: 0x003E1954
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.CROP.FULLY_GROWN, UI.OVERLAYS.CROP.TOOLTIPS.FULLY_GROWN, GlobalAssets.Instance.colorSet.cropGrown, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWING, UI.OVERLAYS.CROP.TOOLTIPS.GROWING, GlobalAssets.Instance.colorSet.cropGrowing, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWTH_HALTED, UI.OVERLAYS.CROP.TOOLTIPS.GROWTH_HALTED, GlobalAssets.Instance.colorSet.cropHalted, null, null, true)
			};
		}

		// Token: 0x0600B348 RID: 45896 RVA: 0x003E3808 File Offset: 0x003E1A08
		public override void Update()
		{
			this.updateCropInfo.Clear();
			this.freeHarvestableNotificationIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (HarvestDesignatable harvestDesignatable in this.layerTargets)
			{
				Vector2I vector2I3 = Grid.PosToXY(harvestDesignatable.transform.GetPosition());
				if (vector2I <= vector2I3 && vector2I3 <= vector2I2)
				{
					this.AddCropUI(harvestDesignatable);
				}
			}
			foreach (OverlayModes.Crop.UpdateCropInfo updateCropInfo in this.updateCropInfo)
			{
				updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>().Refresh(updateCropInfo.harvestable);
			}
			for (int i = this.freeHarvestableNotificationIdx; i < this.harvestableNotificationList.Count; i++)
			{
				if (this.harvestableNotificationList[i].activeSelf)
				{
					this.harvestableNotificationList[i].SetActive(false);
				}
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x0600B349 RID: 45897 RVA: 0x003E39F8 File Offset: 0x003E1BF8
		public override void Disable()
		{
			this.DisableHarvestableUINotifications();
			base.Disable();
		}

		// Token: 0x0600B34A RID: 45898 RVA: 0x003E3A08 File Offset: 0x003E1C08
		private void DisableHarvestableUINotifications()
		{
			this.freeHarvestableNotificationIdx = 0;
			foreach (GameObject gameObject in this.harvestableNotificationList)
			{
				gameObject.SetActive(false);
			}
			this.updateCropInfo.Clear();
		}

		// Token: 0x0600B34B RID: 45899 RVA: 0x003E3A6C File Offset: 0x003E1C6C
		public GameObject GetFreeCropUI()
		{
			GameObject gameObject;
			if (this.freeHarvestableNotificationIdx < this.harvestableNotificationList.Count)
			{
				gameObject = this.harvestableNotificationList[this.freeHarvestableNotificationIdx];
				if (!gameObject.gameObject.activeSelf)
				{
					gameObject.gameObject.SetActive(true);
				}
				this.freeHarvestableNotificationIdx++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.harvestableNotificationPrefab.gameObject, this.uiRoot.transform.gameObject, false);
				this.harvestableNotificationList.Add(gameObject);
				this.freeHarvestableNotificationIdx++;
			}
			return gameObject;
		}

		// Token: 0x0600B34C RID: 45900 RVA: 0x003E3B08 File Offset: 0x003E1D08
		private void AddCropUI(HarvestDesignatable harvestable)
		{
			GameObject freeCropUI = this.GetFreeCropUI();
			OverlayModes.Crop.UpdateCropInfo item = new OverlayModes.Crop.UpdateCropInfo(harvestable, freeCropUI);
			Vector3 b = Grid.CellToPos(Grid.PosToCell(harvestable), 0.5f, -1.25f, 0f) + harvestable.iconOffset;
			freeCropUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b);
			this.updateCropInfo.Add(item);
		}

		// Token: 0x04008D8C RID: 36236
		public static readonly HashedString ID = "Crop";

		// Token: 0x04008D8D RID: 36237
		private Canvas uiRoot;

		// Token: 0x04008D8E RID: 36238
		private List<OverlayModes.Crop.UpdateCropInfo> updateCropInfo = new List<OverlayModes.Crop.UpdateCropInfo>();

		// Token: 0x04008D8F RID: 36239
		private int freeHarvestableNotificationIdx;

		// Token: 0x04008D90 RID: 36240
		private List<GameObject> harvestableNotificationList = new List<GameObject>();

		// Token: 0x04008D91 RID: 36241
		private GameObject harvestableNotificationPrefab;

		// Token: 0x04008D92 RID: 36242
		private OverlayModes.ColorHighlightCondition[] highlightConditions;

		// Token: 0x02002A58 RID: 10840
		private struct UpdateCropInfo
		{
			// Token: 0x0600D468 RID: 54376 RVA: 0x0043C5FB File Offset: 0x0043A7FB
			public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI)
			{
				this.harvestable = harvestable;
				this.harvestableUI = harvestableUI;
			}

			// Token: 0x0400BB1D RID: 47901
			public HarvestDesignatable harvestable;

			// Token: 0x0400BB1E RID: 47902
			public GameObject harvestableUI;
		}
	}

	// Token: 0x02001E28 RID: 7720
	public class Harvest : OverlayModes.BasePlantMode
	{
		// Token: 0x0600B34E RID: 45902 RVA: 0x003E3B84 File Offset: 0x003E1D84
		public override HashedString ViewMode()
		{
			return OverlayModes.Harvest.ID;
		}

		// Token: 0x0600B34F RID: 45903 RVA: 0x003E3B8B File Offset: 0x003E1D8B
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x0600B350 RID: 45904 RVA: 0x003E3B94 File Offset: 0x003E1D94
		public Harvest()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour harvestable) => new Color(0.65f, 0.65f, 0.65f, 0.65f), (KMonoBehaviour harvestable) => true);
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
		}

		// Token: 0x0600B351 RID: 45905 RVA: 0x003E3C00 File Offset: 0x003E1E00
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x04008D93 RID: 36243
		public static readonly HashedString ID = "HarvestWhenReady";

		// Token: 0x04008D94 RID: 36244
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001E29 RID: 7721
	public abstract class BasePlantMode : OverlayModes.Mode
	{
		// Token: 0x0600B353 RID: 45907 RVA: 0x003E3CEC File Offset: 0x003E1EEC
		public BasePlantMode(ICollection<Tag> ids)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			this.targetIDs = ids;
		}

		// Token: 0x0600B354 RID: 45908 RVA: 0x003E3D5B File Offset: 0x003E1F5B
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<HarvestDesignatable>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
		}

		// Token: 0x0600B355 RID: 45909 RVA: 0x003E3D9C File Offset: 0x003E1F9C
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (!this.targetIDs.Contains(saveLoadTag))
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			this.partition.Add(component);
		}

		// Token: 0x0600B356 RID: 45910 RVA: 0x003E3DE4 File Offset: 0x003E1FE4
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x0600B357 RID: 45911 RVA: 0x003E3E44 File Offset: 0x003E2044
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			base.DisableHighlightTypeOverlay<HarvestDesignatable>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.partition.Clear();
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x04008D95 RID: 36245
		protected UniformGrid<HarvestDesignatable> partition;

		// Token: 0x04008D96 RID: 36246
		protected HashSet<HarvestDesignatable> layerTargets = new HashSet<HarvestDesignatable>();

		// Token: 0x04008D97 RID: 36247
		protected ICollection<Tag> targetIDs;

		// Token: 0x04008D98 RID: 36248
		protected int targetLayer;

		// Token: 0x04008D99 RID: 36249
		private int cameraLayerMask;

		// Token: 0x04008D9A RID: 36250
		private int selectionMask;
	}

	// Token: 0x02001E2A RID: 7722
	public class Decor : OverlayModes.Mode
	{
		// Token: 0x0600B358 RID: 45912 RVA: 0x003E3E9B File Offset: 0x003E209B
		public override HashedString ViewMode()
		{
			return OverlayModes.Decor.ID;
		}

		// Token: 0x0600B359 RID: 45913 RVA: 0x003E3EA2 File Offset: 0x003E20A2
		public override string GetSoundName()
		{
			return "Decor";
		}

		// Token: 0x0600B35A RID: 45914 RVA: 0x003E3EAC File Offset: 0x003E20AC
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.DECOR.HIGHDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.HIGHDECOR, GlobalAssets.Instance.colorSet.decorPositive, null, null, true),
				new LegendEntry(UI.OVERLAYS.DECOR.LOWDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.LOWDECOR, GlobalAssets.Instance.colorSet.decorNegative, null, null, true)
			};
		}

		// Token: 0x0600B35B RID: 45915 RVA: 0x003E3F2C File Offset: 0x003E212C
		public Decor()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour dp)
			{
				Color black = Color.black;
				Color b = Color.black;
				if (dp != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					float decorForCell = (dp as DecorProvider).GetDecorForCell(cell);
					if (decorForCell > 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
					}
					else if (decorForCell < 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
					}
					else if (dp.GetComponent<MonumentPart>() != null && dp.GetComponent<MonumentPart>().IsMonumentCompleted())
					{
						foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(dp.GetComponent<AttachableBuilding>()))
						{
							decorForCell = gameObject.GetComponent<DecorProvider>().GetDecorForCell(cell);
							if (decorForCell > 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
								break;
							}
							if (decorForCell < 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
								break;
							}
						}
					}
				}
				return Color.Lerp(black, b, 0.85f);
			}, (KMonoBehaviour dp) => SelectToolHoverTextCard.highlightedObjects.Contains(dp.gameObject));
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x0600B35C RID: 45916 RVA: 0x003E3FE4 File Offset: 0x003E21E4
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<DecorProvider>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			foreach (Tag item in new Tag[]
			{
				new Tag("Tile"),
				new Tag("SnowTile"),
				new Tag("WoodTile"),
				new Tag("MeshTile"),
				new Tag("InsulationTile"),
				new Tag("GasPermeableMembrane"),
				new Tag("CarpetTile")
			})
			{
				this.targetIDs.Remove(item);
			}
			foreach (Tag item2 in OverlayScreen.GasVentIDs)
			{
				this.targetIDs.Remove(item2);
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				this.targetIDs.Remove(item3);
			}
			this.partition = OverlayModes.Mode.PopulatePartition<DecorProvider>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600B35D RID: 45917 RVA: 0x003E416C File Offset: 0x003E236C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<DecorProvider>(this.layerTargets, vector2I, vector2I2, null);
			this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), this.workingTargets);
			for (int i = 0; i < this.workingTargets.Count; i++)
			{
				DecorProvider instance = this.workingTargets[i];
				base.AddTargetIfVisible<DecorProvider>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<DecorProvider>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
			this.workingTargets.Clear();
		}

		// Token: 0x0600B35E RID: 45918 RVA: 0x003E4230 File Offset: 0x003E2430
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				DecorProvider component = item.GetComponent<DecorProvider>();
				if (component != null)
				{
					this.partition.Add(component);
				}
			}
		}

		// Token: 0x0600B35F RID: 45919 RVA: 0x003E4274 File Offset: 0x003E2474
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			DecorProvider component = item.GetComponent<DecorProvider>();
			if (component != null)
			{
				if (this.layerTargets.Contains(component))
				{
					this.layerTargets.Remove(component);
				}
				this.partition.Remove(component);
			}
		}

		// Token: 0x0600B360 RID: 45920 RVA: 0x003E42D0 File Offset: 0x003E24D0
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<DecorProvider>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04008D9B RID: 36251
		public static readonly HashedString ID = "Decor";

		// Token: 0x04008D9C RID: 36252
		private UniformGrid<DecorProvider> partition;

		// Token: 0x04008D9D RID: 36253
		private HashSet<DecorProvider> layerTargets = new HashSet<DecorProvider>();

		// Token: 0x04008D9E RID: 36254
		private List<DecorProvider> workingTargets = new List<DecorProvider>();

		// Token: 0x04008D9F RID: 36255
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04008DA0 RID: 36256
		private int targetLayer;

		// Token: 0x04008DA1 RID: 36257
		private int cameraLayerMask;

		// Token: 0x04008DA2 RID: 36258
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001E2B RID: 7723
	public class Disease : OverlayModes.Mode
	{
		// Token: 0x0600B362 RID: 45922 RVA: 0x003E4330 File Offset: 0x003E2530
		private static float CalculateHUE(Color32 colour)
		{
			byte b = Math.Max(colour.r, Math.Max(colour.g, colour.b));
			byte b2 = Math.Min(colour.r, Math.Min(colour.g, colour.b));
			float result = 0f;
			int num = (int)(b - b2);
			if (num == 0)
			{
				result = 0f;
			}
			else if (b == colour.r)
			{
				result = (float)(colour.g - colour.b) / (float)num % 6f;
			}
			else if (b == colour.g)
			{
				result = (float)(colour.b - colour.r) / (float)num + 2f;
			}
			else if (b == colour.b)
			{
				result = (float)(colour.r - colour.g) / (float)num + 4f;
			}
			return result;
		}

		// Token: 0x0600B363 RID: 45923 RVA: 0x003E43F4 File Offset: 0x003E25F4
		public override HashedString ViewMode()
		{
			return OverlayModes.Disease.ID;
		}

		// Token: 0x0600B364 RID: 45924 RVA: 0x003E43FB File Offset: 0x003E25FB
		public override string GetSoundName()
		{
			return "Disease";
		}

		// Token: 0x0600B365 RID: 45925 RVA: 0x003E4404 File Offset: 0x003E2604
		public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab)
		{
			this.diseaseUIParent = diseaseUIParent;
			this.diseaseOverlayPrefab = diseaseOverlayPrefab;
			this.legendFilters = this.CreateDefaultFilters();
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x0600B366 RID: 45926 RVA: 0x003E448C File Offset: 0x003E268C
		public override void Enable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disease);
			CameraController.Instance.ToggleColouredOverlayView(true);
			Camera.main.cullingMask |= this.cameraLayerMask;
			base.RegisterSaveLoadListeners();
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(this.ViewMode());
				}
			}
		}

		// Token: 0x0600B367 RID: 45927 RVA: 0x003E4524 File Offset: 0x003E2724
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GASCONDUIT,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600B368 RID: 45928 RVA: 0x003E454F File Offset: 0x003E274F
		public override void OnFiltersChanged()
		{
			Game.Instance.showGasConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, this.legendFilters);
			Game.Instance.showLiquidConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, this.legendFilters);
		}

		// Token: 0x0600B369 RID: 45929 RVA: 0x003E4588 File Offset: 0x003E2788
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			if (item == null)
			{
				return;
			}
			KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			InfraredVisualizerComponents.ClearOverlayColour(component);
		}

		// Token: 0x0600B36A RID: 45930 RVA: 0x003E45B6 File Offset: 0x003E27B6
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
		}

		// Token: 0x0600B36B RID: 45931 RVA: 0x003E45B8 File Offset: 0x003E27B8
		public override void Disable()
		{
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(OverlayModes.None.ID);
				}
			}
			base.UnregisterSaveLoadListeners();
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			foreach (KMonoBehaviour kmonoBehaviour in this.layerTargets)
			{
				if (!(kmonoBehaviour == null))
				{
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
					Vector3 position = kmonoBehaviour.transform.GetPosition();
					position.z = defaultDepth;
					kmonoBehaviour.transform.SetPosition(position);
					KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
					component.enabled = false;
					component.enabled = true;
				}
			}
			CameraController.Instance.ToggleColouredOverlayView(false);
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			Game.Instance.showGasConduitDisease = false;
			Game.Instance.showLiquidConduitDisease = false;
			this.freeDiseaseUI = 0;
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.gameObject.SetActive(false);
			}
			this.updateDiseaseInfo.Clear();
			this.privateTargets.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x0600B36C RID: 45932 RVA: 0x003E475C File Offset: 0x003E295C
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<OverlayModes.Disease.DiseaseSortInfo> list2 = new List<OverlayModes.Disease.DiseaseSortInfo>();
			foreach (Klei.AI.Disease d in Db.Get().Diseases.resources)
			{
				list2.Add(new OverlayModes.Disease.DiseaseSortInfo(d));
			}
			list2.Sort((OverlayModes.Disease.DiseaseSortInfo a, OverlayModes.Disease.DiseaseSortInfo b) => a.sortkey.CompareTo(b.sortkey));
			foreach (OverlayModes.Disease.DiseaseSortInfo diseaseSortInfo in list2)
			{
				list.Add(new LegendEntry(diseaseSortInfo.disease.Name, diseaseSortInfo.disease.overlayLegendHovertext.ToString(), GlobalAssets.Instance.colorSet.GetColorByName(diseaseSortInfo.disease.overlayColourName), null, null, true));
			}
			return list;
		}

		// Token: 0x0600B36D RID: 45933 RVA: 0x003E4874 File Offset: 0x003E2A74
		public GameObject GetFreeDiseaseUI()
		{
			GameObject gameObject;
			if (this.freeDiseaseUI < this.diseaseUIList.Count)
			{
				gameObject = this.diseaseUIList[this.freeDiseaseUI];
				gameObject.gameObject.SetActive(true);
				this.freeDiseaseUI++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.diseaseOverlayPrefab, this.diseaseUIParent.transform.gameObject, false);
				this.diseaseUIList.Add(gameObject);
				this.freeDiseaseUI++;
			}
			return gameObject;
		}

		// Token: 0x0600B36E RID: 45934 RVA: 0x003E48FC File Offset: 0x003E2AFC
		private void AddDiseaseUI(MinionIdentity target)
		{
			GameObject gameObject = this.GetFreeDiseaseUI();
			DiseaseOverlayWidget component = gameObject.GetComponent<DiseaseOverlayWidget>();
			AmountInstance amount_inst = target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel);
			OverlayModes.Disease.UpdateDiseaseInfo item = new OverlayModes.Disease.UpdateDiseaseInfo(amount_inst, component);
			KAnimControllerBase component2 = target.GetComponent<KAnimControllerBase>();
			Vector3 position = (component2 != null) ? component2.GetWorldPivot() : (target.transform.GetPosition() + Vector3.down);
			gameObject.GetComponent<RectTransform>().SetPosition(position);
			this.updateDiseaseInfo.Add(item);
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x003E4988 File Offset: 0x003E2B88
		public override void Update()
		{
			Vector2I u;
			Vector2I v;
			Grid.GetVisibleExtents(out u, out v);
			this.queuedAdds.Clear();
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (!(minionIdentity == null))
				{
					Vector2I vector2I = Grid.PosToXY(minionIdentity.transform.GetPosition());
					if (u <= vector2I && vector2I <= v && !this.privateTargets.Contains(minionIdentity))
					{
						this.AddDiseaseUI(minionIdentity);
						this.queuedAdds.Add(minionIdentity);
					}
				}
			}
			foreach (KMonoBehaviour item in this.queuedAdds)
			{
				this.privateTargets.Add(item);
			}
			this.queuedAdds.Clear();
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);
			}
			bool flag = false;
			if (Game.Instance.showLiquidConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.LiquidVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item2 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item2))
						{
							OverlayScreen.DiseaseIDs.Add(item2);
							flag = true;
						}
					}
					goto IL_1D3;
				}
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item3))
				{
					OverlayScreen.DiseaseIDs.Remove(item3);
					flag = true;
				}
			}
			IL_1D3:
			if (Game.Instance.showGasConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.GasVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item4 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item4))
						{
							OverlayScreen.DiseaseIDs.Add(item4);
							flag = true;
						}
					}
					goto IL_279;
				}
			}
			foreach (Tag item5 in OverlayScreen.GasVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item5))
				{
					OverlayScreen.DiseaseIDs.Remove(item5);
					flag = true;
				}
			}
			IL_279:
			if (flag)
			{
				this.SetLayerZ(-50f);
			}
		}

		// Token: 0x0600B370 RID: 45936 RVA: 0x003E4C74 File Offset: 0x003E2E74
		private void SetLayerZ(float offset_z)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.ClearOutsideViewObjects<KMonoBehaviour>(this.layerTargets, vector2I, vector2I2, OverlayScreen.DiseaseIDs, delegate(KMonoBehaviour go)
			{
				if (go != null)
				{
					float defaultDepth2 = OverlayModes.Mode.GetDefaultDepth(go);
					Vector3 position2 = go.transform.GetPosition();
					position2.z = defaultDepth2;
					go.transform.SetPosition(position2);
					KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
					component2.enabled = false;
					component2.enabled = true;
				}
			});
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			foreach (Tag key in OverlayScreen.DiseaseIDs)
			{
				List<SaveLoadRoot> list;
				if (lists.TryGetValue(key, out list))
				{
					foreach (KMonoBehaviour kmonoBehaviour in list)
					{
						if (!(kmonoBehaviour == null) && !this.layerTargets.Contains(kmonoBehaviour))
						{
							Vector3 position = kmonoBehaviour.transform.GetPosition();
							if (Grid.IsVisible(Grid.PosToCell(position)) && vector2I <= position && position <= vector2I2)
							{
								float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
								position.z = defaultDepth + offset_z;
								kmonoBehaviour.transform.SetPosition(position);
								KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
								component.enabled = false;
								component.enabled = true;
								this.layerTargets.Add(kmonoBehaviour);
							}
						}
					}
				}
			}
		}

		// Token: 0x04008DA3 RID: 36259
		public static readonly HashedString ID = "Disease";

		// Token: 0x04008DA4 RID: 36260
		private int cameraLayerMask;

		// Token: 0x04008DA5 RID: 36261
		private int freeDiseaseUI;

		// Token: 0x04008DA6 RID: 36262
		private List<GameObject> diseaseUIList = new List<GameObject>();

		// Token: 0x04008DA7 RID: 36263
		private List<OverlayModes.Disease.UpdateDiseaseInfo> updateDiseaseInfo = new List<OverlayModes.Disease.UpdateDiseaseInfo>();

		// Token: 0x04008DA8 RID: 36264
		private HashSet<KMonoBehaviour> layerTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04008DA9 RID: 36265
		private HashSet<KMonoBehaviour> privateTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04008DAA RID: 36266
		private List<KMonoBehaviour> queuedAdds = new List<KMonoBehaviour>();

		// Token: 0x04008DAB RID: 36267
		private Canvas diseaseUIParent;

		// Token: 0x04008DAC RID: 36268
		private GameObject diseaseOverlayPrefab;

		// Token: 0x02002A5C RID: 10844
		private struct DiseaseSortInfo
		{
			// Token: 0x0600D479 RID: 54393 RVA: 0x0043C846 File Offset: 0x0043AA46
			public DiseaseSortInfo(Klei.AI.Disease d)
			{
				this.disease = d;
				this.sortkey = OverlayModes.Disease.CalculateHUE(GlobalAssets.Instance.colorSet.GetColorByName(d.overlayColourName));
			}

			// Token: 0x0400BB2C RID: 47916
			public float sortkey;

			// Token: 0x0400BB2D RID: 47917
			public Klei.AI.Disease disease;
		}

		// Token: 0x02002A5D RID: 10845
		private struct UpdateDiseaseInfo
		{
			// Token: 0x0600D47A RID: 54394 RVA: 0x0043C86F File Offset: 0x0043AA6F
			public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui)
			{
				this.ui = ui;
				this.valueSrc = amount_inst;
			}

			// Token: 0x0400BB2E RID: 47918
			public DiseaseOverlayWidget ui;

			// Token: 0x0400BB2F RID: 47919
			public AmountInstance valueSrc;
		}
	}

	// Token: 0x02001E2C RID: 7724
	public class Logic : OverlayModes.Mode
	{
		// Token: 0x0600B372 RID: 45938 RVA: 0x003E4E0D File Offset: 0x003E300D
		public override HashedString ViewMode()
		{
			return OverlayModes.Logic.ID;
		}

		// Token: 0x0600B373 RID: 45939 RVA: 0x003E4E14 File Offset: 0x003E3014
		public override string GetSoundName()
		{
			return "Logic";
		}

		// Token: 0x0600B374 RID: 45940 RVA: 0x003E4E1C File Offset: 0x003E301C
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.LOGIC.INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.INPUT, Color.white, null, Assets.GetSprite("logicInput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.OUTPUT, Color.white, null, Assets.GetSprite("logicOutput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_INPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_in"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_OUTPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_out"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RESET_UPDATE, UI.OVERLAYS.LOGIC.TOOLTIPS.RESET_UPDATE, Color.white, null, Assets.GetSprite("logicResetUpdate"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CONTROL_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.CONTROL_INPUT, Color.white, null, Assets.GetSprite("control_input_frame_legend"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CIRCUIT_STATUS_HEADER, null, Color.white, null, null, false),
				new LegendEntry(UI.OVERLAYS.LOGIC.ONE, null, GlobalAssets.Instance.colorSet.logicOnText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.ZERO, null, GlobalAssets.Instance.colorSet.logicOffText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.DISCONNECTED, UI.OVERLAYS.LOGIC.TOOLTIPS.DISCONNECTED, GlobalAssets.Instance.colorSet.logicDisconnected, null, null, true)
			};
		}

		// Token: 0x0600B375 RID: 45941 RVA: 0x003E501C File Offset: 0x003E321C
		public Logic(LogicModeUI ui_asset)
		{
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.uiAsset = ui_asset;
		}

		// Token: 0x0600B376 RID: 45942 RVA: 0x003E5100 File Offset: 0x003E3300
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.gameObjPartition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayModes.Logic.HighlightItemIDs);
			this.ioPartition = this.CreateLogicUIPartition();
			GridCompositor.Instance.ToggleMinor(true);
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
		}

		// Token: 0x0600B377 RID: 45943 RVA: 0x003E51CC File Offset: 0x003E33CC
		public override void Disable()
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterLogicOn, STOP_MODE.ALLOWFADEOUT);
			foreach (SaveLoadRoot saveLoadRoot in this.gameObjTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = false;
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = true;
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.gameObjTargets);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.wireControllers);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.ribbonControllers);
			this.ResetRibbonSymbolTints<KBatchedAnimController>(this.ribbonControllers);
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
			{
				if (bridgeInfo.controller != null)
				{
					OverlayModes.Mode.ResetDisplayValues(bridgeInfo.controller);
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
			{
				if (bridgeInfo2.controller != null)
				{
					this.ResetRibbonTint(bridgeInfo2.controller);
				}
			}
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				uiinfo.Release();
			}
			this.uiInfo.Clear();
			this.uiNodes.Clear();
			this.ioPartition.Clear();
			this.ioTargets.Clear();
			this.gameObjPartition.Clear();
			this.gameObjTargets.Clear();
			this.wireControllers.Clear();
			this.ribbonControllers.Clear();
			this.bridgeControllers.Clear();
			this.ribbonBridgeControllers.Clear();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600B378 RID: 45944 RVA: 0x003E5488 File Offset: 0x003E3688
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayModes.Logic.HighlightItemIDs.Contains(saveLoadTag))
			{
				this.gameObjPartition.Add(item);
			}
		}

		// Token: 0x0600B379 RID: 45945 RVA: 0x003E54BC File Offset: 0x003E36BC
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.gameObjTargets.Contains(item))
			{
				this.gameObjTargets.Remove(item);
			}
			this.gameObjPartition.Remove(item);
		}

		// Token: 0x0600B37A RID: 45946 RVA: 0x003E5508 File Offset: 0x003E3708
		private void OnUIElemAdded(ILogicUIElement elem)
		{
			this.ioPartition.Add(elem);
		}

		// Token: 0x0600B37B RID: 45947 RVA: 0x003E5516 File Offset: 0x003E3716
		private void OnUIElemRemoved(ILogicUIElement elem)
		{
			this.ioPartition.Remove(elem);
			if (this.ioTargets.Contains(elem))
			{
				this.ioTargets.Remove(elem);
				this.FreeUI(elem);
			}
		}

		// Token: 0x0600B37C RID: 45948 RVA: 0x003E5548 File Offset: 0x003E3748
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			Tag wire_id = TagManager.Create("LogicWire");
			Tag ribbon_id = TagManager.Create("LogicRibbon");
			Tag bridge_id = TagManager.Create("LogicWireBridge");
			Tag ribbon_bridge_id = TagManager.Create("LogicRibbonBridge");
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.gameObjTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				KPrefabID component7 = root.GetComponent<KPrefabID>();
				if (component7 != null)
				{
					Tag prefabTag = component7.PrefabTag;
					if (prefabTag == wire_id)
					{
						this.wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == ribbon_id)
					{
						this.ResetRibbonTint(root.GetComponent<KBatchedAnimController>());
						this.ribbonControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.bridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					if (prefabTag == ribbon_bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.ResetRibbonTint(controller);
						this.ribbonBridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
					Vector3 position = root.transform.GetPosition();
					position.z = defaultDepth;
					root.transform.SetPosition(position);
					root.GetComponent<KBatchedAnimController>().enabled = false;
					root.GetComponent<KBatchedAnimController>().enabled = true;
				}
			});
			OverlayModes.Mode.RemoveOffscreenTargets<ILogicUIElement>(this.ioTargets, this.workingIOTargets, vector2I, vector2I2, new Action<ILogicUIElement>(this.FreeUI), null);
			Action<SaveLoadRoot> <>9__3;
			foreach (object obj in this.gameObjPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
				if (saveLoadRoot != null)
				{
					KPrefabID component = saveLoadRoot.GetComponent<KPrefabID>();
					if (component.PrefabTag == wire_id || component.PrefabTag == bridge_id || component.PrefabTag == ribbon_id || component.PrefabTag == ribbon_bridge_id)
					{
						SaveLoadRoot instance = saveLoadRoot;
						Vector2I vis_min = vector2I;
						Vector2I vis_max = vector2I2;
						ICollection<SaveLoadRoot> targets = this.gameObjTargets;
						int layer = this.conduitTargetLayer;
						Action<SaveLoadRoot> on_added;
						if ((on_added = <>9__3) == null)
						{
							on_added = (<>9__3 = delegate(SaveLoadRoot root)
							{
								if (root == null)
								{
									return;
								}
								KPrefabID component7 = root.GetComponent<KPrefabID>();
								if (OverlayModes.Logic.HighlightItemIDs.Contains(component7.PrefabTag))
								{
									if (component7.PrefabTag == wire_id)
									{
										this.wireControllers.Add(root.GetComponent<KBatchedAnimController>());
										return;
									}
									if (component7.PrefabTag == ribbon_id)
									{
										this.ribbonControllers.Add(root.GetComponent<KBatchedAnimController>());
										return;
									}
									if (component7.PrefabTag == bridge_id)
									{
										KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
										int networkCell2 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
										this.bridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
										{
											cell = networkCell2,
											controller = component8
										});
										return;
									}
									if (component7.PrefabTag == ribbon_bridge_id)
									{
										KBatchedAnimController component9 = root.GetComponent<KBatchedAnimController>();
										int networkCell3 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
										this.ribbonBridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
										{
											cell = networkCell3,
											controller = component9
										});
									}
								}
							});
						}
						base.AddTargetIfVisible<SaveLoadRoot>(instance, vis_min, vis_max, targets, layer, on_added, null);
					}
					else
					{
						base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.gameObjTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
						{
							Vector3 position = root.transform.GetPosition();
							float z = position.z;
							KPrefabID component7 = root.GetComponent<KPrefabID>();
							if (component7 != null)
							{
								if (component7.HasTag(GameTags.OverlayInFrontOfConduits))
								{
									z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) - 0.2f;
								}
								else if (component7.HasTag(GameTags.OverlayBehindConduits))
								{
									z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) + 0.2f;
								}
							}
							position.z = z;
							root.transform.SetPosition(position);
							KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
							component8.enabled = false;
							component8.enabled = true;
						}, null);
					}
				}
			}
			foreach (object obj2 in this.ioPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				ILogicUIElement logicUIElement = (ILogicUIElement)obj2;
				if (logicUIElement != null)
				{
					base.AddTargetIfVisible<ILogicUIElement>(logicUIElement, vector2I, vector2I2, this.ioTargets, this.objectTargetLayer, new Action<ILogicUIElement>(this.AddUI), (KMonoBehaviour kcmp) => kcmp != null && OverlayModes.Logic.HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag));
				}
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			if (gameObject != null)
			{
				IBridgedNetworkItem component2 = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component2 != null)
				{
					int networkCell = component2.GetNetworkCell();
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, Game.Instance.logicCircuitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
			Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
			logicOff.a = (logicOn.a = 0);
			foreach (KBatchedAnimController kbatchedAnimController in this.wireControllers)
			{
				if (!(kbatchedAnimController == null))
				{
					Color32 color = logicOff;
					LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController.transform.GetPosition()));
					if (networkForCell != null)
					{
						color = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component3 = kbatchedAnimController.GetComponent<IBridgedNetworkItem>();
						if (component3 != null && component3.IsConnectedToNetworks(this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
					}
					kbatchedAnimController.TintColour = color;
				}
			}
			foreach (KBatchedAnimController kbatchedAnimController2 in this.ribbonControllers)
			{
				if (!(kbatchedAnimController2 == null))
				{
					Color32 color2 = logicOff;
					Color32 color3 = logicOff;
					Color32 color4 = logicOff;
					Color32 color5 = logicOff;
					LogicCircuitNetwork networkForCell2 = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController2.transform.GetPosition()));
					if (networkForCell2 != null)
					{
						color2 = (networkForCell2.IsBitActive(0) ? logicOn : logicOff);
						color3 = (networkForCell2.IsBitActive(1) ? logicOn : logicOff);
						color4 = (networkForCell2.IsBitActive(2) ? logicOn : logicOff);
						color5 = (networkForCell2.IsBitActive(3) ? logicOn : logicOff);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component4 = kbatchedAnimController2.GetComponent<IBridgedNetworkItem>();
						if (component4 != null && component4.IsConnectedToNetworks(this.connectedNetworks))
						{
							color2.r = (byte)((float)color2.r * num);
							color2.g = (byte)((float)color2.g * num);
							color2.b = (byte)((float)color2.b * num);
							color3.r = (byte)((float)color3.r * num);
							color3.g = (byte)((float)color3.g * num);
							color3.b = (byte)((float)color3.b * num);
							color4.r = (byte)((float)color4.r * num);
							color4.g = (byte)((float)color4.g * num);
							color4.b = (byte)((float)color4.b * num);
							color5.r = (byte)((float)color5.r * num);
							color5.g = (byte)((float)color5.g * num);
							color5.b = (byte)((float)color5.b * num);
						}
					}
					kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color2);
					kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color3);
					kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color4);
					kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color5);
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
			{
				if (!(bridgeInfo.controller == null))
				{
					Color32 color6 = logicOff;
					LogicCircuitNetwork networkForCell3 = logicCircuitManager.GetNetworkForCell(bridgeInfo.cell);
					if (networkForCell3 != null)
					{
						color6 = (networkForCell3.IsBitActive(0) ? logicOn : logicOff);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component5 = bridgeInfo.controller.GetComponent<IBridgedNetworkItem>();
						if (component5 != null && component5.IsConnectedToNetworks(this.connectedNetworks))
						{
							color6.r = (byte)((float)color6.r * num);
							color6.g = (byte)((float)color6.g * num);
							color6.b = (byte)((float)color6.b * num);
						}
					}
					bridgeInfo.controller.TintColour = color6;
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
			{
				if (!(bridgeInfo2.controller == null))
				{
					Color32 color7 = logicOff;
					Color32 color8 = logicOff;
					Color32 color9 = logicOff;
					Color32 color10 = logicOff;
					LogicCircuitNetwork networkForCell4 = logicCircuitManager.GetNetworkForCell(bridgeInfo2.cell);
					if (networkForCell4 != null)
					{
						color7 = (networkForCell4.IsBitActive(0) ? logicOn : logicOff);
						color8 = (networkForCell4.IsBitActive(1) ? logicOn : logicOff);
						color9 = (networkForCell4.IsBitActive(2) ? logicOn : logicOff);
						color10 = (networkForCell4.IsBitActive(3) ? logicOn : logicOff);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component6 = bridgeInfo2.controller.GetComponent<IBridgedNetworkItem>();
						if (component6 != null && component6.IsConnectedToNetworks(this.connectedNetworks))
						{
							color7.r = (byte)((float)color7.r * num);
							color7.g = (byte)((float)color7.g * num);
							color7.b = (byte)((float)color7.b * num);
							color8.r = (byte)((float)color8.r * num);
							color8.g = (byte)((float)color8.g * num);
							color8.b = (byte)((float)color8.b * num);
							color9.r = (byte)((float)color9.r * num);
							color9.g = (byte)((float)color9.g * num);
							color9.b = (byte)((float)color9.b * num);
							color10.r = (byte)((float)color10.r * num);
							color10.g = (byte)((float)color10.g * num);
							color10.b = (byte)((float)color10.b * num);
						}
					}
					bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color7);
					bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color8);
					bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color9);
					bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color10);
				}
			}
			this.UpdateUI();
		}

		// Token: 0x0600B37D RID: 45949 RVA: 0x003E5F4C File Offset: 0x003E414C
		private void UpdateUI()
		{
			Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
			Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
			Color32 logicDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
			logicOff.a = (logicOn.a = byte.MaxValue);
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(uiinfo.cell);
				Color32 c = logicDisconnected;
				LogicControlInputUI component = uiinfo.instance.GetComponent<LogicControlInputUI>();
				if (component != null)
				{
					component.SetContent(networkForCell);
				}
				else if (uiinfo.bitDepth == 4)
				{
					LogicRibbonDisplayUI component2 = uiinfo.instance.GetComponent<LogicRibbonDisplayUI>();
					if (component2 != null)
					{
						component2.SetContent(networkForCell);
					}
				}
				else if (uiinfo.bitDepth == 1)
				{
					if (networkForCell != null)
					{
						c = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
					}
					if (uiinfo.image.color != c)
					{
						uiinfo.image.color = c;
					}
				}
			}
		}

		// Token: 0x0600B37E RID: 45950 RVA: 0x003E60A4 File Offset: 0x003E42A4
		private void AddUI(ILogicUIElement ui_elem)
		{
			if (this.uiNodes.ContainsKey(ui_elem))
			{
				return;
			}
			HandleVector<int>.Handle uiHandle = this.uiInfo.Allocate(new OverlayModes.Logic.UIInfo(ui_elem, this.uiAsset));
			this.uiNodes.Add(ui_elem, new OverlayModes.Logic.EventInfo
			{
				uiHandle = uiHandle
			});
		}

		// Token: 0x0600B37F RID: 45951 RVA: 0x003E60F8 File Offset: 0x003E42F8
		private void FreeUI(ILogicUIElement item)
		{
			if (item == null)
			{
				return;
			}
			OverlayModes.Logic.EventInfo eventInfo;
			if (this.uiNodes.TryGetValue(item, out eventInfo))
			{
				this.uiInfo.GetData(eventInfo.uiHandle).Release();
				this.uiInfo.Free(eventInfo.uiHandle);
				this.uiNodes.Remove(item);
			}
		}

		// Token: 0x0600B380 RID: 45952 RVA: 0x003E6154 File Offset: 0x003E4354
		protected UniformGrid<ILogicUIElement> CreateLogicUIPartition()
		{
			UniformGrid<ILogicUIElement> uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (ILogicUIElement logicUIElement in Game.Instance.logicCircuitManager.GetVisElements())
			{
				if (logicUIElement != null)
				{
					uniformGrid.Add(logicUIElement);
				}
			}
			return uniformGrid;
		}

		// Token: 0x0600B381 RID: 45953 RVA: 0x003E61C8 File Offset: 0x003E43C8
		private bool IsBitActive(int value, int bit)
		{
			return (value & 1 << bit) > 0;
		}

		// Token: 0x0600B382 RID: 45954 RVA: 0x003E61D8 File Offset: 0x003E43D8
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x0600B383 RID: 45955 RVA: 0x003E6268 File Offset: 0x003E4468
		private void ResetRibbonSymbolTints<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					this.ResetRibbonTint(component);
				}
			}
		}

		// Token: 0x0600B384 RID: 45956 RVA: 0x003E62CC File Offset: 0x003E44CC
		private void ResetRibbonTint(KBatchedAnimController kbac)
		{
			if (kbac != null)
			{
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, Color.white);
			}
		}

		// Token: 0x04008DAD RID: 36269
		public static readonly HashedString ID = "Logic";

		// Token: 0x04008DAE RID: 36270
		public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();

		// Token: 0x04008DAF RID: 36271
		public static KAnimHashedString RIBBON_WIRE_1_SYMBOL_NAME = "wire1";

		// Token: 0x04008DB0 RID: 36272
		public static KAnimHashedString RIBBON_WIRE_2_SYMBOL_NAME = "wire2";

		// Token: 0x04008DB1 RID: 36273
		public static KAnimHashedString RIBBON_WIRE_3_SYMBOL_NAME = "wire3";

		// Token: 0x04008DB2 RID: 36274
		public static KAnimHashedString RIBBON_WIRE_4_SYMBOL_NAME = "wire4";

		// Token: 0x04008DB3 RID: 36275
		private int conduitTargetLayer;

		// Token: 0x04008DB4 RID: 36276
		private int objectTargetLayer;

		// Token: 0x04008DB5 RID: 36277
		private int cameraLayerMask;

		// Token: 0x04008DB6 RID: 36278
		private int selectionMask;

		// Token: 0x04008DB7 RID: 36279
		private UniformGrid<ILogicUIElement> ioPartition;

		// Token: 0x04008DB8 RID: 36280
		private HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04008DB9 RID: 36281
		private HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04008DBA RID: 36282
		private HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x04008DBB RID: 36283
		private HashSet<KBatchedAnimController> ribbonControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x04008DBC RID: 36284
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04008DBD RID: 36285
		private List<int> visited = new List<int>();

		// Token: 0x04008DBE RID: 36286
		private HashSet<OverlayModes.Logic.BridgeInfo> bridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x04008DBF RID: 36287
		private HashSet<OverlayModes.Logic.BridgeInfo> ribbonBridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x04008DC0 RID: 36288
		private UniformGrid<SaveLoadRoot> gameObjPartition;

		// Token: 0x04008DC1 RID: 36289
		private HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008DC2 RID: 36290
		private LogicModeUI uiAsset;

		// Token: 0x04008DC3 RID: 36291
		private Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo> uiNodes = new Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo>();

		// Token: 0x04008DC4 RID: 36292
		private KCompactedVector<OverlayModes.Logic.UIInfo> uiInfo = new KCompactedVector<OverlayModes.Logic.UIInfo>(0);

		// Token: 0x02002A5F RID: 10847
		private struct BridgeInfo
		{
			// Token: 0x0400BB33 RID: 47923
			public int cell;

			// Token: 0x0400BB34 RID: 47924
			public KBatchedAnimController controller;
		}

		// Token: 0x02002A60 RID: 10848
		private struct EventInfo
		{
			// Token: 0x0400BB35 RID: 47925
			public HandleVector<int>.Handle uiHandle;
		}

		// Token: 0x02002A61 RID: 10849
		private struct UIInfo
		{
			// Token: 0x0600D47F RID: 54399 RVA: 0x0043C8FC File Offset: 0x0043AAFC
			public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data)
			{
				this.cell = ui_elem.GetLogicUICell();
				GameObject original = null;
				Sprite sprite = null;
				this.bitDepth = 1;
				switch (ui_elem.GetLogicPortSpriteType())
				{
				case LogicPortSpriteType.Input:
					original = ui_data.prefab;
					sprite = ui_data.inputSprite;
					break;
				case LogicPortSpriteType.Output:
					original = ui_data.prefab;
					sprite = ui_data.outputSprite;
					break;
				case LogicPortSpriteType.ResetUpdate:
					original = ui_data.prefab;
					sprite = ui_data.resetSprite;
					break;
				case LogicPortSpriteType.ControlInput:
					original = ui_data.controlInputPrefab;
					break;
				case LogicPortSpriteType.RibbonInput:
					original = ui_data.ribbonInputPrefab;
					this.bitDepth = 4;
					break;
				case LogicPortSpriteType.RibbonOutput:
					original = ui_data.ribbonOutputPrefab;
					this.bitDepth = 4;
					break;
				}
				this.instance = global::Util.KInstantiate(original, Grid.CellToPosCCC(this.cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
				this.instance.SetActive(true);
				this.image = this.instance.GetComponent<Image>();
				if (this.image != null)
				{
					this.image.raycastTarget = false;
					this.image.sprite = sprite;
				}
			}

			// Token: 0x0600D480 RID: 54400 RVA: 0x0043CA0C File Offset: 0x0043AC0C
			public void Release()
			{
				global::Util.KDestroyGameObject(this.instance);
			}

			// Token: 0x0400BB36 RID: 47926
			public GameObject instance;

			// Token: 0x0400BB37 RID: 47927
			public Image image;

			// Token: 0x0400BB38 RID: 47928
			public int cell;

			// Token: 0x0400BB39 RID: 47929
			public int bitDepth;
		}
	}

	// Token: 0x02001E2D RID: 7725
	public enum BringToFrontLayerSetting
	{
		// Token: 0x04008DC6 RID: 36294
		None,
		// Token: 0x04008DC7 RID: 36295
		Constant,
		// Token: 0x04008DC8 RID: 36296
		Conditional
	}

	// Token: 0x02001E2E RID: 7726
	public class ColorHighlightCondition
	{
		// Token: 0x0600B386 RID: 45958 RVA: 0x003E6386 File Offset: 0x003E4586
		public ColorHighlightCondition(Func<KMonoBehaviour, Color> highlight_color, Func<KMonoBehaviour, bool> highlight_condition)
		{
			this.highlight_color = highlight_color;
			this.highlight_condition = highlight_condition;
		}

		// Token: 0x04008DC9 RID: 36297
		public Func<KMonoBehaviour, Color> highlight_color;

		// Token: 0x04008DCA RID: 36298
		public Func<KMonoBehaviour, bool> highlight_condition;
	}

	// Token: 0x02001E2F RID: 7727
	public class None : OverlayModes.Mode
	{
		// Token: 0x0600B387 RID: 45959 RVA: 0x003E639C File Offset: 0x003E459C
		public override HashedString ViewMode()
		{
			return OverlayModes.None.ID;
		}

		// Token: 0x0600B388 RID: 45960 RVA: 0x003E63A3 File Offset: 0x003E45A3
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x04008DCB RID: 36299
		public static readonly HashedString ID = HashedString.Invalid;
	}

	// Token: 0x02001E30 RID: 7728
	public class PathProber : OverlayModes.Mode
	{
		// Token: 0x0600B38B RID: 45963 RVA: 0x003E63BE File Offset: 0x003E45BE
		public override HashedString ViewMode()
		{
			return OverlayModes.PathProber.ID;
		}

		// Token: 0x0600B38C RID: 45964 RVA: 0x003E63C5 File Offset: 0x003E45C5
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x04008DCC RID: 36300
		public static readonly HashedString ID = "PathProber";
	}

	// Token: 0x02001E31 RID: 7729
	public class Oxygen : OverlayModes.Mode
	{
		// Token: 0x0600B38F RID: 45967 RVA: 0x003E63E5 File Offset: 0x003E45E5
		public override HashedString ViewMode()
		{
			return OverlayModes.Oxygen.ID;
		}

		// Token: 0x0600B390 RID: 45968 RVA: 0x003E63EC File Offset: 0x003E45EC
		public override string GetSoundName()
		{
			return "Oxygen";
		}

		// Token: 0x0600B391 RID: 45969 RVA: 0x003E63F4 File Offset: 0x003E45F4
		public override void Enable()
		{
			base.Enable();
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x0600B392 RID: 45970 RVA: 0x003E6433 File Offset: 0x003E4633
		public override void Disable()
		{
			base.Disable();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x04008DCD RID: 36301
		public static readonly HashedString ID = "Oxygen";
	}

	// Token: 0x02001E32 RID: 7730
	public class Light : OverlayModes.Mode
	{
		// Token: 0x0600B395 RID: 45973 RVA: 0x003E645E File Offset: 0x003E465E
		public override HashedString ViewMode()
		{
			return OverlayModes.Light.ID;
		}

		// Token: 0x0600B396 RID: 45974 RVA: 0x003E6465 File Offset: 0x003E4665
		public override string GetSoundName()
		{
			return "Lights";
		}

		// Token: 0x04008DCE RID: 36302
		public static readonly HashedString ID = "Light";
	}

	// Token: 0x02001E33 RID: 7731
	public class Priorities : OverlayModes.Mode
	{
		// Token: 0x0600B399 RID: 45977 RVA: 0x003E6485 File Offset: 0x003E4685
		public override HashedString ViewMode()
		{
			return OverlayModes.Priorities.ID;
		}

		// Token: 0x0600B39A RID: 45978 RVA: 0x003E648C File Offset: 0x003E468C
		public override string GetSoundName()
		{
			return "Priorities";
		}

		// Token: 0x04008DCF RID: 36303
		public static readonly HashedString ID = "Priorities";
	}

	// Token: 0x02001E34 RID: 7732
	public class ThermalConductivity : OverlayModes.Mode
	{
		// Token: 0x0600B39D RID: 45981 RVA: 0x003E64AC File Offset: 0x003E46AC
		public override HashedString ViewMode()
		{
			return OverlayModes.ThermalConductivity.ID;
		}

		// Token: 0x0600B39E RID: 45982 RVA: 0x003E64B3 File Offset: 0x003E46B3
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x04008DD0 RID: 36304
		public static readonly HashedString ID = "ThermalConductivity";
	}

	// Token: 0x02001E35 RID: 7733
	public class HeatFlow : OverlayModes.Mode
	{
		// Token: 0x0600B3A1 RID: 45985 RVA: 0x003E64D3 File Offset: 0x003E46D3
		public override HashedString ViewMode()
		{
			return OverlayModes.HeatFlow.ID;
		}

		// Token: 0x0600B3A2 RID: 45986 RVA: 0x003E64DA File Offset: 0x003E46DA
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x04008DD1 RID: 36305
		public static readonly HashedString ID = "HeatFlow";
	}

	// Token: 0x02001E36 RID: 7734
	public class Rooms : OverlayModes.Mode
	{
		// Token: 0x0600B3A5 RID: 45989 RVA: 0x003E64FA File Offset: 0x003E46FA
		public override HashedString ViewMode()
		{
			return OverlayModes.Rooms.ID;
		}

		// Token: 0x0600B3A6 RID: 45990 RVA: 0x003E6501 File Offset: 0x003E4701
		public override string GetSoundName()
		{
			return "Rooms";
		}

		// Token: 0x0600B3A7 RID: 45991 RVA: 0x003E6508 File Offset: 0x003E4708
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<RoomType> list2 = new List<RoomType>(Db.Get().RoomTypes.resources);
			list2.Sort((RoomType a, RoomType b) => a.sortKey.CompareTo(b.sortKey));
			foreach (RoomType roomType in list2)
			{
				string text = roomType.GetCriteriaString();
				if (roomType.effects != null && roomType.effects.Length != 0)
				{
					text = text + "\n\n" + roomType.GetRoomEffectsString();
				}
				list.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect, text, GlobalAssets.Instance.colorSet.GetColorByName(roomType.category.colorName), null, null, true));
			}
			return list;
		}

		// Token: 0x04008DD2 RID: 36306
		public static readonly HashedString ID = "Rooms";
	}

	// Token: 0x02001E37 RID: 7735
	public abstract class Mode
	{
		// Token: 0x0600B3AA RID: 45994 RVA: 0x003E6615 File Offset: 0x003E4815
		public static void Clear()
		{
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600B3AB RID: 45995
		public abstract HashedString ViewMode();

		// Token: 0x0600B3AC RID: 45996 RVA: 0x003E6621 File Offset: 0x003E4821
		public virtual void Enable()
		{
		}

		// Token: 0x0600B3AD RID: 45997 RVA: 0x003E6623 File Offset: 0x003E4823
		public virtual void Update()
		{
		}

		// Token: 0x0600B3AE RID: 45998 RVA: 0x003E6625 File Offset: 0x003E4825
		public virtual void Disable()
		{
		}

		// Token: 0x0600B3AF RID: 45999 RVA: 0x003E6627 File Offset: 0x003E4827
		public virtual List<LegendEntry> GetCustomLegendData()
		{
			return null;
		}

		// Token: 0x0600B3B0 RID: 46000 RVA: 0x003E662A File Offset: 0x003E482A
		public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return null;
		}

		// Token: 0x0600B3B1 RID: 46001 RVA: 0x003E662D File Offset: 0x003E482D
		public virtual void OnFiltersChanged()
		{
		}

		// Token: 0x0600B3B2 RID: 46002 RVA: 0x003E662F File Offset: 0x003E482F
		public virtual void DisableOverlay()
		{
		}

		// Token: 0x0600B3B3 RID: 46003
		public abstract string GetSoundName();

		// Token: 0x0600B3B4 RID: 46004 RVA: 0x003E6631 File Offset: 0x003E4831
		protected bool InFilter(string layer, Dictionary<string, ToolParameterMenu.ToggleState> filter)
		{
			return (filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On) || (filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On);
		}

		// Token: 0x0600B3B5 RID: 46005 RVA: 0x003E6664 File Offset: 0x003E4864
		public void RegisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister += this.OnSaveLoadRootRegistered;
			saveManager.onUnregister += this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600B3B6 RID: 46006 RVA: 0x003E6695 File Offset: 0x003E4895
		public void UnregisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister -= this.OnSaveLoadRootRegistered;
			saveManager.onUnregister -= this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600B3B7 RID: 46007 RVA: 0x003E66C6 File Offset: 0x003E48C6
		protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot root)
		{
		}

		// Token: 0x0600B3B8 RID: 46008 RVA: 0x003E66C8 File Offset: 0x003E48C8
		protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root)
		{
		}

		// Token: 0x0600B3B9 RID: 46009 RVA: 0x003E66CC File Offset: 0x003E48CC
		protected void ProcessExistingSaveLoadRoots()
		{
			foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in SaveLoader.Instance.saveManager.GetLists())
			{
				foreach (SaveLoadRoot root in keyValuePair.Value)
				{
					this.OnSaveLoadRootRegistered(root);
				}
			}
		}

		// Token: 0x0600B3BA RID: 46010 RVA: 0x003E6764 File Offset: 0x003E4964
		protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject
		{
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			UniformGrid<T> uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (Tag key in tags)
			{
				List<SaveLoadRoot> list = null;
				if (lists.TryGetValue(key, out list))
				{
					foreach (SaveLoadRoot saveLoadRoot in list)
					{
						T component = saveLoadRoot.GetComponent<T>();
						if (component != null)
						{
							uniformGrid.Add(component);
						}
					}
				}
			}
			return uniformGrid;
		}

		// Token: 0x0600B3BB RID: 46011 RVA: 0x003E6828 File Offset: 0x003E4A28
		protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
				}
			}
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x003E6894 File Offset: 0x003E4A94
		protected static void ResetDisplayValues(KBatchedAnimController controller)
		{
			controller.SetLayer(0);
			controller.HighlightColour = Color.clear;
			controller.TintColour = Color.white;
			controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x003E68D0 File Offset: 0x003E4AD0
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, Vector2I min, Vector2I max, Action<T> on_removed = null) where T : KMonoBehaviour
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, min, max, null, delegate(T cmp)
			{
				if (cmp != null)
				{
					KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
					if (on_removed != null)
					{
						on_removed(cmp);
					}
				}
			});
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600B3BE RID: 46014 RVA: 0x003E690C File Offset: 0x003E4B0C
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, Vector2I vis_min, Vector2I vis_max, ICollection<Tag> item_ids, Action<T> on_remove) where T : KMonoBehaviour
		{
			OverlayModes.Mode.workingTargets.Clear();
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector2I vector2I = Grid.PosToXY(t.transform.GetPosition());
					if (!(vis_min <= vector2I) || !(vector2I <= vis_max) || t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
					{
						OverlayModes.Mode.workingTargets.Add(t);
					}
					else
					{
						KPrefabID component = t.GetComponent<KPrefabID>();
						if (item_ids != null && !item_ids.Contains(component.PrefabTag) && t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
						{
							OverlayModes.Mode.workingTargets.Add(t);
						}
					}
				}
			}
			foreach (KMonoBehaviour kmonoBehaviour in OverlayModes.Mode.workingTargets)
			{
				T t2 = (T)((object)kmonoBehaviour);
				if (!(t2 == null))
				{
					if (on_remove != null)
					{
						on_remove(t2);
					}
					targets.Remove(t2);
				}
			}
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600B3BF RID: 46015 RVA: 0x003E6A80 File Offset: 0x003E4C80
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null, Func<T, bool> special_clear_condition = null) where T : IUniformGridObject
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, working_targets, vis_min, vis_max, delegate(T cmp)
			{
				if (cmp != null && on_removed != null)
				{
					on_removed(cmp);
				}
			});
			if (special_clear_condition != null)
			{
				working_targets.Clear();
				foreach (T t in targets)
				{
					if (special_clear_condition(t))
					{
						working_targets.Add(t);
					}
				}
				foreach (T t2 in working_targets)
				{
					if (t2 != null)
					{
						if (on_removed != null)
						{
							on_removed(t2);
						}
						targets.Remove(t2);
					}
				}
				working_targets.Clear();
			}
		}

		// Token: 0x0600B3C0 RID: 46016 RVA: 0x003E6B5C File Offset: 0x003E4D5C
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null) where T : IUniformGridObject
		{
			working_targets.Clear();
			foreach (T t in targets)
			{
				if (t != null)
				{
					Vector2 vector = t.PosMin();
					Vector2 vector2 = t.PosMin();
					if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || (float)vis_max.x < vector.x || (float)vis_max.y < vector.y)
					{
						working_targets.Add(t);
					}
				}
			}
			foreach (T t2 in working_targets)
			{
				if (t2 != null)
				{
					if (on_removed != null)
					{
						on_removed(t2);
					}
					targets.Remove(t2);
				}
			}
			working_targets.Clear();
		}

		// Token: 0x0600B3C1 RID: 46017 RVA: 0x003E6C60 File Offset: 0x003E4E60
		protected static float GetDefaultDepth(KMonoBehaviour cmp)
		{
			BuildingComplete component = cmp.GetComponent<BuildingComplete>();
			float layerZ;
			if (component != null)
			{
				layerZ = Grid.GetLayerZ(component.Def.SceneLayer);
			}
			else
			{
				layerZ = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			}
			return layerZ;
		}

		// Token: 0x0600B3C2 RID: 46018 RVA: 0x003E6C9C File Offset: 0x003E4E9C
		protected void UpdateHighlightTypeOverlay<T>(Vector2I min, Vector2I max, ICollection<T> targets, ICollection<Tag> item_ids, OverlayModes.ColorHighlightCondition[] highlights, OverlayModes.BringToFrontLayerSetting bringToFrontSetting, int layer) where T : KMonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector3 position = t.transform.GetPosition();
					int cell = Grid.PosToCell(position);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && min <= position && position <= max)
					{
						KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
						if (!(component == null))
						{
							int layer2 = 0;
							Color32 highlightColour = Color.clear;
							if (highlights != null)
							{
								foreach (OverlayModes.ColorHighlightCondition colorHighlightCondition in highlights)
								{
									if (colorHighlightCondition.highlight_condition(t))
									{
										highlightColour = colorHighlightCondition.highlight_color(t);
										layer2 = layer;
										break;
									}
								}
							}
							if (bringToFrontSetting != OverlayModes.BringToFrontLayerSetting.Constant)
							{
								if (bringToFrontSetting == OverlayModes.BringToFrontLayerSetting.Conditional)
								{
									component.SetLayer(layer2);
								}
							}
							else
							{
								component.SetLayer(layer);
							}
							component.HighlightColour = highlightColour;
						}
					}
				}
			}
		}

		// Token: 0x0600B3C3 RID: 46019 RVA: 0x003E6DF4 File Offset: 0x003E4FF4
		protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour
		{
			Color32 highlightColour = Color.clear;
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.HighlightColour = highlightColour;
						component.SetLayer(0);
					}
				}
			}
			targets.Clear();
		}

		// Token: 0x0600B3C4 RID: 46020 RVA: 0x003E6E78 File Offset: 0x003E5078
		protected void AddTargetIfVisible<T>(T instance, Vector2I vis_min, Vector2I vis_max, ICollection<T> targets, int layer, Action<T> on_added = null, Func<KMonoBehaviour, bool> should_add = null) where T : IUniformGridObject
		{
			if (instance.Equals(null))
			{
				return;
			}
			Vector2 vector = instance.PosMin();
			Vector2 vector2 = instance.PosMax();
			if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || vector.x > (float)vis_max.x || vector.y > (float)vis_max.y)
			{
				return;
			}
			if (targets.Contains(instance))
			{
				return;
			}
			bool flag = false;
			int num = (int)vector.y;
			while ((float)num <= vector2.y)
			{
				int num2 = (int)vector.x;
				while ((float)num2 <= vector2.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					if ((Grid.IsValidCell(num3) && Grid.Visible[num3] > 20 && (int)Grid.WorldIdx[num3] == ClusterManager.Instance.activeWorldId) || !PropertyTextures.IsFogOfWarEnabled)
					{
						flag = true;
						break;
					}
					num2++;
				}
				num++;
			}
			if (flag)
			{
				bool flag2 = true;
				KMonoBehaviour kmonoBehaviour = instance as KMonoBehaviour;
				if (kmonoBehaviour != null && should_add != null)
				{
					flag2 = should_add(kmonoBehaviour);
				}
				if (flag2)
				{
					if (kmonoBehaviour != null)
					{
						KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
						if (component != null)
						{
							component.SetLayer(layer);
						}
					}
					targets.Add(instance);
					if (on_added != null)
					{
						on_added(instance);
					}
				}
			}
		}

		// Token: 0x04008DD3 RID: 36307
		public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;

		// Token: 0x04008DD4 RID: 36308
		private static List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
	}

	// Token: 0x02001E38 RID: 7736
	public class ModeUtil
	{
		// Token: 0x0600B3C7 RID: 46023 RVA: 0x003E6FEC File Offset: 0x003E51EC
		public static float GetHighlightScale()
		{
			return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
		}
	}

	// Token: 0x02001E39 RID: 7737
	public class Power : OverlayModes.Mode
	{
		// Token: 0x0600B3C9 RID: 46025 RVA: 0x003E701A File Offset: 0x003E521A
		public override HashedString ViewMode()
		{
			return OverlayModes.Power.ID;
		}

		// Token: 0x0600B3CA RID: 46026 RVA: 0x003E7021 File Offset: 0x003E5221
		public override string GetSoundName()
		{
			return "Power";
		}

		// Token: 0x0600B3CB RID: 46027 RVA: 0x003E7028 File Offset: 0x003E5228
		public Power(Canvas powerLabelParent, LocText powerLabelPrefab, BatteryUI batteryUIPrefab, Vector3 powerLabelOffset, Vector3 batteryUIOffset, Vector3 batteryUITransformerOffset, Vector3 batteryUISmallTransformerOffset)
		{
			this.powerLabelParent = powerLabelParent;
			this.powerLabelPrefab = powerLabelPrefab;
			this.batteryUIPrefab = batteryUIPrefab;
			this.powerLabelOffset = powerLabelOffset;
			this.batteryUIOffset = batteryUIOffset;
			this.batteryUITransformerOffset = batteryUITransformerOffset;
			this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
			this.previousTilePower = new float[Grid.CellCount];
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x0600B3CC RID: 46028 RVA: 0x003E7120 File Offset: 0x003E5320
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayScreen.WireIDs);
			GridCompositor.Instance.ToggleMinor(true);
		}

		// Token: 0x0600B3CD RID: 46029 RVA: 0x003E7178 File Offset: 0x003E5378
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			this.privateTargets.Clear();
			this.queuedAdds.Clear();
			this.DisablePowerLabels();
			this.DisableBatteryUIs();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600B3CE RID: 46030 RVA: 0x003E71FC File Offset: 0x003E53FC
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayScreen.WireIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600B3CF RID: 46031 RVA: 0x003E7230 File Offset: 0x003E5430
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600B3D0 RID: 46032 RVA: 0x003E727C File Offset: 0x003E547C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			if (gameObject != null)
			{
				IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component != null)
				{
					int networkCell = component.GetNetworkCell();
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, Game.Instance.electricalConduitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			CircuitManager circuitManager = Game.Instance.circuitManager;
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					IBridgedNetworkItem component2 = saveLoadRoot.GetComponent<IBridgedNetworkItem>();
					if (component2 != null)
					{
						KAnimControllerBase component3 = (component2 as KMonoBehaviour).GetComponent<KBatchedAnimController>();
						int networkCell2 = component2.GetNetworkCell();
						UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(networkCell2);
						ushort num2 = (networkForCell != null) ? ((ushort)networkForCell.id) : ushort.MaxValue;
						float num3 = circuitManager.GetWattsUsedByCircuit(num2);
						if (num3 == -1f)
						{
							num3 = this.previousTilePower[networkCell2];
						}
						else
						{
							this.previousTilePower[networkCell2] = num3;
						}
						float num4 = circuitManager.GetMaxSafeWattageForCircuit(num2);
						num4 += POWER.FLOAT_FUDGE_FACTOR;
						float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(num2);
						Color32 color;
						if (num3 <= 0f)
						{
							color = GlobalAssets.Instance.colorSet.powerCircuitUnpowered;
						}
						else if (num3 > num4)
						{
							color = GlobalAssets.Instance.colorSet.powerCircuitOverloading;
						}
						else if (wattsNeededWhenActive > num4 && num4 > 0f && num3 / num4 >= 0.75f)
						{
							color = GlobalAssets.Instance.colorSet.powerCircuitStraining;
						}
						else
						{
							color = GlobalAssets.Instance.colorSet.powerCircuitSafe;
						}
						if (this.connectedNetworks.Count > 0 && component2.IsConnectedToNetworks(this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
						component3.TintColour = color;
					}
				}
			}
			this.queuedAdds.Clear();
			foreach (Battery battery in Components.Batteries.Items)
			{
				Vector2I vector2I3 = Grid.PosToXY(battery.transform.GetPosition());
				if (vector2I <= vector2I3 && vector2I3 <= vector2I2 && battery.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
				{
					SaveLoadRoot component4 = battery.GetComponent<SaveLoadRoot>();
					if (!this.privateTargets.Contains(component4))
					{
						this.AddBatteryUI(battery);
						this.queuedAdds.Add(component4);
					}
				}
			}
			foreach (Generator generator in Components.Generators.Items)
			{
				Vector2I vector2I4 = Grid.PosToXY(generator.transform.GetPosition());
				if (vector2I <= vector2I4 && vector2I4 <= vector2I2 && generator.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
				{
					SaveLoadRoot component5 = generator.GetComponent<SaveLoadRoot>();
					if (!this.privateTargets.Contains(component5))
					{
						this.privateTargets.Add(component5);
						if (generator.GetComponent<PowerTransformer>() == null)
						{
							this.AddPowerLabels(generator);
						}
					}
				}
			}
			foreach (EnergyConsumer energyConsumer in Components.EnergyConsumers.Items)
			{
				Vector2I vector2I5 = Grid.PosToXY(energyConsumer.transform.GetPosition());
				if (vector2I <= vector2I5 && vector2I5 <= vector2I2 && energyConsumer.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
				{
					SaveLoadRoot component6 = energyConsumer.GetComponent<SaveLoadRoot>();
					if (!this.privateTargets.Contains(component6))
					{
						this.privateTargets.Add(component6);
						this.AddPowerLabels(energyConsumer);
					}
				}
			}
			foreach (SaveLoadRoot item in this.queuedAdds)
			{
				this.privateTargets.Add(item);
			}
			this.queuedAdds.Clear();
			this.UpdatePowerLabels();
		}

		// Token: 0x0600B3D1 RID: 46033 RVA: 0x003E784C File Offset: 0x003E5A4C
		private LocText GetFreePowerLabel()
		{
			LocText locText;
			if (this.freePowerLabelIdx < this.powerLabels.Count)
			{
				locText = this.powerLabels[this.freePowerLabelIdx];
				this.freePowerLabelIdx++;
			}
			else
			{
				locText = global::Util.KInstantiateUI<LocText>(this.powerLabelPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.powerLabels.Add(locText);
				this.freePowerLabelIdx++;
			}
			return locText;
		}

		// Token: 0x0600B3D2 RID: 46034 RVA: 0x003E78D0 File Offset: 0x003E5AD0
		private void UpdatePowerLabels()
		{
			foreach (OverlayModes.Power.UpdatePowerInfo updatePowerInfo in this.updatePowerInfo)
			{
				KMonoBehaviour item = updatePowerInfo.item;
				LocText powerLabel = updatePowerInfo.powerLabel;
				LocText unitLabel = updatePowerInfo.unitLabel;
				Generator generator = updatePowerInfo.generator;
				IEnergyConsumer consumer = updatePowerInfo.consumer;
				if (updatePowerInfo.item == null || updatePowerInfo.item.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
				{
					powerLabel.gameObject.SetActive(false);
				}
				else
				{
					powerLabel.gameObject.SetActive(true);
					if (generator != null && consumer == null)
					{
						int num;
						if (generator.GetComponent<ManualGenerator>() == null)
						{
							generator.GetComponent<Operational>();
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						else
						{
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						powerLabel.text = ((num != 0) ? ("+" + num.ToString()) : num.ToString());
						BuildingEnabledButton component = item.GetComponent<BuildingEnabledButton>();
						Color color = (component != null && !component.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerGenerator;
						powerLabel.color = color;
						unitLabel.color = color;
						BuildingCellVisualizer component2 = generator.GetComponent<BuildingCellVisualizer>();
						if (component2 != null)
						{
							Image powerOutputIcon = component2.GetPowerOutputIcon();
							if (powerOutputIcon != null)
							{
								powerOutputIcon.color = color;
							}
						}
					}
					if (consumer != null)
					{
						BuildingEnabledButton component3 = item.GetComponent<BuildingEnabledButton>();
						Color color2 = (component3 != null && !component3.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerConsumer;
						int num2 = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
						string text = num2.ToString();
						powerLabel.text = ((num2 != 0) ? ("-" + text) : text);
						powerLabel.color = color2;
						unitLabel.color = color2;
						Image powerInputIcon = item.GetComponentInChildren<BuildingCellVisualizer>().GetPowerInputIcon();
						if (powerInputIcon != null)
						{
							powerInputIcon.color = color2;
						}
					}
				}
			}
			foreach (OverlayModes.Power.UpdateBatteryInfo updateBatteryInfo in this.updateBatteryInfo)
			{
				updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
			}
		}

		// Token: 0x0600B3D3 RID: 46035 RVA: 0x003E7BAC File Offset: 0x003E5DAC
		private void AddPowerLabels(KMonoBehaviour item)
		{
			if (item.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				IEnergyConsumer componentInChildren = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
				Generator componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
				if (componentInChildren != null || componentInChildren2 != null)
				{
					float num = -10f;
					if (componentInChildren2 != null)
					{
						LocText freePowerLabel = this.GetFreePowerLabel();
						freePowerLabel.gameObject.SetActive(true);
						freePowerLabel.gameObject.name = item.gameObject.name + "power label";
						LocText component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
						component.gameObject.SetActive(true);
						freePowerLabel.enabled = true;
						component.enabled = true;
						Vector3 a = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel.rectTransform.SetPosition(a + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						if (componentInChildren != null && componentInChildren.PowerCell == componentInChildren2.PowerCell)
						{
							num -= 15f;
						}
						this.SetToolTip(freePowerLabel, UI.OVERLAYS.POWER.WATTS_GENERATED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel, component, componentInChildren2, null));
					}
					if (componentInChildren != null && componentInChildren.GetType() != typeof(Battery))
					{
						LocText freePowerLabel2 = this.GetFreePowerLabel();
						LocText component2 = freePowerLabel2.transform.GetChild(0).GetComponent<LocText>();
						freePowerLabel2.gameObject.SetActive(true);
						component2.gameObject.SetActive(true);
						freePowerLabel2.gameObject.name = item.gameObject.name + "power label";
						freePowerLabel2.enabled = true;
						component2.enabled = true;
						Vector3 a2 = Grid.CellToPos(componentInChildren.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel2.rectTransform.SetPosition(a2 + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						this.SetToolTip(freePowerLabel2, UI.OVERLAYS.POWER.WATTS_CONSUMED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel2, component2, null, componentInChildren));
					}
				}
			}
		}

		// Token: 0x0600B3D4 RID: 46036 RVA: 0x003E7DF8 File Offset: 0x003E5FF8
		private void DisablePowerLabels()
		{
			this.freePowerLabelIdx = 0;
			foreach (LocText locText in this.powerLabels)
			{
				locText.gameObject.SetActive(false);
			}
			this.updatePowerInfo.Clear();
		}

		// Token: 0x0600B3D5 RID: 46037 RVA: 0x003E7E60 File Offset: 0x003E6060
		private void AddBatteryUI(Battery bat)
		{
			BatteryUI freeBatteryUI = this.GetFreeBatteryUI();
			freeBatteryUI.SetContent(bat);
			Vector3 b = Grid.CellToPos(bat.PowerCell, 0.5f, 0f, 0f);
			bool flag = bat.powerTransformer != null;
			float num = 1f;
			Rotatable component = bat.GetComponent<Rotatable>();
			if (component != null && component.GetVisualizerFlipX())
			{
				num = -1f;
			}
			Vector3 b2 = this.batteryUIOffset;
			if (flag)
			{
				b2 = ((bat.GetComponent<Building>().Def.WidthInCells == 2) ? this.batteryUISmallTransformerOffset : this.batteryUITransformerOffset);
			}
			b2.x *= num;
			freeBatteryUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b + b2);
			this.updateBatteryInfo.Add(new OverlayModes.Power.UpdateBatteryInfo(bat, freeBatteryUI));
		}

		// Token: 0x0600B3D6 RID: 46038 RVA: 0x003E7F30 File Offset: 0x003E6130
		private void SetToolTip(LocText label, string text)
		{
			ToolTip component = label.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = text;
			}
		}

		// Token: 0x0600B3D7 RID: 46039 RVA: 0x003E7F54 File Offset: 0x003E6154
		private void DisableBatteryUIs()
		{
			this.freeBatteryUIIdx = 0;
			foreach (BatteryUI batteryUI in this.batteryUIList)
			{
				batteryUI.gameObject.SetActive(false);
			}
			this.updateBatteryInfo.Clear();
		}

		// Token: 0x0600B3D8 RID: 46040 RVA: 0x003E7FBC File Offset: 0x003E61BC
		private BatteryUI GetFreeBatteryUI()
		{
			BatteryUI batteryUI;
			if (this.freeBatteryUIIdx < this.batteryUIList.Count)
			{
				batteryUI = this.batteryUIList[this.freeBatteryUIIdx];
				batteryUI.gameObject.SetActive(true);
				this.freeBatteryUIIdx++;
			}
			else
			{
				batteryUI = global::Util.KInstantiateUI<BatteryUI>(this.batteryUIPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.batteryUIList.Add(batteryUI);
				this.freeBatteryUIIdx++;
			}
			return batteryUI;
		}

		// Token: 0x0600B3D9 RID: 46041 RVA: 0x003E804C File Offset: 0x003E624C
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x04008DD5 RID: 36309
		public static readonly HashedString ID = "Power";

		// Token: 0x04008DD6 RID: 36310
		private int targetLayer;

		// Token: 0x04008DD7 RID: 36311
		private int cameraLayerMask;

		// Token: 0x04008DD8 RID: 36312
		private int selectionMask;

		// Token: 0x04008DD9 RID: 36313
		private List<OverlayModes.Power.UpdatePowerInfo> updatePowerInfo = new List<OverlayModes.Power.UpdatePowerInfo>();

		// Token: 0x04008DDA RID: 36314
		private List<OverlayModes.Power.UpdateBatteryInfo> updateBatteryInfo = new List<OverlayModes.Power.UpdateBatteryInfo>();

		// Token: 0x04008DDB RID: 36315
		private float[] previousTilePower;

		// Token: 0x04008DDC RID: 36316
		private Canvas powerLabelParent;

		// Token: 0x04008DDD RID: 36317
		private LocText powerLabelPrefab;

		// Token: 0x04008DDE RID: 36318
		private Vector3 powerLabelOffset;

		// Token: 0x04008DDF RID: 36319
		private BatteryUI batteryUIPrefab;

		// Token: 0x04008DE0 RID: 36320
		private Vector3 batteryUIOffset;

		// Token: 0x04008DE1 RID: 36321
		private Vector3 batteryUITransformerOffset;

		// Token: 0x04008DE2 RID: 36322
		private Vector3 batteryUISmallTransformerOffset;

		// Token: 0x04008DE3 RID: 36323
		private int freePowerLabelIdx;

		// Token: 0x04008DE4 RID: 36324
		private int freeBatteryUIIdx;

		// Token: 0x04008DE5 RID: 36325
		private List<LocText> powerLabels = new List<LocText>();

		// Token: 0x04008DE6 RID: 36326
		private List<BatteryUI> batteryUIList = new List<BatteryUI>();

		// Token: 0x04008DE7 RID: 36327
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04008DE8 RID: 36328
		private List<SaveLoadRoot> queuedAdds = new List<SaveLoadRoot>();

		// Token: 0x04008DE9 RID: 36329
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008DEA RID: 36330
		private HashSet<SaveLoadRoot> privateTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008DEB RID: 36331
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04008DEC RID: 36332
		private List<int> visited = new List<int>();

		// Token: 0x02002A69 RID: 10857
		private struct UpdatePowerInfo
		{
			// Token: 0x0600D493 RID: 54419 RVA: 0x0043CE60 File Offset: 0x0043B060
			public UpdatePowerInfo(KMonoBehaviour item, LocText power_label, LocText unit_label, Generator g, IEnergyConsumer c)
			{
				this.item = item;
				this.powerLabel = power_label;
				this.unitLabel = unit_label;
				this.generator = g;
				this.consumer = c;
			}

			// Token: 0x0400BB49 RID: 47945
			public KMonoBehaviour item;

			// Token: 0x0400BB4A RID: 47946
			public LocText powerLabel;

			// Token: 0x0400BB4B RID: 47947
			public LocText unitLabel;

			// Token: 0x0400BB4C RID: 47948
			public Generator generator;

			// Token: 0x0400BB4D RID: 47949
			public IEnergyConsumer consumer;
		}

		// Token: 0x02002A6A RID: 10858
		private struct UpdateBatteryInfo
		{
			// Token: 0x0600D494 RID: 54420 RVA: 0x0043CE87 File Offset: 0x0043B087
			public UpdateBatteryInfo(Battery battery, BatteryUI ui)
			{
				this.battery = battery;
				this.ui = ui;
			}

			// Token: 0x0400BB4E RID: 47950
			public Battery battery;

			// Token: 0x0400BB4F RID: 47951
			public BatteryUI ui;
		}
	}

	// Token: 0x02001E3A RID: 7738
	public class Radiation : OverlayModes.Mode
	{
		// Token: 0x0600B3DB RID: 46043 RVA: 0x003E80EA File Offset: 0x003E62EA
		public override HashedString ViewMode()
		{
			return OverlayModes.Radiation.ID;
		}

		// Token: 0x0600B3DC RID: 46044 RVA: 0x003E80F1 File Offset: 0x003E62F1
		public override string GetSoundName()
		{
			return "Radiation";
		}

		// Token: 0x0600B3DD RID: 46045 RVA: 0x003E80F8 File Offset: 0x003E62F8
		public override void Enable()
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterRadiationOn);
		}

		// Token: 0x0600B3DE RID: 46046 RVA: 0x003E810F File Offset: 0x003E630F
		public override void Disable()
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterRadiationOn, STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x04008DED RID: 36333
		public static readonly HashedString ID = "Radiation";
	}

	// Token: 0x02001E3B RID: 7739
	public class SolidConveyor : OverlayModes.Mode
	{
		// Token: 0x0600B3E1 RID: 46049 RVA: 0x003E8140 File Offset: 0x003E6340
		public override HashedString ViewMode()
		{
			return OverlayModes.SolidConveyor.ID;
		}

		// Token: 0x0600B3E2 RID: 46050 RVA: 0x003E8147 File Offset: 0x003E6347
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x0600B3E3 RID: 46051 RVA: 0x003E8150 File Offset: 0x003E6350
		public SolidConveyor()
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x0600B3E4 RID: 46052 RVA: 0x003E81E8 File Offset: 0x003E63E8
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600B3E5 RID: 46053 RVA: 0x003E8244 File Offset: 0x003E6444
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600B3E6 RID: 46054 RVA: 0x003E8278 File Offset: 0x003E6478
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600B3E7 RID: 46055 RVA: 0x003E82C4 File Offset: 0x003E64C4
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600B3E8 RID: 46056 RVA: 0x003E832C File Offset: 0x003E652C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				SolidConduit component = gameObject.GetComponent<SolidConduit>();
				if (component != null)
				{
					int cell = Grid.PosToCell(component);
					UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem = Game.Instance.solidConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(cell, solidConduitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					Color32 color = this.tint_color;
					SolidConduit component2 = saveLoadRoot.GetComponent<SolidConduit>();
					if (component2 != null)
					{
						if (this.connectedNetworks.Count > 0 && this.IsConnectedToNetworks(component2, this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
						saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = color;
					}
				}
			}
		}

		// Token: 0x0600B3E9 RID: 46057 RVA: 0x003E8550 File Offset: 0x003E6750
		public bool IsConnectedToNetworks(SolidConduit conduit, ICollection<UtilityNetwork> networks)
		{
			UtilityNetwork network = conduit.GetNetwork();
			return networks.Contains(network);
		}

		// Token: 0x0600B3EA RID: 46058 RVA: 0x003E856C File Offset: 0x003E676C
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						GameObject gameObject = networkItem.GameObject;
						if (gameObject != null)
						{
							IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
							if (component != null)
							{
								component.AddNetworks(networks);
							}
						}
					}
				}
			}
		}

		// Token: 0x04008DEE RID: 36334
		public static readonly HashedString ID = "SolidConveyor";

		// Token: 0x04008DEF RID: 36335
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04008DF0 RID: 36336
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008DF1 RID: 36337
		private ICollection<Tag> targetIDs = OverlayScreen.SolidConveyorIDs;

		// Token: 0x04008DF2 RID: 36338
		private Color32 tint_color = new Color32(201, 201, 201, 0);

		// Token: 0x04008DF3 RID: 36339
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04008DF4 RID: 36340
		private List<int> visited = new List<int>();

		// Token: 0x04008DF5 RID: 36341
		private int targetLayer;

		// Token: 0x04008DF6 RID: 36342
		private int cameraLayerMask;

		// Token: 0x04008DF7 RID: 36343
		private int selectionMask;
	}

	// Token: 0x02001E3C RID: 7740
	public class Sound : OverlayModes.Mode
	{
		// Token: 0x0600B3EC RID: 46060 RVA: 0x003E8646 File Offset: 0x003E6846
		public override HashedString ViewMode()
		{
			return OverlayModes.Sound.ID;
		}

		// Token: 0x0600B3ED RID: 46061 RVA: 0x003E864D File Offset: 0x003E684D
		public override string GetSoundName()
		{
			return "Sound";
		}

		// Token: 0x0600B3EE RID: 46062 RVA: 0x003E8654 File Offset: 0x003E6854
		public Sound()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour np)
			{
				Color black = Color.black;
				Color black2 = Color.black;
				float t = 0.8f;
				if (np != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					if ((np as NoisePolluter).GetNoiseForCell(cell) < 36f)
					{
						t = 1f;
						black2 = new Color(0.4f, 0.4f, 0.4f);
					}
				}
				return Color.Lerp(black, black2, t);
			}, delegate(KMonoBehaviour np)
			{
				List<GameObject> highlightedObjects = SelectToolHoverTextCard.highlightedObjects;
				bool result = false;
				for (int i = 0; i < highlightedObjects.Count; i++)
				{
					if (highlightedObjects[i] != null && highlightedObjects[i] == np.gameObject)
					{
						result = true;
						break;
					}
				}
				return result;
			});
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
		}

		// Token: 0x0600B3EF RID: 46063 RVA: 0x003E8714 File Offset: 0x003E6914
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			this.partition = OverlayModes.Mode.PopulatePartition<NoisePolluter>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600B3F0 RID: 46064 RVA: 0x003E8764 File Offset: 0x003E6964
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<NoisePolluter>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				NoisePolluter instance = (NoisePolluter)obj;
				base.AddTargetIfVisible<NoisePolluter>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<NoisePolluter>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600B3F1 RID: 46065 RVA: 0x003E8834 File Offset: 0x003E6A34
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				NoisePolluter component = item.GetComponent<NoisePolluter>();
				this.partition.Add(component);
			}
		}

		// Token: 0x0600B3F2 RID: 46066 RVA: 0x003E8870 File Offset: 0x003E6A70
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			NoisePolluter component = item.GetComponent<NoisePolluter>();
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x0600B3F3 RID: 46067 RVA: 0x003E88C4 File Offset: 0x003E6AC4
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<NoisePolluter>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04008DF8 RID: 36344
		public static readonly HashedString ID = "Sound";

		// Token: 0x04008DF9 RID: 36345
		private UniformGrid<NoisePolluter> partition;

		// Token: 0x04008DFA RID: 36346
		private HashSet<NoisePolluter> layerTargets = new HashSet<NoisePolluter>();

		// Token: 0x04008DFB RID: 36347
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04008DFC RID: 36348
		private int targetLayer;

		// Token: 0x04008DFD RID: 36349
		private int cameraLayerMask;

		// Token: 0x04008DFE RID: 36350
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001E3D RID: 7741
	public class Suit : OverlayModes.Mode
	{
		// Token: 0x0600B3F5 RID: 46069 RVA: 0x003E8922 File Offset: 0x003E6B22
		public override HashedString ViewMode()
		{
			return OverlayModes.Suit.ID;
		}

		// Token: 0x0600B3F6 RID: 46070 RVA: 0x003E8929 File Offset: 0x003E6B29
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x0600B3F7 RID: 46071 RVA: 0x003E8930 File Offset: 0x003E6B30
		public Suit(Canvas ui_parent, GameObject overlay_prefab)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = OverlayScreen.SuitIDs;
			this.uiParent = ui_parent;
			this.overlayPrefab = overlay_prefab;
		}

		// Token: 0x0600B3F8 RID: 46072 RVA: 0x003E89B0 File Offset: 0x003E6BB0
		public override void Enable()
		{
			this.partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			base.ProcessExistingSaveLoadRoots();
			base.RegisterSaveLoadListeners();
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x003E8A18 File Offset: 0x003E6C18
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			this.partition.Clear();
			this.partition = null;
			this.layerTargets.Clear();
			for (int i = 0; i < this.uiList.Count; i++)
			{
				this.uiList[i].SetActive(false);
			}
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x003E8AB0 File Offset: 0x003E6CB0
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x003E8AE4 File Offset: 0x003E6CE4
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600B3FC RID: 46076 RVA: 0x003E8B30 File Offset: 0x003E6D30
		private GameObject GetFreeUI()
		{
			GameObject gameObject;
			if (this.freeUiIdx >= this.uiList.Count)
			{
				gameObject = global::Util.KInstantiateUI(this.overlayPrefab, this.uiParent.transform.gameObject, false);
				this.uiList.Add(gameObject);
			}
			else
			{
				List<GameObject> list = this.uiList;
				int num = this.freeUiIdx;
				this.freeUiIdx = num + 1;
				gameObject = list[num];
			}
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x0600B3FD RID: 46077 RVA: 0x003E8BAC File Offset: 0x003E6DAC
		public override void Update()
		{
			this.freeUiIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = Color.white;
					bool flag = false;
					if (saveLoadRoot.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
					{
						flag = true;
					}
					else
					{
						SuitLocker component = saveLoadRoot.GetComponent<SuitLocker>();
						if (component != null)
						{
							flag = (component.GetStoredOutfit() != null);
						}
					}
					if (flag)
					{
						this.GetFreeUI().GetComponent<RectTransform>().SetPosition(saveLoadRoot.transform.GetPosition());
					}
				}
			}
			for (int i = this.freeUiIdx; i < this.uiList.Count; i++)
			{
				if (this.uiList[i].activeSelf)
				{
					this.uiList[i].SetActive(false);
				}
			}
		}

		// Token: 0x04008DFF RID: 36351
		public static readonly HashedString ID = "Suit";

		// Token: 0x04008E00 RID: 36352
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04008E01 RID: 36353
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008E02 RID: 36354
		private ICollection<Tag> targetIDs;

		// Token: 0x04008E03 RID: 36355
		private List<GameObject> uiList = new List<GameObject>();

		// Token: 0x04008E04 RID: 36356
		private int freeUiIdx;

		// Token: 0x04008E05 RID: 36357
		private int targetLayer;

		// Token: 0x04008E06 RID: 36358
		private int cameraLayerMask;

		// Token: 0x04008E07 RID: 36359
		private int selectionMask;

		// Token: 0x04008E08 RID: 36360
		private Canvas uiParent;

		// Token: 0x04008E09 RID: 36361
		private GameObject overlayPrefab;
	}

	// Token: 0x02001E3E RID: 7742
	public class Temperature : OverlayModes.Mode
	{
		// Token: 0x0600B3FF RID: 46079 RVA: 0x003E8D79 File Offset: 0x003E6F79
		public override HashedString ViewMode()
		{
			return OverlayModes.Temperature.ID;
		}

		// Token: 0x0600B400 RID: 46080 RVA: 0x003E8D80 File Offset: 0x003E6F80
		public override string GetSoundName()
		{
			return "Temperature";
		}

		// Token: 0x0600B401 RID: 46081 RVA: 0x003E8D88 File Offset: 0x003E6F88
		public Temperature()
		{
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x0600B402 RID: 46082 RVA: 0x003E941F File Offset: 0x003E761F
		public override void Update()
		{
			base.Update();
			if (this.previousUserSetting != SimDebugView.Instance.user_temperatureThresholds)
			{
				this.RefreshLegendValues();
				this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			}
		}

		// Token: 0x0600B403 RID: 46083 RVA: 0x003E9454 File Offset: 0x003E7654
		public override void Enable()
		{
			base.Enable();
			this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			this.RefreshLegendValues();
			CameraController.Instance.EnableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects.TemperatureOverlay);
		}

		// Token: 0x0600B404 RID: 46084 RVA: 0x003E9480 File Offset: 0x003E7680
		public void RefreshLegendValues()
		{
			int num = SimDebugView.Instance.temperatureThresholds.Length - 1;
			for (int i = 0; i < num; i++)
			{
				this.temperatureLegend[i].colour = GlobalAssets.Instance.colorSet.GetColorByName(SimDebugView.Instance.temperatureThresholds[num - i].colorName);
				this.temperatureLegend[i].desc_arg = GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - i].value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			}
		}

		// Token: 0x0600B405 RID: 46085 RVA: 0x003E9515 File Offset: 0x003E7715
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.HEATFLOW,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.STATECHANGE,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600B406 RID: 46086 RVA: 0x003E954C File Offset: 0x003E774C
		public override List<LegendEntry> GetCustomLegendData()
		{
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				return this.temperatureLegend;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				return this.expandedTemperatureLegend;
			case Game.TemperatureOverlayModes.HeatFlow:
				return this.heatFlowLegend;
			case Game.TemperatureOverlayModes.StateChange:
				return this.stateChangeLegend;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				return new List<LegendEntry>();
			default:
				return this.temperatureLegend;
			}
		}

		// Token: 0x0600B407 RID: 46087 RVA: 0x003E95A8 File Offset: 0x003E77A8
		public override void OnFiltersChanged()
		{
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.RelativeTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;
			}
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.HeatFlow:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.StateChange:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600B408 RID: 46088 RVA: 0x003E96E3 File Offset: 0x003E78E3
		public override void Disable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			CameraController.Instance.ToggleColouredOverlayView(false);
			CameraController.Instance.DisableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects.TemperatureOverlay);
			base.Disable();
		}

		// Token: 0x04008E0A RID: 36362
		public static readonly HashedString ID = "Temperature";

		// Token: 0x04008E0B RID: 36363
		private Vector2 previousUserSetting;

		// Token: 0x04008E0C RID: 36364
		public List<LegendEntry> temperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04008E0D RID: 36365
		public List<LegendEntry> heatFlowLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0.9098039f, 0.25882354f, 0.14901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0.30980393f, 0.30980393f, 0.30980393f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0.2509804f, 0.6313726f, 0.90588236f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04008E0E RID: 36366
		public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04008E0F RID: 36367
		public List<LegendEntry> stateChangeLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.STATECHANGE.HIGHPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.STABLE, UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.LOWPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};
	}

	// Token: 0x02001E3F RID: 7743
	public class TileMode : OverlayModes.Mode
	{
		// Token: 0x0600B40A RID: 46090 RVA: 0x003E971D File Offset: 0x003E791D
		public override HashedString ViewMode()
		{
			return OverlayModes.TileMode.ID;
		}

		// Token: 0x0600B40B RID: 46091 RVA: 0x003E9724 File Offset: 0x003E7924
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x0600B40C RID: 46092 RVA: 0x003E972C File Offset: 0x003E792C
		public TileMode()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour primary_element)
			{
				Color result = Color.black;
				if (primary_element != null)
				{
					result = (primary_element as PrimaryElement).Element.substance.uiColour;
				}
				return result;
			}, (KMonoBehaviour primary_element) => primary_element.gameObject.GetComponent<KBatchedAnimController>().IsVisible());
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x0600B40D RID: 46093 RVA: 0x003E97E4 File Offset: 0x003E79E4
		public override void Enable()
		{
			base.Enable();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<PrimaryElement>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			Camera.main.cullingMask |= this.cameraLayerMask;
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x0600B40E RID: 46094 RVA: 0x003E984C File Offset: 0x003E7A4C
		private static global::Util.IterationInstruction updateVisitor(object obj, ref ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I> context)
		{
			PrimaryElement component = Unsafe.As<Pickupable>(obj).gameObject.GetComponent<PrimaryElement>();
			if (component != null)
			{
				context.Item1.TryAddObject(component, context.Item2, context.Item3);
			}
			return global::Util.IterationInstruction.Continue;
		}

		// Token: 0x0600B40F RID: 46095 RVA: 0x003E988C File Offset: 0x003E7A8C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I> valueTuple = new ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I>(this, vector2I, vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<PrimaryElement>(this.layerTargets, vector2I, vector2I2, null);
			int height = vector2I2.y - vector2I.y;
			int width = vector2I2.x - vector2I.x;
			Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
			GameScenePartitioner.Instance.VisitEntries<ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I>>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.pickupablesLayer, new GameScenePartitioner.VisitorRef<ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I>>(OverlayModes.TileMode.updateVisitor), ref valueTuple);
			GameScenePartitioner.Instance.VisitEntries<ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I>>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.completeBuildings, new GameScenePartitioner.VisitorRef<ValueTuple<OverlayModes.TileMode, Vector2I, Vector2I>>(OverlayModes.TileMode.updateVisitor), ref valueTuple);
			base.UpdateHighlightTypeOverlay<PrimaryElement>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600B410 RID: 46096 RVA: 0x003E998C File Offset: 0x003E7B8C
		private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max)
		{
			Element element = pe.Element;
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					base.AddTargetIfVisible<PrimaryElement>(pe, min, max, this.layerTargets, this.targetLayer, null, null);
					break;
				}
			}
		}

		// Token: 0x0600B411 RID: 46097 RVA: 0x003E9A08 File Offset: 0x003E7C08
		public override void Disable()
		{
			base.Disable();
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x0600B412 RID: 46098 RVA: 0x003E9A54 File Offset: 0x003E7C54
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.METAL,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.BUILDABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FILTER,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.ORGANICS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FARMABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GAS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUID,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.MISC,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600B413 RID: 46099 RVA: 0x003E9AEC File Offset: 0x003E7CEC
		public override void OnFiltersChanged()
		{
			Game.Instance.tileOverlayFilters.Clear();
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.METAL, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
				Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Filter);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
				Game.Instance.tileOverlayFilters.Add(GameTags.Sublimating);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Organics);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.GAS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.MISC, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Other);
			}
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			this.layerTargets.Clear();
			Game.Instance.ForceOverlayUpdate(false);
		}

		// Token: 0x04008E10 RID: 36368
		public static readonly HashedString ID = "TileMode";

		// Token: 0x04008E11 RID: 36369
		private HashSet<PrimaryElement> layerTargets = new HashSet<PrimaryElement>();

		// Token: 0x04008E12 RID: 36370
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04008E13 RID: 36371
		private int targetLayer;

		// Token: 0x04008E14 RID: 36372
		private int cameraLayerMask;

		// Token: 0x04008E15 RID: 36373
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}
}
