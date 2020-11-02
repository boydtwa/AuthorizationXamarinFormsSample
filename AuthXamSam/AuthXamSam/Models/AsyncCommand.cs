﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AuthXamSam.Models
{
    /// <summary>
    /// see: https://johnthiriet.com/mvvm-going-async-with-async-command/ and
    ///      https://johnthiriet.com/removing-async-void/
    /// usefull for using an asyncronous call to the web service in a
    /// viewmodel constructor which is not async.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    public interface IAsyncCommandWithParameter<T> : ICommand
    {
        Task ExecuteAsyncWithParameter(T parameter);
        bool CanExecute(T parameter);
    }

    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }

    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool IsExceuting
        {
            get { return _isExecuting; }
        }

        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter) => CanExecute();

        void ICommand.Execute(object parameter) => ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        #endregion
    }


    public class AsyncCommandWithParameter<T> : IAsyncCommandWithParameter<T>
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommandWithParameter(Func<T, Task> execute, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsyncWithParameter(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        void ICommand.Execute(object parameter) => ExecuteAsyncWithParameter((T)parameter).FireAndForgetSafeAsync(_errorHandler);

        //bool ICommand.CanExecute(object parameter)
        //{
        //    return CanExecute((T)parameter);
        //}

        //void ICommand.Execute(object parameter)
        //{
        //    ExecuteAsyncWithParameter((T)parameter).FireAndForgetSafeAsync(_errorHandler);
        //}
        #endregion
    }

    public static class TaskUtilities
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
}
