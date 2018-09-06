
namespace treenirepository.DataModels
{

  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

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
      // we might want to leave this undone and handle updates on single sections.
      // Sections = exerciseDTO.Sections.Select(s => new Section(s)).ToList();

    }
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public string ContentData { get; set; }
    [InverseProperty("Exercise")]
    public ICollection<Section> Sections {get;set;}
  }
}