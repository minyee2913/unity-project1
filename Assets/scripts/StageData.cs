using UnityEngine;

public class StageData : MonoBehaviour
{
    public Stage[] stages = new Stage[6];

    private void Start()
    {
        {
            int[] types = { 0 };
            stages[0] = new Stage(
                1,
                "Prepare for Battle",
                0,
                3,
                8,
                types
            );
        }
        {
            int[] types = { 0, 1 };
            stages[1] = new Stage(
                2,
                "minigame",
                0,
                2.5f,
                10,
                types
            );
        }
        {
            int[] types = { 0, 1 };
            stages[2] = new Stage(
                3,
                "journey",
                0,
                2,
                12,
                types
            );
        }
        {
            int[] types = { 0, 1, 2 };
            stages[3] = new Stage(
                4,
                "icyCave",
                0,
                1.5f,
                14,
                types
            );
        }
        {
            int[] types = { 0, 1, 2 };
            stages[4] = new Stage(
                5,
                "unknown",
                0,
                1f,
                16,
                types
            );
        }
        {
            int[] types = { 10 };
            stages[5] = new Stage(
                6,
                "boss",
                1,
                1f,
                20,
                types
            );
        }
    }
}
