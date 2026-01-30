using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE0 RID: 2784
public class FossilDigsiteLampLight : Light2D
{
	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x060050EC RID: 20716 RVA: 0x001D5584 File Offset: 0x001D3784
	// (set) Token: 0x060050EB RID: 20715 RVA: 0x001D557B File Offset: 0x001D377B
	public bool independent { get; private set; }

	// Token: 0x060050ED RID: 20717 RVA: 0x001D558C File Offset: 0x001D378C
	protected override void OnPrefabInit()
	{
		base.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	// Token: 0x060050EE RID: 20718 RVA: 0x001D55AC File Offset: 0x001D37AC
	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		this.independent = isIndependent;
		Operational component = base.GetComponent<Operational>();
		if (component != null && this.independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	// Token: 0x060050EF RID: 20719 RVA: 0x001D55F7 File Offset: 0x001D37F7
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}

	// Token: 0x040035FC RID: 13820
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = ((Boxed<bool>)data).value;
		}
	});
}
