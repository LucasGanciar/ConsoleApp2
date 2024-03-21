using System.Threading.Tasks.Dataflow;

namespace GuessNumberGame
{
    class Program
    {
        public static char[] letras = "aáàâãäbcçdeéèêëfghiíìîïjklmnñoóòôõöpqrstuúùûüvwxyzAÁÀÂÃÄBCÇDEÉÈÊËFGHIÍÌÎÏJKLMNÑOÓÒÔÕÖPQRSTUVWXYZ".ToCharArray();

        static async Task Main(string[] args)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount // Adjust the degree of parallelism as needed
            };

            var actionBlock = new ActionBlock<char>(async letra1 =>
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(letras, letra2 =>
                    {
                        for (int numero = 1; numero <= 100; numero++)
                        {
                            string chave = $"{letra1}{numero}{letra2}";
                            Console.WriteLine(chave);
                            _ = EnviarChave(chave);
                        }
                    });
                });
            }, options);

            foreach (var letra1 in letras)
            {
                await actionBlock.SendAsync(letra1);
            }

            actionBlock.Complete();
            await actionBlock.Completion;
        }

        private static async Task EnviarChave(string chave)
        {
            string endpointUrl = "https://fiap-inaugural.azurewebsites.net/fiap";
            string grupo = "grupinho_da_galera_de_portugal";

            var jsonBody = new
            {
                Key = chave,
                grupo = grupo
            };

            string jsonBodyString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonBody);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(endpointUrl, new StringContent(jsonBodyString, System.Text.Encoding.UTF8, "application/json"));
                    Console.WriteLine(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao enviar chave: " + ex.Message);
                }
            }
        }
    }
}