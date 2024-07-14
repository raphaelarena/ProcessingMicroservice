namespace ProcessingMicroservice.Entities
{
    public class Usuario : Base
    {
        public int UsuarioId { get; set; }
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Senha { get; set; }

        public int TipoUsuario { get; set; }

        public int Idreferente { get; set; }
    }
}
