
namespace Treenirepository.DataModels
{

  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Section", Schema = "dbo")]
  public class Section
  {
    public Section()
    {
    }
    public Section(Models.Section sectionDTO)
    {
      Id = sectionDTO.Id;
      Name = sectionDTO.Name;
      Duration = sectionDTO.Duration;
      SetupDuration = sectionDTO.SetupDuration;
      Color = sectionDTO.Color;
      ExerciseId = sectionDTO.ExerciseId;
      Description = sectionDTO.Description;

    }
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int Duration { get; set; }
    [Required]
    public int SetupDuration { get; set; }
    [Required]
    public int Color { get; set; }
    [Required]
    public int ExerciseId { get; set; }
    [ForeignKey(nameof(ExerciseId))]
    public Exercise Exercise { get; set; }
  }
}