public class Stage
{
    public int stage;
    public string stageMusic;
    public int stageTheme;
    public float activeDelay;
    public int leastBlock;
    public int[] activeTypes;
    public Stage(int _stage, string _stageMusic, int _stageTheme, float _activeDelay, int _leastBlock, int[] _activeTypes) {
        stage = _stage;
        stageMusic = _stageMusic;
        stageTheme = _stageTheme;
        activeDelay = _activeDelay;
        leastBlock = _leastBlock;
        activeTypes = _activeTypes;
    }
}
