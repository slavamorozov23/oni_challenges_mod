using System;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000986 RID: 2438
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x060045E6 RID: 17894 RVA: 0x001940E4 File Offset: 0x001922E4
	public bool InPlanterBox
	{
		get
		{
			return this.isInPlanterBox;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x060045E7 RID: 17895 RVA: 0x001940EC File Offset: 0x001922EC
	// (set) Token: 0x060045E8 RID: 17896 RVA: 0x001940F4 File Offset: 0x001922F4
	public bool MarkedForHarvest
	{
		get
		{
			return this.isMarkedForHarvest;
		}
		set
		{
			this.isMarkedForHarvest = value;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x060045E9 RID: 17897 RVA: 0x001940FD File Offset: 0x001922FD
	public bool HarvestWhenReady
	{
		get
		{
			return this.harvestWhenReady;
		}
	}

	// Token: 0x060045EA RID: 17898 RVA: 0x00194105 File Offset: 0x00192305
	protected HarvestDesignatable()
	{
		this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x00194138 File Offset: 0x00192338
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x00194154 File Offset: 0x00192354
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.growingStateManager = base.GetComponent<IManageGrowingStates>();
		if (this.growingStateManager == null)
		{
			this.growingStateManager = base.gameObject.GetSMI<IManageGrowingStates>();
		}
		if (this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		Components.HarvestDesignatables.Add(this);
		base.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
		base.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
		Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
		this.area = base.GetComponent<OccupyArea>();
	}

	// Token: 0x060045ED RID: 17901 RVA: 0x00194238 File Offset: 0x00192438
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HarvestDesignatables.Remove(this);
		this.DestroyOverlayIcon();
		Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x001942BC File Offset: 0x001924BC
	private void DestroyOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			UnityEngine.Object.Destroy(this.HarvestWhenReadyOverlayIcon.gameObject);
			this.HarvestWhenReadyOverlayIcon = null;
		}
	}

	// Token: 0x060045EF RID: 17903 RVA: 0x001942E4 File Offset: 0x001924E4
	private void CreateOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			return;
		}
		if (base.GetComponent<AttackableBase>() == null)
		{
			this.HarvestWhenReadyOverlayIcon = Util.KInstantiate(Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas, null).GetComponent<RectTransform>();
			Extents extents = base.GetComponent<OccupyArea>().GetExtents();
			Vector3 position;
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Hanging))
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)(extents.y + extents.height)) + this.iconOffset;
			}
			else
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)extents.y) + this.iconOffset;
			}
			this.HarvestWhenReadyOverlayIcon.transform.SetPosition(position);
			this.RefreshOverlayIcon(null);
		}
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x001943DC File Offset: 0x001925DC
	private void OnDisableOverlay(object data)
	{
		this.DestroyOverlayIcon();
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x001943E4 File Offset: 0x001925E4
	private void OnEnableOverlay(object data)
	{
		if (((Boxed<HashedString>)data).value == OverlayModes.Harvest.ID)
		{
			this.CreateOverlayIcon();
			return;
		}
		this.DestroyOverlayIcon();
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0019440C File Offset: 0x0019260C
	private void RefreshOverlayIcon(object data = null)
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			if ((Grid.IsVisible(Grid.PosToCell(base.gameObject)) && base.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) || (CameraController.Instance != null && CameraController.Instance.FreeCameraEnabled))
			{
				if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
				{
					this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
				}
			}
			else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
			{
				this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
			}
			HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
			if (this.harvestWhenReady)
			{
				Image image = (Image)component.GetReference("On");
				image.gameObject.SetActive(true);
				image.color = GlobalAssets.Instance.colorSet.harvestEnabled;
				component.GetReference("Off").gameObject.SetActive(false);
				return;
			}
			component.GetReference("On").gameObject.SetActive(false);
			Image image2 = (Image)component.GetReference("Off");
			image2.gameObject.SetActive(true);
			image2.color = GlobalAssets.Instance.colorSet.harvestDisabled;
		}
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x00194560 File Offset: 0x00192760
	public bool CanBeHarvested()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		return !(component != null) || component.CanBeHarvested;
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x00194585 File Offset: 0x00192785
	public void SetInPlanterBox(bool state)
	{
		if (state)
		{
			if (!this.isInPlanterBox)
			{
				this.isInPlanterBox = true;
				this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
				return;
			}
		}
		else
		{
			this.isInPlanterBox = false;
		}
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x001945B0 File Offset: 0x001927B0
	public void SetHarvestWhenReady(bool state)
	{
		this.harvestWhenReady = state;
		if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		if (this.isMarkedForHarvest && !this.harvestWhenReady)
		{
			this.OnCancel(null);
			if (this.CanBeHarvested() && this.isInPlanterBox)
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		base.Trigger(-266953818, null);
		this.RefreshOverlayIcon(null);
	}

	// Token: 0x060045F6 RID: 17910 RVA: 0x00194638 File Offset: 0x00192838
	protected virtual void OnCancel(object _ = null)
	{
	}

	// Token: 0x060045F7 RID: 17911 RVA: 0x0019463C File Offset: 0x0019283C
	public virtual void MarkForHarvest()
	{
		if (!this.CanBeHarvested())
		{
			return;
		}
		this.isMarkedForHarvest = true;
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.OnMarkedForHarvest();
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x0019468B File Offset: 0x0019288B
	protected virtual void OnClickHarvestWhenReady()
	{
		this.SetHarvestWhenReady(true);
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x00194694 File Offset: 0x00192894
	protected virtual void OnClickCancelHarvestWhenReady()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.Trigger(2127324410, null);
		}
		this.SetHarvestWhenReady(false);
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x001946C4 File Offset: 0x001928C4
	public virtual void OnRefreshUserMenu(object data)
	{
		if (this.showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickCancelHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x04002F14 RID: 12052
	public Vector2 iconOffset = Vector2.zero;

	// Token: 0x04002F15 RID: 12053
	public bool defaultHarvestStateWhenPlanted = true;

	// Token: 0x04002F16 RID: 12054
	public OccupyArea area;

	// Token: 0x04002F17 RID: 12055
	[Serialize]
	protected bool isMarkedForHarvest;

	// Token: 0x04002F18 RID: 12056
	[Serialize]
	private bool isInPlanterBox;

	// Token: 0x04002F19 RID: 12057
	public bool showUserMenuButtons = true;

	// Token: 0x04002F1A RID: 12058
	public IManageGrowingStates growingStateManager;

	// Token: 0x04002F1B RID: 12059
	[Serialize]
	protected bool harvestWhenReady;

	// Token: 0x04002F1C RID: 12060
	public RectTransform HarvestWhenReadyOverlayIcon;

	// Token: 0x04002F1D RID: 12061
	private Action<object> onEnableOverlayDelegate;

	// Token: 0x04002F1E RID: 12062
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04002F1F RID: 12063
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002F20 RID: 12064
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.SetInPlanterBox(true);
	});
}
