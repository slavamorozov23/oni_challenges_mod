using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class CaloriesConsumedElementProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003C34 RID: 15412 RVA: 0x00150E43 File Offset: 0x0014F043
	protected override void OnSpawn()
	{
		base.OnSpawn();
		new CaloriesConsumedSecondaryExcretionMonitor.Instance(base.gameObject.GetComponent<StateMachineController>())
		{
			sm = 
			{
				producedElement = this.producedElement
			},
			sm = 
			{
				kgProducedPerKcalConsumed = this.kgProducedPerKcalConsumed
			}
		}.StartSM();
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x00150E84 File Offset: 0x0014F084
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04002522 RID: 9506
	public SimHashes producedElement;

	// Token: 0x04002523 RID: 9507
	public float kgProducedPerKcalConsumed = 1f;
}
