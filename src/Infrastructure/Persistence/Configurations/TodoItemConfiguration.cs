using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Note)
            .HasMaxLength(2);

        builder.HasMany(t => t.Tags)
            .WithOne(t => t.TodoItem)
            .HasForeignKey(t => t.TodoItemId);

        builder.Property(x => x.Colour)
          .HasConversion(
              v => v.Code,
              v => Colour.From(v));
    }
}
