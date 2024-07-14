namespace ProcessingMicroservice.Entities
{
    public class Turma : Base
    {
        public int TurmaId { get; set; }

        public int CursoId { get; set; }

        public string? Nome { get; set; }

        public TimeSpan? Horario { get; set; }

        public string? Local { get; set; }
    }
}
