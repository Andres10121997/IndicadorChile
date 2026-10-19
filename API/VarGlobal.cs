using System;
using System.Threading.Tasks;

namespace API
{
    internal static class VarGlobal
    {
        #region Constructor Method
        static VarGlobal()
        {

        }
        #endregion



        #region Field
        #region Objects
        internal static ParallelOptions ParallelForEachOptions
        {
            get => new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                TaskScheduler = TaskScheduler.Current
            };
        }
        #endregion
        #endregion
    }
}