﻿using System.Collections.Generic;
using CodePasswordDLL;

namespace MailSenderWPF
{
    public static class VariablesClass
    {
        /// <summary>
        /// Отправителли
        /// </summary>
        public static Dictionary<string, string> Senders
        {
            get { return DicSenders; }
        }
        /// <summary>
        /// Smpt сервера
        /// </summary>
        public static Dictionary<string, int> SmptServer
        {
            get { return DicSmpt; }
        }
        /// <summary>
        /// Коллекция отправителей
        /// </summary>
        private static Dictionary<string, string> DicSenders = new Dictionary<string, string>()
        {
             { "TestSMTe@yandex.ru",CodePassword.GetCodPassword("is&d7sipo8")},
             { "smtptest22@mail.ru", CodePassword.GetCodPassword("Isduj5s4")}
        };
        /// <summary>
        /// Коллекция настроек Smpt Серверов
        /// </summary>
        private static Dictionary<string, int> DicSmpt = new Dictionary<string, int>()
        {
             { "smtp.yandex.ru",25},
             { "smtp.mail.ru",25},
             { "smtp.gmail.com",25}
        };
    }
}


