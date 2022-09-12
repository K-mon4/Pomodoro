using SQLite;

using System;


namespace Pomodoro.Models
{
    [Table("Tododb")]
    public class TodoDB
    {
        [PrimaryKey,Column("todoname"), Unique]
        public string TodoName { get; set; }
        [Column("lastdone")]
        public string LastDone { get; set; }
        [Column("timecreated")]
        public string CreatedTime { get; set; }
        [Column("totaltime")]
        public int TimeTotal { get; set; }
    }

    [Table("TodoLog")]
    public class todoLog
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }

        [Column("todoname")]
        public string TodoName { get; set; }

        [Column("startedtime")]
        public string StartedTime { get; set; }

        [Column("endedtime")]
        public string EndedTime { get; set; }

        // Timespan as minutes
        [Column("duration")]
        public int Duration { get; set; }
    }

    public class databaseIO
    {
        public SQLiteConnection conn = new SQLiteConnection(Path.Combine(filepath, filename));
        public databaseIO()
        {
            
            conn.CreateTable<TodoDB>();
            conn.CreateTable<todoLog>();
        }
        public static string filepath = FileSystem.AppDataDirectory;
        public static string filename = "todolist.db";
        
        public List<Todo> getTodolist()
        {
            List<TodoDB> tdblist = conn.Table<TodoDB>().ToList();
            List<Todo> tdlist = new List<Todo>();
            foreach(TodoDB tdb in  tdblist)
            {
                
                Todo todo = new Todo(tdb.TodoName, tdb.CreatedTime, tdb.LastDone, tdb.TimeTotal);
                tdlist.Add(todo);
            }
            return tdlist;
        }


        public int addTodo(string name)
        {

            int result = conn.Insert(new TodoDB {
                TodoName= name, CreatedTime=DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"),
                LastDone= DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.FFFFFF"), TimeTotal=0 });
            return result;
        }

        public void deleteTodo(string name)
        {
            conn.Delete<TodoDB>(name);


        }

        public int addLog(string todoname, string startedtime, string endedtime, int duration)
        {
            todoLog tdl = new todoLog
            {
                TodoName = todoname,
                StartedTime = startedtime,
                EndedTime = endedtime,
                Duration = duration
            };
            int result = conn.Insert(tdl);

            TodoDB tdb = (from u in conn.Table<TodoDB>()
                          where u.TodoName == todoname
                          select u).FirstOrDefault();
            if(tdb == null)
            {
                addTodo(todoname);
            }
            tdb.TimeTotal += duration;

            conn.Update(tdb);

            return result;
        }

        public List<todoLog> getLogs()
        {
            List<todoLog> loglist = conn.Table<todoLog>().ToList();
            return loglist;
        }

    }
    

}

