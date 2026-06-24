using ChatBot.Builders;
using ChatBot.Constants;
using ChatBot.Renderers.Extensions;
using Services.Contracts.Enums;
using Services.Contracts.Models;
using Telegram.Bot.Types.ReplyMarkups;
using UseCases.Contracts.OutputPorts.Cache;

namespace ChatBot.Factories;

public class HeroStatsKeyboardFactory(IHeroStatsCache cache)
{
    public InlineKeyboardMarkup Create(UserPreferences prefs)
    {
        var totalPages = (cache.HeroCount - 1) / ValueConstants.HeroesPerPage;
        var pageIndex = Math.Min(prefs.PageNumber, totalPages);
        var options = prefs.ProcessingOptions;

        string ToPage(int page) => $"{CallbackData.Prefixes.ToPage}:{page}:{totalPages}";

        var builder = new InlineKeyboardBuilder();

        // 1. Строка навигации (без стилей)
        builder.AddRow(
            ("◀◀", ToPage(0)),
            ("◀", ToPage(pageIndex - 1)),
            ($"{pageIndex + 1}/{totalPages + 1}", CallbackData.Actions.Noop),
            ("▶", ToPage(pageIndex + 1)),
            ("▶▶", ToPage(totalPages))
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
        var switchFormatButtonText = $"{(prefs.IsImageFormat ? "✅" : "")} Режим изображения (медленный)";
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