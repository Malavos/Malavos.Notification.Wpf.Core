﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Malavos.Notification.Wpf.Core;
using Malavos.Notification.Wpf.Core.Classes;
using Malavos.Notification.Wpf.Core.Command;
using Notifications.Wpf.ViewModels.Base; //using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Command;

namespace Malavos.Notification.Wpf.Core.ViewModels
{
    public class NotificationProgressViewModel : ViewModel
    {
        public readonly CancellationTokenSource Cancel;

        #region Титул окна

        private string _Title;
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Message : string - Текст сообщения

        /// <summary>Текст сообщения</summary>
        private string _Message;

        /// <summary>Текст сообщения</summary>
        public string Message { get => _Message; set => Set(ref _Message, value); }

        #endregion

        #region process : double - Прогресс задачи

        /// <summary>Прогресс задачи</summary>
        private double _process;

        /// <summary>Прогресс задачи</summary>
        public double process { get => _process; set => Set(ref _process, value); }

        #endregion

        #region IsIndeterminate : bool - Состояние прогресс бара - бегунок или прогресс

        /// <summary>Состояние прогресс бара - бегунок или прогресс</summary>
        private bool _ShowProgress;

        /// <summary>Состояние прогресс бара - бегунок или прогресс</summary>
        public bool ShowProgress { get => _ShowProgress; set => Set(ref _ShowProgress, value); }

        #endregion

        #region ProgressBar : IProgress<(double, string, string,bool)> - Прогресс

        /// <summary>Прогресс</summary>
        private ProgressFinaly<(int? percent, string message, string title, bool? showCancel)> _progress;

        /// <summary>Прогресс</summary>
        public ProgressFinaly<(int? percent, string message, string title, bool? showCancel)> progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        #endregion

        #region ShowCancelButton : bool - видимость кнопки отмены

        /// <summary>видимость кнопки отмены</summary>
        private bool _ShowCancelButton;

        /// <summary>видимость кнопки отмены</summary>
        public bool ShowCancelButton { get => _ShowCancelButton; set => Set(ref _ShowCancelButton, value); }

        #endregion

        #region Collapse : bool - Вид прогресса - свернуть до полосы

        /// <summary>Вид прогресса - свернуть до полосы</summary>
        private bool _Collapse;

        /// <summary>Вид прогресса - свернуть до полосы</summary>
        public bool Collapse
        {
            get => _Collapse;
            set
            {
                Set(ref _Collapse, value);
                GeneralPadding = value ? new Thickness(1) : new Thickness(12);
                BarMargin = value ? new Thickness(1) : new Thickness(5);
                BarHeight = value ? 32 : 20;
            }
        }

        #region GeneralPadding : int - Отступ элементов от внешней рамки

        /// <summary>Отступ элементов от внешней рамки</summary>
        private Thickness _GeneralPadding = new Thickness(12);

        /// <summary>Отступ элементов от внешней рамки</summary>
        public Thickness GeneralPadding { get => _GeneralPadding; set => Set(ref _GeneralPadding, value); }

        #endregion

        #region BarMargin : Thickness - отступ прогресс бара от рамки строки

        /// <summary>Отступ прогресс бара от рамки строки</summary>
        private Thickness _BarMargin = new Thickness(5);

        /// <summary>Отступ прогресс бара от рамки строки</summary>
        public Thickness BarMargin { get => _BarMargin; set => Set(ref _BarMargin, value); }

        #endregion

        #region BarHeight : double - ввысота прогресс бара

        /// <summary>высота прогресс бара</summary>
        private double _BarHeight = 20;

        /// <summary>высота прогресс бара</summary>
        public double BarHeight { get => _BarHeight; set => Set(ref _BarHeight, value); }

        #endregion

        #endregion

        #region CollapseWindowCommand : ICommand - Команда свертывания прогресс бара в строку

        /// <summary>Команда свертывания прогресс бара в строку</summary>
        private ICommand _CollapseWindowCommand;

        /// <summary>Команда свертывания прогресс бара в строку</summary>
        public ICommand CollapseWindowCommand { get => _CollapseWindowCommand; set => Set(ref _CollapseWindowCommand, value); }
        private void CollapseWindow(object Obj)
        {
            Collapse = !Collapse;
        }

        #endregion

        #region TrimType : NotificationTextTrimType - Обрезать сообщения за выходом размера

        /// <summary>Обрезать сообщения за выходом размера</summary>
        private NotificationTextTrimType _TrimType = NotificationTextTrimType.NoTrim;

        /// <summary>Обрезать сообщения за выходом размера</summary>
        public NotificationTextTrimType TrimType { get => _TrimType; set => Set(ref _TrimType, value); }

        #endregion

        #region RowsCount : uint - Число строк текста

        /// <summary>Число строк текста</summary>
        private uint _RowsCount = 2U;

        /// <summary>Число строк текста</summary>
        public uint RowsCount { get => _RowsCount; set => Set(ref _RowsCount, value); }

        #endregion

        #region WaitingTime : string - Время ожидания окончания операции

        /// <summary>Время ожидания окончания операции</summary>
        private string _WaitingTime;

        /// <summary>Время ожидания окончания операции</summary>
        public string WaitingTime { get => _WaitingTime; set => Set(ref _WaitingTime, value); }

        #endregion
        /// <summary>
        /// Содержимое левой кнопки
        /// </summary>
        public object RightButtonContent { get; set; } = "Cancel";

        public NotificationProgressViewModel(out ProgressFinaly<(int? percent, string message, string title, bool? showCancel)> progresModel, CancellationTokenSource cancel,
            bool showCancelButton, bool showProgress, bool trimText, uint DefaultRowsCount, string BaseWaitingMessage)
        {
            ShowProgress = showProgress;
            Cancel = cancel;
            progress = progresModel = new ProgressFinaly<(int? percent, string message, string title, bool? showCancel)>(OnProgress);
            ShowCancelButton = showCancelButton;
            CollapseWindowCommand = new LamdaCommand(CollapseWindow);
            if (trimText)
                TrimType = NotificationTextTrimType.Trim;
            RowsCount = DefaultRowsCount;
            if (BaseWaitingMessage != null) progress.WaitingTimer.BaseWaitingMessage = BaseWaitingMessage;
        }

        void OnProgress((int? percent, string message, string title, bool? showCancel) ProgressInfo)
        {
            if (ProgressInfo.percent is null)
            {
                if (ShowProgress)
                {
                    ShowProgress = false;
                    progress.WaitingTimer.Restart();
                    WaitingTime = string.Empty;
                }
            }
            else
            {
                if (!ShowProgress)
                {
                    ShowProgress = true;
                    progress.WaitingTimer.Restart();
                }
                process = (double)ProgressInfo.percent;

                WaitingTime = progress.WaitingTimer.BaseWaitingMessage is null ? null : process > 10 ? progress.WaitingTimer.GetStringTime((double)ProgressInfo.percent, 100) : progress.WaitingTimer.BaseWaitingMessage;
            }
            Message = ProgressInfo.message;
            if (ProgressInfo.title != null) Title = ProgressInfo.title;
            if (ProgressInfo.showCancel != null)
                ShowCancelButton = (bool)ProgressInfo.showCancel;
        }


        public void CancelProgress(object Sender, RoutedEventArgs E) => Cancel.Cancel();

    }
}
