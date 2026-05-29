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
    }
    }
