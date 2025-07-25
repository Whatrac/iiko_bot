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



namespace Bot
{
    internal class Work 
    {

        private static TelegramBotClient bot = new("");

        private static CancellationTokenSource cts = new CancellationTokenSource();
        static public async Task Main()
        {
            try
            {
                // Запускаем фоновую задачу для отслеживания ESC
                var escMonitorTask = MonitorEscapeKey(cts);

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandlePollingErrorAsync,
                    receiverOptions,
                    cts.Token
                );

                Console.WriteLine("Бот запущен. Нажмите ESC для остановки.");

                await escMonitorTask;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Работа бота завершена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                cts.Dispose();
            }
        }

        private static async Task MonitorEscapeKey(CancellationTokenSource cts) // смотрим нажат ли esc
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nОбнаружено нажатие ESC. Остановка бота...");
                    cts.Cancel();
                    break;
                }
                await Task.Delay(100); 
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

            if (callbackData == "floart")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "menu_floart")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawMenuFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "info_floart_first")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawInfoFLoArt(chatId);
            }
            else if (callbackData == "place_floart_first")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawPlaseFloArt(chatId);
            }
            else if (callbackData == "floart_back")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "voshod")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await GettingUserChatId(bot);
                await DrawButtonVoshod(chatId);
            }
            else if (callbackData == "menu_voshod")
            {
                await GettingUserChatId(bot);
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawMenuVoshod(chatId);
            }
            else if (callbackData == "info_voshod_first")
            {
                await GettingUserChatId(bot);
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawInfoVoshod(chatId);
            }
            else if (callbackData == "place_voshod_first")
            {
                await GettingUserChatId(bot);
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawPlaseVoshod(chatId);
            }
            else if (callbackData == "rest_back")
            {
                await GettingUserChatId(bot);
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFirst(chatId);
            }
            else if (callbackData == "voshod_back")
            {
                await GettingUserChatId(bot);
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonVoshod(chatId);
            }
        }

        static async Task DrawButtonFirst(long chatId)
        {
            var keyboard1 = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"FloArt",
                        callbackData:"floart")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Восход",
                        callbackData:"voshod")}

            });
            await bot.SendTextMessageAsync(chatId, "Выберите заведение", replyMarkup: keyboard1);
        }
        static async Task DrawButtonFloArt(long chatId)
        {

            var keyboard2 = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Меню FloArt",
                        callbackData:"menu_floart")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Узнать больше о кофейне",
                        callbackData:"info_floart_first")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Место нахождения",
                        callbackData:"place_floart_first")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору ресторана",
                        callbackData:"rest_back")}


            });

            await bot.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: keyboard2);
        }
        static async Task DrawButtonVoshod(long chatId)
        {
            var keyboard3 = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Меню Восход",
                        callbackData:"menu_voshod")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Узнать больше о кофейне 'Восход'",
                        callbackData:"info_voshod_first")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Место положение кафе 'Восход'",
                        callbackData:"place_voshod_first")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору ресторана",
                        callbackData:"rest_back")}


            });
            await bot.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: keyboard3);
        }
        static async Task DrawMenuFloArt(long chatId)
        {
            var keyboardFloArt = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("скоро тут появится меню кафе FloArt")},
                new[] {InlineKeyboardButton.WithCallbackData(
                    text:"Вернуться к выбору действия",
                    callbackData:"floart_back")}
            });

            await bot.SendTextMessageAsync(chatId, "Выберите любую позицию", replyMarkup: keyboardFloArt);
        }

        static async Task DrawMenuVoshod(long chatId)
        {
            var keyboardVoshod = new InlineKeyboardMarkup(new[]
            {
                new[] 
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "скоро тут появится меню кафе Восход", 
                        callbackData: "voshod_menu_soon")
                },
                new[] 
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Вернуться к выбору действия", 
                        callbackData: "voshod_back")  
                }
            });

            await bot.SendTextMessageAsync(chatId, "Выберите любую позицию.", replyMarkup: keyboardVoshod);
        }

        static async Task DrawInfoFLoArt(long chatId)
        {
            var keyboardFloArtInfo = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия",
                        callbackData:"floart_back")}
            });
            await bot.SendTextMessageAsync(chatId, "Тут очень скоро появится информация о кофейне FloArt",replyMarkup: keyboardFloArtInfo);
        }
        static async Task DrawInfoVoshod(long chatId)
        {
            var keyboardVoshodInfo = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия",
                        callbackData:"voshod_back")}
            });
            await bot.SendTextMessageAsync(chatId, "Тут очень скоро появится информация о кофейне Восход",replyMarkup: keyboardVoshodInfo);
        }

        static async Task DrawPlaseFloArt(long chatId)
        {
            var keyboardPlaseFlo = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Найти FloArt на карте")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия(FloArt)",
                        callbackData:"floart_back")}
            });

            await bot.SendTextMessageAsync(chatId, "Тут скоро будет место положение кофейни FLoArt", replyMarkup: keyboardPlaseFlo);
        }

        static async Task DrawPlaseVoshod(long chatId)
        {
            var keyboardPlaseVoshod = new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData("Найти Восход на карте")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия(Восход)",
                        callbackData:"voshod_back")}
            });

            await bot.SendTextMessageAsync(chatId, "Тут очень скоро будет место положение кафе Восход", replyMarkup: keyboardPlaseVoshod);
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
