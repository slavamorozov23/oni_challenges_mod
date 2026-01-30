using System;
using Klei.AI;

// Token: 0x020005E8 RID: 1512
public class HeatImmunityProvider : EffectImmunityProviderStation<HeatImmunityProvider.Instance>
{
	// Token: 0x04001480 RID: 5248
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "RefreshingTouch";

	// Token: 0x020014C8 RID: 5320
	public new class Def : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.Def
	{
	}

	// Token: 0x020014C9 RID: 5321
	public new class Instance : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06009115 RID: 37141 RVA: 0x00370394 File Offset: 0x0036E594
		public Instance(IStateMachineTarget master, HeatImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06009116 RID: 37142 RVA: 0x0037039E File Offset: 0x0036E59E
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("RefreshingTouch", true);
		}
	}
}
