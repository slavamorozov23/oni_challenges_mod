using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C20 RID: 3104
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
	// Token: 0x06005D59 RID: 23897 RVA: 0x0021CAAC File Offset: 0x0021ACAC
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new WhiteBoard.Data
		{
			keyValueStore = new Dictionary<HashedString, object>()
		});
	}

	// Token: 0x06005D5A RID: 23898 RVA: 0x0021CAD8 File Offset: 0x0021ACD8
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Clear();
		data.keyValueStore = null;
		base.SetData(h, data);
	}

	// Token: 0x06005D5B RID: 23899 RVA: 0x0021CB08 File Offset: 0x0021AD08
	public bool HasValue(HandleVector<int>.Handle h, HashedString key)
	{
		return h.IsValid() && base.GetData(h).keyValueStore.ContainsKey(key);
	}

	// Token: 0x06005D5C RID: 23900 RVA: 0x0021CB27 File Offset: 0x0021AD27
	public object GetValue(HandleVector<int>.Handle h, HashedString key)
	{
		return base.GetData(h).keyValueStore[key];
	}

	// Token: 0x06005D5D RID: 23901 RVA: 0x0021CB3C File Offset: 0x0021AD3C
	public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore[key] = value;
		base.SetData(h, data);
	}

	// Token: 0x06005D5E RID: 23902 RVA: 0x0021CB70 File Offset: 0x0021AD70
	public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Remove(key);
		base.SetData(h, data);
	}

	// Token: 0x02001DB9 RID: 7609
	public struct Data
	{
		// Token: 0x04008C13 RID: 35859
		public Dictionary<HashedString, object> keyValueStore;
	}
}
