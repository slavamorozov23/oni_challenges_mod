using System;
using UnityEngine;

// Token: 0x02000752 RID: 1874
[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
	// Token: 0x06002F65 RID: 12133 RVA: 0x00111AA6 File Offset: 0x0010FCA6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x00111ABF File Offset: 0x0010FCBF
	private void OnStorageChanged(object data)
	{
		if (this.storage.GetMassAvailable(this.emitTag) >= this.emitMass)
		{
			this.storage.DropSome(this.emitTag, this.emitMass, false, false, this.emitOffset, true, true);
		}
	}

	// Token: 0x04001C24 RID: 7204
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04001C25 RID: 7205
	[SerializeField]
	public float emitMass;

	// Token: 0x04001C26 RID: 7206
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04001C27 RID: 7207
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001C28 RID: 7208
	private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>(delegate(ElementDropper component, object data)
	{
		component.OnStorageChanged(data);
	});
}
