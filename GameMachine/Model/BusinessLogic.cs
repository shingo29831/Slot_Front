using System;

public class BusinessLogic
{
    //シンボルの種類
    public const int NONE = -1;
    public const int BELL = 1;
    public const int REPLAY = 2;
    public const int WATERMELON = 3;
    public const int CHERRY = 4;
    public const int SEVEN = 5;
    public const int BAR = 6;


    //役の種類
    public const int NONE_ROLE = 0;
    public const int BELL_ROLE = 1;
    public const int REPLAY_ROLE = 2;
    public const int WATERMELON_ROLE = 3;
    public const int CHERRY_ROLE = 4;
    public const int REACH_ROLE = 5;
    public const int REGULAR_ROLE = 6;
    public const int BIG_ROLE = 7;

    //ボーナスに当選後REGかBIGの抽選時に使用
    public const int REGULAR_HIT_STATE = 1;
    public const int BIG_HIT_STATE = 2;

    //ボーナスに入った時に使用
    public const int REGULAR_BONUS = 1;
    public const int BIG_BONUS = 2;


    //リールの列
    public const int LEFT_REEL = 1;
    public const int CENTER_REEL = 2;
    public const int RIGHT_REEL = 3;

    //リールのシンボルの並び0～20まで
    public static readonly int[] leftReelOrder = { BELL, REPLAY, BELL, BAR, CHERRY, BELL, REPLAY, BELL, WATERMELON, SEVEN, BELL, REPLAY, BELL, CHERRY, BAR, BELL, REPLAY, BELL, REPLAY, SEVEN, BELL };
    public static readonly int[] centerReelOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };
    public static readonly int[] rightReelOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };
    public BusinessLogic()
	{

	}
}
 