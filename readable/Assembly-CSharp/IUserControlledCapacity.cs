using System;

// Token: 0x02000E21 RID: 3617
public interface IUserControlledCapacity
{
	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x060072B4 RID: 29364
	// (set) Token: 0x060072B5 RID: 29365
	float UserMaxCapacity { get; set; }

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x060072B6 RID: 29366
	float AmountStored { get; }

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x060072B7 RID: 29367
	float MinCapacity { get; }

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x060072B8 RID: 29368
	float MaxCapacity { get; }

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x060072B9 RID: 29369
	bool WholeValues { get; }

	// Token: 0x060072BA RID: 29370 RVA: 0x002BCE3A File Offset: 0x002BB03A
	bool ControlEnabled()
	{
		return true;
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x060072BB RID: 29371
	LocString CapacityUnits { get; }
}
