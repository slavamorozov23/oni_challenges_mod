using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
[AddComponentMenu("KMonoBehaviour/scripts/SicknessTrigger")]
public class SicknessTrigger : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060054F3 RID: 21747 RVA: 0x001EF9CC File Offset: 0x001EDBCC
	public void AddTrigger(GameHashes src_event, string[] sickness_ids, SicknessTrigger.SourceCallback source_callback)
	{
		this.triggers.Add(new SicknessTrigger.TriggerInfo
		{
			srcEvent = src_event,
			sickness_ids = sickness_ids,
			sourceCallback = source_callback
		});
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x001EFA08 File Offset: 0x001EDC08
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			SicknessTrigger.TriggerInfo trigger = this.triggers[i];
			base.Subscribe((int)trigger.srcEvent, delegate(object data)
			{
				this.OnSicknessTrigger((GameObject)data, trigger);
			});
		}
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x001EFA68 File Offset: 0x001EDC68
	private void OnSicknessTrigger(GameObject target, SicknessTrigger.TriggerInfo trigger)
	{
		int num = UnityEngine.Random.Range(0, trigger.sickness_ids.Length);
		string text = trigger.sickness_ids[num];
		Sickness sickness = null;
		Database.Sicknesses sicknesses = Db.Get().Sicknesses;
		for (int i = 0; i < sicknesses.Count; i++)
		{
			if (sicknesses[i].Id == text)
			{
				sickness = sicknesses[i];
				break;
			}
		}
		if (sickness != null)
		{
			string infection_source_info = trigger.sourceCallback(base.gameObject, target);
			SicknessExposureInfo exposure_info = new SicknessExposureInfo(sickness.Id, infection_source_info);
			target.GetComponent<MinionModifiers>().sicknesses.Infect(exposure_info);
			return;
		}
		DebugUtil.DevLogErrorFormat(base.gameObject, "Couldn't find sickness with id [{0}]", new object[]
		{
			text
		});
	}

	// Token: 0x060054F6 RID: 21750 RVA: 0x001EFB24 File Offset: 0x001EDD24
	public List<Descriptor> EffectDescriptors(GameObject go)
	{
		Dictionary<GameHashes, HashSet<string>> dictionary = new Dictionary<GameHashes, HashSet<string>>();
		foreach (SicknessTrigger.TriggerInfo triggerInfo in this.triggers)
		{
			HashSet<string> hashSet = null;
			if (!dictionary.TryGetValue(triggerInfo.srcEvent, out hashSet))
			{
				hashSet = new HashSet<string>();
				dictionary[triggerInfo.srcEvent] = hashSet;
			}
			foreach (string item in triggerInfo.sickness_ids)
			{
				hashSet.Add(item);
			}
		}
		List<Descriptor> list = new List<Descriptor>();
		List<string> list2 = new List<string>();
		string properName = base.GetComponent<KSelectable>().GetProperName();
		foreach (KeyValuePair<GameHashes, HashSet<string>> keyValuePair in dictionary)
		{
			HashSet<string> value = keyValuePair.Value;
			list2.Clear();
			foreach (string id in value)
			{
				Sickness sickness = Db.Get().Sicknesses.TryGet(id);
				list2.Add(sickness.Name);
			}
			string newValue = string.Join(", ", list2.ToArray());
			string text = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			string text2 = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS.TOOLTIPS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			text = text.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			text2 = text2.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			list.Add(new Descriptor(text, text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x001EFD74 File Offset: 0x001EDF74
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.EffectDescriptors(go);
	}

	// Token: 0x0400395F RID: 14687
	public List<SicknessTrigger.TriggerInfo> triggers = new List<SicknessTrigger.TriggerInfo>();

	// Token: 0x02001CA7 RID: 7335
	// (Invoke) Token: 0x0600AE40 RID: 44608
	public delegate string SourceCallback(GameObject source, GameObject target);

	// Token: 0x02001CA8 RID: 7336
	[Serializable]
	public struct TriggerInfo
	{
		// Token: 0x040088B9 RID: 35001
		[HashedEnum]
		public GameHashes srcEvent;

		// Token: 0x040088BA RID: 35002
		public string[] sickness_ids;

		// Token: 0x040088BB RID: 35003
		public SicknessTrigger.SourceCallback sourceCallback;
	}
}
