using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class WorkableReactable : Reactable
{
	// Token: 0x06001C1F RID: 7199 RVA: 0x0009B538 File Offset: 0x00099738
	public WorkableReactable(Workable workable, HashedString id, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable.gameObject, id, chore_type, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
	{
		this.workable = workable;
		this.allowedDirection = allowed_direction;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x0009B57C File Offset: 0x0009977C
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.workable == null)
		{
			return false;
		}
		if (this.reactor != null)
		{
			return false;
		}
		Brain component = new_reactor.GetComponent<Brain>();
		if (component == null)
		{
			return false;
		}
		if (!component.IsRunning())
		{
			return false;
		}
		Navigator component2 = new_reactor.GetComponent<Navigator>();
		if (component2 == null)
		{
			return false;
		}
		if (!component2.IsMoving())
		{
			return false;
		}
		if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
		{
			return true;
		}
		Facing component3 = new_reactor.GetComponent<Facing>();
		if (component3 == null)
		{
			return false;
		}
		bool facing = component3.GetFacing();
		return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x0009B621 File Offset: 0x00099821
	protected override void InternalBegin()
	{
		this.worker = this.reactor.GetComponent<WorkerBase>();
		this.worker.StartWork(new WorkerBase.StartWorkInfo(this.workable));
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x0009B64A File Offset: 0x0009984A
	public override void Update(float dt)
	{
		if (this.worker.GetWorkable() == null)
		{
			base.End();
			return;
		}
		if (this.worker.Work(dt) != WorkerBase.WorkResult.InProgress)
		{
			base.End();
		}
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x0009B67B File Offset: 0x0009987B
	protected override void InternalEnd()
	{
		if (this.worker != null)
		{
			this.worker.StopWork();
		}
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x0009B696 File Offset: 0x00099896
	protected override void InternalCleanup()
	{
	}

	// Token: 0x0400109A RID: 4250
	protected Workable workable;

	// Token: 0x0400109B RID: 4251
	private WorkerBase worker;

	// Token: 0x0400109C RID: 4252
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x020013A4 RID: 5028
	public enum AllowedDirection
	{
		// Token: 0x04006C0C RID: 27660
		Any,
		// Token: 0x04006C0D RID: 27661
		Left,
		// Token: 0x04006C0E RID: 27662
		Right
	}
}
