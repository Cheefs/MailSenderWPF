﻿using System;
using System.Windows;
using MailSenderWPF.ViewModel;
using MailSenderDll;
using System.Collections.Generic;
using System.Linq;

namespace MailSenderWPF
{
    /// <summary>
    /// Логика взаимодействия для WpfMailSender.xaml
    /// </summary>
    public partial class MainWindow : Window, IViev
    {

        #region  реализация интерфейса IViev
        public string MsgText { get => rtbText.Text; set => rtbText.Text = value; }
        public string Sender { get => cbSenderSelect.Text; set => cbSenderSelect.Text = value; }
        public string MsgHead { get => tbxHeadMsg.Text; set => tbxHeadMsg.Text = value; }
        public string SmptServer { get => cbSmtpSelect.Text; set => cbSmtpSelect.Text = value; }
        public  bool FlagNow { get; set; }

        #endregion

        private readonly ViewModelLocator _locator;
         Dictionary<DateTime, string> dicDates = new Dictionary<DateTime, string>();
        public MainWindow()
        {

            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = this;

            _locator = (ViewModelLocator)FindResource("Locator");
            Scheduler sc = new Scheduler(this);
            
            lvwSheduler.Items.Clear();
            lvwSheduler.ItemsSource = dicDates;

            lvwSheduler.DisplayMemberPath = "Key";
            lvwSheduler.SelectedValuePath = "Value";

            cbSenderSelect.ItemsSource = VariablesClass.Senders;
            cbSenderSelect.DisplayMemberPath = "Key";
            cbSenderSelect.SelectedValuePath = "Value";
            cbSenderSelect.SelectedIndex = 0;


            cbSmtpSelect.ItemsSource = VariablesClass.SmptServer;
            cbSmtpSelect.DisplayMemberPath = "Key";
            cbSmtpSelect.SelectedValuePath = "Value";
            cbSmtpSelect.SelectedIndex = 0;

            ListVievItemsScheduler.BtnDeleteClick += delegate 
            {
                if(dicDates.Keys!=null)
                {
                    dicDates.Remove(dicDates.Keys.First<DateTime>());
                    lvwSheduler.Items.Refresh();
                }
            };

            btnClock.Click += delegate { tabControl.SelectedItem = tabPlanner; };

            btnSendAtOnce.Click += delegate
            {
                FlagNow = true;
                MailSender mailSender = new MailSender(cbSenderSelect.Text, cbSenderSelect.SelectedValue.ToString());
                if (MsgText == "" || MsgHead == "")
                {
                    MessageBox.Show("Заполните текст письма");
                    rtbxBody.Visibility = Visibility.Visible;
                }
                else
                sc.SendMails(_locator.Main.Emails);
            };

            btnSend.Click += delegate
            {
                sc.SendEmails(dicDates, _locator.Main.Emails);
                lvwSheduler.Items.Refresh();

                rtbxBody.Visibility = Visibility.Hidden;
                rtbText.Text = null;

            };

            BtnAddToPlanner.Click += delegate
            {
                var locator = (ViewModelLocator)FindResource("Locator");

                var tsSendTime = sc.GetSendTime(ListVievItemsScheduler.Text);

                DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
                if (dtSendDateTime < DateTime.Now)
                {
                    MessageBox.Show(@"Дата и время отправки писем не могут быть раньше, чем настоящее
                    время");
                    return;
                }
                dicDates.Add(cldSchedulDateTimes.SelectedDate ??
                DateTime.Today.Add(tsSendTime), MsgText);
                lvwSheduler.Items.Refresh();
            };

            #region ComingSoon
            edcMails.BtnAddClick += delegate
            {
                if (ctrlCaveEmail.Visibility == Visibility.Hidden)
                    ctrlCaveEmail.Visibility = Visibility.Visible;
                else ctrlCaveEmail.Visibility = Visibility.Hidden;
            };
            edcMails.BtnDeleteClick += delegate { MessageBox.Show("ComingSoon"); };
            edcMails.BtnEditClick += delegate { MessageBox.Show("ComingSoon"); };

            edcSender.BtnAddClick += delegate { MessageBox.Show("ComingSoon"); };
            edcSender.BtnDeleteClick += delegate { MessageBox.Show("ComingSoon"); };
            edcSender.BtnEditClick += delegate { MessageBox.Show("ComingSoon"); };

            edcSmpt.BtnAddClick += delegate { MessageBox.Show("ComingSoon"); };
            edcSmpt.BtnDeleteClick += delegate { MessageBox.Show("ComingSoon"); };
            edcSmpt.BtnEditClick += delegate { MessageBox.Show("ComingSoon"); };
            #endregion

            ListVievItemsScheduler.BtnAddClick += delegate  
            {
                if(rtbxBody.Visibility == Visibility.Visible)
                rtbxBody.Visibility =Visibility.Hidden;
                else rtbxBody.Visibility = Visibility.Visible;
            };
            
            tabSwtcher.btnNextClick += delegate
            {
                if (tabControl.SelectedIndex > 0)
                {
                    tabSwtcher.IsHideBtnNext = true;
                }
                tabSwtcher.IsHideBtnPrevios = false;
                tabControl.SelectedIndex++;
            };
            tabSwtcher.btnPreviosClick += delegate
            {
                tabSwtcher.IsHideBtnNext = false;
                if (tabControl.SelectedIndex == 1)
                {
                    tabSwtcher.IsHideBtnPrevios = true;
                }
                tabControl.SelectedIndex--;
            };
        }

    }
}
