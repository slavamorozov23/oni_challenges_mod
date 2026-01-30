using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E51 RID: 3665
public class MissileSelectionSideScreen : SideScreenContent
{
	// Token: 0x0600742F RID: 29743 RVA: 0x002C5CDC File Offset: 0x002C3EDC
	public override int GetSideScreenSortOrder()
	{
		return 500;
	}

	// Token: 0x06007430 RID: 29744 RVA: 0x002C5CE3 File Offset: 0x002C3EE3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IMissileSelectionInterface>() != null || target.GetSMI<IMissileSelectionInterface>() != null;
	}

	// Token: 0x06007431 RID: 29745 RVA: 0x002C5CF8 File Offset: 0x002C3EF8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetMissileLauncher = target.GetComponent<IMissileSelectionInterface>();
		if (this.targetMissileLauncher == null)
		{
			this.targetMissileLauncher = target.GetSMI<IMissileSelectionInterface>();
		}
		this.Build();
	}

	// Token: 0x06007432 RID: 29746 RVA: 0x002C5D28 File Offset: 0x002C3F28
	private void Build()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		this.ammunitiontags = this.targetMissileLauncher.GetValidAmmunitionTags();
		this.UpdateLongRangeMissiles();
		foreach (Tag tag in this.ammunitiontags)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x06007433 RID: 29747 RVA: 0x002C5E18 File Offset: 0x002C4018
	private void UpdateLongRangeMissiles()
	{
		if (DlcManager.IsExpansion1Active())
		{
			using (List<Tag>.Enumerator enumerator = MissileLauncherConfig.CosmicBlastShotTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag item = enumerator.Current;
					if (!this.ammunitiontags.Contains(item))
					{
						this.ammunitiontags.Add(item);
					}
				}
				return;
			}
		}
		if (GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash, -1) == null)
		{
			this.ammunitiontags.Remove("MissileLongRange");
			return;
		}
		if (!this.ammunitiontags.Contains("MissileLongRange"))
		{
			this.ammunitiontags.Add("MissileLongRange");
		}
	}

	// Token: 0x06007434 RID: 29748 RVA: 0x002C5EE8 File Offset: 0x002C40E8
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.targetMissileLauncher.ChangeAmmunition(kvp.Key, !this.targetMissileLauncher.AmmunitionIsAllowed(kvp.Key));
					this.targetMissileLauncher.OnRowToggleClick();
					DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.targetMissileLauncher.AmmunitionIsAllowed(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(true);
			}
		}
	}

	// Token: 0x06007435 RID: 29749 RVA: 0x002C607C File Offset: 0x002C427C
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.TITLE;
	}

	// Token: 0x04005062 RID: 20578
	private IMissileSelectionInterface targetMissileLauncher;

	// Token: 0x04005063 RID: 20579
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04005064 RID: 20580
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04005065 RID: 20581
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04005066 RID: 20582
	private List<Tag> ammunitiontags = new List<Tag>
	{
		"MissileBasic"
	};

	// Token: 0x04005067 RID: 20583
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
