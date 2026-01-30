using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000561 RID: 1377
internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06001EAB RID: 7851 RVA: 0x000A6A04 File Offset: 0x000A4C04
	public static UpdateObjectCountParameter.Settings GetSettings(HashedString path_hash, SoundDescription description)
	{
		UpdateObjectCountParameter.Settings settings = default(UpdateObjectCountParameter.Settings);
		if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
		{
			settings = default(UpdateObjectCountParameter.Settings);
			EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
			USER_PROPERTY user_PROPERTY;
			if (eventDescription.getUserProperty("minObj", out user_PROPERTY) == RESULT.OK)
			{
				settings.minObjects = (float)((short)user_PROPERTY.floatValue());
			}
			else
			{
				settings.minObjects = 1f;
			}
			USER_PROPERTY user_PROPERTY2;
			if (eventDescription.getUserProperty("maxObj", out user_PROPERTY2) == RESULT.OK)
			{
				settings.maxObjects = user_PROPERTY2.floatValue();
			}
			else
			{
				settings.maxObjects = 0f;
			}
			USER_PROPERTY user_PROPERTY3;
			if (eventDescription.getUserProperty("curveType", out user_PROPERTY3) == RESULT.OK && user_PROPERTY3.stringValue() == "exp")
			{
				settings.useExponentialCurve = true;
			}
			settings.parameterId = description.GetParameterId(UpdateObjectCountParameter.parameterHash);
			settings.path = path_hash;
			UpdateObjectCountParameter.settings[path_hash] = settings;
		}
		return settings;
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000A6AEC File Offset: 0x000A4CEC
	public static void ApplySettings(EventInstance ev, int count, UpdateObjectCountParameter.Settings settings)
	{
		float num = 0f;
		if (settings.maxObjects != settings.minObjects)
		{
			num = ((float)count - settings.minObjects) / (settings.maxObjects - settings.minObjects);
			num = Mathf.Clamp01(num);
		}
		if (settings.useExponentialCurve)
		{
			num *= num;
		}
		ev.setParameterByID(settings.parameterId, num, false);
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x000A6B48 File Offset: 0x000A4D48
	public UpdateObjectCountParameter() : base("objectCount")
	{
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000A6B68 File Offset: 0x000A4D68
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		UpdateObjectCountParameter.Entry item = new UpdateObjectCountParameter.Entry
		{
			ev = sound.ev,
			settings = settings
		};
		this.entries.Add(item);
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000A6BB4 File Offset: 0x000A4DB4
	public override void Update(float dt)
	{
		DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
		foreach (UpdateObjectCountParameter.Entry entry in this.entries)
		{
			int num = 0;
			pooledDictionary.TryGetValue(entry.settings.path, out num);
			num = (pooledDictionary[entry.settings.path] = num + 1);
		}
		foreach (UpdateObjectCountParameter.Entry entry2 in this.entries)
		{
			int count = pooledDictionary[entry2.settings.path];
			UpdateObjectCountParameter.ApplySettings(entry2.ev, count, entry2.settings);
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000A6CA0 File Offset: 0x000A4EA0
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000A6CF8 File Offset: 0x000A4EF8
	public static void Clear()
	{
		UpdateObjectCountParameter.settings.Clear();
	}

	// Token: 0x040011E7 RID: 4583
	private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

	// Token: 0x040011E8 RID: 4584
	private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();

	// Token: 0x040011E9 RID: 4585
	private static readonly HashedString parameterHash = "objectCount";

	// Token: 0x020013F4 RID: 5108
	private struct Entry
	{
		// Token: 0x04006CC7 RID: 27847
		public EventInstance ev;

		// Token: 0x04006CC8 RID: 27848
		public UpdateObjectCountParameter.Settings settings;
	}

	// Token: 0x020013F5 RID: 5109
	public struct Settings
	{
		// Token: 0x04006CC9 RID: 27849
		public HashedString path;

		// Token: 0x04006CCA RID: 27850
		public PARAMETER_ID parameterId;

		// Token: 0x04006CCB RID: 27851
		public float minObjects;

		// Token: 0x04006CCC RID: 27852
		public float maxObjects;

		// Token: 0x04006CCD RID: 27853
		public bool useExponentialCurve;
	}
}
