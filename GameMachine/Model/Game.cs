using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using static Constants;
using static Model.Setting;


//このクラスはゲームの内部処理を担当する
namespace Model;



public class Game
{
    static bool leftReelbtn = false;
    static bool centerReelbtn = false;
    static bool rightReelbtn = false;

    static int bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static int[] dispSymbol = new int[3];
    static int stopReelCount = 2; //テストで2


    static Symbols[] leftReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] centerReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] rightReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };


    public static int nowLeftReel = 0;
    public static int nowCenterReel = 0;
    public static int nowRightReel = 0;

    public static bool leftReelMoving = true;
    public static bool centerReelMoving = false; //テストでfalse
    public static bool rightReelMoving = false; //テストでfalse


    public static Roles nowRole = Roles.NONE; //テスト前はNONE
    public static Symbols symbolsAccordingRole = Symbols.NONE;

    //reachRowsはリーチとなる場所を3次元配列で格納する各リールのポジション(上・中・下)に二つまでの入ったら役が成立するシンボルを代入する
    public Symbols[,,] reachPositions = { { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //左リール : 0
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //中央リール : 1
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } } }; //右リール : 2

    public static readonly int NOT = -1;


    //リールの現在の位置をオーバフローさせないように計算する 第一引数に移動前,第二引数に移動数を代入
    public static int CalcReelPosition(int reelPosition,int move)
    {
        reelPosition += move;
        if (reelPosition < 0)
        {
            reelPosition += 21;
        }
        else if (reelPosition > 20) 
        {
            reelPosition %= 21;
        }

        return reelPosition;
    }


    //リールを一つずつ移動させる　移動先が決定されている場合はリールのポジションを代入なければ NONE:-1 を代入
    public static void UpReelPosition(Reels selectReel,int destinationPosition)
    {
        switch (selectReel)
        {
            case Reels.LEFT:
                if(nowLeftReel != destinationPosition || destinationPosition == NONE)
                {
                    nowLeftReel = CalcReelPosition (nowLeftReel, 1);
                }

                break;

            case Reels.CENTER:
                if (nowCenterReel != destinationPosition || destinationPosition ==NONE)
                {
                    nowCenterReel = CalcReelPosition (nowCenterReel, 1);
                }
                
                break;

            case Reels.RIGHT:
                if (nowRightReel != destinationPosition || destinationPosition == NONE)
                {
                    nowRightReel = CalcReelPosition (nowRightReel, 1);
                }
                
                break;
        }
    }


    //リールの今のポジションを取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHT
    public static int GetNowReelPosition(Reels selectReel)
    {
        int nowPosition = NONE;
        switch(selectReel)
        {
            case Reels.LEFT:
                nowPosition = nowLeftReel;
                break;

            case Reels.CENTER:
                nowPosition = nowCenterReel;
                break;

            case Reels.RIGHT:
                nowPosition = nowRightReel;
                break;
        }
        return nowPosition;
    }


    //表示するリールの位置を取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHTとTOP,MIDDLE,BOTTOM
    public static int GetDispSymbol(Reels selectReel,Positions position)
    {
        int reelPosition = NONE;
        int nowReelPosition = GetNowReelPosition (selectReel);
        switch (position)
        {
            case Positions.TOP:
                reelPosition = CalcReelPosition(nowReelPosition,2);
                break;

            case Positions.MIDDLE:
                reelPosition = CalcReelPosition(nowReelPosition, 1);
                break;

            case Positions.BOTTOM:
                reelPosition = nowReelPosition;
                break;
        }

        return reelPosition;
    }


    //選択されたリールに表示されている全てのシンボルのビットフラグを取得する
    public static Symbols GetNowReelSymbols(Reels selectReel)
    {
        Symbols nowReelSymbols = Symbols.NONE;
        Symbols[] reelOrder = GetReelOrder(selectReel);

        nowReelSymbols = reelOrder[GetDispSymbol(selectReel,Positions.TOP)];
        nowReelSymbols |= reelOrder[GetDispSymbol(selectReel, Positions.MIDDLE)];
        nowReelSymbols |= reelOrder[GetDispSymbol(selectReel, Positions.BOTTOM)];

        return nowReelSymbols;
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
    public static Roles SelectBonusLottery()
    {
        Roles bonus = Roles.NONE;
        Random rnd = new Random();
        int regularProbabilityWeight = Setting.getBonusesProbabilityWeight(0);
        int bigProbabilityWeight = Setting.getBonusesProbabilityWeight(1);
        int sumWeight = regularProbabilityWeight + bigProbabilityWeight;
        int rndnum = rnd.Next(1, sumWeight+1);  //1以上sumWeight以下の値がランダムに出力

        if(regularProbabilityWeight <= rndnum)
        {
            bonus = Roles.REGULAR;
        }else if(regularProbabilityWeight > rndnum && sumWeight <= rndnum) 
        {
            bonus = Roles.BIG;
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


    //リールのシンボルの並びをReels.LEFT,Reels.CENTER,Reels.RIGHTで選択し取得する
    public static Symbols[] GetReelOrder(Reels selectReel) 
    {
        Symbols[] reelOrder = {Symbols.NONE };
        switch (selectReel)
        {
            case Reels.LEFT:
                reelOrder = ReelOrder.LEFT_REEL_ORDER;
                break;


            case Reels.CENTER:
                reelOrder = ReelOrder.CENTER_REEL_ORDER;
                break;


            case Reels.RIGHT:
                reelOrder = ReelOrder.RIGHT_REEL_ORDER;
                break;
            
        }
        return reelOrder;
    }

   


    //どのリールが動いてるか代入する
    public static void SetReelMoving(Reels selectReel, bool isMoving)
    {
        switch(selectReel)
        {
            case Reels.LEFT:
                leftReelMoving = isMoving;
                break;
            case Reels.CENTER: 
                centerReelMoving = isMoving;
                break;
            case Reels.RIGHT:
                rightReelMoving = isMoving;
                break;
        }
    }

    //リールを全て動いている判定にする
    public static void ResetReelsMoving()
    {
        SetReelMoving(Reels.LEFT,true);
        SetReelMoving(Reels.CENTER,true);
        SetReelMoving(Reels.RIGHT,true);
    }



    //止める位置
    //編集中
    public static int GetStopReelPosition(Reels selectReel)
    {
        int stopReelPosition = GetNowReelPosition(selectReel);

        switch (stopReelCount)
        {
            case 0:
                stopReelPosition = GetFirstReelPosition(selectReel);
                break;
            case 1:
                stopReelPosition = GetSecondReelPosition(selectReel);
                break;
            case 2:
                stopReelPosition = GetThirdReelPostion(selectReel);
                break;
        }


        return stopReelPosition;
    }

    //一つ目にストップさせるリールの処理
    public static int GetFirstReelPosition(Reels selectReel)
    {
        int reelPosition = NONE;


        Positions candidateStopPositions = Positions.TOP | Positions.MIDDLE | Positions.BOTTOM;
        

        reelPosition = GetReelPositionForCandidatePositions(selectReel, candidateStopPositions);

        return reelPosition;
    }

    //二つ目にストップさせるリールの処理
    public static int GetSecondReelPosition(Reels selectReel)
    {
        int reelPosition = NONE;
        int candidateReelPosition = NONE;
        int nowReelPosition = GetNowReelPosition(selectReel);

        Positions candidateStopPositions = GetPositionToReach(selectReel); //役を成立させるリーチがあった時にシンボルを止める場所を代入する

        if (candidateStopPositions == Positions.NONE)
        {
            candidateStopPositions = Positions.TOP | Positions.MIDDLE | Positions.BOTTOM;
        }

        reelPosition = GetReelPositionForCandidatePositions(selectReel, candidateStopPositions);

        return reelPosition;
    }


    //三つ目にストップさせるリールの処理
    public static int GetThirdReelPostion(Reels selectReel)
    {
        int reelPosition = NONE;
        int candidateReelPosition = NONE;
        int nowReelPosition = GetNowReelPosition(selectReel);

        Positions candidateStopPositions = GetRoleReachPositions(); //役を成立させるリーチがあった時にシンボルを止める場所を代入する

        if(candidateStopPositions == Positions.NONE)
        {
            candidateStopPositions = Positions.TOP | Positions.MIDDLE | Positions.BOTTOM;
        }

        reelPosition = GetReelPositionForCandidatePositions(selectReel,candidateStopPositions);



        return reelPosition;
    }

    public static int GetReelPositionForCandidatePositions(Reels selectReel,Positions selectPositions)
    {
        int reelPosition = NONE;
        int[] candidateReelPosition = { NONE, NONE, NONE };
        Positions[] positions = { Positions.BOTTOM, Positions.MIDDLE, Positions.TOP };

        int nearestGap = NONE;
        int gap = NONE;

        sbyte element = 0;
        foreach (Positions position in positions)
        {
            
            if (selectPositions.HasFlag(position))
            {
                candidateReelPosition[element] = CalcReelPosition(GetStopCandidateForPosition(selectReel, position),element * -1);
            }
            element++;
        }

        for(element = 0; element < candidateReelPosition.Length; element++)
        {
            gap = CalcGapNowReelPosition(selectReel, candidateReelPosition[element]); //差を代入する
            if (( nearestGap == NONE || nearestGap > gap ) && gap != NONE) //差に値がある時にnearestGapよりも小さい値の時に処理する
            {
                reelPosition = candidateReelPosition[element];
                nearestGap = gap;
            }
        }


        if (reelPosition == NONE) //reelPositionがNONEだった時
        {
            reelPosition = GetReelPositionForRoleFailure(selectReel); //除外範囲ではない最も近い値をいれる
        }

        return reelPosition;
    }


    //役が成立できない時に、除外範囲ではない最も近いリールの位置を代入する
    public static int GetReelPositionForRoleFailure(Reels selectReel)
    {
        int reelPosition = GetNowReelPosition(selectReel);
        for (int gapNowReelPosition = 0; gapNowReelPosition < 5; gapNowReelPosition++) //reelPositionがNONEだった時に最も近い代入可能な位置をいれる
        {
            if (GetIsExclusion(selectReel, reelPosition) == false) //除外範囲ではない時
            {
                break;
            }
            reelPosition = CalcReelPosition(reelPosition, 1);
        }
        return reelPosition;
    }



    //現在のリールの位置からの差を返す 探索位置がNONEだった場合はNONEを返す
    public static int CalcGapNowReelPosition(Reels selectReel, int reelSearchPosition)
    {
        int gap = 0;
        int reelPosition = GetNowReelPosition(selectReel);
        while (reelPosition != reelSearchPosition && reelSearchPosition !=NONE)
        {
            reelPosition = CalcReelPosition(reelPosition, 1);
            gap++;
        }

        if(reelPosition == NONE)
        {
            gap = NONE;
        }

        return gap;
    }


    //現在の役を成立させれるシンボルかboolで返す
    //複数のシンボルが入っていても一つでも役を成立させれるならtrueを返す
    public static bool GetIsEstablishingRole(Symbols searchSymbols)
    {

        Symbols[] symbols = { Symbols.BELL, Symbols.REPLAY, Symbols.WATERMELON, Symbols.CHERRY, Symbols.BAR, Symbols.SEVEN, Symbols.REACH};

        Symbols[] seachSymbolsArray = { Symbols.NONE, Symbols.NONE, Symbols.NONE};
        foreach(Symbols symbol in symbols)
        {
            if (searchSymbols.HasFlag(symbol) && GetSymbolsAccordingRole().HasFlag(symbol)) //どのシンボルが入っているか見ていき同時に役を成立させるか判定する
            {
                return true;
            }
        }


        return false;
    }


    //役を成立させるシンボルを取得
    public static Symbols GetSymbolsAccordingRole()
    {
        switch (nowRole)
        {
            case Roles.BELL:
                return Symbols.BELL;

            case Roles.REPLAY:
                return Symbols.REPLAY;

            case Roles.WATERMELON:
                return Symbols.WATERMELON;

            case Roles.WEAK_CHERRY:
                return Symbols.CHERRY;

            case Roles.STRONG_CHERRY:
                return Symbols.CHERRY;

            case Roles.VERY_STRONG_CHERRY:
                return Symbols.CHERRY;

            case Roles.REGULAR:
                return Symbols.SEVEN | Symbols.BAR | Symbols.REACH;

            case Roles.BIG:
                return Symbols.SEVEN | Symbols.BAR;

            default:
                return Symbols.NONE;
        }

        
    }


    //選択したポジションに入れることが可能で、現在のポジションに最も近い役を成立させるシンボルの位置を取得
    //除外範囲は含まない
    //TOPを選択した場合、止める基準地点のBOTTOMではなくTOPの位置で返すため注意
    public static int GetStopCandidateForPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        int nowReelPosition = GetNowReelPosition(selectReel);
        int maxRange = 6;
        int repetitions = 7;
        int gapBottomPosition = NONE;
        switch (position)
        {
            case Positions.TOP:
                maxRange = 6;
                repetitions = 5;
                gapBottomPosition = -2;
                break;

            case Positions.MIDDLE:
                maxRange = 5;
                repetitions = 5;
                gapBottomPosition = -1;
                break;

            case Positions.BOTTOM:
                maxRange = 4;
                repetitions = 5;
                gapBottomPosition = 0;
                break;
        }
        int searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        int findedPostion = NONE;
        for(int i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (GetIsEstablishingRole(reelOrder[searchReelPosition]) && GetIsExclusion(selectReel,CalcReelPosition(searchReelPosition ,gapBottomPosition * -1)) == false) //役を成立させれるシンボル && 除外範囲ではない
            {
                 findedPostion = searchReelPosition;
            }
        }
        return findedPostion;
    }

    //選択したポジションに入れることが可能な現在のポジションに最も近いBARを取得
    public static int GetBarPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        int nowReelPosition = GetNowReelPosition(selectReel);
        int maxRange = 6;
        int repetitions = 7;
        int gapBottomPosition = 0;
        switch (position)
        {
            case Positions.TOP:
                maxRange = 6;
                repetitions = 5;
                gapBottomPosition = 2;
                break;

            case Positions.MIDDLE:
                maxRange = 5;
                repetitions = 5;
                gapBottomPosition = 1;
                break;

            case  Positions.BOTTOM:
                maxRange = 4;
                repetitions = 5;
                gapBottomPosition= 0;
                break;
        }

        int searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        int barPostion = NONE;

        for (int i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (reelOrder[searchReelPosition] == Symbols.BAR && GetIsExclusion(selectReel,CalcReelPosition(searchReelPosition,gapBottomPosition * -1))) //
            {
                barPostion = searchReelPosition;
            }
        }
        return barPostion;
    }

    //選択したリールの位置が除外か否かを返す
    //getIsExclusion(リールの選択,リールの位置)
    public static bool GetIsExclusion(Reels selectReel, int reelPosition)
    {

        Symbols[] reelOrder = GetReelOrder(selectReel);




        //以下は弱チェリーを回避する処理

        int searchPosition = reelPosition;

        for (int i = 0; i < 3 && selectReel == Reels.LEFT && nowRole == Roles.WEAK_CHERRY; i++) //
        {
            if (reelOrder[searchPosition] == Symbols.CHERRY)
            {
                return true;
            }
            searchPosition = CalcReelPosition(searchPosition, 1);
        }

        //弱チェリー回避ここまで

        //ここからシンボルの除外
        Symbols topExclusionSymbols = GetExclusionSymbolsForPosition(selectReel,Positions.TOP);
        Symbols middleExclusionSymbols = GetExclusionSymbolsForPosition(selectReel, Positions.MIDDLE);
        Symbols bottomExclusionSymbols = GetExclusionSymbolsForPosition(selectReel, Positions.BOTTOM);

        Symbols[] exclusionSymbolsForReel = { bottomExclusionSymbols, middleExclusionSymbols, topExclusionSymbols };
        Symbols[] symbols = {Symbols.BELL, Symbols.REPLAY, Symbols.WATERMELON, Symbols.CHERRY, Symbols.BAR, Symbols.SEVEN};

        int cnt= 0;
        foreach(Symbols exclusionSymbols in exclusionSymbolsForReel) //除外するシンボルをBOTTOM～TOPの順で代入
        {
            foreach (Symbols symbol in symbols)
            {
                if(exclusionSymbols.HasFlag(symbol) && reelOrder[CalcReelPosition(reelPosition,cnt)] == symbol) //除外するシンボルで判定しているか && 除外するシンボルであるか 
                {
                    return true;
                }
            }
            cnt++; //ポジションを一つずつ上げる
        }   

        return false;
    }

    //除外するシンボルをビットフラグで返す　positionにはTOP,MIDDLE,BOTTOMが入る
    public static Symbols GetExclusionSymbolsForPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        Symbols exclusionSymbols = Symbols.NONE;

        Symbols reachSymbols = GetReachSymbolsForPosition(position);

        switch (nowRole)
        {
            case Roles.BELL:
                exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BELL); //C#の仕様が分からないのでややこしいがリーチのビットフラグからベルのフラグを消している
                break;


            case Roles.REPLAY:
                exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.REPLAY);
                break;


            case Roles.WATERMELON:
                exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.WATERMELON);
                break;


            case Roles.WEAK_CHERRY:
                if (selectReel == Reels.LEFT) //弱チェリーが当選した時、左リールでは除外用ビットフラグからチェリーを消す
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.CHERRY);
                }
                if(selectReel == Reels.CENTER || selectReel == Reels.RIGHT) //弱チェリーが当選した時、中・右リールではリーチになった全てのシンボルをビットフラグに入れる
                {
                    exclusionSymbols = reachSymbols;
                }
                break;


            case Roles.STRONG_CHERRY:
                if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT) && (position == Positions.TOP || position == Positions.BOTTOM)) //左・右リールで上・下段の時、除外用ビットフラグのチェリーを下げる
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.CHERRY);
                }
                if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT ) && position == Positions.MIDDLE) //強チェリーが当選した時、左または右リールの中段にチェリーが来ないようにする
                {
                    exclusionSymbols = reachSymbols;
                }
                if(selectReel == Reels.CENTER) //中央リールでは、どこにチェリーが来てもよい
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.CHERRY);
                }
                break;


            case Roles.VERY_STRONG_CHERRY:
                exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.CHERRY); //配置的に強チェリーになっても良いためどこにチェリーがきても良い
                break;


            case Roles.REGULAR:
                if (reachSymbols.HasFlag(Symbols.BAR))
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.SEVEN) ; //除外用ビットフラグからSEVENフラグを消す BARを揃えるとBBになるため注意
                }
                if (reachSymbols.HasFlag(Symbols.SEVEN))
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BAR); 
                }
                if (reachSymbols.HasFlag(Symbols.REACH))
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BAR | Symbols.SEVEN) ;
                }
                break;


            case Roles.BIG:
                if (reachSymbols.HasFlag(Symbols.BAR))
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BAR);
                }
                if (reachSymbols.HasFlag(Symbols.SEVEN))
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.SEVEN);
                }

                Symbols[] centerReelOrder = GetReelOrder(Reels.CENTER);
                Symbols centerSymbols = GetNowReelSymbols(Reels.CENTER);

                if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時でに中央リールのシンボルを選択する時は除外用ビットフラグからBARを消す
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BAR);
                }

                if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.SEVEN) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールに7が来ていた時
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & Symbols.BAR);
                }

                if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.BAR) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールにBARが来ていた時
                {
                    exclusionSymbols = reachSymbols ^ (reachSymbols & (Symbols.BAR | Symbols.SEVEN)); //7とBARを除外用ビットフラグから消す
                }
                break;
        }


        return exclusionSymbols;
    }


    //当選した役をリーチにさせる、選択したリールのPositionsをビットフラグで返す
    //ない場合とリールが2つ以上止まっている時はNONEで返す
    public static Positions GetPositionToReach(Reels selectReel)
    {
        Reels stopReel = Reels.NONE;
        if (leftReelMoving)
        {
            stopReel = Reels.LEFT;
        }

        if (centerReelMoving)
        {
            stopReel |= Reels.CENTER;
        }

        if (rightReelMoving)
        {
            stopReel |= Reels.RIGHT;
        }

        switch (stopReel)
        {
            case Reels.LEFT:
            case Reels.CENTER:
            case Reels.RIGHT:
                break;
            default:
                return Positions.NONE;
        }

        Lines candidateLines = GetReachCandidateLines(stopReel);
        Positions candidatePositions = GetCandidatePositionsForLines(selectReel,candidateLines);
        


        return candidatePositions;
    }

    //左リールを基準にし、upperToLowerは左上から右下
    public enum Lines : int
    {
        NONE = 0,
        upperToLower = 1, //左上から右下
        upperToUpper = 2, //左上から右上
        middleToMiddle = 4, //左中から右中
        lowerToLower = 8, //左下から右下
        lowerToUpper = 16, //左下から右上
    }

    //リーチにさせれるPositionsを返す
    //リーチにさせれるラインを第二引数にいれる
    public static Positions GetCandidatePositionsForLines(Reels selectReel, Lines candidateLines)
    {
        Positions candidatePositions = Positions.NONE;

        switch (selectReel)
        {
            case Reels.LEFT:
                if (candidateLines.HasFlag(Lines.upperToLower))
                {
                    candidatePositions |= Positions.TOP;
                }
                if (candidateLines.HasFlag(Lines.upperToUpper))
                {
                    candidatePositions |= Positions.TOP;
                }
                if (candidateLines.HasFlag(Lines.middleToMiddle))
                {
                    candidatePositions |= Positions.MIDDLE;
                }
                if (candidateLines.HasFlag(Lines.lowerToLower))
                {
                    candidatePositions |= Positions.BOTTOM;
                }
                if (candidateLines.HasFlag(Lines.lowerToUpper))
                {
                    candidatePositions |= Positions.BOTTOM;
                }
                break;

            case Reels.CENTER:
                if (candidateLines.HasFlag(Lines.upperToLower))
                {
                    candidatePositions |= Positions.MIDDLE;
                }
                if (candidateLines.HasFlag(Lines.upperToUpper))
                {
                    candidatePositions |= Positions.TOP;
                }
                if (candidateLines.HasFlag(Lines.middleToMiddle))
                {
                    candidatePositions |= Positions.MIDDLE;
                }
                if (candidateLines.HasFlag(Lines.lowerToLower))
                {
                    candidatePositions |= Positions.TOP;
                }
                if (candidateLines.HasFlag(Lines.lowerToUpper))
                {
                    candidatePositions |= Positions.MIDDLE;
                }
                break;

            case Reels.RIGHT:
                if (candidateLines.HasFlag(Lines.upperToLower))
                {
                    candidatePositions |= Positions.BOTTOM;
                }
                if (candidateLines.HasFlag(Lines.upperToUpper))
                {
                    candidatePositions |= Positions.TOP;
                }
                if (candidateLines.HasFlag(Lines.middleToMiddle))
                {
                    candidatePositions |= Positions.MIDDLE;
                }
                if (candidateLines.HasFlag(Lines.lowerToLower))
                {
                    candidatePositions |= Positions.BOTTOM;
                }
                if (candidateLines.HasFlag(Lines.lowerToUpper))
                {
                    candidatePositions |= Positions.TOP;
                }
                break;


        }



        return candidatePositions;
    }


    //リーチにさせれるラインを返す
    public static Lines GetReachCandidateLines(Reels stopReel)
    {
       

        Symbols[] nowStopReelOrder = GetReelOrder(stopReel);
        int nowStopReelPosition = GetNowReelPosition(stopReel);
        Positions stopReelSymbolPositions = Positions.NONE;


        //役を成立させるシンボルがはいっていたらPositionsビットフラグを上げる
        if (GetIsEstablishingRole(nowStopReelOrder[CalcReelPosition(nowStopReelPosition, 2)]))
        {
            stopReelSymbolPositions = Positions.TOP;
        }

        if (GetIsEstablishingRole(nowStopReelOrder[CalcReelPosition(nowStopReelPosition, 1)]))
        {
            stopReelSymbolPositions |= Positions.MIDDLE;
        }

        if (GetIsEstablishingRole(nowStopReelOrder[nowStopReelPosition]))
        {
            stopReelSymbolPositions |= Positions.BOTTOM;
        }

        Lines reachCandidateLines = Lines.NONE;

        switch (stopReel)
        {
            case Reels.LEFT:
                if (stopReelSymbolPositions.HasFlag(Positions.TOP))
                {
                    reachCandidateLines |= Lines.upperToLower;
                    reachCandidateLines |= Lines.upperToUpper;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.MIDDLE))
                {
                    reachCandidateLines |= Lines.middleToMiddle;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.BOTTOM))
                {
                    reachCandidateLines |= Lines.lowerToLower;
                    reachCandidateLines |= Lines.lowerToUpper;
                }
                break;

            case Reels.CENTER:
                if (stopReelSymbolPositions.HasFlag(Positions.TOP))
                {
                    reachCandidateLines |= Lines.upperToUpper;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.MIDDLE))
                {
                    reachCandidateLines |= Lines.upperToLower;
                    reachCandidateLines |= Lines.middleToMiddle;
                    reachCandidateLines |= Lines.lowerToUpper;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.BOTTOM))
                {
                    reachCandidateLines |= Lines.lowerToLower;
                }
                break;

            case Reels.RIGHT:
                if (stopReelSymbolPositions.HasFlag(Positions.TOP))
                {
                    reachCandidateLines |= Lines.upperToUpper;
                    reachCandidateLines |= Lines.lowerToUpper;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.MIDDLE))
                {
                    reachCandidateLines |= Lines.middleToMiddle;
                }
                if (stopReelSymbolPositions.HasFlag(Positions.BOTTOM))
                {
                    reachCandidateLines |= Lines.upperToLower;
                    reachCandidateLines |= Lines.lowerToLower;
                }
                break;
        }

        return reachCandidateLines;
    }



    //役が成立するリーチのPositionsを返す
    private static Positions GetRoleReachPositions()
    {

        Positions[] positions = { Positions.TOP, Positions.MIDDLE, Positions.BOTTOM };
        Positions reachPositions = Positions.NONE;


        foreach(Positions position in positions)
        {
            if(GetIsEstablishingRole(GetReachSymbolsForPosition(position)))
            {
                reachPositions |= position;
            }
        }


        return reachPositions;
    }



    //リールの指定したポジションでリーチのシンボルを取得する
    //searchPositionは探索場所をTOP,MIDDLE,BOTTOMを代入する
    private static Symbols GetReachSymbolsForPosition(Positions searchPosition)
    {
        Symbols reachSymbols = Symbols.NONE;
        Reels movingReel = Reels.NONE;


        if (stopReelCount < 2)
        {
            return Symbols.NONE;
        }


        if (leftReelMoving)
        {
            movingReel |= Reels.LEFT;
        }

        if (centerReelMoving)
        {
            movingReel = Reels.CENTER;
        }

        if (rightReelMoving)
        {
            movingReel = Reels.RIGHT;
        }



        //動いているリールの、それぞれの段に対応したリーチのシンボルをビットフラグで代入する
        if (movingReel == Reels.LEFT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.lowerToUpper);
                    break;
            }
        }

        if (movingReel == Reels.CENTER)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.middleToMiddle);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.lowerToUpper);
                    break;
                case Positions.BOTTOM:
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.lowerToLower);
                    break;
            }
        }

        if (movingReel == Reels.RIGHT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.lowerToUpper);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(movingReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(movingReel, Lines.upperToLower);
                    break;
            }
        }

        return reachSymbols;
    }

    //現時点リーチ目を滑らせない仕様
    //入ったら当選するシンボルを指定したLinesを元に取得する
    private static Symbols GetReachSymbolForLine(Reels selectReel , Lines line )
    {

        Symbols reachSymbol = Symbols.NONE; //リーチになっているシンボルが入る,OR演算で複数代入可能
        Symbols lineSymbols = Symbols.NONE; //選択したライン上にあるシンボルを代入する

        Reels stoppedReels = Reels.NONE;

        Symbols[] leftReelOrder = GetReelOrder(Reels.LEFT);
        Symbols[] centerReelOrder = GetReelOrder(Reels.CENTER);
        Symbols[] rightReelOrder = GetReelOrder(Reels.RIGHT);

        switch (selectReel)
        {
            case Reels.LEFT:
                stoppedReels = Reels.CENTER | Reels.RIGHT;
                break;
            case Reels.CENTER:
                stoppedReels = Reels.LEFT | Reels.RIGHT;
                break;
            case Reels.RIGHT:
                stoppedReels = Reels.LEFT | Reels.CENTER;
                break;
        }

        if (stoppedReels.HasFlag(Reels.LEFT)) //止まっているリールに左リールが含まれている時
        {
            switch(line)
            {
                case Lines.upperToLower: //左上から右下のライン
                    lineSymbols = leftReelOrder[CalcReelPosition(nowLeftReel, 2)];
                    break;
                case Lines.upperToUpper: //左上から右上のライン
                    lineSymbols = leftReelOrder[CalcReelPosition(nowLeftReel, 2)];
                    break;
                case Lines.middleToMiddle: //左中から右中のライン
                    lineSymbols = leftReelOrder[CalcReelPosition(nowLeftReel, 1)];
                    break;
                case Lines.lowerToLower: //左下から右下のライン
                    lineSymbols = leftReelOrder[nowLeftReel];
                    break;
                case Lines.lowerToUpper: //左下から右上のライン
                    lineSymbols = leftReelOrder[nowLeftReel];
                    break;
            }
        }

        if (stoppedReels.HasFlag(Reels.CENTER)) //止まっているリールに中央リールが含まれている時
        {
            switch (line)
            {
                case Lines.upperToLower:
                    lineSymbols |= centerReelOrder[CalcReelPosition(nowCenterReel, 1)]; // "|="で左辺と右辺のOR演算の結果を代入する
                    break;
                case Lines.upperToUpper:
                    lineSymbols |= centerReelOrder[CalcReelPosition(nowCenterReel, 2)];
                    break;
                case Lines.middleToMiddle:
                    lineSymbols |= centerReelOrder[CalcReelPosition(nowCenterReel, 1)];
                    break;
                case Lines.lowerToLower:
                    lineSymbols |= centerReelOrder[nowCenterReel];
                    break;
                case Lines.lowerToUpper:
                    lineSymbols |= centerReelOrder[CalcReelPosition(nowCenterReel, 1)];
                    break;
            }
        }

        if (stoppedReels.HasFlag(Reels.RIGHT)) //止まっているリールに右リールが含まれている時
        {
            switch (line)
            {
                case Lines.upperToLower: //左上から右下のライン
                    lineSymbols |= rightReelOrder[nowRightReel];
                    break;
                case Lines.upperToUpper: //左上から右上のライン
                    lineSymbols |= rightReelOrder[CalcReelPosition(nowRightReel, 2)];
                    break;
                case Lines.middleToMiddle: //左中から右中のライン
                    lineSymbols |= rightReelOrder[CalcReelPosition(nowRightReel, 1)];
                    break;
                case Lines.lowerToLower: //左下から右下のライン
                    lineSymbols |= rightReelOrder[nowRightReel];
                    break;
                case Lines.lowerToUpper: //左下から右上のライン
                    lineSymbols |= rightReelOrder[CalcReelPosition(nowRightReel, 2)];
                    break;
            }
        }

        switch (lineSymbols)
        {
            case Symbols.BELL:
            case Symbols.REPLAY:
            case Symbols.WATERMELON:
            case Symbols.CHERRY: 
            case Symbols.BAR:
            case Symbols.SEVEN: //フォールスルー　ここから上のcaseの場合以下の処理をbreakまで実行する
                reachSymbol = lineSymbols;
                break;

            default:
                reachSymbol = Symbols.NONE;
                break;
        }

        if(lineSymbols.HasFlag(Symbols.BAR) && lineSymbols.HasFlag(Symbols.SEVEN)) //リーチ目になりそうな状態 
        {
            reachSymbol = Symbols.REACH; //REGが来た場合、優先で7を入れること BIGがきた場合は7以外を揃えること
        }



        return reachSymbol;
    }


    

}