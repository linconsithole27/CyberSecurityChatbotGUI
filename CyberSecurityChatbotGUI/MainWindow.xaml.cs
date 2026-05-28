using System;
using System.Collections.Generic;
using System.Windows;

namespace CyberSecurityChatbotGUI
{
    public partial class MainWindow : Window

    {
        private Random random = new Random();
        private string favouriteTopic = "";
        public MainWindow()
        {
            InitializeComponent();

            ChatDisplay.Text = "Welcome to the Cybersecurity Awareness Chatbot\n";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text;

            ChatDisplay.Text += "\nYou: " + userMessage;

            string response = GetResponse(userMessage);

            ChatDisplay.Text += "\nBot: " + response + "\n";

            UserInput.Clear();
        }

        private string GetResponse(string input)
        {
            if (input.Contains("password"))
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
            input = input.ToLower();

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

            if (input.Contains("password"))
            {
                return passwordResponses[random.Next(passwordResponses.Count)];
            }
            else if (input.Contains("phishing"))
            {
                return phishingResponses[random.Next(phishingResponses.Count)];
            }
            else if (input.Contains("privacy"))
            {
                return privacyResponses[random.Next(privacyResponses.Count)];
            }
            else
            {
                return "I do not understand please rephrase";
            }
        }
    }
}