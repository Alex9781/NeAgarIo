using UnityEngine;

/// <summary>
///  ласс дл€ удобного хранени€ констант и быстрого доступа к ним.
/// </summary>
public class GameGlobalSettings : MonoBehaviour
{
    /// <summary>
    /// –азмер игрового пол€, за рамками которого не будет по€вл€тьс€ еда и не могут находитьс€ игроки.
    /// </summary>
    public static Vector2 GameField = new Vector2(100, 100);
}
