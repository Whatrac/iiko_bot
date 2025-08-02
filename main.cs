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
            else if (callbackData == "kofe")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawKofeFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "espresso")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "floart_back")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawButtonFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "floart_back_razdel")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawMenuFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "floart_back_kofe")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawKofeFloArt(chatId);
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
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Кофе",
                        callbackData:"kofe"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Чай листовой",
                        callbackData:"tea_list"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Сезонные напитки",
                        callbackData:"seasonal_drinks"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Свежевыжатые соки",
                        callbackData:"juice"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Лимонады",
                        callbackData:"limonade"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Чай без чая",
                        callbackData:"tea_not_tea"
                    )
                },
                new[] {InlineKeyboardButton.WithCallbackData(
                    text:"Вернуться к выбору действия",
                    callbackData:"floart_back")}
            });

            await bot.SendTextMessageAsync(chatId, "Чтобы вы хотели взять?", replyMarkup: keyboardFloArt);
        }

        static async Task DrawKofeFloArt(long chatId)
        {
            var keyboardKofeFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Эспрессо",
                        callbackData: "espresso"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Американо",
                        callbackData:"amricano"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Капучино",
                        callbackData: "capuchino"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Латте",
                        callbackData: "latte"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Раф",
                        callbackData:"raf"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Флет Уайт",
                        callbackData: "flat"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData: "floart_back_razdel"
                    )
                }
            });
            bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardKofeFloArt);
        }
        static async Task DrawSupplementsFloArt(long chatId)
        {
            var keyboardSupplementsFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Альтернативное молоко",
                        callbackData: "alternativ_milk"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Сиропы",
                        callbackData: "syrups"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Вернуться к выбору напитка",
                        callbackData: "floart_back_kofe"
                    )
                }
            });
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
                    InlineKeyboardButton.WithUrl(
                        text:"Перейти на наш сайт",
                        url:"https://www.flo-art.ru/#!/tab/817656526-1"
                    )},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия",
                        callbackData:"floart_back")}
            });
            await bot.SendTextMessageAsync(chatId, "ФЛО АРТ — кофейня, где вы можете наслаждаться прекрасным ароматным кофе, вкусной едой и мастер-классами в атмосфере полного спокойствия среди живых цветов\nТак же у нас можно заказать цветы",replyMarkup: keyboardFloArtInfo);
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
                new[]
                { InlineKeyboardButton.WithUrl(
                    text:"Найти FloArt на карте",
                    url:"https://yandex.ru/maps/-/CHXrUJix")},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text:"Вернуться к выбору действия",
                        callbackData:"floart_back")}
            });

            await bot.SendTextMessageAsync(chatId, "Вы можете построить маршрут до FloArt нажав кнопку", replyMarkup: keyboardPlaseFlo);
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
