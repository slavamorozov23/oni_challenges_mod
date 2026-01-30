using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000BD1 RID: 3025
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x0020DA5C File Offset: 0x0020BC5C
	// (set) Token: 0x06005AA9 RID: 23209 RVA: 0x0020DA64 File Offset: 0x0020BC64
	public Type RequiredType
	{
		get
		{
			return this.requiredType;
		}
		set
		{
			this.requiredType = value;
			this.typeNameString = this.requiredType.Name;
		}
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x0020DA7E File Offset: 0x0020BC7E
	public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string)
	{
		this.myAttachable = myAttachable;
		this.requiredType = required_type;
		this.typeNameString = type_name_string;
	}

	// Token: 0x06005AAB RID: 23211 RVA: 0x0020DA9C File Offset: 0x0020BC9C
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.myAttachable != null)
		{
			using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.myAttachable).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent(this.requiredType))
					{
						return ProcessCondition.Status.Ready;
					}
				}
			}
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x0020DB14 File Offset: 0x0020BD14
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return this.typeNameString;
	}

	// Token: 0x06005AAD RID: 23213 RVA: 0x0020DB20 File Offset: 0x0020BD20
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, this.typeNameString.ToLower());
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, this.typeNameString.ToLower());
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x0020DB5B File Offset: 0x0020BD5B
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C78 RID: 15480
	private string typeNameString;

	// Token: 0x04003C79 RID: 15481
	private Type requiredType;

	// Token: 0x04003C7A RID: 15482
	private AttachableBuilding myAttachable;
}
