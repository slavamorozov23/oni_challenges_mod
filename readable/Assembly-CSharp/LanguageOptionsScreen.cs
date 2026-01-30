using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D60 RID: 3424
public class LanguageOptionsScreen : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x060069EA RID: 27114 RVA: 0x00280DD0 File Offset: 0x0027EFD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += this.Deactivate;
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += this.Deactivate;
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.uninstallButton.onClick += delegate()
		{
			this.OnClickUninstall();
		};
		this.uninstallButton.gameObject.SetActive(false);
		this.RebuildScreen();
	}

	// Token: 0x060069EB RID: 27115 RVA: 0x00280E7C File Offset: 0x0027F07C
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.uninstallButton.isInteractable = (KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()) != Localization.SelectedLanguageType.None.ToString());
		this.RebuildPreinstalledButtons();
		this.RebuildUGCButtons();
	}

	// Token: 0x060069EC RID: 27116 RVA: 0x00280F1C File Offset: 0x0027F11C
	private void RebuildPreinstalledButtons()
	{
		using (List<string>.Enumerator enumerator = Localization.PreinstalledLanguages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string code = enumerator.Current;
				if (!(code != Localization.DEFAULT_LANGUAGE_CODE) || File.Exists(Localization.GetPreinstalledLocalizationFilePath(code)))
				{
					GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.preinstalledLanguagesContainer, false);
					gameObject.name = code + "_button";
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					LocText reference = component.GetReference<LocText>("Title");
					reference.text = Localization.GetPreinstalledLocalizationTitle(code);
					reference.enabled = false;
					reference.enabled = true;
					Texture2D preinstalledLocalizationImage = Localization.GetPreinstalledLocalizationImage(code);
					if (preinstalledLocalizationImage != null)
					{
						component.GetReference<Image>("Image").sprite = Sprite.Create(preinstalledLocalizationImage, new Rect(Vector2.zero, new Vector2((float)preinstalledLocalizationImage.width, (float)preinstalledLocalizationImage.height)), Vector2.one * 0.5f);
					}
					gameObject.GetComponent<KButton>().onClick += delegate()
					{
						this.ConfirmLanguagePreinstalledOrMod((code != Localization.DEFAULT_LANGUAGE_CODE) ? code : string.Empty, null);
					};
					this.buttons.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x060069ED RID: 27117 RVA: 0x0028108C File Offset: 0x0027F28C
	protected override void OnActivate()
	{
		base.OnActivate();
		Global.Instance.modManager.Sanitize(base.gameObject);
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.AddClient(this);
		}
	}

	// Token: 0x060069EE RID: 27118 RVA: 0x002810C1 File Offset: 0x0027F2C1
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.RemoveClient(this);
		}
	}

	// Token: 0x060069EF RID: 27119 RVA: 0x002810E4 File Offset: 0x0027F2E4
	private void ConfirmLanguageChoiceDialog(string[] lines, bool is_template, System.Action install_language)
	{
		Localization.Locale locale = Localization.GetLocale(lines);
		Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
		TMP_FontAsset font = Localization.GetFont(locale.FontName);
		ConfirmDialogScreen screen = this.GetConfirmDialog();
		HashSet<MemberInfo> excluded_members = new HashSet<MemberInfo>(typeof(ConfirmDialogScreen).GetMember("cancelButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
		Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);
		Func<LocString, string> func = delegate(LocString loc_string)
		{
			string result;
			if (!translated_strings.TryGetValue(loc_string.key.String, out result))
			{
				return loc_string;
			}
			return result;
		};
		ConfirmDialogScreen screen2 = screen;
		string title_text = func(UI.CONFIRMDIALOG.DIALOG_HEADER);
		screen2.PopupConfirmDialog(func(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT), delegate
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			install_language();
			App.instance.Restart();
		}, delegate
		{
			Localization.SetFont<ConfirmDialogScreen>(screen, Localization.FontAsset, Localization.IsRightToLeft, excluded_members);
		}, null, null, title_text, func(UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART), UI.FRONTEND.TRANSLATIONS_SCREEN.CANCEL, null);
	}

	// Token: 0x060069F0 RID: 27120 RVA: 0x002811C6 File Offset: 0x0027F3C6
	private void ConfirmPreinstalledLanguage(string selected_preinstalled_translation)
	{
		Localization.GetSelectedLanguageType();
	}

	// Token: 0x060069F1 RID: 27121 RVA: 0x002811D0 File Offset: 0x0027F3D0
	private void ConfirmLanguagePreinstalledOrMod(string selected_preinstalled_translation, string mod_id)
	{
		Localization.SelectedLanguageType selectedLanguageType = Localization.GetSelectedLanguageType();
		if (mod_id != null)
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.UGC && mod_id == this.currentLanguageModId)
			{
				this.Deactivate();
				return;
			}
			string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(mod_id);
			this.ConfirmLanguageChoiceDialog(languageLinesForMod, false, delegate
			{
				LanguageOptionsScreen.SetCurrentLanguage(mod_id);
			});
			return;
		}
		else if (!string.IsNullOrEmpty(selected_preinstalled_translation))
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (selectedLanguageType == Localization.SelectedLanguageType.Preinstalled && currentLanguageCode == selected_preinstalled_translation)
			{
				this.Deactivate();
				return;
			}
			string[] lines = File.ReadAllLines(Localization.GetPreinstalledLocalizationFilePath(selected_preinstalled_translation), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines, false, delegate
			{
				Localization.LoadPreinstalledTranslation(selected_preinstalled_translation);
			});
			return;
		}
		else
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.None)
			{
				this.Deactivate();
				return;
			}
			string[] lines2 = File.ReadAllLines(Localization.GetDefaultLocalizationFilePath(), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines2, true, delegate
			{
				Localization.ClearLanguage();
			});
			return;
		}
	}

	// Token: 0x060069F2 RID: 27122 RVA: 0x002812DA File Offset: 0x0027F4DA
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x060069F3 RID: 27123 RVA: 0x00281310 File Offset: 0x0027F510
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060069F4 RID: 27124 RVA: 0x00281334 File Offset: 0x0027F534
	private void RebuildUGCButtons()
	{
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if ((mod.available_content & Content.Translation) != (Content)0 && mod.status == Mod.Status.Installed)
			{
				GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.ugcLanguagesContainer, false);
				gameObject.name = mod.title + "_button";
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				TMP_FontAsset font = Localization.GetFont(Localization.GetFontName(LanguageOptionsScreen.GetLanguageLinesForMod(mod)));
				LocText reference = component.GetReference<LocText>("Title");
				reference.SetText(string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.UGC_MOD_TITLE_FORMAT, mod.title));
				reference.font = font;
				Texture2D previewImage = mod.GetPreviewImage();
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				string mod_id = mod.label.id;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.ConfirmLanguagePreinstalledOrMod(string.Empty, mod_id);
				};
				this.buttons.Add(gameObject);
			}
		}
	}

	// Token: 0x060069F5 RID: 27125 RVA: 0x002814C0 File Offset: 0x0027F6C0
	private void Uninstall()
	{
		this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.ARE_YOU_SURE, delegate
		{
			Localization.ClearLanguage();
			this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(this.Deactivate), null, null, null, null, null, null);
		}, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x060069F6 RID: 27126 RVA: 0x00281513 File Offset: 0x0027F713
	private void OnClickUninstall()
	{
		this.Uninstall();
	}

	// Token: 0x060069F7 RID: 27127 RVA: 0x0028151B File Offset: 0x0027F71B
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=language");
	}

	// Token: 0x060069F8 RID: 27128 RVA: 0x00281528 File Offset: 0x0027F728
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
		ulong value;
		if (ulong.TryParse(savedLanguageMod, out value))
		{
			PublishedFileId_t value2 = (PublishedFileId_t)value;
			if (removed.Contains(value2))
			{
				global::Debug.Log("Unsubscribe detected for currently installed language mod [" + savedLanguageMod + "]");
				this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, delegate
				{
					Localization.ClearLanguage();
					App.instance.Restart();
				}, null, null, null, null, UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART, null, null);
			}
			if (updated.Contains(value2))
			{
				global::Debug.Log("Download complete for currently installed language [" + savedLanguageMod + "] updating in background. Changes will happen next restart.");
			}
		}
		this.RebuildScreen();
	}

	// Token: 0x060069F9 RID: 27129 RVA: 0x002815D5 File Offset: 0x0027F7D5
	public static string GetSavedLanguageMod()
	{
		return KPlayerPrefs.GetString("InstalledLanguage");
	}

	// Token: 0x060069FA RID: 27130 RVA: 0x002815E1 File Offset: 0x0027F7E1
	public static void SetSavedLanguageMod(string mod_id)
	{
		KPlayerPrefs.SetString("InstalledLanguage", mod_id);
	}

	// Token: 0x060069FB RID: 27131 RVA: 0x002815EE File Offset: 0x0027F7EE
	public static void CleanUpSavedLanguageMod()
	{
		KPlayerPrefs.SetString("InstalledLanguage", null);
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x060069FC RID: 27132 RVA: 0x002815FB File Offset: 0x0027F7FB
	// (set) Token: 0x060069FD RID: 27133 RVA: 0x00281603 File Offset: 0x0027F803
	public string currentLanguageModId
	{
		get
		{
			return this._currentLanguageModId;
		}
		private set
		{
			this._currentLanguageModId = value;
			LanguageOptionsScreen.SetSavedLanguageMod(this._currentLanguageModId);
		}
	}

	// Token: 0x060069FE RID: 27134 RVA: 0x00281617 File Offset: 0x0027F817
	public static bool SetCurrentLanguage(string mod_id)
	{
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
		if (LanguageOptionsScreen.LoadTranslation(mod_id))
		{
			LanguageOptionsScreen.SetSavedLanguageMod(mod_id);
			return true;
		}
		return false;
	}

	// Token: 0x060069FF RID: 27135 RVA: 0x00281630 File Offset: 0x0027F830
	public static bool HasInstalledLanguage()
	{
		string currentModId = LanguageOptionsScreen.GetSavedLanguageMod();
		if (currentModId == null)
		{
			return false;
		}
		if (Global.Instance.modManager.mods.Find((Mod m) => m.label.id == currentModId) == null)
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			return false;
		}
		return true;
	}

	// Token: 0x06006A00 RID: 27136 RVA: 0x00281684 File Offset: 0x0027F884
	public static string GetInstalledLanguageCode()
	{
		string result = "";
		string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(LanguageOptionsScreen.GetSavedLanguageMod());
		if (languageLinesForMod != null)
		{
			Localization.Locale locale = Localization.GetLocale(languageLinesForMod);
			if (locale != null)
			{
				result = locale.Code;
			}
		}
		return result;
	}

	// Token: 0x06006A01 RID: 27137 RVA: 0x002816B8 File Offset: 0x0027F8B8
	private static bool LoadTranslation(string mod_id)
	{
		Mod mod = Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id);
		if (mod == null)
		{
			global::Debug.LogWarning("Tried loading a translation from a non-existent mod id: " + mod_id);
			return false;
		}
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		return languageFilename != null && Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.UGC, languageFilename);
	}

	// Token: 0x06006A02 RID: 27138 RVA: 0x00281720 File Offset: 0x0027F920
	private static string GetLanguageFilename(Mod mod)
	{
		global::Debug.Assert(mod.content_source.GetType() == typeof(KMod.Directory), "Can only load translations from extracted mods.");
		string text = Path.Combine(mod.ContentPath, "strings.po");
		if (!File.Exists(text))
		{
			global::Debug.LogWarning("GetLanguageFile: " + text + " missing for mod " + mod.label.title);
			return null;
		}
		return text;
	}

	// Token: 0x06006A03 RID: 27139 RVA: 0x00281790 File Offset: 0x0027F990
	private static string[] GetLanguageLinesForMod(string mod_id)
	{
		return LanguageOptionsScreen.GetLanguageLinesForMod(Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id));
	}

	// Token: 0x06006A04 RID: 27140 RVA: 0x002817D0 File Offset: 0x0027F9D0
	private static string[] GetLanguageLinesForMod(Mod mod)
	{
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		if (languageFilename == null)
		{
			return null;
		}
		string[] array = File.ReadAllLines(languageFilename, Encoding.UTF8);
		if (array == null || array.Length == 0)
		{
			global::Debug.LogWarning("Couldn't find any strings in the translation mod " + mod.label.title);
			return null;
		}
		return array;
	}

	// Token: 0x040048C6 RID: 18630
	private static readonly string[] poFile = new string[]
	{
		"strings.po"
	};

	// Token: 0x040048C7 RID: 18631
	public const string KPLAYER_PREFS_LANGUAGE_KEY = "InstalledLanguage";

	// Token: 0x040048C8 RID: 18632
	public const string TAG_LANGUAGE = "language";

	// Token: 0x040048C9 RID: 18633
	public KButton textButton;

	// Token: 0x040048CA RID: 18634
	public KButton dismissButton;

	// Token: 0x040048CB RID: 18635
	public KButton closeButton;

	// Token: 0x040048CC RID: 18636
	public KButton workshopButton;

	// Token: 0x040048CD RID: 18637
	public KButton uninstallButton;

	// Token: 0x040048CE RID: 18638
	[Space]
	public GameObject languageButtonPrefab;

	// Token: 0x040048CF RID: 18639
	public GameObject preinstalledLanguagesTitle;

	// Token: 0x040048D0 RID: 18640
	public GameObject preinstalledLanguagesContainer;

	// Token: 0x040048D1 RID: 18641
	public GameObject ugcLanguagesTitle;

	// Token: 0x040048D2 RID: 18642
	public GameObject ugcLanguagesContainer;

	// Token: 0x040048D3 RID: 18643
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x040048D4 RID: 18644
	private string _currentLanguageModId;

	// Token: 0x040048D5 RID: 18645
	private System.DateTime currentLastModified;
}
