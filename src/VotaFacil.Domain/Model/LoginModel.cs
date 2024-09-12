using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotaFacil.Domain.Entidades
{
    [Table("login")]
    public class LoginModel
    {
        [Key, Column("id")] public int Id { get; set; }

        [Required, Column("username"), MaxLength(50)] public string Username { get; set; }

        [Required, Column("password"), MaxLength(100)] public string Password { get; set; }

        [Column("token")] public string Token { get; set; }

        [Required, Column("criado_em")] public DateTime CriadoEm { get; set; }

        [Column("ultimo_login")] public DateTime? UltimoLogin { get; set; }

        [Required, Column("status")] public bool Status { get; set; }

        [Column("expiracao_token")] public DateTime? ExpiracaoToken { get; set; }

        [Column("eleitor_id"), ForeignKey("Eleitor")]
        public Guid EleitorId { get; set; }
        public EleitorModel Eleitor { get; set; }


        public LoginModel()
        {
            CriadoEm = DateTime.UtcNow;
            Status = true;
        }

        public async Task<bool> Login()
        {
            if (!Status)
             return false;

            UltimoLogin = DateTime.UtcNow;
            ExpiracaoToken = DateTime.UtcNow.AddHours(1);
            return true;
        }

        public async Task Logout()
        {
            UltimoLogin = DateTime.UtcNow;
        }
    }
}
