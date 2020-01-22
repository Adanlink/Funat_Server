using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Server.Database.Models.Enums;
using Server.Database.Models.Interfaces;

namespace Server.Database.Models
{
    public class AccountModel : IMappedModel
    {
        public long Id { get; set; }

        #region Credentials

        public string Username { get; set; }

        public string Email { get; set; }

        public string EncodedHash { get; set; }
        #endregion

        public DateTime TimeOfCreation { get; set; }

        #region SessionId
        public Guid SessionId { get; set; } = Guid.NewGuid();

        public DateTime SessionIdExpiryDate { get; set; }
        #endregion

        #region Preferences
        public LanguageType PreferedLanguage { get; set; }
        #endregion

        public virtual ICollection<CharacterModel> Characters { get; set; }
    }
}
