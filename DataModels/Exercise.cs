
namespace treenirepository.DataModels
{

  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  
  [Table("Exercise", Schema = "dbo")]
  public class Exercise
  {
    public Exercise()
    {
    }
    public Exercise(Models.Exercise exerciseDTO)
    {
      Id = exerciseDTO.Id;
      Name = exerciseDTO.Name;
      Created = exerciseDTO.Created != null ? exerciseDTO.Created : DateTime.Now;
      ContentData = exerciseDTO.ContentData;

    }
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public string ContentData { get; set; }
  }
}