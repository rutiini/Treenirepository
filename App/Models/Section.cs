
namespace Treenirepository.Models
{

  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Runtime.Serialization;

  [DataContract(Name = "section")]
  public class Section
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
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Length of {0} must be between {2} and {1} characters.")]
    public string Name { get; set; }

    [DataMember(Name = "description")]
    public string Description { get; set; }

    [DataMember(Name = "duration")]
    [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Duration { get; set; }

    [DataMember(Name = "setupDuration")]
    [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int SetupDuration { get; set; }

    [DataMember(Name = "color")]
    [Range(0, 50, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Color { get; set; }

    // TODO: weigh whether we should even expose this property to users, we only use sections under an exercise for now..
    [DataMember(Name = "exerciseId")]
    [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ExerciseId { get; set; }
  }
}