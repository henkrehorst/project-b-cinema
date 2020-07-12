using System;
using System.Collections.Generic;
using System.Text;
using bioscoop_app.Helper;

namespace bioscoop_app.Model
{
    public class Gift : DataType
    {
        public static GiftPrice Prices;
        public string Code;
        public readonly string Email;
        public readonly string Type;
        public bool IsValid = true;

        public Gift()
        {
            
        }

        public Gift(string email, string type) : this()
        {
            Email = email;
            Type = type;
        }

        public void GenerateCode()
        {
            int codeLength = 6;
            int start = 65;
            int end = 122;
            Random random = new Random();
            string newCode = "";

            for (int i = 0; i < codeLength; i++)
            {
                newCode += (char)random.Next(start, end + 1);
            }

            Code = newCode;
        }
    }
}
