using System;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
	// Token: 0x060020D8 RID: 8408 RVA: 0x000BE104 File Offset: 0x000BC304
	protected override void OnPrefabInit()
	{
		base.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000BE117 File Offset: 0x000BC317
	protected virtual void OnCancel(object _)
	{
		this.DeleteObject();
	}

	// Token: 0x0400132B RID: 4907
	private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>(delegate(Cancellable component, object data)
	{
		component.OnCancel(data);
	});
}
