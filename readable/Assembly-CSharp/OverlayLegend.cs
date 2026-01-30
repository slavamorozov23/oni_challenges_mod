using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD3 RID: 3539
public class OverlayLegend : KScreen
{
	// Token: 0x06006EC2 RID: 28354 RVA: 0x0029F208 File Offset: 0x0029D408
	[ContextMenu("Set all fonts color")]
	public void SetAllFontsColor()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].fontColor == Color.clear)
				{
					overlayInfo.infoUnits[i].fontColor = Color.white;
				}
			}
		}
	}

	// Token: 0x06006EC3 RID: 28355 RVA: 0x0029F2A0 File Offset: 0x0029D4A0
	[ContextMenu("Set all tooltips")]
	public void SetAllTooltips()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			string text = overlayInfo.name;
			text = text.Replace("NAME", "");
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				string text2 = overlayInfo.infoUnits[i].description;
				text2 = text2.Replace(text, "");
				text2 = text + "TOOLTIPS." + text2;
				overlayInfo.infoUnits[i].tooltip = text2;
			}
		}
	}

	// Token: 0x06006EC4 RID: 28356 RVA: 0x0029F364 File Offset: 0x0029D564
	[ContextMenu("Set Sliced for empty icons")]
	public void SetSlicedForEmptyIcons()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].icon == this.emptySprite)
				{
					overlayInfo.infoUnits[i].sliceIcon = true;
				}
			}
		}
	}

	// Token: 0x06006EC5 RID: 28357 RVA: 0x0029F3F8 File Offset: 0x0029D5F8
	protected override void OnSpawn()
	{
		base.ConsumeMouseScroll = true;
		base.OnSpawn();
		if (OverlayLegend.Instance == null)
		{
			OverlayLegend.Instance = this;
			this.activeUnitObjs = new List<GameObject>();
			this.inactiveUnitObjs = new List<GameObject>();
			foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
			{
				overlayInfo.name = Strings.Get(overlayInfo.name);
				for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
				{
					overlayInfo.infoUnits[i].description = Strings.Get(overlayInfo.infoUnits[i].description);
					if (!string.IsNullOrEmpty(overlayInfo.infoUnits[i].tooltip))
					{
						overlayInfo.infoUnits[i].tooltip = Strings.Get(overlayInfo.infoUnits[i].tooltip);
					}
				}
			}
			base.GetComponent<LayoutElement>().minWidth = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 322 : 288);
			this.ClearLegend();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006EC6 RID: 28358 RVA: 0x0029F554 File Offset: 0x0029D754
	protected override void OnLoadLevel()
	{
		OverlayLegend.Instance = null;
		this.activeDiagrams.Clear();
		UnityEngine.Object.Destroy(base.gameObject);
		base.OnLoadLevel();
	}

	// Token: 0x06006EC7 RID: 28359 RVA: 0x0029F578 File Offset: 0x0029D778
	private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
	{
		if (overlayInfo == null)
		{
			this.ClearLegend();
			return;
		}
		if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
		{
			this.ClearLegend();
			return;
		}
		this.Show(true);
		this.title.text = overlayInfo.name;
		if (overlayInfo.isProgrammaticallyPopulated)
		{
			this.PopulateGeneratedLegend(overlayInfo, false);
		}
		else
		{
			this.PopulateOverlayInfoUnits(overlayInfo, false);
			this.PopulateOverlayDiagrams(overlayInfo, false);
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x06006EC8 RID: 28360 RVA: 0x0029F5F4 File Offset: 0x0029D7F4
	public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
	{
		if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
		{
			return;
		}
		this.ClearLegend();
		OverlayLegend.OverlayInfo legend = this.overlayInfoList.Find((OverlayLegend.OverlayInfo ol) => ol.mode == mode.ViewMode());
		this.currentMode = mode;
		this.SetLegend(legend);
	}

	// Token: 0x06006EC9 RID: 28361 RVA: 0x0029F668 File Offset: 0x0029D868
	public GameObject GetFreeUnitObject()
	{
		if (this.inactiveUnitObjs.Count == 0)
		{
			this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
		}
		GameObject gameObject = this.inactiveUnitObjs[0];
		this.inactiveUnitObjs.RemoveAt(0);
		this.activeUnitObjs.Add(gameObject);
		return gameObject;
	}

	// Token: 0x06006ECA RID: 28362 RVA: 0x0029F6C8 File Offset: 0x0029D8C8
	private void RemoveActiveObjects()
	{
		while (this.activeUnitObjs.Count > 0)
		{
			this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
			this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
			this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
			this.activeUnitObjs[0].SetActive(false);
			this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
			this.activeUnitObjs.RemoveAt(0);
		}
	}

	// Token: 0x06006ECB RID: 28363 RVA: 0x0029F77E File Offset: 0x0029D97E
	public void ClearLegend()
	{
		this.RemoveActiveObjects();
		this.ClearFilters();
		this.ClearDiagrams();
		this.Show(false);
	}

	// Token: 0x06006ECC RID: 28364 RVA: 0x0029F799 File Offset: 0x0029D999
	public void ClearFilters()
	{
		if (this.filterMenu != null)
		{
			UnityEngine.Object.Destroy(this.filterMenu.gameObject);
		}
		this.filterMenu = null;
	}

	// Token: 0x06006ECD RID: 28365 RVA: 0x0029F7C0 File Offset: 0x0029D9C0
	public void ClearDiagrams()
	{
		for (int i = 0; i < this.activeDiagrams.Count; i++)
		{
			if (this.activeDiagrams[i] != null)
			{
				UnityEngine.Object.Destroy(this.activeDiagrams[i]);
			}
		}
		this.activeDiagrams.Clear();
		Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = 0f;
		this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}

	// Token: 0x06006ECE RID: 28366 RVA: 0x0029F844 File Offset: 0x0029DA44
	public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
	{
		for (int i = 0; i < this.overlayInfoList.Count; i++)
		{
			if (this.overlayInfoList[i].mode == mode.ViewMode())
			{
				return this.overlayInfoList[i];
			}
		}
		return null;
	}

	// Token: 0x06006ECF RID: 28367 RVA: 0x0029F894 File Offset: 0x0029DA94
	private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<OverlayLegend.OverlayInfoUnit>.Enumerator enumerator = overlayInfo.infoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OverlayLegend.OverlayInfoUnit overlayInfoUnit = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					if (overlayInfoUnit.icon != null)
					{
						Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
						component.gameObject.SetActive(true);
						component.sprite = overlayInfoUnit.icon;
						component.color = overlayInfoUnit.color;
						component.enabled = true;
						component.type = (overlayInfoUnit.sliceIcon ? Image.Type.Sliced : Image.Type.Simple);
					}
					else
					{
						freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
					}
					if (!string.IsNullOrEmpty(overlayInfoUnit.description))
					{
						LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
						componentInChildren.text = string.Format(overlayInfoUnit.description, overlayInfoUnit.formatData);
						componentInChildren.color = overlayInfoUnit.fontColor;
						componentInChildren.enabled = true;
					}
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					if (!string.IsNullOrEmpty(overlayInfoUnit.tooltip))
					{
						component2.toolTip = string.Format(overlayInfoUnit.tooltip, overlayInfoUnit.tooltipFormatData);
						component2.enabled = true;
					}
					else
					{
						component2.enabled = false;
					}
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				return;
			}
		}
		this.activeUnitsParent.SetActive(false);
	}

	// Token: 0x06006ED0 RID: 28368 RVA: 0x0029FA40 File Offset: 0x0029DC40
	private void PopulateOverlayDiagrams(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (!isRefresh)
		{
			if (overlayInfo.mode == OverlayModes.Temperature.ID)
			{
				Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
				if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
				{
					if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
					{
						this.ClearDiagrams();
						overlayInfo = this.overlayInfoList.Find((OverlayLegend.OverlayInfo match) => match.name == UI.OVERLAYS.RELATIVETEMPERATURE.NAME);
					}
				}
				else
				{
					SimDebugView.Instance.user_temperatureThresholds[0] = 0f;
					SimDebugView.Instance.user_temperatureThresholds[1] = 2073f;
				}
			}
			if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
			{
				this.diagramsParent.SetActive(true);
				using (List<GameObject>.Enumerator enumerator = overlayInfo.diagrams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject original = enumerator.Current;
						GameObject item = Util.KInstantiateUI(original, this.diagramsParent, false);
						this.activeDiagrams.Add(item);
					}
					return;
				}
			}
			this.diagramsParent.SetActive(false);
		}
	}

	// Token: 0x06006ED1 RID: 28369 RVA: 0x0029FB5C File Offset: 0x0029DD5C
	private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
	{
		if (isRefresh)
		{
			this.RemoveActiveObjects();
			this.ClearDiagrams();
		}
		if (info.infoUnits != null && info.infoUnits.Count > 0)
		{
			this.PopulateOverlayInfoUnits(info, isRefresh);
		}
		this.PopulateOverlayDiagrams(info, false);
		List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
		if (customLegendData != null)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<LegendEntry>.Enumerator enumerator = customLegendData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LegendEntry legendEntry = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
					component.gameObject.SetActive(legendEntry.displaySprite);
					component.sprite = legendEntry.sprite;
					component.color = legendEntry.colour;
					component.enabled = true;
					component.type = Image.Type.Simple;
					LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
					componentInChildren.text = legendEntry.name;
					componentInChildren.color = Color.white;
					componentInChildren.enabled = true;
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					component2.enabled = (legendEntry.desc != null || legendEntry.desc_arg != null);
					component2.toolTip = ((legendEntry.desc_arg == null) ? legendEntry.desc : string.Format(legendEntry.desc, legendEntry.desc_arg));
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_165;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_165:
		if (!isRefresh && this.currentMode.legendFilters != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent.transform.parent.gameObject, false);
			gameObject.transform.SetAsFirstSibling();
			this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
			this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
			this.filterMenu.onParametersChanged += this.OnFiltersChanged;
			this.OnFiltersChanged();
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x06006ED2 RID: 28370 RVA: 0x0029FD60 File Offset: 0x0029DF60
	private void OnFiltersChanged()
	{
		this.currentMode.OnFiltersChanged();
		this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
		Game.Instance.ForceOverlayUpdate(false);
	}

	// Token: 0x06006ED3 RID: 28371 RVA: 0x0029FD8B File Offset: 0x0029DF8B
	private void DisableOverlay()
	{
		this.filterMenu.onParametersChanged -= this.OnFiltersChanged;
		this.filterMenu.ClearMenu();
		this.filterMenu.gameObject.SetActive(false);
		this.filterMenu = null;
	}

	// Token: 0x06006ED4 RID: 28372 RVA: 0x0029FDC8 File Offset: 0x0029DFC8
	private void ConfigureUIHeight()
	{
		this.scrollRectLayout.enabled = false;
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		this.scrollRectLayout.preferredWidth = this.scrollRectLayout.rectTransform().sizeDelta.x;
		float y = this.scrollRectLayout.rectTransform().sizeDelta.y;
		this.scrollRectLayout.preferredHeight = Mathf.Min(y, 512f);
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = false;
		this.scrollRectLayout.enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
	}

	// Token: 0x04004BBE RID: 19390
	public static OverlayLegend Instance;

	// Token: 0x04004BBF RID: 19391
	[SerializeField]
	private LocText title;

	// Token: 0x04004BC0 RID: 19392
	[SerializeField]
	private Sprite emptySprite;

	// Token: 0x04004BC1 RID: 19393
	[SerializeField]
	private List<OverlayLegend.OverlayInfo> overlayInfoList;

	// Token: 0x04004BC2 RID: 19394
	[SerializeField]
	private GameObject unitPrefab;

	// Token: 0x04004BC3 RID: 19395
	[SerializeField]
	private GameObject activeUnitsParent;

	// Token: 0x04004BC4 RID: 19396
	[SerializeField]
	private GameObject diagramsParent;

	// Token: 0x04004BC5 RID: 19397
	[SerializeField]
	private GameObject inactiveUnitsParent;

	// Token: 0x04004BC6 RID: 19398
	[SerializeField]
	private GameObject toolParameterMenuPrefab;

	// Token: 0x04004BC7 RID: 19399
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x04004BC8 RID: 19400
	private ToolParameterMenu filterMenu;

	// Token: 0x04004BC9 RID: 19401
	private OverlayModes.Mode currentMode;

	// Token: 0x04004BCA RID: 19402
	private List<GameObject> inactiveUnitObjs;

	// Token: 0x04004BCB RID: 19403
	private List<GameObject> activeUnitObjs;

	// Token: 0x04004BCC RID: 19404
	private List<GameObject> activeDiagrams = new List<GameObject>();

	// Token: 0x02002034 RID: 8244
	[Serializable]
	public class OverlayInfoUnit
	{
		// Token: 0x0600B890 RID: 47248 RVA: 0x003F6460 File Offset: 0x003F4660
		public OverlayInfoUnit(Sprite icon, string description, Color color, Color fontColor, object formatData = null, bool sliceIcon = false)
		{
			this.icon = icon;
			this.description = description;
			this.color = color;
			this.fontColor = fontColor;
			this.formatData = formatData;
			this.sliceIcon = sliceIcon;
		}

		// Token: 0x04009541 RID: 38209
		public Sprite icon;

		// Token: 0x04009542 RID: 38210
		public string description;

		// Token: 0x04009543 RID: 38211
		public string tooltip;

		// Token: 0x04009544 RID: 38212
		public Color color;

		// Token: 0x04009545 RID: 38213
		public Color fontColor;

		// Token: 0x04009546 RID: 38214
		public object formatData;

		// Token: 0x04009547 RID: 38215
		public object tooltipFormatData;

		// Token: 0x04009548 RID: 38216
		public bool sliceIcon;
	}

	// Token: 0x02002035 RID: 8245
	[Serializable]
	public class OverlayInfo
	{
		// Token: 0x04009549 RID: 38217
		public string name;

		// Token: 0x0400954A RID: 38218
		public HashedString mode;

		// Token: 0x0400954B RID: 38219
		public List<OverlayLegend.OverlayInfoUnit> infoUnits;

		// Token: 0x0400954C RID: 38220
		public List<GameObject> diagrams;

		// Token: 0x0400954D RID: 38221
		public bool isProgrammaticallyPopulated;
	}
}
