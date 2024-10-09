using System.Xml.Linq;


class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(" ======== Bem-vindo aos Correios ========");

        using (HttpClient client = new HttpClient())
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine(" ======== Bem-vindo aos Correios ========");
                Console.Write("Digite o CEP (ou 'sair' para encerrar): ");
                string cep = Console.ReadLine();

                if (cep?.ToLower() == "sair")
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !long.TryParse(cep, out _))
                {
                    Console.WriteLine("CEP inválido. Certifique-se de digitar um CEP com 8 dígitos numéricos.");
                }
                else
                {
                    string url = "https://viacep.com.br/ws/" + cep + "/xml/";

                    try
                    {
                        var response = await client.GetStringAsync(url);
                        XDocument xmlDoc = XDocument.Parse(response);

                        string rua = xmlDoc.Root.Element("logradouro")?.Value;
                        string bairro = xmlDoc.Root.Element("bairro")?.Value;
                        string cidade = xmlDoc.Root.Element("localidade")?.Value;

                        if (rua != null && bairro != null && cidade != null)
                        {
                            Console.Clear();
                            Console.WriteLine(" ======== Informações do CEP ========");
                            Console.WriteLine($"CEP: {cep}");
                            Console.WriteLine($"Rua: {rua}");
                            Console.WriteLine($"Bairro: {bairro}");
                            Console.WriteLine($"Cidade: {cidade}");

                            XDocument simpleXml = new XDocument(
                                new XElement("Endereco",
                                    new XElement("CEP", cep),
                                    new XElement("Rua", rua),
                                    new XElement("Bairro", bairro),
                                    new XElement("Cidade", cidade)
                                )
                            );

                            string filePath = $"cep_info_{cep}_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                            simpleXml.Save(filePath);

                            Console.WriteLine($"Informações do CEP salvas em formato XML no arquivo: {filePath}");
                        }
                        else
                        {
                            Console.WriteLine("CEP não encontrado.");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Erro ao consultar o CEP: " + e.Message);
                    }
                }

                Console.WriteLine("\nPressione Enter para buscar outro CEP...");
                Console.ReadLine();
            }
        }

        Console.WriteLine("Obrigado por utilizar o sistema. Até a próxima!");
    }
}







