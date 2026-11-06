using System.Runtime.Serialization;

namespace Entities.Enums;

public enum Role
{
    /// <summary>
    ///     Легкая линия / Керри /  1 позиция
    /// </summary>
    Safelane,

    /// <summary>
    ///     Центральная линия / Мидер / 2 позиция
    /// </summary>
    Midlane,

    /// <summary>
    ///     Сложная линия / Оффлейнер / 3 позиция
    /// </summary>
    Offlane,

    /// <summary>
    ///     Частичная поддержка / Саппорт / Четвертая позиция (на линии с оффлейнером)
    /// </summary>
    Support,

    /// <summary>
    ///     Полная поддержка / Фулсаппорт / Пятая позиция (на линии с керри)
    /// </summary>
    HardSupport
}