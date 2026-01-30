using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
public class StatusItemStackTraceWatcher : IDisposable
{
	// Token: 0x06002A2D RID: 10797 RVA: 0x000F7046 File Offset: 0x000F5246
	public bool GetShouldWatch()
	{
		return this.shouldWatch;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000F704E File Offset: 0x000F524E
	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch == shouldWatch)
		{
			return;
		}
		this.shouldWatch = shouldWatch;
		this.Refresh();
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x000F7067 File Offset: 0x000F5267
	public Option<StatusItemGroup> GetTarget()
	{
		return this.currentTarget;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x000F7070 File Offset: 0x000F5270
	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if (this.currentTarget.IsNone() && nextTarget.IsNone())
		{
			return;
		}
		if (this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
		{
			return;
		}
		this.currentTarget = nextTarget;
		this.Refresh();
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000F70CC File Offset: 0x000F52CC
	private void Refresh()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
		if (!this.shouldWatch)
		{
			return;
		}
		if (this.currentTarget.IsSome())
		{
			StatusItemGroup target = this.currentTarget.Unwrap();
			Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
			{
				this.entryIdToStackTraceMap[entry.id] = new StackTrace(true);
			};
			StatusItemGroup target3 = target;
			target3.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(target3.OnAddStatusItem, onAddStatusItem);
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				StatusItemGroup target2 = target;
				target2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(target2.OnAddStatusItem, onAddStatusItem);
			}));
			StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
			destroyListener.owner = this;
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				if (destroyListener.IsNullOrDestroyed())
				{
					return;
				}
				UnityEngine.Object.Destroy(destroyListener);
			}));
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				this.entryIdToStackTraceMap.Clear();
			}));
		}
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000F71E9 File Offset: 0x000F53E9
	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000F71FD File Offset: 0x000F53FD
	public void Dispose()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
	}

	// Token: 0x04001914 RID: 6420
	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	// Token: 0x04001915 RID: 6421
	private Option<StatusItemGroup> currentTarget;

	// Token: 0x04001916 RID: 6422
	private bool shouldWatch;

	// Token: 0x04001917 RID: 6423
	private System.Action onCleanup;

	// Token: 0x0200156F RID: 5487
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		// Token: 0x06009349 RID: 37705 RVA: 0x003755F0 File Offset: 0x003737F0
		private void OnDestroy()
		{
			bool flag = this.owner != null;
			bool flag2 = this.owner.currentTarget.IsSome() && this.owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (flag && flag2)
			{
				this.owner.SetTarget(Option.None);
			}
		}

		// Token: 0x040071C8 RID: 29128
		public StatusItemStackTraceWatcher owner;
	}
}
