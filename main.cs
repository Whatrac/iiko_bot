using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.WebSockets;


namespace Bot
{
    internal class Work 
    {

        private static TelegramBotClient bot = new("");
        static public async Task Main()
        {
            try
            {
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // Получаем все типы обновлений
                };
                bot.StartReceiving
                (
                    HandleUpdateAsync,
                    HandlePollingErrorAsync,
                    receiverOptions,
                    new CancellationToken()
                );
                Console.WriteLine("Бот запущен. Нажмите Ctrl+C для остановки.");    
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            Console.WriteLine("Обновление получено");
            if (update.Message is { } message)
            {
                Console.WriteLine($"Сообщение: {update.Message.Text}");
                if (message.Text == "/start")
                {
                    await DrawButtonFirst(message.Chat.Id);
                }
            }

            else if (update.CallbackQuery is { } callbackQuery)
            {
                await HandleCallbackQuery(botClient, callbackQuery);
            }
        }


        static public async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var chatId = callbackQuery.Message.Chat.Id;
            var callbackData = callbackQuery.Data;
            var messageIdToDelete = callbackQuery.Message.MessageId;

            if (callbackData == "FloArt")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Меню FloArt")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawMenuFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Узнать больше о кофейне 'FloArt'")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawInfoFLoArt(chatId);
            }
            else if (callbackData == "Узнать больше о кофейне 'Восход'")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawInfoVoshod(chatId);
            }
            else if (callbackData == "Место нахождения FloArt")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);

                await botClient.SendTextMessageAsync(chatId, "Тут скоро будет наше место положение");
            }
            else if (callbackData == "Восход")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawButtonVoshod(chatId);
            }
            else if (callbackData == "Меню 'Восход'")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawMenuVoshod(chatId);
            }
            else if (callbackData == "Место положение кафе 'Восход'")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await botClient.SendTextMessageAsync(chatId, "Тут очень скоро будет место положение кафе 'Восход'");
            }
            else if (callbackData == "Вернуться к выбору ресторана")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFirst(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Вернуться к выбору действия(FloArt)")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Вернутся к выбору действия(Восход)")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawButtonVoshod(chatId);
            }
        }

        static async Task DrawButtonFirst(long chatId)
        {
            var keyboard1 = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("FloArt")},
                new[] {InlineKeyboardButton.WithCallbackData("Восход")}

            });
            await bot.SendTextMessageAsync(chatId, "Выберите заведение", replyMarkup: keyboard1);
        }
        static async Task DrawButtonFloArt(long chatId)
        {

            var keyboard2 = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Меню FloArt")},
                new[] {InlineKeyboardButton.WithCallbackData("Узнать больше о кофейне 'FloArt'")},
                new[] {InlineKeyboardButton.WithCallbackData("Место нахождения FloArt")},
                new[] {InlineKeyboardButton.WithCallbackData("Вернуться к выбору ресторана")}


            });

            await bot.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: keyboard2);
        }
        static async Task DrawButtonVoshod(long chatId)
        {
            var keyboard3 = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Меню 'Восход'")},
                new[] {InlineKeyboardButton.WithCallbackData("Узнать больше о кофейне 'Восход'")},
                new[] {InlineKeyboardButton.WithCallbackData("Место положение кафе 'Восход'")},
                new[] {InlineKeyboardButton.WithCallbackData("Вернуться к выбору ресторана")}


            });
            await bot.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: keyboard3);
        }
        static async Task DrawMenuFloArt(long chatId)
        {
            var keyboardFloArt = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("скоро тут появится меню кафе FloArt")},
                new[] {InlineKeyboardButton.WithCallbackData("Вернуться к выбору действия(FloArt)")}
            });

            await bot.SendTextMessageAsync(chatId, "Выберите любую позицию", replyMarkup: keyboardFloArt);
        }

        static async Task DrawMenuVoshod(long chatId)
        {
            var keyboardVoshod = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("скоро тут появится меню кафе Восход")},
                new[] {InlineKeyboardButton.WithCallbackData("Вернутся к выбору действия(Восход)")}
            });

            await bot.SendTextMessageAsync(chatId, "Выберите любую позицию.", replyMarkup: keyboardVoshod);
        }

        static async Task DrawInfoFLoArt(long chatId)
        {
            var keyboardFloArtInfo = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Вернуться к выбору действия(FloArt)")}
            });
            await bot.SendTextMessageAsync(chatId, "Тут очень скоро появится информация о кофейне FloArt",replyMarkup: keyboardFloArtInfo);
        }
        static async Task DrawInfoVoshod(long chatId)
        {
            var keyboardVoshodInfo = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Вернутся к выбору действия(Восход)")}
            });
            await bot.SendTextMessageAsync(chatId, "Тут очень скоро появится информация о кофейне Восход",replyMarkup: keyboardVoshodInfo);
        }
        



        static async Task GettingUserChatId(TelegramBotClient bot)
        {
            var updates = await bot.GetUpdatesAsync();

            foreach (var update in updates)
            {
                // Если это сообщение от пользователя
                if (update.Message != null)
                {
                    Console.WriteLine($"Username: @{update.Message.From.Username}");
                    Console.WriteLine($"Chat ID: {update.Message.Chat.Id}");
                }
                // Если это нажатие Inline-кнопки
                else if (update.CallbackQuery != null)
                {
                    Console.WriteLine($"Username: @{update.CallbackQuery.From.Username}");
                    Console.WriteLine($"Chat ID: {update.CallbackQuery.Message.Chat.Id}");
                }
            }
        }
    }
}
