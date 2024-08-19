using System;

public class BusinessLogic
{
    public const int BELL = 0;
    public const int REPLAY = 1;
    public const int WATERMELON = 2;
    public const int CHERRY = 3;
    public const int SEVEN = 4;
    public const int BAR = 5;

    public static int[] leftLaneOrder = { BELL, REPLAY, BELL, BAR, CHERRY, BELL, REPLAY, BELL, WATERMELON, SEVEN, BELL, REPLAY, BELL, CHERRY, BAR, BELL, REPLAY, BELL, REPLAY, SEVEN, BELL };
    public static int[] centerLaneOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };
    public static int[] rightLaneOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };
    public BusinessLogic()
	{

	}
}
