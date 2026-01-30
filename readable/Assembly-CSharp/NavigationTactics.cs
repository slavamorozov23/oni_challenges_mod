using System;

// Token: 0x0200060F RID: 1551
public static class NavigationTactics
{
	// Token: 0x0400152A RID: 5418
	public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);

	// Token: 0x0400152B RID: 5419
	public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);

	// Token: 0x0400152C RID: 5420
	public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);

	// Token: 0x0400152D RID: 5421
	public static NavTactic FetchDronePickup = new NavTactic(1, 0, 0, 0, 1, 0, 1, 1);
}
