using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E82 RID: 3714
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	// Token: 0x06007634 RID: 30260 RVA: 0x002D18FF File Offset: 0x002CFAFF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06007635 RID: 30261 RVA: 0x002D1907 File Offset: 0x002CFB07
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	// Token: 0x06007636 RID: 30262 RVA: 0x002D1915 File Offset: 0x002CFB15
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x06007637 RID: 30263 RVA: 0x002D1918 File Offset: 0x002CFB18
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.SetDescriptionSidescreenFoldState(this.targetReceptacle != null && this.targetReceptacle.Occupant == null);
	}

	// Token: 0x06007638 RID: 30264 RVA: 0x002D194C File Offset: 0x002CFB4C
	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (this.targetReceptacle != null && this.targetReceptacle.Occupant != null)
		{
			this.descriptionLabel.SetText("");
			this.SetDescriptionSidescreenFoldState(false);
			return;
		}
		this.SetDescriptionSidescreenFoldState(true);
	}

	// Token: 0x06007639 RID: 30265 RVA: 0x002D19A0 File Offset: 0x002CFBA0
	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		this.descriptionContent.minHeight = (visible ? this.descriptionLayoutDefaultSize : 0f);
	}

	// Token: 0x040051C3 RID: 20931
	public LayoutElement descriptionContent;

	// Token: 0x040051C4 RID: 20932
	public float descriptionLayoutDefaultSize = -1f;
}
