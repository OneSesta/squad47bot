﻿namespace TelegramBot.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    class RandomBasedCommands : IBotCommandHandler
    {
        private TelegramBotClient _client;

        public RandomBasedCommands(TelegramBotClient client)
        {
            _client = client;
        }

        public void HandleUpdate(Update update)
        {
            string request = Utils.PrettifyCommand(update.Message.Text);
            string answer="";
            Random random = new Random();

            switch (request)
            {
                //случайное от 0 до 47
                case "/rand":
                    answer = random.Next(0, 48).ToString();
                    break;

                //орёл или решка
                case "/flip":
                    answer = random.Next(0, 2) == 0 ? "Орёл" : "Решка";
                    break;

                // Узнай свою оценку по Литвинову
                case "/scorebylitvinov":
                    int rand = random.Next(1, 11);
                    if (rand < 9)
                        rand = random.Next(1, 61);
                    else
                        rand = random.Next(61, 101);

                    if (rand < 40)
                        answer = "По Литвинову ты идешь на пересдачу";
                    else if (rand < 60)
                        answer = $"Твоя оценка по Литвинову: {rand}";
                    else
                        answer = $"Твоя оценка по Литвинову: {rand}, гнида ебучая";
                    break;

                case "/para":
                    answer = $"На следующую пару {(random.Next(0, 2) == 0 ? "можно не идти" : "лучше сходить")}";
                    break;
            }

            if (answer!="")
                _client.SendTextMessageAsync(update.Message.Chat.Id, answer, ParseMode.Default, false, false, update.Message.MessageId);
        }

        public bool CanHandleUpdate(Update update)
        {
            if (update.Type != UpdateType.MessageUpdate)
                return false;

            string request = Utils.PrettifyCommand(update.Message.Text);

            return request == "/flip"
                || request == "/rand"
                || request == "/scorebylitvinov"
                || request == "/para";
        }
    }
}