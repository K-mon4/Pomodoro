using System;
namespace Pomodoro.Models
{
	public class TodoListFunctions
	{
		public TodoListFunctions()
		{
		}

		public List<Todo> GetTodoList()
		{
			List<Todo> todolist = new List<Todo>();
			todolist.Add(new Todo("Todo 1"));
			todolist.Add(new Todo("Todo 2"));
			todolist.Add(new Todo("Todo 3"));

            return todolist;
		}
	}
}

