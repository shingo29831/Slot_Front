using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using static Constants;
using static GameMachine.Model.Setting;


//このクラスはゲームの内部処理を担当する
namespace GameMachine.Model
{



    public class Game
    {
        static int hasCoin = 0; //テスト前0
        static int increasedCoin = 0;
        static int lastBonusCount = 0;
        static int preBonusCoin = 0;

        static bool nextBonusFlag = false;
        static bool inBonus = false;
        public static bool hitBonusFlag = false; //test前false

        public static sbyte StopReelCount { get; set; } = 0; //テスト前0


        private static sbyte nowLeftReel = 0;
        private static sbyte nowCenterReel = 0;
        private static sbyte nowRightReel = 0;


        private static sbyte nextLeftReel = 0;
        private static sbyte nextCenterReel = 0;
        private static sbyte nextRightReel = 0;

        private static bool leftReelMoving = true;
        private static bool centerReelMoving = true; //テスト前:true
        private static bool rightReelMoving = true; //テスト前:true


        private static Roles nowRole = Roles.NONE; //テスト前:NONE
        private static Roles nowBonus = Roles.NONE; //テスト前:NONE

        public static Roles establishedRole = Roles.NONE;

        private static readonly Symbols[] SYMBOLS_ARRAY = { Symbols.BELL, Symbols.REPLAY, Symbols.WATERMELON, Symbols.CHERRY, Symbols.BAR, Symbols.SEVEN, Symbols.REACH };
        private static readonly Roles[] ROLES_ARRAY = { Roles.BELL, Roles.REPLAY, Roles.WATERMELON, Roles.WEAK_CHERRY, Roles.STRONG_CHERRY, Roles.VERY_STRONG_CHERRY };
        private static readonly Positions[] POSITIONS_ARRAY = { Positions.BOTTOM, Positions.MIDDLE, Positions.TOP };
        private static readonly Reels[] REELS_ARRAY = { Reels.LEFT, Reels.CENTER, Reels.RIGHT };
        public static readonly Lines[] LINES_ARRAY = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
        private static readonly Roles allRoles = Roles.BELL | Roles.REPLAY | Roles.WATERMELON | Roles.WEAK_CHERRY | Roles.STRONG_CHERRY | Roles.VERY_STRONG_CHERRY;


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

        //
        //
        //
        //クラス外から使用可能なメソッドここから

        public static void SetNowRole(Roles role) { Game.nowRole = role; }
        public static Roles GetNowRole() { return Game.nowRole; }
        public static void SetEstablishedRole(Roles role) { Game.establishedRole = role; }
        public static Roles GetEstablishedRole() { return Game.establishedRole; }

        public static bool GetInBonus() { return Game.inBonus; }

        public static void SetNowBonus(Roles role) { Game.nowBonus = role; }
        public static Roles GetNowBonus() { return Game.nowBonus; }

        public static bool IncreaseHasCoin(int increase) { Game.hasCoin += increase; return true; }
        public static int GetHasCoin() { return Game.hasCoin; }

        public static bool SetHasCoin(int hasCoin) { Game.hasCoin = hasCoin; return true; }

        public static void SetPreBonusCoin(int coin) { Game.preBonusCoin = coin; }
        public static int GetPreBonusCoin() { return Game.preBonusCoin; }

        public static int GetIncreasedCoin() { return increasedCoin; }

        public static int GetLastBonusCount() { return Game.lastBonusCount ; }

        public static void ResetBonusCount()
        {
            increasedCoin = 0;
        }



