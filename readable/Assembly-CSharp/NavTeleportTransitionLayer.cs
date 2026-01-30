using System;

// Token: 0x0200066C RID: 1644
public class NavTeleportTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060027C4 RID: 10180 RVA: 0x000E42D4 File Offset: 0x000E24D4
	public NavTeleportTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x000E42E0 File Offset: 0x000E24E0
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		if (transition.start == NavType.Teleport)
		{
			int num = Grid.PosToCell(navigator);
			int num2;
			int num3;
			Grid.CellToXY(num, out num2, out num3);
			int num4 = num2;
			int num5 = num3;
			int cell;
			if (navigator.NavGrid.teleportTransitions.TryGetValue(num, out cell))
			{
				Grid.CellToXY(cell, out num4, out num5);
			}
			transition.x = num4 - num2;
			transition.y = num5 - num3;
		}
	}
}
