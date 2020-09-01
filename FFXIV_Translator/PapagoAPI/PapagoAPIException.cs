using System;

namespace FFXIV_Translator.PapagoAPIs
{
    public class PapagoAPIException : Exception
    {
        public new string Message { get; private set; }

        public PapagoAPIException(string msg)
        {
            Message = msg;
        }
    }
}
