using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using Klei;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020009E5 RID: 2533
public static class Localization
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x060049BB RID: 18875 RVA: 0x001AB8A0 File Offset: 0x001A9AA0
	public static TMP_FontAsset FontAsset
	{
		get
		{
			return Localization.sFontAsset;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x060049BC RID: 18876 RVA: 0x001AB8A7 File Offset: 0x001A9AA7
	public static bool IsRightToLeft
	{
		get
		{
			return Localization.sLocale != null && Localization.sLocale.IsRightToLeft;
		}
	}

	// Token: 0x060049BD RID: 18877 RVA: 0x001AB8BC File Offset: 0x001A9ABC
	private static IEnumerable<Type> CollectLocStringTreeRoots(string locstrings_namespace, Assembly assembly)
	{
		return from type in assembly.GetTypes()
		where type.IsClass && type.Namespace == locstrings_namespace && !type.IsNested
		select type;
	}

	// Token: 0x060049BE RID: 18878 RVA: 0x001AB8F0 File Offset: 0x001A9AF0
	private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (FieldInfo fieldInfo in locstring_tree_root.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
				}
				else
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					if (locString == null)
					{
						global::Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
					}
					else
					{
						dictionary[fieldInfo.Name] = locString.text;
					}
				}
			}
		}
		foreach (Type type in locstring_tree_root.GetNestedTypes())
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		return dictionary;
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x001AB9D8 File Offset: 0x001A9BD8
	private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtime_locstring_tree)
	{
		List<string> list = new List<string>(runtime_locstring_tree.Keys);
		list.Sort();
		foreach (string text in list)
		{
			string text2 = path + "." + text;
			object obj = runtime_locstring_tree[text];
			if (obj.GetType() != typeof(string))
			{
				Localization.WriteStringsTemplate(text2, writer, obj as Dictionary<string, object>);
			}
			else
			{
				string text3 = obj as string;
				text3 = text3.Replace("\\", "\\\\");
				text3 = text3.Replace("\"", "\\\"");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("’", "'");
				text3 = text3.Replace("“", "\\\"");
				text3 = text3.Replace("”", "\\\"");
				text3 = text3.Replace("…", "...");
				writer.WriteLine("#. " + text2);
				writer.WriteLine("msgctxt \"{0}\"", text2);
				writer.WriteLine("msgid \"" + text3 + "\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("");
			}
		}
	}

	// Token: 0x060049C0 RID: 18880 RVA: 0x001ABB58 File Offset: 0x001A9D58
	public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> current_runtime_locstring_forest)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		if (current_runtime_locstring_forest != null)
		{
			dictionary.Concat(current_runtime_locstring_forest);
		}
		using (StreamWriter streamWriter = new StreamWriter(output_filename, false, new UTF8Encoding(false)))
		{
			streamWriter.WriteLine("msgid \"\"");
			streamWriter.WriteLine("msgstr \"\"");
			streamWriter.WriteLine("\"Application: Oxygen Not Included\"");
			streamWriter.WriteLine("\"POT Version: 2.0\"");
			streamWriter.WriteLine("");
			Localization.WriteStringsTemplate(locstrings_namespace, streamWriter, dictionary);
		}
		DebugUtil.LogArgs(new object[]
		{
			"Generated " + output_filename
		});
	}

	// Token: 0x060049C1 RID: 18881 RVA: 0x001ABC54 File Offset: 0x001A9E54
	public static void GenerateStringsTemplate(Type locstring_tree_root, string output_folder)
	{
		output_folder = FileSystem.Normalize(output_folder);
		if (!FileUtil.CreateDirectory(output_folder, 5))
		{
			return;
		}
		Localization.GenerateStringsTemplate(locstring_tree_root.Namespace, Assembly.GetAssembly(locstring_tree_root), FileSystem.Normalize(Path.Combine(output_folder, string.Format("{0}_template.pot", locstring_tree_root.Namespace.ToLower()))), null);
	}

	// Token: 0x060049C2 RID: 18882 RVA: 0x001ABCA8 File Offset: 0x001A9EA8
	public static void Initialize()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Localization.Initialize!"
		});
		bool flag = false;
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);
			break;
		case Localization.SelectedLanguageType.Preinstalled:
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (!string.IsNullOrEmpty(currentLanguageCode))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Preinstalled localization"
				});
				DebugUtil.LogArgs(new object[]
				{
					" -> ",
					currentLanguageCode
				});
				Localization.LoadPreinstalledTranslation(currentLanguageCode);
			}
			else
			{
				flag = true;
			}
			break;
		}
		case Localization.SelectedLanguageType.UGC:
			if (LanguageOptionsScreen.HasInstalledLanguage())
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Mod-based localization"
				});
				string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
				if (LanguageOptionsScreen.SetCurrentLanguage(savedLanguageMod))
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Loaded language from mod: " + savedLanguageMod
					});
				}
				else
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Failed to load language from mod: " + savedLanguageMod
					});
				}
			}
			else
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x060049C3 RID: 18883 RVA: 0x001ABDAC File Offset: 0x001A9FAC
	public static void VerifyTranslationModSubscription(GameObject context)
	{
		if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.UGC)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (LanguageOptionsScreen.HasInstalledLanguage())
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = new PublishedFileId_t((ulong)KPlayerPrefs.GetInt("InstalledLanguage", (int)PublishedFileId_t.Invalid.m_PublishedFileId));
		Label rhs = new Label
		{
			distribution_platform = Label.DistributionPlatform.Steam,
			id = publishedFileId_t.ToString()
		};
		string arg = UI.FRONTEND.TRANSLATIONS_SCREEN.UNKNOWN;
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.label.Match(rhs))
			{
				arg = mod.title;
				break;
			}
		}
		Localization.ClearLanguage();
		KScreen component = KScreenManager.AddChild(context, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		ConfirmDialogScreen component2 = component.GetComponent<ConfirmDialogScreen>();
		string title_text = UI.CONFIRMDIALOG.DIALOG_HEADER;
		string text = string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.MISSING_LANGUAGE_PACK, arg);
		string confirm_text = UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART;
		component2.PopupConfirmDialog(text, new System.Action(App.instance.Restart), null, null, null, title_text, confirm_text, null, null);
	}

	// Token: 0x060049C4 RID: 18884 RVA: 0x001ABEF4 File Offset: 0x001AA0F4
	public static void LoadPreinstalledTranslation(string code)
	{
		if (!string.IsNullOrEmpty(code) && code != Localization.DEFAULT_LANGUAGE_CODE)
		{
			string preinstalledLocalizationFilePath = Localization.GetPreinstalledLocalizationFilePath(code);
			if (Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.Preinstalled, preinstalledLocalizationFilePath))
			{
				KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, code);
				return;
			}
		}
		else
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x060049C5 RID: 18885 RVA: 0x001ABF37 File Offset: 0x001AA137
	public static bool LoadLocalTranslationFile(Localization.SelectedLanguageType source, string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		bool flag = Localization.LoadTranslationFromLines(File.ReadAllLines(path, Encoding.UTF8));
		if (flag)
		{
			KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, source.ToString());
			return flag;
		}
		Localization.ClearLanguage();
		return flag;
	}

	// Token: 0x060049C6 RID: 18886 RVA: 0x001ABF74 File Offset: 0x001AA174
	private static bool LoadTranslationFromLines(string[] lines)
	{
		if (lines == null || lines.Length == 0)
		{
			return false;
		}
		Localization.sLocale = Localization.GetLocale(lines);
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
		bool flag = Localization.LoadTranslation(lines, false);
		if (flag)
		{
			Localization.currentFontName = Localization.GetFontName(lines);
			Localization.SwapToLocalizedFont(Localization.currentFontName);
		}
		return flag;
	}

	// Token: 0x060049C7 RID: 18887 RVA: 0x001ABFD8 File Offset: 0x001AA1D8
	public static bool LoadTranslation(string[] lines, bool isTemplate = false)
	{
		bool result;
		try
		{
			Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, isTemplate));
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x001AC01C File Offset: 0x001AA21C
	public static Dictionary<string, string> LoadStringsFile(string path, bool isTemplate)
	{
		return Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), isTemplate);
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x001AC030 File Offset: 0x001AA230
	public static Dictionary<string, string> ExtractTranslatedStrings(string[] lines, bool isTemplate = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Localization.Entry entry = default(Localization.Entry);
		string key = isTemplate ? "msgid" : "msgstr";
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i];
			if (text == null || text.Length == 0)
			{
				entry = default(Localization.Entry);
			}
			else
			{
				string parameter = Localization.GetParameter("msgctxt", i, lines);
				if (parameter != null)
				{
					entry.msgctxt = parameter;
				}
				parameter = Localization.GetParameter(key, i, lines);
				if (parameter != null)
				{
					entry.msgstr = parameter;
				}
			}
			if (entry.IsPopulated)
			{
				dictionary[entry.msgctxt] = entry.msgstr;
				entry = default(Localization.Entry);
			}
		}
		return dictionary;
	}

	// Token: 0x060049CA RID: 18890 RVA: 0x001AC0DC File Offset: 0x001AA2DC
	private static string FixupString(string result)
	{
		result = result.Replace("\\n", "\n");
		result = result.Replace("\\\"", "\"");
		result = result.Replace("<style=“", "<style=\"");
		result = result.Replace("”>", "\">");
		result = result.Replace("<color=^p", "<color=#");
		return result;
	}

	// Token: 0x060049CB RID: 18891 RVA: 0x001AC144 File Offset: 0x001AA344
	private static string GetParameter(string key, int idx, string[] all_lines)
	{
		if (!all_lines[idx].StartsWith(key))
		{
			return null;
		}
		List<string> list = new List<string>();
		string text = all_lines[idx];
		text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
		list.Add(text);
		for (int i = idx + 1; i < all_lines.Length; i++)
		{
			string text2 = all_lines[i];
			if (!text2.StartsWith("\""))
			{
				break;
			}
			list.Add(text2);
		}
		string text3 = "";
		foreach (string text4 in list)
		{
			if (text4.EndsWith("\r"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			text4 = text4.Substring(1, text4.Length - 2);
			text4 = Localization.FixupString(text4);
			text3 += text4;
		}
		return text3;
	}

	// Token: 0x060049CC RID: 18892 RVA: 0x001AC244 File Offset: 0x001AA444
	private static void AddAssembly(string locstrings_namespace, Assembly assembly)
	{
		List<Assembly> list;
		if (!Localization.translatable_assemblies.TryGetValue(locstrings_namespace, out list))
		{
			list = new List<Assembly>();
			Localization.translatable_assemblies.Add(locstrings_namespace, list);
		}
		list.Add(assembly);
	}

	// Token: 0x060049CD RID: 18893 RVA: 0x001AC279 File Offset: 0x001AA479
	public static void AddAssembly(Assembly assembly)
	{
		Localization.AddAssembly("STRINGS", assembly);
	}

	// Token: 0x060049CE RID: 18894 RVA: 0x001AC288 File Offset: 0x001AA488
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Assembly assembly = Assembly.GetAssembly(locstring_tree_root);
		Localization.AddAssembly(locstring_tree_root.Namespace, assembly);
		string parent_path = locstring_tree_root.Namespace + ".";
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstring_tree_root.Namespace, assembly))
		{
			LocString.CreateLocStringKeys(type, parent_path);
		}
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x001AC300 File Offset: 0x001AA500
	public static void OverloadStrings(Dictionary<string, string> translated_strings)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		foreach (KeyValuePair<string, List<Assembly>> keyValuePair in Localization.translatable_assemblies)
		{
			foreach (Assembly assembly in keyValuePair.Value)
			{
				foreach (Type type in Localization.CollectLocStringTreeRoots(keyValuePair.Key, assembly))
				{
					string path = keyValuePair.Key + "." + type.Name;
					Localization.OverloadStrings(translated_strings, path, type, ref text, ref text2, ref text3);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have missing or mismatched parameters:\n" + text
			});
		}
		if (!string.IsNullOrEmpty(text2))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have mismatched <link> tags:\n" + text2
			});
		}
		if (!string.IsNullOrEmpty(text3))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" + text3
			});
		}
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x001AC474 File Offset: 0x001AA674
	public static void OverloadStrings(Dictionary<string, string> translated_strings, string path, Type locstring_hierarchy, ref string parameter_errors, ref string link_errors, ref string link_count_errors)
	{
		foreach (FieldInfo fieldInfo in locstring_hierarchy.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				string text = path + "." + fieldInfo.Name;
				string text2 = null;
				if (translated_strings.TryGetValue(text, out text2))
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					LocString value = new LocString(text2, text);
					if (!Localization.AreParametersPreserved(locString.text, text2))
					{
						parameter_errors = parameter_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, text2))
					{
						link_count_errors = link_count_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasMatchingLinkTags(text2, 0))
					{
						link_errors = link_errors + "\t" + text + "\n";
					}
					else
					{
						fieldInfo.SetValue(null, value);
					}
				}
			}
		}
		foreach (Type type in locstring_hierarchy.GetNestedTypes())
		{
			string path2 = path + "." + type.Name;
			Localization.OverloadStrings(translated_strings, path2, type, ref parameter_errors, ref link_errors, ref link_count_errors);
		}
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x001AC5AE File Offset: 0x001AA7AE
	public static string GetDefaultLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings_template.pot");
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x001AC5BF File Offset: 0x001AA7BF
	public static string GetModLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings.po");
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x001AC5D0 File Offset: 0x001AA7D0
	public static string GetPreinstalledLocalizationFilePath(string code)
	{
		string path = "strings/strings_preinstalled_" + code + ".po";
		return Path.Combine(Application.streamingAssetsPath, path);
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x001AC5F9 File Offset: 0x001AA7F9
	public static string GetPreinstalledLocalizationTitle(string code)
	{
		return Strings.Get("STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PREINSTALLED_LANGUAGES." + code.ToUpper());
	}

	// Token: 0x060049D5 RID: 18901 RVA: 0x001AC618 File Offset: 0x001AA818
	public static Texture2D GetPreinstalledLocalizationImage(string code)
	{
		string path = Path.Combine(Application.streamingAssetsPath, "strings/preinstalled_icon_" + code + ".png");
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}
		return null;
	}

	// Token: 0x060049D6 RID: 18902 RVA: 0x001AC660 File Offset: 0x001AA860
	public static void SetLocale(Localization.Locale locale)
	{
		Localization.sLocale = locale;
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x001AC688 File Offset: 0x001AA888
	public static Localization.Locale GetLocale()
	{
		return Localization.sLocale;
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x001AC690 File Offset: 0x001AA890
	private static string GetFontParam(string line)
	{
		string text = null;
		if (line.StartsWith("\"Font:"))
		{
			text = line.Substring("\"Font:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x001AC6E8 File Offset: 0x001AA8E8
	public static string GetCurrentLanguageCode()
	{
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			return Localization.DEFAULT_LANGUAGE_CODE;
		case Localization.SelectedLanguageType.Preinstalled:
			return KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_CODE_KEY);
		case Localization.SelectedLanguageType.UGC:
			return LanguageOptionsScreen.GetInstalledLanguageCode();
		default:
			return "";
		}
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x001AC72C File Offset: 0x001AA92C
	public static Localization.SelectedLanguageType GetSelectedLanguageType()
	{
		return (Localization.SelectedLanguageType)Enum.Parse(typeof(Localization.SelectedLanguageType), KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()), true);
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x001AC768 File Offset: 0x001AA968
	private static string GetLanguageCode(string line)
	{
		string text = null;
		if (line.StartsWith("\"Language:"))
		{
			text = line.Substring("\"Language:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x001AC7C0 File Offset: 0x001AA9C0
	private static Localization.Locale GetLocaleForCode(string code)
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesCode(code))
			{
				result = locale;
				break;
			}
		}
		return result;
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x001AC81C File Offset: 0x001AAA1C
	public static Localization.Locale GetLocale(string[] lines)
	{
		Localization.Locale locale = null;
		string text = null;
		if (lines != null && lines.Length != 0)
		{
			foreach (string text2 in lines)
			{
				if (text2 != null && text2.Length != 0)
				{
					text = Localization.GetLanguageCode(text2);
					if (text != null)
					{
						locale = Localization.GetLocaleForCode(text);
					}
					if (text != null)
					{
						break;
					}
				}
			}
		}
		if (locale == null)
		{
			locale = Localization.GetDefaultLocale();
		}
		if (text != null && locale.Code == "")
		{
			locale.SetCode(text);
		}
		return locale;
	}

	// Token: 0x060049DE RID: 18910 RVA: 0x001AC891 File Offset: 0x001AAA91
	private static string GetFontName(string filename)
	{
		return Localization.GetFontName(File.ReadAllLines(filename, Encoding.UTF8));
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x001AC8A4 File Offset: 0x001AAAA4
	public static Localization.Locale GetDefaultLocale()
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = new Localization.Locale(locale);
				break;
			}
		}
		return result;
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x001AC904 File Offset: 0x001AAB04
	public static string GetDefaultFontName()
	{
		string result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = locale.FontName;
				break;
			}
		}
		return result;
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x001AC964 File Offset: 0x001AAB64
	public static string ValidateFontName(string fontName)
	{
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesFont(fontName))
			{
				return locale.FontName;
			}
		}
		return null;
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x001AC9C4 File Offset: 0x001AABC4
	public static string GetFontName(string[] lines)
	{
		string text = null;
		if (lines != null)
		{
			foreach (string text2 in lines)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					string fontParam = Localization.GetFontParam(text2);
					if (fontParam != null)
					{
						text = Localization.ValidateFontName(fontParam);
					}
				}
				if (text != null)
				{
					break;
				}
			}
		}
		if (text == null)
		{
			if (Localization.sLocale != null)
			{
				text = Localization.sLocale.FontName;
			}
			else
			{
				text = Localization.GetDefaultFontName();
			}
		}
		return text;
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x001ACA27 File Offset: 0x001AAC27
	public static void SwapToLocalizedFont()
	{
		Localization.SwapToLocalizedFont(Localization.currentFontName);
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x001ACA34 File Offset: 0x001AAC34
	public static bool SwapToLocalizedFont(string fontname)
	{
		if (string.IsNullOrEmpty(fontname))
		{
			return false;
		}
		Localization.sFontAsset = Localization.GetFont(fontname);
		foreach (TextStyleSetting textStyleSetting in Resources.FindObjectsOfTypeAll<TextStyleSetting>())
		{
			if (textStyleSetting != null)
			{
				textStyleSetting.sdfFont = Localization.sFontAsset;
			}
		}
		bool isRightToLeft = Localization.IsRightToLeft;
		foreach (LocText locText in Resources.FindObjectsOfTypeAll<LocText>())
		{
			if (locText != null)
			{
				locText.SwapFont(Localization.sFontAsset, isRightToLeft);
			}
		}
		return true;
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x001ACABC File Offset: 0x001AACBC
	private static bool SetFont(Type target_type, object target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		if (target_type == null || target == null || font == null)
		{
			return false;
		}
		foreach (FieldInfo fieldInfo in target_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!excluded_members.Contains(fieldInfo))
			{
				if (fieldInfo.FieldType == typeof(TextStyleSetting))
				{
					((TextStyleSetting)fieldInfo.GetValue(target)).sdfFont = font;
				}
				else if (fieldInfo.FieldType == typeof(LocText))
				{
					((LocText)fieldInfo.GetValue(target)).SwapFont(font, is_right_to_left);
				}
				else if (fieldInfo.FieldType == typeof(GameObject))
				{
					foreach (Component component in ((GameObject)fieldInfo.GetValue(target)).GetComponents<Component>())
					{
						Localization.SetFont(component.GetType(), component, font, is_right_to_left, excluded_members);
					}
				}
				else if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.FieldType != fieldInfo.DeclaringType)
				{
					Localization.SetFont(fieldInfo.FieldType, fieldInfo.GetValue(target), font, is_right_to_left, excluded_members);
				}
			}
		}
		return true;
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x001ACBF5 File Offset: 0x001AADF5
	public static bool SetFont<T>(T target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		return Localization.SetFont(typeof(T), target, font, is_right_to_left, excluded_members);
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x001ACC10 File Offset: 0x001AAE10
	public static TMP_FontAsset GetFont(string fontname)
	{
		foreach (TMP_FontAsset tmp_FontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
		{
			if (tmp_FontAsset.name == fontname)
			{
				return tmp_FontAsset;
			}
		}
		return null;
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x001ACC48 File Offset: 0x001AAE48
	private static bool HasSameOrLessTokenCount(string english_string, string translated_string, string token)
	{
		int num = english_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		int num2 = translated_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		return num >= num2;
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x001ACC84 File Offset: 0x001AAE84
	private static bool HasSameOrLessLinkCountAsEnglish(string english_string, string translated_string)
	{
		return Localization.HasSameOrLessTokenCount(english_string, translated_string, "<link") && Localization.HasSameOrLessTokenCount(english_string, translated_string, "</link");
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x001ACCA4 File Offset: 0x001AAEA4
	private static bool HasMatchingLinkTags(string str, int idx = 0)
	{
		int num = str.IndexOf("<link", idx);
		int num2 = str.IndexOf("</link", idx);
		if (num == -1 && num2 == -1)
		{
			return true;
		}
		if (num == -1 && num2 != -1)
		{
			return false;
		}
		if (num != -1 && num2 == -1)
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		int num3 = str.IndexOf("<link", num + 1);
		return (num < 0 || num3 == -1 || num3 >= num2) && Localization.HasMatchingLinkTags(str, num2 + 1);
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x001ACD18 File Offset: 0x001AAF18
	private static bool AreParametersPreserved(string old_string, string new_string)
	{
		MatchCollection matchCollection = Regex.Matches(old_string, "({.[^}]*?})(?!.*\\1)");
		MatchCollection matchCollection2 = Regex.Matches(new_string, "({.[^}]*?})(?!.*\\1)");
		bool result = false;
		if (matchCollection == null && matchCollection2 == null)
		{
			result = true;
		}
		else if (matchCollection != null && matchCollection2 != null && matchCollection.Count == matchCollection2.Count)
		{
			result = true;
			foreach (object obj in matchCollection)
			{
				string a = obj.ToString();
				bool flag = false;
				foreach (object obj2 in matchCollection2)
				{
					string b = obj2.ToString();
					if (a == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x001ACE10 File Offset: 0x001AB010
	public static bool HasDirtyWords(string str)
	{
		return Localization.FilterDirtyWords(str) != str;
	}

	// Token: 0x060049ED RID: 18925 RVA: 0x001ACE1E File Offset: 0x001AB01E
	public static string FilterDirtyWords(string str)
	{
		return DistributionPlatform.Inst.ApplyWordFilter(str);
	}

	// Token: 0x060049EE RID: 18926 RVA: 0x001ACE2B File Offset: 0x001AB02B
	public static string GetFileDateFormat(int format_idx)
	{
		return "{" + format_idx.ToString() + ":dd / MMM / yyyy}";
	}

	// Token: 0x060049EF RID: 18927 RVA: 0x001ACE44 File Offset: 0x001AB044
	public static void ClearLanguage()
	{
		DebugUtil.LogArgs(new object[]
		{
			" -> Clearing selected language! Either it didn't load correct or returning to english by menu."
		});
		Localization.sFontAsset = null;
		Localization.sLocale = null;
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString());
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, "");
		Localization.SwapToLocalizedFont(Localization.GetDefaultLocale().FontName);
		string defaultLocalizationFilePath = Localization.GetDefaultLocalizationFilePath();
		if (File.Exists(defaultLocalizationFilePath))
		{
			Localization.LoadTranslation(File.ReadAllLines(defaultLocalizationFilePath, Encoding.UTF8), true);
		}
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x001ACED0 File Offset: 0x001AB0D0
	private static string ReverseText(string source)
	{
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = source.Split(separator);
		string text = "";
		int num = 0;
		foreach (string text2 in array)
		{
			num++;
			char[] array3 = new char[text2.Length];
			for (int j = 0; j < text2.Length; j++)
			{
				array3[array3.Length - 1 - j] = text2[j];
			}
			text += new string(array3);
			if (num < array.Length)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x001ACF74 File Offset: 0x001AB174
	public static string Fixup(string text)
	{
		if (Localization.sLocale != null && text != null && text != "" && Localization.sLocale.Lang == Localization.Language.Arabic)
		{
			return Localization.ReverseText(ArabicFixer.Fix(text));
		}
		return text;
	}

	// Token: 0x04003108 RID: 12552
	private static TMP_FontAsset sFontAsset = null;

	// Token: 0x04003109 RID: 12553
	private static readonly List<Localization.Locale> Locales = new List<Localization.Locale>
	{
		new Localization.Locale(Localization.Language.Chinese, Localization.Direction.LeftToRight, "zh", "NotoSansCJKsc-Regular"),
		new Localization.Locale(Localization.Language.Japanese, Localization.Direction.LeftToRight, "ja", "NotoSansCJKjp-Regular"),
		new Localization.Locale(Localization.Language.Korean, Localization.Direction.LeftToRight, "ko", "NotoSansCJKkr-Regular"),
		new Localization.Locale(Localization.Language.Russian, Localization.Direction.LeftToRight, "ru", "RobotoCondensed-Regular"),
		new Localization.Locale(Localization.Language.Thai, Localization.Direction.LeftToRight, "th", "NotoSansThai-Regular"),
		new Localization.Locale(Localization.Language.Arabic, Localization.Direction.RightToLeft, "ar", "NotoNaskhArabic-Regular"),
		new Localization.Locale(Localization.Language.Hebrew, Localization.Direction.RightToLeft, "he", "NotoSansHebrew-Regular"),
		new Localization.Locale(Localization.Language.Unspecified, Localization.Direction.LeftToRight, "", "RobotoCondensed-Regular")
	};

	// Token: 0x0400310A RID: 12554
	private static Localization.Locale sLocale = null;

	// Token: 0x0400310B RID: 12555
	private static string currentFontName = null;

	// Token: 0x0400310C RID: 12556
	public static string DEFAULT_LANGUAGE_CODE = "en";

	// Token: 0x0400310D RID: 12557
	public static readonly List<string> PreinstalledLanguages = new List<string>
	{
		Localization.DEFAULT_LANGUAGE_CODE,
		"zh_klei",
		"ko_klei",
		"ru_klei"
	};

	// Token: 0x0400310E RID: 12558
	public static string SELECTED_LANGUAGE_TYPE_KEY = "SelectedLanguageType";

	// Token: 0x0400310F RID: 12559
	public static string SELECTED_LANGUAGE_CODE_KEY = "SelectedLanguageCode";

	// Token: 0x04003110 RID: 12560
	private static Dictionary<string, List<Assembly>> translatable_assemblies = new Dictionary<string, List<Assembly>>();

	// Token: 0x04003111 RID: 12561
	public const BindingFlags non_static_data_member_fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	// Token: 0x04003112 RID: 12562
	private const string start_link_token = "<link";

	// Token: 0x04003113 RID: 12563
	private const string end_link_token = "</link";

	// Token: 0x02001A48 RID: 6728
	public enum Language
	{
		// Token: 0x0400813C RID: 33084
		Chinese,
		// Token: 0x0400813D RID: 33085
		Japanese,
		// Token: 0x0400813E RID: 33086
		Korean,
		// Token: 0x0400813F RID: 33087
		Russian,
		// Token: 0x04008140 RID: 33088
		Thai,
		// Token: 0x04008141 RID: 33089
		Arabic,
		// Token: 0x04008142 RID: 33090
		Hebrew,
		// Token: 0x04008143 RID: 33091
		Unspecified
	}

	// Token: 0x02001A49 RID: 6729
	public enum Direction
	{
		// Token: 0x04008145 RID: 33093
		LeftToRight,
		// Token: 0x04008146 RID: 33094
		RightToLeft
	}

	// Token: 0x02001A4A RID: 6730
	public class Locale
	{
		// Token: 0x0600A51F RID: 42271 RVA: 0x003B5E9F File Offset: 0x003B409F
		public Locale(Localization.Locale other)
		{
			this.mLanguage = other.mLanguage;
			this.mDirection = other.mDirection;
			this.mCode = other.mCode;
			this.mFontName = other.mFontName;
		}

		// Token: 0x0600A520 RID: 42272 RVA: 0x003B5ED7 File Offset: 0x003B40D7
		public Locale(Localization.Language language, Localization.Direction direction, string code, string fontName)
		{
			this.mLanguage = language;
			this.mDirection = direction;
			this.mCode = code.ToLower();
			this.mFontName = fontName;
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600A521 RID: 42273 RVA: 0x003B5F01 File Offset: 0x003B4101
		public Localization.Language Lang
		{
			get
			{
				return this.mLanguage;
			}
		}

		// Token: 0x0600A522 RID: 42274 RVA: 0x003B5F09 File Offset: 0x003B4109
		public void SetCode(string code)
		{
			this.mCode = code;
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600A523 RID: 42275 RVA: 0x003B5F12 File Offset: 0x003B4112
		public string Code
		{
			get
			{
				return this.mCode;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x0600A524 RID: 42276 RVA: 0x003B5F1A File Offset: 0x003B411A
		public string FontName
		{
			get
			{
				return this.mFontName;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x0600A525 RID: 42277 RVA: 0x003B5F22 File Offset: 0x003B4122
		public bool IsRightToLeft
		{
			get
			{
				return this.mDirection == Localization.Direction.RightToLeft;
			}
		}

		// Token: 0x0600A526 RID: 42278 RVA: 0x003B5F2D File Offset: 0x003B412D
		public bool MatchesCode(string language_code)
		{
			return language_code.ToLower().Contains(this.mCode);
		}

		// Token: 0x0600A527 RID: 42279 RVA: 0x003B5F40 File Offset: 0x003B4140
		public bool MatchesFont(string fontname)
		{
			return fontname.ToLower() == this.mFontName.ToLower();
		}

		// Token: 0x0600A528 RID: 42280 RVA: 0x003B5F58 File Offset: 0x003B4158
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.mCode,
				":",
				this.mLanguage.ToString(),
				":",
				this.mDirection.ToString(),
				":",
				this.mFontName
			});
		}

		// Token: 0x04008147 RID: 33095
		private Localization.Language mLanguage;

		// Token: 0x04008148 RID: 33096
		private string mCode;

		// Token: 0x04008149 RID: 33097
		private string mFontName;

		// Token: 0x0400814A RID: 33098
		private Localization.Direction mDirection;
	}

	// Token: 0x02001A4B RID: 6731
	private struct Entry
	{
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600A529 RID: 42281 RVA: 0x003B5FC2 File Offset: 0x003B41C2
		public bool IsPopulated
		{
			get
			{
				return this.msgctxt != null && this.msgstr != null && this.msgstr.Length > 0;
			}
		}

		// Token: 0x0400814B RID: 33099
		public string msgctxt;

		// Token: 0x0400814C RID: 33100
		public string msgstr;
	}

	// Token: 0x02001A4C RID: 6732
	public enum SelectedLanguageType
	{
		// Token: 0x0400814E RID: 33102
		None,
		// Token: 0x0400814F RID: 33103
		Preinstalled,
		// Token: 0x04008150 RID: 33104
		UGC
	}
}
