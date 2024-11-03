using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using static Constants;

namespace GameMachine.Model
{


    public class Setting
    {

        public enum BonusesIndex : byte
        {
            BIG = 0,
            REGULAR = 1,
        }


        private static String pcID = "info";
        private static String userID = "";
        private static byte expected = 1;
        private static byte bigProbability = 30;//ボーナス当選後のBIGのx/100(x%)当選確率
        private static byte regProbability = (byte)(100 - bigProbability);
        private static readonly Roles[] roles = { Roles.BELL, Roles.REPLAY, Roles.WATERMELON, Roles.WEAK_CHERRY, Roles.STRONG_CHERRY, Roles.VERY_STRONG_CHERRY };
        private static readonly int totalWeight = 1000;
        private static int[] rolesWeight = { 70, 75, 27, 50, 4, 1, 0, 0 }; //x/1000の確率
                                                                           //ベル,リプレイ,スイカ,弱チェリー,強チェリー,中段チェリーの順で入れること
        private static int[] rolesWeightInBonus = { 0, 0, 50, 75, 8, 2, 0, 0 };
        private static byte[] rolesBonusProbability = { 0, 0, 10, 10, 25, 100 }; //x/100の確率(x%)
                                                                                 //ベル,リプレイ,スイカ,弱チェリー,強チェリー,中段チェリーの順で入れること

        private static readonly byte[] rolesReturn = { 9, 0, 9, 2, 2, 2, 15, 15 }; //通常時(ボーナスではない)の払い出し枚数
                                                                                 //ベル,リプレイ,スイカ,弱チェリー,強チェリー,中段チェリーの順で入れること
        private static readonly byte inBonusReturn = 15; //通常時(ボーナスではない)の払い出し枚数
                                                         //ベル,リプレイ,スイカ,弱チェリー,強チェリー,中段チェリーの順で入れること

        private static readonly byte[] imageProbability = { 55, 20, 10 , 8 , 5 , 2}; //台設定示唆リザルト画面確率


        public static byte GetImageProbability(byte index)
        {
            return imageProbability[index];
        }
        

        public static int GetTotalWeight()
        {
            return totalWeight;
        }

        public static int[] GetRolesWeight()
        {
            return rolesWeight;
        }

        public static byte GetRoleReturn(Roles role)
        {
            return rolesReturn[RolesToIndex(role)];
        }
        public static void SetExpected(byte expected) { Setting.expected = expected; }

        public static byte GetExpected() { return Setting.expected; }

        public static void SetUserID(String userID) { Setting.userID = userID; }
        public static String GetUserID() { return Setting.userID; }

        public static void MakeTableID()
        {
            byte[] hashValue = GenerateHash(GetMacAddress() + GetProductID());

            // ハッシュ値を表示
            pcID = BitConverter.ToString(hashValue).Replace("-", "");
        }

        public static void SetTableID(String pcID)
        {
            Setting.pcID = pcID;
        }

        public static String GetTableID()
        {
            return Setting.pcID;
        }

        public static byte[] GenerateHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        private static string GetMacAddress()
        {
            string macAddress = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // 有効であることを確認
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddress;
        }

        private static string GetProductID()
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + "wmic os get serialnumber")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = @"C:\Windows\System32\"
            };

            StringBuilder sb = new StringBuilder();
            Process p = Process.Start(processInfo);
            p.OutputDataReceived += (sender, args_) => sb.AppendLine(args_.Data);
            p.BeginOutputReadLine();
            p.WaitForExit();

            return sb.ToString();
        }



        //ボーナス当選時のBIGボーナス確率
        public static void SetBigProbability(byte probability) { bigProbability = probability; }
        public static byte GetBigProbability() { return bigProbability; }



        //台設定（台の確率）を変えるメソッド
        public static void WeightingProbabilityes(sbyte expected) //重み付け
        {
            switch (expected)
            {
                case 1:
                    SetRoleBonusProbability(Roles.WATERMELON, 10);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 10);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 25);
                    break;
                case 2:
                    SetRoleBonusProbability(Roles.WATERMELON, 11);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 11);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 25);
                    break;
                case 3:
                    SetRoleBonusProbability(Roles.WATERMELON, 12);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 12);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 26);
                    break;
                case 4:
                    SetRoleBonusProbability(Roles.WATERMELON, 13);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 13);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 27);
                    break;
                case 5:
                    SetRoleBonusProbability(Roles.WATERMELON, 14);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 14);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 29);
                    break;
                case 6:
                    SetRoleBonusProbability(Roles.WATERMELON, 15);
                    SetRoleBonusProbability(Roles.WEAK_CHERRY, 14);
                    SetRoleBonusProbability(Roles.STRONG_CHERRY, 30);
                    break;


            }
        }


        //通常時の役の確率の重みweight/1000
        public static void SetRoleWeight(Roles role, int weight)
        {
            rolesWeight[RolesToIndex(role)] = weight;
        }

        public static int GetRoleWeight(Roles role)
        {
            return rolesWeight[RolesToIndex(role)];
        }


        //ボーナス時の役の確率の重みweight/1000
        public static void SetRoleWeightInBonus(Roles role, int weight)
        {
            rolesWeightInBonus[RolesToIndex(role)] = weight;
        }

        public static int GetRoleWeightInBonus(Roles role)
        {
            if (role == Roles.BELL)
            {
                return 100 - (GetRoleWeightInBonus(Roles.WATERMELON) + GetRoleWeightInBonus(Roles.WEAK_CHERRY) + GetRoleWeightInBonus(Roles.STRONG_CHERRY));
            }

            return rolesWeightInBonus[RolesToIndex(role)];
        }



        //レア役のボーナス確率 probability%
        public static void SetRoleBonusProbability(Roles role, byte probability)
        {
            rolesBonusProbability[RolesToIndex(role)] = probability;
        }

        public static byte GetRoleBonusProbability(Roles role)
        {
            return rolesBonusProbability[RolesToIndex(role)];
        }




        //役を要素番号に変更するメソッド
        //BELLが0になるがBELL以外も0になる可能性のため注意
        private static byte RolesToIndex(Roles role)
        {
            switch (role)
            {
                case Roles.BELL: return 0;
                case Roles.REPLAY: return 1;
                case Roles.WATERMELON: return 2;
                case Roles.WEAK_CHERRY: return 3;
                case Roles.STRONG_CHERRY: return 4;
                case Roles.VERY_STRONG_CHERRY: return 5;
                case Roles.REGULAR: return 6;
                case Roles.BIG: return 7;
            }
            return 0;
        }
    }

}