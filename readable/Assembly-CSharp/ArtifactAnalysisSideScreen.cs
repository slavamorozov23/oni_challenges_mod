using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E14 RID: 3604
public class ArtifactAnalysisSideScreen : SideScreenContent
{
	// Token: 0x06007236 RID: 29238 RVA: 0x002BA428 File Offset: 0x002B8628
	public override string GetTitle()
	{
		if (this.targetArtifactStation != null)
		{
			return string.Format(base.GetTitle(), this.targetArtifactStation.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x06007237 RID: 29239 RVA: 0x002BA455 File Offset: 0x002B8655
	public override void ClearTarget()
	{
		this.targetArtifactStation = null;
		base.ClearTarget();
	}

	// Token: 0x06007238 RID: 29240 RVA: 0x002BA464 File Offset: 0x002B8664
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x06007239 RID: 29241 RVA: 0x002BA470 File Offset: 0x002B8670
	private void RefreshRows()
	{
		if (this.undiscoveredRow == null)
		{
			this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
			HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
			component.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
			component.GetReference<Image>("icon").sprite = Assets.GetSprite("unknown");
			component.GetReference<Image>("icon").color = Color.grey;
		}
		List<string> analyzedArtifactIDs = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
		this.undiscoveredRow.SetActive(analyzedArtifactIDs.Count == 0);
		foreach (string text in analyzedArtifactIDs)
		{
			if (!this.rows.ContainsKey(text))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(text, gameObject);
				GameObject artifactPrefab = Assets.GetPrefab(text);
				HierarchyReferences component2 = gameObject.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
				component2.GetReference<Image>("icon").sprite = Def.GetUISprite(artifactPrefab, text, false).first;
				component2.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenEvent(artifactPrefab);
				};
			}
		}
	}

	// Token: 0x0600723A RID: 29242 RVA: 0x002BA620 File Offset: 0x002B8820
	private void OpenEvent(GameObject artifactPrefab)
	{
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.artifact = artifactPrefab;
		artifactPrefab.GetComponent<KPrefabID>();
		artifactPrefab.GetComponent<InfoDescription>();
		string text = artifactPrefab.PrefabID().Name.ToUpper();
		text = text.Replace("ARTIFACT_", "");
		string key = "STRINGS.UI.SPACEARTIFACTS." + text + ".ARTIFACT";
		string text2 = string.Format("<b>{0}</b>", artifactPrefab.GetProperName());
		StringEntry stringEntry;
		Strings.TryGet(key, out stringEntry);
		if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
		{
			text2 = text2 + "\n\n" + stringEntry.String;
		}
		if (text2 != null && !text2.IsNullOrWhiteSpace())
		{
			statesInstance.SetTextParameter("desc", text2);
		}
		statesInstance.ShowEventPopup();
	}

	// Token: 0x0600723B RID: 29243 RVA: 0x002BA6F6 File Offset: 0x002B88F6
	public override void SetTarget(GameObject target)
	{
		this.targetArtifactStation = target;
		base.SetTarget(target);
		this.RefreshRows();
	}

	// Token: 0x04004EE5 RID: 20197
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004EE6 RID: 20198
	private GameObject targetArtifactStation;

	// Token: 0x04004EE7 RID: 20199
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x04004EE8 RID: 20200
	private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

	// Token: 0x04004EE9 RID: 20201
	private GameObject undiscoveredRow;
}
