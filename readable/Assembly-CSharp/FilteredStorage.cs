using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class FilteredStorage
{
	// Token: 0x06002F8C RID: 12172 RVA: 0x00112A5F File Offset: 0x00110C5F
	public void SetHasMeter(bool has_meter)
	{
		this.hasMeter = has_meter;
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x00112A68 File Offset: 0x00110C68
	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		this.forbiddenTags = forbidden_tags;
		this.capacityControl = capacity_control;
		this.useLogicMeter = use_logic_meter;
		this.choreType = fetch_chore_type;
		root.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		root.Subscribe(-543130682, new Action<object>(this.OnUserSettingsChanged));
		this.filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = this.filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		this.storage = root.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(-1852328367, new Action<object>(this.OnFunctionalChanged));
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x00112B5B File Offset: 0x00110D5B
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x00112B70 File Offset: 0x00110D70
	private void CreateMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
	}

	// Token: 0x06002F90 RID: 12176 RVA: 0x00112BBF File Offset: 0x00110DBF
	private void CreateLogicMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.logicMeter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x00112BF2 File Offset: 0x00110DF2
	public void SetMeter(MeterController meter)
	{
		this.hasMeter = true;
		this.meter = meter;
		this.UpdateMeter();
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x00112C08 File Offset: 0x00110E08
	public void CleanUp()
	{
		if (this.filterable != null)
		{
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Parent destroyed");
		}
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x00112C64 File Offset: 0x00110E64
	public void FilterChanged()
	{
		if (this.hasMeter)
		{
			if (this.meter == null)
			{
				this.CreateMeter();
			}
			if (this.logicMeter == null && this.useLogicMeter)
			{
				this.CreateLogicMeter();
			}
		}
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x00112CB4 File Offset: 0x00110EB4
	private void OnUserSettingsChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x00112CCD File Offset: 0x00110ECD
	private void OnStorageChanged(object data)
	{
		if (this.fetchList == null)
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}
		this.UpdateMeter();
	}

	// Token: 0x06002F96 RID: 12182 RVA: 0x00112CEE File Offset: 0x00110EEE
	private void OnFunctionalChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x00112D04 File Offset: 0x00110F04
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06002F98 RID: 12184 RVA: 0x00112D3C File Offset: 0x00110F3C
	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
		return num >= 1f;
	}

	// Token: 0x06002F99 RID: 12185 RVA: 0x00112D7D File Offset: 0x00110F7D
	private void OnFetchComplete()
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002F9A RID: 12186 RVA: 0x00112D90 File Offset: 0x00110F90
	private float GetMaxCapacity()
	{
		float num = this.storage.capacityKg;
		if (this.capacityControl != null)
		{
			num = Mathf.Min(num, this.capacityControl.UserMaxCapacity);
		}
		return num;
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x00112DC4 File Offset: 0x00110FC4
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.GetMaxCapacity() - this.storage.storageFullMargin;
	}

	// Token: 0x06002F9C RID: 12188 RVA: 0x00112DD8 File Offset: 0x00110FD8
	private float GetAmountStored()
	{
		float result = this.storage.MassStored();
		if (this.capacityControl != null)
		{
			result = this.capacityControl.AmountStored;
		}
		return result;
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x00112E08 File Offset: 0x00111008
	private bool IsFunctional()
	{
		Operational component = this.storage.GetComponent<Operational>();
		return component == null || component.IsFunctional;
	}

	// Token: 0x06002F9E RID: 12190 RVA: 0x00112E34 File Offset: 0x00111034
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("");
			this.fetchList = null;
		}
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float amountStored = this.GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && this.IsFunctional())
		{
			num = Mathf.Max(0f, this.GetMaxCapacity() - amountStored);
			this.fetchList = new FetchList2(this.storage, this.choreType);
			this.fetchList.ShowStatusItem = false;
			this.fetchList.Add(tags, this.requiredTag, this.forbiddenTags, num, Operational.State.Functional);
			this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
		}
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x00112F08 File Offset: 0x00111108
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x00112F2C File Offset: 0x0011112C
	public void SetRequiredTag(Tag tag)
	{
		if (this.requiredTag != tag)
		{
			this.requiredTag = tag;
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x00112F54 File Offset: 0x00111154
	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags == null)
		{
			this.forbiddenTags = new Tag[0];
		}
		if (!this.forbiddenTags.Contains(forbidden_tag))
		{
			this.forbiddenTags = this.forbiddenTags.Append(forbidden_tag);
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x00112FA8 File Offset: 0x001111A8
	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(this.forbiddenTags);
			list.Remove(forbidden_tag);
			this.forbiddenTags = list.ToArray();
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x04001C46 RID: 7238
	public static readonly HashedString FULL_PORT_ID = "FULL";

	// Token: 0x04001C47 RID: 7239
	private KMonoBehaviour root;

	// Token: 0x04001C48 RID: 7240
	private FetchList2 fetchList;

	// Token: 0x04001C49 RID: 7241
	private IUserControlledCapacity capacityControl;

	// Token: 0x04001C4A RID: 7242
	private TreeFilterable filterable;

	// Token: 0x04001C4B RID: 7243
	private Storage storage;

	// Token: 0x04001C4C RID: 7244
	private MeterController meter;

	// Token: 0x04001C4D RID: 7245
	private MeterController logicMeter;

	// Token: 0x04001C4E RID: 7246
	private Tag requiredTag = Tag.Invalid;

	// Token: 0x04001C4F RID: 7247
	private Tag[] forbiddenTags;

	// Token: 0x04001C50 RID: 7248
	private bool hasMeter = true;

	// Token: 0x04001C51 RID: 7249
	private bool useLogicMeter;

	// Token: 0x04001C52 RID: 7250
	private ChoreType choreType;
}
