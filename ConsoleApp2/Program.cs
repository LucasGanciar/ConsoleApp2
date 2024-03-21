namespace GuessNumberGame
{
    class Program
    {
        public static char[] letras = "aáàâãäbcçdeéèêëfghiíìîïjklmnñoóòôõöpqrstuúùûüvwxyzAÁÀÂÃÄBCÇDEÉÈÊËFGHIÍÌÎÏJKLMNÑOÓÒÔÕÖPQRSTUVWXYZ".ToCharArray();
        static async Task Main(string[] args)
        {
            foreach (var letra1 in letras)
            {
                foreach (var letra2 in letras)
                {
                    for (int numero = 1; numero <= 100; numero++)
                    {
                        string chave = $"{letra1}{numero}{letra2}";
                        Console.WriteLine(chave);
                        await EnviarChave(chave);
                    }
                }
            }
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