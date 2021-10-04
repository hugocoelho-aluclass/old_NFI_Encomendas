using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public Guid ApplicationId { get; set; }

        [Key]
        public Guid RoleId { get; set; }

        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual Applications Applications { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}