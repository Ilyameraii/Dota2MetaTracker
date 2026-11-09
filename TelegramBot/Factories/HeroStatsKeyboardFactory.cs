using TelegramBot.Constants;
using TelegramBot.Models;
using TelegramBot.Renderers.Extensions;
using Entities.Enums;
using Entities.Models;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Builders;

namespace TelegramBot.Factories;

public class HeroStatsKeyboardFactory
{
    public InlineKeyboardMarkup Create(HeroMetaPage page)
    {
        var options = page.Preferences.ProcessingOptions;
        string ToPage(int pageNumber) => $"{CallbackData.Prefixes.ToPage}{pageNumber}";

        var builder = new InlineKeyboardBuilder();

        // 1. Строка навигации (без стилей)
        builder.AddRow(
            ("◀◀", ToPage(0)),
            ("◀", ToPage(Math.Max(0, page.PageIndex - 1))),
            ($"{page.PageIndex + 1}/{page.TotalPages + 1}", CallbackData.Actions.Noop),
            ("▶", ToPage(Math.Min(page.TotalPages, page.PageIndex + 1))),
            ("▶▶", ToPage(page.TotalPages))
        );

        // 2. Строки рангов и ролей (смешанные стили в одной строке)
        foreach (var p in RankRolePairs)
        {
            builder.AddRow(row => row
                .AddButton(
                    p.RankFlag.ToDisplayName(options.Ranks),
                    CallbackData.Prefixes.Rank + p.RankFlag,
                    KeyboardButtonStyle.Danger)
                .AddButton(
                    p.RoleFlag.ToDisplayName(options.Roles),
                    CallbackData.Prefixes.Role + p.RoleFlag,
                    KeyboardButtonStyle.Success)
            );
        }

        // 3. Кнопки сортировки (чанками по 2, стиль Primary для всей строки)
        var sortButtons = Enum.GetValues<SortType>()
            .Select(sortType => (
                Text: sortType.ToDisplayName(options.SortBy, options.IsDescending),
                Callback: CallbackData.Prefixes.Sort + sortType
            ))
            .Chunk(2);

        foreach (var chunk in sortButtons)
        {
            builder.AddRow(chunk.ToArray());
            builder.WithStyle(KeyboardButtonStyle.Primary);
        }

        // 4. Кнопка сброса (стиль Danger)
        builder.AddButton("Сбросить", CallbackData.Actions.ClearOptions, KeyboardButtonStyle.Danger);

        // 5. Кнопка переключения формата (без стиля)
        var switchFormatButtonText = $"{(page.Preferences.IsImageFormat ? "✅" : "")} Режим изображения (медленный)";
        builder.AddButton(switchFormatButtonText, CallbackData.Actions.SwitchFormat);

        return builder.Build();
    }

    private static readonly ( RankFlags RankFlag, RoleFlags RoleFlag)[] RankRolePairs =
    [
        (RankFlags.HeraldGuardian, RoleFlags.Safelane),
        (RankFlags.CrusaderArchon, RoleFlags.Midlane),
        (RankFlags.LegendAncient, RoleFlags.Offlane),
        (RankFlags.DivineImmortal, RoleFlags.Support),
        (RankFlags.Uncalibrated, RoleFlags.HardSupport),
    ];
}