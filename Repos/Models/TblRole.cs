using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Repos.Models;

[Table("tbl_role")]
public partial class TblRole
{
    [Key]
    [Column("roleid")]
    [StringLength(50)]
    [Unicode(false)]
    public string Roleid { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }
}
