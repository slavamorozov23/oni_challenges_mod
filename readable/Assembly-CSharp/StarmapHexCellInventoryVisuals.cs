using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class StarmapHexCellInventoryVisuals : ClusterGridEntity
{
	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600117C RID: 4476 RVA: 0x000671D0 File Offset: 0x000653D0
	public override string Name
	{
		get
		{
			return UI.CLUSTERMAP.HEXCELL_INVENTORY.NAME;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x000671DC File Offset: 0x000653DC
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Debri;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x000671E0 File Offset: 0x000653E0
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_elements_kanim"),
					initialAnim = "idle_6",
					playMode = KAnim.PlayMode.Loop,
					additionalInfo = this
				}
			};
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x00067233 File Offset: 0x00065433
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06001180 RID: 4480 RVA: 0x00067236 File Offset: 0x00065436
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Hidden;
		}
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x00067239 File Offset: 0x00065439
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.inventory = base.GetComponent<StarmapHexCellInventory>();
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00067250 File Offset: 0x00065450
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.inventory.RegisterInventory(base.Location))
		{
			StarmapHexCellInventory.AllInventories[base.Location].TransferAllItemsFromExternalInventory(this.inventory);
			base.gameObject.DeleteObject();
			return;
		}
		base.Subscribe(-1697596308, new Action<object>(this.RefreshVisuals));
		base.Subscribe(-1503271301, new Action<object>(this.OnSelectObject));
		this.RefreshVisuals(null);
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x000672D7 File Offset: 0x000654D7
	private void OnSelectObject(object data)
	{
		this.ToggleSelectionGlow(((Boxed<bool>)data).value);
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x000672EA File Offset: 0x000654EA
	public void RefreshVisuals(object o)
	{
		this.RefreshVisuals();
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x000672F4 File Offset: 0x000654F4
	public void RefreshVisuals()
	{
		if (ClusterMapScreen.Instance == null || !ClusterMapScreen.Instance.isActiveAndEnabled)
		{
			return;
		}
		bool flag = this.inventory.ItemCount > 0;
		if (this.animController != null)
		{
			int num = Mathf.Min(6, this.inventory.ItemCount);
			string s = "idle_" + num.ToString();
			this.animController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
			for (int i = 0; i < this.symbolAnimControllers.Length; i++)
			{
				KBatchedAnimController kbatchedAnimController = this.symbolAnimControllers[i];
				KBatchedAnimTracker component = kbatchedAnimController.GetComponent<KBatchedAnimTracker>();
				if (i < num)
				{
					GameObject prefab = Assets.GetPrefab(this.inventory.Items[i].ID);
					Element element = ElementLoader.GetElement(prefab.PrefabID());
					KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
					string text = (element != null && element.IsLiquid) ? "idle2" : (string.IsNullOrEmpty(component2.initialAnim) ? "object" : component2.initialAnim);
					string text2;
					KAnimFile animFileFromPrefabWithTag = Def.GetAnimFileFromPrefabWithTag(prefab, text, out text2);
					kbatchedAnimController.SwapAnims(new KAnimFile[]
					{
						animFileFromPrefabWithTag
					});
					kbatchedAnimController.Play(text, KAnim.PlayMode.Once, 1f, 0f);
					if (element != null)
					{
						Color color = element.substance.colour;
						color.a = 1f;
						if (!element.IsSolid)
						{
							kbatchedAnimController.SetSymbolTint(new KAnimHashedString("substance_tinter"), color);
						}
						if (element.IsGas)
						{
							kbatchedAnimController.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), color);
						}
					}
					kbatchedAnimController.gameObject.SetActive(true);
					component.forceAlwaysVisible = true;
				}
				else
				{
					component.forceAlwaysVisible = false;
					kbatchedAnimController.gameObject.SetActive(false);
				}
			}
		}
		if (flag != this.m_selectable.IsSelectable)
		{
			this.m_selectable.IsSelectable = flag;
		}
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x000674EA File Offset: 0x000656EA
	public override void onClustermapVisualizerAnimCreated(KBatchedAnimController controller, ClusterGridEntity.AnimConfig config)
	{
		if (config.additionalInfo == this)
		{
			this.animController = controller;
			this.SetupAnimControllerAndSymbols();
			this.RefreshVisuals(null);
		}
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00067509 File Offset: 0x00065709
	private void ToggleSelectionGlow(bool glow)
	{
		this.animController.SetSymbolVisiblity(StarmapHexCellInventoryVisuals.GLOW_SYMBOL, glow);
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00067524 File Offset: 0x00065724
	private void SetupAnimControllerAndSymbols()
	{
		this.DeleteSymbolControllers();
		if (this.animController != null)
		{
			this.animController.SetSymbolVisiblity(StarmapHexCellInventoryVisuals.GLOW_SYMBOL, false);
			this.symbolAnimControllers = new KBatchedAnimController[6];
			for (int i = 0; i < this.symbolAnimControllers.Length; i++)
			{
				string symbolName = "swap0" + (i + 1).ToString();
				KBatchedAnimController kbatchedAnimController = this.CreateSymbolController(symbolName);
				this.symbolAnimControllers[i] = kbatchedAnimController;
			}
		}
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x000675A4 File Offset: 0x000657A4
	private KBatchedAnimController CreateSymbolController(string symbolName)
	{
		KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolName);
		bool flag;
		Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolName, out flag);
		bool flag2;
		Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolName, out flag2);
		Vector3 position = symbolTransform.GetColumn(3);
		Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
		kbatchedAnimController.transform.SetParent(this.animController.transform, false);
		kbatchedAnimController.transform.SetPosition(position);
		Vector3 localPosition = kbatchedAnimController.transform.localPosition;
		localPosition.z = -0.0025f;
		kbatchedAnimController.transform.localPosition = localPosition;
		kbatchedAnimController.transform.localScale = localScale;
		KBatchedAnimTracker kbatchedAnimTracker = kbatchedAnimController.gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = this.animController;
		kbatchedAnimTracker.symbol = new HashedString(symbolName);
		kbatchedAnimTracker.forceAlwaysVisible = false;
		kbatchedAnimController.gameObject.SetActive(false);
		this.animController.SetSymbolVisiblity(symbolName, false);
		return kbatchedAnimController;
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x000676A0 File Offset: 0x000658A0
	private KBatchedAnimController CreateEmptyKAnimController(string name)
	{
		GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
		gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("harvestable_elements_kanim")
		};
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = ((this.animController == null) ? 0.08f : this.animController.animScale);
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.NoLayer;
		return kbatchedAnimController;
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00067734 File Offset: 0x00065934
	private void DeleteSymbolControllers()
	{
		if (this.symbolAnimControllers != null)
		{
			for (int i = 0; i < this.symbolAnimControllers.Length; i++)
			{
				KBatchedAnimController kbatchedAnimController = this.symbolAnimControllers[i];
				if (kbatchedAnimController != null)
				{
					kbatchedAnimController.gameObject.DeleteObject();
				}
			}
			this.symbolAnimControllers = null;
		}
	}

	// Token: 0x04000B08 RID: 2824
	public const int MAX_VISUAL_ITEMS = 6;

	// Token: 0x04000B09 RID: 2825
	public const string ANIM_FILE_NAME = "harvestable_elements_kanim";

	// Token: 0x04000B0A RID: 2826
	public const string DEFAULT_ANIM_STATE_NAME = "idle_6";

	// Token: 0x04000B0B RID: 2827
	public const string ANIM_STATE_NAME_PREFIX = "idle_";

	// Token: 0x04000B0C RID: 2828
	public const string SYMBOL_SWAP_NAME_PREFIX = "swap0";

	// Token: 0x04000B0D RID: 2829
	private static readonly HashedString GLOW_SYMBOL = "glow";

	// Token: 0x04000B0E RID: 2830
	public StarmapHexCellInventory inventory;

	// Token: 0x04000B0F RID: 2831
	private KBatchedAnimController animController;

	// Token: 0x04000B10 RID: 2832
	private KBatchedAnimController[] symbolAnimControllers;
}
