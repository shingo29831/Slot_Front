using System;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Xml.Linq;
using static Constants;
using static Model.Setting;


//このクラスはゲームの内部処理を担当する
namespace Model;



public class Game
{
    static bool leftReelbtn = false;
    static bool centerReelbtn = false;
    static bool rightReelbtn = false;

    static sbyte bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static sbyte[] dispSymbol = new sbyte[3];
    public static sbyte stopReelCount = 0; //テスト前0


    static Symbols[] leftReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] centerReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] rightReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };


    static sbyte nowLeftReel = 0;
    static sbyte nowCenterReel = 6;
    static sbyte nowRightReel = 6;

    public static bool leftReelMoving = true;
    public static bool centerReelMoving = true; //テストでfalse
    public static bool rightReelMoving = true; //テストでfalse


    public static Roles nowRole = Roles.BELL; //テスト前はNONE
    public static Symbols symbolsAccordingRole = Symbols.NONE;

    //reachRowsはリーチとなる場所を3次元配列で格納する各リールのポジション(上・中・下)に二つまでの入ったら役が成立するシンボルを代入する
    public Symbols[,,] reachPositions = { { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //左リール : 0
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //中央リール : 1
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } } }; //右リール : 2

    public static readonly Symbols[] SYMBOLS_ARRAY = { Symbols.BELL, Symbols.REPLAY, Symbols.WATERMELON, Symbols.CHERRY, Symbols.BAR, Symbols.SEVEN, Symbols.REACH};

    public static readonly Positions[] POSITIONS_ARRAY = { Positions.BOTTOM, Positions.MIDDLE, Positions.TOP};
    public static readonly Reels[] REELS_ARRAY = { Reels.LEFT, Reels.CENTER, Reels.RIGHT };
    public static readonly Lines[] LINES_ARRAY = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
    public static readonly sbyte NOT = -1;


    //左リールを基準にし、upperToLowerは左上から右下
    public enum Lines : sbyte
    {
        NONE = 0,
        upperToLower = 1, //左上から右下
        upperToUpper = 2, //左上から右上
        middleToMiddle = 4, //左中から右中
        lowerToLower = 8, //左下から右下
        lowerToUpper = 16, //左下から右上
    }

    //リールの現在の位置をオーバフローさせないように計算する 第一引数に移動前,第二引数に移動数を代入
    public static sbyte CalcReelPosition(sbyte reelPosition,sbyte move)
    {
        reelPosition += move;
        if (reelPosition < 0)
        {
            reelPosition += 21;
        }
        if (reelPosition > 20) 
        {
            reelPosition %= 21;
        }

        return reelPosition;
    }


    //リールを一つずつ移動させる　移動先が決定されている場合はリールのポジションを代入なければ NONE:-1 を代入
    public static void UpReelPosition(Reels selectReel,sbyte destinationPosition)
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
    public static sbyte GetNowReelPosition(Reels selectReel)
    {
        sbyte nowPosition = NONE;
        switch(selectReel)
        {
            case Reels.LEFT:
                return nowLeftReel;
                break;

            case Reels.CENTER:
                return nowCenterReel;
                break;

            case Reels.RIGHT:
                return nowRightReel;
                break;
        }
        return nowPosition;
    }


    //表示するリールの位置を取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHTとTOP,MIDDLE,BOTTOM
    public static sbyte GetSymbolForPosition(Reels selectReel,Positions position)
    {
        sbyte reelPosition = NONE;
        sbyte nowReelPosition = GetNowReelPosition (selectReel);
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

        nowReelSymbols = reelOrder[GetSymbolForPosition(selectReel,Positions.TOP)];
        nowReelSymbols |= reelOrder[GetSymbolForPosition(selectReel, Positions.MIDDLE)];
        nowReelSymbols |= reelOrder[GetSymbolForPosition(selectReel, Positions.BOTTOM)];

        return nowReelSymbols;
    }


    //ボーナス抽選開始後実行する"ボーナス抽選関数" bonusProbabilityに設定された確率に合わせて抽選する
    public static bool BonusLottery()
    {
        Random rnd = new Random();
        sbyte rndnum = (sbyte)rnd.Next(1, 101);  //1以上101未満の値がランダムに出力
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
        sbyte regularProbabilityWeight = Setting.getBonusesProbabilityWeight(0);
        sbyte bigProbabilityWeight = Setting.getBonusesProbabilityWeight(1);
        sbyte sumWeight = (sbyte)(regularProbabilityWeight + bigProbabilityWeight);
        sbyte rndnum = (sbyte)rnd.Next(1, sumWeight+1);  //1以上sumWeight以下の値がランダムに出力

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
    public static sbyte HitRoleLottery()
    {

        sbyte role = 0;
        sbyte sumWeight = 0;
        sbyte lotteryRange = 6;


        for (sbyte i = 0; i <= lotteryRange; i++)
        {
            sumWeight += Setting.getRoleWeight(i);
        }

        Random rnd = new Random();
        sbyte rndnum = (sbyte)rnd.Next(1, sumWeight + 1);
        sumWeight = 0;
        sbyte tmp = 0;
        for (sbyte i = 0; i <= lotteryRange; i++)
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
        stopReelCount++;
    }

    //リールを全て動いている判定にする
    public static void ResetReelsMoving()
    {
        SetReelMoving(Reels.LEFT,true);
        SetReelMoving(Reels.CENTER,true);
        SetReelMoving(Reels.RIGHT,true);
        stopReelCount = 0;
    }


    //選択したリールの次のリールポジションを返す
    public static sbyte GetReelPosition(Reels selectReel)
    {
        sbyte reelPosition = NONE;

        sbyte nowReelposition = GetNowReelPosition(selectReel);
        bool isFindedReelPosition = false;
        bool isFindedProxyReelPosition = false;
        //nowReelPositionの5つ先まで止まるため5を代入
        for (sbyte gapNowReelPosition = 4; gapNowReelPosition >= 0; gapNowReelPosition--)
        {
            sbyte searchReelPosition = CalcReelPosition(nowReelposition,gapNowReelPosition);
            bool isExclusion = GetIsExclusion(selectReel, searchReelPosition);
            if (isExclusion == false && GetIsAchieveRole(selectReel,searchReelPosition))
            {
                reelPosition = searchReelPosition;
                isFindedReelPosition = true;
                //MessageBox.Show("役"+searchReelPosition.ToString());
            }
            if(isExclusion == false && GetIsReachRole(selectReel,searchReelPosition) && isFindedReelPosition == false)
            {
                reelPosition = searchReelPosition;
                isFindedProxyReelPosition = true;
                //MessageBox.Show("リーチ目" + searchReelPosition.ToString());
            }
            if(isExclusion == false && (isFindedReelPosition | isFindedProxyReelPosition) == false)
            {
                reelPosition = searchReelPosition;
                //MessageBox.Show("役なし" + searchReelPosition.ToString());
            }

        }


        return reelPosition;
    }



    //選択したリールの位置が除外か否かを返す
    //getIsExclusion(リールの選択,リールの位置)
    private static bool GetIsExclusion(Reels selectReel, sbyte reelPosition)
    {

        Symbols[] reelOrder = GetReelOrder(selectReel);



        //以下は弱チェリーを回避する処理

        sbyte searchPosition = reelPosition;


        for (sbyte i = 0; i < 3 && selectReel == Reels.LEFT && GetSymbolsAccordingRole().HasFlag(Symbols.CHERRY) == false; i++) //
        {
            if (reelOrder[searchPosition] == Symbols.CHERRY)
            {
               
                return true;
            }
            searchPosition = CalcReelPosition(searchPosition, 1);
        }

        //弱チェリー回避ここまで

        //ここからシンボルの除外
        Symbols topExclusionSymbols = GetExclusionSymbolsForPosition(selectReel, Positions.TOP);
        Symbols middleExclusionSymbols = GetExclusionSymbolsForPosition(selectReel, Positions.MIDDLE);
        Symbols bottomExclusionSymbols = GetExclusionSymbolsForPosition(selectReel, Positions.BOTTOM);

        Symbols[] exclusionSymbolsForReel = { bottomExclusionSymbols, middleExclusionSymbols, topExclusionSymbols };

        sbyte gap = 0;
        foreach (Symbols exclusionSymbols in exclusionSymbolsForReel) //除外するシンボルをBOTTOM～TOPの順で代入
        {
            foreach (Symbols symbol in SYMBOLS_ARRAY)
            {
                if (exclusionSymbols.HasFlag(symbol) && reelOrder[CalcReelPosition(reelPosition, gap)] == symbol) //除外するシンボルで判定しているか && 除外するシンボルであるか 
                {
                    return true;
                }
            }
            gap++; //ポジションを一つずつ上げる
        }

        return false;
    }


    //除外するシンボルをビットフラグで返す　positionにはTOP,MIDDLE,BOTTOMが入る
    private static Symbols GetExclusionSymbolsForPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        Symbols exclusionSymbols = Symbols.NONE;

        Symbols reachSymbols = GetReachSymbolsForMovingReelsPosition(position);

        switch (nowRole)
        {
            case Roles.BELL:
                //リーチのビットフラグからベルのフラグを消している
                exclusionSymbols = reachSymbols & ~Symbols.BELL;
                break;


            case Roles.REPLAY:
                exclusionSymbols = reachSymbols & ~Symbols.REPLAY;
                break;


            case Roles.WATERMELON:
                exclusionSymbols = reachSymbols & ~Symbols.WATERMELON;
                break;


            case Roles.WEAK_CHERRY:
                if (selectReel == Reels.LEFT) //弱チェリーが当選した時、左リールでは除外用ビットフラグからチェリーを消す
                {
                    exclusionSymbols = reachSymbols & ~Symbols.CHERRY;
                }
                if (selectReel == Reels.CENTER || selectReel == Reels.RIGHT) //弱チェリーが当選した時、中・右リールではリーチになった全てのシンボルをビットフラグに入れる
                {
                    exclusionSymbols = reachSymbols;
                }
                break;


            case Roles.STRONG_CHERRY:
                if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT) && (position == Positions.TOP || position == Positions.BOTTOM)) //左・右リールで上・下段の時、除外用ビットフラグのチェリーを下げる
                {
                    exclusionSymbols = reachSymbols & ~Symbols.CHERRY;
                }
                if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT) && position == Positions.MIDDLE) //強チェリーが当選した時、左または右リールの中段にチェリーが来ないようにする
                {
                    exclusionSymbols = reachSymbols;
                }
                if (selectReel == Reels.CENTER) //中央リールでは、どこにチェリーが来てもよい
                {
                    exclusionSymbols = reachSymbols & ~Symbols.CHERRY;
                }
                break;


            case Roles.VERY_STRONG_CHERRY:
                exclusionSymbols = reachSymbols & ~Symbols.CHERRY; //配置的に強チェリーになっても良いためどこにチェリーがきても良い
                break;


            case Roles.REGULAR:
                if (reachSymbols.HasFlag(Symbols.BAR))
                {
                    exclusionSymbols = reachSymbols & ~Symbols.SEVEN; //除外用ビットフラグからSEVENフラグを消す BARを揃えるとBBになるため注意
                }
                if (reachSymbols.HasFlag(Symbols.SEVEN))
                {
                    exclusionSymbols = reachSymbols & ~Symbols.BAR;
                }
                if (reachSymbols.HasFlag(Symbols.REACH))
                {
                    exclusionSymbols = reachSymbols & ~(Symbols.BAR | Symbols.SEVEN);
                }
                break;


            case Roles.BIG:
                if (reachSymbols.HasFlag(Symbols.BAR))
                {
                    exclusionSymbols = reachSymbols & ~Symbols.BAR;
                }
                if (reachSymbols.HasFlag(Symbols.SEVEN))
                {
                    exclusionSymbols = reachSymbols & ~Symbols.SEVEN;
                }

                Symbols[] centerReelOrder = GetReelOrder(Reels.CENTER);
                Symbols centerSymbols = GetNowReelSymbols(Reels.CENTER);

                if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時でに中央リールのシンボルを選択する時は除外用ビットフラグからBARを消す
                {
                    exclusionSymbols = reachSymbols & ~Symbols.BAR;
                }

                if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.SEVEN) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールに7が来ていた時
                {
                    exclusionSymbols = reachSymbols & ~Symbols.BAR;
                }

                if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.BAR) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールにBARが来ていた時
                {
                    exclusionSymbols = reachSymbols & ~(Symbols.BAR | Symbols.SEVEN); //7とBARを除外用ビットフラグから消す
                }
                break;

            case Roles.NONE:
                exclusionSymbols = reachSymbols;
                break;
        }


        return exclusionSymbols;
    }




    //役を達成できるか否か返す
    public static bool GetIsAchieveRole(Reels selectReel,sbyte reelPosition)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);


        Symbols topAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.TOP);
        Symbols middleAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.MIDDLE);
        Symbols bottomAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.BOTTOM);

        Symbols[] achieveRoleSymbolsForReel = { bottomAchieveRoleSymbols, middleAchieveRoleSymbols, topAchieveRoleSymbols };

        sbyte cnt = 0;
        foreach (Symbols achieveRoleSymbols in achieveRoleSymbolsForReel) //除外するシンボルをBOTTOM～TOPの順で代入
        {
            foreach (Symbols symbol in SYMBOLS_ARRAY)
            {
                if (achieveRoleSymbols.HasFlag(symbol) && reelOrder[CalcReelPosition(reelPosition, cnt)] == symbol) //roleReachで判定しているか && 除外するシンボルであるか 
                {
                    return true;
                }
            }
            cnt++; //ポジションを一つずつ上げる
        }

        return false;
    }


    //一つ目は役を達成できるシンボルを返す
    //二つ目は候補のライン上のシンボルを返す
    //三つ目はリーチ上の役を成立できるシンボルを返す
    public static Symbols GetAchieveRoleSymbolsForPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        Symbols exclusionSymbols = Symbols.NONE;
        Lines searchLines = Lines.NONE;
        
        Positions searchPositions = Positions.NONE;
        Reels stopReels = (Reels.LEFT|Reels.CENTER|Reels.RIGHT) & GetMovingReels();

        switch (stopReelCount)
        {
            case 0:
                return GetSymbolsAccordingRole();
                break;
            case 1:
                if (selectReel == Reels.CENTER && nowRole == Roles.REGULAR && GetPositionsToReach(selectReel).HasFlag(position)) //中リールでレギュラー役が来た時、リーチにさせれるpositionだった時
                {
                    return Symbols.SEVEN;
                }
                if((Reels.LEFT|Reels.RIGHT).HasFlag(selectReel) && nowRole == Roles.REGULAR && //左または右リールでレギュラー役の時
                    GetIsStopBarForLines((Reels.LEFT | Reels.RIGHT) & ~selectReel,position) && GetPositionsToReach(selectReel).HasFlag(position)) //かつ止まっているリールにバーがあった時でリーチにさせれるpositionだった時
                {
                    return Symbols.SEVEN;
                }
                if (GetPositionsToReach(selectReel).HasFlag(position)) //リーチにさせれるpositionだった時
                {
                    return GetSymbolsAccordingRole();
                }
                break;
            case 2:
                if (GetIsReachLinesForRole(position)) //ここバグ修正中
                {
                    return GetSymbolsCanArcheveReachRole();
                }

                break;
        }

        


        return exclusionSymbols;
    }

    //positionが役を達成できるリーチライン上にある時true
    public static bool GetIsReachLinesForRole(Positions position)
    {
        
        Lines reachLines = GetReachLinesCanArcheveRole();
        Reels movingReel = GetLastMovingReel();
        bool isRole = false;
        foreach(Symbols symbol in SYMBOLS_ARRAY)
        {
            if (GetSymbolsCanArcheveReachRole().HasFlag(symbol))
            {
                isRole = true;
            }
        }
        

        if (GetPositionsForLines(movingReel, reachLines).HasFlag(position) && isRole)
        {
            return true;
        }
        
        return false;
    }

    //バーが止まっているか否か返す　二つ目のリールの処理用
    //(一つ目ののリール,二つ目のリールのPositions)
    public static bool GetIsStopBarForLines(Reels selectStopReel,Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectStopReel);
        Positions stopReelBarPosition = GetPositionsForLines(selectStopReel,GetReachCandidateLines()) ;
        if(selectStopReel != Reels.CENTER && reelOrder[GetSymbolForPosition(selectStopReel,stopReelBarPosition)] == Symbols.BAR)
        {
            return true;
        }
        return false;
    }


    public static bool GetIsReachRole(Reels selectReel,sbyte reelPosition)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);


        Symbols topReachRoleSymbols = GetReachRoleSymbolsForPosition(selectReel, Positions.TOP);
        Symbols middleReachRoleSymbols = GetReachRoleSymbolsForPosition(selectReel, Positions.MIDDLE);
        Symbols bottomReachRoleSymbols = GetReachRoleSymbolsForPosition(selectReel, Positions.BOTTOM);

        Symbols[] reachRoleSymbolsForReel = { bottomReachRoleSymbols, middleReachRoleSymbols, topReachRoleSymbols };

        sbyte cnt = 0;
        foreach (Symbols reachRoleSymbols in reachRoleSymbolsForReel) //除外するシンボルをBOTTOM～TOPの順で代入
        {
            foreach (Symbols symbol in SYMBOLS_ARRAY)
            {
                if (reachRoleSymbols.HasFlag(symbol) && reelOrder[CalcReelPosition(reelPosition, cnt)] == symbol) //roleReachで判定しているか && 除外するシンボルであるか 
                {
                    return true;
                }
            }
            cnt++; //ポジションを一つずつ上げる
        }

        return false;
    }

    //リーチ役が成立するシンボルを返す
    public static Symbols GetReachRoleSymbolsForPosition(Reels selectReel, Positions position)
    {
        Positions searchPositions = Positions.NONE;
        Symbols reachRoleSymbols = Symbols.NONE;

        foreach(Lines line in LINES_ARRAY)
        {
            if (GetReachLinesCanArcheveRole().HasFlag(line) && GetPositionsForLines(selectReel,line).HasFlag(position))
            {
                reachRoleSymbols = GetSymbolForReachRoleLine(selectReel,line);
            }
            if(GetReachCandidateLines().HasFlag(line) && GetPositionsForLines(selectReel, line).HasFlag(position))
            {
                reachRoleSymbols = GetSymbolForReachRoleLine(selectReel,line);
            }
        }

        return reachRoleSymbols;

    }

    //リーチ目の条件となるシンボルを返す
    public static Symbols GetSymbolForReachRoleLine(Reels selectReel,Lines line)
    {
        Reels stopReels = (Reels.LEFT|Reels.CENTER|Reels.RIGHT) &~GetMovingReels();

        if (stopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.REACH && GetSymbolForLine(Reels.CENTER,line) == Symbols.BAR)
        {
            return Symbols.SEVEN|Symbols.BAR;
        }
        if (stopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.REACH && GetSymbolForLine(Reels.CENTER, line) == Symbols.SEVEN)
        {
            return Symbols.NONE;
        }
        if (stopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.BAR)
        {
            return Symbols.SEVEN;
        }
        if (stopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.SEVEN)
        {
            return Symbols.NONE;
        }
        if (stopReelCount == 2 && selectReel == Reels.CENTER && GetReachSymbolForLine(selectReel,line) == Symbols.REACH)
        {
            return Symbols.BAR;
        }
        if (stopReelCount == 2 && selectReel == Reels.CENTER && (Symbols.SEVEN|Symbols.BAR).HasFlag(GetReachSymbolForLine(selectReel, line)) ) //停止リールが2つで中リールの時リーチなっていないシンボルを返す
        {
            return (Symbols.SEVEN|Symbols.BAR) &~GetReachSymbolForLine(selectReel, line);
        }
        if (stopReelCount == 1 && (Roles.BIG|Roles.REGULAR).HasFlag(nowRole)) //停止リールが1つでnowRoleがBIGかREGの時
        {
            return Symbols.SEVEN | Symbols.BAR;
        }
        
        return Symbols.NONE;


    }



    //役が成立できない時に、除外範囲ではない最も近いリールの位置を代入する
    public static sbyte GetReelPositionForRoleFailure(Reels selectReel)
    {
        sbyte reelPosition = GetNowReelPosition(selectReel);
        for (sbyte gapNowReelPosition = 0; gapNowReelPosition < 5; gapNowReelPosition++) //reelPositionがNONEだった時に最も近い代入可能な位置をいれる
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
    public static sbyte CalcGapNowReelPosition(Reels selectReel, sbyte reelSearchPosition)
    {
        sbyte gap = 0;
        sbyte reelPosition = GetNowReelPosition(selectReel);
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

    //現在の役を成立させれるシンボルであるかboolで返す
    //停止リールが2つで役を成立させるリーチの時はPositionsを元にそのリーチを達成できる時のみtrue
    public static bool GetIsCanAchieveRoleForSymbols(Symbols searchSymbols,Positions searchPosition)
    {

        foreach(Symbols symbol in SYMBOLS_ARRAY) //全てのシンボルのビットフラグを順に代入
        {
            //searchSymbolsのビットフラグがnowRoleを達成できるリーチなシンボルか
            if (searchSymbols.HasFlag(symbol) && GetSymbolsCanArcheveReachRole().HasFlag(symbol)) //symbolがsearchSymbolで役を成立させれるシンボルの時は実行
            {
                return true;
            }
        }

        return false;
    }

    //役を成立させれるシンボルを返す
    //停止リールが2の時はリーチを揃えるシンボルを返す
    public static Symbols GetSymbolsCanAchieveRoleForPosition(Positions searchPosition)
    {
        //停止リールが2よりすくない場合は役を成立させれるシンボルをそのまま返す　それ以上の時はリーチを揃えるシンボルを返す
        if(stopReelCount < 2)
        {
            return GetSymbolsAccordingRole();
        }

        Symbols reachSymbols = GetReachSymbolsForMovingReelsPosition(searchPosition);
        Symbols canAchieveRoleSymbols = Symbols.NONE;
        Symbols[] candidateSymbolArray = { Symbols.NONE, Symbols.NONE };
        sbyte element = 0;
        foreach (Symbols symbol in SYMBOLS_ARRAY) //全てのシンボルのビットフラグを順に代入
        {
            //searchSymbolsのビットフラグがnowRoleを達成できるシンボルであり、リーチのシンボルに含まれる時
            if (GetSymbolsAccordingRole().HasFlag(symbol) && reachSymbols.HasFlag(symbol))
            {
                candidateSymbolArray[element] = symbol;
                element++;
            }
        }





        return canAchieveRoleSymbols;
    }





    //現在の役を成立させれるシンボルをビットフラグで返す
    //複数のシンボルが入っていても一つでも役を成立させれるならtrueを返す
    //現在の役が揃うか隣接しているシンボルを元に求めること
    public static bool GetIsEstablishingRole(Symbols searchSymbols)//あとで変更
    {

        Symbols[] seachSymbolsArray = { Symbols.NONE, Symbols.NONE, Symbols.NONE};
        foreach(Symbols symbol in SYMBOLS_ARRAY)
        {
            if (searchSymbols.HasFlag(symbol) && GetSymbolsAccordingRole().HasFlag(symbol)) //どのシンボルが入っているか見ていき同時に役を成立させるか判定する
            {
                return true;
            }
        }


        return false;
    }


    //役を成立させるシンボルを取得
    private static Symbols GetSymbolsAccordingRole()
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
    public static sbyte GetStopCandidateForPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        sbyte nowReelPosition = GetNowReelPosition(selectReel);
        sbyte maxRange = 6;
        sbyte repetitions = 7;
        sbyte gapBottomPosition = NONE;
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
        sbyte searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        sbyte findedPostion = NONE;
        for(sbyte i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (GetIsEstablishingRole(reelOrder[searchReelPosition]) && GetIsExclusion(selectReel,CalcReelPosition(searchReelPosition ,(sbyte)(gapBottomPosition * -1))) == false) //役を成立させれるシンボル && 除外範囲ではない
            {
                 findedPostion = searchReelPosition;
            }
        }
        return findedPostion;
    }

    //選択したポジションに入れることが可能な現在のポジションに最も近いBARを取得
    public static sbyte GetBarPosition(Reels selectReel, Positions position)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel);
        sbyte nowReelPosition = GetNowReelPosition(selectReel);
        sbyte maxRange = 6;
        sbyte repetitions = 7;
        sbyte gapBottomPosition = 0;
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

        sbyte searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        sbyte barPostion = NONE;

        for (sbyte i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (reelOrder[searchReelPosition] == Symbols.BAR && GetIsExclusion(selectReel,CalcReelPosition(searchReelPosition,(sbyte)(gapBottomPosition * -1)))) //
            {
                barPostion = searchReelPosition;
            }
        }
        return barPostion;
    }

    

    


    //当選した役をリーチにさせる、選択したリールのPositionsをビットフラグで返す
    //ない場合とリールが2つ以上止まっている時はNONEで返す
    private static Positions GetPositionsToReach(Reels selectReel)
    {
        if(stopReelCount >= 2)
        {
            return Positions.NONE;
        }

        Lines candidateLines = GetReachCandidateLines();
        Positions candidatePositions = GetPositionsForLines(selectReel,candidateLines);
        


        return candidatePositions;
    }


    //選択したリール上でLines上のPositionsを返す
    //ラインを第二引数にいれる
    private static Positions GetPositionsForLines(Reels selectReel, Lines candidateLines)
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
                    candidatePositions |= Positions.BOTTOM;
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
    private static Lines GetReachCandidateLines()
    {
        if (stopReelCount != 1)
        {
            return Lines.NONE;
        }

        Reels stopReel = (Reels.LEFT | Reels.CENTER | Reels.RIGHT) & ~GetMovingReels();

        Symbols[] nowStopReelOrder = GetReelOrder(stopReel);
        sbyte nowStopReelPosition = GetNowReelPosition(stopReel);
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

        Lines reachLines = GetReachLinesCanArcheveRole();
        Positions reachPositions = Positions.NONE;

        Reels movingReels = GetLastMovingReel();

        foreach (Lines line in LINES_ARRAY)
        {
            if (reachLines.HasFlag(line))
            {
                reachPositions = GetPositionsForLines(movingReels, line);
            }
        }



        return reachPositions;
    }



    //役を成立させれるリーチのラインを返す
    private static Lines GetReachLinesCanArcheveRole()
    {
        Reels movingReel = GetLastMovingReel();
        Lines reachLine = Lines.NONE;
        if(stopReelCount != 2)
        {
            return Lines.NONE;
        }

        foreach (Lines line in LINES_ARRAY)
        {
            if(GetSymbolsAccordingRole().HasFlag(GetReachSymbolForLine(movingReel, line)))
            {
                reachLine |= line;
            }
        }

        return reachLine;
    }




    //役を成立させれるシンボルを返す
    public static Symbols GetSymbolsCanArcheveReachRole()
    {
        Symbols symbolArcheveReachRole = Symbols.NONE;
        Lines reachLines = GetReachLinesCanArcheveRole();

 

        foreach (Lines line in LINES_ARRAY)
        {
            if (reachLines.HasFlag(line))
            {
                symbolArcheveReachRole |= GetChangeToAvailableSymbol(line);
            }
        }


        return symbolArcheveReachRole;

    }

    //役を成立できるリーチ状態のシンボルを比較可能な状態にする
    public static Symbols GetChangeToAvailableSymbol(Lines line)
    {
        Reels movingReel = GetLastMovingReel();
        Reels stopedReels = (Reels.LEFT | Reels.CENTER | Reels.RIGHT) & ~movingReel;
        Reels[] stopedReelArray = { Reels.NONE, Reels.NONE };
        Symbols[] stopedSymbolArray = { Symbols.NONE, Symbols.NONE };

        Symbols reachSymbol = Symbols.NONE;

        sbyte element = 0;
        foreach (Reels reel in REELS_ARRAY) //左～右へ探索
        {
            if (stopedReels.HasFlag(reel))
            {
                stopedReelArray[element] = reel;
                element++;
            }
        }

        stopedSymbolArray[0] = GetSymbolForLine(stopedReelArray[0], line);
        stopedSymbolArray[1] = GetSymbolForLine(stopedReelArray[1], line);
        reachSymbol = GetReachSymbolForLine(movingReel, line);

        if(nowRole != Roles.REGULAR)
        {
            return reachSymbol;
        }

        if(reachSymbol == Symbols.REACH && movingReel == Reels.LEFT && stopedSymbolArray[0] == Symbols.SEVEN)
        {
            return Symbols.SEVEN;
        }

        if(reachSymbol == Symbols.REACH && movingReel == Reels.CENTER)
        {
            return Symbols.SEVEN;
        }

        if(reachSymbol == Symbols.REACH && movingReel == Reels.RIGHT && stopedSymbolArray[1] == Symbols.SEVEN)
        {
            return Symbols.SEVEN;
        }

        if(nowRole == Roles.REGULAR && reachSymbol == Symbols.SEVEN)
        {
            return Symbols.BAR;
        }

        return reachSymbol;
    }



    //選択したリールとライン上にあるシンボルを返す
    public static Symbols GetSymbolForLine(Reels selectReel,Lines line)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel); 
        Positions searchPosition = GetPositionsForLines(selectReel, line);
        return reelOrder[GetSymbolForPosition(selectReel, searchPosition)];

    }


    //リールの指定したポジションでリーチのシンボルを取得する
    //searchPositionは探索場所をTOP,MIDDLE,BOTTOMを代入する
    private static Symbols GetReachSymbolsForMovingReelsPosition(Positions searchPosition)
    {
        Reels movingReel = GetLastMovingReel();

        return GetReachSymbolsForPosition(movingReel, searchPosition);
    }


    //選択したリールのPositionsを元に、リーチのシンボルを返す
    //なければNONE
    private static Symbols GetReachSymbolsForPosition(Reels selectReel,Positions searchPosition)
    {
        Symbols reachSymbols = Symbols.NONE;

        if (selectReel == Reels.LEFT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    break;
            }
        }

        if (selectReel == Reels.CENTER)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    break;
                case Positions.BOTTOM:
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    break;
            }
        }

        if (selectReel == Reels.RIGHT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    break;
            }
        }

        return reachSymbols;
    }



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

    private static Reels GetLastMovingReel()
    {
        if(stopReelCount < 2)
        {
            return Reels.NONE;
        }

        return GetMovingReels();
    }

    private static Reels GetMovingReels()
    {
        Reels movingReels = Reels.NONE;


        if (leftReelMoving)
        {
            movingReels |= Reels.LEFT;
        }

        if (centerReelMoving)
        {
            movingReels |= Reels.CENTER;
        }

        if (rightReelMoving)
        {
            movingReels |= Reels.RIGHT;
        }


        return movingReels;
    }
    

}