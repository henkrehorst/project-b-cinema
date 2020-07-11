﻿using System;
using System.Collections.Generic;
using System.Text;
using bioscoop_app.Helper;

namespace bioscoop_app.Model
{
    public class Gift : DataType
    {
        public static GiftPrice Prices;
        public readonly string Code;
        public readonly string Email;

        public Gift()
        {
            int codeLength = 6;
            int start = 65;
            int end = 122;
            Random random = new Random();
            string newCode = "";

            for(int i = 0; i < codeLength; i++)
            {
                newCode += (char)random.Next(start, end + 1);
            }

            Code = newCode;
        }

        public Gift(string email) : this()
        {
            int validate = 0;

            for(int i = 0; i < email.Length; i++)
            {
                if((char)email[i] == '@' && validate == 0)
                {
                    validate++;
                }
                else if((char)email[i] == '.')
                {
                    validate++;
                }
            }

            if(validate != 2)
            {
                Email = null;
            }
            else
            {
                Email = email;
            }
        }

        public string GetCode()
        {
            return Code;
        }
    }
}