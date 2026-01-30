using System;
using UnityEngine;

// Token: 0x02000DDC RID: 3548
public class PlanSubCategoryToggle : KMonoBehaviour
{
	// Token: 0x06006F88 RID: 28552 RVA: 0x002A5C3F File Offset: 0x002A3E3F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	// Token: 0x04004C4E RID: 19534
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04004C4F RID: 19535
	[SerializeField]
	private GameObject gridContainer;

	// Token: 0x04004C50 RID: 19536
	private bool open = true;
}
