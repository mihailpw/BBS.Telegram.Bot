using BBS.Telegram.Bot.Form;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm;

public class UserInfoBag : IFormBag
{
    public string? Name { get; set; }
    public bool? GenderMale { get; set; }
    public string? GenderOther { get; set; }
}