﻿namespace TelegramBot.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Input;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using TelegramBot.Commands;
    using TelegramBot.Models;

    class MainViewModel : INotifyPropertyChanged
    {
        private static TelegramBotClient Bot;

        public TelegramBotClient BotClient
        {
            get { return Bot; }
        }

        public bool IsActive{
            get
            {
                return Bot.IsReceiving;
            }
        }

        #region Commands
        public ICommand ActivateCommand
        {
            get;
            private set;
        }

        public ICommand DeactivateCommand
        {
            get;
            private set;
        }

        public ICommand OpenFilesFolderCommand
        {
            get;
            private set;
        }

        public ICommand ExitApplicationCommand
        {
            get;
            private set;
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private string _log;
        public string Log
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Log"));
            }
        }

        public MainViewModel()
        {
#if DEBUG
            Bot = new TelegramBotClient("447136859:AAGMz8BN0p21JLO7i9Ob4ridbKTUDpCAD1E");
#else
            Bot = new TelegramBotClient("488598835:AAFJAn6w1rifdR6z-8wDsaSxvwXXDVusSgU");
#endif
            ActivateCommand = new ActivateBotCommand(this);
            DeactivateCommand = new DeactivateBotCommand(this);
            OpenFilesFolderCommand = new OpenFilesFolderCommand(this);
            ExitApplicationCommand = new ExitApplicationCommand(this);
            ActivateCommand.Execute(null);
        }

        public void Activate()
        {
            Bot.OnMessage += BotOnMessageReceived;
            Bot.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
        }
        public void Deactivate()
        {
            Bot.StopReceiving();
            Bot.OnMessage -= BotOnMessageReceived;
        }
        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            Random Rnd = new Random();
            Telegram.Bot.Types.Message msg = e.Message;
            if (msg == null || msg.Type != MessageType.TextMessage) return;

            #region Answer logic
            String Answer = "";

            if (msg.Text.StartsWith("/start"))
            {
                Answer = "/flip - подбросить монетку\r\n/rand - случайное число от 1 до 47\r\n/scorebylitvinov - твоя оценка по Литвинову";
            }
            else if (msg.Text.StartsWith("/flip"))
            {
                //Орел и решка
                String flipAnswer = ""; //для хранения ответа, орел решка
                int Random = Rnd.Next(0, 2);
                if (Random == 0)
                {
                    flipAnswer = "Орел";
                }
                else
                {
                    flipAnswer = "Решка";
                }
                Answer = flipAnswer;
            }
            else if(msg.Text.StartsWith("/rand"))
            {
                //Случайное число
                int Rand = Rnd.Next(0, 48);
                Answer = Rand.ToString();
            }
            else if(msg.Text.StartsWith("/scorebylitvinov"))
            {           
                //Узнай свою оценку по Литвинову
                int RandLit = Rnd.Next(1, 11);
                switch (RandLit)
                {
                    case 1:
                        RandLit = Rnd.Next(1, 44);
                        break;
                    case 2:
                        RandLit = Rnd.Next(10, 44);
                        break;
                    case 3:
                        RandLit = Rnd.Next(10, 51);
                        break;
                    case 4:
                        RandLit = Rnd.Next(15, 61);
                        break;
                    case 5:
                        RandLit = Rnd.Next(15, 61);
                        break;
                    case 6:
                        RandLit = Rnd.Next(15, 61);
                        break;
                    case 7:
                        RandLit = Rnd.Next(15, 66);
                        break;
                    case 8:
                        RandLit = Rnd.Next(20, 75);
                        break;
                    case 9:
                        RandLit = Rnd.Next(50, 75);
                        break;
                    case 10:
                        RandLit = Rnd.Next(65, 100);
                        break;
                }
                if (RandLit < 60) { Answer = "Твоя оценка по Литвинову: " + RandLit.ToString();}
                else { Answer = "Твоя оценка по Литвинову: " + RandLit.ToString() + ", гнида ебучая"; }

            }
            else if(msg.Text.StartsWith("/para"))
            {
                //Проверка на то, нужно ли идти на пару
                int Random = Rnd.Next(0, 2);
                String paraAnswer = ""; //для хранения ответа, нужно ли идти на пару
                if (Random == 0)//Используем из Орла и решки, так как похожий принцип
                {
                    paraAnswer = "лучше сходить";
                }
                else
                {
                    paraAnswer = "можно не идти";
                }
                
                Answer = "На следующую пару " + paraAnswer;
            }
            else if (msg.Text.Contains("лаб"))
            {
                FileStream stream = null;
                try
                {
                    stream = FilesAccessor.GetFileByCommand(msg.Text);
                    await Bot.SendDocumentAsync(msg.Chat.Id, new FileToSend(stream.Name, stream), "Лови", false, msg.MessageId);
                    Log += $"\r\n\r\n{DateTime.Now.ToLocalTime().ToString()}:\r\nCommand received:\r\n{msg.Text}\r\nFrom: {e.Message.From.FirstName} {e.Message.From.LastName}\r\nAnswered with file: {stream.Name}";
                }
                catch (Exception)
                {
                    return;
                }
                finally
                {
                    stream?.Dispose();
                }
            }
            #endregion

            if (Answer!="")
            {
                await Bot.SendTextMessageAsync(msg.Chat.Id, Answer, ParseMode.Default, false, false, msg.MessageId);
                Log += $"\r\n\r\n{DateTime.Now.ToLocalTime().ToString()}:\r\nCommand received:\r\n{e.Message.Text}\r\nFrom: {e.Message.From.FirstName} {e.Message.From.LastName}\r\nAnswered with: {Answer}";
            }
            

        }
    }
}
