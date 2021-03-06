﻿namespace RandomBasedCommandsModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using TelegramBot.Common;

    public class RandomBasedCommands : IBotUpdateHandler
    {
        private IBotUpdateDispatcher _updateDispatcher;
        private IBotLogger _logger;

        public RandomBasedCommands(IBotUpdateDispatcher updateDispatcher, IBotLogger logger)
        {
            _updateDispatcher = updateDispatcher;
            _updateDispatcher.AddHandler(this);
            _logger = logger;
        }

        public async void HandleUpdate(Update update, ITelegramBotClient client)
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

            Message message;
            if (answer != "")
            {
                message = await client.SendTextMessageAsync(update.Message.Chat.Id, answer, ParseMode.Default, false, false, update.Message.MessageId);
                _logger?.LogUpdate(update, message);
            }
        }

        public bool CanHandleUpdate(Update update, ITelegramBotClient client)
        {
            if (update.Type != UpdateType.MessageUpdate || update.Message.Type != MessageType.TextMessage)
                return false;

            string request = Utils.PrettifyCommand(update.Message.Text);

            return request == "/flip"
                || request == "/rand"
                || request == "/scorebylitvinov"
                || request == "/para";
        }
    }
}
