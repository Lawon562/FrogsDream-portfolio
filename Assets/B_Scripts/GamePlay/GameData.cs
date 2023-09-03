
public class GameData
{
    // Game Option Data
    public static float bgmVolume = 1f;
    public static float sfxVolume = 1f;

    // Game Play Data
    public static int stage = 0;
    public static int maxEnemy = stage * 3 + 20;
    public static bool isPauseState;
    public static int maxBallCount = 10 + (stage - 1) * 5;
    public static float playTime;

    public static void InitData()
    {
        bgmVolume = 1f;
        sfxVolume = 1f;

        stage = 1;
        maxEnemy = stage * 3 + 20;
        isPauseState = false;
        maxBallCount = 10 + (stage - 1) * 10;
    }
}
