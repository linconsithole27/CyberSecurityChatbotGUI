using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CyberSecurityChatbotGUI
{
    public partial class MainWindow : Window

    {
        private Random random = new Random();
        private string favouriteTopic = "";
        private string currentTopic = "";
        private DatabaseHelper database = new DatabaseHelper();
        private List<string> activityLog = new List<string>();

        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();

        private int currentQuestion = 0;

        private int score = 0;
        private void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Play();
            }
            catch
            {
                MessageBox.Show("Voice greeting could not play");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            LoadQuizQuestions();
            PlayVoiceGreeting();

            ChatDisplay.Text =
                @"   _____      _               _____           
  / ____|    | |             / ____|          
 | |    _   _| |__   ___ _ _| (___   ___  ___ 
 | |   | | | | '_ \ / _ \ '__\___ \ / _ \/ __|
 | |___| |_| | |_) |  __/ |  ____) |  __/ (__ 
  \_____\__, |_.__/ \___|_| |_____/ \___|\___|
         __/ |                                
        |___/                                 

     Welcome to the Cybersecurity Awareness Chatbot";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            string userMessage = UserInput.Text;

            ChatDisplay.Text += "\nYou: " + userMessage;

            string response = GetResponse(userMessage);

            ChatDisplay.Text += "\nBot: " + response + "\n";

            UserInput.Clear();
        }
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private string GetResponse(string input)
        {
            input = input.ToLower();
            if (input.Contains("start quiz") || input.Contains("quiz"))
            {
                StartQuizButton_Click(null, null);

                return "Starting the cybersecurity quiz.";
            }
            if (input.Contains("activity log") ||
    input.Contains("what have you done") ||
    input.Contains("show log"))
            {
                ActivityLogButton_Click(null, null);

                return "Displaying your activity log.";
            }
            if (input.Contains("show tasks") ||
    input.Contains("view tasks") ||
    input.Contains("my tasks"))
            {
                ViewTasksButton_Click(null, null);

                return "Here are your saved tasks.";
            }
            if (input.Contains("add task") ||
            input.Contains("create task") ||
            input.Contains("new task"))
            {
                TaskItem task = new TaskItem();

                task.Title = input.Replace("add task", "").Trim();

                if (string.IsNullOrWhiteSpace(task.Title))
                {
                    task.Title = "Cybersecurity Task";
                }

                task.Description = "Added through chatbot NLP";

                task.ReminderDate = null;

                task.Completed = false;

                database.AddTask(task);

                activityLog.Add("Task added through chatbot: " + task.Title);

                return "Your task has been added successfully!";
            }
            // Sentiment Detection
            if (input.Contains("worried"))
            {
                return "It is understandable to feel worried about online scams. Always verify suspicious links and emails before clicking them.";
            }

            if (input.Contains("frustrated"))
            {
                return "Cybersecurity can feel overwhelming sometimes but taking small safety steps helps a lot.";
            }

            if (input.Contains("curious"))
            {
                return "Curiosity is great. Learning about cybersecurity helps you stay safer online.";
            }

            // Memory System
            if (input.Contains("i like privacy"))
            {
                favouriteTopic = "privacy";

                return "Great I will remember that you are interested in privacy";
            }

            if (input.Contains("i like passwords"))
            {
                favouriteTopic = "passwords";

                return "Great I will remember that you are interested in passwords";
            }
            if (input.StartsWith("complete task"))
            {
                string number = input.Replace("complete task", "").Trim();

                if (int.TryParse(number, out int id))
                {
                    database.CompleteTask(id);

                    activityLog.Add("Task completed through chatbot. ID: " + id);

                    return "Task marked as completed.";
                }

                return "Please enter a valid task ID.";
            }

            if (input.StartsWith("delete task"))
            {
                string number = input.Replace("delete task", "").Trim();

                if (int.TryParse(number, out int id))
                {
                    database.DeleteTask(id);

                    activityLog.Add("Task deleted through chatbot. ID: " + id);

                    return "Task deleted successfully.";
                }

                return "Please enter a valid task ID.";
            }
            // Lists for Random Responses
            List<string> passwordResponses = new List<string>()
    {
        "Use strong unique passwords for every account",
        "Avoid using your name or birthday in passwords",
        "Use a mix of letters numbers and symbols"
    };

            List<string> phishingResponses = new List<string>()
    {
        "Do not click suspicious email links",
        "Scammers pretend to be trusted organisations",
        "Always verify emails before entering information"
    };

            List<string> privacyResponses = new List<string>()
    {
        "Review your privacy settings regularly",
        "Do not share personal information publicly",
        "Enable two factor authentication for extra security"
    };

            // Keyword Recognition
            if (input.Contains("password"))
            {
                currentTopic = "password";

                return passwordResponses[random.Next(passwordResponses.Count)];
            }

            if (input.Contains("phishing"))
            {
                currentTopic = "phishing";

                return phishingResponses[random.Next(phishingResponses.Count)];
            }

            if (input.Contains("privacy"))
            {
                currentTopic = "privacy";

                return privacyResponses[random.Next(privacyResponses.Count)];
            }

            // Follow Up Conversation
            if (input.Contains("tell me more") || input.Contains("another tip"))
            {
                if (currentTopic == "password")
                {
                    return "Avoid reusing the same password across multiple accounts";
                }

                if (currentTopic == "phishing")
                {
                    return "Phishing scams often create fake websites to steal your information";
                }

                if (currentTopic == "privacy")
                {
                    return "Always check app permissions before installing applications";
                }
            }

            // Memory Recall
            if (input.Contains("tell me more"))
            {
                if (favouriteTopic == "privacy")
                {
                    return "Since you are interested in privacy remember to review your account settings regularly";
                }

                if (favouriteTopic == "passwords")
                {
                    return "Since you are interested in passwords remember to use strong unique passwords";
                }
            }

            // Default Response
            return "I do not understand please rephrase";
        }


        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskItem task = new TaskItem();

            task.Title = TaskTitle.Text;
            task.Description = TaskDescription.Text;
            task.ReminderDate = ReminderDate.SelectedDate;
            task.Completed = false;

            database.AddTask(task);
            activityLog.Add(DateTime.Now.ToString("g") + " - Task added: " + task.Title);

            ChatDisplay.Text += "\nBot: Task added successfully.\n";

            TaskTitle.Clear();
            TaskDescription.Clear();
            ReminderDate.SelectedDate = null;
        }
        private void ViewTasksButton_Click(object sender, RoutedEventArgs e)
        {
            List<TaskItem> tasks = database.GetTasks();

            ChatDisplay.Text += "\n----- TASK LIST -----\n";

            foreach (TaskItem task in tasks)
            {
                ChatDisplay.Text +=
                  $"ID: {task.Id}\n" +
                  $"Title: {task.Title}\n" +
                  $"Description: {task.Description}\n" +
                  $"Reminder: {task.ReminderDate}\n" +
                  $"Completed: {task.Completed}\n\n";




            }
        }
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int id;

            if (int.TryParse(TaskId.Text, out id))
            {
                database.CompleteTask(id);
                activityLog.Add(DateTime.Now.ToString("g") + " - Task completed. ID: " + id);

                ChatDisplay.Text += "\nBot: Task marked as completed.\n";

                TaskId.Clear();
            }
            else
            {
                ChatDisplay.Text += "\nBot: Please enter a valid Task ID.\n";
            }
        }
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int id;

            if (int.TryParse(TaskId.Text, out id))
            {
                database.DeleteTask(id);
                activityLog.Add(DateTime.Now.ToString("g") + " - Task deleted. ID: " + id);

                ChatDisplay.Text += "\nBot: Task deleted successfully.\n";

                TaskId.Clear();
            }
            else
            {
                ChatDisplay.Text += "\nBot: Please enter a valid Task ID.\n";
            }
        }
        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            ChatDisplay.Text += "\n===== ACTIVITY LOG =====\n";

            if (activityLog.Count == 0)
            {
                ChatDisplay.Text += "No activity has been recorded yet.\n";
                return;
            }

            int start = Math.Max(0, activityLog.Count - 5);

            for (int i = start; i < activityLog.Count; i++)
            {
                ChatDisplay.Text += activityLog[i] + "\n";
            }
        }

        private void LoadQuizQuestions()
        {
            quizQuestions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                OptionA = "Reply with your password",
                OptionB = "Delete the email",
                OptionC = "Report it as phishing",
                OptionD = "Ignore it",
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What makes a password strong?",
                OptionA = "Your birthday",
                OptionB = "123456",
                OptionC = "A mix of letters, numbers and symbols",
                OptionD = "Your surname",
                CorrectAnswer = "C",
                Explanation = "Strong passwords are difficult to guess."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What does 2FA stand for?",
                OptionA = "Two-Factor Authentication",
                OptionB = "Two Fast Accounts",
                OptionC = "Two File Access",
                OptionD = "Twice For All",
                CorrectAnswer = "A",
                Explanation = "2FA adds an extra layer of security."


            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which of these is an example of phishing?",
                OptionA = "An email pretending to be your bank",
                OptionB = "Updating Windows",
                OptionC = "Using antivirus software",
                OptionD = "Changing your password",
                CorrectAnswer = "A",
                Explanation = "Phishing scams often impersonate trusted organisations."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "True or False: You should reuse the same password for all your accounts.",
                OptionA = "True",
                OptionB = "False",
                OptionC = "Not Sure",
                OptionD = "Sometimes",
                CorrectAnswer = "B",
                Explanation = "Each account should have a unique password."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What does HTTPS indicate?",
                OptionA = "A secure website connection",
                OptionB = "A virus",
                OptionC = "A weak password",
                OptionD = "An email attachment",
                CorrectAnswer = "A",
                Explanation = "HTTPS encrypts communication between your browser and the website."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What should you do before clicking a link in an email?",
                OptionA = "Click immediately",
                OptionB = "Check where the link goes",
                OptionC = "Forward it to friends",
                OptionD = "Ignore every email",
                CorrectAnswer = "B",
                Explanation = "Always verify links before clicking."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which is the safest way to protect an online account?",
                OptionA = "Use 2FA",
                OptionB = "Share your password",
                OptionC = "Use 'password123'",
                OptionD = "Never log out",
                CorrectAnswer = "A",
                Explanation = "Two-factor authentication adds another security layer."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What is malware?",
                OptionA = "Malicious software",
                OptionB = "A secure application",
                OptionC = "A password manager",
                OptionD = "An internet browser",
                CorrectAnswer = "A",
                Explanation = "Malware is software designed to damage or steal data."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive a suspicious attachment?",
                OptionA = "Open it immediately",
                OptionB = "Delete or scan it first",
                OptionC = "Forward it",
                OptionD = "Ignore your antivirus",
                CorrectAnswer = "B",
                Explanation = "Never open suspicious attachments without checking them."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Why should you keep your software updated?",
                OptionA = "To fix security vulnerabilities",
                OptionB = "To waste storage",
                OptionC = "To slow down the PC",
                OptionD = "No reason",
                CorrectAnswer = "A",
                Explanation = "Updates often contain important security fixes."
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which information should never be shared publicly?",
                OptionA = "Your favourite colour",
                OptionB = "Your banking password",
                OptionC = "Your hobby",
                OptionD = "Your favourite movie",
                CorrectAnswer = "B",
                Explanation = "Sensitive information like banking passwords should remain private."
            });
        }
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion = 0;
            score = 0;

            activityLog.Add(DateTime.Now.ToString("g") + " - Quiz started");

            ShowQuestion();
        }

        private void OptionAButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("A");
        }

        private void OptionBButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("B");
        }

        private void OptionCButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("C");
        }

        private void OptionDButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer("D");
        }
    

    private void ShowQuestion()
        {
            if (currentQuestion >= quizQuestions.Count)
            {
                QuestionText.Text = $"Quiz Finished!\nYour score is {score}/{quizQuestions.Count}";

                activityLog.Add(DateTime.Now.ToString("g") + $" - Quiz completed. Score: {score}/{quizQuestions.Count}");

                return;
            }

            QuizQuestion question = quizQuestions[currentQuestion];

            QuestionText.Text =
                question.Question +
                "\n\nA. " + question.OptionA +
                "\nB. " + question.OptionB +
                "\nC. " + question.OptionC +
                "\nD. " + question.OptionD;
        }
        private void CheckAnswer(string answer)
        {
            QuizQuestion question = quizQuestions[currentQuestion];

            if (answer == question.CorrectAnswer)
            {
                score++;

                MessageBox.Show(
                    "Correct!\n\n" + question.Explanation,
                    "Quiz");
            }
            else
            {
                MessageBox.Show(
                    "Incorrect.\n\n" + question.Explanation,
                    "Quiz");
            }

            currentQuestion++;

            ShowQuestion();
        }
    }

}