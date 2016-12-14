using Python = IronPython.Hosting.Python;

namespace MindEngine.Core.Service.Scripting.IronPython
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.Scripting;
    using Microsoft.Scripting.Hosting;

    public class MMIronPythonSessionFlushEventArgs : EventArgs
    {
        public MMIronPythonSessionFlushEventArgs(string output, string error)
        {
            this.Output = output;
            this.Error = error;
        }

        public string Output { get; }

        public string Error { get; }
    }

    internal interface IMMIronPythonSession
    {
        #region Script Engine

        ScriptScope ScriptGlobal { get; }

        ScriptEngine ScriptEngine { get; }

        #endregion

        #region Script Session

        event EventHandler<MMIronPythonSessionFlushEventArgs> Flush;

        #endregion
    }

    public class MMIronPythonSession : IMMIronPythonSession
    {
        private const string CreateSessionAsyncThreadName  = "IpySession.CreateSessionAsync";

        private const string EvalExpressionAsyncThreadName = "IpySession.EvalExpressionAsync";

        public MMIronPythonSession()
        {
            this.CreateSessionAsync();
        }

        ~MMIronPythonSession()
        {
            this.Dispose(true);
        }

        public event EventHandler<MMIronPythonSessionFlushEventArgs> Flush = delegate {};

        public ScriptEngine ScriptEngine { get; set; }

        public ScriptScope ScriptGlobal { get; set; }

        private StringBuilder ScriptError { get; set; }

        private StringBuilder ScriptOutput { get; set; }

        private MemoryStream ScriptOutputStream { get; set; }

        private StreamWriter ScriptOutputStreamWriter { get; set; }

        public bool IsInitialized => this.ScriptEngine != null && this.ScriptGlobal != null;

        private void Check()
        {
            if (this.ScriptEngine != null && this.ScriptGlobal != null)
            {
                throw new InvalidOperationException("Session is not initialized.");
            }
        }

        #region Session Operations

        private void CreateSessionAsync()
        {
            this.ThreadAP.StartThread(CreateSessionAsyncThreadName, this.CreateSession);
        }

        private void CreateSession()
        {
            this.ScriptEngine = Python.CreateEngine();
            this.ScriptGlobal = this.ScriptEngine.CreateScope();

            this.CreateOutputStream();
            this.CreateBuffer();
        }

        public void SetVariable(string name, object value)
        {
            this.ScriptGlobal.SetVariable(name, value);
        }

        #endregion

        #region Evaluation Operations

        public void EvalExpressionAsync(string expression)
        {
            if (this.IsInitialized)
            {
                this.ThreadAP.StartThread(
                    EvalExpressionAsyncThreadName,
                    () => this.EvalExpression(expression));
            }
            else
            {
                this.ThreadAP.DeferThread(
                    EvalExpressionAsyncThreadName,
                    () => this.EvalExpressionAsync(expression));
            }
        }

        public void EvalExpression(string expression)
        {
            try
            {
                var source = this.ScriptEngine.CreateScriptSourceFromString(
                    expression,
                    SourceCodeKind.Statements);
                var compiled = source.Compile();

                compiled.Execute(this.ScriptGlobal);
            }
            catch (Exception e)
            {
                this.ScriptError.Append(e);
            }
            finally
            {
                this.ReadOutput();
            }

            this.FlushBuffer();
        }

        #endregion

        #region Output Operations

        private void ClearBuffer()
        {
            this.ScriptOutput  .Clear();
            this.ScriptError.Clear();
        }

        private void ClearStream()
        {
            this.DisposeOutputStream();
            this.CreateOutputStream();
        }

        private void CreateOutputStream()
        {
            this.ScriptOutputStream       = new MemoryStream();
            this.ScriptOutputStreamWriter = new StreamWriter(this.ScriptOutputStream);

            this.ScriptEngine.Runtime.IO.SetOutput(
                this.ScriptOutputStream,
                this.ScriptOutputStreamWriter);
        }

        private void CreateBuffer()
        {
            this.ScriptError = new StringBuilder();
            this.ScriptOutput   = new StringBuilder();
        }

        private void FlushBuffer()
        {
            this.FlushBuffer(this.ScriptOutput.ToString(), this.ScriptError.ToString());
        }

        private void FlushBuffer(string output, string error)
        {
            this.Flush?.Invoke(this, new MMIronPythonSessionFlushEventArgs(output, error));

            this.ClearBuffer();
            this.ClearStream();
        }

        private void ReadOutput()
        {
            this.ScriptOutput.Append(this.ReadStream(this.ScriptOutputStream).Trim());
        }

        private string ReadStream(MemoryStream memoryStream)
        {
            var length = (int)memoryStream.Length;
            var bytes  = new byte[length];

            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Read(bytes, 0, length);

            return Encoding.GetEncoding("utf-8")
                           .GetString(bytes, 0, length);
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.DisposeOutputStream();
                        this.DisposeEvent();
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
            }
        }

        private void DisposeOutputStream()
        {
            if (this.ScriptOutputStreamWriter != null)
            {
                this.ScriptOutputStreamWriter.Dispose();
                this.ScriptOutputStreamWriter = null;
                this.ScriptOutputStream = null;
            }
        }

        #endregion
    }
}