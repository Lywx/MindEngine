namespace MindEngine.Core.Service.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct MMDebugBlockTimerRecord
    {
#if DEBUG
        public static Dictionary<int, MMDebugBlockTimerRecord> CallerRecords = new Dictionary<int, MMDebugBlockTimerRecord>();
#endif

        public readonly string CallerFilePath;

        public readonly int CallerLineNumber;

        public readonly string CallerMemberName;

        // TODO(Wuxiang): May add threading support later.
        public readonly int CallerThreadIndex;

        public long CallerTimestamp;

        public MMDebugBlockTimerRecord(string callerFilePath, string callerMemberName, int callerLineNumber)
        {
            this.CallerFilePath = callerFilePath;
            this.CallerMemberName = callerMemberName;
            this.CallerLineNumber = callerLineNumber;

            this.CallerThreadIndex = 0;
            this.CallerTimestamp = 0;

#if DEBUG
            CallerRecords[this.GetHashCode()] = this;
#endif
        }

        private string GetCallerFileName()
        {
            var filenameIndex = this.CallerFilePath.LastIndexOf(@"/", StringComparison.Ordinal) + 1;
            var fileName = this.CallerFilePath.Skip(filenameIndex).ToString();

            return fileName;
        }

        // Test Sample:
        //
        // void Main()
        // {
        //     HashTest.Test();
        // }
        //
        // public class HashTest
        // {
        //     private static int Hash(string s)
        //     {
        //         int res = 0;
        //         for (int i = 0; i < s.Length; i++)
        //         {
        //             res += (i * s[i]) % int.MaxValue;
        //         }
        //         return res;
        //     }
        //
        //     public static int Test()
        //     {
        //         Console.WriteLine(Hash("dd3.cs"));
        //         Console.WriteLine(Hash("ftp.cs"));
        //         return 0;
        //     }
        // }
        //
        // Reference http://stackoverflow.com/a/12272556/1940602
        private int HashCallerFileName(string filename)
        {
            var result = 0;

            for (var i = 0; i < filename.Length; i++)
            {
                result += (i * filename[i]) % int.MaxValue;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return this.HashCallerFileName(this.GetCallerFileName()) * this.CallerLineNumber;
        }
    }
}
