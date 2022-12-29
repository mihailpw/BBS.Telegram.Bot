using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Utils;

public static class UpdateExt
{
    public static long? GetChatId(this Update update)
        => update switch
        {
            { Message: { } }            => update.Message.Chat.Id,
            { EditedMessage: { } }      => update.EditedMessage.Chat.Id,
            { ChannelPost: { } }        => update.ChannelPost.Chat.Id,
            { EditedChannelPost: { } }  => update.EditedChannelPost.Chat.Id,
            { MyChatMember: { } }       => update.MyChatMember.Chat.Id,
            { ChatMember: { } }         => update.ChatMember.Chat.Id,
            { ChatJoinRequest: { } }    => update.ChatJoinRequest.Chat.Id,
            _                           => null
        };

    public static long? GetUserId(this Update update)
        => update switch
        {
            { Message: { } }            => update.Message.From?.Id,
            { EditedMessage: { } }      => update.EditedMessage.From?.Id,
            { InlineQuery: { } }        => update.InlineQuery.From.Id,
            { ChosenInlineResult: { } } => update.ChosenInlineResult.From.Id,
            { CallbackQuery: { } }      => update.CallbackQuery.From.Id,
            { ChannelPost: { } }        => update.ChannelPost.From?.Id,
            { EditedChannelPost: { } }  => update.EditedChannelPost.From?.Id,
            { ShippingQuery: { } }      => update.ShippingQuery.From.Id,
            { PreCheckoutQuery: { } }   => update.PreCheckoutQuery.From.Id,
            { PollAnswer: { } }         => update.PollAnswer.User.Id,
            { MyChatMember: { } }       => update.MyChatMember.From.Id,
            { ChatMember: { } }         => update.ChatMember.From.Id,
            { ChatJoinRequest: { } }    => update.ChatJoinRequest.From.Id,
            _                           => null
        };
}