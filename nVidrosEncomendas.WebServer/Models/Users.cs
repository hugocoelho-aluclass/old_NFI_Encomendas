using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public partial class Users
    {
        public Users()
        {
            //Utilizadores_UniqueIds = new HashSet<Utilizadores_UniqueIds>();
            Roles = new HashSet<Roles>();
        }

        public Guid ApplicationId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public bool IsAnonymous { get; set; }

        public DateTime LastActivityDate { get; set; }

        public virtual Applications Applications { get; set; }

        public virtual Memberships Memberships { get; set; }

        public virtual Profiles Profiles { get; set; }

        //public virtual ICollection<Utilizadores_UniqueIds> Utilizadores_UniqueIds { get; set; }

        public virtual ICollection<Roles> Roles { get; set; }
    }
}
