using System;

namespace TheBlindMan
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TheBlindManGame game = new TheBlindManGame())
            {
                game.Run();
            }
        }
    }
#endif
}

