namespace ProcessingMicroservice.Entities
{
    public class Curso : Base
    {
        public int CursoId { get; set; }    
        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
