using System;

// Token: 0x020008D3 RID: 2259
public enum BuildLocationRule
{
	// Token: 0x040026AB RID: 9899
	Anywhere,
	// Token: 0x040026AC RID: 9900
	OnFloor,
	// Token: 0x040026AD RID: 9901
	OnFloorOverSpace,
	// Token: 0x040026AE RID: 9902
	OnCeiling,
	// Token: 0x040026AF RID: 9903
	OnWall,
	// Token: 0x040026B0 RID: 9904
	InCorner,
	// Token: 0x040026B1 RID: 9905
	Tile,
	// Token: 0x040026B2 RID: 9906
	NotInTiles,
	// Token: 0x040026B3 RID: 9907
	Conduit,
	// Token: 0x040026B4 RID: 9908
	LogicBridge,
	// Token: 0x040026B5 RID: 9909
	WireBridge,
	// Token: 0x040026B6 RID: 9910
	HighWattBridgeTile,
	// Token: 0x040026B7 RID: 9911
	BuildingAttachPoint,
	// Token: 0x040026B8 RID: 9912
	OnFloorOrBuildingAttachPoint,
	// Token: 0x040026B9 RID: 9913
	OnFoundationRotatable,
	// Token: 0x040026BA RID: 9914
	BelowRocketCeiling,
	// Token: 0x040026BB RID: 9915
	OnRocketEnvelope,
	// Token: 0x040026BC RID: 9916
	WallFloor,
	// Token: 0x040026BD RID: 9917
	NoLiquidConduitAtOrigin,
	// Token: 0x040026BE RID: 9918
	OnBackWall
}
