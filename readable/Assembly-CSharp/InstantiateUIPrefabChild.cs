using System;
using UnityEngine;

// Token: 0x02000D32 RID: 3378
[AddComponentMenu("KMonoBehaviour/scripts/InstantiateUIPrefabChild")]
public class InstantiateUIPrefabChild : KMonoBehaviour
{
	// Token: 0x06006863 RID: 26723 RVA: 0x00275DA7 File Offset: 0x00273FA7
	protected override void OnPrefabInit()
	{
		if (this.InstantiateOnAwake)
		{
			this.Instantiate();
		}
	}

	// Token: 0x06006864 RID: 26724 RVA: 0x00275DB8 File Offset: 0x00273FB8
	public void Instantiate()
	{
		if (this.alreadyInstantiated)
		{
			global::Debug.LogWarning(base.gameObject.name + "trying to instantiate UI prefabs multiple times.");
			return;
		}
		this.alreadyInstantiated = true;
		foreach (GameObject gameObject in this.prefabs)
		{
			if (!(gameObject == null))
			{
				Vector3 v = gameObject.rectTransform().anchoredPosition;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.rectTransform().anchoredPosition = v;
				gameObject2.rectTransform().localScale = Vector3.one;
				if (this.setAsFirstSibling)
				{
					gameObject2.transform.SetAsFirstSibling();
				}
			}
		}
	}

	// Token: 0x040047B4 RID: 18356
	public GameObject[] prefabs;

	// Token: 0x040047B5 RID: 18357
	public bool InstantiateOnAwake = true;

	// Token: 0x040047B6 RID: 18358
	private bool alreadyInstantiated;

	// Token: 0x040047B7 RID: 18359
	public bool setAsFirstSibling;
}
