using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000DEF RID: 3567
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
	// Token: 0x06007045 RID: 28741 RVA: 0x002AA4D8 File Offset: 0x002A86D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techLineMap = new Dictionary<Tech, UILineRenderer>();
		this.BG.color = this.defaultColor;
		this.QueueStateChanged(false);
		if (this.targetTech != null)
		{
			using (List<TechInstance>.Enumerator enumerator = Research.Instance.GetResearchQueue().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.tech == this.targetTech)
					{
						this.QueueStateChanged(true);
					}
				}
			}
		}
		base.StartCoroutine(this.DelayedSetupForLines());
	}

	// Token: 0x06007046 RID: 28742 RVA: 0x002AA57C File Offset: 0x002A877C
	private IEnumerator DelayedSetupForLines()
	{
		yield return null;
		yield return null;
		this.SetupLines();
		yield break;
	}

	// Token: 0x06007047 RID: 28743 RVA: 0x002AA58C File Offset: 0x002A878C
	public void SetupLines()
	{
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			float num = this.researchScreenReference.GetEntry(this.targetTech).rectTransform().rect.width / 2f;
			float num2 = this.researchScreenReference.GetEntry(tech).rectTransform().rect.width / 2f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (tech.center.y > this.targetTech.center.y + 2f)
			{
				zero = new Vector2(0f, 20f);
				zero2 = new Vector2(0f, -20f);
			}
			else if (tech.center.y < this.targetTech.center.y - 2f)
			{
				zero = new Vector2(0f, -20f);
				zero2 = new Vector2(0f, 20f);
			}
			UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
			float num3 = 32f;
			component.Points = new Vector2[]
			{
				new Vector2(0f, 0f) + zero,
				new Vector2(-num3, 0f) + zero,
				new Vector2(-num3, tech.center.y - this.targetTech.center.y) + zero2,
				new Vector2(-(this.targetTech.center.x - num - (tech.center.x + num2)) - 4f, tech.center.y - this.targetTech.center.y) + zero2
			};
			component.LineThickness = (float)this.lineThickness_inactive;
			component.color = this.inactiveLineColor;
			this.techLineMap.Add(tech, component);
		}
	}

	// Token: 0x06007048 RID: 28744 RVA: 0x002AA7F8 File Offset: 0x002A89F8
	public void SetTech(Tech newTech)
	{
		if (newTech == null)
		{
			global::Debug.LogError("The research provided is null!");
			return;
		}
		if (this.targetTech == newTech)
		{
			return;
		}
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (newTech.costsByResearchTypeID.ContainsKey(researchType.id) && newTech.costsByResearchTypeID[researchType.id] > 0f)
			{
				GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
				Image image = gameObject.GetComponentsInChildren<Image>()[2];
				Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
				image.color = researchType.color;
				component.sprite = researchType.sprite;
				this.progressBarsByResearchTypeID[researchType.id] = gameObject;
			}
		}
		if (this.researchScreen == null)
		{
			this.researchScreen = base.transform.parent.GetComponentInParent<ResearchScreen>();
		}
		if (newTech.IsComplete())
		{
			this.ResearchCompleted(false);
		}
		this.targetTech = newTech;
		this.researchName.text = this.targetTech.Name;
		string text = "";
		foreach (TechItem techItem in this.targetTech.unlockedItems)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(techItem))
			{
				HierarchyReferences component2 = this.GetFreeIcon().GetComponent<HierarchyReferences>();
				if (text != "")
				{
					text += ", ";
				}
				text += techItem.Name;
				component2.GetReference<KImage>("Icon").sprite = techItem.UISprite();
				component2.GetReference<KImage>("Background");
				KImage reference = component2.GetReference<KImage>("DLCOverlay");
				bool flag = techItem.requiredDlcIds != null;
				reference.gameObject.SetActive(flag);
				if (flag)
				{
					reference.color = DlcManager.GetDlcBannerColor(techItem.requiredDlcIds[techItem.requiredDlcIds.Length - 1]);
				}
				string text2 = string.Format("{0}\n{1}", techItem.Name, techItem.description);
				if (flag)
				{
					text2 += "\n";
					foreach (string dlcId in techItem.requiredDlcIds)
					{
						text2 += string.Format(RESEARCH.MESSAGING.DLC.DLC_CONTENT, DlcManager.GetDlcTitle(dlcId));
					}
				}
				component2.GetComponent<ToolTip>().toolTip = text2;
			}
		}
		text = string.Format(UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, text);
		this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", this.targetTech.Name, this.targetTech.desc, text);
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.toggle.onPointerEnter += delegate()
		{
			this.researchScreen.TurnEverythingOff();
			this.OnHover(true, this.targetTech);
		};
		this.toggle.soundPlayer.AcceptClickCondition = (() => !this.targetTech.IsComplete());
		this.toggle.onPointerExit += delegate()
		{
			this.researchScreen.TurnEverythingOff();
		};
	}

	// Token: 0x06007049 RID: 28745 RVA: 0x002AAB88 File Offset: 0x002A8D88
	public void SetEverythingOff()
	{
		if (!this.isOn)
		{
			return;
		}
		this.borderHighlight.gameObject.SetActive(false);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_inactive;
			keyValuePair.Value.Points[0].x = 0f;
			keyValuePair.Value.color = this.inactiveLineColor;
		}
		this.isOn = false;
	}

	// Token: 0x0600704A RID: 28746 RVA: 0x002AAC38 File Offset: 0x002A8E38
	public void SetEverythingOn()
	{
		if (this.isOn)
		{
			return;
		}
		this.UpdateProgressBars();
		this.borderHighlight.gameObject.SetActive(true);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_active;
			keyValuePair.Value.Points[0].x = (float)(-(float)this.lineThickness_inactive);
			keyValuePair.Value.color = this.activeLineColor;
		}
		base.transform.SetAsLastSibling();
		this.isOn = true;
	}

	// Token: 0x0600704B RID: 28747 RVA: 0x002AACFC File Offset: 0x002A8EFC
	public void OnHover(bool entered, Tech hoverSource)
	{
		this.SetEverythingOn();
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			ResearchEntry entry = this.researchScreen.GetEntry(tech);
			if (entry != null)
			{
				entry.OnHover(entered, this.targetTech);
			}
		}
	}

	// Token: 0x0600704C RID: 28748 RVA: 0x002AAD78 File Offset: 0x002A8F78
	private void OnResearchClicked()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null && activeResearch.tech != this.targetTech)
		{
			this.researchScreen.CancelResearch();
		}
		Research.Instance.SetActiveResearch(this.targetTech, true);
		if (DebugHandler.InstantBuildMode)
		{
			Research.Instance.CompleteQueue();
		}
		this.UpdateProgressBars();
	}

	// Token: 0x0600704D RID: 28749 RVA: 0x002AADD4 File Offset: 0x002A8FD4
	private void OnResearchCanceled()
	{
		if (this.targetTech.IsComplete())
		{
			return;
		}
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.researchScreen.CancelResearch();
		Research.Instance.CancelResearch(this.targetTech, true);
	}

	// Token: 0x0600704E RID: 28750 RVA: 0x002AAE30 File Offset: 0x002A9030
	public void QueueStateChanged(bool isSelected)
	{
		if (isSelected)
		{
			if (!this.targetTech.IsComplete())
			{
				this.toggle.isOn = true;
				this.BG.color = this.pendingColor;
				this.titleBG.color = this.pendingHeaderColor;
				this.toggle.ClearOnClick();
				this.toggle.onClick += this.OnResearchCanceled;
			}
			else
			{
				this.toggle.isOn = false;
			}
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
			{
				keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		if (this.targetTech.IsComplete())
		{
			this.toggle.isOn = false;
			this.BG.color = this.completedColor;
			this.titleBG.color = this.completedHeaderColor;
			this.defaultColor = this.completedColor;
			this.toggle.ClearOnClick();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.progressBarsByResearchTypeID)
			{
				keyValuePair2.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		this.toggle.isOn = false;
		this.BG.color = this.defaultColor;
		this.titleBG.color = this.incompleteHeaderColor;
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.progressBarsByResearchTypeID)
		{
			keyValuePair3.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.52156866f, 0.52156866f, 0.52156866f);
		}
	}

	// Token: 0x0600704F RID: 28751 RVA: 0x002AB0D4 File Offset: 0x002A92D4
	public void UpdateFilterState(bool state)
	{
		this.filterLowlight.gameObject.SetActive(!state);
	}

	// Token: 0x06007050 RID: 28752 RVA: 0x002AB0F7 File Offset: 0x002A92F7
	public void SetPercentage(float percent)
	{
	}

	// Token: 0x06007051 RID: 28753 RVA: 0x002AB0FC File Offset: 0x002A92FC
	public void UpdateProgressBars()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
		{
			Transform child = keyValuePair.Value.transform.GetChild(0);
			float fillAmount;
			if (this.targetTech.IsComplete())
			{
				fillAmount = 1f;
				child.GetComponentInChildren<LocText>().text = this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
			}
			else
			{
				TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
				if (orAdd == null)
				{
					continue;
				}
				child.GetComponentInChildren<LocText>().text = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
				fillAmount = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
			}
			child.GetComponentsInChildren<Image>()[2].fillAmount = fillAmount;
			child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
		}
	}

	// Token: 0x06007052 RID: 28754 RVA: 0x002AB2B4 File Offset: 0x002A94B4
	private GameObject GetFreeIcon()
	{
		GameObject gameObject = Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06007053 RID: 28755 RVA: 0x002AB2CF File Offset: 0x002A94CF
	private Image GetFreeLine()
	{
		return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x06007054 RID: 28756 RVA: 0x002AB2E8 File Offset: 0x002A94E8
	public void ResearchCompleted(bool notify = true)
	{
		this.BG.color = this.completedColor;
		this.titleBG.color = this.completedHeaderColor;
		this.defaultColor = this.completedColor;
		if (notify)
		{
			this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = this.targetTech.Id;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, "ResearchCompleted");
		}
		this.toggle.ClearOnClick();
		if (notify)
		{
			ResearchCompleteMessage message = new ResearchCompleteMessage(this.targetTech);
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x04004D18 RID: 19736
	[Header("Labels")]
	[SerializeField]
	private LocText researchName;

	// Token: 0x04004D19 RID: 19737
	[Header("Transforms")]
	[SerializeField]
	private Transform progressBarContainer;

	// Token: 0x04004D1A RID: 19738
	[SerializeField]
	private Transform lineContainer;

	// Token: 0x04004D1B RID: 19739
	[Header("Prefabs")]
	[SerializeField]
	private GameObject iconPanel;

	// Token: 0x04004D1C RID: 19740
	[SerializeField]
	private GameObject iconPrefab;

	// Token: 0x04004D1D RID: 19741
	[SerializeField]
	private GameObject linePrefab;

	// Token: 0x04004D1E RID: 19742
	[SerializeField]
	private GameObject progressBarPrefab;

	// Token: 0x04004D1F RID: 19743
	[Header("Graphics")]
	[SerializeField]
	private Image BG;

	// Token: 0x04004D20 RID: 19744
	[SerializeField]
	private Image titleBG;

	// Token: 0x04004D21 RID: 19745
	[SerializeField]
	private Image borderHighlight;

	// Token: 0x04004D22 RID: 19746
	[SerializeField]
	private Image filterHighlight;

	// Token: 0x04004D23 RID: 19747
	[SerializeField]
	private Image filterLowlight;

	// Token: 0x04004D24 RID: 19748
	[SerializeField]
	private Sprite hoverBG;

	// Token: 0x04004D25 RID: 19749
	[SerializeField]
	private Sprite completedBG;

	// Token: 0x04004D26 RID: 19750
	[Header("Colors")]
	[SerializeField]
	private Color defaultColor = Color.blue;

	// Token: 0x04004D27 RID: 19751
	[SerializeField]
	private Color completedColor = Color.yellow;

	// Token: 0x04004D28 RID: 19752
	[SerializeField]
	private Color pendingColor = Color.magenta;

	// Token: 0x04004D29 RID: 19753
	[SerializeField]
	private Color completedHeaderColor = Color.grey;

	// Token: 0x04004D2A RID: 19754
	[SerializeField]
	private Color incompleteHeaderColor = Color.grey;

	// Token: 0x04004D2B RID: 19755
	[SerializeField]
	private Color pendingHeaderColor = Color.grey;

	// Token: 0x04004D2C RID: 19756
	private Sprite defaultBG;

	// Token: 0x04004D2D RID: 19757
	[MyCmpGet]
	private KToggle toggle;

	// Token: 0x04004D2E RID: 19758
	private ResearchScreen researchScreen;

	// Token: 0x04004D2F RID: 19759
	private Dictionary<Tech, UILineRenderer> techLineMap;

	// Token: 0x04004D30 RID: 19760
	private Tech targetTech;

	// Token: 0x04004D31 RID: 19761
	private bool isOn = true;

	// Token: 0x04004D32 RID: 19762
	private Coroutine fadeRoutine;

	// Token: 0x04004D33 RID: 19763
	public Color activeLineColor;

	// Token: 0x04004D34 RID: 19764
	public Color inactiveLineColor;

	// Token: 0x04004D35 RID: 19765
	public int lineThickness_active = 6;

	// Token: 0x04004D36 RID: 19766
	public int lineThickness_inactive = 2;

	// Token: 0x04004D37 RID: 19767
	public Material StandardUIMaterial;

	// Token: 0x04004D38 RID: 19768
	public ResearchScreen researchScreenReference;

	// Token: 0x04004D39 RID: 19769
	private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();

	// Token: 0x04004D3A RID: 19770
	public static readonly string UnlockedTechKey = "UnlockedTech";

	// Token: 0x04004D3B RID: 19771
	private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>
	{
		{
			ResearchEntry.UnlockedTechKey,
			null
		}
	};
}
