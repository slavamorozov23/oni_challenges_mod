using System;

// Token: 0x0200066B RID: 1643
public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x060027C1 RID: 10177 RVA: 0x000E424D File Offset: 0x000E244D
	public ReactableTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x000E4256 File Offset: 0x000E2456
	protected override bool IsOverrideComplete()
	{
		return !this.reactionMonitor.IsReacting() && base.IsOverrideComplete();
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x000E4270 File Offset: 0x000E2470
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.reactionMonitor == null)
		{
			this.reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		this.reactionMonitor.PollForReactables(transition);
		if (this.reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.end;
		}
	}

	// Token: 0x0400175A RID: 5978
	private ReactionMonitor.Instance reactionMonitor;
}
