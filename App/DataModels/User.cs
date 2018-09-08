
namespace Treenirepository.DataModels
{

  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  
  [Table("User", Schema = "dbo")]
  public class User
  {
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Password { get; set; }
  }
}