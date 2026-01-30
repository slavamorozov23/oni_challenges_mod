using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF1 RID: 2801
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005166 RID: 20838 RVA: 0x001D8278 File Offset: 0x001D6478
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.emitter = new RadiationGridEmitter(Grid.PosToCell(base.gameObject), this.intensity);
		this.emitter.projectionCount = this.projectionCount;
		this.emitter.direction = this.direction;
		this.emitter.angle = this.angle;
		if (base.GetComponent<Operational>() == null)
		{
			this.emitter.enabled = true;
		}
		else
		{
			base.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
		}
		RadiationGridManager.emitters.Add(this.emitter);
	}

	// Token: 0x06005167 RID: 20839 RVA: 0x001D831E File Offset: 0x001D651E
	protected override void OnCleanUp()
	{
		RadiationGridManager.emitters.Remove(this.emitter);
		base.OnCleanUp();
	}

	// Token: 0x06005168 RID: 20840 RVA: 0x001D8338 File Offset: 0x001D6538
	private void OnOperationalChanged(object _)
	{
		bool isActive = base.GetComponent<Operational>().IsActive;
		this.emitter.enabled = isActive;
	}

	// Token: 0x06005169 RID: 20841 RVA: 0x001D835D File Offset: 0x001D655D
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.intensity), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x0600516A RID: 20842 RVA: 0x001D8395 File Offset: 0x001D6595
	private void Update()
	{
		this.emitter.originCell = Grid.PosToCell(base.gameObject);
	}

	// Token: 0x04003707 RID: 14087
	public RadiationGridEmitter emitter;

	// Token: 0x04003708 RID: 14088
	public int intensity;

	// Token: 0x04003709 RID: 14089
	public int projectionCount;

	// Token: 0x0400370A RID: 14090
	public int direction;

	// Token: 0x0400370B RID: 14091
	public int angle = 360;
}
