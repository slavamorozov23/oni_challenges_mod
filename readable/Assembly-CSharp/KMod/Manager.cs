using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Klei;
using Newtonsoft.Json;
using STRINGS;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000FC5 RID: 4037
	public class Manager
	{
		// Token: 0x06007ECE RID: 32462 RVA: 0x003291D5 File Offset: 0x003273D5
		public static string GetDirectory()
		{
			return Path.Combine(Util.RootFolder(), "mods/");
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x00329224 File Offset: 0x00327424
		public void LoadModDBAndInitialize()
		{
			string filename = this.GetFilename();
			try
			{
				if (FileUtil.FileExists(filename, 0))
				{
					Manager.PersistentData persistentData = JsonConvert.DeserializeObject<Manager.PersistentData>(File.ReadAllText(filename));
					this.mods = persistentData.mods;
				}
			}
			catch (Exception)
			{
				global::Debug.LogWarningFormat(UI.FRONTEND.MODS.DB_CORRUPT, new object[]
				{
					filename
				});
				this.mods = new List<Mod>();
			}
			foreach (Mod mod in this.mods)
			{
				if (mod.enabledForDlc == null)
				{
					mod.SetEnabledForDlc("", mod.enabled);
				}
			}
			List<Mod> list = new List<Mod>();
			bool flag = false;
			foreach (Mod mod2 in this.mods)
			{
				Mod.Status status = mod2.status;
				if (status != Mod.Status.UninstallPending)
				{
					if (status == Mod.Status.ReinstallPending)
					{
						global::Debug.LogFormat("Latent reinstall of mod {0}", new object[]
						{
							mod2.title
						});
						if (!string.IsNullOrEmpty(mod2.reinstall_path) && File.Exists(mod2.reinstall_path))
						{
							bool enabledForActiveDlc = mod2.IsEnabledForActiveDlc();
							mod2.file_source = new ZipFile(mod2.reinstall_path);
							mod2.SetEnabledForActiveDlc(false);
							if (mod2.Uninstall())
							{
								mod2.Install();
								if (mod2.status == Mod.Status.Installed)
								{
									mod2.SetEnabledForActiveDlc(enabledForActiveDlc);
								}
							}
							flag = true;
						}
						else if (mod2.IsEnabledForActiveDlc())
						{
							mod2.SetEnabledForActiveDlc(false);
							flag = true;
						}
					}
				}
				else
				{
					global::Debug.LogFormat("Latent uninstall of mod {0} from {1}", new object[]
					{
						mod2.title,
						mod2.label.install_path
					});
					if (mod2.Uninstall())
					{
						list.Add(mod2);
					}
					else
					{
						DebugUtil.Assert(mod2.status == Mod.Status.UninstallPending);
						global::Debug.LogFormat("\t...failed to uninstall mod {0}", new object[]
						{
							mod2.title
						});
					}
					if (mod2.status != Mod.Status.UninstallPending)
					{
						flag = true;
					}
				}
				if (!string.IsNullOrEmpty(mod2.reinstall_path))
				{
					mod2.reinstall_path = null;
					flag = true;
				}
			}
			foreach (Mod item in list)
			{
				this.mods.Remove(item);
			}
			foreach (Mod mod3 in this.mods)
			{
				mod3.ScanContent();
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x00329544 File Offset: 0x00327744
		public void Shutdown()
		{
			foreach (Mod mod in this.mods)
			{
				mod.Unload((Content)0);
			}
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x00329598 File Offset: 0x00327798
		public void Sanitize(GameObject parent)
		{
			ListPool<Label, Manager>.PooledList pooledList = ListPool<Label, Manager>.Allocate();
			foreach (Mod mod in this.mods)
			{
				if (!mod.is_subscribed)
				{
					pooledList.Add(mod.label);
				}
			}
			foreach (Label label in pooledList)
			{
				this.Unsubscribe(label, this);
			}
			pooledList.Recycle();
			this.Report(parent);
			this.WriteDevBootReport();
			if (GenericGameSettings.instance.devBootSmoke)
			{
				App.QuitCode(KCrashReporter.hasCrash ? 1 : 0);
			}
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x00329670 File Offset: 0x00327870
		public bool HaveMods()
		{
			foreach (Mod mod in this.mods)
			{
				if (mod.status == Mod.Status.Installed && mod.HasContent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007ED4 RID: 32468 RVA: 0x003296D4 File Offset: 0x003278D4
		public List<Mod> GetAllCrashableMods()
		{
			List<Mod> list = new List<Mod>();
			foreach (Mod mod in this.mods)
			{
				if (mod.DevModCrashTriggered || (mod.status != Mod.Status.NotInstalled && mod.IsActive() && !mod.HasOnlyTranslationContent()))
				{
					list.Add(mod);
				}
			}
			return list;
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x00329750 File Offset: 0x00327950
		public bool HasCrashableMods()
		{
			return this.GetAllCrashableMods().Count > 0;
		}

		// Token: 0x06007ED6 RID: 32470 RVA: 0x00329760 File Offset: 0x00327960
		private void Install(Mod mod)
		{
			if (mod.status != Mod.Status.NotInstalled)
			{
				return;
			}
			global::Debug.LogFormat("\tInstalling mod: {0}", new object[]
			{
				mod.title
			});
			mod.Install();
			if (mod.status == Mod.Status.Installed)
			{
				global::Debug.Log("\tSuccessfully installed.");
				this.events.Add(new Event
				{
					event_type = EventType.Installed,
					mod = mod.label
				});
				return;
			}
			global::Debug.Log("\tFailed install. Will install on restart.");
			this.events.Add(new Event
			{
				event_type = EventType.InstallFailed,
				mod = mod.label
			});
			this.events.Add(new Event
			{
				event_type = EventType.RestartRequested,
				mod = mod.label
			});
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x00329830 File Offset: 0x00327A30
		private void Uninstall(Mod mod)
		{
			if (mod.status == Mod.Status.NotInstalled)
			{
				return;
			}
			global::Debug.LogFormat("\tUninstalling mod {0}", new object[]
			{
				mod.title
			});
			mod.Uninstall();
			if (mod.status == Mod.Status.UninstallPending)
			{
				global::Debug.Log("\tFailed. Will re-install on restart.");
				mod.status = Mod.Status.ReinstallPending;
				this.events.Add(new Event
				{
					event_type = EventType.RestartRequested,
					mod = mod.label
				});
			}
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x003298AC File Offset: 0x00327AAC
		public void Subscribe(Mod mod, object caller)
		{
			global::Debug.LogFormat("Subscribe to mod {0}", new object[]
			{
				mod.title
			});
			Mod mod2 = this.mods.Find((Mod candidate) => mod.label.Match(candidate.label));
			mod.is_subscribed = true;
			if (mod2 == null)
			{
				this.mods.Add(mod);
				this.Install(mod);
			}
			else
			{
				if (mod2.status == Mod.Status.UninstallPending)
				{
					mod2.status = Mod.Status.Installed;
					this.events.Add(new Event
					{
						event_type = EventType.Installed,
						mod = mod2.label
					});
				}
				bool flag = mod2.label.version != mod.label.version;
				bool flag2 = mod2.available_content != mod.available_content;
				bool flag3 = flag || flag2 || mod2.status == Mod.Status.ReinstallPending;
				if (flag)
				{
					this.events.Add(new Event
					{
						event_type = EventType.VersionUpdate,
						mod = mod.label
					});
				}
				if (flag2)
				{
					this.events.Add(new Event
					{
						event_type = EventType.AvailableContentChanged,
						mod = mod.label
					});
				}
				string root = mod.file_source.GetRoot();
				mod2.CopyPersistentDataTo(mod);
				int index = this.mods.IndexOf(mod2);
				this.mods.RemoveAt(index);
				this.mods.Insert(index, mod);
				if (!mod2.description.IsNullOrWhiteSpace())
				{
					mod.description = mod2.description;
				}
				else
				{
					mod.description = UI.FRONTEND.MODS.NO_DESCRIPTION;
				}
				if (!mod2.title.IsNullOrWhiteSpace())
				{
					mod.title = mod2.title;
				}
				if (flag3 || mod.status == Mod.Status.NotInstalled)
				{
					if (mod.IsEnabledForActiveDlc())
					{
						mod.reinstall_path = root;
						mod.status = Mod.Status.ReinstallPending;
						this.events.Add(new Event
						{
							event_type = EventType.RestartRequested,
							mod = mod.label
						});
					}
					else
					{
						if (flag3)
						{
							this.Uninstall(mod);
						}
						this.Install(mod);
					}
				}
				else
				{
					mod.file_source = mod2.file_source;
				}
			}
			mod.file_source.Dispose();
			this.dirty = true;
			this.Update(caller);
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x00329B68 File Offset: 0x00327D68
		public void Update(Mod mod, object caller)
		{
			global::Debug.LogFormat("Update mod {0}", new object[]
			{
				mod.title
			});
			Mod mod2 = this.mods.Find((Mod candidate) => mod.label.Match(candidate.label));
			DebugUtil.DevAssert(!string.IsNullOrEmpty(mod2.label.id), "Should be subscribed to a mod we are getting an Update notification for", null);
			if (mod2.status == Mod.Status.UninstallPending)
			{
				return;
			}
			this.events.Add(new Event
			{
				event_type = EventType.VersionUpdate,
				mod = mod.label
			});
			string root = mod.file_source.GetRoot();
			mod2.CopyPersistentDataTo(mod);
			mod.is_subscribed = mod2.is_subscribed;
			int index = this.mods.IndexOf(mod2);
			this.mods.RemoveAt(index);
			this.mods.Insert(index, mod);
			if (mod.IsEnabledForActiveDlc())
			{
				mod.reinstall_path = root;
				mod.status = Mod.Status.ReinstallPending;
				this.events.Add(new Event
				{
					event_type = EventType.RestartRequested,
					mod = mod.label
				});
			}
			else
			{
				this.Uninstall(mod);
				this.Install(mod);
			}
			mod.file_source.Dispose();
			this.dirty = true;
			this.Update(caller);
		}

		// Token: 0x06007EDA RID: 32474 RVA: 0x00329CF8 File Offset: 0x00327EF8
		public void Unsubscribe(Label label, object caller)
		{
			global::Debug.LogFormat("Unsubscribe from mod {0}", new object[]
			{
				label.ToString()
			});
			int num = 0;
			foreach (Mod mod in this.mods)
			{
				if (mod.label.Match(label))
				{
					global::Debug.LogFormat("\t...found it: {0}", new object[]
					{
						mod.title
					});
					break;
				}
				num++;
			}
			if (num == this.mods.Count)
			{
				global::Debug.LogFormat("\t...not found", Array.Empty<object>());
				return;
			}
			Mod mod2 = this.mods[num];
			mod2.SetEnabledForActiveDlc(false);
			mod2.Unload((Content)0);
			this.events.Add(new Event
			{
				event_type = EventType.Uninstalled,
				mod = mod2.label
			});
			if (mod2.IsActive())
			{
				global::Debug.LogFormat("\tCould not unload all content provided by mod {0} : {1}\nUninstall will likely fail", new object[]
				{
					mod2.title,
					mod2.label.ToString()
				});
				this.events.Add(new Event
				{
					event_type = EventType.RestartRequested,
					mod = mod2.label
				});
			}
			if (mod2.status == Mod.Status.Installed)
			{
				global::Debug.LogFormat("\tUninstall mod {0} : {1}", new object[]
				{
					mod2.title,
					mod2.label.ToString()
				});
				mod2.Uninstall();
			}
			if (mod2.status == Mod.Status.NotInstalled)
			{
				global::Debug.LogFormat("\t...success. Removing from management list {0} : {1}", new object[]
				{
					mod2.title,
					mod2.label.ToString()
				});
				this.mods.RemoveAt(num);
			}
			this.dirty = true;
			this.Update(caller);
		}

		// Token: 0x06007EDB RID: 32475 RVA: 0x00329EE4 File Offset: 0x003280E4
		public bool IsInDevMode()
		{
			return this.mods.Exists((Mod mod) => mod.IsEnabledForActiveDlc() && mod.label.distribution_platform == Label.DistributionPlatform.Dev);
		}

		// Token: 0x06007EDC RID: 32476 RVA: 0x00329F10 File Offset: 0x00328110
		public void Load(Content content)
		{
			if ((content & Content.DLL) != (Content)0 && this.load_user_mod_loader_dll)
			{
				if (!DLLLoader.LoadUserModLoaderDLL())
				{
					global::Debug.Log("Using builtin mod system.");
				}
				else
				{
					global::Debug.LogWarning("Using ModLoader.DLL for custom mod loading! This is not the standard mod loading method.");
				}
				this.load_user_mod_loader_dll = false;
			}
			foreach (Mod mod in this.mods)
			{
				if (mod.IsEnabledForActiveDlc())
				{
					mod.Load(content);
				}
			}
			if ((content & Content.DLL) != (Content)0)
			{
				IReadOnlyList<Mod> readOnlyList = this.mods.AsReadOnly();
				foreach (Mod mod2 in this.mods)
				{
					if (mod2.IsEnabledForActiveDlc())
					{
						mod2.PostLoad(readOnlyList);
					}
				}
			}
			bool flag = false;
			foreach (Mod mod3 in this.mods)
			{
				Content content2 = mod3.loaded_content & content;
				Content content3 = mod3.available_content & content;
				if (mod3.IsEnabledForActiveDlc() && content2 != content3)
				{
					mod3.SetCrashed();
					if (!mod3.IsEnabledForActiveDlc())
					{
						flag = true;
						this.events.Add(new Event
						{
							event_type = EventType.Deactivated,
							mod = mod3.label
						});
					}
					global::Debug.LogFormat("Failed to load mod {0}...disabling", new object[]
					{
						mod3.title
					});
					this.events.Add(new Event
					{
						event_type = EventType.LoadError,
						mod = mod3.label
					});
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06007EDD RID: 32477 RVA: 0x0032A0F8 File Offset: 0x003282F8
		public void Unload(Content content)
		{
			foreach (Mod mod in this.mods)
			{
				mod.Unload(content);
			}
		}

		// Token: 0x06007EDE RID: 32478 RVA: 0x0032A14C File Offset: 0x0032834C
		public void Update(object change_source)
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.Save();
			if (this.on_update != null)
			{
				this.on_update(change_source);
			}
		}

		// Token: 0x06007EDF RID: 32479 RVA: 0x0032A17C File Offset: 0x0032837C
		public bool MatchFootprint(List<Label> footprint, Content relevant_content)
		{
			if (footprint == null)
			{
				return true;
			}
			bool flag = true;
			bool flag2 = true;
			bool flag3 = false;
			int num = -1;
			Func<Label, Mod, bool> is_match = (Label label, Mod mod) => mod.label.Match(label);
			using (List<Label>.Enumerator enumerator = footprint.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Label label = enumerator.Current;
					bool flag4 = false;
					for (int num2 = num + 1; num2 != this.mods.Count; num2++)
					{
						Mod mod3 = this.mods[num2];
						num = num2;
						Content content = mod3.available_content & relevant_content;
						bool flag5 = content > (Content)0;
						if (is_match(label, mod3))
						{
							if (flag5)
							{
								if (!mod3.IsEnabledForActiveDlc())
								{
									List<Event> list = this.events;
									Event item = new Event
									{
										event_type = EventType.ExpectedActive,
										mod = label
									};
									list.Add(item);
									flag = false;
								}
								else if (!mod3.AllActive(content))
								{
									List<Event> list2 = this.events;
									Event item = new Event
									{
										event_type = EventType.LoadError,
										mod = label
									};
									list2.Add(item);
								}
							}
							flag4 = true;
							break;
						}
						if (flag5 && mod3.IsEnabledForActiveDlc())
						{
							List<Event> list3 = this.events;
							Event item = new Event
							{
								event_type = EventType.ExpectedInactive,
								mod = mod3.label
							};
							list3.Add(item);
							flag3 = true;
						}
					}
					if (!flag4)
					{
						List<Event> list4 = this.events;
						Event item = new Event
						{
							event_type = (this.mods.Exists((Mod candidate) => is_match(label, candidate)) ? EventType.OutOfOrder : EventType.NotFound),
							mod = label
						};
						list4.Add(item);
						flag2 = false;
					}
				}
			}
			for (int num3 = num + 1; num3 != this.mods.Count; num3++)
			{
				Mod mod2 = this.mods[num3];
				if ((mod2.available_content & relevant_content) > (Content)0 && mod2.IsEnabledForActiveDlc())
				{
					List<Event> list5 = this.events;
					Event item = new Event
					{
						event_type = EventType.ExpectedInactive,
						mod = mod2.label
					};
					list5.Add(item);
					flag3 = true;
				}
			}
			return flag2 && flag && !flag3;
		}

		// Token: 0x06007EE0 RID: 32480 RVA: 0x0032A410 File Offset: 0x00328610
		private string GetFilename()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), "mods.json"));
		}

		// Token: 0x06007EE1 RID: 32481 RVA: 0x0032A428 File Offset: 0x00328628
		public static void Dialog(GameObject parent = null, string title = null, string text = null, string confirm_text = null, System.Action on_confirm = null, string cancel_text = null, System.Action on_cancel = null, string configurable_text = null, System.Action on_configurable_clicked = null, Sprite image_sprite = null)
		{
			((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent ?? Global.Instance.globalCanvas)).PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, title, confirm_text, cancel_text, image_sprite);
		}

		// Token: 0x06007EE2 RID: 32482 RVA: 0x0032A478 File Offset: 0x00328678
		private static string MakeModList(List<Event> events, EventType event_type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			int num = 30;
			foreach (Event @event in events)
			{
				if (@event.event_type == event_type)
				{
					stringBuilder.AppendLine(@event.mod.title);
					if (--num <= 0)
					{
						stringBuilder.AppendLine(UI.FRONTEND.MOD_DIALOGS.ADDITIONAL_MOD_EVENTS);
						break;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007EE3 RID: 32483 RVA: 0x0032A50C File Offset: 0x0032870C
		private static string MakeEventList(List<Event> events)
		{
			return Manager.MakeEventList(events, "\n");
		}

		// Token: 0x06007EE4 RID: 32484 RVA: 0x0032A51C File Offset: 0x0032871C
		private static string MakeEventList(List<Event> events, string prefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(prefix);
			string arg = null;
			string text = null;
			int num = 30;
			foreach (Event @event in events)
			{
				Event.GetUIStrings(@event.event_type, out arg, out text);
				stringBuilder.AppendFormat("{0}: {1}", arg, @event.mod.title);
				if (!string.IsNullOrEmpty(@event.details))
				{
					stringBuilder.AppendFormat(" ({0})", @event.details);
				}
				stringBuilder.Append("\n");
				if (--num <= 0)
				{
					stringBuilder.AppendLine(UI.FRONTEND.MOD_DIALOGS.ADDITIONAL_MOD_EVENTS);
					break;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x0032A5F4 File Offset: 0x003287F4
		private static string MakeModList(List<Event> events)
		{
			return Manager.MakeModList(events, "\n");
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x0032A604 File Offset: 0x00328804
		private static string MakeModList(List<Event> events, string prefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(prefix);
			HashSetPool<string, Manager>.PooledHashSet pooledHashSet = HashSetPool<string, Manager>.Allocate();
			int num = 30;
			foreach (Event @event in events)
			{
				if (pooledHashSet.Add(@event.mod.title))
				{
					stringBuilder.AppendLine(@event.mod.title);
					if (--num <= 0)
					{
						stringBuilder.AppendLine(UI.FRONTEND.MOD_DIALOGS.ADDITIONAL_MOD_EVENTS);
						break;
					}
				}
			}
			pooledHashSet.Recycle();
			return stringBuilder.ToString();
		}

		// Token: 0x06007EE7 RID: 32487 RVA: 0x0032A6B4 File Offset: 0x003288B4
		private void LoadFailureDialog(GameObject parent)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			foreach (Event @event in this.events)
			{
				if (@event.event_type == EventType.LoadError)
				{
					foreach (Mod mod in this.mods)
					{
						if (!mod.IsLocal && mod.label.Match(@event.mod))
						{
							mod.status = Mod.Status.ReinstallPending;
						}
					}
				}
			}
			this.dirty = true;
			this.Update(this);
			string title = UI.FRONTEND.MOD_DIALOGS.LOAD_FAILURE.TITLE;
			string text = string.Format(UI.FRONTEND.MOD_DIALOGS.LOAD_FAILURE.MESSAGE, Manager.MakeModList(this.events, EventType.LoadError));
			string confirm_text = UI.FRONTEND.MOD_DIALOGS.RESTART.OK;
			string cancel_text = UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL;
			Manager.Dialog(parent, title, text, confirm_text, new System.Action(App.instance.Restart), cancel_text, delegate
			{
			}, null, null, null);
			this.events.Clear();
		}

		// Token: 0x06007EE8 RID: 32488 RVA: 0x0032A804 File Offset: 0x00328A04
		private void DevRestartDialog(GameObject parent, bool is_crash)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			if (is_crash)
			{
				Manager.Dialog(parent, UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.TITLE, string.Format(UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.DEV_MESSAGE, Manager.MakeEventList(this.events)), UI.FRONTEND.MOD_DIALOGS.RESTART.OK, delegate
				{
					foreach (Mod mod in this.mods)
					{
						mod.SetEnabledForActiveDlc(false);
					}
					this.dirty = true;
					this.Update(this);
					App.instance.Restart();
				}, UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL, delegate
				{
				}, null, null, null);
			}
			else
			{
				Manager.Dialog(parent, UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, string.Format(UI.FRONTEND.MOD_DIALOGS.RESTART.DEV_MESSAGE, Manager.MakeEventList(this.events)), UI.FRONTEND.MOD_DIALOGS.RESTART.OK, delegate
				{
					App.instance.Restart();
				}, UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL, delegate
				{
				}, null, null, null);
			}
			this.events.Clear();
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x0032A91C File Offset: 0x00328B1C
		public void RestartDialog(string title, string message_format, System.Action on_cancel, bool with_details, GameObject parent, string cancel_text = null)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			string text = string.Format(message_format, with_details ? Manager.MakeEventList(this.events, null) : Manager.MakeModList(this.events, null));
			string confirm_text = UI.FRONTEND.MOD_DIALOGS.RESTART.OK;
			string cancel_text2 = cancel_text ?? UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL;
			Manager.Dialog(parent, title, text, confirm_text, new System.Action(App.instance.Restart), cancel_text2, on_cancel, null, null, null);
			this.events.Clear();
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x0032A9A0 File Offset: 0x00328BA0
		public void NotifyDialog(string title, string message_format, GameObject parent)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			Manager.Dialog(parent, title, string.Format(message_format, Manager.MakeEventList(this.events)), null, null, null, null, null, null, null);
			this.events.Clear();
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x0032A9E8 File Offset: 0x00328BE8
		public void SearchForModsInStackTrace(StackTrace stackTrace)
		{
			foreach (StackFrame stackFrame in stackTrace.GetFrames())
			{
				if (stackFrame != null)
				{
					Assembly assembly = null;
					MethodBase method = stackFrame.GetMethod();
					if (method != null)
					{
						Type declaringType = method.DeclaringType;
						if (declaringType != null)
						{
							assembly = declaringType.Assembly;
						}
					}
					foreach (Mod mod in this.mods)
					{
						if (mod.loaded_mod_data != null && !mod.foundInStackTrace)
						{
							if (assembly != null && mod.loaded_mod_data.dlls.Contains(assembly))
							{
								global::Debug.Log(string.Format("{0}'s assembly declared the method {1}:{2} in the stack trace, adding to referenced mods list", mod.title, method.DeclaringType, method.Name));
								mod.foundInStackTrace = true;
							}
							else if (method != null && mod.loaded_mod_data.patched_methods.Contains(method))
							{
								global::Debug.Log(string.Format("{0}'s patched_method {1}:{2} appears in the stack trace, adding to referenced mods list", mod.title, method.DeclaringType, method.Name));
								mod.foundInStackTrace = true;
							}
						}
					}
				}
			}
			string stackStr = stackTrace.ToString();
			this.SearchForModsInStackTrace(stackStr);
		}

		// Token: 0x06007EEC RID: 32492 RVA: 0x0032AB50 File Offset: 0x00328D50
		public void SearchForModsInStackTrace(string stackStr)
		{
			foreach (Mod mod in this.mods)
			{
				if (mod.loaded_mod_data != null && !mod.foundInStackTrace)
				{
					foreach (MethodBase methodBase in mod.loaded_mod_data.patched_methods)
					{
						if (new Regex(Regex.Escape(methodBase.DeclaringType.ToString()) + "[.:]" + Regex.Escape(methodBase.Name.ToString())).Match(stackStr).Success)
						{
							global::Debug.Log(string.Format("{0}'s patched_method {1}.{2} matched in the stack trace, adding to referenced mods list", mod.title, methodBase.DeclaringType, methodBase.Name));
							mod.foundInStackTrace = true;
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007EED RID: 32493 RVA: 0x0032AC58 File Offset: 0x00328E58
		public void HandleErrors(List<YamlIO.Error> world_gen_errors)
		{
			string value = FileSystem.Normalize(Manager.GetDirectory());
			ListPool<Mod, Manager>.PooledList pooledList = ListPool<Mod, Manager>.Allocate();
			foreach (YamlIO.Error error in world_gen_errors)
			{
				string text = (error.file.source != null) ? FileSystem.Normalize(error.file.source.GetRoot()) : string.Empty;
				YamlIO.LogError(error, text.Contains(value));
				if (error.severity != YamlIO.Error.Severity.Recoverable && text.Contains(value))
				{
					foreach (Mod mod in this.mods)
					{
						if (mod.IsEnabledForActiveDlc() && text.Contains(mod.label.install_path))
						{
							this.events.Add(new Event
							{
								event_type = EventType.BadWorldGen,
								mod = mod.label,
								details = Path.GetFileName(error.file.full_path)
							});
							break;
						}
					}
				}
			}
			foreach (Mod mod2 in pooledList)
			{
				mod2.SetCrashed();
				if (!mod2.IsDev)
				{
					this.events.Add(new Event
					{
						event_type = EventType.Deactivated,
						mod = mod2.label
					});
				}
				this.dirty = true;
			}
			pooledList.Recycle();
			this.Update(this);
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x0032AE54 File Offset: 0x00329054
		private void WriteDevBootReport()
		{
			if (!GenericGameSettings.instance.devBootModReport)
			{
				return;
			}
			string contents = JsonConvert.SerializeObject(this.mods, Formatting.Indented);
			File.WriteAllText(Path.GetDirectoryName(Application.dataPath) + "/modReport.json", contents);
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x0032AE98 File Offset: 0x00329098
		public void Report(GameObject parent)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.events.Count; i++)
			{
				Event @event = this.events[i];
				for (int num = this.events.Count - 1; num != i; num--)
				{
					if (this.events[num].event_type == @event.event_type && this.events[num].mod.Match(@event.mod) && this.events[num].details == @event.details)
					{
						this.events.RemoveAt(num);
					}
				}
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (Event event2 in this.events)
			{
				EventType event_type = event2.event_type;
				if (event_type <= EventType.ActiveDuringCrash)
				{
					if (event_type != EventType.LoadError)
					{
						if (event_type == EventType.ActiveDuringCrash)
						{
							flag = true;
						}
					}
					else
					{
						flag2 = true;
					}
				}
				else if (event_type != EventType.RestartRequested)
				{
					if (event_type == EventType.Deactivated)
					{
						if ((this.FindMod(event2.mod).available_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)) != (Content)0)
						{
							flag3 = true;
						}
					}
				}
				else
				{
					flag3 = true;
				}
			}
			flag3 = (flag || flag2 || flag3);
			bool flag4 = this.IsInDevMode();
			if (flag3 && flag4)
			{
				this.DevRestartDialog(parent, flag);
				return;
			}
			if (flag2)
			{
				this.LoadFailureDialog(parent);
				return;
			}
			if (flag)
			{
				this.RestartDialog(UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.TITLE, UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.MESSAGE, null, false, parent, null);
				return;
			}
			if (flag3)
			{
				this.RestartDialog(UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, UI.FRONTEND.MOD_DIALOGS.RESTART.MESSAGE, null, true, parent, null);
				return;
			}
			this.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, flag4 ? UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.DEV_MESSAGE : UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.MESSAGE, parent);
		}

		// Token: 0x06007EF0 RID: 32496 RVA: 0x0032B08C File Offset: 0x0032928C
		public bool Save()
		{
			if (!FileUtil.CreateDirectory(Manager.GetDirectory(), 5))
			{
				return false;
			}
			using (FileStream stream = FileUtil.Create(this.GetFilename(), 5))
			{
				if (stream == null)
				{
					return false;
				}
				using (StreamWriter streamWriter = FileUtil.DoIODialog<StreamWriter>(() => new StreamWriter(stream), this.GetFilename(), null, 5))
				{
					if (streamWriter == null)
					{
						return false;
					}
					string value = JsonConvert.SerializeObject(new Manager.PersistentData(this.current_version, this.mods), Formatting.Indented);
					streamWriter.Write(value);
				}
			}
			return true;
		}

		// Token: 0x06007EF1 RID: 32497 RVA: 0x0032B14C File Offset: 0x0032934C
		public Mod FindMod(Label label)
		{
			foreach (Mod mod in this.mods)
			{
				if (mod.label.Equals(label))
				{
					return mod;
				}
			}
			return null;
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x0032B1B8 File Offset: 0x003293B8
		public bool IsModEnabled(Label id)
		{
			Mod mod = this.FindMod(id);
			return mod != null && mod.IsEnabledForActiveDlc();
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x0032B1D8 File Offset: 0x003293D8
		public bool EnableMod(Label id, bool enabled, object caller)
		{
			Mod mod = this.FindMod(id);
			if (mod == null)
			{
				return false;
			}
			if (mod.IsEmpty())
			{
				return false;
			}
			if (mod.IsEnabledForActiveDlc() == enabled)
			{
				return false;
			}
			mod.SetEnabledForActiveDlc(enabled);
			if (enabled)
			{
				mod.Load((Content)0);
			}
			else
			{
				mod.Unload((Content)0);
			}
			this.dirty = true;
			this.Update(caller);
			return true;
		}

		// Token: 0x06007EF4 RID: 32500 RVA: 0x0032B230 File Offset: 0x00329430
		public void Reinsert(int source_index, int target_index, bool move_to_end, object caller)
		{
			if (move_to_end)
			{
				target_index = this.mods.Count;
			}
			DebugUtil.Assert(source_index != target_index);
			if (source_index < -1 || this.mods.Count <= source_index)
			{
				return;
			}
			if (target_index < -1 || this.mods.Count <= target_index)
			{
				return;
			}
			Mod item = this.mods[source_index];
			this.mods.RemoveAt(source_index);
			if (source_index > target_index)
			{
				target_index++;
			}
			if (target_index == this.mods.Count)
			{
				this.mods.Add(item);
			}
			else
			{
				this.mods.Insert(target_index, item);
			}
			this.dirty = true;
			this.Update(caller);
		}

		// Token: 0x06007EF5 RID: 32501 RVA: 0x0032B2DC File Offset: 0x003294DC
		public void SendMetricsEvent()
		{
			ListPool<string, Manager>.PooledList pooledList = ListPool<string, Manager>.Allocate();
			foreach (Mod mod in this.mods)
			{
				if (mod.IsEnabledForActiveDlc())
				{
					pooledList.Add(mod.title);
				}
			}
			DictionaryPool<string, object, Manager>.PooledDictionary pooledDictionary = DictionaryPool<string, object, Manager>.Allocate();
			pooledDictionary["ModCount"] = pooledList.Count;
			pooledDictionary["Mods"] = pooledList;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(pooledDictionary, "Mods");
			pooledDictionary.Recycle();
			pooledList.Recycle();
			KCrashReporter.haveActiveMods = (pooledList.Count > 0);
		}

		// Token: 0x04005DAD RID: 23981
		public const Content all_content = Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation;

		// Token: 0x04005DAE RID: 23982
		public const Content boot_content = Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation;

		// Token: 0x04005DAF RID: 23983
		public const Content on_demand_content = (Content)0;

		// Token: 0x04005DB0 RID: 23984
		public List<IDistributionPlatform> distribution_platforms = new List<IDistributionPlatform>();

		// Token: 0x04005DB1 RID: 23985
		public List<Mod> mods = new List<Mod>();

		// Token: 0x04005DB2 RID: 23986
		public List<Event> events = new List<Event>();

		// Token: 0x04005DB3 RID: 23987
		private bool dirty = true;

		// Token: 0x04005DB4 RID: 23988
		public Manager.OnUpdate on_update;

		// Token: 0x04005DB5 RID: 23989
		private const int IO_OP_RETRY_COUNT = 5;

		// Token: 0x04005DB6 RID: 23990
		private bool load_user_mod_loader_dll = true;

		// Token: 0x04005DB7 RID: 23991
		private const int MAX_DIALOG_ENTRIES = 30;

		// Token: 0x04005DB8 RID: 23992
		private int current_version = 1;

		// Token: 0x020021C9 RID: 8649
		// (Invoke) Token: 0x0600BE1D RID: 48669
		public delegate void OnUpdate(object change_source);

		// Token: 0x020021CA RID: 8650
		private class PersistentData
		{
			// Token: 0x0600BE20 RID: 48672 RVA: 0x004069B8 File Offset: 0x00404BB8
			public PersistentData()
			{
			}

			// Token: 0x0600BE21 RID: 48673 RVA: 0x004069C0 File Offset: 0x00404BC0
			public PersistentData(int version, List<Mod> mods)
			{
				this.version = version;
				this.mods = mods;
			}

			// Token: 0x04009B6E RID: 39790
			public int version;

			// Token: 0x04009B6F RID: 39791
			public List<Mod> mods;
		}
	}
}
