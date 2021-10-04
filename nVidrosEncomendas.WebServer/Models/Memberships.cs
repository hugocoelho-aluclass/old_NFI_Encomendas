namespace NVidrosEncomendas.WebServer.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public partial class Memberships
    {
        public Guid ApplicationId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        public int PasswordFormat { get; set; }

        [Required]
        [StringLength(128)]
        public string PasswordSalt { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string PasswordQuestion { get; set; }

        [StringLength(128)]
        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime LastLockoutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public DateTime FailedPasswordAttemptWindowStart { get; set; }

        public int FailedPasswordAnswerAttemptCount { get; set; }

        public DateTime FailedPasswordAnswerAttemptWindowsStart { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public virtual Applications Applications { get; set; }

        public virtual Users Users { get; set; }
    }
}
