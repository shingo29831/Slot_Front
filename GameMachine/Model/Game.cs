using System;
using System.Diagnostics;
using System.Security.Permissions;
using static Constants;


//このクラスはゲームの内部処理を担当する
namespace Model;
public class Game 
    //: BusinessLogic
{
    static bool leftReelbtn = false;
    static bool centerReelbtn = false;
    static bool rightReelbtn = false;

    static int bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static int[] dispSymbol = new int[3];
    static int stopReelCount = 0;

    public static int[,] reachRows = { { 1,4}, { 2,5} ,{0,0 } };
    //new int[3, 2];

    static int[] leftReel = { NONE, NONE, NONE };
    static int[] centerReel = { NONE, NONE, NONE };
    static int[] rightReel = { NONE, NONE, NONE };

    const int TOP = 0;
    const int MIDDLE = 1;
    const int BOTTOM = 2;

    private static int nowLeftReel = 0;
    private static int nowCenterReel = 0;
    private static int nowRightReel = 0;


    const int LEFT = 1;
    const int CENTER = 2;
    const int RIGHT = 3;


    //リールの現在の位置をオーバフローさせないように計算する 第一引数に移動前,第二引数に移動数を代入
    public static int CalcReelPosition(int reelPosition,int move)
    {
        reelPosition += move;
        if (reelPosition < 0)
        {
            reelPosition += 21;
        }else if (reelPosition > 20) 
        {
            reelPosition -= 21;
        }

        return reelPosition;
    }


    //リールを一つずつ移動させる　移動先が決定されている場合はリールのポジションを代入なければ NONE:-1 を代入
    public static void UpReelPosition(int selectReel,int destinationPosition)
    {
        switch (selectReel)
        {
            case LEFT:
                if(nowLeftReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if(nowLeftReel != destinationPosition)
                {
                    nowLeftReel++;
                }
 
                if (nowLeftReel > 20)
                {
                    nowLeftReel = 0;
                }
                break;

            case CENTER:
                if (nowCenterReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowCenterReel != destinationPosition)
                {
                    nowCenterReel++;
                }
                
                if (nowCenterReel > 20)
                {
                    nowCenterReel = 0;
                }
                break;

            case RIGHT:
                if (nowRightReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowRightReel != destinationPosition)
                {
                    nowRightReel++;
                }
                
                if (nowRightReel > 20)
                {
                    nowRightReel = 0;
                }
                break;
        }
    }


    //リールの今のポジションを取得する　引数に定数クラスのLEFT,CENTER,RIGHT
    public static int GetNowReelPosition(int selectReel)
    {
        int nowPosition = NONE;
        switch(selectReel)
        {
            case LEFT:
                nowPosition = nowLeftReel;
                break;

            case CENTER:
                nowPosition = nowCenterReel;
                break;

            case RIGHT:
                nowPosition = nowRightReel;
                break;
        }
        return nowPosition;
    }


    //表示するリールの位置を取得する　引数に定数クラスのLEFT,CENTER,RIGHTとTOP,MIDDLE,BOTTOM
    public static int GetDispSymbol(int selectReel,int dispPosition)
    {
        int reelPosition = NONE;
        switch (selectReel)
        {
            case LEFT:
                reelPosition = nowLeftReel + dispPosition;
                break;

            case CENTER:
                reelPosition = nowCenterReel + dispPosition;
                break;

            case RIGHT:
                reelPosition = nowRightReel + dispPosition;
                break;
        }

        if(reelPosition > 20)
        {
            reelPosition -= 21;
        }
        return reelPosition;
    }

    //ボーナス抽選開始後実行する"ボーナス抽選関数" bonusProbabilityに設定された確率に合わせて抽選する
    public static bool BonusLottery()
    {
        Random rnd = new Random();
        int rndnum = rnd.Next(1, 101);  //1以上101未満の値がランダムに出力
        if (rndnum <= Setting.getBonusProbability())
        {
            return true;
        }
        return false;
    }


    //ボーナス抽選当選後実行するレギュラーボーナスまたはビックボーナスを決定する
    public static int SelectBonusLottery()
    {
        int bonus = 0;
        Random rnd = new Random();
        int regularProbabilityWeight = Setting.getBonusesProbabilityWeight(Bonus.REGULAR);
        int bigProbabilityWeight = Setting.getBonusesProbabilityWeight(Bonus.BIG);
        int sumWeight = regularProbabilityWeight + bigProbabilityWeight;
        int rndnum = rnd.Next(1, sumWeight+1);  //1以上sumWeight以下の値がランダムに出力

        if(regularProbabilityWeight <= rndnum)
        {
            bonus = State.REGULAR;
        }else if(regularProbabilityWeight > rndnum && sumWeight <= rndnum) 
        {
            bonus = State.BIG;
        }

        return bonus;
    }


    //役の抽選の関数　ボーナス以外の役が当選する
    public static int HitRoleLottery()
    {

        int role = 0;
        int sumWeight = 0;
        int lotteryRange = 6;


        for (int i = 0; i <= lotteryRange; i++)
        {
            sumWeight += Setting.getRoleWeight(i);
        }

        Random rnd = new Random();
        int rndnum = rnd.Next(1, sumWeight + 1);
        sumWeight = 0;
        int tmp = 0;
        for (int i = 0; i <= lotteryRange; i++)
        {
            tmp = sumWeight;
            sumWeight += Setting.getRoleWeight(i);


            if (tmp < rndnum && rndnum <= sumWeight)
            {
                role = i;
            }
        }


        return role;
    }


    //リールのシンボルの並びをLEFT,CENTER,RIGHTで選択し取得する
    public static int[] GetReelOrder(int selectReel) 
    {
        int[] reelOrder = {NONE };
        switch (selectReel)
        {
            case LEFT:
                reelOrder = ReelOrder.leftReelOrder;
                break;


            case CENTER:
                reelOrder = ReelOrder.centerReelOrder;
                break;


            case RIGHT:
                reelOrder = ReelOrder.rightReelOrder;
                break;
            
        }
        return reelOrder;
    }

    
    //リールで表示されるシンボルの選択をする関数
    //第1引数には処理するリールの位置を定数クラスのLEFT・CENTER・RIGHTで選択
    //第2引数には決まった役（ロール）
    //処理内容にはリーチの行の把握とレギュラーボーナス時にはBARと7がどこにあるか把握する必要もあり
    //一つ目のリールの処理
    public static int GetFarstReelPosition(int selectReel,int role)
    {

        int nowReelPosition = GetNowReelPosition(selectReel);
        int reelPosition = NONE;
        int[] reelOrder = GetReelOrder(selectReel); //選択されたリールのシンボル配列を参照渡しする
        int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE }; //roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE }; //roleを達成できるシンボルの位置を格納する 要素番号１はREGとBIGで7とBARで使用
        int searchReelPosition = nowReelPosition;


        Random rnd = new Random();


        //現在のリールのポジションで下から上に7つを候補として代入
        for (int i = 0; i < symbolCandidate.Length; i++)
        {
            symbolCandidate[i] = searchReelPosition;
            searchReelPosition = CalcReelPosition(searchReelPosition, 1);
        }



        //役を成立させるシンボルを代入、4以下(ボーナス以外)のロールはそのままシンボルとして代入、強・最強チェリーはCHERRYを代入REGとBIGは7と第二候補としてBARを代入
        if (role <= 4)
        {
            symbolsAccordingRole[0] = role;
        }
        else if (role <= Role.OTHER_BONUS)
        {
            symbolsAccordingRole[0] = Symbol.CHERRY;
        }
        else if (role == Role.REGULAR || role == Role.BIG)
        {
            symbolsAccordingRole[0] = Symbol.SEVEN;
            symbolsAccordingRole[1] = Symbol.BAR;
        }



        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for (int j = symbolCandidate.Length - 1; j >= 0; j--) //候補となるシンボルの数実行、現在位置から遠い順
        {
            if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0]) //止まり先候補のシンボルとロールを成立させるシンボルを比較
            {
                stopCandidate[0] = reelOrder[symbolCandidate[j]]; //停止候補に代入
                reelPosition = symbolCandidate[j];
            }
            else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
            {
                stopCandidate[1] = symbolCandidate[j];
            }
        }



        if (reelPosition == NONE && stopCandidate[1] != NONE) //リールの第一停止候補がなかった場合に第二候補を代入する
        {
            reelPosition = stopCandidate[1];
        }
        else if (reelPosition == NONE) //第一,第二停止候補がなかった場合、現在の位置を代入
        {
            reelPosition = nowReelPosition;
        }
        if (reelPosition == CalcReelPosition(nowReelPosition, 6)) //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す
        {
            reelPosition = CalcReelPosition(reelPosition, -2);
        }
        else if (reelPosition != nowReelPosition) //基本的に選ばれたシンボルが選択時下に来るため真ん中にくるように一つ戻す
        {
            reelPosition = CalcReelPosition(reelPosition, -1);
        }

        return reelPosition;
    }


    //あとで編集
    //どこのリールであるか、一つ目のリールのシンボルの並びと場所の引数が必要
    //一つ目のリールの情報を元に
    public static int GetSecondReelPosition(int selectFirstReel,int selectSecondReel, int role)
    {

        int nowFirstReelPosition = GetNowReelPosition(selectFirstReel);
        int nowSecondReelPosition = GetNowReelPosition(selectSecondReel);
        int reelPosition = NONE;
        int[] firstReelOrder = GetReelOrder(selectFirstReel); //選択されたリールのシンボル配列を参照渡しする
        int[] firstReelSymbolsOrder;
        int[] secondReelOrder = GetReelOrder(selectSecondReel);
        int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE };//ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE }; //roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE }; //roleを達成できるシンボルの位置を格納する 要素番号１はREGとBIGで7とBARで使用
        int searchReelPosition = nowFirstReelPosition;


        Random rnd = new Random();


        //現在のリールのポジションで下から上に7つを候補として代入
        for (int i = 0; i < symbolCandidate.Length; i++)
        {
            symbolCandidate[i] = searchReelPosition;
            searchReelPosition = CalcReelPosition(searchReelPosition, 1);
        }



        //役を成立させるシンボルを代入、4以下のロールはそのままシンボルとして代入、REGとBIGは7と第二候補としてBARを代入
        if (role <= Role.OTHER_BONUS)
        {
            symbolsAccordingRole[0] = role;
        }
        else if (role <= Role.OTHER_BONUS)
        {
            symbolsAccordingRole[0] = Symbol.CHERRY;
        }
        else if (role == Role.REGULAR || role == Role.BIG)
        {
            symbolsAccordingRole[0] = Symbol.SEVEN;
            symbolsAccordingRole[1] = Symbol.BAR;
        }



        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for (int j = symbolCandidate.Length - 1; j >= 0; j--) //候補となるシンボルの数実行、現在位置から遠い順
        {
            if (secondReelOrder[symbolCandidate[j]] == symbolsAccordingRole[0]) //止まり先候補のシンボルとロールを成立させるシンボルを比較
            {
                stopCandidate[0] = secondReelOrder[symbolCandidate[j]]; //停止候補に代入
                reelPosition = symbolCandidate[j];
            }
            else if (secondReelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
            {
                stopCandidate[1] = symbolCandidate[j];
            }
        }



        if (reelPosition == NONE && stopCandidate[1] != NONE) //リールの第一停止候補がなかった場合に第二候補を代入する
        {
            reelPosition = stopCandidate[1];
        }
        else if (reelPosition == NONE) //第一,第二停止候補がなかった場合、現在の位置を代入
        {
            reelPosition = nowFirstReelPosition;
        }
        if (reelPosition == CalcReelPosition(nowFirstReelPosition, 6)) //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す
        {
            reelPosition = CalcReelPosition(reelPosition, -2);
        }
        else if (reelPosition != nowFirstReelPosition) //基本的に選ばれたシンボルが選択時下に来るため真ん中にくるように一つ戻す
        {
            reelPosition = CalcReelPosition(reelPosition, -1);
        }

        return reelPosition;
    }






    //上手く動かん変更する
    //止まったリールからリーチの場所を探し二次元配列で出力する、止まっていないリールはNONE:-1が入る
    //リーチを探す処理で行が場所を示し、列に複数のシンボルのリーチが書かれている
    public static void GetReachRows(int leftPosition, int centerPosition, int rightPosition) 
    {
        

        


        for (int i = 0; i < 3; i++) //BOTTOM:0～TOP:2までのシンボルを取得
        {
            leftReel[i] = Game.GetDispSymbol(SelectReel.LEFT, leftPosition + 1);
        }
        for (int i = 0; i < 3; i++)
        {
            centerReel[i] = Game.GetDispSymbol(SelectReel.CENTER, centerPosition + 1);
        }
        for (int i = 0; i < 3; i++)
        {
            rightReel[i] = Game.GetDispSymbol(SelectReel.RIGHT, rightPosition + 1);
        }


        // 左リールの処理
        for (int row = Position.BOTTOM; row <= Position.TOP; row++) //BOTTOM:0～TOP:2までの処理
        {
            if (leftPosition == NONE && centerReel[row] == rightReel[row])
            {
                reachRows[row, 0] = centerReel[row];
            }
        }

        if (leftPosition == NONE && centerReel[Position.MIDDLE] == rightReel[Position.BOTTOM])
        {
            reachRows[Position.TOP, 1] = centerReel[Position.MIDDLE];
        }

        if (leftPosition == NONE && centerReel[Position.MIDDLE] == rightReel[Position.TOP])
        {
            reachRows[Position.BOTTOM, 1] = centerReel[Position.MIDDLE];
        }




        //真ん中リールの処理
        for (int row = 0; row < 3; row++)
        {
            if (centerPosition == NONE && leftReel[row] == rightReel[row])
            {
                reachRows[row, 0] = leftReel[row];
            }
        }

        for (int row = 0; row < 3; row++)
        {
            if (centerPosition == NONE && leftReel[row] == rightReel[2 - row])
            {
                reachRows[row, 1] = leftReel[row];
            }
        }

        //右リールの処理
        for (int row = 0; row < 3; row++)
        {
            if (rightPosition == NONE && centerReel[row] == leftReel[row])
            {
                reachRows[row, 0] = centerReel[row];
            }
        }

        if (rightPosition == NONE && centerReel[Position.MIDDLE] == leftReel[Position.BOTTOM])
        {
            reachRows[Position.TOP, 1] = centerReel[Position.MIDDLE];
        }
        else if (rightPosition == NONE && centerReel[Position.MIDDLE] == leftReel[Position.TOP])
        {
            reachRows[Position.BOTTOM, 1] = centerReel[Position.MIDDLE];
        }

    }







    public static int GetReachRow(int selectReel, int selectPosition)
    {
        int[,] nowDispSymbols = { { NONE, NONE, NONE }, { NONE, NONE, NONE }, { NONE, NONE, NONE } };

        for (int reel = 1; reel <= 3; reel++)
        {
            for (int row = 0; row < 3; row++)
            {
                if(reel != selectReel)
                {
                    nowDispSymbols[reel, row] = Game.GetDispSymbol(reel, row);
                }
            }
        }

        if (selectReel == SelectReel.LEFT)
        {

        }



        return 0;
    }







    //あとで変更使うかわからない
    public static int[] DispSymbolLottery()
    {

        if(stopReelCount == 3)
        {

            stopReelCount = 0;
        }
        
        return dispSymbol;
    }

    private static void StopReelCountup() { stopReelCount++; }
}
