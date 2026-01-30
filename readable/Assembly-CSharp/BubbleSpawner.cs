using System;
using UnityEngine;

// Token: 0x02000712 RID: 1810
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
	// Token: 0x06002D25 RID: 11557 RVA: 0x00105F29 File Offset: 0x00104129
	protected override void OnSpawn()
	{
		this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
		base.OnSpawn();
		base.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x00105F68 File Offset: 0x00104168
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Mass >= this.emitMass)
		{
			gameObject.GetComponent<PrimaryElement>().Mass -= this.emitMass;
			BubbleManager.instance.SpawnBubble(base.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
		}
	}

	// Token: 0x04001ADA RID: 6874
	public SimHashes element;

	// Token: 0x04001ADB RID: 6875
	public float emitMass;

	// Token: 0x04001ADC RID: 6876
	public float emitVariance;

	// Token: 0x04001ADD RID: 6877
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04001ADE RID: 6878
	public Vector2 initialVelocity;

	// Token: 0x04001ADF RID: 6879
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001AE0 RID: 6880
	private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>(delegate(BubbleSpawner component, object data)
	{
		component.OnStorageChanged(data);
	});
}
