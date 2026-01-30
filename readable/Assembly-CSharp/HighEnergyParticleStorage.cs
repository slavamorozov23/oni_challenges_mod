using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200098D RID: 2445
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06004647 RID: 17991 RVA: 0x00195D01 File Offset: 0x00193F01
	public float Particles
	{
		get
		{
			return this.particles;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06004648 RID: 17992 RVA: 0x00195D09 File Offset: 0x00193F09
	// (set) Token: 0x06004649 RID: 17993 RVA: 0x00195D11 File Offset: 0x00193F11
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x0600464A RID: 17994 RVA: 0x00195D1C File Offset: 0x00193F1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Combine(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
			component.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(component.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		this.SetupStorageStatusItems();
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x00195D8B File Offset: 0x00193F8B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateLogicPorts();
	}

	// Token: 0x0600464C RID: 17996 RVA: 0x00195D9C File Offset: 0x00193F9C
	private void UpdateLogicPorts()
	{
		if (this._logicPorts != null)
		{
			bool flag = this.IsFull();
			this._logicPorts.SendSignal(this.PORT_ID, flag ? 1 : 0);
		}
	}

	// Token: 0x0600464D RID: 17997 RVA: 0x00195DDB File Offset: 0x00193FDB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Remove(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
		}
	}

	// Token: 0x0600464E RID: 17998 RVA: 0x00195E18 File Offset: 0x00194018
	private void OnParticleCapture(HighEnergyParticle particle)
	{
		float num = Mathf.Min(particle.payload, this.capacity - this.particles);
		this.Store(num);
		particle.payload -= num;
		if (particle.payload > 0f)
		{
			base.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
		}
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x00195E72 File Offset: 0x00194072
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.particles < this.capacity && this.receiverOpen;
	}

	// Token: 0x06004650 RID: 18000 RVA: 0x00195E8C File Offset: 0x0019408C
	private void DeltaParticles(float delta)
	{
		this.particles += delta;
		if (this.particles <= 0f)
		{
			base.Trigger(155636535, base.transform.gameObject);
		}
		base.Trigger(-1837862626, base.transform.gameObject);
		this.UpdateLogicPorts();
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x00195EE8 File Offset: 0x001940E8
	public float Store(float amount)
	{
		float num = Mathf.Min(amount, this.RemainingCapacity());
		this.DeltaParticles(num);
		return num;
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x00195F0A File Offset: 0x0019410A
	public float ConsumeAndGet(float amount)
	{
		amount = Mathf.Min(this.Particles, amount);
		this.DeltaParticles(-amount);
		return amount;
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x00195F23 File Offset: 0x00194123
	[ContextMenu("Trigger Stored Event")]
	public void DEBUG_TriggerStorageEvent()
	{
		base.Trigger(-1837862626, base.transform.gameObject);
	}

	// Token: 0x06004654 RID: 18004 RVA: 0x00195F3B File Offset: 0x0019413B
	[ContextMenu("Trigger Zero Event")]
	public void DEBUG_TriggerZeroEvent()
	{
		this.ConsumeAndGet(this.particles + 1f);
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x00195F50 File Offset: 0x00194150
	public float ConsumeAll()
	{
		return this.ConsumeAndGet(this.particles);
	}

	// Token: 0x06004656 RID: 18006 RVA: 0x00195F5E File Offset: 0x0019415E
	public bool HasRadiation()
	{
		return this.Particles > 0f;
	}

	// Token: 0x06004657 RID: 18007 RVA: 0x00195F6D File Offset: 0x0019416D
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		return null;
	}

	// Token: 0x06004658 RID: 18008 RVA: 0x00195F70 File Offset: 0x00194170
	public List<GameObject> GetItems()
	{
		return new List<GameObject>
		{
			base.gameObject
		};
	}

	// Token: 0x06004659 RID: 18009 RVA: 0x00195F83 File Offset: 0x00194183
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x0600465A RID: 18010 RVA: 0x00195F95 File Offset: 0x00194195
	public bool IsEmpty()
	{
		return this.Particles == 0f;
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x00195FA4 File Offset: 0x001941A4
	public float Capacity()
	{
		return this.capacity;
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x00195FAC File Offset: 0x001941AC
	public float RemainingCapacity()
	{
		return Mathf.Max(this.capacity - this.Particles, 0f);
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x00195FC5 File Offset: 0x001941C5
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x00195FCD File Offset: 0x001941CD
	public float GetAmountAvailable(Tag tag)
	{
		if (tag != GameTags.HighEnergyParticle)
		{
			return 0f;
		}
		return this.Particles;
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x00195FE8 File Offset: 0x001941E8
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount", null);
		this.ConsumeAndGet(amount);
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x00196008 File Offset: 0x00194208
	private void SetupStorageStatusItems()
	{
		if (HighEnergyParticleStorage.capacityStatusItem == null)
		{
			HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = (HighEnergyParticleStorage)data;
				string newValue = Util.FormatWholeNumber(highEnergyParticleStorage.particles);
				string newValue2 = Util.FormatWholeNumber(highEnergyParticleStorage.capacity);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				str = str.Replace("{Units}", UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, this);
		}
	}

	// Token: 0x04002F51 RID: 12113
	[Serialize]
	[SerializeField]
	private float particles;

	// Token: 0x04002F52 RID: 12114
	[Serialize]
	public float capacity = float.MaxValue;

	// Token: 0x04002F53 RID: 12115
	public bool showInUI = true;

	// Token: 0x04002F54 RID: 12116
	public bool showCapacityStatusItem;

	// Token: 0x04002F55 RID: 12117
	public bool showCapacityAsMainStatus;

	// Token: 0x04002F57 RID: 12119
	public bool autoStore;

	// Token: 0x04002F58 RID: 12120
	[Serialize]
	public bool receiverOpen = true;

	// Token: 0x04002F59 RID: 12121
	[MyCmpGet]
	private LogicPorts _logicPorts;

	// Token: 0x04002F5A RID: 12122
	public string PORT_ID = "";

	// Token: 0x04002F5B RID: 12123
	private static StatusItem capacityStatusItem;
}
