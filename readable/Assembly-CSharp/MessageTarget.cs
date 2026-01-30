using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D97 RID: 3479
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
	// Token: 0x06006C57 RID: 27735 RVA: 0x00291198 File Offset: 0x0028F398
	public MessageTarget(KPrefabID prefab_id)
	{
		this.prefabId.Set(prefab_id);
		this.position = prefab_id.transform.GetPosition();
		this.name = "Unknown";
		KSelectable component = prefab_id.GetComponent<KSelectable>();
		if (component != null)
		{
			this.name = component.GetName();
		}
		prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
	}

	// Token: 0x06006C58 RID: 27736 RVA: 0x00291212 File Offset: 0x0028F412
	public Vector3 GetPosition()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetPosition();
		}
		return this.position;
	}

	// Token: 0x06006C59 RID: 27737 RVA: 0x00291243 File Offset: 0x0028F443
	public KSelectable GetSelectable()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetComponent<KSelectable>();
		}
		return null;
	}

	// Token: 0x06006C5A RID: 27738 RVA: 0x0029126F File Offset: 0x0028F46F
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06006C5B RID: 27739 RVA: 0x00291278 File Offset: 0x0028F478
	private void OnAbsorbedBy(object data)
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		this.prefabId.Set(component);
	}

	// Token: 0x06006C5C RID: 27740 RVA: 0x002912EC File Offset: 0x0028F4EC
	public void OnCleanUp()
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
			this.prefabId.Set(null);
		}
	}

	// Token: 0x04004A34 RID: 18996
	[Serialize]
	private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

	// Token: 0x04004A35 RID: 18997
	[Serialize]
	private Vector3 position;

	// Token: 0x04004A36 RID: 18998
	[Serialize]
	private string name;
}
