using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D48 RID: 3400
public static class KleiItemsStatusRefresher
{
	// Token: 0x0600695C RID: 26972 RVA: 0x0027E764 File Offset: 0x0027C964
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
	}

	// Token: 0x0600695D RID: 26973 RVA: 0x0027E778 File Offset: 0x0027C978
	private static void OnRefreshResponseFromServer()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x0027E7C8 File Offset: 0x0027C9C8
	public static void Refresh()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x0600695F RID: 26975 RVA: 0x0027E818 File Offset: 0x0027CA18
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
	{
		return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
	}

	// Token: 0x06006960 RID: 26976 RVA: 0x0027E825 File Offset: 0x0027CA25
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
	}

	// Token: 0x0400486E RID: 18542
	public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

	// Token: 0x02001F81 RID: 8065
	public class UIListener : MonoBehaviour
	{
		// Token: 0x0600B688 RID: 46728 RVA: 0x003F1078 File Offset: 0x003EF278
		public void Internal_RefreshUI()
		{
			if (this.refreshUIFn != null)
			{
				this.refreshUIFn();
			}
		}

		// Token: 0x0600B689 RID: 46729 RVA: 0x003F108D File Offset: 0x003EF28D
		public void OnRefreshUI(System.Action fn)
		{
			this.refreshUIFn = fn;
		}

		// Token: 0x0600B68A RID: 46730 RVA: 0x003F1096 File Offset: 0x003EF296
		private void OnEnable()
		{
			KleiItemsStatusRefresher.listeners.Add(this);
		}

		// Token: 0x0600B68B RID: 46731 RVA: 0x003F10A4 File Offset: 0x003EF2A4
		private void OnDisable()
		{
			KleiItemsStatusRefresher.listeners.Remove(this);
		}

		// Token: 0x0400931A RID: 37658
		private System.Action refreshUIFn;
	}
}
