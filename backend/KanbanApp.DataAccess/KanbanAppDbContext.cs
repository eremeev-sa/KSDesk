using KanbanApp.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.DataAccess
{
	public class KanbanAppDbContext : DbContext
	{
		public KanbanAppDbContext(DbContextOptions<KanbanAppDbContext> options)
			: base(options)
		{
		}

		public DbSet<UserKanbanEntity> Users { get; set; }
		public DbSet<TaskKanbanEntity> Tasks { get; set; }
		public DbSet<BoardKanbanEntity> Boards { get; set; }
		public DbSet<ColumnKanbanEntity> Columns { get; set; }
		public DbSet<SubtaskKanbanEntity> Subtasks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// При удалении доски все связанные с ней колонки удаляются
			modelBuilder.Entity<ColumnKanbanEntity>()
				.HasOne(c => c.Board)
				.WithMany(b => b.Columns)
				.HasForeignKey(c => c.BoardId)
				.OnDelete(DeleteBehavior.Cascade);

			// При удалении колонки все связанные задачи удаляются
			modelBuilder.Entity<TaskKanbanEntity>()
				.HasOne(t => t.Column)
				.WithMany(c => c.Tasks)
				.HasForeignKey(t => t.ColumnId)
				.OnDelete(DeleteBehavior.Cascade);

			// При удалении пользователя задачи остаются, но поле AssignedUserId становится null
			modelBuilder.Entity<TaskKanbanEntity>()
				.HasOne(t => t.Assigned)
				.WithMany(u => u.AssignedTasks)
				.HasForeignKey(t => t.AssignedId)
				.OnDelete(DeleteBehavior.SetNull);

			// При удалении задачи все связанные подзадачи удаляются
			modelBuilder.Entity<SubtaskKanbanEntity>()
				.HasOne(t => t.Task)
				.WithMany(c => c.Subtasks)
				.HasForeignKey(t => t.TaskId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
