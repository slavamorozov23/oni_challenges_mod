using System;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02000F01 RID: 3841
	public class AccessabilityManager : KMonoBehaviour
	{
		// Token: 0x06007B96 RID: 31638 RVA: 0x00300402 File Offset: 0x002FE602
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Components.FoodRehydrators.Add(base.gameObject);
			base.Subscribe(824508782, new Action<object>(this.ActiveChangedHandler));
		}

		// Token: 0x06007B97 RID: 31639 RVA: 0x00300432 File Offset: 0x002FE632
		protected override void OnCleanUp()
		{
			Components.FoodRehydrators.Remove(base.gameObject);
			base.OnCleanUp();
		}

		// Token: 0x06007B98 RID: 31640 RVA: 0x0030044A File Offset: 0x002FE64A
		public void Reserve(GameObject reserver)
		{
			this.reserver = reserver;
			global::Debug.Assert(reserver != null && reserver.GetComponent<MinionResume>() != null);
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x00300470 File Offset: 0x002FE670
		public void Unreserve()
		{
			this.activeWorkable = null;
			this.reserver = null;
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x00300480 File Offset: 0x002FE680
		public void SetActiveWorkable(Workable work)
		{
			DebugUtil.DevAssert(this.activeWorkable == null || work == null, "FoodRehydrator::AccessabilityManager activating a second workable", null);
			this.activeWorkable = work;
			this.operational.SetActive(this.activeWorkable != null, false);
		}

		// Token: 0x06007B9B RID: 31643 RVA: 0x003004CF File Offset: 0x002FE6CF
		public bool CanAccess(GameObject worker)
		{
			return this.operational.IsOperational && (this.reserver == null || this.reserver == worker);
		}

		// Token: 0x06007B9C RID: 31644 RVA: 0x003004FC File Offset: 0x002FE6FC
		protected void ActiveChangedHandler(object obj)
		{
			if (!this.operational.IsActive)
			{
				this.CancelActiveWorkable();
			}
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x00300511 File Offset: 0x002FE711
		public void CancelActiveWorkable()
		{
			if (this.activeWorkable != null)
			{
				this.activeWorkable.StopWork(this.activeWorkable.worker, true);
			}
		}

		// Token: 0x0400563B RID: 22075
		[MyCmpReq]
		private Operational operational;

		// Token: 0x0400563C RID: 22076
		private GameObject reserver;

		// Token: 0x0400563D RID: 22077
		private Workable activeWorkable;
	}
}
