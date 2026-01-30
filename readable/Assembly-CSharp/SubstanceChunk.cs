using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000BEA RID: 3050
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SubstanceChunk")]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06005B86 RID: 23430 RVA: 0x0021226C File Offset: 0x0021046C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Color color = base.GetComponent<PrimaryElement>().Element.substance.colour;
		color.a = 1f;
		base.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint, color);
		base.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint2, color);
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x002122C8 File Offset: 0x002104C8
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP, true), 1f);
	}

	// Token: 0x06005B88 RID: 23432 RVA: 0x00212324 File Offset: 0x00210524
	private void OnRelease()
	{
		int gameCell = Grid.PosToCell(base.transform.GetPosition());
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			SimMessages.AddRemoveSubstance(gameCell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x04003CFC RID: 15612
	private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");

	// Token: 0x04003CFD RID: 15613
	private static readonly KAnimHashedString symbolToTint2 = new KAnimHashedString("substance_tinter_cap");
}
