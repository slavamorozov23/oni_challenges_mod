using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000EF9 RID: 3833
	internal class OldExample : KMonoBehaviour2
	{
		// Token: 0x06007B3E RID: 31550 RVA: 0x002FFE3C File Offset: 0x002FE03C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
			bool flag = false;
			base.Trigger(0, flag);
		}

		// Token: 0x06007B3F RID: 31551 RVA: 0x002FFE71 File Offset: 0x002FE071
		private void OnObjectDestroyed(object data)
		{
			Debug.Log((bool)data);
		}
	}
}
