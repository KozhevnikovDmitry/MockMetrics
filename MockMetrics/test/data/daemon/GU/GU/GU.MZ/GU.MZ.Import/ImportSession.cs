using System;
using System.Threading.Tasks;
using Common.DA.Interface;

namespace GU.MZ.Import
{
    public class ImportSession
    {
        private readonly Importer _importer;
        private readonly Func<IDomainDbManager> _getDb;
        private Task<Exception> importTask;
        private IDomainDbManager _currentDb;

        public bool NotCommitedSession { get; private set; }

        public ImportSession(Importer importer, Func<IDomainDbManager> getDb)
        {
            NotCommitedSession = false;
            _importer = importer;
            _getDb = getDb;
            _importer.OnProgress += OnProgress;
        }

        public void Import(string fileName, string logName)
        {
            try
            {
                _currentDb = _getDb();
                _currentDb.BeginDomainTransaction();
                importTask = new Task<Exception>(() => CallImporter(_currentDb, fileName, logName));
                importTask.ContinueWith(t => CompleteImport(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
                importTask.Start();
                NotCommitedSession = true;
            }
            catch (Exception)
            {
                Complete("Импорт прерван из-за ошибки");
                throw;
            }
        }

        private Exception CallImporter(IDomainDbManager db, string registrPath, string logPath)
        {
            try
            {
                _importer.Import(db, registrPath, logPath);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void CompleteImport(Exception ex)
        {
            if (ex != null)
            {
                OnError(ex);
            }
            WorkerOnRunWorkerCompleted();
        }

        public void Cancel()
        {
            NotCommitedSession = false;
            if (!importTask.IsCompleted)
            {
                _importer.CancellationRequested = true;
            }
            else
            {
                _currentDb.RollbackDomainTransaction();
                _currentDb.Dispose();
            }
        }

        public void CommitImport()
        {
            NotCommitedSession = false;
            _currentDb.CommitDomainTransaction();
            _currentDb.Dispose();
        }

        #region Events

        public event Action<int> Progress;

        public event Action<string> Complete;

        public event Action<Exception> Error;

        private void WorkerOnRunWorkerCompleted()
        {
            if (Complete == null) return;

            if (_importer.CancellationRequested)
            {
                _importer.CancellationRequested = false;
                OnProgress(0);
                Complete("Импорт прерван");
                return;
            }

            if (_importer.Exception != null)
            {
                OnProgress(0);
                Complete("Импорт прерван из-за ошибки");
                return;
            }

            OnProgress(100);
            Complete("Импорт успешно завершён");
        }

        private void OnProgress(int percentage)
        {
            if (Progress != null)
            {
                Progress(percentage);
            }
        }

        private void OnError(Exception ex)
        {
            if (Error == null) return;
            Error(ex);
        }

        #endregion
    }
}
