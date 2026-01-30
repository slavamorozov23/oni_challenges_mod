using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB4 RID: 2740
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PrimaryElement")]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004F80 RID: 20352 RVA: 0x001CD98C File Offset: 0x001CBB8C
	public void SetUseSimDiseaseInfo(bool use)
	{
		this.useSimDiseaseInfo = use;
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06004F81 RID: 20353 RVA: 0x001CD995 File Offset: 0x001CBB95
	// (set) Token: 0x06004F82 RID: 20354 RVA: 0x001CD9A0 File Offset: 0x001CBBA0
	[Serialize]
	public float Units
	{
		get
		{
			return this._units;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				DebugUtil.DevLogError("Invalid units value for element, setting Units to 0");
				this._units = 0f;
			}
			else
			{
				this._units = value;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06004F83 RID: 20355 RVA: 0x001CD9EF File Offset: 0x001CBBEF
	// (set) Token: 0x06004F84 RID: 20356 RVA: 0x001CD9FD File Offset: 0x001CBBFD
	public float Temperature
	{
		get
		{
			return this.getTemperatureCallback(this);
		}
		set
		{
			this.SetTemperature(value);
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06004F85 RID: 20357 RVA: 0x001CDA06 File Offset: 0x001CBC06
	// (set) Token: 0x06004F86 RID: 20358 RVA: 0x001CDA0E File Offset: 0x001CBC0E
	public float InternalTemperature
	{
		get
		{
			return this._Temperature;
		}
		set
		{
			this._Temperature = value;
		}
	}

	// Token: 0x06004F87 RID: 20359 RVA: 0x001CDA18 File Offset: 0x001CBC18
	[OnSerializing]
	private void OnSerializing()
	{
		this._Temperature = this.Temperature;
		this.SanitizeMassAndTemperature();
		this.diseaseID.HashValue = 0;
		this.diseaseCount = 0;
		if (this.useSimDiseaseInfo)
		{
			int i = Grid.PosToCell(base.transform.GetPosition());
			if (Grid.DiseaseIdx[i] != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)Grid.DiseaseIdx[i]].id;
				this.diseaseCount = Grid.DiseaseCount[i];
				return;
			}
		}
		else if (this.diseaseHandle.IsValid())
		{
			DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
			if (header.diseaseIdx != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)header.diseaseIdx].id;
				this.diseaseCount = header.diseaseCount;
			}
		}
	}

	// Token: 0x06004F88 RID: 20360 RVA: 0x001CDB08 File Offset: 0x001CBD08
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.ElementID == (SimHashes)351109216)
		{
			this.ElementID = SimHashes.Creature;
		}
		this.SanitizeMassAndTemperature();
		float temperature = this._Temperature;
		if (float.IsNaN(temperature) || float.IsInfinity(temperature) || temperature < 0f || 10000f < temperature)
		{
			DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", base.name, this.Temperature), null);
			temperature = this.Element.defaultValues.temperature;
		}
		this._Temperature = temperature;
		this.Temperature = temperature;
		if (this.Element == null)
		{
			DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + "Primary element has no element.", null);
		}
		if (this.Mass < 0f)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.Mass == 0f && !this.KeepZeroMassObject)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized element with 0 mass. Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.onDataChanged != null)
		{
			this.onDataChanged(this);
		}
		byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
		if (index == 255 || this.diseaseCount <= 0)
		{
			if (this.diseaseHandle.IsValid())
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else
		{
			if (this.diseaseHandle.IsValid())
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
				header.diseaseIdx = index;
				header.diseaseCount = this.diseaseCount;
				GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
				return;
			}
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, index, this.diseaseCount);
		}
	}

	// Token: 0x06004F89 RID: 20361 RVA: 0x001CDCEC File Offset: 0x001CBEEC
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001CDCF4 File Offset: 0x001CBEF4
	private void SanitizeMassAndTemperature()
	{
		if (this._Temperature <= 0f)
		{
			DebugUtil.DevLogError(base.gameObject.name + " is attempting to serialize a temperature of <= 0K. Resetting to default. world=" + base.gameObject.DebugGetMyWorldName());
			this._Temperature = this.Element.defaultValues.temperature;
		}
		if (this.Mass > PrimaryElement.MAX_MASS)
		{
			DebugUtil.DevLogError(string.Format("{0} is attempting to serialize very large mass {1}. Resetting to default. world={2}", base.gameObject.name, this.Mass, base.gameObject.DebugGetMyWorldName()));
			this.Mass = this.Element.defaultValues.mass;
		}
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001CDD9C File Offset: 0x001CBF9C
	// (set) Token: 0x06004F8C RID: 20364 RVA: 0x001CDDAB File Offset: 0x001CBFAB
	public float Mass
	{
		get
		{
			return this.Units * this.MassPerUnit;
		}
		set
		{
			this.SetMass(value);
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x06004F8D RID: 20365 RVA: 0x001CDDC8 File Offset: 0x001CBFC8
	private void SetMass(float mass)
	{
		if ((mass > PrimaryElement.MAX_MASS || mass < 0f) && this.ElementID != SimHashes.Regolith)
		{
			DebugUtil.DevLogErrorFormat(base.gameObject, "{0} is getting an abnormal mass set {1}.", new object[]
			{
				base.gameObject.name,
				mass
			});
		}
		mass = Mathf.Clamp(mass, 0f, PrimaryElement.MAX_MASS);
		this.Units = mass / this.MassPerUnit;
		if (this.Units <= 0f && !this.KeepZeroMassObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001CDE60 File Offset: 0x001CC060
	private void SetTemperature(float temperature)
	{
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"Invalid temperature [" + temperature.ToString() + "]"
			});
			return;
		}
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0", null);
		}
		this.setTemperatureCallback(this, temperature);
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001CDEC9 File Offset: 0x001CC0C9
	public void SetMassTemperature(float mass, float temperature)
	{
		this.SetMass(mass);
		this.SetTemperature(temperature);
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06004F90 RID: 20368 RVA: 0x001CDED9 File Offset: 0x001CC0D9
	public Element Element
	{
		get
		{
			if (this._Element == null)
			{
				this._Element = ElementLoader.FindElementByHash(this.ElementID);
			}
			return this._Element;
		}
	}

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06004F91 RID: 20369 RVA: 0x001CDEFC File Offset: 0x001CC0FC
	public byte DiseaseIdx
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseIdx;
			}
			byte result = byte.MaxValue;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseIdx[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
			}
			return result;
		}
	}

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06004F92 RID: 20370 RVA: 0x001CDF74 File Offset: 0x001CC174
	public int DiseaseCount
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseCount;
			}
			int result = 0;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseCount[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
			}
			return result;
		}
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001CDFE7 File Offset: 0x001CC1E7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.InfraredVisualizers.Add(base.gameObject);
		base.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
		base.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
	}

	// Token: 0x06004F94 RID: 20372 RVA: 0x001CE024 File Offset: 0x001CC224
	protected override void OnSpawn()
	{
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in this.Element.attributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001CE088 File Offset: 0x001CC288
	public void ForcePermanentDiseaseContainer(bool force_on)
	{
		if (force_on)
		{
			if (!this.diseaseHandle.IsValid())
			{
				this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, byte.MaxValue, 0);
			}
		}
		else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == 255)
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		this.forcePermanentDiseaseContainer = force_on;
	}

	// Token: 0x06004F96 RID: 20374 RVA: 0x001CE0FF File Offset: 0x001CC2FF
	protected override void OnCleanUp()
	{
		GameComps.InfraredVisualizers.Remove(base.gameObject);
		if (this.diseaseHandle.IsValid())
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004F97 RID: 20375 RVA: 0x001CE13F File Offset: 0x001CC33F
	public void SetElement(SimHashes element_id, bool addTags = true)
	{
		this.ElementID = element_id;
		if (addTags)
		{
			this.UpdateTags();
		}
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x001CE154 File Offset: 0x001CC354
	public void UpdateTags()
	{
		if (this.ElementID == (SimHashes)0)
		{
			global::Debug.Log("UpdateTags() Primary element 0", base.gameObject);
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component != null)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag item in this.Element.oreTags)
			{
				list.Add(item);
			}
			if (component.HasAnyTags(PrimaryElement.metalTags))
			{
				list.Add(GameTags.StoredMetal);
			}
			foreach (Tag tag in list)
			{
				component.AddTag(tag, false);
			}
		}
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x001CE218 File Offset: 0x001CC418
	public void ModifyDiseaseCount(int delta, string reason)
	{
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), byte.MaxValue, delta);
			return;
		}
		if (delta != 0 && this.diseaseHandle.IsValid() && GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) <= 0 && !this.forcePermanentDiseaseContainer)
		{
			base.Trigger(-1689370368, BoxedBools.False);
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x001CE2B4 File Offset: 0x001CC4B4
	public void AddDisease(byte disease_idx, int delta, string reason)
	{
		if (delta == 0)
		{
			return;
		}
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), disease_idx, delta);
			return;
		}
		if (this.diseaseHandle.IsValid())
		{
			if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) <= 0)
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else if (delta > 0)
		{
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, disease_idx, delta);
			base.Trigger(-1689370368, BoxedBools.True);
			base.Trigger(-283306403, null);
		}
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001CE36D File Offset: 0x001CC56D
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		return primary_element._Temperature;
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001CE378 File Offset: 0x001CC578
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		global::Debug.Assert(!float.IsNaN(temperature));
		if (temperature <= 0f)
		{
			DebugUtil.LogErrorArgs(primary_element.gameObject, new object[]
			{
				primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."
			});
		}
		primary_element._Temperature = temperature;
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001CE3CC File Offset: 0x001CC5CC
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
		SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
		this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
		pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001CE440 File Offset: 0x001CC640
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001CE480 File Offset: 0x001CC680
	private void SetDiseaseVisualProvider(GameObject visualizer)
	{
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
			payload.visualDiseaseProvider = visualizer;
			GameComps.DiseaseContainers.SetPayload(handle, ref payload);
		}
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001CE4CC File Offset: 0x001CC6CC
	public void RedirectDisease(GameObject target)
	{
		this.SetDiseaseVisualProvider(target);
		this.diseaseRedirectTarget = (target ? target.GetComponent<PrimaryElement>() : null);
		global::Debug.Assert(this.diseaseRedirectTarget != this, "Disease redirect target set to myself");
	}

	// Token: 0x04003523 RID: 13603
	public static float MAX_MASS = 100000f;

	// Token: 0x04003524 RID: 13604
	public SimTemperatureTransfer sttOptimizationHook;

	// Token: 0x04003525 RID: 13605
	public PrimaryElement.GetTemperatureCallback getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);

	// Token: 0x04003526 RID: 13606
	public PrimaryElement.SetTemperatureCallback setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);

	// Token: 0x04003527 RID: 13607
	private PrimaryElement diseaseRedirectTarget;

	// Token: 0x04003528 RID: 13608
	private bool useSimDiseaseInfo;

	// Token: 0x04003529 RID: 13609
	public const float DefaultChunkMass = 400f;

	// Token: 0x0400352A RID: 13610
	private static readonly Tag[] metalTags = new Tag[]
	{
		GameTags.Metal,
		GameTags.RefinedMetal
	};

	// Token: 0x0400352B RID: 13611
	[Serialize]
	[HashedEnum]
	public SimHashes ElementID;

	// Token: 0x0400352C RID: 13612
	private float _units = 1f;

	// Token: 0x0400352D RID: 13613
	[Serialize]
	[SerializeField]
	private float _Temperature;

	// Token: 0x0400352E RID: 13614
	[Serialize]
	[NonSerialized]
	public bool KeepZeroMassObject;

	// Token: 0x0400352F RID: 13615
	[Serialize]
	private HashedString diseaseID;

	// Token: 0x04003530 RID: 13616
	[Serialize]
	private int diseaseCount;

	// Token: 0x04003531 RID: 13617
	private HandleVector<int>.Handle diseaseHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04003532 RID: 13618
	public float MassPerUnit = 1f;

	// Token: 0x04003533 RID: 13619
	[NonSerialized]
	private Element _Element;

	// Token: 0x04003534 RID: 13620
	[NonSerialized]
	public Action<PrimaryElement> onDataChanged;

	// Token: 0x04003535 RID: 13621
	[NonSerialized]
	private bool forcePermanentDiseaseContainer;

	// Token: 0x04003536 RID: 13622
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x04003537 RID: 13623
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x02001BF7 RID: 7159
	// (Invoke) Token: 0x0600AC28 RID: 44072
	public delegate float GetTemperatureCallback(PrimaryElement primary_element);

	// Token: 0x02001BF8 RID: 7160
	// (Invoke) Token: 0x0600AC2C RID: 44076
	public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
