using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public abstract class LoopingSoundParameterUpdater
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060023A2 RID: 9122 RVA: 0x000CDE06 File Offset: 0x000CC006
	// (set) Token: 0x060023A3 RID: 9123 RVA: 0x000CDE0E File Offset: 0x000CC00E
	public HashedString parameter { get; private set; }

	// Token: 0x060023A4 RID: 9124 RVA: 0x000CDE17 File Offset: 0x000CC017
	public LoopingSoundParameterUpdater(HashedString parameter)
	{
		this.parameter = parameter;
	}

	// Token: 0x060023A5 RID: 9125
	public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x060023A6 RID: 9126
	public abstract void Update(float dt);

	// Token: 0x060023A7 RID: 9127
	public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x020014D4 RID: 5332
	public struct Sound
	{
		// Token: 0x04006FB6 RID: 28598
		public EventInstance ev;

		// Token: 0x04006FB7 RID: 28599
		public HashedString path;

		// Token: 0x04006FB8 RID: 28600
		public Transform transform;

		// Token: 0x04006FB9 RID: 28601
		public SoundDescription description;

		// Token: 0x04006FBA RID: 28602
		public bool objectIsSelectedAndVisible;
	}
}
