using System;

namespace ClassLibrary1.Fundamentals
{
    public class ErrorLogger
    {
        public string LastError { get; set; }
        public event EventHandler<Guid> ErrorLogged;

        public void Log(string error)
        {
            // check null, "", " "
            if (string.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException();
            LastError = error;

            // write a log to storage
            // ...
            ErrorLogged?.Invoke(this, Guid.NewGuid());
            // OnErrorLogged(Guid.NewGuid()); // another way to be called by private method
        }

        protected virtual void OnErrorLogged(Guid errorId)
        {
            ErrorLogged?.Invoke(this, errorId);
        }
    }
}