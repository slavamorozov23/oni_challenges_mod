using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AFE RID: 2814
[AddComponentMenu("KMonoBehaviour/scripts/ResearchPointObject")]
public class ResearchPointObject : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060051ED RID: 20973 RVA: 0x001DBE70 File Offset: 0x001DA070
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Research.Instance.AddResearchPoints(this.TypeID, 1f);
		ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, base.transform, 1.5f, false);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060051EE RID: 20974 RVA: 0x001DBEDC File Offset: 0x001DA0DC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
		list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, researchType.name), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, researchType.description), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x04003770 RID: 14192
	public string TypeID = "";
}
