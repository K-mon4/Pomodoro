using System;
namespace Pomodoro.Models
{
	public class TodoListFunctions
	{
		public databaseIO databaseio = new databaseIO();
		public TodoListFunctions()
		{

		}
		public List<Todo> GetTodoList()
		{
			return databaseio.getTodolist();
		}
		public void Addtodb(string name)
		{
			databaseio.addTodo(name);
		}

		public void Deletefromdb(string name)
		{
			databaseio.deleteTodo(name);
		}

		public List<Todo> GetsampleTodoList()
		{
			List<Todo> todolist = new List<Todo>();
			todolist.Add(new Todo("Todo 1"));
			todolist.Add(new Todo("Todo 2"));
			todolist.Add(new Todo("Todo 3"));

            return todolist;
		}
	}
}

