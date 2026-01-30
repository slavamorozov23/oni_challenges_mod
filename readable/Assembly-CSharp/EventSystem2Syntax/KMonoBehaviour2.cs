using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000EFB RID: 3835
	internal class KMonoBehaviour2
	{
		// Token: 0x06007B44 RID: 31556 RVA: 0x002FFECC File Offset: 0x002FE0CC
		protected virtual void OnPrefabInit()
		{
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x002FFECE File Offset: 0x002FE0CE
		public void Subscribe(int evt, Action<object> cb)
		{
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x002FFED0 File Offset: 0x002FE0D0
		public void Trigger(int evt, object data)
		{
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x002FFED2 File Offset: 0x002FE0D2
		public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
		{
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x002FFED4 File Offset: 0x002FE0D4
		public void Trigger<EventType>(EventType evt) where EventType : IEventData
		{
		}
	}
}
