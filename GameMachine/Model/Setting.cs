using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using static Constants;

namespace Model;


public class Setting
{
    public enum  RolesIndex: byte
    {
        NONE = 0,
        BELL = 1,
        REPLAY = 2,
        WATERMELON = 3,
        WEAK_CHERRY = 4,
        STRONG_CHERRY = 5,
        VERY_STRONG_CHERRY = 6,
    }

    public enum BonusesIndex: byte
    {
        BIG = 0,
        REGULAR = 1,
    }


	static String pcID = "info";
    static String userID = "";
    static sbyte expected = 0;
    static sbyte bonusProbability = 50; //最大100%での表記
    static byte[] rolesReturn = {0,9,0, };
    static byte[] bonusesProbability = {10,20};//ボーナス当選後のBIG/REGに入る確率の割合 {BIG,REG}の順で格納
    static byte[] rolesWeight = {20,8,5,4,4,2,1};
    //NONE,ベル,リプレイ,スイカ,弱チェリー,強チェリー,中段チェリーの順で入れること
  


    public static void setUserID(String userID) { Setting.userID = userID; }
    public static String getUserID() { return Setting.userID; }

    public static void makeTableID()
    {
        byte[] hashValue = generateHash(getMacAddress() + getProductID());

        // ハッシュ値を表示
        pcID = BitConverter.ToString(hashValue).Replace("-", "");
    }

    public static void setTableID(String pcID)
	{
        Setting.pcID = pcID;
    }

    public static String getTableID()
    {
        return Setting.pcID;
    }

    public static byte[] generateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }

    private static string getMacAddress()
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

    private static string getProductID()
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


    public static void setBonusProbability(sbyte bonusProbability) { Setting.bonusProbability = bonusProbability; }
    public static sbyte getBonusProbability() {  return Setting.bonusProbability; }


    public static void setBonusesProbabilityWeight(sbyte bonus, sbyte weight) { bonusesProbabilityWeight[bonus] = weight; }
    public static sbyte getBonusesProbabilityWeight(sbyte bonus) { return bonusesProbabilityWeight[bonus]; }


    public static void weightingSymbol(sbyte expected) //重み付け 変更必要
    {
        switch (expected)
        {


        }
    } 

    public static sbyte getRoleWeight(sbyte symbol) 
    {
        return rolesWeight[symbol];
    }
}
