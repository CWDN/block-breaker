using System;

namespace BlockBreaker
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockBreaker game = new BlockBreaker())
            {
                game.Run();
            }
        }
    }
#endif
}

