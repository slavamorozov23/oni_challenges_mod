using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

// Token: 0x0200083B RID: 2107
[AddComponentMenu("KMonoBehaviour/scripts/CellSelectionObject")]
public class CellSelectionObject : KMonoBehaviour
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06003978 RID: 14712 RVA: 0x00140C73 File Offset: 0x0013EE73
	public int SelectedCell
	{
		get
		{
			return this.selectedCell;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06003979 RID: 14713 RVA: 0x00140C7B File Offset: 0x0013EE7B
	public float FlowRate
	{
		get
		{
			return Grid.AccumulatedFlow[this.selectedCell] / 3f;
		}
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x00140C94 File Offset: 0x0013EE94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mCollider = base.GetComponent<KBoxCollider2D>();
		this.mCollider.size = new Vector2(1.1f, 1.1f);
		this.mSelectable = base.GetComponent<KSelectable>();
		this.SelectedDisplaySprite.transform.localScale = Vector3.one * 0.390625f;
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		base.Subscribe(Game.Instance.gameObject, 493375141, new Action<object>(this.ForceRefreshUserMenu));
		this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, () => Grid.Element[this.mouseCell].IsLiquid);
		if (CellSelectionObject.selectionObjectA == null)
		{
			CellSelectionObject.selectionObjectA = this;
			return;
		}
		if (CellSelectionObject.selectionObjectB == null)
		{
			CellSelectionObject.selectionObjectB = this;
			return;
		}
		global::Debug.LogError("CellSelectionObjects not properly cleaned up.");
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x00140DB6 File Offset: 0x0013EFB6
	protected override void OnCleanUp()
	{
		CellSelectionObject.selectionObjectA = null;
		CellSelectionObject.selectionObjectB = null;
		base.OnCleanUp();
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x00140DCA File Offset: 0x0013EFCA
	public static bool IsSelectionObject(GameObject testObject)
	{
		return testObject == CellSelectionObject.selectionObjectA.gameObject || testObject == CellSelectionObject.selectionObjectB.gameObject;
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x00140DF0 File Offset: 0x0013EFF0
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x0600397E RID: 14718 RVA: 0x00140DFC File Offset: 0x0013EFFC
	private void Update()
	{
		if (!this.isAppFocused || SelectTool.Instance == null)
		{
			return;
		}
		if (Game.Instance == null || !Game.Instance.GameStarted())
		{
			return;
		}
		this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
		if (SelectTool.Instance.selected != this.mSelectable)
		{
			this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
			{
				bool flag = true;
				foreach (KeyValuePair<HashedString, Func<bool>> keyValuePair in this.overlayFilterMap)
				{
					if (keyValuePair.Value == null)
					{
						global::Debug.LogWarning("Filter value is null");
					}
					else if (OverlayScreen.Instance == null)
					{
						global::Debug.LogWarning("Overlay screen Instance is null");
					}
					else if (OverlayScreen.Instance.GetMode() == keyValuePair.Key)
					{
						flag = false;
						if (base.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
						{
							base.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
						}
						if (!keyValuePair.Value())
						{
							this.SelectedDisplaySprite.SetActive(false);
							return;
						}
						break;
					}
				}
				if (flag && base.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					base.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				Vector3 position = Grid.CellToPos(this.mouseCell, 0f, 0f, 0f) + this.offset;
				position.z = this.zDepth;
				base.transform.SetPosition(position);
				this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
			}
			if (SelectTool.Instance.hover != this.mSelectable)
			{
				this.SelectedDisplaySprite.SetActive(false);
			}
		}
		this.updateTimer += Time.deltaTime;
		if (this.updateTimer >= 0.5f)
		{
			this.updateTimer = 0f;
			if (SelectTool.Instance.selected == this.mSelectable)
			{
				this.UpdateValues();
			}
		}
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x00141084 File Offset: 0x0013F284
	public void UpdateValues()
	{
		if (!Grid.IsValidCell(this.selectedCell))
		{
			return;
		}
		this.Mass = Grid.Mass[this.selectedCell];
		this.element = Grid.Element[this.selectedCell];
		this.ElementName = this.element.name;
		this.state = this.element.state;
		this.tags = this.element.GetMaterialCategoryTag();
		this.temperature = Grid.Temperature[this.selectedCell];
		this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
		this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
		this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
		DetailsScreen.Instance.Trigger(-1514841199, null);
		this.UpdateStatusItem();
		int num = Grid.CellAbove(this.selectedCell);
		bool flag = this.element.IsLiquid && Grid.IsValidCell(num) && (Grid.Element[num].IsGas || Grid.Element[num].IsVacuum);
		if (this.element.sublimateId != (SimHashes)0 && (this.element.IsSolid || flag))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, this);
			bool flag2;
			bool flag3;
			GameUtil.IsEmissionBlocked(this.selectedCell, out flag2, out flag3);
			if (flag2)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
			}
			else if (flag3)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
			else
			{
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
		}
		if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, this);
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
		}
		bool on = CellSelectionObject.IsExposedToSpace(this.selectedCell);
		this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, on, null);
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x00141388 File Offset: 0x0013F588
	public static bool IsExposedToSpace(int cell)
	{
		return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && Grid.Objects[cell, 2] == null && Grid.Objects[cell, 9] == null && !Grid.HasDoor[cell];
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x001413E8 File Offset: 0x0013F5E8
	private void UpdateStatusItem()
	{
		if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
			return;
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
		{
			Func<Element> data = () => this.element;
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, this);
		}
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x00141578 File Offset: 0x0013F778
	public void OnObjectSelected(object o)
	{
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		this.UpdateStatusItem();
		if (SelectTool.Instance.selected == this.mSelectable)
		{
			this.selectedCell = Grid.PosToCell(base.gameObject);
			this.UpdateValues();
			Vector3 position = Grid.CellToPos(this.selectedCell, 0f, 0f, 0f) + this.offset;
			position.z = this.zDepthSelected;
			base.transform.SetPosition(position);
			this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
		}
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x00141625 File Offset: 0x0013F825
	public string MassString()
	{
		return string.Format("{0:0.00}", this.Mass);
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x0014163C File Offset: 0x0013F83C
	private void ForceRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0400231F RID: 8991
	private static CellSelectionObject selectionObjectA;

	// Token: 0x04002320 RID: 8992
	private static CellSelectionObject selectionObjectB;

	// Token: 0x04002321 RID: 8993
	[HideInInspector]
	public CellSelectionObject alternateSelectionObject;

	// Token: 0x04002322 RID: 8994
	private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.5f;

	// Token: 0x04002323 RID: 8995
	private float zDepthSelected = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection);

	// Token: 0x04002324 RID: 8996
	private KBoxCollider2D mCollider;

	// Token: 0x04002325 RID: 8997
	private KSelectable mSelectable;

	// Token: 0x04002326 RID: 8998
	private Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

	// Token: 0x04002327 RID: 8999
	public GameObject SelectedDisplaySprite;

	// Token: 0x04002328 RID: 9000
	public Sprite Sprite_Selected;

	// Token: 0x04002329 RID: 9001
	public Sprite Sprite_Hover;

	// Token: 0x0400232A RID: 9002
	public int mouseCell;

	// Token: 0x0400232B RID: 9003
	private int selectedCell;

	// Token: 0x0400232C RID: 9004
	public string ElementName;

	// Token: 0x0400232D RID: 9005
	public Element element;

	// Token: 0x0400232E RID: 9006
	public Element.State state;

	// Token: 0x0400232F RID: 9007
	public float Mass;

	// Token: 0x04002330 RID: 9008
	public float temperature;

	// Token: 0x04002331 RID: 9009
	public Tag tags;

	// Token: 0x04002332 RID: 9010
	public byte diseaseIdx;

	// Token: 0x04002333 RID: 9011
	public int diseaseCount;

	// Token: 0x04002334 RID: 9012
	private float updateTimer;

	// Token: 0x04002335 RID: 9013
	private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();

	// Token: 0x04002336 RID: 9014
	private bool isAppFocused = true;
}
