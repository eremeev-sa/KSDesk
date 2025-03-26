namespace KanbanApp.Core.Model
{
	[Flags]
	public enum Permissions
	{
		None = 0,
		Read = 1 << 0,    // 0001
		Write = 1 << 1,   // 0010
		Delete = 1 << 2,  // 0100
		Admin = 1 << 3    // 1000
	}
}