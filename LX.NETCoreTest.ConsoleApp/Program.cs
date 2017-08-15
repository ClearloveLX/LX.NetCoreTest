using LX.NETCoreTest.Common;
using System;

namespace LX.NETCoreTest.ConsoleApp {
    class Program {
        static void Main(string[] args) {
            string AUrl = "http:gk1213656215@outlook.com";
            string[] Item = ShortUrl(AUrl);
            string result = string.Empty;
            for (int i = 0; i < Item.Length; i++) {
                result += Item[i].ToString();
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(r);
            Console.Read();
        }




        public static string[] ShortUrl(string Url) {
            string[] chars = new string[] {"a" , "b" , "c" , "d" , "e" , "f" , "g" , "h" ,
              "i" , "j" , "k" , "l" , "m" , "n" , "o" , "p" , "q" , "r" , "s" , "t" ,
              "u" , "v" , "w" , "x" , "y" , "z" , "0" , "1" , "2" , "3" , "4" , "5" ,
              "6" , "7" , "8" , "9" , "A" , "B" , "C" , "D" , "E" , "F" , "G" , "H" ,
              "I" , "J" , "K" , "L" , "M" , "N" , "O" , "P" , "Q" , "R" , "S" , "T" ,
              "U" , "V" , "W" , "X" , "Y" , "Z" };
            String MD5EncryptResult = PublicClass._Md5(Url);
            string hex = MD5EncryptResult;

            string[] resUrl = new string[4];
            for (int i = 0; i < 4; i++) {
                //把加密符按照8位一组16禁止与0x3FFFFFFF进行位于运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++) {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    outChars += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
                resUrl[i] = outChars;
            }
            return resUrl;
        }
    }
}