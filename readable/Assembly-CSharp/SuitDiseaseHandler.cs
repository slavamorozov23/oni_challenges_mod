using System;
using UnityEngine;

// Token: 0x02000BEC RID: 3052
[AddComponentMenu("KMonoBehaviour/scripts/SuitDiseaseHandler")]
public class SuitDiseaseHandler : KMonoBehaviour
{
	// Token: 0x06005B92 RID: 23442 RVA: 0x002124C9 File Offset: 0x002106C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitDiseaseHandler>(-1617557748, SuitDiseaseHandler.OnEquippedDelegate);
		base.Subscribe<SuitDiseaseHandler>(-170173755, SuitDiseaseHandler.OnUnequippedDelegate);
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x002124F4 File Offset: 0x002106F4
	private PrimaryElement GetPrimaryElement(object data)
	{
		GameObject targetGameObject = ((Equipment)data).GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject)
		{
			return targetGameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00212524 File Offset: 0x00210724
	private void OnEquipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(true);
			primaryElement.RedirectDisease(base.gameObject);
		}
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x00212558 File Offset: 0x00210758
	private void OnUnequipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(false);
			primaryElement.RedirectDisease(null);
		}
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x00212584 File Offset: 0x00210784
	private void OnModifyDiseaseCount(int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().ModifyDiseaseCount(delta, reason);
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x00212593 File Offset: 0x00210793
	private void OnAddDisease(byte disease_idx, int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().AddDisease(disease_idx, delta, reason);
	}

	// Token: 0x04003D01 RID: 15617
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04003D02 RID: 15618
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnUnequipped(data);
	});
}
