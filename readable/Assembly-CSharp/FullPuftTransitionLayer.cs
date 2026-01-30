using System;

// Token: 0x02000667 RID: 1639
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060027B2 RID: 10162 RVA: 0x000E3C76 File Offset: 0x000E1E76
	public FullPuftTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x000E3C80 File Offset: 0x000E1E80
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi != null && smi.stomach.IsReadyToPoop())
		{
			string s = HashCache.Get().Get(transition.anim.HashValue) + "_full";
			if (navigator.animController.HasAnimation(s))
			{
				transition.anim = s;
			}
		}
	}
}
