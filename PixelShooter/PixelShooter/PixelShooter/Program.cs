using System;

namespace PixelShooter
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (GamePixelShooter game = new GamePixelShooter())
            {
                game.Run();
            }
        }
    }
#endif
}
