using System;
using UnityEngine;

// Token: 0x02000EAA RID: 3754
public abstract class TargetScreen : KScreen
{
	// Token: 0x06007839 RID: 30777
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600783A RID: 30778 RVA: 0x002E3D10 File Offset: 0x002E1F10
	public virtual void SetTarget(GameObject target)
	{
		if (this.selectedTarget != target)
		{
			if (this.selectedTarget != null)
			{
				this.OnDeselectTarget(this.selectedTarget);
			}
			this.selectedTarget = target;
			if (this.selectedTarget != null)
			{
				this.OnSelectTarget(this.selectedTarget);
			}
		}
	}

	// Token: 0x0600783B RID: 30779 RVA: 0x002E3D66 File Offset: 0x002E1F66
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.SetTarget(null);
	}

	// Token: 0x0600783C RID: 30780 RVA: 0x002E3D75 File Offset: 0x002E1F75
	public virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600783D RID: 30781 RVA: 0x002E3D8F File Offset: 0x002E1F8F
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600783E RID: 30782 RVA: 0x002E3DA8 File Offset: 0x002E1FA8
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x040053CA RID: 21450
	protected GameObject selectedTarget;
}
