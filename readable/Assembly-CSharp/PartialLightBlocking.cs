using System;
using KSerialization;

// Token: 0x020007D8 RID: 2008
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
	// Token: 0x06003554 RID: 13652 RVA: 0x0012D4A9 File Offset: 0x0012B6A9
	protected override void OnSpawn()
	{
		this.SetLightBlocking();
		base.OnSpawn();
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x0012D4B7 File Offset: 0x0012B6B7
	protected override void OnCleanUp()
	{
		this.ClearLightBlocking();
		base.OnCleanUp();
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x0012D4C8 File Offset: 0x0012B6C8
	public void SetLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.SetCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x0012D4FC File Offset: 0x0012B6FC
	public void ClearLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ClearCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x04002043 RID: 8259
	private const byte PartialLightBlockingProperties = 48;
}
