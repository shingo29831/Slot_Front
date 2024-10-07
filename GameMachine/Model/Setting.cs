using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using static Constants;

namespace Model;
public class Setting
{
	static String tableID = "info";
    static String userID = "";
    static sbyte expected = 0;
    static sbyte bonusProbability = 50; //最大100%での表記
    static sbyte[] bonusesProbabilityWeight = {0,10,20};//ボーナス当選後のREG/BIGに入る確率
    static sbyte[] rolesWeight = {20,8,5,4,4,2,1,0,1,1};
    //NONE,ベル,リプレイ,スイカ,弱チェリー,強チェリー,最強チェリー,リーチ,REG,BIGの順で入れること
  


    public static void setUserID(String userID) { Setting.userID = userID; }
    public static String getUserID() { return Setting.userID; }

    public static void makeTableID()
    {
        byte[] hashValue = generateHash(getMacAddress() + getProductID());

        // ハッシュ値を表示
        tableID = BitConverter.ToString(hashValue).Replace("-", "");
    }

    public static void setTableID(String tableID)
	{
        Setting.tableID = tableID;
    }

    public static String getTableID()
    {
        return Setting.tableID;
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
