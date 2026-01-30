using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C44 RID: 3140
public class UIGameObjectPool
{
	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06005F03 RID: 24323 RVA: 0x0022BDC9 File Offset: 0x00229FC9
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06005F04 RID: 24324 RVA: 0x0022BDD6 File Offset: 0x00229FD6
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06005F05 RID: 24325 RVA: 0x0022BDE3 File Offset: 0x00229FE3
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06005F06 RID: 24326 RVA: 0x0022BDF2 File Offset: 0x00229FF2
	public UIGameObjectPool(GameObject prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<GameObject>();
		this.activeElements = new List<GameObject>();
	}

	// Token: 0x06005F07 RID: 24327 RVA: 0x0022BE30 File Offset: 0x0022A030
	public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			GameObject gameObject = this.freeElements[0];
			this.activeElements.Add(gameObject);
			if (gameObject.transform.parent != instantiateParent)
			{
				gameObject.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		GameObject gameObject2 = this.activeElements[this.activeElements.Count - 1];
		if (gameObject2.gameObject.activeInHierarchy != forceActive)
		{
			gameObject2.gameObject.SetActive(forceActive);
		}
		return gameObject2;
	}

	// Token: 0x06005F08 RID: 24328 RVA: 0x0022BEE8 File Offset: 0x0022A0E8
	public void ClearElement(GameObject element)
	{
		if (!this.activeElements.Contains(element))
		{
			object obj = this.freeElements.Contains(element) ? (element.name + ": The element provided is already inactive") : (element.name + ": The element provided does not belong to this pool");
			element.SetActive(false);
			if (this.disabledElementParent != null)
			{
				element.transform.SetParent(this.disabledElementParent);
			}
			global::Debug.LogError(obj);
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06005F09 RID: 24329 RVA: 0x0022BFA0 File Offset: 0x0022A1A0
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06005F0A RID: 24330 RVA: 0x0022C01C File Offset: 0x0022A21C
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06005F0B RID: 24331 RVA: 0x0022C02A File Offset: 0x0022A22A
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06005F0C RID: 24332 RVA: 0x0022C061 File Offset: 0x0022A261
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06005F0D RID: 24333 RVA: 0x0022C098 File Offset: 0x0022A298
	public void ForEachActiveElement(Action<GameObject> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06005F0E RID: 24334 RVA: 0x0022C0A6 File Offset: 0x0022A2A6
	public void ForEachFreeElement(Action<GameObject> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04003F71 RID: 16241
	private GameObject prefab;

	// Token: 0x04003F72 RID: 16242
	private List<GameObject> freeElements = new List<GameObject>();

	// Token: 0x04003F73 RID: 16243
	private List<GameObject> activeElements = new List<GameObject>();

	// Token: 0x04003F74 RID: 16244
	public Transform disabledElementParent;
}
