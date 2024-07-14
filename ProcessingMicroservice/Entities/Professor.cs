namespace ProcessingMicroservice.Entities
{
    public class Professor : Base
    {
        public int ProfessorId { get; set; }    
        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Telefone { get; set; }

        public string? Endereco { get; set; }

        public int UsuarioId { get; set; }

        public DateTime? DataContratacao { get; set; }
    }
}
