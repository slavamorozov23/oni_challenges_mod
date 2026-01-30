using System;
using UnityEngine;

// Token: 0x02000EA9 RID: 3753
public abstract class TargetPanel : KMonoBehaviour
{
	// Token: 0x06007833 RID: 30771
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x06007834 RID: 30772 RVA: 0x002E3C68 File Offset: 0x002E1E68
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

	// Token: 0x06007835 RID: 30773 RVA: 0x002E3CBE File Offset: 0x002E1EBE
	protected virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x06007836 RID: 30774 RVA: 0x002E3CD8 File Offset: 0x002E1ED8
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x06007837 RID: 30775 RVA: 0x002E3CF1 File Offset: 0x002E1EF1
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x040053C9 RID: 21449
	protected GameObject selectedTarget;
}
