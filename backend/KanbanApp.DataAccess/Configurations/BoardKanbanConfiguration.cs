using KanbanApp.Core.Model;
using KanbanApp.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanApp.DataAccess.Configurations
{
	internal class BoardKanbanConfiguration : IEntityTypeConfiguration<BoardKanbanEntity>
	{
		public void Configure(EntityTypeBuilder<BoardKanbanEntity> builder)
		{
			// Первичный ключ для сущности
			builder.HasKey(x => x.Id);

			// Настройка свойства Name
			builder.Property(b => b.Name)
				.HasMaxLength(BoardKanban.MAX_KANBANBOARD_NAME_LENGTH) // Максимальная длина													  
				.IsRequired();  // Имя обязательно для заполнения
		}
	}
}
