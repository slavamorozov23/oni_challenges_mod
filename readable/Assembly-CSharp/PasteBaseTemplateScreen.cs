using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000DD5 RID: 3541
public class PasteBaseTemplateScreen : KScreen
{
	// Token: 0x06006EDE RID: 28382 RVA: 0x002A00D2 File Offset: 0x0029E2D2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PasteBaseTemplateScreen.Instance = this;
		TemplateCache.Init();
		this.button_directory_up.onClick += this.UpDirectory;
		base.ConsumeMouseScroll = true;
		this.RefreshStampButtons();
	}

	// Token: 0x06006EDF RID: 28383 RVA: 0x002A0109 File Offset: 0x0029E309
	protected override void OnForcedCleanUp()
	{
		PasteBaseTemplateScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006EE0 RID: 28384 RVA: 0x002A0118 File Offset: 0x0029E318
	[ContextMenu("Refresh")]
	public void RefreshStampButtons()
	{
		this.directory_path_text.text = this.m_CurrentDirectory;
		this.button_directory_up.isInteractable = (this.m_CurrentDirectory != PasteBaseTemplateScreen.NO_DIRECTORY);
		foreach (GameObject obj in this.m_template_buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_template_buttons.Clear();
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.directory_path_text.text = "";
			using (List<string>.Enumerator enumerator2 = DlcManager.RELEASED_VERSIONS.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string dlcId = enumerator2.Current;
					if (Game.IsDlcActiveForCurrentSave(dlcId))
					{
						GameObject gameObject = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
						gameObject.GetComponent<KButton>().onClick += delegate()
						{
							this.UpdateDirectory(SettingsCache.GetScope(dlcId));
						};
						gameObject.GetComponentInChildren<LocText>().text = ((dlcId == "") ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text : SettingsCache.GetScope(dlcId));
						this.m_template_buttons.Add(gameObject);
					}
				}
			}
			return;
		}
		string path = TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory);
		if (Directory.Exists(path))
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				string path2 = directories[i];
				string directory_name = System.IO.Path.GetFileNameWithoutExtension(path2);
				GameObject gameObject2 = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
				gameObject2.GetComponent<KButton>().onClick += delegate()
				{
					this.UpdateDirectory(directory_name);
				};
				gameObject2.GetComponentInChildren<LocText>().text = directory_name;
				this.m_template_buttons.Add(gameObject2);
			}
		}
		ListPool<FileHandle, PasteBaseTemplateScreen>.PooledList pooledList = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
		FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory), "*.yaml", pooledList);
		foreach (FileHandle fileHandle in pooledList)
		{
			string file_path_no_extension = System.IO.Path.GetFileNameWithoutExtension(fileHandle.full_path);
			GameObject gameObject3 = global::Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
			gameObject3.GetComponent<KButton>().onClick += delegate()
			{
				this.OnClickPasteButton(file_path_no_extension);
			};
			gameObject3.GetComponentInChildren<LocText>().text = file_path_no_extension;
			this.m_template_buttons.Add(gameObject3);
		}
	}

	// Token: 0x06006EE1 RID: 28385 RVA: 0x002A0404 File Offset: 0x0029E604
	private void UpdateDirectory(string relativePath)
	{
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.m_CurrentDirectory = "";
		}
		this.m_CurrentDirectory = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			relativePath
		});
		this.RefreshStampButtons();
	}

	// Token: 0x06006EE2 RID: 28386 RVA: 0x002A0454 File Offset: 0x0029E654
	private void UpDirectory()
	{
		int num = this.m_CurrentDirectory.LastIndexOf("/");
		if (num > 0)
		{
			this.m_CurrentDirectory = this.m_CurrentDirectory.Substring(0, num);
		}
		else
		{
			string dlcId;
			string str;
			SettingsCache.GetDlcIdAndPath(this.m_CurrentDirectory, out dlcId, out str);
			if (str.IsNullOrWhiteSpace())
			{
				this.m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
			}
			else
			{
				this.m_CurrentDirectory = SettingsCache.GetScope(dlcId);
			}
		}
		this.RefreshStampButtons();
	}

	// Token: 0x06006EE3 RID: 28387 RVA: 0x002A04C4 File Offset: 0x0029E6C4
	private void OnClickPasteButton(string template_name)
	{
		if (template_name == null)
		{
			return;
		}
		string text = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			template_name
		});
		DebugTool.Instance.DeactivateTool(null);
		DebugBaseTemplateButton.Instance.ClearSelection();
		DebugBaseTemplateButton.Instance.nameField.text = text;
		TemplateContainer template = TemplateCache.GetTemplate(text);
		StampTool.Instance.Activate(template, true, false);
	}

	// Token: 0x04004BD4 RID: 19412
	public static PasteBaseTemplateScreen Instance;

	// Token: 0x04004BD5 RID: 19413
	public GameObject button_list_container;

	// Token: 0x04004BD6 RID: 19414
	public GameObject prefab_paste_button;

	// Token: 0x04004BD7 RID: 19415
	public GameObject prefab_directory_button;

	// Token: 0x04004BD8 RID: 19416
	public KButton button_directory_up;

	// Token: 0x04004BD9 RID: 19417
	public LocText directory_path_text;

	// Token: 0x04004BDA RID: 19418
	private List<GameObject> m_template_buttons = new List<GameObject>();

	// Token: 0x04004BDB RID: 19419
	private static readonly string NO_DIRECTORY = "NONE";

	// Token: 0x04004BDC RID: 19420
	private string m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
}
