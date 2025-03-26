using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using KanbanApp.Core.Model;

public class TaskKanbanConfiguration : IEntityTypeConfiguration<TaskKanbanEntity>
{
	public void Configure(EntityTypeBuilder<TaskKanbanEntity> builder)
	{
		// Устанавливаем первичный ключ для сущности
		builder.HasKey(x => x.Id);

		// Настройка свойства Name
		builder.Property(b => b.Name)
			.HasMaxLength(TaskKanban.MAX_TASK_NAME_LENGTH) // Ограничение длины имени задачи
			.IsRequired(); // Задача обязательно должна иметь имя

		// Свойство Priority (приоритет)
		builder.Property(b => b.Priority)
			.IsRequired(); 

		// Свойство Description (описание задачи)
		builder.Property(b => b.Description);
	}
}
