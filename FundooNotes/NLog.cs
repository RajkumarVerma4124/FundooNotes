using NLog;

namespace FundooNotes
{
    public class Nlog
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        //Method to write nlog error info to a file
        public static void ErrorInfo(string msg)
        {
            logger.Error(msg);
        }

        //Method to write nlog error info to a file
        public static void SuccessInfo(string msg)
        {
            logger.Info(msg);
        }
    }
}
