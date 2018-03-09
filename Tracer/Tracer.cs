using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tracer
{
    public class Tracer
    {
        // Always correct!!!
        private string _folderPath;
        // Always correct!!!
        private string _fileName;


        private static readonly object Locker = new object();

        private static Tracer _instanceInner;
        private static Tracer Instance
        {
            get
            {
                lock (Locker)
                {
                    return _instanceInner ?? (_instanceInner = new Tracer());
                }
            }
            set
            {
                lock (Locker)
                {
                    _instanceInner = value;
                }
            }
        }

        #region Constructors

        private Tracer()
        {
            _folderPath = GetDefaultFolderPath;

            BaseInit();
        }

        private Tracer(string folderPath)
        {
            _folderPath = folderPath;

            BaseInit();
        }

        void BaseInit()
        {
            var renamed = IsRenamed();
            TryToCreateDirectory();
            CreateNewFile();

            if (renamed)
            {

            }
        }

        #endregion

        #region Private

        private bool IsRenamed()
        {
            // переименовывю папки
            if (string.IsNullOrWhiteSpace(_folderPath))
            {
                _folderPath = GetDefaultFolderPath;
                return true;
            }

            var invilid = Path.GetInvalidFileNameChars();
            if (_folderPath.Any(x => invilid.Contains(x)))
            {
                _folderPath = GetDefaultFolderPath;
                return true;
            }

            return false;
        }

        private bool TryToCreateDirectory()
        {

            // папка  уже была
            if (Directory.Exists(_folderPath))
                return true;

            try
            {
                // успешно создал
                Directory.CreateDirectory(_folderPath);
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    _fileName = Path.GetTempFileName();
                }
                catch
                {
                    throw new Exception("Случилось невероятное - не смог создать временный файл");
                }

                // папку не создал, зато сделал временный файл
                File.AppendAllText(_fileName, e.Message);
                return false;
            }
        }

        private string GetDefaultFolderPath
        {
            get
            {
                var exeName = AppDomain.CurrentDomain.FriendlyName;
                var result = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    exeName);

                return result;
            }
        }

        private void CreateNewFile()
        {
            _fileName = Path.Combine(_folderPath, Guid.NewGuid().ToString());
            var stream = File.CreateText(_fileName);
            stream.Close();
        }

        private void WriteWithoutExceptions(string text)
        {
            try
            {
                File.AppendAllText(_fileName, text);
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Static

        /// <summary>
        /// Возвращает полный путь к файлу, который пишется в данный момент
        /// </summary>
        public static string GetCurrentFilePath => Instance._fileName;
        /// <summary>
        /// Создаёт новый файл и пешт в него
        /// </summary>
        public static void BeginNewFile()
        {
            Instance.CreateNewFile();
        }
        /// <summary>
        /// Логи будут писаться в этой папке
        /// </summary>
        /// <param name="folderName"></param>
        public static void SetFolder(string folderName)
        {
            Instance = new Tracer(folderName);
        }
        /// <summary>
        /// Пишет сообщение об ошибке, где оно было выброшено и StackTrace
        /// </summary>
        /// <param name="e"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        public static void Write(Exception e,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = -1)
        {
            var message = $"ERROR\n{file} {member} {line}\n{e.Message}\n{e.StackTrace}\n\n\n";
            Instance.WriteWithoutExceptions(message);
        }
        /// <summary>
        /// Записываю сообщение и где оно было вызвано
        /// </summary>
        /// <param name="text"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        public static void Write(string text,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = -1)
        {
            var message = $"{file} {member} {line}\n{text}";
            Instance.WriteWithoutExceptions(message);
        }
        #endregion

    }
}
