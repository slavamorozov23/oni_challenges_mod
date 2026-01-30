using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000974 RID: 2420
public class GlobalAssets : KMonoBehaviour
{
	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x0600450C RID: 17676 RVA: 0x00190137 File Offset: 0x0018E337
	// (set) Token: 0x0600450D RID: 17677 RVA: 0x0019013E File Offset: 0x0018E33E
	public static GlobalAssets Instance { get; private set; }

	// Token: 0x0600450E RID: 17678 RVA: 0x00190148 File Offset: 0x0018E348
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalAssets.Instance = this;
		if (GlobalAssets.SoundTable.Count == 0)
		{
			Bank[] array = null;
			try
			{
				if (RuntimeManager.StudioSystem.getBankList(out array) != RESULT.OK)
				{
					array = null;
				}
			}
			catch
			{
				array = null;
			}
			if (array != null)
			{
				foreach (Bank bank in array)
				{
					EventDescription[] array3;
					RESULT eventList = bank.getEventList(out array3);
					if (eventList != RESULT.OK)
					{
						string text;
						bank.getPath(out text);
						global::Debug.LogError(string.Format("ERROR [{0}] loading FMOD events for bank [{1}]", eventList, text));
					}
					else
					{
						foreach (EventDescription eventDescription in array3)
						{
							string text;
							eventDescription.getPath(out text);
							if (text == null)
							{
								bank.getPath(out text);
								GUID guid;
								eventDescription.getID(out guid);
								global::Debug.LogError(string.Format("Got a FMOD event with a null path! {0} {1} in bank {2}", eventDescription.ToString(), guid, text));
							}
							else
							{
								string text2 = Assets.GetSimpleSoundEventName(text);
								text2 = text2.ToLowerInvariant();
								if (text2.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(text2))
								{
									GlobalAssets.SoundTable[text2] = text;
									if (text.ToLower().Contains("lowpriority") || text2.Contains("lowpriority"))
									{
										GlobalAssets.LowPrioritySounds.Add(text);
									}
									else if (text.ToLower().Contains("highpriority") || text2.Contains("highpriority"))
									{
										GlobalAssets.HighPrioritySounds.Add(text);
									}
								}
							}
						}
					}
				}
			}
		}
		SetDefaults.Initialize();
		GraphicsOptionsScreen.SetColorModeFromPrefs();
		this.AddColorModeStyles();
		LocString.CreateLocStringKeys(typeof(UI), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(GAMEPLAY_EVENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROOMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(BUILDING.DETAILS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(SETITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(COLONY_ACHIEVEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CREATURES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(RESEARCH), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(DUPLICANTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROBOTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ELEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(MISC), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(VIDEOS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(NAMEGEN), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CLUSTER_NAMES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLD_TRAITS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT_BINDINGS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(LORE), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CODEX), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BLUEPRINTS), "STRINGS.");
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x00190518 File Offset: 0x0018E718
	private void AddColorModeStyles()
	{
		TMP_Style style = new TMP_Style("logic_on", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOn)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style);
		TMP_Style style2 = new TMP_Style("logic_off", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOff)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style2);
		TMP_StyleSheet.RefreshStyles();
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x0019059E File Offset: 0x0018E79E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GlobalAssets.Instance = null;
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x001905AC File Offset: 0x0018E7AC
	public static string GetSound(string name, bool force_no_warning = false)
	{
		if (name == null)
		{
			return null;
		}
		name = name.ToLowerInvariant();
		string result = null;
		GlobalAssets.SoundTable.TryGetValue(name, out result);
		return result;
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x001905D7 File Offset: 0x0018E7D7
	public static bool IsLowPriority(string path)
	{
		return GlobalAssets.LowPrioritySounds.Contains(path);
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x001905E4 File Offset: 0x0018E7E4
	public static bool IsHighPriority(string path)
	{
		return GlobalAssets.HighPrioritySounds.Contains(path);
	}

	// Token: 0x04002E4A RID: 11850
	private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();

	// Token: 0x04002E4B RID: 11851
	private static HashSet<string> LowPrioritySounds = new HashSet<string>();

	// Token: 0x04002E4C RID: 11852
	private static HashSet<string> HighPrioritySounds = new HashSet<string>();

	// Token: 0x04002E4E RID: 11854
	public ColorSet colorSet;

	// Token: 0x04002E4F RID: 11855
	public ColorSet[] colorSetOptions;
}
