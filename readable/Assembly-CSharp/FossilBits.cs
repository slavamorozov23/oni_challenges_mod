using System;
using KSerialization;
using STRINGS;

// Token: 0x02000219 RID: 537
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	// Token: 0x06000AD6 RID: 2774 RVA: 0x00041AB2 File Offset: 0x0003FCB2
	protected override bool IsMarkedForExcavation()
	{
		return this.MarkedForDig;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00041ABA File Offset: 0x0003FCBA
	public void SetEntombStatusItemVisibility(bool visible)
	{
		this.entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00041AC8 File Offset: 0x0003FCC8
	public void CreateWorkableChore()
	{
		if (this.chore == null && this.operational.IsOperational)
		{
			this.chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00041B16 File Offset: 0x0003FD16
	public void CancelWorkChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("FossilBits.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00041B38 File Offset: 0x0003FD38
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		base.SetWorkTime(30f);
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00041B8C File Offset: 0x0003FD8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		base.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00041BAC File Offset: 0x0003FDAC
	private void OnOperationalChanged(object state)
	{
		if (((Boxed<bool>)state).value)
		{
			if (this.MarkedForDig)
			{
				this.CreateWorkableChore();
				return;
			}
		}
		else if (this.MarkedForDig)
		{
			this.CancelWorkChore();
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00041BD8 File Offset: 0x0003FDD8
	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00041C8E File Offset: 0x0003FE8E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00041CA8 File Offset: 0x0003FEA8
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00041CAB File Offset: 0x0003FEAB
	public string SidescreenButtonText
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00041CCA File Offset: 0x0003FECA
	public string SidescreenButtonTooltip
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00041CE9 File Offset: 0x0003FEE9
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00041CF0 File Offset: 0x0003FEF0
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00041CF3 File Offset: 0x0003FEF3
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00041CF8 File Offset: 0x0003FEF8
	public void OnSidescreenButtonPressed()
	{
		this.MarkedForDig = !this.MarkedForDig;
		base.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		if (this.MarkedForDig)
		{
			this.CreateWorkableChore();
		}
		else
		{
			this.CancelWorkChore();
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00041D49 File Offset: 0x0003FF49
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040007AA RID: 1962
	[Serialize]
	public bool MarkedForDig;

	// Token: 0x040007AB RID: 1963
	private Chore chore;

	// Token: 0x040007AC RID: 1964
	[MyCmpGet]
	private EntombVulnerable entombComponent;

	// Token: 0x040007AD RID: 1965
	[MyCmpGet]
	private Operational operational;
}
