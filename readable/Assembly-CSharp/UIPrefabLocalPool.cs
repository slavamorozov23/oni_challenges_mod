using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class UIPrefabLocalPool
{
	// Token: 0x0600186B RID: 6251 RVA: 0x0008853C File Offset: 0x0008673C
	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x00088568 File Offset: 0x00086768
	public GameObject Borrow()
	{
		GameObject gameObject;
		if (this.availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
		}
		else
		{
			gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
			this.availableInstances.Remove(gameObject.GetInstanceID());
		}
		this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000885E2 File Offset: 0x000867E2
	public void Return(GameObject instance)
	{
		this.checkedOutInstances.Remove(instance.GetInstanceID());
		this.availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(false);
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00088610 File Offset: 0x00086810
	public void ReturnAll()
	{
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.checkedOutInstances)
		{
			int num;
			GameObject gameObject;
			keyValuePair.Deconstruct(out num, out gameObject);
			int key = num;
			GameObject gameObject2 = gameObject;
			this.availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(false);
		}
		this.checkedOutInstances.Clear();
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x0008868C File Offset: 0x0008688C
	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return this.checkedOutInstances.Values;
	}

	// Token: 0x04000E35 RID: 3637
	public readonly GameObject sourcePrefab;

	// Token: 0x04000E36 RID: 3638
	public readonly GameObject parent;

	// Token: 0x04000E37 RID: 3639
	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	// Token: 0x04000E38 RID: 3640
	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();
}
