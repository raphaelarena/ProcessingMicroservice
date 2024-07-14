namespace ProcessingMicroservice.Entities
{
    public class Aluno : Base
    {
        public int AlunoId { get; set; }    
        public string? Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string? Email { get; set; }

        public string? Telefone { get; set; }

        public string? Endereco { get; set; }

        public int UsuarioId { get; set; }
    }
}
