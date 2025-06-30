﻿namespace Todo_App.Application.TodoLists.Queries.GetTodos;

public class TodosVm
{
    public IList<PriorityLevelDto> PriorityLevels { get; set; } = new List<PriorityLevelDto>();
    public IList<ColourDto> SupportedColours { get; set; } = new List<ColourDto>();
    public IList<TodoListDto> Lists { get; set; } = new List<TodoListDto>();
}
