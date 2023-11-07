namespace Sample_BF
{
    public class Claim
    {
        public Claim(string idInvoice, int amount, string concept, string clientComent)
        {
            Id = Guid.NewGuid().ToString();
            State = "Abierta";

            IdInvoice = idInvoice;
            Amount = amount;
            Concept = concept;
            ClientComment = clientComent;
            OpenAIComment = "";
        }

        public void SetAsClose() => State = "Cerrada";

        public string Id { get; set; }
        public string State { get; set; }
        public string IdInvoice { get; set; }
        public int Amount { get; set; }
        public string Concept { get; set; }
        public string ClientComment { get; set; }
        public string OpenAIComment { get; set; }
    }
}
