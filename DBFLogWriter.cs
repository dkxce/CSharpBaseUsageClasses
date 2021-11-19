/*********************
*                    *
*   DBF LOG WRITER   *
*  milokz@gmail.com  *
*                    *
*********************/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace DBFSharp
{
    /// <summary>
    ///     File Period of Storage
    /// </summary>
    public enum PeriodEnum : byte
    {
        /// <summary>
        ///     1 file per day
        /// </summary>
        Day = 0,     // 1 file per day
        /// <summary>
        ///     1 file per week
        /// </summary>
        Week = 1,    // 1 file per week
        /// <summary>
        ///     1 file per month
        /// </summary>
        Month = 2,   // 1 file per month
        /// <summary>
        ///     1 file per quartal
        /// </summary>
        Quartal = 3, // 1 file per quartal
        /// <summary>
        ///     1 file per year
        /// </summary>
        Year = 4,    // 1 file per year
        /// <summary>
        ///     Never
        /// </summary>
        Never = 5
    }

    /// <summary>
    ///     Log Data Record
    /// </summary>
    public class ListElement
    {
        /// <summary>
        ///     Data
        /// </summary>
        public Dictionary<string, object> data;

        /// <summary>
        ///     DateTime of Data
        /// </summary>
        public DateTime dt;

        public ListElement(DateTime dt, Dictionary<string, object> data)
        {
            this.data = data;
            this.dt = dt;
        }
        public object this[string index]
        {
            get
            {
                if (!data.ContainsKey(index)) return null;
                return data[index];
            }
            set
            {
                if (data.ContainsKey(index))
                    data[index] = value;
                else
                    data.Add(index, value);
            }
        }
    }

    /// <summary>
    ///     Default Log Writer Class
    /// </summary>
    public abstract class LogWriter: IDisposable
    {
        protected string _fileDir = AppDomain.CurrentDomain.BaseDirectory;
        protected string _fileExt = ".bin";

        protected Exception _lastError = null;
        protected uint _errCounter = 0;
        protected PeriodEnum _storePeriod = PeriodEnum.Week;

        public delegate void OnError(LogWriter logWriter);
        public OnError OnException = null;

        public abstract void WriteAsync(DateTime dt, string device, string devType, string source, string parameters, string comment);
        public abstract void WriteAsync(DateTime dt, Dictionary<string, object> data);
        public abstract int DataCountToWrite();
        
        public Exception LastError { get { return _lastError; } }
        public uint ErrorsCounter { get { return _errCounter; } }
        public PeriodEnum Period { get { return _storePeriod; } }

        protected bool IsNewPeriod(DateTime dt, PeriodEnum periodEnum, ref int period, ref string fileName)
        {
            int nper = GetPeriod(periodEnum, dt);
            if (nper != period)
            {
                period = nper;
                string _file_ = "{0}\\LOG-FILE{4}";
                if (periodEnum == PeriodEnum.Day) _file_ = "{0}\\LOG-Y{1}{2}{3:000}{4}";
                if ((periodEnum == PeriodEnum.Week) || (periodEnum == PeriodEnum.Month)) _file_ = "{0}\\LOG-Y{1}{2}{3:00}{4}";
                if (periodEnum == PeriodEnum.Quartal) _file_ = "{0}\\LOG-Y{1}{2}{3:0}{4}";
                if (periodEnum == PeriodEnum.Year) _file_ = "{0}\\LOG-Y{1}{4}";
                fileName = String.Format(_file_, _fileDir, dt.Year, PeriodToString(periodEnum), period, _fileExt);
                return true;
            };
            return false;
        }

        private static string PeriodToString(PeriodEnum period)
        {
            if (period == PeriodEnum.Day) return "D";
            if (period == PeriodEnum.Week) return "W";
            if (period == PeriodEnum.Month) return "M";
            if (period == PeriodEnum.Quartal) return "Q";
            if (period == PeriodEnum.Year) return "Y";
            return "N";
        }

        private static int GetPeriod(PeriodEnum period, DateTime dt)
        {
            if (period == PeriodEnum.Day)
            {
                DateTime dts = new DateTime(dt.Year, 1, 1);
                return (int)(dt.Subtract(dts).TotalDays + 1);
            };
            if (period == PeriodEnum.Week) return System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (period == PeriodEnum.Month) return dt.Month;
            if (period == PeriodEnum.Quartal) return (dt.Month - 1) / 3 + 1;
            if (period == PeriodEnum.Year) return dt.Year;
            return 0; // Never
        }

        public bool isEmpty { get { return DataCountToWrite() == 0; } }

        public virtual void Dispose()
        {            
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    ///     DBF Log Writer Class
    /// </summary>
    public class DBFLogWriter : LogWriter
    {        
        private bool _loopThread = true;
        private List<ListElement> _recordsToWrite = new List<ListElement>();
        private System.Threading.Mutex _recordsMutex = new System.Threading.Mutex();
        private System.Threading.Thread _writeThread = null;
        private int _writeTimeout = 250;
        private int _killTimeout = 100;        
        
        public DBFLogWriter()
        {
            _storePeriod = PeriodEnum.Week;
            Init();
        }

        public DBFLogWriter(PeriodEnum storePeriod)
        {
            _storePeriod = storePeriod;
            Init();
        }

        public DBFLogWriter(PeriodEnum storePeriod, string logFileDirectory)
        {
            _storePeriod = storePeriod;
            _fileDir = logFileDirectory;
            Init();
        }

        public DBFLogWriter(string logFileDirectory)
        {
            _storePeriod = PeriodEnum.Week;
            _fileDir = logFileDirectory;
            Init();
        }

        private void Init()
        {
            _fileExt = ".dbf";
            _writeThread = new System.Threading.Thread(WriteThread);
            _writeThread.Start();
        }

        ~DBFLogWriter()
        {
            this.Dispose();
        }

        public override void Dispose()
        {
            if (!_loopThread) return;

            bool needWait = false;
            do 
            {
                _recordsMutex.WaitOne();
                needWait = _recordsToWrite.Count > 0;
                _recordsMutex.ReleaseMutex();
                if (needWait) System.Threading.Thread.Sleep(_killTimeout);
            }
            while (needWait);

            _loopThread = false;
            _writeThread.Join();

            base.Dispose();
        }

        public override int DataCountToWrite()
        {
            int res = 0;
            _recordsMutex.WaitOne();
            res = _recordsToWrite.Count;
            _recordsMutex.ReleaseMutex();
            return res;
        }

        public override void WriteAsync(DateTime dt, string device, string devType, string source, string parameters, string comment)
        {
            dt = dt.ToUniversalTime();
            Dictionary<string, object> rec = new Dictionary<string, object>();
            
            rec.Add("DATE", dt.ToString("yyyyMMdd"));
            rec.Add("TIME", dt.ToString("HHmmss"));
            rec.Add("DEVICE", device);
            rec.Add("TYPE", devType);
            rec.Add("SOURCE", source);
            rec.Add("PARAMS", parameters);
            rec.Add("COMMENT", comment);

            _recordsMutex.WaitOne();
            _recordsToWrite.Add(new ListElement(dt, rec));
            _recordsMutex.ReleaseMutex();
        }

        public override void WriteAsync(DateTime dt, Dictionary<string, object> rec)
        {
            dt = dt.ToUniversalTime();
            if (rec.ContainsKey("ID")) rec.Remove("ID");
            if (rec.ContainsKey("DATE")) rec.Remove("DATE");
            if (rec.ContainsKey("TIME")) rec.Remove("TIME");            
            rec.Add("DATE", dt.ToString("yyyyMMdd"));
            rec.Add("TIME", dt.ToString("HHmmss"));

            _recordsMutex.WaitOne();
            _recordsToWrite.Add(new ListElement(dt, rec));
            _recordsMutex.ReleaseMutex();
        }

        private void WriteThread()
        {
            bool hasEx = false;
            while (_loopThread)
            {                
                _recordsMutex.WaitOne();
                if (_recordsToWrite.Count > 0)
                {
                    try { ProcessRecords(_recordsToWrite); }
                    catch (Exception ex) { _lastError = ex; _errCounter++; hasEx = true; };
                };
                _recordsToWrite.Clear();
                _recordsMutex.ReleaseMutex();
                if (hasEx && (OnException != null)) { try { OnException(this); } catch { }; };
                System.Threading.Thread.Sleep(_writeTimeout);
                // GC.KeepAlive(this);
            };
        }

        private void ProcessRecords(List<ListElement> els)
        {
            int fileID = -1;
            string fileName = "";        
            DBFFile dbff = null;

            foreach (ListElement el in els)
            {
                if (IsNewPeriod(el.dt, _storePeriod, ref fileID, ref fileName))
                {
                    if (dbff != null) dbff.Close();
                    dbff = GetFile(fileName);
                };
                el["ID"] = GetRecordID(dbff.RecordsCount + 1);
                dbff.WriteRecord(el.data);
            };
            dbff.Close();
        }
        
        private static DBFFile GetFile(string fileName)
        {
            DBFFile dbffile = new DBFFile(fileName, System.IO.FileMode.OpenOrCreate);
            if (!dbffile.HeaderExists)
            {
                FieldInfos finfos = new FieldInfos();
                /* DBF FILE HEADER */
                finfos.Add("ID",      014, FieldType.Numeric  );  // 00000000000000 // Record ID
                finfos.Add("DATE",    008, FieldType.Character);  // yyyyMMdd       // Date
                finfos.Add("TIME",    006, FieldType.Character);  // HHmmss         // Time
                finfos.Add("DEVICE",  030, FieldType.Character);  // DEVICE ID      // Device ID
                finfos.Add("TYPE",    040, FieldType.Character);  // DEVICE TYPE    // Device Type/Vendor
                finfos.Add("SOURCE",  040, FieldType.Character);  // DATA SOURCE    // Source of Data
                finfos.Add("PARAMS",  100, FieldType.Character);  // DATA PARAMS    // Parameters
                finfos.Add("COMMENT", 200, FieldType.Character);  // DATA COMMENT   // Description
                /*******************/
                dbffile.WriteHeader(finfos);
            };
            return dbffile;
        }

        private static string GetRecordID(uint nextID)
        {
            return String.Format("{0:00000000000000}", nextID);
        }
    }
}
