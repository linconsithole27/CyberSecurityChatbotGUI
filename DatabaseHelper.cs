using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace CyberSecurityChatbotGUI
{
    public class DatabaseHelper
    {
        private string connectionString =
            "server=localhost;database=cybersecuritychatbot;uid=root;pwd=;";

        public MySqlConnection GetConnection()
        {

            return new MySqlConnection(connectionString);
        }
        public void AddTask(TaskItem task)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO tasks (title, description, reminder_date, completed) VALUES (@title, @description, @reminder, @completed)";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@title", task.Title);
                command.Parameters.AddWithValue("@description", task.Description);
                command.Parameters.AddWithValue("@reminder",
                    task.ReminderDate.HasValue ? task.ReminderDate.Value : DBNull.Value);
                command.Parameters.AddWithValue("@completed", task.Completed);

                command.ExecuteNonQuery();
            }
        }
            public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM tasks";

                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TaskItem task = new TaskItem();

                    task.Id = Convert.ToInt32(reader["id"]);
                    task.Title = reader["title"].ToString();
                    task.Description = reader["description"].ToString();

                    if (reader["reminder_date"] != DBNull.Value)
                        task.ReminderDate = Convert.ToDateTime(reader["reminder_date"]);

                    task.Completed = Convert.ToBoolean(reader["completed"]);

                    tasks.Add(task);
                }
            }

            return tasks;
        }
        public void CompleteTask(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE tasks SET completed = true WHERE id = @id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }
            public void DeleteTask(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM tasks WHERE id = @id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }
    }
    }
    
