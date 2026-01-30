using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class EventInfoData
{
	// Token: 0x06001ACB RID: 6859 RVA: 0x0009390C File Offset: 0x00091B0C
	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x0009395A File Offset: 0x00091B5A
	public List<EventInfoData.Option> GetOptions()
	{
		this.FinalizeText();
		return this.options;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x00093968 File Offset: 0x00091B68
	public EventInfoData.Option AddOption(string mainText, string description = null)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			description = description
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000939A0 File Offset: 0x00091BA0
	public EventInfoData.Option SimpleOption(string mainText, System.Action callback)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			callback = callback
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000939D5 File Offset: 0x00091BD5
	public EventInfoData.Option AddDefaultOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000939E8 File Offset: 0x00091BE8
	public EventInfoData.Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000939FB File Offset: 0x00091BFB
	public void SetTextParameter(string key, string value)
	{
		this.textParameters[key] = value;
		this.dirty = true;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x00093A14 File Offset: 0x00091C14
	public void FinalizeText()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		foreach (KeyValuePair<string, string> keyValuePair in this.textParameters)
		{
			string oldValue = "{" + keyValuePair.Key + "}";
			if (this.title != null)
			{
				this.title = this.title.Replace(oldValue, keyValuePair.Value);
			}
			if (this.description != null)
			{
				this.description = this.description.Replace(oldValue, keyValuePair.Value);
			}
			if (this.location != null)
			{
				this.location = this.location.Replace(oldValue, keyValuePair.Value);
			}
			if (this.whenDescription != null)
			{
				this.whenDescription = this.whenDescription.Replace(oldValue, keyValuePair.Value);
			}
			foreach (EventInfoData.Option option in this.options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, keyValuePair.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, keyValuePair.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, keyValuePair.Value);
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					if (optionIcon.tooltip != null)
					{
						optionIcon.tooltip = optionIcon.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					if (optionIcon2.tooltip != null)
					{
						optionIcon2.tooltip = optionIcon2.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
			}
		}
	}

	// Token: 0x04000F66 RID: 3942
	public string title;

	// Token: 0x04000F67 RID: 3943
	public string description;

	// Token: 0x04000F68 RID: 3944
	public string location;

	// Token: 0x04000F69 RID: 3945
	public string whenDescription;

	// Token: 0x04000F6A RID: 3946
	public Transform clickFocus;

	// Token: 0x04000F6B RID: 3947
	public GameObject[] minions;

	// Token: 0x04000F6C RID: 3948
	public GameObject artifact;

	// Token: 0x04000F6D RID: 3949
	public HashedString animFileName;

	// Token: 0x04000F6E RID: 3950
	public HashedString mainAnim = "event";

	// Token: 0x04000F6F RID: 3951
	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	// Token: 0x04000F70 RID: 3952
	public List<EventInfoData.Option> options = new List<EventInfoData.Option>();

	// Token: 0x04000F71 RID: 3953
	public System.Action showCallback;

	// Token: 0x04000F72 RID: 3954
	private bool dirty;

	// Token: 0x02001351 RID: 4945
	public class OptionIcon
	{
		// Token: 0x06008B89 RID: 35721 RVA: 0x0035F91F File Offset: 0x0035DB1F
		public OptionIcon(Sprite sprite, EventInfoData.OptionIcon.ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}

		// Token: 0x04006AEA RID: 27370
		public EventInfoData.OptionIcon.ContainerType containerType;

		// Token: 0x04006AEB RID: 27371
		public Sprite sprite;

		// Token: 0x04006AEC RID: 27372
		public string tooltip;

		// Token: 0x04006AED RID: 27373
		public float scale;

		// Token: 0x020027FA RID: 10234
		public enum ContainerType
		{
			// Token: 0x0400B136 RID: 45366
			Neutral,
			// Token: 0x0400B137 RID: 45367
			Positive,
			// Token: 0x0400B138 RID: 45368
			Negative,
			// Token: 0x0400B139 RID: 45369
			Information
		}
	}

	// Token: 0x02001352 RID: 4946
	public class Option
	{
		// Token: 0x06008B8A RID: 35722 RVA: 0x0035F944 File Offset: 0x0035DB44
		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			this.informationIcons.Add(new EventInfoData.OptionIcon(null, EventInfoData.OptionIcon.ContainerType.Information, tooltip, scale));
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x0035F95A File Offset: 0x0035DB5A
		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		// Token: 0x06008B8C RID: 35724 RVA: 0x0035F970 File Offset: 0x0035DB70
		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		// Token: 0x06008B8D RID: 35725 RVA: 0x0035F986 File Offset: 0x0035DB86
		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Negative, tooltip, scale));
		}

		// Token: 0x04006AEE RID: 27374
		public string mainText;

		// Token: 0x04006AEF RID: 27375
		public string description;

		// Token: 0x04006AF0 RID: 27376
		public string tooltip;

		// Token: 0x04006AF1 RID: 27377
		public System.Action callback;

		// Token: 0x04006AF2 RID: 27378
		public List<EventInfoData.OptionIcon> informationIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x04006AF3 RID: 27379
		public List<EventInfoData.OptionIcon> consequenceIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x04006AF4 RID: 27380
		public bool allowed = true;
	}
}
