using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020005AA RID: 1450
public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
	// Token: 0x04001344 RID: 4932
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x0200142B RID: 5163
	public new class Def : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def, IGameObjectEffectDescriptor
	{
		// Token: 0x06008EDB RID: 36571 RVA: 0x00369E30 File Offset: 0x00368030
		public override string[] DefaultAnims()
		{
			return new string[]
			{
				"warmup_pre",
				"warmup_loop",
				"warmup_pst"
			};
		}

		// Token: 0x06008EDC RID: 36572 RVA: 0x00369E50 File Offset: 0x00368050
		public override string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x06008EDD RID: 36573 RVA: 0x00369E58 File Offset: 0x00368058
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false)
			};
		}
	}

	// Token: 0x0200142C RID: 5164
	public new class Instance : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06008EDF RID: 36575 RVA: 0x00369EC5 File Offset: 0x003680C5
		public Instance(IStateMachineTarget master, ColdImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06008EE0 RID: 36576 RVA: 0x00369ECF File Offset: 0x003680CF
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("WarmTouch", true);
		}
	}
}
