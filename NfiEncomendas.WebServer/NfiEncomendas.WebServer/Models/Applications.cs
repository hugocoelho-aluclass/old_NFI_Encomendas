
namespace NfiEncomendas.WebServer.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Applications
    {
        public Applications()
        {
            Memberships = new HashSet<Memberships>();
            Roles = new HashSet<Roles>();
            Users = new HashSet<Users>();
        }

        [Required]
        [StringLength(235)]
        public string ApplicationName { get; set; }

        [Key]
        public Guid ApplicationId { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual ICollection<Memberships> Memberships { get; set; }

        public virtual ICollection<Roles> Roles { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}