using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200098C RID: 2444
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600463C RID: 17980 RVA: 0x00195BAF File Offset: 0x00193DAF
	public int GetHighEnergyParticleInputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleInputCell();
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x00195BBC File Offset: 0x00193DBC
	public int GetHighEnergyParticleOutputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleOutputCell();
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x00195BC9 File Offset: 0x00193DC9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x00195BD1 File Offset: 0x00193DD1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticlePorts.Add(this);
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x00195BE4 File Offset: 0x00193DE4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HighEnergyParticlePorts.Remove(this);
	}

	// Token: 0x06004641 RID: 17985 RVA: 0x00195BF8 File Offset: 0x00193DF8
	public bool InputActive()
	{
		Operational component = base.GetComponent<Operational>();
		return this.particleInputEnabled && component != null && component.IsFunctional && (!this.requireOperational || component.IsOperational);
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x00195C37 File Offset: 0x00193E37
	public bool AllowCapture(HighEnergyParticle particle)
	{
		return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x00195C4F File Offset: 0x00193E4F
	public void Capture(HighEnergyParticle particle)
	{
		this.currentParticle = particle;
		if (this.onParticleCapture != null)
		{
			this.onParticleCapture(particle);
		}
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x00195C6C File Offset: 0x00193E6C
	public void Uncapture(HighEnergyParticle particle)
	{
		if (this.onParticleUncapture != null)
		{
			this.onParticleUncapture(particle);
		}
		this.currentParticle = null;
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x00195C8C File Offset: 0x00193E8C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.particleInputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement, false));
		}
		if (this.particleOutputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04002F47 RID: 12103
	[MyCmpGet]
	private Building m_building;

	// Token: 0x04002F48 RID: 12104
	public HighEnergyParticlePort.OnParticleCapture onParticleCapture;

	// Token: 0x04002F49 RID: 12105
	public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;

	// Token: 0x04002F4A RID: 12106
	public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;

	// Token: 0x04002F4B RID: 12107
	public HighEnergyParticle currentParticle;

	// Token: 0x04002F4C RID: 12108
	public bool requireOperational = true;

	// Token: 0x04002F4D RID: 12109
	public bool particleInputEnabled;

	// Token: 0x04002F4E RID: 12110
	public bool particleOutputEnabled;

	// Token: 0x04002F4F RID: 12111
	public CellOffset particleInputOffset;

	// Token: 0x04002F50 RID: 12112
	public CellOffset particleOutputOffset;

	// Token: 0x020019F6 RID: 6646
	// (Invoke) Token: 0x0600A37B RID: 41851
	public delegate void OnParticleCapture(HighEnergyParticle particle);

	// Token: 0x020019F7 RID: 6647
	// (Invoke) Token: 0x0600A37F RID: 41855
	public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
