using System;
using UnityEngine;

// Token: 0x02000D25 RID: 3365
[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
	// Token: 0x06006812 RID: 26642 RVA: 0x00274AC8 File Offset: 0x00272CC8
	public bool HasReference(string name)
	{
		ElementReference[] array = this.references;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006813 RID: 26643 RVA: 0x00274B04 File Offset: 0x00272D04
	public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				if (elementReference.behaviour is SpecifiedType)
				{
					return (SpecifiedType)((object)elementReference.behaviour);
				}
				global::Debug.LogError(string.Format("Behavior is not specified type", Array.Empty<object>()));
			}
		}
		global::Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
		return default(SpecifiedType);
	}

	// Token: 0x06006814 RID: 26644 RVA: 0x00274B84 File Offset: 0x00272D84
	public Component GetReference(string name)
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				return elementReference.behaviour;
			}
		}
		global::Debug.LogWarning("Couldn't find reference to object named " + name + " Make sure the name matches the field in the inspector.");
		return null;
	}

	// Token: 0x0400477F RID: 18303
	public ElementReference[] references;
}
