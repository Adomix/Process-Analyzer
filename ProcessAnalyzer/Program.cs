using System.Threading.Tasks;

namespace ProcessAnalyzer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var analyzer = new Analyzer();
            await analyzer.BeginLogic();
        }
    }
}