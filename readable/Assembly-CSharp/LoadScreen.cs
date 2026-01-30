using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ProcGen;
using ProcGenGame;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C61 RID: 3169
public class LoadScreen : KModalScreen
{
	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06006040 RID: 24640 RVA: 0x00234A24 File Offset: 0x00232C24
	// (set) Token: 0x06006041 RID: 24641 RVA: 0x00234A2B File Offset: 0x00232C2B
	public static LoadScreen Instance { get; private set; }

	// Token: 0x06006042 RID: 24642 RVA: 0x00234A33 File Offset: 0x00232C33
	public static void DestroyInstance()
	{
		LoadScreen.Instance = null;
	}

	// Token: 0x06006043 RID: 24643 RVA: 0x00234A3C File Offset: 0x00232C3C
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(LoadScreen.Instance == null);
		LoadScreen.Instance = this;
		base.OnPrefabInit();
		this.saveButtonPrefab.gameObject.SetActive(false);
		this.colonyListPool = new UIPool<HierarchyReferences>(this.saveButtonPrefab);
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				this.Deactivate();
			};
		}
		if (this.colonyCloudButton != null)
		{
			this.colonyCloudButton.onClick += delegate()
			{
				this.ConvertAllToCloud();
			};
		}
		if (this.colonyLocalButton != null)
		{
			this.colonyLocalButton.onClick += delegate()
			{
				this.ConvertAllToLocal();
			};
		}
		if (this.colonyInfoButton != null)
		{
			this.colonyInfoButton.onClick += delegate()
			{
				this.ShowSaveInfo();
			};
		}
		if (this.loadMoreButton != null)
		{
			this.loadMoreButton.onClick += delegate()
			{
				this.displayedPageCount++;
				this.RefreshColonyList();
				this.ShowColonyList();
			};
		}
	}

	// Token: 0x06006044 RID: 24644 RVA: 0x00234B59 File Offset: 0x00232D59
	private bool IsInMenu()
	{
		return App.GetCurrentSceneName() == "frontend";
	}

	// Token: 0x06006045 RID: 24645 RVA: 0x00234B6A File Offset: 0x00232D6A
	private bool CloudSavesVisible()
	{
		return SaveLoader.GetCloudSavesAvailable() && this.IsInMenu();
	}

	// Token: 0x06006046 RID: 24646 RVA: 0x00234B7C File Offset: 0x00232D7C
	protected override void OnActivate()
	{
		base.OnActivate();
		WorldGen.LoadSettings(false);
		this.SetCloudSaveInfoActive(this.CloudSavesVisible());
		this.displayedPageCount = 1;
		this.RefreshColonyList();
		this.ShowColonyList();
		bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
		this.cloudTutorialBouncer.gameObject.SetActive(cloudSavesAvailable);
		if (cloudSavesAvailable && !this.cloudTutorialBouncer.IsBouncing())
		{
			int @int = KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			if (@int < 5)
			{
				this.cloudTutorialBouncer.Bounce();
				KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", @int + 1);
				KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			}
			else
			{
				this.cloudTutorialBouncer.gameObject.SetActive(false);
			}
		}
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.colonyInfoButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006047 RID: 24647 RVA: 0x00234C44 File Offset: 0x00232E44
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColoniesDetails(List<SaveLoader.SaveFileEntry> files)
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> dictionary = new Dictionary<string, List<LoadScreen.SaveGameFileDetails>>();
		if (files.Count <= 0)
		{
			return dictionary;
		}
		for (int i = 0; i < files.Count; i++)
		{
			if (this.IsFileValid(files[i].path))
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(files[i].path);
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				System.DateTime timeStamp = files[i].timeStamp;
				long size = 0L;
				try
				{
					size = new FileInfo(files[i].path).Length;
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Failed to get size for file: " + files[i].ToString() + "\n" + ex.ToString());
				}
				LoadScreen.SaveGameFileDetails saveGameFileDetails = new LoadScreen.SaveGameFileDetails
				{
					BaseName = second.baseName,
					FileName = files[i].path,
					FileDate = timeStamp,
					FileHeader = first,
					FileInfo = second,
					Size = size,
					UniqueID = SaveGame.GetSaveUniqueID(second)
				};
				if (!dictionary.ContainsKey(saveGameFileDetails.UniqueID))
				{
					dictionary.Add(saveGameFileDetails.UniqueID, new List<LoadScreen.SaveGameFileDetails>());
				}
				dictionary[saveGameFileDetails.UniqueID].Add(saveGameFileDetails);
			}
		}
		return dictionary;
	}

	// Token: 0x06006048 RID: 24648 RVA: 0x00234DAC File Offset: 0x00232FAC
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.both);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x06006049 RID: 24649 RVA: 0x00234DC8 File Offset: 0x00232FC8
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetLocalColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.local);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x0600604A RID: 24650 RVA: 0x00234DE4 File Offset: 0x00232FE4
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetCloudColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.cloud);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x0600604B RID: 24651 RVA: 0x00234E00 File Offset: 0x00233000
	private bool IsFileValid(string filename)
	{
		bool result = false;
		try
		{
			SaveGame.Header header;
			result = (SaveLoader.LoadHeader(filename, out header).saveMajorVersion >= 7);
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Corrupted save file: " + filename + "\n" + ex.ToString());
		}
		return result;
	}

	// Token: 0x0600604C RID: 24652 RVA: 0x00234E54 File Offset: 0x00233054
	private void CheckCloudLocalOverlap()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in this.GetColonies(false))
		{
			bool flag = false;
			List<LoadScreen.SaveGameFileDetails> list = new List<LoadScreen.SaveGameFileDetails>();
			foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails in keyValuePair.Value)
			{
				if (SaveLoader.IsSaveCloud(saveGameFileDetails.FileName))
				{
					flag = true;
				}
				else
				{
					list.Add(saveGameFileDetails);
				}
			}
			if (flag && list.Count != 0)
			{
				string baseName = list[0].BaseName;
				string path = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), baseName);
				string text = System.IO.Path.Combine(cloudSavePrefix, baseName);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				global::Debug.Log("Saves / Found overlapped cloud/local saves for colony '" + baseName + "', moving to cloud...");
				foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails2 in list)
				{
					string fileName = saveGameFileDetails2.FileName;
					string source = System.IO.Path.ChangeExtension(fileName, "png");
					string path2 = text;
					if (SaveLoader.IsSaveAuto(fileName))
					{
						string text2 = System.IO.Path.Combine(path2, "auto_save");
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						path2 = text2;
					}
					string text3 = System.IO.Path.Combine(path2, System.IO.Path.GetFileName(fileName));
					global::Tuple<bool, bool> tuple;
					if (this.FileMatch(fileName, text3, out tuple))
					{
						global::Debug.Log("Saves / file match found for `" + fileName + "`...");
						this.MigrateFile(fileName, text3, false);
						string dest = System.IO.Path.ChangeExtension(text3, "png");
						this.MigrateFile(source, dest, true);
					}
					else
					{
						global::Debug.Log("Saves / no file match found for `" + fileName + "`... move as copy");
						string nextUsableSavePath = SaveLoader.GetNextUsableSavePath(text3);
						this.MigrateFile(fileName, nextUsableSavePath, false);
						string dest2 = System.IO.Path.ChangeExtension(nextUsableSavePath, "png");
						this.MigrateFile(source, dest2, true);
					}
				}
				this.RemoveEmptyFolder(path);
			}
		}
	}

	// Token: 0x0600604D RID: 24653 RVA: 0x002350CC File Offset: 0x002332CC
	private void DeleteFileAndEmptyFolder(string file)
	{
		if (File.Exists(file))
		{
			File.Delete(file);
		}
		this.RemoveEmptyFolder(System.IO.Path.GetDirectoryName(file));
	}

	// Token: 0x0600604E RID: 24654 RVA: 0x002350E8 File Offset: 0x002332E8
	private void RemoveEmptyFolder(string path)
	{
		if (!Directory.Exists(path))
		{
			return;
		}
		if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
		{
			return;
		}
		if (Directory.EnumerateFileSystemEntries(path).Any<string>())
		{
			return;
		}
		try
		{
			Directory.Delete(path);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Failed to remove empty directory `" + path + "`...");
			global::Debug.LogWarning(obj);
		}
	}

	// Token: 0x0600604F RID: 24655 RVA: 0x0023515C File Offset: 0x0023335C
	private void RefreshColonyList()
	{
		if (this.colonyListPool != null)
		{
			this.colonyListPool.ClearAll();
		}
		this.CheckCloudLocalOverlap();
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> colonies = this.GetColonies(true);
		if (colonies.Count > 0)
		{
			int num = 0;
			foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in colonies)
			{
				if (num >= this.displayedPageCount * 20)
				{
					break;
				}
				this.AddColonyToList(keyValuePair.Value);
				num++;
			}
			this.loadMoreButton.gameObject.SetActive(colonies.Count != num);
			this.loadMoreButton.gameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06006050 RID: 24656 RVA: 0x00235220 File Offset: 0x00233420
	private string GetFileHash(string path)
	{
		string result;
		using (MD5 md = MD5.Create())
		{
			using (FileStream fileStream = File.OpenRead(path))
			{
				result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
			}
		}
		return result;
	}

	// Token: 0x06006051 RID: 24657 RVA: 0x00235290 File Offset: 0x00233490
	private bool FileMatch(string file, string other_file, out global::Tuple<bool, bool> matches)
	{
		matches = new global::Tuple<bool, bool>(false, false);
		if (!File.Exists(file))
		{
			return false;
		}
		if (!File.Exists(other_file))
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			string fileHash = this.GetFileHash(file);
			string fileHash2 = this.GetFileHash(other_file);
			FileInfo fileInfo = new FileInfo(file);
			FileInfo fileInfo2 = new FileInfo(other_file);
			flag = (fileInfo.Length == fileInfo2.Length);
			flag2 = (fileHash == fileHash2);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"FileMatch / file match failed for `",
				file,
				"` vs `",
				other_file,
				"`!"
			}));
			global::Debug.LogWarning(obj);
			return false;
		}
		matches.first = flag;
		matches.second = flag2;
		return flag && flag2;
	}

	// Token: 0x06006052 RID: 24658 RVA: 0x00235358 File Offset: 0x00233558
	private bool MigrateFile(string source, string dest, bool ignoreMissing = false)
	{
		global::Debug.Log(string.Concat(new string[]
		{
			"Migration / moving `",
			source,
			"` to `",
			dest,
			"` ..."
		}));
		if (dest == source)
		{
			global::Debug.Log(string.Concat(new string[]
			{
				"Migration / ignored `",
				source,
				"` to `",
				dest,
				"` ... same location"
			}));
			return true;
		}
		global::Tuple<bool, bool> tuple;
		if (this.FileMatch(source, dest, out tuple))
		{
			global::Debug.Log("Migration / dest and source are identical size + hash ... removing original");
			try
			{
				this.DeleteFileAndEmptyFolder(source);
			}
			catch (Exception ex)
			{
				global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
				global::Debug.LogWarning(ex);
				throw ex;
			}
			return true;
		}
		try
		{
			global::Debug.Log("Migration / copying...");
			File.Copy(source, dest, false);
		}
		catch (FileNotFoundException obj) when (ignoreMissing)
		{
			global::Debug.Log("Migration / File `" + source + "` wasn't found but we're ignoring that.");
			return true;
		}
		catch (Exception ex2)
		{
			global::Debug.LogWarning("Migration / copy failed for `" + source + "`! Leaving it alone");
			global::Debug.LogWarning(ex2);
			global::Debug.LogWarning("failed to convert colony: " + ex2.ToString());
			throw ex2;
		}
		global::Debug.Log("Migration / copy ok ...");
		global::Tuple<bool, bool> tuple2;
		if (!this.FileMatch(source, dest, out tuple2))
		{
			global::Debug.LogWarning("Migration / failed to match dest file for `" + source + "`!");
			global::Debug.LogWarning(string.Format("Migration / did hash match? {0} did size match? {1}", tuple2.second, tuple2.first));
			throw new Exception("Hash/Size didn't match for source and destination");
		}
		global::Debug.Log("Migration / hash validation ok ... removing original");
		try
		{
			this.DeleteFileAndEmptyFolder(source);
		}
		catch (Exception ex3)
		{
			global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
			global::Debug.LogWarning(ex3);
			throw ex3;
		}
		global::Debug.Log("Migration / moved ok for `" + source + "`!");
		return true;
	}

	// Token: 0x06006053 RID: 24659 RVA: 0x0023555C File Offset: 0x0023375C
	private bool MigrateSave(string dest_root, string file, bool is_auto_save, out string saveError)
	{
		saveError = null;
		global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(file);
		SaveGame.Header first = fileInfo.first;
		string path = fileInfo.second.baseName.TrimEnd(' ');
		string fileName = System.IO.Path.GetFileName(file);
		string text = System.IO.Path.Combine(dest_root, path);
		if (!Directory.Exists(text))
		{
			text = Directory.CreateDirectory(text).FullName;
		}
		string path2 = text;
		if (is_auto_save)
		{
			string text2 = System.IO.Path.Combine(text, "auto_save");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			path2 = text2;
		}
		string text3 = System.IO.Path.Combine(path2, fileName);
		string source = System.IO.Path.ChangeExtension(file, "png");
		string dest = System.IO.Path.ChangeExtension(text3, "png");
		try
		{
			this.MigrateFile(file, text3, false);
			this.MigrateFile(source, dest, true);
		}
		catch (Exception ex)
		{
			saveError = ex.Message;
			return false;
		}
		return true;
	}

	// Token: 0x06006054 RID: 24660 RVA: 0x00235638 File Offset: 0x00233838
	private ValueTuple<int, int, ulong> GetSavesSizeAndCounts(List<LoadScreen.SaveGameFileDetails> list)
	{
		ulong num = 0UL;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			LoadScreen.SaveGameFileDetails saveGameFileDetails = list[i];
			num += (ulong)saveGameFileDetails.Size;
			if (saveGameFileDetails.FileInfo.isAutoSave)
			{
				num3++;
			}
			else
			{
				num2++;
			}
		}
		return new ValueTuple<int, int, ulong>(num2, num3, num);
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x00235690 File Offset: 0x00233890
	private int CountValidSaves(string path, SearchOption searchType = SearchOption.AllDirectories)
	{
		int num = 0;
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(path, false, searchType);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			if (this.IsFileValid(saveFiles[i].path))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x002356D4 File Offset: 0x002338D4
	private ValueTuple<int, int> GetMigrationSaveCounts()
	{
		int item = this.CountValidSaves(SaveLoader.GetSavePrefixAndCreateFolder(), SearchOption.TopDirectoryOnly);
		int item2 = this.CountValidSaves(SaveLoader.GetAutoSavePrefix(), SearchOption.AllDirectories);
		return new ValueTuple<int, int>(item, item2);
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x00235700 File Offset: 0x00233900
	private ValueTuple<int, int> MigrateSaves(out string errorColony, out string errorMessage)
	{
		errorColony = null;
		errorMessage = null;
		int num = 0;
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(savePrefixAndCreateFolder, false, SearchOption.TopDirectoryOnly);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			SaveLoader.SaveFileEntry saveFileEntry = saveFiles[i];
			if (this.IsFileValid(saveFileEntry.path))
			{
				string text;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry.path, false, out text))
				{
					num++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry.path;
					errorMessage = text;
				}
			}
		}
		int num2 = 0;
		List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetAutoSavePrefix(), false, SearchOption.AllDirectories);
		for (int j = 0; j < saveFiles2.Count; j++)
		{
			SaveLoader.SaveFileEntry saveFileEntry2 = saveFiles2[j];
			if (this.IsFileValid(saveFileEntry2.path))
			{
				string text2;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry2.path, true, out text2))
				{
					num2++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry2.path;
					errorMessage = text2;
				}
			}
		}
		return new ValueTuple<int, int>(num, num2);
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x002357F0 File Offset: 0x002339F0
	public void ShowMigrationIfNecessary(bool fromMainMenu)
	{
		ValueTuple<int, int> migrationSaveCounts = this.GetMigrationSaveCounts();
		int saveCount = migrationSaveCounts.Item1;
		int autoCount = migrationSaveCounts.Item2;
		if (saveCount == 0 && autoCount == 0)
		{
			if (fromMainMenu)
			{
				this.Deactivate();
			}
			return;
		}
		base.Activate();
		this.migrationPanelRefs.gameObject.SetActive(true);
		KButton migrateButton = this.migrationPanelRefs.GetReference<RectTransform>("MigrateSaves").GetComponent<KButton>();
		KButton continueButton = this.migrationPanelRefs.GetReference<RectTransform>("Continue").GetComponent<KButton>();
		KButton moreInfoButton = this.migrationPanelRefs.GetReference<RectTransform>("MoreInfo").GetComponent<KButton>();
		KButton component = this.migrationPanelRefs.GetReference<RectTransform>("OpenSaves").GetComponent<KButton>();
		LocText statsText = this.migrationPanelRefs.GetReference<RectTransform>("CountText").GetComponent<LocText>();
		LocText infoText = this.migrationPanelRefs.GetReference<RectTransform>("InfoText").GetComponent<LocText>();
		migrateButton.gameObject.SetActive(true);
		continueButton.gameObject.SetActive(false);
		moreInfoButton.gameObject.SetActive(false);
		statsText.text = string.Format(UI.FRONTEND.LOADSCREEN.MIGRATE_COUNT, saveCount, autoCount);
		component.ClearOnClick();
		component.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefixAndCreateFolder());
		};
		migrateButton.ClearOnClick();
		migrateButton.onClick += delegate()
		{
			migrateButton.gameObject.SetActive(false);
			string text;
			string newValue;
			ValueTuple<int, int> valueTuple = this.MigrateSaves(out text, out newValue);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			bool flag = text == null;
			string format = flag ? UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT.text : UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES.Replace("{ErrorColony}", text).Replace("{ErrorMessage}", newValue);
			statsText.text = string.Format(format, new object[]
			{
				item,
				saveCount,
				item2,
				autoCount
			});
			infoText.gameObject.SetActive(false);
			if (flag)
			{
				continueButton.gameObject.SetActive(true);
			}
			else
			{
				moreInfoButton.gameObject.SetActive(true);
			}
			MainMenu.Instance.RefreshResumeButton(false);
			this.RefreshColonyList();
		};
		continueButton.ClearOnClick();
		continueButton.onClick += delegate()
		{
			this.migrationPanelRefs.gameObject.SetActive(false);
			this.cloudTutorialBouncer.Bounce();
		};
		moreInfoButton.ClearOnClick();
		Action<InfoDialogScreen> <>9__4;
		Action<InfoDialogScreen> <>9__6;
		moreInfoButton.onClick += delegate()
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				InfoDialogScreen infoDialogScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST);
				string text = UI.CONFIRMDIALOG.OK;
				Action<InfoDialogScreen> action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate(InfoDialogScreen d)
					{
						this.migrationPanelRefs.gameObject.SetActive(false);
						this.cloudTutorialBouncer.Bounce();
						d.Deactivate();
					});
				}
				infoDialogScreen.AddOption(text, action, true).Activate();
				return;
			}
			InfoDialogScreen infoDialogScreen2 = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false);
			string text2 = UI.CONFIRMDIALOG.OK;
			Action<InfoDialogScreen> action2;
			if ((action2 = <>9__6) == null)
			{
				action2 = (<>9__6 = delegate(InfoDialogScreen d)
				{
					this.migrationPanelRefs.gameObject.SetActive(false);
					this.cloudTutorialBouncer.Bounce();
					d.Deactivate();
				});
			}
			infoDialogScreen2.AddOption(text2, action2, true).Activate();
		};
	}

	// Token: 0x06006059 RID: 24665 RVA: 0x002359E9 File Offset: 0x00233BE9
	private void SetCloudSaveInfoActive(bool active)
	{
		this.colonyCloudButton.gameObject.SetActive(active);
		this.colonyLocalButton.gameObject.SetActive(active);
	}

	// Token: 0x0600605A RID: 24666 RVA: 0x00235A10 File Offset: 0x00233C10
	private bool ConvertToLocalOrCloud(string fromRoot, string destRoot, string colonyName)
	{
		string text = System.IO.Path.Combine(fromRoot, colonyName);
		string text2 = System.IO.Path.Combine(destRoot, colonyName);
		global::Debug.Log(string.Concat(new string[]
		{
			"Convert / Colony '",
			colonyName,
			"' from `",
			text,
			"` => `",
			text2,
			"`"
		}));
		try
		{
			Directory.Move(text, text2);
			return true;
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("failed to convert colony: " + ex.ToString());
			string message = UI.FRONTEND.LOADSCREEN.CONVERT_ERROR.Replace("{Colony}", colonyName).Replace("{Error}", ex.Message);
			this.ShowConvertError(message);
		}
		return false;
	}

	// Token: 0x0600605B RID: 24667 RVA: 0x00235ACC File Offset: 0x00233CCC
	private bool ConvertColonyToCloud(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony to cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(savePrefix, cloudSavePrefix, colonyName);
	}

	// Token: 0x0600605C RID: 24668 RVA: 0x00235B00 File Offset: 0x00233D00
	private bool ConvertColonyToLocal(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony from cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(cloudSavePrefix, savePrefix, colonyName);
	}

	// Token: 0x0600605D RID: 24669 RVA: 0x00235B34 File Offset: 0x00233D34
	private void DoConvertAllToLocal()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> cloudColonies = this.GetCloudColonies(false);
		if (cloudColonies.Count == 0)
		{
			return;
		}
		bool flag = true;
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in cloudColonies)
		{
			flag &= this.ConvertColonyToLocal(keyValuePair.Value[0].BaseName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_LOCAL_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(false);
	}

	// Token: 0x0600605E RID: 24670 RVA: 0x00235BF0 File Offset: 0x00233DF0
	private void DoConvertAllToCloud()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> localColonies = this.GetLocalColonies(false);
		if (localColonies.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in localColonies)
		{
			string baseName = keyValuePair.Value[0].BaseName;
			if (!list.Contains(baseName))
			{
				list.Add(baseName);
			}
		}
		bool flag = true;
		foreach (string colonyName in list)
		{
			flag &= this.ConvertColonyToCloud(colonyName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_CLOUD_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(true);
	}

	// Token: 0x0600605F RID: 24671 RVA: 0x00235D04 File Offset: 0x00233F04
	private void ConvertAllToCloud()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToCloud();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		}, this.localToCloudSprite);
	}

	// Token: 0x06006060 RID: 24672 RVA: 0x00235D88 File Offset: 0x00233F88
	private void ConvertAllToLocal()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToLocal();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetCloudSavePrefix());
		}, this.cloudToLocalSprite);
	}

	// Token: 0x06006061 RID: 24673 RVA: 0x00235E0C File Offset: 0x0023400C
	private void ShowSaveInfo()
	{
		if (this.infoScreen == null)
		{
			this.infoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TITLE).AddSprite(this.infoSprite).AddPlainText(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TEXT).AddOption(UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL(SaveLoader.GetSavePrefix());
			}, true).AddDefaultCancel();
			string cloudRoot = SaveLoader.GetCloudSavePrefix();
			if (cloudRoot != null && this.CloudSavesVisible())
			{
				this.infoScreen.AddOption(UI.FRONTEND.LOADSCREEN.OPEN_CLOUDSAVE_FOLDER, delegate(InfoDialogScreen d)
				{
					App.OpenWebURL(cloudRoot);
				}, true);
			}
			this.infoScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006062 RID: 24674 RVA: 0x00235EFD File Offset: 0x002340FD
	protected override void OnDeactivate()
	{
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.selectedSave = null;
		base.OnDeactivate();
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x00235F24 File Offset: 0x00234124
	private void ShowColonyList()
	{
		this.colonyListRoot.SetActive(true);
		this.colonyViewRoot.SetActive(false);
		this.currentColony = null;
		this.selectedSave = null;
	}

	// Token: 0x06006064 RID: 24676 RVA: 0x00235F4C File Offset: 0x0023414C
	private bool CheckSaveVersion(LoadScreen.SaveGameFileDetails save, LocText display)
	{
		if (LoadScreen.IsSaveFileFromUnsupportedFutureBuild(save.FileHeader, save.FileInfo))
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, new object[]
				{
					save.FileName,
					save.FileHeader.buildVersion,
					save.FileInfo.saveMinorVersion,
					706793U,
					37
				});
			}
			return false;
		}
		if (save.FileInfo.saveMajorVersion < 7)
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, new object[]
				{
					save.FileName,
					save.FileInfo.saveMajorVersion,
					save.FileInfo.saveMinorVersion,
					7,
					37
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x06006065 RID: 24677 RVA: 0x00236050 File Offset: 0x00234250
	private bool CheckSaveDLCsCompatable(LoadScreen.SaveGameFileDetails save)
	{
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		return save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2);
	}

	// Token: 0x06006066 RID: 24678 RVA: 0x00236070 File Offset: 0x00234270
	private string GetSaveDLCIncompatabilityTooltip(LoadScreen.SaveGameFileDetails save)
	{
		string text = "";
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		if (save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
		{
			text = null;
		}
		else
		{
			text = UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION;
			foreach (string dlcId in hashSet)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE, DlcManager.GetDlcTitle(dlcId));
			}
			foreach (string dlcId2 in hashSet2)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE, DlcManager.GetDlcTitle(dlcId2));
			}
		}
		return text;
	}

	// Token: 0x06006067 RID: 24679 RVA: 0x00236160 File Offset: 0x00234360
	private void ShowColonySave(LoadScreen.SaveGameFileDetails save)
	{
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		component.GetReference<RectTransform>("Title").GetComponent<LocText>().text = save.BaseName;
		component.GetReference<RectTransform>("Date").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
		string text = save.FileInfo.clusterId;
		if (text != null && !SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
		{
			string text2 = SettingsCache.GetScope("EXPANSION1_ID") + text;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text2))
			{
				text = text2;
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Failed to find cluster " + text + " including the scoped path, setting to default cluster name."
				});
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				text = WorldGenSettings.ClusterDefaultName;
			}
		}
		ProcGen.World world = (text != null) ? SettingsCache.clusterLayouts.GetWorldData(text, 0) : null;
		string arg = (world != null) ? Strings.Get(world.name) : " - ";
		component.GetReference<LocText>("InfoWorld").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.WORLD_NAME, arg);
		component.GetReference<LocText>("InfoCycles").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.CYCLES_SURVIVED, save.FileInfo.numberOfCycles);
		component.GetReference<LocText>("InfoDupes").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.DUPLICANTS_ALIVE, save.FileInfo.numberOfDuplicants);
		TMP_Text component2 = component.GetReference<RectTransform>("FileSize").GetComponent<LocText>();
		string formattedBytes = GameUtil.GetFormattedBytes((ulong)save.Size);
		component2.text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_SIZE, formattedBytes);
		component.GetReference<RectTransform>("Filename").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_NAME, System.IO.Path.GetFileName(save.FileName));
		LocText component3 = component.GetReference<RectTransform>("AutoInfo").GetComponent<LocText>();
		component3.gameObject.SetActive(!this.CheckSaveVersion(save, component3));
		Image component4 = component.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(save.FileName, save.BaseName, component4, false);
		KButton component5 = component.GetReference<RectTransform>("DeleteButton").GetComponent<KButton>();
		component5.ClearOnClick();
		System.Action <>9__1;
		component5.onClick += delegate()
		{
			LoadScreen <>4__this = this;
			System.Action onDelete;
			if ((onDelete = <>9__1) == null)
			{
				onDelete = (<>9__1 = delegate()
				{
					int num = this.currentColony.IndexOf(save);
					this.currentColony.Remove(save);
					this.ShowColony(this.currentColony, num - 1);
				});
			}
			<>4__this.Delete(onDelete);
		};
	}

	// Token: 0x06006068 RID: 24680 RVA: 0x00236448 File Offset: 0x00234648
	private void ShowColony(List<LoadScreen.SaveGameFileDetails> saves, int selectIndex = -1)
	{
		if (saves.Count <= 0)
		{
			this.RefreshColonyList();
			this.ShowColonyList();
			return;
		}
		this.currentColony = saves;
		this.colonyListRoot.SetActive(false);
		this.colonyViewRoot.SetActive(true);
		string baseName = saves[0].BaseName;
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		KButton component2 = component.GetReference<RectTransform>("Back").GetComponent<KButton>();
		component2.ClearOnClick();
		component2.onClick += delegate()
		{
			this.ShowColonyList();
		};
		component.GetReference<RectTransform>("ColonyTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_TITLE, baseName);
		GameObject gameObject = component.GetReference<RectTransform>("Content").gameObject;
		RectTransform reference = component.GetReference<RectTransform>("SaveTemplate");
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			if (gameObject2 != null && gameObject2.name.Contains("Clone"))
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		if (selectIndex < 0)
		{
			selectIndex = 0;
		}
		if (selectIndex > saves.Count - 1)
		{
			selectIndex = saves.Count - 1;
		}
		for (int j = 0; j < saves.Count; j++)
		{
			LoadScreen.SaveGameFileDetails save = saves[j];
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(reference, gameObject.transform);
			HierarchyReferences component3 = rectTransform.GetComponent<HierarchyReferences>();
			rectTransform.gameObject.SetActive(true);
			component3.GetReference<RectTransform>("AutoLabel").gameObject.SetActive(save.FileInfo.isAutoSave);
			component3.GetReference<RectTransform>("SaveText").GetComponent<LocText>().text = System.IO.Path.GetFileNameWithoutExtension(save.FileName);
			component3.GetReference<RectTransform>("DateText").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
			component3.GetReference<RectTransform>("NewestLabel").gameObject.SetActive(j == 0);
			RectTransform reference2 = component3.GetReference<RectTransform>("DLCIconPrefab");
			foreach (string text in DlcManager.RELEASED_VERSIONS)
			{
				if (save.FileInfo.dlcIds.Contains(text))
				{
					GameObject gameObject3 = global::Util.KInstantiateUI(reference2.gameObject, reference2.transform.parent.gameObject, true);
					gameObject3.GetComponent<Image>().sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(text));
					gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(text)));
				}
			}
			object obj = this.CheckSaveVersion(save, null) && this.CheckSaveDLCsCompatable(save);
			KButton button = rectTransform.GetComponent<KButton>();
			button.ClearOnClick();
			button.onClick += delegate()
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			};
			object obj2 = obj;
			if (obj2 != null)
			{
				button.onDoubleClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			KButton component4 = component3.GetReference<RectTransform>("LoadButton").GetComponent<KButton>();
			component4.ClearOnClick();
			if (obj2 == null)
			{
				component4.isInteractable = false;
				component4.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
				component4.GetComponent<ToolTip>().SetSimpleTooltip(this.GetSaveDLCIncompatabilityTooltip(save));
			}
			else
			{
				component4.onClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			if (j == selectIndex)
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			}
		}
	}

	// Token: 0x06006069 RID: 24681 RVA: 0x0023684C File Offset: 0x00234A4C
	private void AddColonyToList(List<LoadScreen.SaveGameFileDetails> saves)
	{
		if (saves.Count == 0)
		{
			return;
		}
		HierarchyReferences freeElement = this.colonyListPool.GetFreeElement(this.saveButtonRoot, true);
		saves.Sort((LoadScreen.SaveGameFileDetails x, LoadScreen.SaveGameFileDetails y) => y.FileDate.CompareTo(x.FileDate));
		LoadScreen.SaveGameFileDetails saveGameFileDetails = saves[0];
		string colonyName = saveGameFileDetails.BaseName;
		ValueTuple<int, int, ulong> savesSizeAndCounts = this.GetSavesSizeAndCounts(saves);
		int item = savesSizeAndCounts.Item1;
		int item2 = savesSizeAndCounts.Item2;
		string formattedBytes = GameUtil.GetFormattedBytes(savesSizeAndCounts.Item3);
		freeElement.GetReference<RectTransform>("HeaderTitle").GetComponent<LocText>().text = colonyName;
		freeElement.GetReference<RectTransform>("HeaderDate").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), saveGameFileDetails.FileDate.ToLocalTime());
		freeElement.GetReference<RectTransform>("SaveTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_INFO, item, item2, formattedBytes);
		Image component = freeElement.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(saveGameFileDetails.FileName, colonyName, component, true);
		List<ValueTuple<Sprite, string>> list = new List<ValueTuple<Sprite, string>>();
		if (saveGameFileDetails.FileInfo.dlcIds.Contains("EXPANSION1_ID"))
		{
			list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo("EXPANSION1_ID")), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle("EXPANSION1_ID"))));
		}
		foreach (string text in saveGameFileDetails.FileInfo.dlcIds)
		{
			if (DlcManager.IsDlcId(text) && !(text == "EXPANSION1_ID"))
			{
				list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo(text)), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(text))));
			}
		}
		GameObject gameObject = freeElement.transform.Find("Header").Find("DlcIcons").Find("Prefab_DlcIcon").gameObject;
		gameObject.SetActive(false);
		for (int i = 0; i < gameObject.transform.parent.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.parent.GetChild(i).gameObject;
			if (gameObject2 != gameObject)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		foreach (ValueTuple<Sprite, string> valueTuple in list)
		{
			GameObject gameObject3 = global::Util.KInstantiateUI(gameObject, gameObject.transform.parent.gameObject, true);
			Image component2 = gameObject3.GetComponent<Image>();
			ToolTip component3 = gameObject3.GetComponent<ToolTip>();
			component2.sprite = valueTuple.Item1;
			component3.SetSimpleTooltip(valueTuple.Item2);
		}
		Component reference = freeElement.GetReference<RectTransform>("LocationIcons");
		bool flag = this.CloudSavesVisible();
		reference.gameObject.SetActive(flag);
		if (flag)
		{
			LocText locationText = freeElement.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
			bool isLocal = SaveLoader.IsSaveLocal(saveGameFileDetails.FileName);
			locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
			KButton cloudButton = freeElement.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
			KButton localButton = freeElement.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
			cloudButton.gameObject.SetActive(!isLocal);
			cloudButton.ClearOnClick();
			System.Action <>9__4;
			cloudButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__4) == null)
				{
					commitAction = (<>9__4 = delegate()
					{
						cloudButton.gameObject.SetActive(false);
						isLocal = true;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToLocal(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.cloudToLocalSprite);
			};
			localButton.gameObject.SetActive(isLocal);
			localButton.ClearOnClick();
			System.Action <>9__5;
			localButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__5) == null)
				{
					commitAction = (<>9__5 = delegate()
					{
						localButton.gameObject.SetActive(false);
						isLocal = false;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToCloud(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.localToCloudSprite);
			};
		}
		this.GetSaveDLCIncompatabilityTooltip(saveGameFileDetails);
		freeElement.GetReference<RectTransform>("Button").GetComponent<KButton>().onClick += delegate()
		{
			this.ShowColony(saves, -1);
		};
		freeElement.transform.SetAsLastSibling();
	}

	// Token: 0x0600606A RID: 24682 RVA: 0x00236CEC File Offset: 0x00234EEC
	private void SetPreview(string filename, string basename, Image preview, bool fallbackToTimelapse = false)
	{
		preview.color = Color.black;
		preview.gameObject.SetActive(false);
		try
		{
			Sprite sprite = RetireColonyUtility.LoadColonyPreview(filename, basename, fallbackToTimelapse);
			if (!(sprite == null))
			{
				Rect rect = preview.rectTransform.parent.rectTransform().rect;
				preview.sprite = sprite;
				preview.color = (sprite ? Color.white : Color.black);
				float num = sprite.bounds.size.x / sprite.bounds.size.y;
				if ((double)num >= 1.77777777777778)
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.height * num, rect.height);
				}
				else
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.width, rect.width / num);
				}
				preview.gameObject.SetActive(true);
			}
		}
		catch (Exception obj)
		{
			global::Debug.Log(obj);
		}
	}

	// Token: 0x0600606B RID: 24683 RVA: 0x00236DFC File Offset: 0x00234FFC
	public static void ForceStopGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.ClearGameFields();
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.SetIsLoading();
		Grid.CellCount = 0;
		Sim.Shutdown();
	}

	// Token: 0x0600606C RID: 24684 RVA: 0x00236E28 File Offset: 0x00235028
	private static bool IsSaveFileFromUnsupportedFutureBuild(SaveGame.Header header, SaveGame.GameInfo gameInfo)
	{
		return gameInfo.saveMajorVersion > 7 || (gameInfo.saveMajorVersion == 7 && gameInfo.saveMinorVersion > 37) || header.buildVersion > 706793U;
	}

	// Token: 0x0600606D RID: 24685 RVA: 0x00236E58 File Offset: 0x00235058
	private void UpdateSelected(KButton button, string filename, List<string> dlcIds)
	{
		if (this.selectedSave != null && this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		if (this.selectedSave == null)
		{
			this.selectedSave = new LoadScreen.SelectedSave();
		}
		this.selectedSave.button = button;
		this.selectedSave.filename = filename;
		this.selectedSave.dlcIds = dlcIds;
		if (this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
	}

	// Token: 0x0600606E RID: 24686 RVA: 0x00236EF8 File Offset: 0x002350F8
	private void Load()
	{
		if (!DlcManager.IsAllContentSubscribed(this.selectedSave.dlcIds))
		{
			string message = this.selectedSave.dlcIds.Contains("") ? UI.FRONTEND.LOADSCREEN.VANILLA_RESTART : UI.FRONTEND.LOADSCREEN.EXPANSION1_RESTART;
			this.ConfirmDoAction(message, delegate
			{
				KPlayerPrefs.SetString("AutoResumeSaveFile", this.selectedSave.filename);
				DlcManager.ToggleDLC("EXPANSION1_ID");
			});
			return;
		}
		LoadingOverlay.Load(new System.Action(this.DoLoad));
	}

	// Token: 0x0600606F RID: 24687 RVA: 0x00236F65 File Offset: 0x00235165
	private void DoLoad()
	{
		if (this.selectedSave == null)
		{
			return;
		}
		LoadScreen.DoLoad(this.selectedSave.filename);
		this.Deactivate();
	}

	// Token: 0x06006070 RID: 24688 RVA: 0x00236F88 File Offset: 0x00235188
	public static void DoLoad(string filename)
	{
		KCrashReporter.MOST_RECENT_SAVEFILE = filename;
		bool flag = true;
		SaveGame.Header header;
		SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
		string arg = null;
		string arg2 = null;
		if (header.buildVersion > 706793U)
		{
			arg = header.buildVersion.ToString();
			arg2 = 706793U.ToString();
		}
		else if (gameInfo.saveMajorVersion < 7)
		{
			arg = string.Format("v{0}.{1}", gameInfo.saveMajorVersion, gameInfo.saveMinorVersion);
			arg2 = string.Format("v{0}.{1}", 7, 37);
		}
		if (!flag)
		{
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.CRASHSCREEN.LOADFAILED, "Version Mismatch", arg, arg2), null, null, null, null, null, null, null, null);
			return;
		}
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		Time.timeScale = 0f;
		SaveLoader.LoadScene();
	}

	// Token: 0x06006071 RID: 24689 RVA: 0x002370A4 File Offset: 0x002352A4
	private void MoreInfo()
	{
		App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
	}

	// Token: 0x06006072 RID: 24690 RVA: 0x002370B0 File Offset: 0x002352B0
	private void Delete(System.Action onDelete)
	{
		if (this.selectedSave == null || string.IsNullOrEmpty(this.selectedSave.filename))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(string.Format(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, System.IO.Path.GetFileName(this.selectedSave.filename)), delegate
		{
			try
			{
				this.DeleteFileAndEmptyFolder(this.selectedSave.filename);
				string file = System.IO.Path.ChangeExtension(this.selectedSave.filename, "png");
				this.DeleteFileAndEmptyFolder(file);
				if (onDelete != null)
				{
					onDelete();
				}
				MainMenu.Instance.RefreshResumeButton(false);
			}
			catch (SystemException ex)
			{
				global::Debug.LogError(ex.ToString());
			}
		});
	}

	// Token: 0x06006073 RID: 24691 RVA: 0x00237127 File Offset: 0x00235327
	private void ShowSimpleDialog(string title, string message)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddPlainText(message).AddDefaultOK(false).Activate();
	}

	// Token: 0x06006074 RID: 24692 RVA: 0x0023715C File Offset: 0x0023535C
	private void ConfirmCloudSaveMigrations(string message, string title, string confirmText, string backupText, System.Action commitAction, System.Action backupAction, Sprite sprite)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddSprite(sprite).AddPlainText(message).AddDefaultCancel().AddOption(confirmText, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
			commitAction();
		}, true).Activate();
	}

	// Token: 0x06006075 RID: 24693 RVA: 0x002371C4 File Offset: 0x002353C4
	private void ShowConvertError(string message)
	{
		if (this.errorInfoScreen == null)
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddDefaultOK(false);
				this.errorInfoScreen.Activate();
				return;
			}
			this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false).AddDefaultOK(false);
			this.errorInfoScreen.Activate();
		}
	}

	// Token: 0x06006076 RID: 24694 RVA: 0x002372C4 File Offset: 0x002354C4
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006077 RID: 24695 RVA: 0x00237343 File Offset: 0x00235543
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.currentColony != null && e.TryConsume(global::Action.Escape))
		{
			this.ShowColonyList();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04004055 RID: 16469
	private const int MAX_CLOUD_TUTORIALS = 5;

	// Token: 0x04004056 RID: 16470
	private const string CLOUD_TUTORIAL_KEY = "LoadScreenCloudTutorialTimes";

	// Token: 0x04004057 RID: 16471
	private const int ITEMS_PER_PAGE = 20;

	// Token: 0x04004058 RID: 16472
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004059 RID: 16473
	[SerializeField]
	private GameObject saveButtonRoot;

	// Token: 0x0400405A RID: 16474
	[SerializeField]
	private GameObject colonyListRoot;

	// Token: 0x0400405B RID: 16475
	[SerializeField]
	private GameObject colonyViewRoot;

	// Token: 0x0400405C RID: 16476
	[SerializeField]
	private HierarchyReferences migrationPanelRefs;

	// Token: 0x0400405D RID: 16477
	[SerializeField]
	private HierarchyReferences saveButtonPrefab;

	// Token: 0x0400405E RID: 16478
	[SerializeField]
	private KButton loadMoreButton;

	// Token: 0x0400405F RID: 16479
	[Space]
	[SerializeField]
	private KButton colonyCloudButton;

	// Token: 0x04004060 RID: 16480
	[SerializeField]
	private KButton colonyLocalButton;

	// Token: 0x04004061 RID: 16481
	[SerializeField]
	private KButton colonyInfoButton;

	// Token: 0x04004062 RID: 16482
	[SerializeField]
	private Sprite localToCloudSprite;

	// Token: 0x04004063 RID: 16483
	[SerializeField]
	private Sprite cloudToLocalSprite;

	// Token: 0x04004064 RID: 16484
	[SerializeField]
	private Sprite errorSprite;

	// Token: 0x04004065 RID: 16485
	[SerializeField]
	private Sprite infoSprite;

	// Token: 0x04004066 RID: 16486
	[SerializeField]
	private Bouncer cloudTutorialBouncer;

	// Token: 0x04004067 RID: 16487
	public bool requireConfirmation = true;

	// Token: 0x04004068 RID: 16488
	private LoadScreen.SelectedSave selectedSave;

	// Token: 0x04004069 RID: 16489
	private List<LoadScreen.SaveGameFileDetails> currentColony;

	// Token: 0x0400406A RID: 16490
	private UIPool<HierarchyReferences> colonyListPool;

	// Token: 0x0400406B RID: 16491
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x0400406C RID: 16492
	private InfoDialogScreen infoScreen;

	// Token: 0x0400406D RID: 16493
	private InfoDialogScreen errorInfoScreen;

	// Token: 0x0400406E RID: 16494
	private ConfirmDialogScreen errorScreen;

	// Token: 0x0400406F RID: 16495
	private InspectSaveScreen inspectScreenInstance;

	// Token: 0x04004070 RID: 16496
	private int displayedPageCount = 1;

	// Token: 0x02001E0B RID: 7691
	private struct SaveGameFileDetails
	{
		// Token: 0x04008D26 RID: 36134
		public string BaseName;

		// Token: 0x04008D27 RID: 36135
		public string FileName;

		// Token: 0x04008D28 RID: 36136
		public string UniqueID;

		// Token: 0x04008D29 RID: 36137
		public System.DateTime FileDate;

		// Token: 0x04008D2A RID: 36138
		public SaveGame.Header FileHeader;

		// Token: 0x04008D2B RID: 36139
		public SaveGame.GameInfo FileInfo;

		// Token: 0x04008D2C RID: 36140
		public long Size;
	}

	// Token: 0x02001E0C RID: 7692
	private class SelectedSave
	{
		// Token: 0x04008D2D RID: 36141
		public string filename;

		// Token: 0x04008D2E RID: 36142
		public List<string> dlcIds;

		// Token: 0x04008D2F RID: 36143
		public KButton button;
	}
}
