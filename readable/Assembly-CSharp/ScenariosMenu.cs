using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DFD RID: 3581
public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x0600716D RID: 29037 RVA: 0x002B5638 File Offset: 0x002B3838
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.RebuildScreen();
	}

	// Token: 0x0600716E RID: 29038 RVA: 0x002B56BC File Offset: 0x002B38BC
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.RebuildUGCButtons();
	}

	// Token: 0x0600716F RID: 29039 RVA: 0x002B5720 File Offset: 0x002B3920
	private void RebuildUGCButtons()
	{
		ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
		bool flag = pooledList.Count > 0;
		this.noScenariosText.gameObject.SetActive(!flag);
		this.contentRoot.gameObject.SetActive(flag);
		bool flag2 = true;
		if (pooledList.Count != 0)
		{
			for (int i = 0; i < pooledList.Count; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
				gameObject.name = pooledList[i].title + "_button";
				gameObject.gameObject.SetActive(true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Title").SetText(pooledList[i].title);
				Texture2D previewImage = pooledList[i].previewImage;
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				KButton component2 = gameObject.GetComponent<KButton>();
				int index = i;
				PublishedFileId_t item = pooledList[index].fileId;
				component2.onClick += delegate()
				{
					this.ShowDetails(item);
				};
				component2.onDoubleClick += delegate()
				{
					this.LoadScenario(item);
				};
				this.buttons.Add(gameObject);
				if (item == this.activeItem)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.HideDetails();
		}
		pooledList.Recycle();
	}

	// Token: 0x06007170 RID: 29040 RVA: 0x002B58CC File Offset: 0x002B3ACC
	private void LoadScenario(PublishedFileId_t item)
	{
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		DebugUtil.LogArgs(new object[]
		{
			"LoadScenario",
			text,
			num,
			num2
		});
		System.DateTime dateTime;
		byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[]
		{
			".sav"
		}, out dateTime, false);
		string text2 = Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
		File.WriteAllBytes(text2, bytesFromZip);
		SaveLoader.SetActiveSaveFilePath(text2);
		Time.timeScale = 0f;
		SaveLoader.LoadScene();
	}

	// Token: 0x06007171 RID: 29041 RVA: 0x002B5958 File Offset: 0x002B3B58
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x06007172 RID: 29042 RVA: 0x002B5990 File Offset: 0x002B3B90
	private void ShowDetails(PublishedFileId_t item)
	{
		this.activeItem = item;
		SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
		if (mod != null)
		{
			this.scenarioTitle.text = mod.title;
			this.scenarioDetails.text = mod.description;
		}
		this.loadScenarioButton.onClick += delegate()
		{
			this.LoadScenario(item);
		};
		this.detailsRoot.gameObject.SetActive(true);
	}

	// Token: 0x06007173 RID: 29043 RVA: 0x002B5A1B File Offset: 0x002B3C1B
	private void HideDetails()
	{
		this.detailsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06007174 RID: 29044 RVA: 0x002B5A2E File Offset: 0x002B3C2E
	protected override void OnActivate()
	{
		base.OnActivate();
		SteamUGCService.Instance.AddClient(this);
		this.HideDetails();
	}

	// Token: 0x06007175 RID: 29045 RVA: 0x002B5A47 File Offset: 0x002B3C47
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SteamUGCService.Instance.RemoveClient(this);
	}

	// Token: 0x06007176 RID: 29046 RVA: 0x002B5A5A File Offset: 0x002B3C5A
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
	}

	// Token: 0x06007177 RID: 29047 RVA: 0x002B5A66 File Offset: 0x002B3C66
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		this.RebuildScreen();
	}

	// Token: 0x04004E4B RID: 20043
	public const string TAG_SCENARIO = "scenario";

	// Token: 0x04004E4C RID: 20044
	public KButton textButton;

	// Token: 0x04004E4D RID: 20045
	public KButton dismissButton;

	// Token: 0x04004E4E RID: 20046
	public KButton closeButton;

	// Token: 0x04004E4F RID: 20047
	public KButton workshopButton;

	// Token: 0x04004E50 RID: 20048
	public KButton loadScenarioButton;

	// Token: 0x04004E51 RID: 20049
	[Space]
	public GameObject ugcContainer;

	// Token: 0x04004E52 RID: 20050
	public GameObject ugcButtonPrefab;

	// Token: 0x04004E53 RID: 20051
	public LocText noScenariosText;

	// Token: 0x04004E54 RID: 20052
	public RectTransform contentRoot;

	// Token: 0x04004E55 RID: 20053
	public RectTransform detailsRoot;

	// Token: 0x04004E56 RID: 20054
	public LocText scenarioTitle;

	// Token: 0x04004E57 RID: 20055
	public LocText scenarioDetails;

	// Token: 0x04004E58 RID: 20056
	private PublishedFileId_t activeItem;

	// Token: 0x04004E59 RID: 20057
	private List<GameObject> buttons = new List<GameObject>();
}
