using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000B28 RID: 2856
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoader")]
public class SaveLoader : KMonoBehaviour
{
	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600537D RID: 21373 RVA: 0x001E6883 File Offset: 0x001E4A83
	// (set) Token: 0x0600537E RID: 21374 RVA: 0x001E688B File Offset: 0x001E4A8B
	public bool loadedFromSave { get; private set; }

	// Token: 0x0600537F RID: 21375 RVA: 0x001E6894 File Offset: 0x001E4A94
	public static void DestroyInstance()
	{
		SaveLoader.Instance = null;
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06005380 RID: 21376 RVA: 0x001E689C File Offset: 0x001E4A9C
	// (set) Token: 0x06005381 RID: 21377 RVA: 0x001E68A3 File Offset: 0x001E4AA3
	public static SaveLoader Instance { get; private set; }

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06005382 RID: 21378 RVA: 0x001E68AB File Offset: 0x001E4AAB
	// (set) Token: 0x06005383 RID: 21379 RVA: 0x001E68B3 File Offset: 0x001E4AB3
	public Action<Cluster> OnWorldGenComplete { get; set; }

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06005384 RID: 21380 RVA: 0x001E68BC File Offset: 0x001E4ABC
	public Cluster Cluster
	{
		get
		{
			return this.m_cluster;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06005385 RID: 21381 RVA: 0x001E68C4 File Offset: 0x001E4AC4
	public ClusterLayout ClusterLayout
	{
		get
		{
			if (this.m_clusterLayout == null)
			{
				this.m_clusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			}
			return this.m_clusterLayout;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06005386 RID: 21382 RVA: 0x001E68E4 File Offset: 0x001E4AE4
	// (set) Token: 0x06005387 RID: 21383 RVA: 0x001E68EC File Offset: 0x001E4AEC
	public SaveGame.GameInfo GameInfo { get; private set; }

	// Token: 0x06005388 RID: 21384 RVA: 0x001E68F5 File Offset: 0x001E4AF5
	protected override void OnPrefabInit()
	{
		SaveLoader.Instance = this;
		this.saveManager = base.GetComponent<SaveManager>();
	}

	// Token: 0x06005389 RID: 21385 RVA: 0x001E6909 File Offset: 0x001E4B09
	private void MoveCorruptFile(string filename)
	{
	}

	// Token: 0x0600538A RID: 21386 RVA: 0x001E690C File Offset: 0x001E4B0C
	protected override void OnSpawn()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (WorldGen.CanLoad(activeSaveFilePath))
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(Db.Get().Diseases);
			this.loadedFromSave = true;
			this.loadedFromSave = this.Load(activeSaveFilePath);
			this.saveFileCorrupt = !this.loadedFromSave;
			if (!this.loadedFromSave)
			{
				SaveLoader.SetActiveSaveFilePath(null);
				if (this.mustRestartOnFail)
				{
					this.MoveCorruptFile(activeSaveFilePath);
					Sim.Shutdown();
					App.LoadScene("frontend");
					return;
				}
			}
		}
		if (!this.loadedFromSave)
		{
			Sim.Shutdown();
			if (!string.IsNullOrEmpty(activeSaveFilePath))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Couldn't load [" + activeSaveFilePath + "]"
				});
			}
			if (this.saveFileCorrupt)
			{
				this.MoveCorruptFile(activeSaveFilePath);
			}
			if (!this.LoadFromWorldGen())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Couldn't start new game with current world gen, moving file"
				});
				KMonoBehaviour.isLoadingScene = true;
				this.MoveCorruptFile(WorldGen.WORLDGEN_SAVE_FILENAME);
				App.LoadScene("frontend");
			}
		}
	}

	// Token: 0x0600538B RID: 21387 RVA: 0x001E6A1C File Offset: 0x001E4C1C
	private static void CompressContents(BinaryWriter fileWriter, byte[] uncompressed, int length)
	{
		using (ZlibStream zlibStream = new ZlibStream(fileWriter.BaseStream, CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
		{
			zlibStream.Write(uncompressed, 0, length);
			zlibStream.Flush();
		}
	}

	// Token: 0x0600538C RID: 21388 RVA: 0x001E6A64 File Offset: 0x001E4C64
	private byte[] FloatToBytes(float[] floats)
	{
		byte[] array = new byte[floats.Length * 4];
		Buffer.BlockCopy(floats, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x0600538D RID: 21389 RVA: 0x001E6A89 File Offset: 0x001E4C89
	private static byte[] DecompressContents(byte[] compressed)
	{
		return ZlibStream.UncompressBuffer(compressed);
	}

	// Token: 0x0600538E RID: 21390 RVA: 0x001E6A94 File Offset: 0x001E4C94
	private float[] BytesToFloat(byte[] bytes)
	{
		float[] array = new float[bytes.Length / 4];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return array;
	}

	// Token: 0x0600538F RID: 21391 RVA: 0x001E6ABC File Offset: 0x001E4CBC
	private SaveFileRoot PrepSaveFile()
	{
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		saveFileRoot.WidthInCells = Grid.WidthInCells;
		saveFileRoot.HeightInCells = Grid.HeightInCells;
		saveFileRoot.streamed["GridVisible"] = Grid.Visible;
		saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
		saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
		Global.Instance.modManager.SendMetricsEvent();
		saveFileRoot.active_mods = new List<Label>();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				saveFileRoot.active_mods.Add(mod.label);
			}
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				Camera.main.transform.parent.GetComponent<CameraController>().Save(binaryWriter);
			}
			saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
		}
		return saveFileRoot;
	}

	// Token: 0x06005390 RID: 21392 RVA: 0x001E6C18 File Offset: 0x001E4E18
	private void Save(BinaryWriter writer)
	{
		writer.WriteKleiString("world");
		Serializer.Serialize(this.PrepSaveFile(), writer);
		Game.SaveSettings(writer);
		Sim.Save(writer, 0, 0);
		this.saveManager.Save(writer);
		Game.Instance.Save(writer);
	}

	// Token: 0x06005391 RID: 21393 RVA: 0x001E6C58 File Offset: 0x001E4E58
	private bool Load(IReader reader)
	{
		global::Debug.Assert(reader.ReadKleiString() == "world");
		Deserializer deserializer = new Deserializer(reader);
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		deserializer.Deserialize(saveFileRoot);
		if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
		{
			saveFileRoot.active_mods = new List<Label>();
			foreach (ModInfo modInfo in saveFileRoot.requiredMods)
			{
				saveFileRoot.active_mods.Add(new Label
				{
					id = modInfo.assetID,
					version = (long)modInfo.lastModifiedTime,
					distribution_platform = Label.DistributionPlatform.Steam,
					title = modInfo.description
				});
			}
			saveFileRoot.requiredMods.Clear();
		}
		KMod.Manager modManager = Global.Instance.modManager;
		modManager.Load(Content.LayerableFiles);
		if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Mod footprint of save file doesn't match current mod configuration"
			});
		}
		string text = string.Format("Mod Footprint ({0}):", saveFileRoot.active_mods.Count);
		foreach (Label label in saveFileRoot.active_mods)
		{
			text = text + "\n  - " + label.title;
		}
		global::Debug.Log(text);
		this.LogActiveMods();
		Global.Instance.modManager.SendMetricsEvent();
		WorldGen.LoadSettings(false);
		CustomGameSettings.Instance.LoadClusters();
		if (this.GameInfo.clusterId == null)
		{
			SaveGame.GameInfo gameInfo = this.GameInfo;
			if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
			{
				gameInfo.clusterId = saveFileRoot.clusterID;
			}
			else
			{
				try
				{
					gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
				}
				catch
				{
					gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
					CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
				}
			}
			this.GameInfo = gameInfo;
		}
		Game.clusterId = this.GameInfo.clusterId;
		Game.LoadSettings(deserializer);
		GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells, false);
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		IReader reader2;
		if (saveFileRoot.streamed.ContainsKey("Sim"))
		{
			reader2 = new FastReader(saveFileRoot.streamed["Sim"]);
		}
		else
		{
			reader2 = reader;
		}
		if (Sim.LoadWorld(reader2) != 0)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\nSimDLL found bad data\n"
			});
			Sim.Shutdown();
			return false;
		}
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		this.mustRestartOnFail = true;
		if (!this.saveManager.Load(reader))
		{
			Sim.Shutdown();
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n"
			});
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Grid.Visible = saveFileRoot.streamed["GridVisible"];
		if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
		{
			Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
		}
		Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
		Game.Instance.Load(deserializer);
		CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
		ClusterManager.Instance.InitializeWorldGrid();
		SimMessages.DefineWorldOffsets((from container in ClusterManager.Instance.WorldContainers
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = container.WorldOffset.x,
			worldOffsetY = container.WorldOffset.y,
			worldSizeX = container.WorldSize.x,
			worldSizeY = container.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		return true;
	}

	// Token: 0x06005392 RID: 21394 RVA: 0x001E707C File Offset: 0x001E527C
	private void LogActiveMods()
	{
		string text = string.Format("Active Mods ({0}):", Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()));
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				text = text + "\n  - " + mod.title;
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x06005393 RID: 21395 RVA: 0x001E7134 File Offset: 0x001E5334
	public static string GetSavePrefix()
	{
		return System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "save_files", System.IO.Path.DirectorySeparatorChar));
	}

	// Token: 0x06005394 RID: 21396 RVA: 0x001E715C File Offset: 0x001E535C
	public static string GetCloudSavePrefix()
	{
		string text = System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "cloud_save_files", System.IO.Path.DirectorySeparatorChar));
		string userID = SaveLoader.GetUserID();
		if (string.IsNullOrEmpty(userID))
		{
			return null;
		}
		text = System.IO.Path.Combine(text, userID);
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06005395 RID: 21397 RVA: 0x001E71B8 File Offset: 0x001E53B8
	public static string GetSavePrefixAndCreateFolder()
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		if (!System.IO.Directory.Exists(savePrefix))
		{
			System.IO.Directory.CreateDirectory(savePrefix);
		}
		return savePrefix;
	}

	// Token: 0x06005396 RID: 21398 RVA: 0x001E71DC File Offset: 0x001E53DC
	public static string GetUserID()
	{
		DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
		if (localUser == null)
		{
			return null;
		}
		return localUser.Id.ToString();
	}

	// Token: 0x06005397 RID: 21399 RVA: 0x001E7204 File Offset: 0x001E5404
	public static string GetNextUsableSavePath(string filename)
	{
		int num = 0;
		string arg = System.IO.Path.ChangeExtension(filename, null);
		while (File.Exists(filename))
		{
			filename = SaveScreen.GetValidSaveFilename(string.Format("{0} ({1})", arg, num));
			num++;
		}
		return filename;
	}

	// Token: 0x06005398 RID: 21400 RVA: 0x001E7242 File Offset: 0x001E5442
	public static string GetOriginalSaveFileName(string filename)
	{
		if (!filename.Contains("/") && !filename.Contains("\\"))
		{
			return filename;
		}
		filename.Replace('\\', '/');
		return System.IO.Path.GetFileName(filename);
	}

	// Token: 0x06005399 RID: 21401 RVA: 0x001E7271 File Offset: 0x001E5471
	public static bool IsSaveAuto(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/auto_save/");
	}

	// Token: 0x0600539A RID: 21402 RVA: 0x001E728A File Offset: 0x001E548A
	public static bool IsSaveLocal(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/save_files/");
	}

	// Token: 0x0600539B RID: 21403 RVA: 0x001E72A3 File Offset: 0x001E54A3
	public static bool IsSaveCloud(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/cloud_save_files/");
	}

	// Token: 0x0600539C RID: 21404 RVA: 0x001E72BC File Offset: 0x001E54BC
	public static string GetAutoSavePrefix()
	{
		string text = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), string.Format("{0}{1}", "auto_save", System.IO.Path.DirectorySeparatorChar));
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x0600539D RID: 21405 RVA: 0x001E72FD File Offset: 0x001E54FD
	public static void SetActiveSaveFilePath(string path)
	{
		KPlayerPrefs.SetString("SaveFilenameKey/", path);
	}

	// Token: 0x0600539E RID: 21406 RVA: 0x001E730A File Offset: 0x001E550A
	public static string GetActiveSaveFilePath()
	{
		return KPlayerPrefs.GetString("SaveFilenameKey/");
	}

	// Token: 0x0600539F RID: 21407 RVA: 0x001E7318 File Offset: 0x001E5518
	public static string GetActiveAutoSavePath()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath == null)
		{
			return SaveLoader.GetAutoSavePrefix();
		}
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "auto_save");
	}

	// Token: 0x060053A0 RID: 21408 RVA: 0x001E7344 File Offset: 0x001E5544
	public static string GetAutosaveFilePath()
	{
		return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
	}

	// Token: 0x060053A1 RID: 21409 RVA: 0x001E7358 File Offset: 0x001E5558
	public static string GetActiveSaveColonyFolder()
	{
		string text = SaveLoader.GetActiveSaveFolder();
		if (text == null)
		{
			text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), SaveLoader.Instance.GameInfo.baseName);
		}
		return text;
	}

	// Token: 0x060053A2 RID: 21410 RVA: 0x001E738C File Offset: 0x001E558C
	public static string GetActiveSaveFolder()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(activeSaveFilePath))
		{
			return System.IO.Path.GetDirectoryName(activeSaveFilePath);
		}
		return null;
	}

	// Token: 0x060053A3 RID: 21411 RVA: 0x001E73B0 File Offset: 0x001E55B0
	public static List<SaveLoader.SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search = SearchOption.AllDirectories)
	{
		List<SaveLoader.SaveFileEntry> list = new List<SaveLoader.SaveFileEntry>();
		if (string.IsNullOrEmpty(save_dir))
		{
			return list;
		}
		try
		{
			if (!System.IO.Directory.Exists(save_dir))
			{
				System.IO.Directory.CreateDirectory(save_dir);
			}
			foreach (string text in System.IO.Directory.GetFiles(save_dir, "*.sav", search))
			{
				try
				{
					if (!text.StartsWith("._"))
					{
						System.DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text);
						SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
						{
							path = text,
							timeStamp = lastWriteTimeUtc
						};
						list.Add(item);
					}
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString());
				}
			}
			if (sort)
			{
				list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
		}
		catch (Exception ex2)
		{
			string text2 = null;
			if (ex2 is UnauthorizedAccessException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, save_dir);
			}
			else if (ex2 is IOException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, save_dir);
			}
			if (text2 == null)
			{
				throw ex2;
			}
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text2, null, null, null, null, null, null, null, null);
		}
		finally
		{
		}
		return list;
	}

	// Token: 0x060053A4 RID: 21412 RVA: 0x001E7570 File Offset: 0x001E5770
	public static List<SaveLoader.SaveFileEntry> GetAllFiles(bool sort, SaveLoader.SaveType type = SaveLoader.SaveType.both)
	{
		switch (type)
		{
		case SaveLoader.SaveType.local:
			return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.cloud:
			return SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.both:
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), false, SearchOption.AllDirectories);
			List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), false, SearchOption.AllDirectories);
			saveFiles.AddRange(saveFiles2);
			if (sort)
			{
				saveFiles.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
			return saveFiles;
		}
		default:
			return new List<SaveLoader.SaveFileEntry>();
		}
	}

	// Token: 0x060053A5 RID: 21413 RVA: 0x001E75FB File Offset: 0x001E57FB
	public static List<SaveLoader.SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search = SearchOption.TopDirectoryOnly)
	{
		return SaveLoader.GetSaveFiles(SaveLoader.GetActiveSaveColonyFolder(), sort, search);
	}

	// Token: 0x060053A6 RID: 21414 RVA: 0x001E7609 File Offset: 0x001E5809
	public static bool GetCloudSavesDefault()
	{
		return !(SaveLoader.GetCloudSavesDefaultPref() == "Disabled");
	}

	// Token: 0x060053A7 RID: 21415 RVA: 0x001E7620 File Offset: 0x001E5820
	public static string GetCloudSavesDefaultPref()
	{
		string text = KPlayerPrefs.GetString("SavesDefaultToCloud", "Enabled");
		if (text != "Enabled" && text != "Disabled")
		{
			text = "Enabled";
		}
		return text;
	}

	// Token: 0x060053A8 RID: 21416 RVA: 0x001E765E File Offset: 0x001E585E
	public static void SetCloudSavesDefault(bool value)
	{
		SaveLoader.SetCloudSavesDefaultPref(value ? "Enabled" : "Disabled");
	}

	// Token: 0x060053A9 RID: 21417 RVA: 0x001E7674 File Offset: 0x001E5874
	public static void SetCloudSavesDefaultPref(string pref)
	{
		if (pref != "Enabled" && pref != "Disabled")
		{
			global::Debug.LogWarning("Ignoring cloud saves default pref `" + pref + "` as it's not valid, expected `Enabled` or `Disabled`");
			return;
		}
		KPlayerPrefs.SetString("SavesDefaultToCloud", pref);
	}

	// Token: 0x060053AA RID: 21418 RVA: 0x001E76B1 File Offset: 0x001E58B1
	public static bool GetCloudSavesAvailable()
	{
		return !string.IsNullOrEmpty(SaveLoader.GetUserID()) && SaveLoader.GetCloudSavePrefix() != null;
	}

	// Token: 0x060053AB RID: 21419 RVA: 0x001E76CC File Offset: 0x001E58CC
	public static string GetLatestSaveForCurrentDLC()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		for (int i = 0; i < allFiles.Count; i++)
		{
			global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(allFiles[i].path);
			if (fileInfo != null)
			{
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (second.saveMajorVersion >= 7 && second.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					return allFiles[i].path;
				}
			}
		}
		return null;
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x001E7740 File Offset: 0x001E5940
	public void InitialSave()
	{
		string text = SaveLoader.GetActiveSaveFilePath();
		if (string.IsNullOrEmpty(text))
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		else if (!text.Contains(".sav"))
		{
			text += ".sav";
		}
		this.LogActiveMods();
		this.Save(text, false, true);
	}

	// Token: 0x060053AD RID: 21421 RVA: 0x001E778C File Offset: 0x001E598C
	public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		KSerialization.Manager.Clear();
		string directoryName = System.IO.Path.GetDirectoryName(filename);
		try
		{
			if (directoryName != null && !System.IO.Directory.Exists(directoryName))
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
		}
		this.ReportSaveMetrics(isAutoSave);
		RetireColonyUtility.SaveColonySummaryData();
		if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true, SearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFiles)
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(saveFileEntry.path);
				if (fileInfo != null && SaveGame.GetSaveUniqueID(fileInfo.second) == SaveLoader.Instance.GameInfo.colonyGuid.ToString())
				{
					list.Add(saveFileEntry.path);
				}
			}
			for (int i = list.Count - 1; i >= 9; i--)
			{
				string text = list[i];
				try
				{
					global::Debug.Log("Deleting old autosave: " + text);
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					global::Debug.LogWarning("Problem deleting autosave: " + text + "\n" + ex2.ToString());
				}
				string text2 = System.IO.Path.ChangeExtension(text, ".png");
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (Exception ex3)
				{
					global::Debug.LogWarning("Problem deleting autosave screenshot: " + text2 + "\n" + ex3.ToString());
				}
			}
		}
		using (MemoryStream memoryStream = new MemoryStream((int)((float)this.lastUncompressedSize * 1.1f)))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Save(binaryWriter);
				this.lastUncompressedSize = (int)memoryStream.Length;
				try
				{
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
					{
						SaveGame.Header header;
						byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
						binaryWriter2.Write(header.buildVersion);
						binaryWriter2.Write(header.headerSize);
						binaryWriter2.Write(header.headerVersion);
						binaryWriter2.Write(header.compression);
						binaryWriter2.Write(saveHeader);
						KSerialization.Manager.SerializeDirectory(binaryWriter2);
						if (this.compressSaveData)
						{
							SaveLoader.CompressContents(binaryWriter2, memoryStream.GetBuffer(), (int)memoryStream.Length);
						}
						else
						{
							binaryWriter2.Write(memoryStream.ToArray());
						}
						KCrashReporter.MOST_RECENT_SAVEFILE = filename;
						Stats.Print();
					}
				}
				catch (Exception ex4)
				{
					if (ex4 is UnauthorizedAccessException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"UnauthorizedAccessException for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					if (ex4 is IOException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"IOException (probably out of disk space) for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					throw ex4;
				}
			}
		}
		if (updateSavePointer)
		{
			SaveLoader.SetActiveSaveFilePath(filename);
		}
		Game.Instance.timelapser.SaveColonyPreview(filename);
		DebugUtil.LogArgs(new object[]
		{
			"Saved to",
			"[" + filename + "]"
		});
		GC.Collect();
		return filename;
	}

	// Token: 0x060053AE RID: 21422 RVA: 0x001E7C34 File Offset: 0x001E5E34
	public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
	{
		byte[] array = new byte[512];
		SaveGame.GameInfo header2;
		using (FileStream fileStream = File.OpenRead(filename))
		{
			fileStream.Read(array, 0, 512);
			header2 = SaveGame.GetHeader(new FastReader(array), out header, filename);
		}
		return header2;
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x001E7C8C File Offset: 0x001E5E8C
	public bool Load(string filename)
	{
		SaveLoader.SetActiveSaveFilePath(filename);
		try
		{
			KSerialization.Manager.Clear();
			byte[] array = File.ReadAllBytes(filename);
			IReader reader = new FastReader(array);
			SaveGame.Header header;
			this.GameInfo = SaveGame.GetHeader(reader, out header, filename);
			ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
			DebugUtil.LogArgs(new object[]
			{
				string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", new object[]
				{
					header.headerVersion,
					header.buildVersion,
					header.headerSize,
					header.IsCompressed,
					filename
				})
			});
			DebugUtil.LogArgs(new object[]
			{
				string.Format("GameInfo loaded from save header:\n  numberOfCycles:{0},\n  numberOfDuplicants:{1},\n  baseName:{2},\n  isAutoSave:{3},\n  originalSaveName:{4},\n  clusterId:{5},\n  worldTraits:{6},\n  colonyGuid:{7},\n  saveVersion:{8}.{9}", new object[]
				{
					this.GameInfo.numberOfCycles,
					this.GameInfo.numberOfDuplicants,
					this.GameInfo.baseName,
					this.GameInfo.isAutoSave,
					this.GameInfo.originalSaveName,
					this.GameInfo.clusterId,
					(this.GameInfo.worldTraits != null && this.GameInfo.worldTraits.Length != 0) ? string.Join(", ", this.GameInfo.worldTraits) : "<i>none</i>",
					this.GameInfo.colonyGuid,
					this.GameInfo.saveMajorVersion,
					this.GameInfo.saveMinorVersion
				})
			});
			string originalSaveName = this.GameInfo.originalSaveName;
			if (originalSaveName.Contains("/") || originalSaveName.Contains("\\"))
			{
				string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(originalSaveName);
				SaveGame.GameInfo gameInfo = this.GameInfo;
				gameInfo.originalSaveName = originalSaveFileName;
				this.GameInfo = gameInfo;
				global::Debug.Log(string.Concat(new string[]
				{
					"Migration / Save originalSaveName updated from: `",
					originalSaveName,
					"` => `",
					this.GameInfo.originalSaveName,
					"`"
				}));
			}
			if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
			{
				Helper.SetTypeInfoMask((SerializationTypeInfo)191);
			}
			KSerialization.Manager.DeserializeDirectory(reader);
			if (header.IsCompressed)
			{
				int num = array.Length - reader.Position;
				byte[] array2 = new byte[num];
				Array.Copy(array, reader.Position, array2, 0, num);
				byte[] array3 = SaveLoader.DecompressContents(array2);
				this.lastUncompressedSize = array3.Length;
				IReader reader2 = new FastReader(array3);
				this.Load(reader2);
			}
			else
			{
				this.lastUncompressedSize = array.Length;
				this.Load(reader);
			}
			KCrashReporter.MOST_RECENT_SAVEFILE = filename;
			if (this.GameInfo.isAutoSave && !string.IsNullOrEmpty(this.GameInfo.originalSaveName))
			{
				string originalSaveFileName2 = SaveLoader.GetOriginalSaveFileName(this.GameInfo.originalSaveName);
				string text;
				if (SaveLoader.IsSaveCloud(filename))
				{
					string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
					if (cloudSavePrefix != null)
					{
						text = System.IO.Path.Combine(cloudSavePrefix, this.GameInfo.baseName, originalSaveFileName2);
					}
					else
					{
						text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename).Replace("auto_save", ""), this.GameInfo.baseName, originalSaveFileName2);
					}
				}
				else
				{
					text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), this.GameInfo.baseName, originalSaveFileName2);
				}
				if (text != null)
				{
					SaveLoader.SetActiveSaveFilePath(text);
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace
			});
			Sim.Shutdown();
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Stats.Print();
		DebugUtil.LogArgs(new object[]
		{
			"Loaded",
			"[" + filename + "]"
		});
		DebugUtil.LogArgs(new object[]
		{
			"World Seeds",
			string.Concat(new string[]
			{
				"[",
				this.clusterDetailSave.globalWorldSeed.ToString(),
				"/",
				this.clusterDetailSave.globalWorldLayoutSeed.ToString(),
				"/",
				this.clusterDetailSave.globalTerrainSeed.ToString(),
				"/",
				this.clusterDetailSave.globalNoiseSeed.ToString(),
				"]"
			})
		});
		GC.Collect();
		return true;
	}

	// Token: 0x060053B0 RID: 21424 RVA: 0x001E8120 File Offset: 0x001E6320
	public bool LoadFromWorldGen()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Attempting to start a new game with current world gen"
		});
		WorldGen.LoadSettings(false);
		FastReader reader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
		this.m_cluster = Cluster.Load(reader);
		ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
		this.m_cluster.LoadClusterSim(pooledList, reader);
		SaveGame.GameInfo gameInfo = this.GameInfo;
		gameInfo.clusterId = this.m_cluster.Id;
		gameInfo.colonyGuid = Guid.NewGuid();
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		gameInfo.dlcIds = new List<string>(currentClusterLayout.requiredDlcIds);
		foreach (string item in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			if (!gameInfo.dlcIds.Contains(item))
			{
				gameInfo.dlcIds.Add(item);
			}
		}
		this.GameInfo = gameInfo;
		ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
		if (pooledList.Count != this.m_cluster.worlds.Count)
		{
			global::Debug.LogError("Attempt failed. Failed to load all worlds.");
			pooledList.Recycle();
			return false;
		}
		GridSettings.Reset(this.m_cluster.size.x, this.m_cluster.size.y);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		this.clusterDetailSave = new WorldDetailSave();
		foreach (SimSaveFileStructure simSaveFileStructure in pooledList)
		{
			this.clusterDetailSave.globalNoiseSeed = simSaveFileStructure.worldDetail.globalNoiseSeed;
			this.clusterDetailSave.globalTerrainSeed = simSaveFileStructure.worldDetail.globalTerrainSeed;
			this.clusterDetailSave.globalWorldLayoutSeed = simSaveFileStructure.worldDetail.globalWorldLayoutSeed;
			this.clusterDetailSave.globalWorldSeed = simSaveFileStructure.worldDetail.globalWorldSeed;
			Vector2 b = Grid.CellToPos2D(Grid.PosToCell(new Vector2I(simSaveFileStructure.x, simSaveFileStructure.y)));
			foreach (WorldDetailSave.OverworldCell overworldCell in simSaveFileStructure.worldDetail.overworldCells)
			{
				for (int num = 0; num != overworldCell.poly.Vertices.Count; num++)
				{
					List<Vector2> vertices = overworldCell.poly.Vertices;
					int index = num;
					vertices[index] += b;
				}
				overworldCell.poly.RefreshBounds();
			}
			this.clusterDetailSave.overworldCells.AddRange(simSaveFileStructure.worldDetail.overworldCells);
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(this.m_cluster.size.x, this.m_cluster.size.y, false);
		SimMessages.DefineWorldOffsets((from world in this.m_cluster.worlds
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = world.WorldOffset.x,
			worldOffsetY = world.WorldOffset.y,
			worldSizeX = world.WorldSize.x,
			worldSizeY = world.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		try
		{
			foreach (SimSaveFileStructure simSaveFileStructure2 in pooledList)
			{
				FastReader reader2 = new FastReader(simSaveFileStructure2.Sim);
				if (Sim.Load(reader2) != 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"\n--- Error loading save ---\nSimDLL found bad data\n"
					});
					Sim.Shutdown();
					pooledList.Recycle();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace);
			Sim.Shutdown();
			pooledList.Recycle();
			return false;
		}
		global::Debug.Log("Attempt success");
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		SceneInitializer.Instance.NewSaveGamePrefab();
		this.cachedGSD = this.m_cluster.currentWorld.SpawnData;
		this.OnWorldGenComplete.Signal(this.m_cluster);
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "NewGame", true);
		StoryManager.Instance.InitialSaveSetup();
		ThreadedHttps<KleiMetrics>.Instance.IncrementGameCount();
		OniMetrics.SendEvent(OniMetrics.Event.NewSave, "New Save");
		pooledList.Recycle();
		return true;
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x060053B1 RID: 21425 RVA: 0x001E8624 File Offset: 0x001E6824
	// (set) Token: 0x060053B2 RID: 21426 RVA: 0x001E862C File Offset: 0x001E682C
	public GameSpawnData cachedGSD { get; private set; }

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x060053B3 RID: 21427 RVA: 0x001E8635 File Offset: 0x001E6835
	// (set) Token: 0x060053B4 RID: 21428 RVA: 0x001E863D File Offset: 0x001E683D
	public WorldDetailSave clusterDetailSave { get; private set; }

	// Token: 0x060053B5 RID: 21429 RVA: 0x001E8646 File Offset: 0x001E6846
	public void SetWorldDetail(WorldDetailSave worldDetail)
	{
		this.clusterDetailSave = worldDetail;
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x001E8650 File Offset: 0x001E6850
	private void ReportSaveMetrics(bool is_auto_save)
	{
		if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || this.saveManager == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[GameClock.NewCycleKey] = GameClock.Instance.GetCycle() + 1;
		dictionary["IsAutoSave"] = is_auto_save;
		dictionary["SavedPrefabs"] = this.GetSavedPrefabMetrics();
		dictionary["ResourcesAccessible"] = this.GetWorldInventoryMetrics();
		dictionary["MinionMetrics"] = this.GetMinionMetrics();
		dictionary["WorldMetrics"] = this.GetWorldMetrics();
		if (is_auto_save)
		{
			dictionary["DailyReport"] = this.GetDailyReportMetrics();
			dictionary["PerformanceMeasurements"] = this.GetPerformanceMeasurements();
			dictionary["AverageFrameTime"] = this.GetFrameTime();
		}
		dictionary["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
		dictionary["CustomMixingSettings"] = CustomGameSettings.Instance.GetSettingsForMixingMetrics();
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(dictionary, "ReportSaveMetrics");
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x001E876C File Offset: 0x001E696C
	private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
	{
		List<SaveLoader.MinionMetricsData> list = new List<SaveLoader.MinionMetricsData>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
				List<SaveLoader.MinionAttrFloatData> list2 = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
				foreach (AmountInstance amountInstance in amounts)
				{
					float value = amountInstance.value;
					if (!float.IsNaN(value) && !float.IsInfinity(value))
					{
						list2.Add(new SaveLoader.MinionAttrFloatData
						{
							Name = amountInstance.modifier.Id,
							Value = amountInstance.value
						});
					}
				}
				MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
				float totalExperienceGained = component.TotalExperienceGained;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
				{
					if (keyValuePair.Value)
					{
						list3.Add(keyValuePair.Key);
					}
				}
				list.Add(new SaveLoader.MinionMetricsData
				{
					Name = minionIdentity.name,
					Modifiers = list2,
					TotalExperienceGained = totalExperienceGained,
					Skills = list3
				});
			}
		}
		return list;
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001E893C File Offset: 0x001E6B3C
	private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
	{
		Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
		List<SaveLoader.SavedPrefabMetricsData> list = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
		{
			Tag key = keyValuePair.Key;
			List<SaveLoadRoot> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				list.Add(new SaveLoader.SavedPrefabMetricsData
				{
					PrefabName = key.ToString(),
					Count = value.Count
				});
			}
		}
		return list;
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x001E89E8 File Offset: 0x001E6BE8
	private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
	{
		Dictionary<Tag, float> allWorldsAccessibleAmounts = ClusterManager.Instance.GetAllWorldsAccessibleAmounts();
		List<SaveLoader.WorldInventoryMetricsData> list = new List<SaveLoader.WorldInventoryMetricsData>(allWorldsAccessibleAmounts.Count);
		foreach (KeyValuePair<Tag, float> keyValuePair in allWorldsAccessibleAmounts)
		{
			float value = keyValuePair.Value;
			if (!float.IsInfinity(value) && !float.IsNaN(value))
			{
				list.Add(new SaveLoader.WorldInventoryMetricsData
				{
					Name = keyValuePair.Key.ToString(),
					Amount = value
				});
			}
		}
		return list;
	}

	// Token: 0x060053BA RID: 21434 RVA: 0x001E8A94 File Offset: 0x001E6C94
	private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
	{
		List<SaveLoader.DailyReportMetricsData> list = new List<SaveLoader.DailyReportMetricsData>();
		int cycle = GameClock.Instance.GetCycle();
		ReportManager.DailyReport dailyReport = ReportManager.Instance.FindReport(cycle);
		if (dailyReport != null)
		{
			foreach (ReportManager.ReportEntry reportEntry in dailyReport.reportEntries)
			{
				SaveLoader.DailyReportMetricsData item = default(SaveLoader.DailyReportMetricsData);
				item.Name = reportEntry.reportType.ToString();
				if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
				{
					item.Net = new float?(reportEntry.Net);
				}
				if (SaveLoader.force_infinity)
				{
					item.Net = null;
				}
				if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
				{
					item.Positive = new float?(reportEntry.Positive);
				}
				if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
				{
					item.Negative = new float?(reportEntry.Negative);
				}
				list.Add(item);
			}
			list.Add(new SaveLoader.DailyReportMetricsData
			{
				Name = "MinionCount",
				Net = new float?((float)Components.LiveMinionIdentities.Count),
				Positive = new float?(0f),
				Negative = new float?(0f)
			});
		}
		return list;
	}

	// Token: 0x060053BB RID: 21435 RVA: 0x001E8C2C File Offset: 0x001E6E2C
	private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
	{
		List<SaveLoader.PerformanceMeasurement> list = new List<SaveLoader.PerformanceMeasurement>();
		if (Global.Instance != null)
		{
			PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesAbove30",
				value = component.NumFramesAbove30
			});
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesBelow30",
				value = component.NumFramesBelow30
			});
			component.Reset();
		}
		return list;
	}

	// Token: 0x060053BC RID: 21436 RVA: 0x001E8CB4 File Offset: 0x001E6EB4
	public float GetFrameTime()
	{
		PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
		DebugUtil.LogArgs(new object[]
		{
			"Average frame time:",
			1f / component.FPS
		});
		return 1f / component.FPS;
	}

	// Token: 0x060053BD RID: 21437 RVA: 0x001E8D00 File Offset: 0x001E6F00
	private List<SaveLoader.WorldMetricsData> GetWorldMetrics()
	{
		List<SaveLoader.WorldMetricsData> list = new List<SaveLoader.WorldMetricsData>();
		if (Global.Instance != null)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					float discoveryTimestamp = worldContainer.IsDiscovered ? worldContainer.DiscoveryTimestamp : -1f;
					float dupeVisitedTimestamp = worldContainer.IsDupeVisited ? worldContainer.DupeVisitedTimestamp : -1f;
					list.Add(new SaveLoader.WorldMetricsData
					{
						Name = worldContainer.worldName,
						DiscoveryTimestamp = discoveryTimestamp,
						DupeVisitedTimestamp = dupeVisitedTimestamp
					});
				}
			}
		}
		return list;
	}

	// Token: 0x060053BE RID: 21438 RVA: 0x001E8DCC File Offset: 0x001E6FCC
	[Obsolete("Use Game.IsDlcActiveForCurrentSave instead")]
	public bool IsDLCActiveForCurrentSave(string dlcid)
	{
		return DlcManager.IsContentSubscribed(dlcid) && (dlcid == "" || dlcid == "" || this.GameInfo.dlcIds.Contains(dlcid));
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x001E8E08 File Offset: 0x001E7008
	[Obsolete("Use Game methods instead")]
	public bool IsDlcListActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (text == "")
			{
				return true;
			}
			if (Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x001E8E4C File Offset: 0x001E704C
	[Obsolete("Use Game methods instead")]
	public bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !Game.IsDlcActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060053C1 RID: 21441 RVA: 0x001E8E90 File Offset: 0x001E7090
	[Obsolete("Use Game methods instead")]
	public bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060053C2 RID: 21442 RVA: 0x001E8ED2 File Offset: 0x001E70D2
	[Obsolete("Use Game's version")]
	public bool IsCorrectDlcActiveForCurrentSave(string[] required, string[] forbidden)
	{
		return this.IsAllDlcActiveForCurrentSave(required) && !this.IsAnyDlcActiveForCurrentSave(forbidden);
	}

	// Token: 0x060053C3 RID: 21443 RVA: 0x001E8EEC File Offset: 0x001E70EC
	public string GetSaveLoadContentLetters()
	{
		if (this.GameInfo.dlcIds.Count <= 0)
		{
			return "V";
		}
		string text = "";
		foreach (string dlcId in this.GameInfo.dlcIds)
		{
			text += DlcManager.GetContentLetter(dlcId);
		}
		return text;
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x001E8F6C File Offset: 0x001E716C
	public void UpgradeActiveSaveDLCInfo(string dlcId, bool trigger_load = false)
	{
		string activeSaveFolder = SaveLoader.GetActiveSaveFolder();
		string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
		string filename = System.IO.Path.Combine(activeSaveFolder, path);
		this.Save(filename, false, false);
		if (!this.GameInfo.dlcIds.Contains(dlcId))
		{
			this.GameInfo.dlcIds.Add(dlcId);
		}
		string current_save = SaveLoader.GetActiveSaveFilePath();
		this.Save(SaveLoader.GetActiveSaveFilePath(), false, false);
		if (trigger_load)
		{
			LoadingOverlay.Load(delegate
			{
				LoadScreen.DoLoad(current_save);
			});
		}
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x001E9005 File Offset: 0x001E7205
	public static void LoadScene()
	{
		PerformanceCaptureMonitor.StartLoadingSave();
		App.LoadScene("backend");
	}

	// Token: 0x04003885 RID: 14469
	[MyCmpGet]
	private GridSettings gridSettings;

	// Token: 0x04003887 RID: 14471
	private bool saveFileCorrupt;

	// Token: 0x04003888 RID: 14472
	private bool compressSaveData = true;

	// Token: 0x04003889 RID: 14473
	private int lastUncompressedSize;

	// Token: 0x0400388A RID: 14474
	public bool saveAsText;

	// Token: 0x0400388B RID: 14475
	public const string MAINMENU_LEVELNAME = "launchscene";

	// Token: 0x0400388C RID: 14476
	public const string FRONTEND_LEVELNAME = "frontend";

	// Token: 0x0400388D RID: 14477
	public const string BACKEND_LEVELNAME = "backend";

	// Token: 0x0400388E RID: 14478
	public const string SAVE_EXTENSION = ".sav";

	// Token: 0x0400388F RID: 14479
	public const string AUTOSAVE_FOLDER = "auto_save";

	// Token: 0x04003890 RID: 14480
	public const string CLOUDSAVE_FOLDER = "cloud_save_files";

	// Token: 0x04003891 RID: 14481
	public const string SAVE_FOLDER = "save_files";

	// Token: 0x04003892 RID: 14482
	public const int MAX_AUTOSAVE_FILES = 10;

	// Token: 0x04003894 RID: 14484
	[NonSerialized]
	public SaveManager saveManager;

	// Token: 0x04003896 RID: 14486
	private Cluster m_cluster;

	// Token: 0x04003897 RID: 14487
	private ClusterLayout m_clusterLayout;

	// Token: 0x04003899 RID: 14489
	private const string CorruptFileSuffix = "_";

	// Token: 0x0400389A RID: 14490
	private const float SAVE_BUFFER_HEAD_ROOM = 0.1f;

	// Token: 0x0400389B RID: 14491
	private bool mustRestartOnFail;

	// Token: 0x0400389E RID: 14494
	public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";

	// Token: 0x0400389F RID: 14495
	public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";

	// Token: 0x040038A0 RID: 14496
	public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";

	// Token: 0x040038A1 RID: 14497
	public const string METRIC_IS_SANDBOX_ENABLED = "IsSandboxEnabled";

	// Token: 0x040038A2 RID: 14498
	public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";

	// Token: 0x040038A3 RID: 14499
	public const string METRIC_DAILY_REPORT_KEY = "DailyReport";

	// Token: 0x040038A4 RID: 14500
	public const string METRIC_WORLD_METRICS_KEY = "WorldMetrics";

	// Token: 0x040038A5 RID: 14501
	public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";

	// Token: 0x040038A6 RID: 14502
	public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";

	// Token: 0x040038A7 RID: 14503
	public const string METRIC_CUSTOM_MIXING_SETTINGS = "CustomMixingSettings";

	// Token: 0x040038A8 RID: 14504
	public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";

	// Token: 0x040038A9 RID: 14505
	public const string METRIC_FRAME_TIME = "AverageFrameTime";

	// Token: 0x040038AA RID: 14506
	private static bool force_infinity;

	// Token: 0x02001C77 RID: 7287
	public class FlowUtilityNetworkInstance
	{
		// Token: 0x0400883A RID: 34874
		public int id = -1;

		// Token: 0x0400883B RID: 34875
		public SimHashes containedElement = SimHashes.Vacuum;

		// Token: 0x0400883C RID: 34876
		public float containedMass;

		// Token: 0x0400883D RID: 34877
		public float containedTemperature;
	}

	// Token: 0x02001C78 RID: 7288
	[SerializationConfig(KSerialization.MemberSerialization.OptOut)]
	public class FlowUtilityNetworkSaver : ISaveLoadable
	{
		// Token: 0x0600ADBE RID: 44478 RVA: 0x003D13C2 File Offset: 0x003CF5C2
		public FlowUtilityNetworkSaver()
		{
			this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
			this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
		}

		// Token: 0x0400883E RID: 34878
		public List<SaveLoader.FlowUtilityNetworkInstance> gas;

		// Token: 0x0400883F RID: 34879
		public List<SaveLoader.FlowUtilityNetworkInstance> liquid;
	}

	// Token: 0x02001C79 RID: 7289
	public struct SaveFileEntry
	{
		// Token: 0x04008840 RID: 34880
		public string path;

		// Token: 0x04008841 RID: 34881
		public System.DateTime timeStamp;
	}

	// Token: 0x02001C7A RID: 7290
	public enum SaveType
	{
		// Token: 0x04008843 RID: 34883
		local,
		// Token: 0x04008844 RID: 34884
		cloud,
		// Token: 0x04008845 RID: 34885
		both
	}

	// Token: 0x02001C7B RID: 7291
	private struct MinionAttrFloatData
	{
		// Token: 0x04008846 RID: 34886
		public string Name;

		// Token: 0x04008847 RID: 34887
		public float Value;
	}

	// Token: 0x02001C7C RID: 7292
	private struct MinionMetricsData
	{
		// Token: 0x04008848 RID: 34888
		public string Name;

		// Token: 0x04008849 RID: 34889
		public List<SaveLoader.MinionAttrFloatData> Modifiers;

		// Token: 0x0400884A RID: 34890
		public float TotalExperienceGained;

		// Token: 0x0400884B RID: 34891
		public List<string> Skills;
	}

	// Token: 0x02001C7D RID: 7293
	private struct SavedPrefabMetricsData
	{
		// Token: 0x0400884C RID: 34892
		public string PrefabName;

		// Token: 0x0400884D RID: 34893
		public int Count;
	}

	// Token: 0x02001C7E RID: 7294
	private struct WorldInventoryMetricsData
	{
		// Token: 0x0400884E RID: 34894
		public string Name;

		// Token: 0x0400884F RID: 34895
		public float Amount;
	}

	// Token: 0x02001C7F RID: 7295
	private struct DailyReportMetricsData
	{
		// Token: 0x04008850 RID: 34896
		public string Name;

		// Token: 0x04008851 RID: 34897
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Net;

		// Token: 0x04008852 RID: 34898
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Positive;

		// Token: 0x04008853 RID: 34899
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Negative;
	}

	// Token: 0x02001C80 RID: 7296
	private struct PerformanceMeasurement
	{
		// Token: 0x04008854 RID: 34900
		public string name;

		// Token: 0x04008855 RID: 34901
		public float value;
	}

	// Token: 0x02001C81 RID: 7297
	private struct WorldMetricsData
	{
		// Token: 0x04008856 RID: 34902
		public string Name;

		// Token: 0x04008857 RID: 34903
		public float DiscoveryTimestamp;

		// Token: 0x04008858 RID: 34904
		public float DupeVisitedTimestamp;
	}
}
