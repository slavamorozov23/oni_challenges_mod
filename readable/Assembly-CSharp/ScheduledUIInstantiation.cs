using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E06 RID: 3590
[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
	// Token: 0x060071C8 RID: 29128 RVA: 0x002B7636 File Offset: 0x002B5836
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.InstantiateOnAwake)
		{
			this.InstantiateElements(null);
			return;
		}
		Game.Instance.Subscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
	}

	// Token: 0x060071C9 RID: 29129 RVA: 0x002B766C File Offset: 0x002B586C
	public void InstantiateElements(object data)
	{
		if (this.completed)
		{
			return;
		}
		this.completed = true;
		foreach (ScheduledUIInstantiation.Instantiation instantiation in this.UIElements)
		{
			if (instantiation.RequiredDlcId.IsNullOrWhiteSpace() || Game.IsDlcActiveForCurrentSave(instantiation.RequiredDlcId))
			{
				foreach (GameObject gameObject in instantiation.prefabs)
				{
					Vector3 v = gameObject.rectTransform().anchoredPosition;
					GameObject gameObject2 = Util.KInstantiateUI(gameObject, instantiation.parent.gameObject, false);
					gameObject2.rectTransform().anchoredPosition = v;
					gameObject2.rectTransform().localScale = Vector3.one;
					this.instantiatedObjects.Add(gameObject2);
				}
			}
		}
		if (!this.InstantiateOnAwake)
		{
			base.Unsubscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
		}
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002B775C File Offset: 0x002B595C
	public T GetInstantiatedObject<T>() where T : Component
	{
		for (int i = 0; i < this.instantiatedObjects.Count; i++)
		{
			if (this.instantiatedObjects[i].GetComponent(typeof(T)) != null)
			{
				return this.instantiatedObjects[i].GetComponent(typeof(T)) as T;
			}
		}
		return default(T);
	}

	// Token: 0x04004E8E RID: 20110
	public ScheduledUIInstantiation.Instantiation[] UIElements;

	// Token: 0x04004E8F RID: 20111
	public bool InstantiateOnAwake;

	// Token: 0x04004E90 RID: 20112
	public GameHashes InstantiationEvent = GameHashes.StartGameUser;

	// Token: 0x04004E91 RID: 20113
	private bool completed;

	// Token: 0x04004E92 RID: 20114
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Token: 0x0200208B RID: 8331
	[Serializable]
	public struct Instantiation
	{
		// Token: 0x0400967D RID: 38525
		public string Name;

		// Token: 0x0400967E RID: 38526
		public string Comment;

		// Token: 0x0400967F RID: 38527
		public GameObject[] prefabs;

		// Token: 0x04009680 RID: 38528
		public Transform parent;

		// Token: 0x04009681 RID: 38529
		public string RequiredDlcId;
	}
}
