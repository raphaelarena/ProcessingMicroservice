namespace ProcessingMicroservice.Entities
{
    public class Matricula : Base
    {
        public int MatriculaId { get; set; }    
        public int AlunoId { get; set; }

        public int TurmaId { get; set; }

        public DateTime? DataMatricula { get; set; }
    }
}
