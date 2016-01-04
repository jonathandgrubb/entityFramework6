using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using NinjaDomain.Classes.Interfaces;

//using NinjaDomain.Classes.Enums;

namespace NinjaDomain.Classes
{
    public class Ninja : IModificationHistory
    {
        public Ninja()
        {
            EquipmentOwned = new List<NinjaEquipment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ServiceInOniwaban { get; set; }
        public Clan Clan { get; set; }
        public int ClanId { get; set; }
        public List<NinjaEquipment> EquipmentOwned { get; set; } 
        public System.DateTime DateOfBirth { get; set; }

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }
    }

    public class Clan : IModificationHistory
    {
        public Clan()
        {
            Ninjas = new List<Ninja>();
        }
        public int Id { get; set; }
        public string ClanName { get; set; }
        public List<Ninja> Ninjas { get; set; }

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }
    }

    public class NinjaEquipment : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentType Type { get; set; }
        [Required]
        public Ninja Ninja { get; set; }

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }
    }
}
