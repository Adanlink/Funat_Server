using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Server.Database.Models.Enums;
using Server.Database.Models.Interfaces;
using Server.SharedThings.Enums;

namespace Server.Database.Models
{
    public class CharacterModel : IMappedModel
    {
        public long Id { get; set; }

        public virtual AccountModel OwnerAccount { get; set; }

        public AuthorityType Authority { get; set; }

        public DateTime TimeOfCreation { get; set; }
        
        public DateTime LastTimePlayed { get; set; }

        public string Nickname { get; set; }
        
        public long MapId { get; set; }
        
        public float MapX { get; set; }

        public float MapY { get; set; }
    }
}
