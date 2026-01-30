using System;

// Token: 0x02000873 RID: 2163
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
	// Token: 0x06003B6D RID: 15213 RVA: 0x0014C22B File Offset: 0x0014A42B
	public void Run(SharedData sharedData, int threadIndex)
	{
		this.RunDivision(sharedData);
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0014C234 File Offset: 0x0014A434
	protected DivisibleTask(string name)
	{
		this.name = name;
	}

	// Token: 0x06003B6F RID: 15215
	protected abstract void RunDivision(SharedData sharedData);

	// Token: 0x040024C6 RID: 9414
	public string name;

	// Token: 0x040024C7 RID: 9415
	public int start;

	// Token: 0x040024C8 RID: 9416
	public int end;
}
