using System;
using UnityEngine;

// Token: 0x02000D78 RID: 3448
public class PortraitTableColumn : TableColumn
{
	// Token: 0x06006AEB RID: 27371 RVA: 0x00287BA4 File Offset: 0x00285DA4
	public PortraitTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true) : base(on_load_action, sort_comparison, null, null, null, false, "")
	{
		this.double_click_to_target = double_click_to_target;
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x00287BD3 File Offset: 0x00285DD3
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		gameObject.GetComponent<CrewPortrait>().targetImage.enabled = true;
		return gameObject;
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x00287BF3 File Offset: 0x00285DF3
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(this.prefab_portrait, parent, true);
	}

	// Token: 0x06006AEE RID: 27374 RVA: 0x00287C04 File Offset: 0x00285E04
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		if (this.double_click_to_target)
		{
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectMinion();
			};
			gameObject.GetComponent<KButton>().onDoubleClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectAndFocusMinion();
			};
		}
		return gameObject;
	}

	// Token: 0x04004997 RID: 18839
	public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;

	// Token: 0x04004998 RID: 18840
	private bool double_click_to_target;
}
