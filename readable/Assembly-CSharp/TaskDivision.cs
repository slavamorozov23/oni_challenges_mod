using System;

// Token: 0x02000874 RID: 2164
internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
	// Token: 0x06003B70 RID: 15216 RVA: 0x0014C244 File Offset: 0x0014A444
	public TaskDivision(int taskCount)
	{
		this.tasks = new Task[taskCount];
		for (int num = 0; num != this.tasks.Length; num++)
		{
			this.tasks[num] = Activator.CreateInstance<Task>();
		}
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x0014C287 File Offset: 0x0014A487
	public TaskDivision() : this(CPUBudget.coreCount)
	{
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x0014C294 File Offset: 0x0014A494
	public void Initialize(int count)
	{
		int num = count / this.tasks.Length;
		for (int num2 = 0; num2 != this.tasks.Length; num2++)
		{
			this.tasks[num2].start = num2 * num;
			this.tasks[num2].end = this.tasks[num2].start + num;
		}
		DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
		this.tasks[this.tasks.Length - 1].end = count;
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0014C358 File Offset: 0x0014A558
	public void Run(SharedData sharedData, int threadIndex)
	{
		Task[] array = this.tasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(sharedData, threadIndex);
		}
	}

	// Token: 0x040024C9 RID: 9417
	public Task[] tasks;
}
