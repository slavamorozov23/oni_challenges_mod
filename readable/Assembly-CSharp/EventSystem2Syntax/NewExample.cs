using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000EFA RID: 3834
	internal class NewExample : KMonoBehaviour2
	{
		// Token: 0x06007B41 RID: 31553 RVA: 0x002FFE8C File Offset: 0x002FE08C
		protected override void OnPrefabInit()
		{
			base.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
			base.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent
			{
				parameter = false
			});
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x002FFEC2 File Offset: 0x002FE0C2
		private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
		{
		}

		// Token: 0x02002180 RID: 8576
		private struct ObjectDestroyedEvent : IEventData
		{
			// Token: 0x0400997E RID: 39294
			public bool parameter;
		}
	}
}
