﻿namespace Todo_App.Domain.Entities;

public class TodoItemTag : BaseAuditableEntity
{
    public int TodoItemId { get; set; }
    public TodoItem TodoItem { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
