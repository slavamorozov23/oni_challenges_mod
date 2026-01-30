using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001055 RID: 4181
	public class EmoteStep
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600817B RID: 33147 RVA: 0x0033F2BF File Offset: 0x0033D4BF
		public int Id
		{
			get
			{
				return this.anim.HashValue;
			}
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x0033F2CC File Offset: 0x0033D4CC
		public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(Action<GameObject> startedCb, Action<GameObject> finishedCb)
		{
			if (startedCb == null && finishedCb == null)
			{
				return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
			}
			EmoteStep.Callbacks item = new EmoteStep.Callbacks
			{
				StartedCb = startedCb,
				FinishedCb = finishedCb
			};
			return this.callbacks.Add(item);
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x0033F30B File Offset: 0x0033D50B
		public void UnregisterCallbacks(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
		{
			this.callbacks.Release(callbackHandle);
		}

		// Token: 0x0600817E RID: 33150 RVA: 0x0033F31A File Offset: 0x0033D51A
		public void UnregisterAllCallbacks()
		{
			this.callbacks = new HandleVector<EmoteStep.Callbacks>(64);
		}

		// Token: 0x0600817F RID: 33151 RVA: 0x0033F32C File Offset: 0x0033D52C
		public void OnStepStarted(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.StartedCb != null)
			{
				item.StartedCb(parameter);
			}
		}

		// Token: 0x06008180 RID: 33152 RVA: 0x0033F368 File Offset: 0x0033D568
		public void OnStepFinished(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.FinishedCb != null)
			{
				item.FinishedCb(parameter);
			}
		}

		// Token: 0x040061FE RID: 25086
		public HashedString anim = HashedString.Invalid;

		// Token: 0x040061FF RID: 25087
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		// Token: 0x04006200 RID: 25088
		public float timeout = -1f;

		// Token: 0x04006201 RID: 25089
		private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64);

		// Token: 0x02002750 RID: 10064
		public struct Callbacks
		{
			// Token: 0x0400AEE2 RID: 44770
			public Action<GameObject> StartedCb;

			// Token: 0x0400AEE3 RID: 44771
			public Action<GameObject> FinishedCb;
		}
	}
}
