using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Helpers
{ /// <summary>
  /// Custom Exceptions By Using System Exception Class
  /// </summary>
    public class FundooNoteException : Exception
    {
        //Declaring exception type 
        public ExceptionTypes type;

        //Using enum to differentiate the mood analysis errors
        public enum ExceptionTypes
        {
            OBJECT_NULL_EXCEPTION       
        }

        //Constructor to initialize the enum exception types
        public FundooNoteException(ExceptionTypes type, string message) : base(message)
        {
            this.type = type;
        }
    }
}
