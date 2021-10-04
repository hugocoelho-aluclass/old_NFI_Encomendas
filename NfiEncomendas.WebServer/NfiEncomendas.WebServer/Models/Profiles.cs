using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NfiEncomendas.WebServer.Models
{
    public partial class Profiles
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(4000)]
        public string PropertyNames { get; set; }

        [Required]
        [StringLength(4000)]
        public string PropertyValueStrings { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] PropertyValueBinary { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public virtual Users Users { get; set; }
    }
}
