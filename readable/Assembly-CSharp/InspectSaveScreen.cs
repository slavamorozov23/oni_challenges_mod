using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5F RID: 3167
public class InspectSaveScreen : KModalScreen
{
	// Token: 0x06006033 RID: 24627 RVA: 0x002346E1 File Offset: 0x002328E1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += this.CloseScreen;
		this.deleteSaveBtn.onClick += this.DeleteSave;
	}

	// Token: 0x06006034 RID: 24628 RVA: 0x00234717 File Offset: 0x00232917
	private void CloseScreen()
	{
		LoadScreen.Instance.Show(true);
		this.Show(false);
	}

	// Token: 0x06006035 RID: 24629 RVA: 0x0023472B File Offset: 0x0023292B
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			this.buttonPool.ClearAll();
			this.buttonFileMap.Clear();
		}
	}

	// Token: 0x06006036 RID: 24630 RVA: 0x00234750 File Offset: 0x00232950
	public void SetTarget(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogError("The directory path provided is empty.");
			this.Show(false);
			return;
		}
		if (!Directory.Exists(path))
		{
			global::Debug.LogError("The directory provided does not exist.");
			this.Show(false);
			return;
		}
		if (this.buttonPool == null)
		{
			this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
		}
		this.currentPath = path;
		List<string> list = (from filename in Directory.GetFiles(path)
		where Path.GetExtension(filename).ToLower() == ".sav"
		orderby File.GetLastWriteTime(filename) descending
		select filename).ToList<string>();
		string text = list[0];
		if (File.Exists(text))
		{
			this.mainSaveBtn.gameObject.SetActive(true);
			this.AddNewSave(this.mainSaveBtn, text);
		}
		else
		{
			this.mainSaveBtn.gameObject.SetActive(false);
		}
		if (list.Count > 1)
		{
			for (int i = 1; i < list.Count; i++)
			{
				this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[i]);
			}
		}
		this.Show(true);
	}

	// Token: 0x06006037 RID: 24631 RVA: 0x00234888 File Offset: 0x00232A88
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006038 RID: 24632 RVA: 0x00234918 File Offset: 0x00232B18
	private void DeleteSave()
	{
		if (string.IsNullOrEmpty(this.currentPath))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, delegate
		{
			string[] files = Directory.GetFiles(this.currentPath);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			Directory.Delete(this.currentPath);
			this.CloseScreen();
		});
	}

	// Token: 0x06006039 RID: 24633 RVA: 0x0023494E File Offset: 0x00232B4E
	private void AddNewSave(KButton btn, string file)
	{
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x00234950 File Offset: 0x00232B50
	private void ButtonClicked(KButton btn)
	{
		LoadingOverlay.Load(delegate
		{
			this.Load(this.buttonFileMap[btn]);
		});
	}

	// Token: 0x0600603B RID: 24635 RVA: 0x00234975 File Offset: 0x00232B75
	private void Load(string filename)
	{
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		SaveLoader.LoadScene();
		this.Deactivate();
	}

	// Token: 0x0600603C RID: 24636 RVA: 0x0023499A File Offset: 0x00232B9A
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.CloseScreen();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0400404B RID: 16459
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400404C RID: 16460
	[SerializeField]
	private KButton mainSaveBtn;

	// Token: 0x0400404D RID: 16461
	[SerializeField]
	private KButton backupBtnPrefab;

	// Token: 0x0400404E RID: 16462
	[SerializeField]
	private KButton deleteSaveBtn;

	// Token: 0x0400404F RID: 16463
	[SerializeField]
	private GameObject buttonGroup;

	// Token: 0x04004050 RID: 16464
	private UIPool<KButton> buttonPool;

	// Token: 0x04004051 RID: 16465
	private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();

	// Token: 0x04004052 RID: 16466
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04004053 RID: 16467
	private string currentPath = "";
}
