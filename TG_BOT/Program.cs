using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5758951049:AAFD6BCcrdlqHSfWUxF7syESPZKvPAojcSU");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)

{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var cen = "< 3к ммр 80р одна победа\r\n" + "3-5 к ммр 120 рублей одна победа\r\n" + "5-6к ммр 300 рублей одна победа";
    var CL = "Здравствуй, 2к ммр узник :)\r\n" + "TG @BronPluzy , @esiw_yar";
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Метовые герои", "Буст аккаунтов", "Видео гайд", "Контакты","Цены"},
})
    {
        ResizeKeyboard = true
    };

    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Choose a response",
        replyMarkup: replyKeyboardMarkup,
        cancellationToken: cancellationToken);
    Message sentMessage1 = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText,
        cancellationToken: cancellationToken);

    if (messageText == "Контакты")
    {
        await botClient.SendTextMessageAsync(chatId, CL, cancellationToken: cancellationToken);
    }
    if (messageText == "Буст аккаунтов")
    {
        await botClient.SendTextMessageAsync(chatId, "Наша организация ООО МИД ОР ФИД предоставляет услуги буста", cancellationToken: cancellationToken);
    }
    if (messageText == "Видео гайд")
    {

        await botClient.SendVideoAsync(
        chatId: chatId,
            video: InputFile.FromUri("https://github.com/Esiw12/BOT/blob/main/Stepa/collapse-looks-nothing-than-arise-magnus-dota2-shorts-magnus-collapse-_(VIDEOMIN.NET).mp4"),

        thumbnail: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg"),
        supportsStreaming: true,
        cancellationToken: cancellationToken);
    }
    if (messageText == "Метовые герои")
    {
        await botClient.SendMediaGroupAsync(
        chatId: chatId,
    media: new IAlbumInputMedia[]
    {
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Esiw12/BOT/blob/main/Stepa/image%20(2).jpg")),
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Esiw12/BOT/blob/main/Stepa/image%20(3).jpg")),
    },
    cancellationToken: cancellationToken);
    }
    if (messageText == "Цены")
    {
        await botClient.SendTextMessageAsync(chatId, cen, cancellationToken: cancellationToken);


    }


}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}   