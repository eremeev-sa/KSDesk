using KanbanApp.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanApp.DataAccess.Configurations
{
	public class SubtaskKanbanConfiguration : IEntityTypeConfiguration<SubtaskKanbanEntity>
	{
		public void Configure(EntityTypeBuilder<SubtaskKanbanEntity> builder)
		{
			// Настройка таблицы
			builder.ToTable("Subtask");

			// Первичный ключ
			builder.HasKey(x => x.Id);

			// Настройка поля Name
			builder.Property(x => x.Name)
				   .IsRequired()
				   .HasMaxLength(Core.Models.SubtaskKanban.MAX_SUBTASK_NAME_LENGTH);
		}
	}
}
