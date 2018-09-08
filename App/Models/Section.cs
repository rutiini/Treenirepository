
namespace Treenirepository.Models
{

  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Runtime.Serialization;

  [DataContract(Name = "section")]
  public class Section : IValidatableObject
  {
    public Section()
    {
    }
    public Section(DataModels.Section sectionData)
    {
      Id = sectionData.Id;
      Name = sectionData.Name;
      Duration = sectionData.Duration;
      SetupDuration = sectionData.SetupDuration;
      Color = sectionData.Color;
      ExerciseId = sectionData.ExerciseId;
      Description = sectionData.Description;

    }
    [DataMember(Name = "id")]
    public int Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "duration")]
    public int Duration { get; set; }
    [DataMember(Name = "setupDuration")]
    public int SetupDuration { get; set; }
    [DataMember(Name = "color")]
    public int Color { get; set; }
    [DataMember(Name = "exerciseId")]
    public int ExerciseId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var input = validationContext.ObjectInstance as Section;
      if (input.Name.Length == 0)
      {
        yield return new ValidationResult("Name is invalid", new[] { "Name" });
      }
      else if (input.Duration <= 0)
      {
        yield return new ValidationResult("Duration is invalid", new[] { "Duration" });
      }
      else if (input.ExerciseId <= 0)
      {
        yield return new ValidationResult("ExerciseId is invalid", new[] { "ExerciseId" });
      }
    }
    // public Exercise? Exercise { get; set; }
  }
}