        //現在ポジションをセットする
        public static void SetNowReelPosition(in Reels selectReel, sbyte position)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    nowLeftReel = position;
                    break;
                case Reels.CENTER:
                    nowCenterReel = position;
                    break;
                case Reels.RIGHT:
                    nowRightReel = position;
                    break;
            }
        }




        public static byte GetSuggestionImage(byte expected)
        {
            int totalWeight = 0;
            for ( byte i = 0; i < expected ; i++)
            {
                totalWeight += GetImageProbability( i );
            }
            Random rnd = new Random();
            int rndNum = rnd.Next(0, totalWeight);
            int sumWeight = 0;
            byte imageIndex = 0;
            for (byte i = 0;i < expected; i++)
            {
                sumWeight += GetImageProbability( i );
                if ( rndNum < sumWeight)
                {
                    imageIndex = i;
                    break;
                }
            }
            return (byte)(imageIndex + 1);

        }



        //リールの今のポジションを取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHT
        public static sbyte GetNowReelPosition(in Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    return nowLeftReel;

                case Reels.CENTER:
                    return nowCenterReel;

                case Reels.RIGHT:
                    return nowRightReel;

            }
            return 0;
        }

        //ボーナスの状態を変更させる、
        public static void SwitchingBonus()
        {
            //揃ったボーナス役ごとにボーナスに突入させる
            if (establishedRole == Roles.BIG | establishedRole == Roles.REGULAR)
            {
                nowBonus = establishedRole;
                inBonus = true;
                hitBonusFlag = false;
            }

            //現在のボーナスごとの終了条件を達成した時にボーナスを終了させる
            if ((nowBonus == Roles.BIG && lastBonusCount >= 150) || (nowBonus == Roles.REGULAR && lastBonusCount >= 75))
            {
                inBonus = false;
                nowBonus = Roles.NONE;
                lastBonusCount = 0;
                hitBonusFlag = false;
            }

            //次のボーナスフラグがたっている時に次のボーナスに再突入させる
            if (nextBonusFlag && inBonus == false)
            {
                nextBonusFlag = false;
                inBonus = true;
                nowBonus = Roles.BIG;
            }
            else if(nextBonusFlag == false && inBonus == false)
            {
                increasedCoin = 0;
            }

            //ボーナス突入していなかったらボーナスコインのカウントを止める
            if (inBonus == false)
            {
                increasedCoin = 0;
            }
        }


        public static sbyte OnPushedStopBtn(in Reels selectReel, sbyte nowReelPosition)
        {
            SetNowReelPosition(selectReel, nowReelPosition);
            CalcNextReelPosition(selectReel); //次の位置を算出
            if (StopReelCount == 3)
            {

                HitEstablishedRoles();
                BonusLottery(); //ボーナス抽選を行う
                CalcCoinReturned();
            }
            return CalcNextReelPosition(selectReel);
        }

        //リールの次のポジションを取得する
        public static sbyte GetNextReelPosition(in Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    return nextLeftReel;

                case Reels.CENTER:
                    return nextCenterReel;

                case Reels.RIGHT:
                    return nextRightReel;

            }
            return 0;
        }


        //リールの現在の位置をオーバフローさせないように計算する 第一引数に移動前,第二引数に移動数を代入
        public static sbyte CalcReelPosition(in sbyte reelPosition, sbyte move)
        {
            sbyte calcReelPosition = reelPosition;
            calcReelPosition += move;
            if (calcReelPosition < 0)
            {
                calcReelPosition += 21;
            }
            if (calcReelPosition > 20)
            {
                calcReelPosition %= 21;
            }

            return calcReelPosition;
        }








        //リールの現在位置を取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHTとTOP,MIDDLE,BOTTOM
        public static sbyte GetReelPositionForPosition(in Reels selectReel, Positions position)
        {
            sbyte reelPosition = NONE;
            sbyte NextReelPosition = GetNextReelPosition(selectReel);
            switch (position)
            {
                case Positions.TOP:
                    reelPosition = CalcReelPosition(NextReelPosition, 2);
                    break;

                case Positions.MIDDLE:
                    reelPosition = CalcReelPosition(NextReelPosition, 1);
                    break;

                case Positions.BOTTOM:
                    reelPosition = CalcReelPosition(NextReelPosition, 0);
                    break;
            }

            return reelPosition;
        }

        //指定した位置で取得する
        public static sbyte GetReelPositionForPosition(in Reels selectReel, Positions position, sbyte reelPosition)
        {
            GetSymbolForPosition(selectReel, position, reelPosition);

            switch (position)
            {
                case Positions.TOP:
                    reelPosition = CalcReelPosition(reelPosition, 2);
                    break;

                case Positions.MIDDLE:
                    reelPosition = CalcReelPosition(reelPosition, 1);
                    break;

                case Positions.BOTTOM:
                    reelPosition = CalcReelPosition(reelPosition, 0);
                    break;
            }

            return reelPosition;
        }




        //ボーナス抽選開始後実行する"ボーナス抽選関数" 役ごとのrolesBonusProbability配列に設定された確率に合わせて抽選する
        //ボーナス抽選だけでボーナスにはまだ突入させない
        public static void BonusLottery()
        {

            Random rnd = new Random();
            byte rndNum = (byte)rnd.Next(0, 100);  //0以上100未満の値がランダムに出力
            byte sumProbability = 0;

            foreach (Roles role in ROLES_ARRAY)
            {
                if (role == Roles.BIG | role == Roles.REGULAR | hitBonusFlag) //役がBIGとREGとボーナスフラグが既にたっていたらボーナス抽選しない
                {
                    break;
                }
                sumProbability += GetRoleBonusProbability(role);
                if (rndNum < Setting.GetRoleBonusProbability(role) && role == nowRole && inBonus) //ボーナス突入済みだった場合は次のボーナスフラグを上げる
                {
                    nextBonusFlag = true;
                }

                if (rndNum < Setting.GetRoleBonusProbability(role) && role == nowRole)
                {
                    hitBonusFlag = true;
                    SelectBonusLottery();
                }
                if (role == nowRole) { break; } //現在の役が来たら繰り返し終了
            }

        }







        //役の抽選の関数
        public static void HitRolesLottery()
        {

            nowRole = Roles.NONE;
            int sumWeight = 0;
            int totalWeight = GetTotalWeight();

            Random rnd = new Random();
            int rndNum = rnd.Next(0, totalWeight); //0～totalWeight-1の乱数

            foreach (Roles role in ROLES_ARRAY)
            {
                sumWeight += GetRoleWeight(role);
                if (rndNum < sumWeight)
                {
                    nowRole = role;
                    break;
                }
            }

            if (inBonus)
            {
                nowRole = Roles.BELL;
            }

            //ボーナス以外の役が揃わない時、ボーナスがあたったフラグがあったら役をボーナスにする
            if (nowRole == Roles.NONE && hitBonusFlag)
            {
                nowRole = nowBonus;
            }
        }



        //どの役が当選しているか設定する
        public static void HitEstablishedRoles()
        {
            establishedRole = Roles.NONE;
            foreach (Lines line in LINES_ARRAY)
            {
                byte sevenNum = 0;
                byte barNum = 0;
                bool hasBar = false;
                if (GetSymbolForLine(Reels.LEFT, line, nextLeftReel).HasFlag(GetSymbolForLine(Reels.CENTER, line, nextCenterReel) | GetSymbolForLine(Reels.RIGHT, line, nextRightReel)))
                {
                    Symbols symbols = GetSymbolForLine(Reels.LEFT, line, nextLeftReel);
                    establishedRole = GetRoleEstablishedBySymbols(symbols, line);
                }
                foreach (Reels reel in REELS_ARRAY)
                {
                    sbyte nextReelPosition = GetNextReelPosition(reel);

                    if (GetSymbolForLine(reel, line, nextReelPosition).HasFlag(Symbols.SEVEN))
                    {
                        sevenNum++;
                    }
                    if (GetSymbolForLine(reel, line, nextReelPosition).HasFlag(Symbols.BAR))
                    {
                        barNum++;
                        hasBar = true;
                    }
                }

                if (barNum == 3)
                {
                    establishedRole = Roles.BIG;
                    hitBonusFlag = false;
                }
                else if (sevenNum == 2 && hasBar && GetSymbolForLine(Reels.LEFT, line, nextLeftReel) == Symbols.SEVEN && GetSymbolForLine(Reels.CENTER, line, nextCenterReel) == Symbols.SEVEN)
                {
                    establishedRole = Roles.REGULAR;
                    hitBonusFlag = false;
                }

            }
            foreach (Positions position in POSITIONS_ARRAY)
            {
                sbyte nextReelPosition = GetNextReelPosition(Reels.LEFT);
                if (GetSymbolForPosition(Reels.LEFT, position, nextReelPosition) == Symbols.CHERRY && !((Roles.BIG | Roles.REGULAR).HasFlag(nowRole)) && (Roles.STRONG_CHERRY | Roles.VERY_STRONG_CHERRY).HasFlag(establishedRole))
                {
                    establishedRole = Roles.WEAK_CHERRY;
                }
            }
        }






        //選択したリールの次のリールポジションを返す
        public static sbyte CalcNextReelPosition(in Reels selectReel)
        {
            sbyte reelPosition = NONE;

            sbyte nowReelPosition = GetNowReelPosition(selectReel);
            bool isFindedReelPosition = false;
            bool isFindedProxyReelPosition = false;
            //nowReelPositionの5つ先まで止まるため5を代入
            for (sbyte gapNowReelPosition = 4; gapNowReelPosition >= 0; gapNowReelPosition--)
            {
                sbyte searchReelPosition = CalcReelPosition(nowReelPosition, gapNowReelPosition); //現在位置から4～0の位置
                bool isExclusion = GetIsExclusion(selectReel, searchReelPosition);
                if (isExclusion == false && GetIsAchieveRole(selectReel, searchReelPosition)) //役が揃うシンボルが来るか
                {
                    reelPosition = searchReelPosition;
                    isFindedReelPosition = true;
                }


                if (isExclusion == false && GetIsReachRole(selectReel, searchReelPosition) && isFindedReelPosition == false) //リーチ目になるか、また次の位置が決まったか
                {
                    reelPosition = searchReelPosition;
                    isFindedProxyReelPosition = true;
                }
                if (isExclusion == false && (isFindedReelPosition | isFindedProxyReelPosition) == false)
                {
                    reelPosition = searchReelPosition;
                }

            }

            SetNextReelPosition(selectReel, reelPosition);
            return reelPosition;
        }

        //リールの次のリール位置を決める
        public static void SetNextReelPosition(in Reels selectReel, sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    nextLeftReel = reelPosition;
                    break;
                case Reels.CENTER:
                    nextCenterReel = reelPosition;
                    break;
                case Reels.RIGHT:
                    nextRightReel = reelPosition;
                    break;
            }

        }



        //リールのシンボルの並びをReels.LEFT,Reels.CENTER,Reels.RIGHTで選択し取得する
        public static Symbols[] GetReelOrder(in Reels selectReel)
        {
            Symbols[] reelOrder = { Symbols.NONE };
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
        public static void SetReelMoving(in Reels selectReel, in bool isMoving)
        {
            switch (selectReel)
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
            if (isMoving == false)
            {
                StopReelCount++;
            }


        }




        //リールを全て動いている判定にする
        public static void ResetReelsMoving()
        {
            SetReelMoving(Reels.LEFT, true);
            SetReelMoving(Reels.CENTER, true);
            SetReelMoving(Reels.RIGHT, true);
            StopReelCount = 0;
        }







        //クラス外からアクセス可能なメソッドここまで
        //
        //
        //


        //ボーナス抽選当選後、実行するビックボーナスまたはレギュラーボーナスを決定する
        //決定はSetting.bigProbabilityを元に決定
        public static void SelectBonusLottery()
        {
            Random rnd = new Random();
            byte bigProbability = Setting.GetBigProbability();

            byte rndNum = (byte)rnd.Next(0, 100);  //0以上99以下の値がランダムに出力

            if (rndNum < bigProbability)
            {
                nowBonus = Roles.BIG;
            }
            else if (rndNum >= bigProbability)
            {
                nowBonus = Roles.REGULAR;
            }
        }


        public static void CalcCoinCollection()
        {
            if (inBonus)
            {
                hasCoin -= 2;
                increasedCoin = hasCoin - preBonusCoin;
            }
            else if (establishedRole == Roles.REPLAY)
            {

            }
            else if (!inBonus)
            {
                hasCoin -= 3;
            }
        }



        //払い出しコイン枚数を持ちコインに加える
        public static void CalcCoinReturned()
        {
            if (inBonus && (establishedRole != Roles.BIG || establishedRole != Roles.REGULAR))
            {
                hasCoin += 15;
                increasedCoin = hasCoin - preBonusCoin;
                lastBonusCount += 15;
            }
            else if (!inBonus && establishedRole != Roles.NONE)
            {
                hasCoin += GetRoleReturn(establishedRole);
            }


        }





        //選択されたリールに表示されている全てのシンボルのビットフラグを取得する
        private static Symbols GetNextReelSymbols(in Reels selectReel)
        {
            Symbols nextReelSymbols = Symbols.NONE;
            Symbols[] reelOrder = GetReelOrder(selectReel);

            nextReelSymbols = reelOrder[GetReelPositionForPosition(selectReel, Positions.TOP, GetNextReelPosition(selectReel))];
            nextReelSymbols |= reelOrder[GetReelPositionForPosition(selectReel, Positions.MIDDLE, GetNextReelPosition(selectReel))];
            nextReelSymbols |= reelOrder[GetReelPositionForPosition(selectReel, Positions.BOTTOM, GetNextReelPosition(selectReel))];

            return nextReelSymbols;
        }









        //一種のシンボルで成立する役を返す
        private static Roles GetRoleEstablishedBySymbols(Symbols symbols, Lines line)
        {
            switch (symbols)
            {
                case Symbols.BELL:
                    return Roles.BELL;
                case Symbols.REPLAY:
                    return Roles.REPLAY;
                case Symbols.WATERMELON:
                    return Roles.WATERMELON;
                case Symbols.CHERRY:
                    if (line == Lines.middleToMiddle)
                    {
                        return Roles.VERY_STRONG_CHERRY;
                    }
                    return Roles.STRONG_CHERRY;
                case Symbols.BAR:
                case Symbols.SEVEN:
                    hitBonusFlag = false; //ボーナスがあたったフラグを下げてボーナスに突入させる
                    return Roles.BIG;
                default:
                    return Roles.NONE;

            }

        }


        //リールのPositionsからシンボルを取得する
        private static Symbols GetNextSymbolForPosition(in Reels selectReel, Positions position)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);

            sbyte reelPosition = GetReelPositionForPosition(selectReel, position);
            return reelOrder[reelPosition];
        }

        //指定したリールの位置で取得する
        private static Symbols GetSymbolForPosition(in Reels selectReel, in Positions position, sbyte reelPosition)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);
            sbyte gap = 0;
            switch (position)
            {
                case Positions.TOP:
                    gap = 2;
                    break;
                case Positions.MIDDLE:
                    gap = 1;
                    break;
                case Positions.BOTTOM:
                    gap = 0;
                    break;

            }

            sbyte getReelPosition = CalcReelPosition(reelPosition, gap);
            return reelOrder[getReelPosition];
        }






        //選択したリールの位置が除外か否かを返す
        //getIsExclusion(リールの選択,リールの位置)
        private static bool GetIsExclusion(in Reels selectReel, sbyte reelPosition)
        {

            Symbols[] reelOrder = GetReelOrder(selectReel);



            //以下は弱チェリーを回避する処理

            sbyte searchPosition = reelPosition;

            //左リールで、かつチェリーシンボルがボーナス当選中以外のとき
            for (sbyte i = 0; i < 3 && selectReel == Reels.LEFT && GetSymbolsAccordingRole().HasFlag(Symbols.CHERRY) == false && (~(Roles.BIG | Roles.REGULAR)).HasFlag(nowRole); i++) //
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
        private static Symbols GetExclusionSymbolsForPosition(in Reels selectReel, Positions position)
        {
            Symbols exclusionSymbols = Symbols.NONE;

            Symbols reachSymbols = GetReachSymbolsForMovingReelsPosition(position);
            //リーチ目用で使用
            Symbols centerSymbols = GetNextReelSymbols(Reels.CENTER);

            //中段チェリー回避
            if(nowRole != Roles.VERY_STRONG_CHERRY && position == Positions.MIDDLE){
                exclusionSymbols = Symbols.CHERRY;
            }

            switch (nowRole)
            {
                case Roles.BELL:
                    //リーチのビットフラグからベルのフラグを消している
                    exclusionSymbols |= reachSymbols & ~Symbols.BELL;

                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
                    break;


                case Roles.REPLAY:
                    exclusionSymbols = reachSymbols & ~Symbols.REPLAY;

                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
                    break;


                case Roles.WATERMELON:
                    exclusionSymbols = reachSymbols & ~Symbols.WATERMELON;
                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
           
                    break;


                case Roles.WEAK_CHERRY:
                    if (selectReel == Reels.LEFT) //弱チェリーが当選した時、左リールでは除外用ビットフラグからチェリーを消す
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols & ~Symbols.CHERRY;
                    }
                    if (selectReel == Reels.CENTER || selectReel == Reels.RIGHT) //弱チェリーが当選した時、中・右リールではリーチになった全てのシンボルをビットフラグに入れる
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols;
                    }
                    if (selectReel == Reels.LEFT && position == Positions.MIDDLE) //弱チェリーが当選した時、左リール中段にチェリーが来ないようにする
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols;
                    }
                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
                    break;


                case Roles.STRONG_CHERRY:
                    if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT) && (position == Positions.TOP || position == Positions.BOTTOM)) //左・右リールで上・下段の時、除外用ビットフラグのチェリーを下げる
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols & ~Symbols.CHERRY;
                    }
                    if ((selectReel == Reels.LEFT || selectReel == Reels.RIGHT) && position == Positions.MIDDLE) //強チェリーが当選した時、左または右リールの中段にチェリーが来ないようにする
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols;
                    }
                    if (selectReel == Reels.CENTER) //中央リールでは、どこにチェリーが来てもよい
                    {
                        exclusionSymbols = exclusionSymbols | reachSymbols & ~Symbols.CHERRY;
                    }
                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
                    break;


                case Roles.VERY_STRONG_CHERRY:
                    exclusionSymbols = reachSymbols & ~Symbols.CHERRY; //配置的に強チェリーになっても良いためどこにチェリーがきても良い

                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols = exclusionSymbols | (Symbols.SEVEN | Symbols.BAR);
                    }
                    break;




                case Roles.REGULAR:
                    exclusionSymbols = reachSymbols & ~(Symbols.SEVEN | Symbols.BAR);
                    if (reachSymbols.HasFlag(Symbols.BAR))
                    {
                        exclusionSymbols = reachSymbols & ~Symbols.SEVEN; //除外用ビットフラグからSEVENフラグを消す BARを揃えるとBBになるため注意
                    }
                    if (reachSymbols.HasFlag(Symbols.SEVEN))
                    {
                        exclusionSymbols = reachSymbols & ~Symbols.BAR;
                    }
                    //77BAR or BAR77用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.SEVEN) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールに7が来ていた時
                    {
                        exclusionSymbols = reachSymbols & ~Symbols.SEVEN;
                    }

                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時でに中央リールのシンボルを選択する時は除外用ビットフラグからSEVENとBARを消す
                    {
                        exclusionSymbols = reachSymbols & ~(Symbols.SEVEN | Symbols.BAR);
                    }
                    if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.BAR) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールにBARが来ていた時
                    {
                        exclusionSymbols = reachSymbols & ~(Symbols.BAR | Symbols.SEVEN); //7とBARを除外用ビットフラグから消す
                    }
                    break;


                case Roles.BIG:
                    exclusionSymbols = reachSymbols & ~(Symbols.SEVEN | Symbols.BAR);
                    if (reachSymbols.HasFlag(Symbols.BAR))
                    {
                        exclusionSymbols = reachSymbols & ~Symbols.BAR;
                    }
                    if (reachSymbols.HasFlag(Symbols.SEVEN))
                    {
                        exclusionSymbols = reachSymbols & ~Symbols.SEVEN;

                    }




                    //リーチ目用処理
                    if (reachSymbols.HasFlag(Symbols.REACH) && selectReel == Reels.CENTER) //リーチ目が出そうな時でに中央リールのシンボルを選択する時は除外用ビットフラグからBARを消す
                    {
                        exclusionSymbols = reachSymbols | Symbols.SEVEN & ~Symbols.BAR;
                    }

                    if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.SEVEN) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールに7が来ていた時
                    {
                        exclusionSymbols = reachSymbols | Symbols.SEVEN & ~Symbols.BAR;
                    }

                    if (reachSymbols.HasFlag(Symbols.REACH) && centerSymbols.HasFlag(Symbols.BAR) && centerReelMoving == false) //リーチ目が出そうな時に中央リールが停止状態で中央リールにBARが来ていた時
                    {
                        exclusionSymbols = reachSymbols & ~(Symbols.BAR | Symbols.SEVEN); //7とBARを除外用ビットフラグから消す
                    }
                    break;

                case Roles.NONE:
                    exclusionSymbols = reachSymbols;
                    if (reachSymbols.HasFlag(Symbols.SEVEN))
                    {
                        exclusionSymbols |= Symbols.SEVEN | Symbols.BAR;

                    }
                    if (reachSymbols.HasFlag(Symbols.BAR)) //
                    {
                        exclusionSymbols |= Symbols.SEVEN | Symbols.BAR;

                    }
                    if (reachSymbols.HasFlag(Symbols.REACH)) //リーチ目が出そうな時はSEVENとBARを除外シンボルフラグを建てる
                    {
                        exclusionSymbols |= Symbols.SEVEN | Symbols.BAR;
                    }

                    break;
            }


            return exclusionSymbols;
        }




        //役を達成できるか否か返す
        private static bool GetIsAchieveRole(in Reels selectReel, sbyte reelPosition)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);


            Symbols topAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.TOP);
            Symbols middleAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.MIDDLE);
            Symbols bottomAchieveRoleSymbols = GetAchieveRoleSymbolsForPosition(selectReel, Positions.BOTTOM);

            if (nowRole == Roles.VERY_STRONG_CHERRY)
            {
                topAchieveRoleSymbols = Symbols.NONE;
                bottomAchieveRoleSymbols = Symbols.NONE;
            }

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


        //Positionsのところにいれることが可能なシンボルを返す
        //一つ目は役を達成できるシンボルを返す
        //二つ目は候補のライン上のシンボルを返す
        //三つ目はリーチ上の役を成立できるシンボルを返す
        private static Symbols GetAchieveRoleSymbolsForPosition(in Reels selectReel, Positions position)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);
            Symbols exclusionSymbols = Symbols.NONE;

            Reels stopReels = (Reels.LEFT | Reels.CENTER | Reels.RIGHT) & GetMovingReels();
            switch (StopReelCount)
            {
                case 0:
                    return GetSymbolsAccordingRole();
                case 1:
                    if ((Reels.LEFT | Reels.CENTER).HasFlag(selectReel) && nowRole == Roles.REGULAR && GetPositionsToReach(selectReel).HasFlag(position)) //中リールでレギュラー役が来た時、リーチにさせれるpositionだった時
                    {
                        return Symbols.SEVEN;
                    }
                    //if ((Reels.LEFT | Reels.CENTER).HasFlag(selectReel) && nowRole == Roles.REGULAR && //左または右リールでレギュラー役の時
                    //    GetIsStopBarForLines((Reels.LEFT | Reels.RIGHT) & ~selectReel, position) && GetPositionsToReach(selectReel).HasFlag(position)) //かつ止まっているリールにバーがあった時でリーチにさせれるpositionだった時
                    //{
                    //    return Symbols.SEVEN;
                    //}
                    if (GetPositionsToReach(selectReel).HasFlag(position)) //リーチにさせれるpositionだった時
                    {
                        return GetSymbolsAccordingRole();
                    }
                    break;
                case 2:
                    if (GetPositionsToHit().HasFlag(position))
                    {
                        return GetSymbolsCanArcheveReachRole();
                    }

                    break;
            }




            return exclusionSymbols;
        }



        //バーが止まっているか否か返す　二つ目のリールの処理用
        //(一つ目ののリール,二つ目のリールのPositions)
        private static bool GetIsStopBarForLines(Reels selectStopReel, Positions position)
        {
            try
            {
                Symbols[] reelOrder = GetReelOrder(selectStopReel);
                Positions stopReelBarPosition = GetPositionsForLines(selectStopReel, GetCandidateLines());
                if (selectStopReel != Reels.CENTER && reelOrder[GetReelPositionForPosition(selectStopReel, stopReelBarPosition)] == Symbols.BAR)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
            return false;
        }


        private static bool GetIsReachRole(in Reels selectReel, sbyte reelPosition)
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
        private static Symbols GetReachRoleSymbolsForPosition(in Reels selectReel, Positions position)
        {
            Symbols reachRoleSymbols = Symbols.NONE;

            foreach (Lines line in LINES_ARRAY)
            {
                if (GetReachLinesCanArcheveRole().HasFlag(line) && GetPositionsForLines(selectReel, line).HasFlag(position))
                {
                    reachRoleSymbols = GetSymbolForReachRoleLine(selectReel, line);
                }
                if (GetCandidateLines().HasFlag(line) && GetPositionsForLines(selectReel, line).HasFlag(position))
                {
                    reachRoleSymbols = GetSymbolForReachRoleLine(selectReel, line);
                }
            }

            return reachRoleSymbols;

        }

        //リーチ目の条件となるシンボルを返す
        private static Symbols GetSymbolForReachRoleLine(in Reels selectReel, Lines line)
        {
            Reels stopReels = (Reels.LEFT | Reels.CENTER | Reels.RIGHT) & ~GetMovingReels();

            if (StopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.REACH && GetSymbolForLine(Reels.CENTER, line) == Symbols.BAR)
            {
                return Symbols.SEVEN | Symbols.BAR;
            }
            if (StopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.REACH && GetSymbolForLine(Reels.CENTER, line) == Symbols.SEVEN)
            {
                return Symbols.NONE;
            }
            if (StopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.BAR)
            {
                return Symbols.SEVEN;
            }
            if (StopReelCount == 2 && stopReels.HasFlag(Reels.CENTER) && GetReachSymbolForLine(selectReel, line) == Symbols.SEVEN)
            {
                return Symbols.NONE;
            }
            if (StopReelCount == 2 && selectReel == Reels.CENTER && GetReachSymbolForLine(selectReel, line) == Symbols.REACH)
            {
                return Symbols.BAR;
            }
            if (StopReelCount == 2 && selectReel == Reels.CENTER && (Symbols.SEVEN | Symbols.BAR).HasFlag(GetReachSymbolForLine(selectReel, line))) //停止リールが2つで中リールの時リーチなっていないシンボルを返す
            {
                return (Symbols.SEVEN | Symbols.BAR) & ~GetReachSymbolForLine(selectReel, line);
            }
            if (StopReelCount == 1 && (Roles.BIG | Roles.REGULAR).HasFlag(nowRole)) //停止リールが1つでnowRoleがBIGかREGの時
            {
                return Symbols.SEVEN | Symbols.BAR;
            }

            return Symbols.NONE;


        }






        //現在のリールの位置からの差を返す 探索位置がNONEだった場合はNONEを返す
        private static sbyte CalcGapNowReelPosition(in Reels selectReel, sbyte reelSearchPosition)
        {
            sbyte gap = 0;
            sbyte reelPosition = GetNowReelPosition(selectReel);
            while (reelPosition != reelSearchPosition && reelSearchPosition != NONE)
            {
                reelPosition = CalcReelPosition(reelPosition, 1);
                gap++;
            }

            if (reelPosition == NONE)
            {
                gap = NONE;
            }

            return gap;
        }







        //現在の役を成立させれるシンボルをビットフラグで返す
        //複数のシンボルが入っていても一つでも役を成立させれるならtrueを返す
        //現在の役が揃うか隣接しているシンボルを元に求めること
        private static bool GetIsEstablishingRole(Symbols searchSymbols)//あとで変更
        {

            Symbols[] seachSymbolsArray = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
            foreach (Symbols symbol in SYMBOLS_ARRAY)
            {
                if (searchSymbols.HasFlag(symbol) && GetSymbolsAccordingRole().HasFlag(symbol)) //どのシンボルが入っているか見ていき同時に役を成立させるか判定する
                {
                    return true;
                }
            }


            return false;
        }

        //役を成立させる条件のLinesを返す
        private static Lines GetLinesAccordingRole()
        {
            Lines lines = Lines.NONE;
            foreach (Lines line in LINES_ARRAY) //全てのLinesビットフラグを上げる
            {
                lines |= line;
            }
            switch (nowRole)
            {
                case Roles.STRONG_CHERRY:
                    return lines & ~Lines.middleToMiddle;

                case Roles.VERY_STRONG_CHERRY:
                    return Lines.middleToMiddle;

                default:
                    return lines;
            }
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



        private static Positions GetPositionsToHit()
        {
            if (StopReelCount < 2)
            {
                return Positions.NONE;
            }
            Reels movingReel = GetMovingReels();
            Lines reachLines = GetReachLinesCanArcheveRole();
            Positions toHitPositions = GetPositionsForLines(movingReel, reachLines);
            return toHitPositions;
        }


        //当選した役をリーチにさせる、選択したリールのPositionsをビットフラグで返す
        //ない場合とリールが2つ以上止まっている時はNONEで返す
        private static Positions GetPositionsToReach(in Reels selectReel)
        {
            if (StopReelCount >= 2)
            {
                return Positions.NONE;
            }

            Lines candidateLines = GetCandidateLines();
            Positions candidatePositions = GetPositionsForLines(selectReel, candidateLines);



            return candidatePositions;
        }


        //選択したリール上でLines上のPositionsを返す
        //ラインを第二引数にいれる
        private static Positions GetPositionsForLines(in Reels selectReel, Lines candidateLines)
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


        //選択したリールから役を成立させる候補のラインを返す
        private static Lines GetCandidateLines(in Reels selectReel)
        {
            if (selectReel == Reels.NONE)
            {
                return Lines.NONE;
            }

            Symbols[] nextStopReelOrder = GetReelOrder(selectReel);
            sbyte nextStopReelPosition = GetNextReelPosition(selectReel);
            Positions stopReelSymbolPositions = Positions.NONE;


            //役を成立させるシンボルがはいっていたらPositionsビットフラグを上げる
            if (GetIsEstablishingRole(nextStopReelOrder[CalcReelPosition(nextStopReelPosition, 2)]))
            {
                stopReelSymbolPositions = Positions.TOP;
            }

            if (GetIsEstablishingRole(nextStopReelOrder[CalcReelPosition(nextStopReelPosition, 1)]))
            {
                stopReelSymbolPositions |= Positions.MIDDLE;
            }

            if (GetIsEstablishingRole(nextStopReelOrder[nextStopReelPosition]))
            {
                stopReelSymbolPositions |= Positions.BOTTOM;
            }

            Lines reachCandidateLines = Lines.NONE;

            switch (selectReel)
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

        //リーチの候補のラインを返す
        private static Lines GetCandidateLines()
        {
            if (StopReelCount != 1)
            {
                return Lines.NONE;
            }

            Reels stopReel = (Reels.LEFT | Reels.CENTER | Reels.RIGHT) & ~GetMovingReels();

            return GetCandidateLines(stopReel);


        }





        //役を成立させれるリーチのラインを返す
        private static Lines GetReachLinesCanArcheveRole()
        {

            Lines reachLine = Lines.NONE;

            foreach (Lines line in LINES_ARRAY)
            {
                if (GetIsReachRoleLine(line))
                {
                    reachLine |= line;
                }
            }

            return reachLine;
        }

        //リーチのLinesであるか否かを取得する
        private static bool GetIsReachRoleLine(Lines line)
        {
            Reels movingReel = GetLastMovingReel();
            Reels[] stopedReelArray = { Reels.NONE, Reels.NONE };
            sbyte index = 0;
            foreach (Reels reel in REELS_ARRAY)
            {
                if (reel != movingReel && movingReel != Reels.NONE)
                {
                    stopedReelArray[index] = reel;
                    index++;
                }
            }

            if (GetLinesAccordingRole().HasFlag(line) && GetCandidateLines(stopedReelArray[0]).HasFlag(line) && GetCandidateLines(stopedReelArray[1]).HasFlag(line)) //当選した役が揃うLineか＆Lineが、止まっているリールのポジションから上がる候補のLine上にあるか　
            {
                return true;
            }

            return false;
        }


        //役を成立させれるシンボルを返す
        private static Symbols GetSymbolsCanArcheveReachRole()
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
        private static Symbols GetChangeToAvailableSymbol(Lines line)
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

            if (!(Roles.REGULAR | Roles.BIG).HasFlag(nowRole))
            {
                return GetSymbolsAccordingRole();
            }

            if (reachSymbol == Symbols.REACH && movingReel == Reels.LEFT && stopedSymbolArray[0] == Symbols.SEVEN)
            {
                return Symbols.SEVEN;
            }

            if (reachSymbol == Symbols.REACH && movingReel == Reels.CENTER)
            {
                return Symbols.SEVEN;
            }

            if (reachSymbol == Symbols.REACH && movingReel == Reels.RIGHT && stopedSymbolArray[1] == Symbols.SEVEN)
            {
                return Symbols.SEVEN;
            }

            if (nowRole == Roles.REGULAR && reachSymbol == Symbols.SEVEN)
            {
                return Symbols.BAR;
            }


            return reachSymbol;
        }



        //選択したリールとライン上にあるシンボルを返す
        private static Symbols GetSymbolForLine(in Reels selectReel, Lines line)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);
            Positions searchPosition = GetPositionsForLines(selectReel, line);
            return reelOrder[GetReelPositionForPosition(selectReel, searchPosition)];

        }

        public static Symbols GetSymbolForLine(in Reels selectReel, Lines line, in sbyte reelPosition)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);
            Positions searchPosition = GetPositionsForLines(selectReel, line);
            GetSymbolForPosition(selectReel, searchPosition, reelPosition);
            return GetSymbolForPosition(selectReel, searchPosition, reelPosition);

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
        private static Symbols GetReachSymbolsForPosition(in Reels selectReel, Positions searchPosition)
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
        private static Symbols GetReachSymbolForLine(in Reels selectReel, Lines line)
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
                switch (line)
                {
                    case Lines.upperToLower: //左上から右下のライン
                        lineSymbols = leftReelOrder[CalcReelPosition(nextLeftReel, 2)];
                        break;
                    case Lines.upperToUpper: //左上から右上のライン
                        lineSymbols = leftReelOrder[CalcReelPosition(nextLeftReel, 2)];
                        break;
                    case Lines.middleToMiddle: //左中から右中のライン
                        lineSymbols = leftReelOrder[CalcReelPosition(nextLeftReel, 1)];
                        break;
                    case Lines.lowerToLower: //左下から右下のライン
                        lineSymbols = leftReelOrder[nextLeftReel];
                        break;
                    case Lines.lowerToUpper: //左下から右上のライン
                        lineSymbols = leftReelOrder[nextLeftReel];
                        break;
                }
            }

            if (stoppedReels.HasFlag(Reels.CENTER)) //止まっているリールに中央リールが含まれている時
            {
                switch (line)
                {
                    case Lines.upperToLower:
                        lineSymbols |= centerReelOrder[CalcReelPosition(nextCenterReel, 1)]; // "|="で左辺と右辺のOR演算の結果を代入する
                        break;
                    case Lines.upperToUpper:
                        lineSymbols |= centerReelOrder[CalcReelPosition(nextCenterReel, 2)];
                        break;
                    case Lines.middleToMiddle:
                        lineSymbols |= centerReelOrder[CalcReelPosition(nextCenterReel, 1)];
                        break;
                    case Lines.lowerToLower:
                        lineSymbols |= centerReelOrder[nextCenterReel];
                        break;
                    case Lines.lowerToUpper:
                        lineSymbols |= centerReelOrder[CalcReelPosition(nextCenterReel, 1)];
                        break;
                }
            }

            if (stoppedReels.HasFlag(Reels.RIGHT)) //止まっているリールに右リールが含まれている時
            {
                switch (line)
                {
                    case Lines.upperToLower: //左上から右下のライン
                        lineSymbols |= rightReelOrder[nextRightReel];
                        break;
                    case Lines.upperToUpper: //左上から右上のライン
                        lineSymbols |= rightReelOrder[CalcReelPosition(nextRightReel, 2)];
                        break;
                    case Lines.middleToMiddle: //左中から右中のライン
                        lineSymbols |= rightReelOrder[CalcReelPosition(nextRightReel, 1)];
                        break;
                    case Lines.lowerToLower: //左下から右下のライン
                        lineSymbols |= rightReelOrder[nextRightReel];
                        break;
                    case Lines.lowerToUpper: //左下から右上のライン
                        lineSymbols |= rightReelOrder[CalcReelPosition(nextRightReel, 2)];
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
                case Symbols.SEVEN://フォールスルー　ここから上のcaseの場合以下の処理をbreakまで実行する
                    reachSymbol = lineSymbols;
                    break;
                case Symbols.BAR | Symbols.SEVEN:
                    reachSymbol = Symbols.REACH;
                    break;

                default:
                    reachSymbol = Symbols.NONE;
                    break;
            }

            if (lineSymbols.HasFlag(Symbols.BAR) && lineSymbols.HasFlag(Symbols.SEVEN)) //リーチ目になりそうな状態 
            {
                reachSymbol = Symbols.REACH; //REGが来た場合、優先で7を入れること BIGがきた場合は7以外を揃えること
            }



            return reachSymbol;
        }

        private static Reels GetLastMovingReel()
        {
            if (StopReelCount < 2)
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

}