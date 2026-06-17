namespace ChatBot.Constants;

/// <summary>
/// Константы callback-данных для inline-кнопок Telegram-бота.
/// </summary>
public static class CallbackData
{
    /// <summary>
    /// Префиксы callback-данных, влияющих на отображаемый список:
    /// переход по страницам, выбор ранга и роли, сортировка.
    /// </summary>
    public static class Prefixes
    {
        /// <summary>
        /// Префикс выбора фильтра по рангу.
        /// </summary>
        public const string Rank = "rank:";

        /// <summary>
        /// Префикс выбора фильтра по роли.
        /// </summary>
        public const string Role = "role:";

        /// <summary>
        /// Префикс выбора способа сортировки списка.
        /// </summary>
        public const string Sort = "sort:";

        /// <summary>
        /// Префикс перехода на указанную страницу списка.
        /// </summary>
        public const string ToPage = "to_page:";
    }

    /// <summary>
    /// Значения callback-данных для отдельных действий, не являющихся префиксами.
    /// </summary>
    public static class Actions
    {
        /// <summary>
        /// Пустое действие: нажатие кнопки не должно изменять состояние
        /// (например, кнопка-индикатор без логики).
        /// </summary>
        public const string Noop = "noop";

        /// <summary>
        /// Сброс всех выбранных фильтров (ранг, роль, сортировка).
        /// </summary>
        public const string ClearOptions = "clear_options";

        /// <summary>
        /// Переключение формата отображения меты (png-изображение и текст).
        /// </summary>
        public const string SwitchFormat = "switch_format";
    }
}