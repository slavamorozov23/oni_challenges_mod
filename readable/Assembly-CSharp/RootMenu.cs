using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C73 RID: 3187
public class RootMenu : KScreen
{
	// Token: 0x06006121 RID: 24865 RVA: 0x0023B536 File Offset: 0x00239736
	public static void DestroyInstance()
	{
		RootMenu.Instance = null;
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06006122 RID: 24866 RVA: 0x0023B53E File Offset: 0x0023973E
	// (set) Token: 0x06006123 RID: 24867 RVA: 0x0023B545 File Offset: 0x00239745
	public static RootMenu Instance { get; private set; }

	// Token: 0x06006124 RID: 24868 RVA: 0x0023B54D File Offset: 0x0023974D
	public override float GetSortKey()
	{
		return -1f;
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x0023B554 File Offset: 0x00239754
	protected override void OnPrefabInit()
	{
		RootMenu.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		base.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
		base.OnPrefabInit();
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x0023B5D4 File Offset: 0x002397D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, base.gameObject, true).GetComponent<DetailsScreen>();
		this.detailsScreen.gameObject.SetActive(true);
		this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
		this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
		this.detailsScreen.gameObject.SetActive(false);
		this.userMenu.gameObject.SetActive(false);
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x0023B66F File Offset: 0x0023986F
	private void OnClickCommon()
	{
		this.CloseSubMenus();
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x0023B677 File Offset: 0x00239877
	public void AddSubMenu(KScreen sub_menu)
	{
		if (sub_menu.activateOnSpawn)
		{
			sub_menu.Show(true);
		}
		this.subMenus.Add(sub_menu);
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x0023B694 File Offset: 0x00239894
	public void RemoveSubMenu(KScreen sub_menu)
	{
		this.subMenus.Remove(sub_menu);
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x0023B6A4 File Offset: 0x002398A4
	private void CloseSubMenus()
	{
		foreach (KScreen kscreen in this.subMenus)
		{
			if (kscreen != null)
			{
				if (kscreen.activateOnSpawn)
				{
					kscreen.gameObject.SetActive(false);
				}
				else
				{
					kscreen.Deactivate();
				}
			}
		}
		this.subMenus.Clear();
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x0023B720 File Offset: 0x00239920
	private void OnSelectObject(object data)
	{
		GameObject gameObject = (GameObject)data;
		bool flag = false;
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && !component.IsInitialized())
			{
				return;
			}
			flag = (component != null || CellSelectionObject.IsSelectionObject(gameObject));
		}
		if (gameObject != this.selectedGO)
		{
			if (this.selectedGO != null)
			{
				this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
			}
			this.selectedGO = null;
			this.CloseSubMenus();
			if (flag)
			{
				this.selectedGO = gameObject;
				this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
				this.AddSubMenu(this.detailsScreen);
				this.AddSubMenu(this.userMenu);
			}
			this.userMenu.SetSelected(this.selectedGO);
		}
		this.Refresh();
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x0023B809 File Offset: 0x00239A09
	public void TriggerRefresh(object obj)
	{
		this.Refresh();
	}

	// Token: 0x0600612D RID: 24877 RVA: 0x0023B811 File Offset: 0x00239A11
	public void Refresh()
	{
		if (this.selectedGO == null)
		{
			return;
		}
		this.detailsScreen.Refresh(this.selectedGO);
		this.userMenu.Refresh(this.selectedGO);
	}

	// Token: 0x0600612E RID: 24878 RVA: 0x0023B844 File Offset: 0x00239A44
	private void OnBuildingStatechanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == this.selectedGO)
		{
			this.OnSelectObject(gameObject);
		}
	}

	// Token: 0x0600612F RID: 24879 RVA: 0x0023B870 File Offset: 0x00239A70
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && e.TryConsume(global::Action.Escape) && SelectTool.Instance.enabled)
		{
			if (!this.canTogglePauseScreen)
			{
				return;
			}
			if (this.AreSubMenusOpen())
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
				this.CloseSubMenus();
				SelectTool.Instance.Select(null, false);
			}
			else if (e.IsAction(global::Action.Escape))
			{
				if (!SelectTool.Instance.enabled)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
				}
				if (PlayerController.Instance.IsUsingDefaultTool())
				{
					if (SelectTool.Instance.selected != null)
					{
						SelectTool.Instance.Select(null, false);
					}
					else
					{
						CameraController.Instance.ForcePanningState(false);
						this.TogglePauseScreen();
					}
				}
				else
				{
					Game.Instance.Trigger(288942073, null);
				}
				ToolMenu.Instance.ClearSelection();
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006130 RID: 24880 RVA: 0x0023B972 File Offset: 0x00239B72
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		if (!e.Consumed && e.TryConsume(global::Action.AlternateView) && this.tileScreenInst != null)
		{
			this.tileScreenInst.Deactivate();
			this.tileScreenInst = null;
		}
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x0023B9AD File Offset: 0x00239BAD
	public void TogglePauseScreen()
	{
		PauseScreen.Instance.Show(true);
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x0023B9BA File Offset: 0x00239BBA
	public void ExternalClose()
	{
		this.OnClickCommon();
	}

	// Token: 0x06006133 RID: 24883 RVA: 0x0023B9C2 File Offset: 0x00239BC2
	private void OnUIClear(object data)
	{
		this.CloseSubMenus();
		SelectTool.Instance.Select(null, true);
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		global::Debug.LogWarning("OnUIClear() Event system is null");
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x0023B9F9 File Offset: 0x00239BF9
	protected override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x0023BA01 File Offset: 0x00239C01
	private bool AreSubMenusOpen()
	{
		return this.subMenus.Count > 0;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x0023BA14 File Offset: 0x00239C14
	private KToggleMenu.ToggleInfo[] GetFillers()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			KPrefabID kprefabID = pickupable.KPrefabID;
			if (kprefabID.HasTag(GameTags.Filler) && hashSet.Add(kprefabID.PrefabTag))
			{
				string text = kprefabID.GetComponent<PrimaryElement>().Element.id.ToString();
				list.Add(new KToggleMenu.ToggleInfo(text, null, global::Action.NumActions));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x0023BAC8 File Offset: 0x00239CC8
	public bool IsBuildingChorePanelActive()
	{
		return this.detailsScreen != null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
	}

	// Token: 0x040040FE RID: 16638
	private DetailsScreen detailsScreen;

	// Token: 0x040040FF RID: 16639
	private UserMenuScreen userMenu;

	// Token: 0x04004100 RID: 16640
	[SerializeField]
	private GameObject detailsScreenPrefab;

	// Token: 0x04004101 RID: 16641
	[SerializeField]
	private UserMenuScreen userMenuPrefab;

	// Token: 0x04004102 RID: 16642
	private GameObject userMenuParent;

	// Token: 0x04004103 RID: 16643
	[SerializeField]
	private TileScreen tileScreen;

	// Token: 0x04004105 RID: 16645
	public KScreen buildMenu;

	// Token: 0x04004106 RID: 16646
	private List<KScreen> subMenus = new List<KScreen>();

	// Token: 0x04004107 RID: 16647
	private TileScreen tileScreenInst;

	// Token: 0x04004108 RID: 16648
	public bool canTogglePauseScreen = true;

	// Token: 0x04004109 RID: 16649
	public GameObject selectedGO;
}
