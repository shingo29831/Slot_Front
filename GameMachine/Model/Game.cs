using System;
using static Constants;
using static Constants.Symbol;
using static Constants.Role;
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
    static int stopReelCount = 0;


    static int[] leftReel = { NONE, NONE, NONE };
    static int[] centerReel = { NONE, NONE, NONE };
    static int[] rightReel = { NONE, NONE, NONE };


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
                    nowLeftReel = CalcReelPosition (nowLeftReel, 1);
                }

                break;

            case CENTER:
                if (nowCenterReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowCenterReel != destinationPosition)
                {
                    nowCenterReel = CalcReelPosition (nowCenterReel, 1);
                }
                
                break;

            case RIGHT:
                if (nowRightReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowRightReel != destinationPosition)
                {
                    nowRightReel = CalcReelPosition (nowRightReel, 1);
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
        int regularProbabilityWeight = Setting.getBonusesProbabilityWeight(REGULAR);
        int bigProbabilityWeight = Setting.getBonusesProbabilityWeight(BIG);
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
    public static int GetFirstReelPosition(int selectReel, int role)
    {
        int nowReelPosition = GetNowReelPosition(selectReel);
        int reelPosition = NONE;
        int[] reelOrder = GetReelOrder(selectReel); //選択されたリールのシンボル配列を参照渡しする
        int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE }; //roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE }; //roleを達成できるシンボルの位置を格納する 要素番号１はREGとBIGで7とBARで使用
        int searchReelPosition = nowReelPosition;

        int maxExclusion = NONE; //除外範囲の最大値を-1(リールより小さい値)に設定
        int minExclusion = 21; //除外範囲の最小値をリールより大きい値に設定

        int oneBeforePosition = NONE;
        int twoBeforePosition = NONE;

        bool cherryFounded = false;
        bool isLeft = false;
        bool isCenter = false;
        bool isRight = false;
        bool isPositionFounded = false;
        bool isBonus = false;

        bool isExcludeOneBefore = false;
        bool isExcludeTwoBefore = false;

        Random rnd = new Random();

        if (selectReel == LEFT)
        {
            isLeft = true; //レフトリールフラグをtrue
        }
        else if(selectReel == CENTER)
        {
            isCenter = true;
        }
        else if (selectReel == RIGHT)
        {
            isRight = true; //ライトリールフラグをtrue
        }

        //現在のリールのポジションで下から上に7つを候補として代入
        for (int i = 0; i < symbolCandidate.Length; i++)
        {
            if (reelOrder[searchReelPosition] == CHERRY && isLeft && cherryFounded == false && (role < WEAK_CHERRY || role > VERY_STRONG_CHERRY))
            {
                cherryFounded = true; //チェリー発見フラグをtrue
                maxExclusion = searchReelPosition; //チェリーシンボルがある位置を候補除外範囲の最大値として代入
                minExclusion = CalcReelPosition(searchReelPosition, -2); //チェリーシンボルから2つ下を候補除外範囲の最小値として代入
            }
            symbolCandidate[i] = searchReelPosition;
            searchReelPosition = CalcReelPosition(searchReelPosition, 1);
        }



        if (isLeft && cherryFounded && maxExclusion < minExclusion)//左リールでチェリーシンボルがみつかった時、最大値と最小値が反対なら交換する
        {
            int tmp = maxExclusion;
            maxExclusion = minExclusion;
            minExclusion = tmp;
        }



        //役を成立させるシンボルを代入、4以下(ボーナス以外)のロールはそのままシンボルとして代入、強・最強チェリーはCHERRYを代入REGとBIGは7と第二候補としてBARを代入
        if (role <= 4)
        {
            symbolsAccordingRole[0] = role;
        }
        else if (role <= OTHER_BONUS)
        {
            symbolsAccordingRole[0] = CHERRY;
        }
        else if (role == REGULAR || role == BIG)
        {
            symbolsAccordingRole[0] = SEVEN;
            symbolsAccordingRole[1] = BAR;
            isBonus = true;
        }




        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for (int i = symbolCandidate.Length - 1; i >= 0; i--) //候補となるシンボルの数実行、現在位置から遠い順
        {
            if (reelOrder[symbolCandidate[i]] == symbolsAccordingRole[0] && //止まり先候補のシンボルとロールを成立させるシンボルを比較
            (isBonus || (symbolCandidate[i] > maxExclusion || symbolCandidate[i] < minExclusion))) //役がBIGまたはREGの時、または除外範囲外であるか比較
            {
                stopCandidate[0] = symbolCandidate[i]; //停止候補に代入
            }
            else if (reelOrder[symbolCandidate[i]] == symbolsAccordingRole[1] && isBonus)//止まり先候補のシンボルとロールを成立させるシンボルを比較、また役がBIGまたはREGの時
            {
                stopCandidate[1] = symbolCandidate[i];
            }
        }

        if (stopCandidate[0] != NONE)
        {
            oneBeforePosition = CalcReelPosition(stopCandidate[0], -1); //ひとつ前のポジションを代入
            twoBeforePosition = CalcReelPosition(stopCandidate[0], -2); //ふたつ前のポジションを代入
        }

        if(oneBeforePosition <= maxExclusion && oneBeforePosition >= minExclusion) //第一候補の1つ前の地点が除外範囲か判定
        {
            isExcludeOneBefore = true;
        }
        if (twoBeforePosition <= maxExclusion && twoBeforePosition >= minExclusion) //第一候補の2つ前の地点が除外範囲か判定
        {
            isExcludeTwoBefore = true;
        }



        for (int i = 0; i <= 3 && isPositionFounded == false; i++)
        {
            if ((CalcReelPosition(nowReelPosition, i) > maxExclusion || CalcReelPosition(nowReelPosition, i) < minExclusion)) //押下した時点から+0～3の地点が除外範囲外の時
            {
                reelPosition = CalcReelPosition(nowReelPosition, i);
                isPositionFounded = true;
            }
        }



        if (stopCandidate[0] != NONE && //第一候補があり
            (stopCandidate[0] == CalcReelPosition(nowReelPosition, 6) || ((isLeft || isRight) && role == STRONG_CHERRY && stopCandidate[0] == CalcReelPosition(nowReelPosition, 5))) && //移動先が6つ先、または左リールか右リールの時に強チェリー役で移動先が5つ先の時
            isExcludeOneBefore == false && isExcludeTwoBefore == false ) //一つ前と二つ前の位置が除外範囲でない時
        {
            reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように停止位置を2つ戻す    
        }
        else if (stopCandidate[0] != NONE && stopCandidate[0] != nowReelPosition && //第1候補があり、移動前と移動先のリール位置が不一致
            isExcludeOneBefore == false && //またひとつ前が除外範囲でない時
            (isCenter && role == STRONG_CHERRY)) //中央リールで強チェリー役の時は除外
        {
            reelPosition = oneBeforePosition; //目的のシンボルが中央に来るように停止位置を1つ戻す

        }
        else if (stopCandidate[0] != NONE &&
            (stopCandidate[0] > maxExclusion || stopCandidate[0] < minExclusion)) //候補が除外範囲外の時
        {

            reelPosition = stopCandidate[0];
        }
        else if (stopCandidate[1] == CalcReelPosition(nowReelPosition, 6) && stopCandidate[1] != NONE && //移動先がもっとも遠く第2候補がある時
            isExcludeOneBefore == false && isExcludeTwoBefore) //一つ前と二つ前の位置が除外範囲外の時
        {

            reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す

        }
        else if (stopCandidate[1] != nowReelPosition && stopCandidate[0] != NONE && stopCandidate[1] != NONE && //移動前と移動先のリール位置が不一致で、第2候補がある時
            isExcludeOneBefore == false) //またひとつ前が除外範囲外の時
        {
            reelPosition = oneBeforePosition;
        }
        else if (stopCandidate[1] != NONE)
        {
            reelPosition = CalcReelPosition(stopCandidate[1], -1);
        }

        if (stopCandidate[0] != NONE && stopCandidate[1] != NONE && stopCandidate[0] > stopCandidate[1]) //BIGかREGの時に7とBARで近い方に止める←シンボルの並びが変わったら不具合
        {
            reelPosition = CalcReelPosition(stopCandidate[1], -1);
        }

        return reelPosition;
    }





    public static int GetSecondReelPosition(int firstReel,int selectReel, int role)
    {
        int nowFirstReelPosition = GetNowReelPosition(firstReel);
        int nowSecondReelPosition = GetNowReelPosition (selectReel);

        int reelPosition = NONE;
        int[] selectReelOrder = GetReelOrder(selectReel); //選択されたリールのシンボル配列を参照渡しする
        int[] firstSymbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] secondSymbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE }; //roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE }; //roleを達成できるシンボルの位置を格納する 要素番号１はREGとBIGで7とBARで使用
        int searchReelPosition = nowFirstReelPosition;
        int searchSecondReelPosition = nowSecondReelPosition;

        int maxExclusion = NONE; //除外範囲の最大値を-1(リールより小さい値)に設定
        int minExclusion = 21; //除外範囲の最小値をリールより大きい値に設定 

        int oneBeforePosition = NONE;
        int twoBeforePosition = NONE;

        bool cherryFounded = false;
        bool isLeft = false;
        bool isPositionFounded = false;
        bool isBonus = false;



        Random rnd = new Random();

        if (selectReel == LEFT)
        {
            isLeft = true; //レフトリールフラグをtrue
        }

        //現在のリールのポジションで下から上に7つを候補として代入
        for (int i = 0; i < secondSymbolCandidate.Length; i++)
        {
            if (selectReelOrder[searchReelPosition] == CHERRY && isLeft && cherryFounded == false && (role < WEAK_CHERRY || role > VERY_STRONG_CHERRY))
            {
                cherryFounded = true; //チェリー発見フラグをtrue
                maxExclusion = searchReelPosition; //チェリーシンボルがある位置を候補除外範囲の最大値として代入
                minExclusion = CalcReelPosition(searchReelPosition, -2); //チェリーシンボルから2つ下を候補除外範囲の最小値として代入
            }
            secondSymbolCandidate[i] = searchReelPosition;
            searchReelPosition = CalcReelPosition(searchReelPosition, 1);
        }


        for (int i = 0; i < firstSymbolCandidate.Length; i++)
        {
            if (selectReelOrder[searchSecondReelPosition] == CHERRY && isLeft && cherryFounded == false && (role < WEAK_CHERRY || role > VERY_STRONG_CHERRY))
            {
                cherryFounded = true; //チェリー発見フラグをtrue
                maxExclusion = searchSecondReelPosition; //チェリーシンボルがある位置を候補除外範囲の最大値として代入
                minExclusion = CalcReelPosition(searchSecondReelPosition, -2); //チェリーシンボルから2つ下を候補除外範囲の最小値として代入
            }
            firstSymbolCandidate[i] = searchSecondReelPosition;
            searchSecondReelPosition = CalcReelPosition(searchSecondReelPosition, 1);
        }






        if (isLeft && cherryFounded && maxExclusion < minExclusion)//左リールでチェリーシンボルがみつかった時、最大値と最小値が反対なら交換する
        {
            int tmp = maxExclusion;
            maxExclusion = minExclusion;
            minExclusion = tmp;
        }



        //役を成立させるシンボルを代入、4以下(ボーナス以外)のロールはそのままシンボルとして代入、強・最強チェリーはCHERRYを代入REGとBIGは7と第二候補としてBARを代入
        if (role <= 4)
        {
            symbolsAccordingRole[0] = role;
        }
        else if (role <= OTHER_BONUS)
        {
            symbolsAccordingRole[0] = CHERRY;
        }
        else if (role == REGULAR || role == BIG)
        {
            symbolsAccordingRole[0] = SEVEN;
            symbolsAccordingRole[1] = BAR;
            isBonus = true;
        }




        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for (int j = secondSymbolCandidate.Length - 1; j >= 0; j--) //候補となるシンボルの数実行、現在位置から遠い順
        {
            if (selectReelOrder[secondSymbolCandidate[j]] == symbolsAccordingRole[0] && //止まり先候補のシンボルとロールを成立させるシンボルを比較
            (isBonus || (secondSymbolCandidate[j] > maxExclusion || secondSymbolCandidate[j] < minExclusion))) //役がBIGまたはREGの時、または除外範囲外であるか比較
            {
                stopCandidate[0] = secondSymbolCandidate[j]; //停止候補に代入
            }
            else if (selectReelOrder[secondSymbolCandidate[j]] == symbolsAccordingRole[1] && isBonus)//止まり先候補のシンボルとロールを成立させるシンボルを比較、また役がBIGまたはREGの時
            {
                stopCandidate[1] = secondSymbolCandidate[j];
            }
        }

        if (stopCandidate[0] != NONE)
        {
            oneBeforePosition = CalcReelPosition(stopCandidate[0], -1); //ひとつ前のポジションを代入
            twoBeforePosition = CalcReelPosition(stopCandidate[0], -2); //ふたつ前のポジションを代入
        }


        for (int i = 0; i <= 3 && isPositionFounded == false; i++)
        {
            if ((CalcReelPosition(nowFirstReelPosition, i) > maxExclusion || CalcReelPosition(nowFirstReelPosition, i) < minExclusion)) //押下した時点から+0～3の地点が除外範囲外の時
            {
                reelPosition = CalcReelPosition(nowFirstReelPosition, i);
                isPositionFounded = true;
            }
        }

        if (stopCandidate[0] == CalcReelPosition(nowFirstReelPosition, 6) && stopCandidate[0] != NONE && //移動先がもっとも遠く第1候補がある時
            (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
            (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //一つ前と二つ前の位置が除外範囲外の時
        {
            reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す    
        }
        else if (stopCandidate[0] != nowFirstReelPosition && stopCandidate[0] != NONE && //移動前と移動先のリール位置が不一致で、第1候補がある時
            (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //またひとつ前が除外範囲外の時
        {
            reelPosition = oneBeforePosition;

        }
        else if (stopCandidate[0] != NONE &&
            (stopCandidate[0] > maxExclusion || stopCandidate[0] < minExclusion)) //候補が除外範囲外の時
        {

            reelPosition = stopCandidate[0];
        }
        else if (stopCandidate[1] == CalcReelPosition(nowFirstReelPosition, 6) && stopCandidate[1] != NONE && //移動先がもっとも遠く第2候補がある時
            (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
            (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //一つ前と二つ前の位置が除外範囲外の時
        {

            reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す

        }
        else if (stopCandidate[1] != nowFirstReelPosition && stopCandidate[0] != NONE && stopCandidate[1] != NONE && //移動前と移動先のリール位置が不一致で、第2候補がある時
            (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //またひとつ前が除外範囲外の時
        {
            reelPosition = oneBeforePosition;
        }
        else if (stopCandidate[1] != NONE)
        {
            reelPosition = CalcReelPosition(stopCandidate[1], -1);
        }

        if (stopCandidate[0] != NONE && stopCandidate[1] != NONE && stopCandidate[0] > stopCandidate[1]) //BIGかREGの時に7とBARで近い方に止める←シンボルの並びが変わったら不具合
        {
            reelPosition = CalcReelPosition(stopCandidate[1], -1);
        }

        return reelPosition;
    }



    private static bool ExclusionRangeDetermination(int maxExclusion, int minExclusion, int referencePosition, int range)
    {
        bool isExclusionRange = false;

        int absoluteRange = 0;
        int searchPosition = referencePosition;
        int add = 1;

        if(range < 0) 
        { 
            absoluteRange = range * -1;
            add = -1;
        }

        for(int  i = 0; i <= absoluteRange && isExclusionRange == false; i++)
        {
            if (searchPosition <= maxExclusion && searchPosition >= minExclusion)
            {
                isExclusionRange = true;
            }
            searchPosition = CalcReelPosition(searchPosition,add);
        }


        return isExclusionRange;
    }


    //書き途中
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

        if (selectReel == SelectReel.LEFT && nowDispSymbols[0,0] == 0) //仮置き
        {

        }



        return 0;
    }

}