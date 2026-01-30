using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Klei;
using Newtonsoft.Json;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000FC4 RID: 4036
	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{title}")]
	public class Mod : IHasDlcRestrictions
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06007E8D RID: 32397 RVA: 0x00327A75 File Offset: 0x00325C75
		// (set) Token: 0x06007E8E RID: 32398 RVA: 0x00327A7D File Offset: 0x00325C7D
		public Content available_content { get; private set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06007E8F RID: 32399 RVA: 0x00327A86 File Offset: 0x00325C86
		// (set) Token: 0x06007E90 RID: 32400 RVA: 0x00327A8E File Offset: 0x00325C8E
		[JsonProperty]
		public string staticID { get; private set; }

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06007E91 RID: 32401 RVA: 0x00327A97 File Offset: 0x00325C97
		// (set) Token: 0x06007E92 RID: 32402 RVA: 0x00327A9F File Offset: 0x00325C9F
		public LocString manage_tooltip { get; private set; }

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06007E93 RID: 32403 RVA: 0x00327AA8 File Offset: 0x00325CA8
		// (set) Token: 0x06007E94 RID: 32404 RVA: 0x00327AB0 File Offset: 0x00325CB0
		public System.Action on_managed { get; private set; }

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06007E95 RID: 32405 RVA: 0x00327AB9 File Offset: 0x00325CB9
		public bool is_managed
		{
			get
			{
				return this.manage_tooltip != null;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06007E96 RID: 32406 RVA: 0x00327AC4 File Offset: 0x00325CC4
		// (set) Token: 0x06007E97 RID: 32407 RVA: 0x00327AD1 File Offset: 0x00325CD1
		public string title
		{
			get
			{
				return this.label.title;
			}
			set
			{
				this.label.title = value;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06007E98 RID: 32408 RVA: 0x00327ADF File Offset: 0x00325CDF
		// (set) Token: 0x06007E99 RID: 32409 RVA: 0x00327AE7 File Offset: 0x00325CE7
		public string description { get; set; }

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06007E9A RID: 32410 RVA: 0x00327AF0 File Offset: 0x00325CF0
		// (set) Token: 0x06007E9B RID: 32411 RVA: 0x00327AF8 File Offset: 0x00325CF8
		public Content loaded_content { get; private set; }

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06007E9C RID: 32412 RVA: 0x00327B01 File Offset: 0x00325D01
		// (set) Token: 0x06007E9D RID: 32413 RVA: 0x00327B09 File Offset: 0x00325D09
		public IFileSource file_source
		{
			get
			{
				return this._fileSource;
			}
			set
			{
				if (this._fileSource != null)
				{
					this._fileSource.Dispose();
				}
				this._fileSource = value;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06007E9E RID: 32414 RVA: 0x00327B25 File Offset: 0x00325D25
		// (set) Token: 0x06007E9F RID: 32415 RVA: 0x00327B2D File Offset: 0x00325D2D
		public bool DevModCrashTriggered { get; private set; }

		// Token: 0x06007EA0 RID: 32416 RVA: 0x00327B36 File Offset: 0x00325D36
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x00327B3E File Offset: 0x00325D3E
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x00327B46 File Offset: 0x00325D46
		[JsonConstructor]
		public Mod()
		{
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x00327B5C File Offset: 0x00325D5C
		public void CopyPersistentDataTo(Mod other_mod)
		{
			other_mod.status = this.status;
			other_mod.enabledForDlc = ((this.enabledForDlc != null) ? new List<string>(this.enabledForDlc) : new List<string>());
			other_mod.crash_count = this.crash_count;
			other_mod.loaded_content = this.loaded_content;
			other_mod.loaded_mod_data = this.loaded_mod_data;
			other_mod.reinstall_path = this.reinstall_path;
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x00327BC8 File Offset: 0x00325DC8
		public Mod(Label label, string staticID, string description, IFileSource file_source, LocString manage_tooltip, System.Action on_managed)
		{
			this.label = label;
			this.status = Mod.Status.NotInstalled;
			this.staticID = staticID;
			this.description = description;
			this.file_source = file_source;
			this.manage_tooltip = manage_tooltip;
			this.on_managed = on_managed;
			this.loaded_content = (Content)0;
			this.available_content = (Content)0;
			this.ScanContent();
		}

		// Token: 0x06007EA5 RID: 32421 RVA: 0x00327C2E File Offset: 0x00325E2E
		public bool IsEnabledForActiveDlc()
		{
			return this.IsEnabledForDlc(DlcManager.GetHighestActiveDlcId());
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x00327C3B File Offset: 0x00325E3B
		public bool IsEnabledForDlc(string dlcId)
		{
			return this.enabledForDlc != null && this.enabledForDlc.Contains(dlcId);
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x00327C53 File Offset: 0x00325E53
		public void SetEnabledForActiveDlc(bool enabled)
		{
			this.SetEnabledForDlc(DlcManager.GetHighestActiveDlcId(), enabled);
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x00327C64 File Offset: 0x00325E64
		public void SetEnabledForDlc(string dlcId, bool set_enabled)
		{
			if (this.enabledForDlc == null)
			{
				this.enabledForDlc = new List<string>();
			}
			bool flag = this.enabledForDlc.Contains(dlcId);
			if (set_enabled && !flag)
			{
				this.enabledForDlc.Add(dlcId);
				return;
			}
			if (!set_enabled && flag)
			{
				this.enabledForDlc.Remove(dlcId);
			}
		}

		// Token: 0x06007EA9 RID: 32425 RVA: 0x00327CBC File Offset: 0x00325EBC
		public void ScanContent()
		{
			this.ModDevLog(string.Format("{0} ({1}): Setting up mod.", this.label, this.label.id));
			this.available_content = (Content)0;
			if (this.file_source == null)
			{
				if (this.label.id.EndsWith(".zip"))
				{
					DebugUtil.DevAssert(false, "Does this actually get used ever?", null);
					this.file_source = new ZipFile(this.label.install_path);
				}
				else
				{
					this.file_source = new Directory(this.label.install_path);
				}
			}
			if (!this.file_source.Exists())
			{
				global::Debug.LogWarning(string.Format("{0}: File source does not appear to be valid, skipping. ({1})", this.label, this.label.install_path));
				return;
			}
			KModHeader header = KModUtil.GetHeader(this.file_source, this.label.defaultStaticID, this.label.title, this.description, this.IsDev);
			if (this.label.title != header.title)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with the title `",
					header.title,
					"`, using that from now on."
				}));
			}
			if (this.label.defaultStaticID != header.staticID)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with a staticID `",
					header.staticID,
					"`, using that from now on."
				}));
			}
			this.label.title = header.title;
			this.staticID = header.staticID;
			this.description = header.description;
			Mod.ArchivedVersion mostSuitableArchive = this.GetMostSuitableArchive();
			if (mostSuitableArchive == null)
			{
				global::Debug.LogWarning(string.Format("{0}: No archive supports this game version, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.DoesntSupportDLCConfig;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.packagedModInfo = mostSuitableArchive.info;
			Content content;
			this.ScanContentFromSource(mostSuitableArchive.relativePath, out content);
			if (content == (Content)0)
			{
				global::Debug.LogWarning(string.Format("{0}: No supported content for mod, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.NoContent;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			bool flag = mostSuitableArchive.info.APIVersion == 2;
			if ((content & Content.DLL) != (Content)0 && !flag)
			{
				global::Debug.LogWarning(string.Format("{0}: DLLs found but not using the correct API version.", this.label));
				this.contentCompatability = ModContentCompatability.OldAPI;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.contentCompatability = ModContentCompatability.OK;
			this.available_content = content;
			this.relative_root = mostSuitableArchive.relativePath;
			global::Debug.Assert(this.content_source == null);
			this.content_source = new Directory(this.ContentPath);
			string arg = string.IsNullOrEmpty(this.relative_root) ? "root" : this.relative_root;
			global::Debug.Log(string.Format("{0}: Successfully loaded from path '{1}' with content '{2}'.", this.label, arg, this.available_content.ToString()));
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x00327FEC File Offset: 0x003261EC
		private Mod.ArchivedVersion GetMostSuitableArchive()
		{
			Mod.PackagedModInfo packagedModInfo = this.GetModInfoForFolder("");
			if (packagedModInfo == null)
			{
				if (!this.ScanContentFromSourceForTranslationsOnly(""))
				{
					global::Debug.Log(string.Format("{0}: Is missing a mod_info.yaml file and will not be loaded, which is required. See the stickied post in the Mods and Tools section on the Klei forums.", this.label));
					return null;
				}
				this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, but since it's a translation we will load it.", this.label));
				packagedModInfo = new Mod.PackagedModInfo
				{
					minimumSupportedBuild = 0
				};
			}
			this.requiredDlcIds = packagedModInfo.requiredDlcIds;
			this.forbiddenDlcIds = packagedModInfo.forbiddenDlcIds;
			Mod.ArchivedVersion archivedVersion = new Mod.ArchivedVersion
			{
				relativePath = "",
				info = packagedModInfo
			};
			if (!this.file_source.Exists("archived_versions"))
			{
				this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
				if (!DlcManager.IsCorrectDlcSubscribed(packagedModInfo))
				{
					return null;
				}
				return archivedVersion;
			}
			else
			{
				List<FileSystemItem> list = new List<FileSystemItem>();
				this.file_source.GetTopLevelItems(list, "archived_versions");
				if (list.Count == 0)
				{
					this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
					if (!DlcManager.IsCorrectDlcSubscribed(packagedModInfo))
					{
						return null;
					}
					return archivedVersion;
				}
				else
				{
					List<Mod.ArchivedVersion> list2 = new List<Mod.ArchivedVersion>();
					list2.Add(archivedVersion);
					foreach (FileSystemItem fileSystemItem in list)
					{
						if (fileSystemItem.type != FileSystemItem.ItemType.File)
						{
							string relativePath = Path.Combine("archived_versions", fileSystemItem.name);
							Mod.PackagedModInfo modInfoForFolder = this.GetModInfoForFolder(relativePath);
							if (modInfoForFolder != null)
							{
								list2.Add(new Mod.ArchivedVersion
								{
									relativePath = relativePath,
									info = modInfoForFolder
								});
							}
						}
					}
					list2 = (from v in list2
					where DlcManager.IsCorrectDlcSubscribed(v.info)
					select v).ToList<Mod.ArchivedVersion>();
					list2 = (from v in list2
					where v.info.APIVersion == 2 || v.info.APIVersion == 0
					select v).ToList<Mod.ArchivedVersion>();
					Mod.ArchivedVersion archivedVersion2 = (from v in list2
					where (long)v.info.minimumSupportedBuild <= 706793L
					orderby v.info.minimumSupportedBuild descending
					select v).FirstOrDefault<Mod.ArchivedVersion>();
					if (archivedVersion2 != null)
					{
						this.requiredDlcIds = archivedVersion2.info.requiredDlcIds;
						this.forbiddenDlcIds = archivedVersion2.info.forbiddenDlcIds;
					}
					if (archivedVersion2 == null)
					{
						return null;
					}
					return archivedVersion2;
				}
			}
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x00328270 File Offset: 0x00326470
		private Mod.PackagedModInfo GetModInfoForFolder(string relative_root)
		{
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relative_root);
			bool flag = false;
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower() == "mod_info.yaml")
				{
					flag = true;
					break;
				}
			}
			string text = string.IsNullOrEmpty(relative_root) ? "root" : relative_root;
			if (!flag)
			{
				this.ModDevLogWarning(string.Concat(new string[]
				{
					"\t",
					this.title,
					": has no mod_info.yaml in folder '",
					text,
					"'"
				}));
				return null;
			}
			string text2 = this.file_source.Read(Path.Combine(relative_root, "mod_info.yaml"));
			if (string.IsNullOrEmpty(text2))
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to read {1} in folder '{2}', skipping", this.label, "mod_info.yaml", text));
				return null;
			}
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !this.IsDev);
			};
			Mod.PackagedModInfo packagedModInfo = YamlIO.Parse<Mod.PackagedModInfo>(text2, default(FileHandle), handle_error, null);
			if (packagedModInfo == null)
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to parse {1} in folder '{2}', text is {3}", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					text2
				}));
				return null;
			}
			if (packagedModInfo.supportedContent != null && packagedModInfo.requiredDlcIds == null && packagedModInfo.forbiddenDlcIds == null)
			{
				packagedModInfo.supportedContent = packagedModInfo.supportedContent.ToUpperInvariant();
				this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using supportedContent which has been deprecated. See stickied post on the Klei forums.", this.label, "mod_info.yaml", text));
				bool flag2 = packagedModInfo.supportedContent.Contains("ALL");
				bool flag3 = packagedModInfo.supportedContent.Contains("VANILLA_ID");
				bool flag4 = packagedModInfo.supportedContent.Contains("EXPANSION1_ID");
				if (flag2)
				{
					packagedModInfo.requiredDlcIds = null;
					packagedModInfo.forbiddenDlcIds = null;
				}
				else
				{
					string pattern = "\\b\\w+_ID\\b";
					List<string> list2 = new List<string>();
					foreach (object obj in Regex.Matches(packagedModInfo.supportedContent, pattern))
					{
						Match match = (Match)obj;
						if (!(match.Value == "VANILLA_ID") && (!(match.Value == "EXPANSION1_ID") || !flag3))
						{
							if (match.Value != "EXPANSION1_ID")
							{
								this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' found a DLC '{3}' it didn't recognize, ignoring.", new object[]
								{
									this.label,
									"mod_info.yaml",
									text,
									match.Value
								}));
							}
							else
							{
								list2.Add(match.Value);
							}
						}
					}
					if (list2.Count > 0)
					{
						packagedModInfo.requiredDlcIds = list2.ToArray();
					}
					if (!flag4)
					{
						packagedModInfo.forbiddenDlcIds = DlcManager.EXPANSION1;
					}
				}
			}
			if (packagedModInfo.requiredDlcIds != null)
			{
				for (int i = 0; i < packagedModInfo.requiredDlcIds.Length; i++)
				{
					packagedModInfo.requiredDlcIds[i] = packagedModInfo.requiredDlcIds[i].ToUpperInvariant();
					if (!DlcManager.IsDlcId(packagedModInfo.requiredDlcIds[i]))
					{
						this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using an unrecognized DLC in requiredDlcIds '{3}'", new object[]
						{
							this.label,
							"mod_info.yaml",
							text,
							packagedModInfo.requiredDlcIds[i]
						}));
					}
				}
			}
			if (packagedModInfo.forbiddenDlcIds != null)
			{
				for (int j = 0; j < packagedModInfo.forbiddenDlcIds.Length; j++)
				{
					packagedModInfo.forbiddenDlcIds[j] = packagedModInfo.forbiddenDlcIds[j].ToUpperInvariant();
					if (!DlcManager.IsDlcId(packagedModInfo.forbiddenDlcIds[j]))
					{
						this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using an unrecognized DLC in forbiddenDlcIds '{3}'", new object[]
						{
							this.label,
							"mod_info.yaml",
							text,
							packagedModInfo.forbiddenDlcIds[j]
						}));
					}
				}
			}
			if (packagedModInfo.lastWorkingBuild != 0)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' is using `{3}`, please upgrade this to `{4}`", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					"lastWorkingBuild",
					"minimumSupportedBuild"
				}));
				if (packagedModInfo.minimumSupportedBuild == 0)
				{
					packagedModInfo.minimumSupportedBuild = packagedModInfo.lastWorkingBuild;
				}
			}
			this.ModDevLog(string.Format("\t{0}: Found valid mod_info.yaml in folder '{1}': requiredDlcIds='{2}', forbiddenDlcIds='{3}' at {4}", new object[]
			{
				this.label,
				text,
				packagedModInfo.requiredDlcIds.DebugToCommaSeparatedList(),
				packagedModInfo.forbiddenDlcIds.DebugToCommaSeparatedList(),
				packagedModInfo.minimumSupportedBuild
			}));
			return packagedModInfo;
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x00328760 File Offset: 0x00326960
		private bool ScanContentFromSource(string relativeRoot, out Content available)
		{
			available = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
				{
					string directory = fileSystemItem.name.ToLower();
					available |= this.AddDirectory(directory);
				}
				else
				{
					string file = fileSystemItem.name.ToLower();
					available |= this.AddFile(file);
				}
			}
			return available > (Content)0;
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x00328800 File Offset: 0x00326A00
		private bool ScanContentFromSourceForTranslationsOnly(string relativeRoot)
		{
			this.available_content = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower().EndsWith(".po"))
				{
					this.available_content |= Content.Translation;
				}
			}
			return this.available_content > (Content)0;
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06007EAE RID: 32430 RVA: 0x00328898 File Offset: 0x00326A98
		public string ContentPath
		{
			get
			{
				return Path.Combine(this.label.install_path, this.relative_root);
			}
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x003288B0 File Offset: 0x00326AB0
		public bool IsEmpty()
		{
			return this.available_content == (Content)0;
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x003288BC File Offset: 0x00326ABC
		private Content AddDirectory(string directory)
		{
			Content content = (Content)0;
			string text = directory.TrimEnd('/');
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1519694028U)
			{
				if (num != 948591336U)
				{
					if (num != 1318520008U)
					{
						if (num == 1519694028U)
						{
							if (text == "elements")
							{
								content |= Content.LayerableFiles;
							}
						}
					}
					else if (text == "buildingfacades")
					{
						content |= Content.Animation;
					}
				}
				else if (text == "templates")
				{
					content |= Content.LayerableFiles;
				}
			}
			else if (num <= 3037049615U)
			{
				if (num != 2960291089U)
				{
					if (num == 3037049615U)
					{
						if (text == "worldgen")
						{
							content |= Content.LayerableFiles;
						}
					}
				}
				else if (text == "strings")
				{
					content |= Content.Strings;
				}
			}
			else if (num != 3319670096U)
			{
				if (num == 3570262116U)
				{
					if (text == "codex")
					{
						content |= Content.LayerableFiles;
					}
				}
			}
			else if (text == "anim")
			{
				content |= Content.Animation;
			}
			return content;
		}

		// Token: 0x06007EB1 RID: 32433 RVA: 0x003289CC File Offset: 0x00326BCC
		private Content AddFile(string file)
		{
			Content content = (Content)0;
			if (file.EndsWith(".dll"))
			{
				content |= Content.DLL;
			}
			if (file.EndsWith(".po"))
			{
				content |= Content.Translation;
			}
			return content;
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x003289FE File Offset: 0x00326BFE
		private static void AccumulateExtensions(Content content, List<string> extensions)
		{
			if ((content & Content.DLL) != (Content)0)
			{
				extensions.Add(".dll");
			}
			if ((content & (Content.Strings | Content.Translation)) != (Content)0)
			{
				extensions.Add(".po");
			}
		}

		// Token: 0x06007EB3 RID: 32435 RVA: 0x00328A24 File Offset: 0x00326C24
		[Conditional("DEBUG")]
		private void Assert(bool condition, string failure_message)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", this.title, this.label.ToString(), failure_message));
				return;
			}
			DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", this.label.ToString(), failure_message));
		}

		// Token: 0x06007EB4 RID: 32436 RVA: 0x00328A8C File Offset: 0x00326C8C
		public void Install()
		{
			if (this.IsLocal)
			{
				this.status = Mod.Status.Installed;
				return;
			}
			this.status = Mod.Status.ReinstallPending;
			if (this.file_source == null)
			{
				return;
			}
			if (!FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				return;
			}
			if (!FileUtil.CreateDirectory(this.label.install_path, 0))
			{
				return;
			}
			this.file_source.CopyTo(this.label.install_path, null);
			this.file_source = new Directory(this.label.install_path);
			this.status = Mod.Status.Installed;
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x00328B1C File Offset: 0x00326D1C
		public bool Uninstall()
		{
			this.SetEnabledForActiveDlc(false);
			if (this.loaded_content != (Content)0)
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: still has loaded content: {1}", this.label.ToString(), this.loaded_content.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: directory deletion failed", this.label.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			this.status = Mod.Status.NotInstalled;
			return true;
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x00328BC4 File Offset: 0x00326DC4
		private bool LoadStrings()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "strings"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				if (!(fileInfo.Extension.ToLower() != ".po"))
				{
					num++;
					Localization.OverloadStrings(Localization.LoadStringsFile(fileInfo.FullName, false));
				}
			}
			return true;
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x00328C41 File Offset: 0x00326E41
		private bool LoadTranslations()
		{
			return false;
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x00328C44 File Offset: 0x00326E44
		private bool LoadAnimation()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "anim"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				foreach (DirectoryInfo directoryInfo in directories[i].GetDirectories())
				{
					KAnimFile.Mod mod = new KAnimFile.Mod();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (!fileInfo.Name.StartsWith("._"))
						{
							if (fileInfo.Extension == ".png")
							{
								byte[] data = File.ReadAllBytes(fileInfo.FullName);
								Texture2D texture2D = new Texture2D(2, 2);
								texture2D.LoadImage(data);
								mod.textures.Add(texture2D);
							}
							else if (fileInfo.Extension == ".bytes")
							{
								string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
								byte[] array = File.ReadAllBytes(fileInfo.FullName);
								if (fileNameWithoutExtension.EndsWith("_anim"))
								{
									mod.anim = array;
								}
								else if (fileNameWithoutExtension.EndsWith("_build"))
								{
									mod.build = array;
								}
								else
								{
									DebugUtil.LogWarningArgs(new object[]
									{
										string.Format("Unhandled TextAsset ({0})...ignoring", fileInfo.FullName)
									});
								}
							}
							else
							{
								DebugUtil.LogWarningArgs(new object[]
								{
									string.Format("Unhandled asset ({0})...ignoring", fileInfo.FullName)
								});
							}
						}
					}
					string name = directoryInfo.Name + "_kanim";
					if (mod.IsValid() && ModUtil.AddKAnimMod(name, mod))
					{
						num++;
					}
				}
			}
			return true;
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x00328E1C File Offset: 0x0032701C
		public void Load(Content content)
		{
			content &= (this.available_content & ~this.loaded_content);
			if (content > (Content)0)
			{
				global::Debug.Log(string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", new object[]
				{
					this.title,
					this.label.id,
					content.ToString(),
					this.available_content.ToString()
				}));
			}
			if ((content & Content.Strings) != (Content)0 && this.LoadStrings())
			{
				this.loaded_content |= Content.Strings;
			}
			if ((content & Content.Translation) != (Content)0 && this.LoadTranslations())
			{
				this.loaded_content |= Content.Translation;
			}
			if ((content & Content.DLL) != (Content)0)
			{
				this.loaded_mod_data = DLLLoader.LoadDLLs(this, this.staticID, this.ContentPath, this.IsDev);
				if (this.loaded_mod_data != null)
				{
					this.loaded_content |= Content.DLL;
				}
			}
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				global::Debug.Assert(this.content_source != null, "Attempting to Load layerable files with content_source not initialized");
				FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
				this.loaded_content |= Content.LayerableFiles;
			}
			if ((content & Content.Animation) != (Content)0 && this.LoadAnimation())
			{
				this.loaded_content |= Content.Animation;
			}
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x00328F5B File Offset: 0x0032715B
		public void PostLoad(IReadOnlyList<Mod> mods)
		{
			if ((this.loaded_content & Content.DLL) != (Content)0 && this.loaded_mod_data != null)
			{
				DLLLoader.PostLoadDLLs(this.staticID, this.loaded_mod_data, mods);
			}
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x00328F81 File Offset: 0x00327181
		public void Unload(Content content)
		{
			content &= this.loaded_content;
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
				this.loaded_content &= ~Content.LayerableFiles;
			}
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x00328FBA File Offset: 0x003271BA
		private void SetCrashCount(int new_crash_count)
		{
			this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06007EBD RID: 32445 RVA: 0x00328FCA File Offset: 0x003271CA
		public bool IsDev
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06007EBE RID: 32446 RVA: 0x00328FDA File Offset: 0x003271DA
		public bool IsLocal
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;
			}
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x00328FFA File Offset: 0x003271FA
		public void SetCrashed()
		{
			this.SetCrashCount(this.crash_count + 1);
			if (!this.IsDev)
			{
				this.SetEnabledForActiveDlc(false);
			}
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x00329019 File Offset: 0x00327219
		public void Uncrash()
		{
			this.SetCrashCount(this.IsDev ? (this.crash_count - 1) : 0);
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x00329034 File Offset: 0x00327234
		public bool IsActive()
		{
			return this.loaded_content > (Content)0;
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x0032903F File Offset: 0x0032723F
		public bool AllActive(Content content)
		{
			return (this.loaded_content & content) == content;
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x0032904C File Offset: 0x0032724C
		public bool AllActive()
		{
			return (this.loaded_content & this.available_content) == this.available_content;
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x00329063 File Offset: 0x00327263
		public bool AnyActive(Content content)
		{
			return (this.loaded_content & content) > (Content)0;
		}

		// Token: 0x06007EC5 RID: 32453 RVA: 0x00329070 File Offset: 0x00327270
		public bool HasContent()
		{
			return this.available_content > (Content)0;
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x0032907B File Offset: 0x0032727B
		public bool HasAnyContent(Content content)
		{
			return (this.available_content & content) > (Content)0;
		}

		// Token: 0x06007EC7 RID: 32455 RVA: 0x00329088 File Offset: 0x00327288
		public bool HasOnlyTranslationContent()
		{
			return this.available_content == Content.Translation;
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x00329094 File Offset: 0x00327294
		public Texture2D GetPreviewImage()
		{
			string text = null;
			foreach (string text2 in Mod.PREVIEW_FILENAMES)
			{
				if (Directory.Exists(this.ContentPath) && File.Exists(Path.Combine(this.ContentPath, text2)))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = File.ReadAllBytes(Path.Combine(this.ContentPath, text));
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				result = texture2D;
			}
			catch
			{
				global::Debug.LogWarning(string.Format("Mod {0} seems to have a preview.png but it didn't load correctly.", this.label));
				result = null;
			}
			return result;
		}

		// Token: 0x06007EC9 RID: 32457 RVA: 0x00329160 File Offset: 0x00327360
		public void ModDevLog(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.Log(msg);
			}
		}

		// Token: 0x06007ECA RID: 32458 RVA: 0x00329170 File Offset: 0x00327370
		public void ModDevLogWarning(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.LogWarning(msg);
			}
		}

		// Token: 0x06007ECB RID: 32459 RVA: 0x00329180 File Offset: 0x00327380
		public void ModDevLogError(string msg)
		{
			if (this.IsDev)
			{
				this.DevModCrashTriggered = true;
				global::Debug.LogError(msg);
			}
		}

		// Token: 0x04005D8C RID: 23948
		public const int MOD_API_VERSION_NONE = 0;

		// Token: 0x04005D8D RID: 23949
		public const int MOD_API_VERSION_HARMONY1 = 1;

		// Token: 0x04005D8E RID: 23950
		public const int MOD_API_VERSION_HARMONY2 = 2;

		// Token: 0x04005D8F RID: 23951
		public const int MOD_API_VERSION = 2;

		// Token: 0x04005D90 RID: 23952
		[JsonProperty]
		public Label label;

		// Token: 0x04005D91 RID: 23953
		[JsonProperty]
		public Mod.Status status;

		// Token: 0x04005D92 RID: 23954
		[JsonProperty]
		public bool enabled;

		// Token: 0x04005D93 RID: 23955
		[JsonProperty]
		public List<string> enabledForDlc;

		// Token: 0x04005D95 RID: 23957
		[JsonProperty]
		public int crash_count;

		// Token: 0x04005D96 RID: 23958
		[JsonProperty]
		public string reinstall_path;

		// Token: 0x04005D98 RID: 23960
		public bool foundInStackTrace;

		// Token: 0x04005D99 RID: 23961
		public string relative_root = "";

		// Token: 0x04005D9A RID: 23962
		public Mod.PackagedModInfo packagedModInfo;

		// Token: 0x04005D9F RID: 23967
		public LoadedModData loaded_mod_data;

		// Token: 0x04005DA0 RID: 23968
		private IFileSource _fileSource;

		// Token: 0x04005DA1 RID: 23969
		public IFileSource content_source;

		// Token: 0x04005DA2 RID: 23970
		public bool is_subscribed;

		// Token: 0x04005DA4 RID: 23972
		private const string VANILLA_ID = "VANILLA_ID";

		// Token: 0x04005DA5 RID: 23973
		private const string ALL_ID = "ALL";

		// Token: 0x04005DA6 RID: 23974
		private const string ARCHIVED_VERSIONS_FOLDER = "archived_versions";

		// Token: 0x04005DA7 RID: 23975
		private const string MOD_INFO_FILENAME = "mod_info.yaml";

		// Token: 0x04005DA8 RID: 23976
		public ModContentCompatability contentCompatability;

		// Token: 0x04005DA9 RID: 23977
		public string[] requiredDlcIds;

		// Token: 0x04005DAA RID: 23978
		public string[] forbiddenDlcIds;

		// Token: 0x04005DAB RID: 23979
		public const int MAX_CRASH_COUNT = 3;

		// Token: 0x04005DAC RID: 23980
		private static readonly List<string> PREVIEW_FILENAMES = new List<string>
		{
			"preview.png",
			"Preview.png",
			"PREVIEW.PNG"
		};

		// Token: 0x020021C5 RID: 8645
		public enum Status
		{
			// Token: 0x04009B5C RID: 39772
			NotInstalled,
			// Token: 0x04009B5D RID: 39773
			Installed,
			// Token: 0x04009B5E RID: 39774
			UninstallPending,
			// Token: 0x04009B5F RID: 39775
			ReinstallPending
		}

		// Token: 0x020021C6 RID: 8646
		public class ArchivedVersion
		{
			// Token: 0x04009B60 RID: 39776
			public string relativePath;

			// Token: 0x04009B61 RID: 39777
			public Mod.PackagedModInfo info;
		}

		// Token: 0x020021C7 RID: 8647
		public class PackagedModInfo : IHasDlcRestrictions
		{
			// Token: 0x17000D49 RID: 3401
			// (get) Token: 0x0600BE05 RID: 48645 RVA: 0x004068C2 File Offset: 0x00404AC2
			// (set) Token: 0x0600BE06 RID: 48646 RVA: 0x004068CA File Offset: 0x00404ACA
			[Obsolete("Use IHasDlcRestrictions interface instead")]
			public string supportedContent { get; set; }

			// Token: 0x17000D4A RID: 3402
			// (get) Token: 0x0600BE07 RID: 48647 RVA: 0x004068D3 File Offset: 0x00404AD3
			// (set) Token: 0x0600BE08 RID: 48648 RVA: 0x004068DB File Offset: 0x00404ADB
			public string[] requiredDlcIds { get; set; }

			// Token: 0x17000D4B RID: 3403
			// (get) Token: 0x0600BE09 RID: 48649 RVA: 0x004068E4 File Offset: 0x00404AE4
			// (set) Token: 0x0600BE0A RID: 48650 RVA: 0x004068EC File Offset: 0x00404AEC
			public string[] forbiddenDlcIds { get; set; }

			// Token: 0x17000D4C RID: 3404
			// (get) Token: 0x0600BE0B RID: 48651 RVA: 0x004068F5 File Offset: 0x00404AF5
			// (set) Token: 0x0600BE0C RID: 48652 RVA: 0x004068FD File Offset: 0x00404AFD
			[Obsolete("Use minimumSupportedBuild instead!")]
			public int lastWorkingBuild { get; set; }

			// Token: 0x17000D4D RID: 3405
			// (get) Token: 0x0600BE0D RID: 48653 RVA: 0x00406906 File Offset: 0x00404B06
			// (set) Token: 0x0600BE0E RID: 48654 RVA: 0x0040690E File Offset: 0x00404B0E
			public int minimumSupportedBuild { get; set; }

			// Token: 0x17000D4E RID: 3406
			// (get) Token: 0x0600BE0F RID: 48655 RVA: 0x00406917 File Offset: 0x00404B17
			// (set) Token: 0x0600BE10 RID: 48656 RVA: 0x0040691F File Offset: 0x00404B1F
			public int APIVersion { get; set; }

			// Token: 0x17000D4F RID: 3407
			// (get) Token: 0x0600BE11 RID: 48657 RVA: 0x00406928 File Offset: 0x00404B28
			// (set) Token: 0x0600BE12 RID: 48658 RVA: 0x00406930 File Offset: 0x00404B30
			public string version { get; set; }

			// Token: 0x0600BE13 RID: 48659 RVA: 0x00406939 File Offset: 0x00404B39
			public string[] GetRequiredDlcIds()
			{
				return this.requiredDlcIds;
			}

			// Token: 0x0600BE14 RID: 48660 RVA: 0x00406941 File Offset: 0x00404B41
			public string[] GetForbiddenDlcIds()
			{
				return this.forbiddenDlcIds;
			}
		}
	}
}
