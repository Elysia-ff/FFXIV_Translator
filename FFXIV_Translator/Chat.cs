using System.Drawing;

namespace FFXIV_Translator
{
    public struct Chat
    {
        public string Str { get; private set; }
        public Size Size { get; set; }

        public int Height { get { return Size.Height; } }

        public Chat(string str, Size size)
        {
            Str = str;
            Size = size;
        }
    }
}
