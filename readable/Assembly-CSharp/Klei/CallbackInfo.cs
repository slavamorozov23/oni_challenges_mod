using System;

namespace Klei
{
	// Token: 0x02001008 RID: 4104
	public struct CallbackInfo
	{
		// Token: 0x06007F84 RID: 32644 RVA: 0x00335256 File Offset: 0x00333456
		public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
		{
			this.handle = h;
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x00335260 File Offset: 0x00333460
		public void Release()
		{
			if (this.handle.IsValid())
			{
				Game.CallbackInfo item = Game.Instance.callbackManager.GetItem(this.handle);
				System.Action cb = item.cb;
				if (!item.manuallyRelease)
				{
					Game.Instance.callbackManager.Release(this.handle);
				}
				cb();
			}
		}

		// Token: 0x04006097 RID: 24727
		private HandleVector<Game.CallbackInfo>.Handle handle;
	}
}
