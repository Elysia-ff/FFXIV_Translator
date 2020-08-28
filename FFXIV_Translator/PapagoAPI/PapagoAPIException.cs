using System;

namespace FFXIV_Translator.PapagoAPIs
{
    public class PapagoAPIException : Exception
    {
        public string Message { get; private set; }

        public PapagoAPIException(string msg)
        {
            Message = msg;
        }
    }
}
