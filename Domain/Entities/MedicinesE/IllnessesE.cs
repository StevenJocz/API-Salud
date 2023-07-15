﻿namespace UNAC.AppSalud.Domain.Entities.MedicinesE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_illnesses")]
    public class IllnessesE
    {
        [Key]
        public int id { get; set; }
        public string s_illness_name { get; set; }
    }
}
