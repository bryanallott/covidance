using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace covidance.Data
{
    [Table("Person")]
    public class PersonInfo
    {
        public Guid Id { get; set; }
        [MaxLength(128)] 
        public string Name { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        public bool Deleted { get; set; }
        [MaxLength(450)] 
        public string UserId { get; set; }
        public virtual ICollection<RecordInfo> Records { get; set; }
    }
    //fever, persistent cough, headache, flu-like symptoms, fatigue
    public enum CovidSymptomsEnum
    {
        None = 0,
        Fever = 1, 
        PersistentCough = 2,
        Headache = 4,
        Fatigue = 8,
        FluLikeSymptoms = 16,
    }

    [Table("Record")]
    public class RecordInfo
    {
        public Guid Id { get; set; }
        
        [ForeignKey("PersonInfo"), Column("PersonId")]
        public Guid PersonInfoId { get; set; }
        public virtual PersonInfo PersonInfo { get; set; }

        public DateTime When { get; set; }
        public DateTime DateCreated { get; set; }
        public double Temperature { get; set; }
        //fever, persistent cough, headache, flu-like symptoms, fatigue
        public CovidSymptomsEnum Symptoms { get; set; }
        public bool RecentContact { get; set; }
        public bool Sanitised { get; set; }
        public bool Bagged { get; set; }
        [MaxLength(128)]
        public string Reason { get; set; }
        [MaxLength(512)]
        public string Photo { get; set; }
    }
}
