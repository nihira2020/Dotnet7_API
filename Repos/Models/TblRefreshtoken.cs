using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Repos.Models;

[Table("tbl_refreshtoken")]
public partial class TblRefreshtoken
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? TokenId { get; set; }

    public string? RefreshToken { get; set; }

    public bool? IsActive { get; set; }
}
