﻿using System.Collections.ObjectModel;
using Common;
using System.Data.Entity;

namespace MailSenderWPF.Services
{
public interface IDataAccessService
    {
        ObservableCollection<Email> GetEmails();
        int AddEmail(Email email);
        int DeleteEmail(Email email);
    }

    public class DataAccessService : IDataAccessService
    {
        public ObservableCollection<Email> Emails = new ObservableCollection<Email>();
        EmailxmlContainer context;
        public DataAccessService()
        {
            context = new EmailxmlContainer();
        }
        public ObservableCollection<Email> GetEmails()
        {
            
            foreach (var item in context.Emails)
            {
                Emails.Add(item);
            }
            return Emails;
        }
        public int AddEmail(Email email)
        {
            context.Emails.Add(email);
            context.SaveChanges();
            return email.Id;
        }
        public int UpdateEmail(Email email)
        {
            context.Emails.Attach(email);
            context.Entry(email).State = EntityState.Modified;
            context.SaveChanges();
            return email.Id;
        }

        public int DeleteEmail(Email email)
        {
            if (context.Entry(email).State == EntityState.Detached)
            {
                context.Emails.Attach(email);
            }
            context.Emails.Remove(email);

            context.SaveChanges();
            return email.Id;
        }
    }
}