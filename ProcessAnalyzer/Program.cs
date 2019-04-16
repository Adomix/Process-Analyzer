using System.Threading.Tasks;

namespace ProcessAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var analyzer = new Analyzer();
            await analyzer.BeginLogic();
        }
    }
}
