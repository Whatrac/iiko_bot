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

        private static TelegramBotClient bot = new("7566820191:AAHdyTDFzjXeTASp_9ECfg-sax7fli9dfl4");

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
            else if (callbackData == "tea_list")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawTeaListFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "fresh_juice")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawFreshFLoArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "seasonal_drinks")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSeasonalDrinksFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "tea_not_tea")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawTeaNotTeaFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "alternativ_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawAlternativMilkFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "signature_drinks")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSignatureDrinksFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "limonade")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawLimonadeFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "espresso")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "americano")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "capuchino")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSizeCapuchinoFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "250ml")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "latte")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSupplementsFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "raf")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "flat")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSupplementsFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "assam")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "erl_grey")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "pyer")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "sencha")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "milk_ylyn")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "silver_tea")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "read_pole_cbor")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "power_of_nature")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "wild_cherry")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "berry_punch")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Cranberry-sea_buckthorn_tea")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "mulled_wine")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "latte_popkorn")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "raf_banana")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "raf_lavanda")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "raf_halva")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "bumble")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "glace")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "cocoa")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "latte_matcha")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Espresso_Tonic")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "fresh_apple")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "fresh_carrot")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "fresh_orange")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "fresh_grapefruit")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Pink_bouquet")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Teleport_to_Hawaii")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "Lantata")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "mindal_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "banana_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "oat_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "coconut_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "lactose_free_milk")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
            }
            else if (callbackData == "syrups") // в дальнейшем в идеале конечно добавить список сиропов, но хз как это сделать потому что там нету его нигде
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Сироп добавят в ваш напиток");
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
            else if (callbackData == "floart_back_kofe_supplement")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await DrawSupplementsFloArt(chatId);
                await GettingUserChatId(bot);
            }
            else if (callbackData == "voshod")
            {
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
            else if (callbackData == "not_supplement")
            {
                await bot.DeleteMessage(chatId, messageIdToDelete);
                await bot.SendTextMessageAsync(chatId, "Отличный выбор! Ваш заказ передам в предприятие. Оплата происходит на месте.\nЕсли вы хотите взять что-то еще напишите команду /start");
                await GettingUserChatId(bot);
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
                        callbackData:"fresh_juice"
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
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Авторские напитки",
                        callbackData:"signature_drinks"
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
                        callbackData:"americano"
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
            await bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardKofeFloArt);
        }
        static async Task DrawSizeCapuchinoFloArt(long chatId)
        {
            var keyboardSizeCapuchinoFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"250мл",
                        callbackData:"250ml"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"300мл",
                        callbackData:"300ml"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору кофе",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите размер своего напитка", replyMarkup: keyboardSizeCapuchinoFloArt);
        }
        static async Task DrawTeaListFloArt(long chatId)
        {
            var keyboardTeaListFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Ассам",
                        callbackData: "assam"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Эрл грей",
                        callbackData: "erl_grey"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Пуэр",
                        callbackData: "pyer"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Сенча",
                        callbackData: "sencha"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Молочный улун",
                        callbackData:"milk_ylyn"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Серебряный жасмин",
                        callbackData: "silver_tea"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Краснополянский сбор",
                        callbackData: "read_pole_cbor"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Сила природы",
                        callbackData: "power_of_nature"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Дикая Вишня",
                        callbackData:"wild_cherry"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Ягодный пунш",
                        callbackData:"berry_punch"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardTeaListFloArt);
        }
        static async Task DrawSeasonalDrinksFloArt(long chatId)
        {
            var keyboardSeasonalDrinks = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Чай клюква-облепиха",
                        callbackData:"Cranberry-sea_buckthorn_tea"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Глинтвейн",
                        callbackData:"mulled_wine"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Латте попкорн",
                        callbackData:"latte_popkorn"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }

            });
            await bot.SendTextMessageAsync(chatId, "Предзаказа нету гг короче", replyMarkup: keyboardSeasonalDrinks);
        }
        static async Task DrawFreshFLoArt(long chatId)
        {
            var keyboardFreshFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Яблоко",
                        callbackData:"fresh_apple"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Морковь",
                        callbackData:"fresh_carrot"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Апельсин",
                        callbackData:"fresh_orange"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Грейпфрут",
                        callbackData:"fresh_grapefruit"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardFreshFloArt);
        }
        static async Task DrawSignatureDrinksFloArt(long chatId)
        {
            var keyboardSignatureDrinksFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Раф Банан",
                        callbackData:"raf_banana"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Раф Лаванда",
                        callbackData:"raf_lavanda"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Раф Халва",
                        callbackData:"raf_halva"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Бамбл",
                        callbackData:"bumble"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Глясе",
                        callbackData:"glace"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Какао",
                        callbackData:"cocoa"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Латте Матча",
                        callbackData:"latte_matcha"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Эспрессо Тоник",
                        callbackData:"Espresso_Tonic"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardSignatureDrinksFloArt);
        }
        static async Task DrawLimonadeFloArt(long chatId)
        {
            var keyboardLimonadeFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Розовый букет",
                        callbackData:"Pink_bouquet"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Телепорт на Гавайи",
                        callbackData:"Teleport_to_Hawaii"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Лантата",
                        callbackData:"Lantata"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardLimonadeFloArt);
        }
        static async Task DrawTeaNotTeaFloArt(long chatId)
        {
            var keyboardTeaNotTeaFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Ягодный антистресс",
                        callbackData:"Berry_anti-stress"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Тропический имбирь",
                        callbackData:"Tropical_Ginger"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Облепиховый рассвет",
                        callbackData:"Sea_Buckthorn_Dawn"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору раздела",
                        callbackData:"floart_back_razdel"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите любой напиток", replyMarkup: keyboardTeaNotTeaFloArt);
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
                        text:"Мне не нужны никакие добавки",
                        callbackData:"not_supplement"
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
            await bot.SendTextMessageAsync(chatId, "Выберите любую добавку", replyMarkup: keyboardSupplementsFloArt);
        }
        static async Task DrawAlternativMilkFloArt(long chatId)
        {
            var keyboardAlternativMilkFloArt = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Минадльное",
                        callbackData:"mindal_milk"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Банановое",
                        callbackData:"banana_milk"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Овсяное",
                        callbackData:"oat_milk"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Кокосовое",
                        callbackData:"coconut_milk"
                    ),
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Безлактозное",
                        callbackData:"lactose_free_milk"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text:"Вернуться к выбору добавки",
                        callbackData:"floart_back_kofe_supplement"
                    )
                }
            });
            await bot.SendTextMessageAsync(chatId, "Выберите любое альтернативное молоко", replyMarkup: keyboardAlternativMilkFloArt);
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
            await bot.SendTextMessageAsync(chatId, "ФЛО АРТ — кофейня, где вы можете наслаждаться прекрасным ароматным кофе, вкусной едой и мастер-классами в атмосфере полного спокойствия среди живых цветов\nТак же у нас можно заказать цветы", replyMarkup: keyboardFloArtInfo);
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
            await bot.SendTextMessageAsync(chatId, "Тут очень скоро появится информация о кофейне Восход", replyMarkup: keyboardVoshodInfo);
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
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient bot)
        {
            var callbackData = callbackQuery.Data;
            
            if (callbackData == "floart") 
            {
                long groupId = -4907268421; // замените на реальный ID группы
                
                try
                {
                    var groupKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Кнопка в группе", "group_button_data")
                    });
                    
                    await bot.SendTextMessageAsync(
                        chatId: groupId,
                        text: $"Пользователь {callbackQuery.From.FirstName} нажал кнопку {callbackData}",
                        replyMarkup: groupKeyboard);
                    
                    await bot.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "Ваше действие отправлено в группу");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке в группу: {ex.Message}");
                }
            }
        }
    }
}
