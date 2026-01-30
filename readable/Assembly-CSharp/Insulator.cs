using System;
using UnityEngine;

// Token: 0x0200099C RID: 2460
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
	// Token: 0x060046B8 RID: 18104 RVA: 0x001993F2 File Offset: 0x001975F2
	protected override void OnSpawn()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00199424 File Offset: 0x00197624
	protected override void OnCleanUp()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), 1f);
	}

	// Token: 0x04002F98 RID: 12184
	[MyCmpReq]
	private Building building;

	// Token: 0x04002F99 RID: 12185
	[SerializeField]
	public CellOffset offset = CellOffset.none;
}
