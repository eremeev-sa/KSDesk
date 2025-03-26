using KanbanApp.Core.Model;
using KanbanApp.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanApp.DataAccess.Configurations
{
	internal class UserKanbanConfiguration : IEntityTypeConfiguration<UserKanbanEntity>
	{
		public void Configure(EntityTypeBuilder<UserKanbanEntity> builder)
		{
			// Основной ключ (ID) для сущности
			builder.HasKey(x => x.Id);

			// Cвойства Name
			builder.Property(b => b.Name)
				.HasMaxLength(UserKanban.MAX_NAME_LENGTH)  // Максимальная длина имени согласно константе в модели UserKanban
				.IsRequired();  // Это поле обязательно для заполнения

			// Свойство Login с аналогичными ограничениями
			builder.Property(b => b.Login)
				.HasMaxLength(UserKanban.MAX_LOGIN_LENGTH)  // Максимальная длина логина
				.IsRequired();  // Логин обязателен

			// Свойство Password
			builder.Property(b => b.Password)
				.HasMaxLength(UserKanban.MAX_PASSWORD_LENGTH)  // Максимальная длина пароля
				.IsRequired();  // Пароль обязателен для заполнения

			// Преобразование перечисления Permissions
			builder.Property(b => b.Permissions)
				.HasConversion<int>();  // Перечисление Permissions будет сохраняться в базе как целое число (например, 0 для Read, 1 для Write и т.д.)

			}

	}
}