using System;
using System.Threading.Tasks;

namespace DemoQATests.Helpers
{
    public static class RetryHelper
    {
        public static async Task<bool> RetryUntilAsync(Func<bool> condition, int maxAttempts = 5, int delayMs = 1000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                if (condition()) return true;
                await Task.Delay(delayMs);
            }
            return false;
        }
    }
}