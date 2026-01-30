using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E18 RID: 3608
public class AssignmentGroupControllerSideScreen : KScreen
{
	// Token: 0x06007268 RID: 29288 RVA: 0x002BB79C File Offset: 0x002B999C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshRows();
		}
	}

	// Token: 0x06007269 RID: 29289 RVA: 0x002BB7B0 File Offset: 0x002B99B0
	protected override void OnCmpDisable()
	{
		for (int i = 0; i < this.identityRowMap.Count; i++)
		{
			UnityEngine.Object.Destroy(this.identityRowMap[i]);
		}
		this.identityRowMap.Clear();
		base.OnCmpDisable();
	}

	// Token: 0x0600726A RID: 29290 RVA: 0x002BB7F5 File Offset: 0x002B99F5
	public void SetTarget(GameObject target)
	{
		this.target = target.GetComponent<AssignmentGroupController>();
		this.RefreshRows();
	}

	// Token: 0x0600726B RID: 29291 RVA: 0x002BB80C File Offset: 0x002B9A0C
	private void RefreshRows()
	{
		int num = 0;
		WorldContainer myWorld = this.target.GetMyWorld();
		ClustercraftExteriorDoor component = this.target.GetComponent<ClustercraftExteriorDoor>();
		if (component != null)
		{
			myWorld = component.GetInteriorDoor().GetMyWorld();
		}
		List<AssignmentGroupControllerSideScreen.RowSortHelper> list = new List<AssignmentGroupControllerSideScreen.RowSortHelper>();
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			MinionAssignablesProxy minionAssignablesProxy = Components.MinionAssignablesProxy[i];
			GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
			WorldContainer myWorld2 = targetGameObject.GetMyWorld();
			if (!(targetGameObject == null) && !targetGameObject.HasTag(GameTags.Dead))
			{
				MinionResume component2 = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				bool isPilot = false;
				if (component2 != null && component2.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					isPilot = true;
				}
				bool isSameWorld = myWorld2.ParentWorldId == myWorld.ParentWorldId;
				list.Add(new AssignmentGroupControllerSideScreen.RowSortHelper
				{
					minion = minionAssignablesProxy,
					isPilot = isPilot,
					isSameWorld = isSameWorld
				});
			}
		}
		list.Sort(delegate(AssignmentGroupControllerSideScreen.RowSortHelper a, AssignmentGroupControllerSideScreen.RowSortHelper b)
		{
			int num2 = b.isSameWorld.CompareTo(a.isSameWorld);
			if (num2 != 0)
			{
				return num2;
			}
			return b.isPilot.CompareTo(a.isPilot);
		});
		foreach (AssignmentGroupControllerSideScreen.RowSortHelper rowSortHelper in list)
		{
			MinionAssignablesProxy minion = rowSortHelper.minion;
			GameObject gameObject;
			if (num >= this.identityRowMap.Count)
			{
				gameObject = Util.KInstantiateUI(this.minionRowPrefab, this.minionRowContainer, true);
				this.identityRowMap.Add(gameObject);
			}
			else
			{
				gameObject = this.identityRowMap[num];
				gameObject.SetActive(true);
			}
			num++;
			HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
			MultiToggle toggle = component3.GetReference<MultiToggle>("Toggle");
			toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
			component3.GetReference<CrewPortrait>("Portrait").SetIdentityObject(minion, false);
			LocText reference = component3.GetReference<LocText>("Label");
			LocText reference2 = component3.GetReference<LocText>("Designation");
			if (rowSortHelper.isSameWorld)
			{
				if (rowSortHelper.isPilot)
				{
					reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.PILOT;
				}
				else
				{
					reference2.text = "";
				}
				reference.color = Color.black;
				reference2.color = Color.black;
			}
			else
			{
				reference.color = Color.grey;
				reference2.color = Color.grey;
				reference2.text = UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.OFFWORLD;
				gameObject.transform.SetAsLastSibling();
			}
			toggle.onClick = delegate()
			{
				this.target.SetMember(minion, !this.target.CheckMinionIsMember(minion));
				toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
				this.RefreshRows();
			};
			string simpleTooltip = this.UpdateToolTip(minion, !rowSortHelper.isSameWorld);
			toggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		}
		for (int j = num; j < this.identityRowMap.Count; j++)
		{
			this.identityRowMap[j].SetActive(false);
		}
		this.minionRowContainer.GetComponent<QuickLayout>().ForceUpdate();
	}

	// Token: 0x0600726C RID: 29292 RVA: 0x002BBB70 File Offset: 0x002B9D70
	private string UpdateToolTip(MinionAssignablesProxy minion, bool offworld)
	{
		string text = this.target.CheckMinionIsMember(minion) ? UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.UNASSIGN : UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.ASSIGN;
		if (offworld)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n\n",
				UIConstants.ColorPrefixYellow,
				UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.DIFFERENT_WORLD,
				UIConstants.ColorSuffix
			});
		}
		return text;
	}

	// Token: 0x04004F0A RID: 20234
	[SerializeField]
	private GameObject header;

	// Token: 0x04004F0B RID: 20235
	[SerializeField]
	private GameObject minionRowPrefab;

	// Token: 0x04004F0C RID: 20236
	[SerializeField]
	private GameObject footer;

	// Token: 0x04004F0D RID: 20237
	[SerializeField]
	private GameObject minionRowContainer;

	// Token: 0x04004F0E RID: 20238
	private AssignmentGroupController target;

	// Token: 0x04004F0F RID: 20239
	private List<GameObject> identityRowMap = new List<GameObject>();

	// Token: 0x020020A0 RID: 8352
	private struct RowSortHelper
	{
		// Token: 0x040096BA RID: 38586
		public MinionAssignablesProxy minion;

		// Token: 0x040096BB RID: 38587
		public bool isPilot;

		// Token: 0x040096BC RID: 38588
		public bool isSameWorld;
	}
}
