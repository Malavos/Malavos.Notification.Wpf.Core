﻿using System;
using System.Windows;
using Malavos.Notification.Wpf.Core.Classes;

namespace Malavos.Notification.Wpf.Core.Classes
{
    public class ProgressFinaly<T> : Progress<T>, IDisposable
    {
        #region IsFinished : bool - progress was finished

        /// <summary>progress was finished</summary>
        public bool IsFinished => _IsFinished;

        /// <summary>progress was finished</summary>
        private bool _IsFinished { get; set; }

        #endregion

        public OperationTimer WaitingTimer { get; } = new ();
        private Controls.Notification Area;

        public ProgressFinaly(Action<T> handler) : base(handler)
        { }

        public void Report(T value) { base.OnReport(value); }

        public void Dispose()
        {
            _IsFinished = true;
            try
            {
                Application.Current.Dispatcher.Invoke(() => Area.Close());
            }
            catch
            {
                // ignored
            }
        }

        public void SetArea(Controls.Notification area)
        {
            Area = area;
        }
        
    }
}