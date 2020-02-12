using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public static class EnumerableAsyncExtensions
    {
        public static async Task<bool> All(this IEnumerable<Task<bool>> tasks)
        {
            foreach (var task in tasks)
            {
                if (!await task)
                {
                    return false;
                }
            }
            return true;
        }

        public static async Task<bool> Any(this IEnumerable<Task<bool>> tasks)
        {
            foreach (var task in tasks)
            {
                if (await task)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
