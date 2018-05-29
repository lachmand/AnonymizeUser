using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace AnonymizeUser
{
    /// <summary>
    /// Accepts  input in form AnonymizeUser "a/r" userId reason cascade
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<2)
            {
                Console.WriteLine("Format: AnonymizeUser a/r userId cascade reason ");
            }
            switch (args[0].ToLower())
            {
                case "a":
                    Anonymize(args);
                    break;
                case "r":
                    Retrieve(args);
                    break;
            }
        }
        static void Anonymize(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("AnonymizeUser <a/r> <userId> <canCascade> <reason>");
                return;
            }

            if (!Int64.TryParse(args[1], out var result))
            {
                Console.WriteLine("user id must be integer");
                return;
            }

            if (!bool.TryParse(args[2], out var canCascade))
            {
                Console.WriteLine("canCascade must be boolean true or false");
                return;
            }

            if (String.IsNullOrEmpty(args[3]))
            {
                Console.WriteLine("reason must be string");
                return;
            }

            AnonymizeToken token = new AnonymizeToken(args[1], args[3], args[2]);
            MySqlUtility mySql = new MySqlUtility();
            BlobUtility blobUtility = new BlobUtility();

            GdprUtility gdprUtility = new GdprUtility(blobUtility,mySql);

            gdprUtility.Anonymize((token));
        }

        static void Retrieve(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("AnonymizeUser <a/r> <userId> <canCascade> <reason>");
                return;
            }

            if (!Int64.TryParse(args[1], out var result))
            {
                Console.WriteLine("user id must be integer");
                return;
            }

            if (!bool.TryParse(args[2], out var canCascade))
            {
                Console.WriteLine("canCascade must be boolean true or false");
                return;
            }

            AnonymizeToken token = new AnonymizeToken(args[1],null, args[2]);
            MySqlUtility mySql = new MySqlUtility();
            BlobUtility blobUtility = new BlobUtility();

            GdprUtility gdprUtility = new GdprUtility(blobUtility, mySql);

            gdprUtility.Retrieve((token));
        }
    }//class
}//ns
