using System.Drawing;

namespace FFXIV_Translator
{
    public struct Chat
    {
        public string Str { get; private set; }
        public Size Size { get; set; }
        public FFChatCode Code { get; private set; }

        public int Height { get { return Size.Height; } }

        public Chat(string str, Size size, FFChatCode code)
        {
            Str = str;
            Size = size;
            Code = code;
        }

        public void SetSize(Size size)
        {
            Size = size;
        }
    }
}
