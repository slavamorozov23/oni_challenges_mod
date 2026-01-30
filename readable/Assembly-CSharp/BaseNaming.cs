using System;
using System.IO;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C47 RID: 3143
[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
	// Token: 0x06005F1D RID: 24349 RVA: 0x0022C51C File Offset: 0x0022A71C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GenerateBaseName();
		this.shuffleBaseNameButton.onClick += this.GenerateBaseName;
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnEditing));
		this.minionSelectScreen = base.GetComponent<MinionSelectScreen>();
	}

	// Token: 0x06005F1E RID: 24350 RVA: 0x0022C590 File Offset: 0x0022A790
	private bool CheckBaseName(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			return true;
		}
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (this.minionSelectScreen != null)
		{
			bool flag = false;
			try
			{
				bool flag2 = Directory.Exists(Path.Combine(savePrefixAndCreateFolder, newName));
				bool flag3 = cloudSavePrefix != null && Directory.Exists(Path.Combine(cloudSavePrefix, newName));
				flag = (flag2 || flag3);
			}
			catch (Exception arg)
			{
				flag = true;
				global::Debug.Log(string.Format("Base Naming / Warning / {0}", arg));
			}
			if (flag)
			{
				this.minionSelectScreen.SetProceedButtonActive(false, string.Format(UI.IMMIGRANTSCREEN.DUPLICATE_COLONY_NAME, newName));
				return false;
			}
			this.minionSelectScreen.SetProceedButtonActive(true, null);
		}
		return true;
	}

	// Token: 0x06005F1F RID: 24351 RVA: 0x0022C640 File Offset: 0x0022A840
	private void OnEditing(string newName)
	{
		Util.ScrubInputField(this.inputField, false, false);
		this.CheckBaseName(this.inputField.text);
	}

	// Token: 0x06005F20 RID: 24352 RVA: 0x0022C664 File Offset: 0x0022A864
	private void OnEndEdit(string newName)
	{
		if (Localization.HasDirtyWords(newName))
		{
			this.inputField.text = this.GenerateBaseNameString();
			newName = this.inputField.text;
		}
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		if (newName.EndsWith(" "))
		{
			newName = newName.TrimEnd(' ');
		}
		if (!this.CheckBaseName(newName))
		{
			return;
		}
		this.inputField.text = newName;
		SaveGame.Instance.SetBaseName(newName);
		string path = Path.ChangeExtension(newName, ".sav");
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		string path2 = savePrefixAndCreateFolder;
		if (SaveLoader.GetCloudSavesAvailable() && Game.Instance.SaveToCloudActive && cloudSavePrefix != null)
		{
			path2 = cloudSavePrefix;
		}
		SaveLoader.SetActiveSaveFilePath(Path.Combine(path2, newName, path));
	}

	// Token: 0x06005F21 RID: 24353 RVA: 0x0022C718 File Offset: 0x0022A918
	private void GenerateBaseName()
	{
		string text = this.GenerateBaseNameString();
		((LocText)this.inputField.placeholder).text = text;
		this.inputField.text = text;
		this.OnEndEdit(text);
	}

	// Token: 0x06005F22 RID: 24354 RVA: 0x0022C758 File Offset: 0x0022A958
	private string GenerateBaseNameString()
	{
		string fullString = LocString.GetStrings(typeof(NAMEGEN.COLONY.FORMATS)).GetRandom<string>();
		fullString = this.ReplaceStringWithRandom(fullString, "{noun}", LocString.GetStrings(typeof(NAMEGEN.COLONY.NOUN)));
		string[] strings = LocString.GetStrings(typeof(NAMEGEN.COLONY.ADJECTIVE));
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective2}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective3}", strings);
		return this.ReplaceStringWithRandom(fullString, "{adjective4}", strings);
	}

	// Token: 0x06005F23 RID: 24355 RVA: 0x0022C7DF File Offset: 0x0022A9DF
	private string ReplaceStringWithRandom(string fullString, string replacementKey, string[] replacementValues)
	{
		if (!fullString.Contains(replacementKey))
		{
			return fullString;
		}
		return fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
	}

	// Token: 0x04003F79 RID: 16249
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04003F7A RID: 16250
	[SerializeField]
	private KButton shuffleBaseNameButton;

	// Token: 0x04003F7B RID: 16251
	private MinionSelectScreen minionSelectScreen;
}
